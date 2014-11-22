using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Threading;

using Microsoft.Win32;

using NLog;

namespace BPExporter
{

    public partial class ExporterForm : Form
    {
        private List<DB> HitsDBs;
        private DB ExporterDB;
        private Project CurrentProject;

        private List<String> CheckedFiles = new List<String>();
        private List<String> CheckedISP = new List<String>();
        private List<String> CheckedRegions = new List<String>();
        private List<String> CheckedCountries = new List<String>();
        
        private Dictionary<long, List<File>> ProjectsCache = new Dictionary<long, List<File>>();

        private BackgroundWorker MoveWorker = new BackgroundWorker();
        private BackgroundWorker NewDBWorker = new BackgroundWorker();

        private AutoResetEvent DBProcessed = new AutoResetEvent(true);
        protected static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public ExporterForm()
        {
            InitializeComponent();

            MoveWorker.DoWork += new DoWorkEventHandler(MoveWorker_DoWork);
            MoveWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(MoveWorker_RunWorkerCompleted);
            MoveWorker.ProgressChanged += new ProgressChangedEventHandler(MoveWorker_ProgressChanged);
            MoveWorker.WorkerReportsProgress = true;

            NewDBWorker.DoWork += new DoWorkEventHandler(NewDBWorker_DoWork);
            NewDBWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(NewDBWorker_RunWorkerCompleted);
            NewDBWorker.ProgressChanged += new ProgressChangedEventHandler(NewDBWorker_ProgressChanged);
            NewDBWorker.WorkerReportsProgress = true;

            c_DE.Checked = true;
        }

        public void InitProjectsTree()
        {
            projectsTree.UseCellFormatEvents = true;

            projectsTree.CanExpandGetter = delegate(object x)
            {
                if (x is Project)
                {
                    Project proj = (Project)x;
                    return ExporterDB.GetAllProjectFiles(proj).Count > 0;
                }
                return false;
            };

            projectsTree.ChildrenGetter = delegate(object x)
            {
                Project project = (Project)x;
                return ExporterDB.GetAllProjectFiles(project).Select( (e) => new { Name = e.Name, AKZ = e.Hash } );
            };

            columnFilename.AspectGetter = delegate(object x) 
            {
                return (String)((Object[])x)[0]; 
            };
  
            columnCount.AspectGetter = delegate(object x) 
            {
                return (int)((Object[])x)[4]; 
            };

            columnHashFiles.AspectGetter = delegate(object x)
            {
                return (String)((Object[])x)[1];
            };

            columnSize.AspectGetter = delegate(object x)
            {
                String s = (String)((Object[])x)[3];
                return long.Parse(s);
            };

            columnSize.AspectToStringConverter = delegate(object x)
            {
                long l = (long)x;
                return l.FormatedSize();
            };

            hitsView.UseFiltering = true;
            filesView.UseFiltering = true;
            hitsView.ShowFilterMenuOnRightClick = false;

            hitsView.ModelFilter = new BrightIdeasSoftware.ModelFilter
            (
                (object x) =>
                {
                    Hit hit = (Hit)x;
                    bool show = true;

                    if( CheckedCountries.Count > 0)
                       show = CheckedCountries.IndexOf(hit.Country) >= 0;

                    if (show && CheckedCountries.Count > 0)
                    {
                        if (CheckedRegions.Count > 0)
                            show = CheckedRegions.IndexOf(hit.Region) >= 0;
                    }

                    if (show && CheckedISP.Count > 0)
                        show = CheckedISP.IndexOf(hit.Isp) >= 0;

                    if( show && useDate.Checked )
                       show = (startDatePicker.Value <= hit.Date && hit.Date <= endDatePicker.Value);

                    return show;
                }
                 
            );

            projectsTree.SetObjects(ExporterDB.GetAllProjects());

            filesView.BooleanCheckStatePutter = delegate(Object rowObject, bool check)
            {
                if (check)
                    CheckedFiles.Add((String)((Object[])rowObject)[1]);
                else
                    CheckedFiles.Remove((String)((Object[])rowObject)[1]);

                MoveButton.Enabled = CheckedFiles.Count != 0;

                return check;
            };

            filesView.BooleanCheckStateGetter = delegate(Object rowObject)
            {
                return CheckedFiles.Contains((String)((Object[])rowObject)[1]);
            };
        }

        private void SetCurrentProject(Project project)
        {
            CurrentProject = project;
            prjName.Text = String.Format("{0}({1})", project.Name, project.Product);
            CurrentProject.TimeZone = TimeZoneInfo.FindSystemTimeZoneById((String)TimeZone.SelectedItem);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                NewProjectDlg dlg = new NewProjectDlg();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Project NewProject = dlg.GetProject();
                    ExporterDB.CreateNewProject(NewProject);
                    SetCurrentProject(NewProject);
                }

                InitProjectsTree();
                UpdateUI(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Error during project creation. Try to change name. {0}", ex.Message) , "Error", 
                    MessageBoxButtons.OK ,MessageBoxIcon.Error);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DisableLoad();

                String DbFilename = dlg.FileName;

                if (DBUtils.IsExporterDB(DbFilename))
                {
                    DBUtils.UpdateIndexes(ExporterDB, DbFilename);
                    InitProjectsTree();
                    UpdateUI(true);
                }
                else
                {

                    DBUtils.Fix(new String[] { DbFilename }, ShowFixProgress(1));

                    HitsDBs = new List<DB>();
                    HitsDBs.Add(new DB(DbFilename));

                    ProcessNewDB(HitsDBs, false, logger);

                    activeDB.Text = DbFilename;
                }
            }
        }

        private bool HashesEqual(String lhs, String rhs)
        {
            String lhsHash = lhs;
            String rhsHash = rhs;

            return lhsHash.ToUpper() == rhsHash.ToUpper();
        }

        private List<Object[]> ExcludeProjectFiles(Project project, List<Object[]> files)
        {
            List<Object[]> result = new List<Object[]>();
            List<File> projectFiles = ExporterDB.GetAllProjectFiles(project);

            bool add = true;
            foreach (Object[] file in files)
            {
                add = true;
                for (int i = 0; i < projectFiles.Count; i++)
                {
                    if (((String)file[0]).ToUpper() == projectFiles[i].Name.ToUpper() && 
                        HashesEqual((String)file[1], projectFiles[i].Hash))
                    {
                        add = false;
                        break;
                    }
                }

                if (add)
                    result.Add(file);
            }

            return result;
        }

        private bool ProcessNewDB(IEnumerable<DB> dbs, bool fixSchema, Logger log)
        {
            filesView.Enabled = false;
            projectsTree.Enabled = false;

            if (CurrentProject != null)
            {
                return CacheHitsByHashes(dbs, "hits", log, NewDBWorkerProgressUpdate);
            }
            else
            {
                MessageBox.Show("Select project first!", "BPExporter Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        private bool CacheHitsByHashes(IEnumerable<DB> dbs, String From, Logger log, Action<long, long> action)
        {
            if (!NewDBWorker.IsBusy){
                NewDBWorker.RunWorkerAsync(new { Dbs = dbs, From = From, Log=log});
                return true;
            }
            return false;
        }

        void NewDBWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress.Value = Math.Abs(e.ProgressPercentage);
        }

        void NewDBWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableLoad();

            if(e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IEnumerable<DB> dbs = (IEnumerable<DB>)e.Result;

            List<Object[]>  files = new List<object[]>();
            List<File> projectFiles = new List<File>();

            foreach( DB db in dbs)
            {
                var Empty = ArrayUtils.Empty<String>();
                files.AddRange(ExcludeProjectFiles(CurrentProject, db.GetAllFilesNamesFromHits(db.ListCache.AsEnumerable())));
                projectFiles.AddRange(ExporterDB.GetAllProjectFiles(CurrentProject));
            }

            if (projectFiles.Count > 0)
            {
                MoveHits(projectFiles.ConvertAll<String>((file) => file.Hash),
                         GetSelectedCountries(),
                         dbs);
            }
       
            statusText.Text = "Loaded";
            Progress.Value = Progress.Minimum;
            Cursor = Cursors.WaitCursor;

            filesView.SetObjects(files);
            UpdateUI(false);
        }

        void NewDBWorker_DoWork(object sender, DoWorkEventArgs e)
        {
                String From = TypeUtils.GetValueFromAnonymousType<String>(e.Argument, "From");
                IEnumerable<DB> dbs = TypeUtils.GetValueFromAnonymousType<IEnumerable<DB>>(e.Argument, "Dbs");
                Logger log = TypeUtils.GetValueFromAnonymousType<Logger>(e.Argument, "Log");

                int counter = 0;
                int count = dbs.Count();

                foreach (DB db in dbs)
                {
                    ++counter;

                    this.UIThread(() =>
                    {
                        statusText.Text = String.Format("Loading {0} of {1} ({2})",
                            counter, count, Path.GetFileName(db.Path));

                        Progress.Value = 0;
                        statusText.Refresh();
                    });

                    HashSet<String> countries = new HashSet<string>(GetSelectedCountries());
                    
                    try
                    {
                        db.CacheHitsByHashes(From, NewDBWorkerProgressUpdate, countries);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        log.Error(ex);
                    }
                }

                e.Result = dbs;
                return; 
        }

        void NewDBWorkerProgressUpdate(long Max, long Count)
        {
            long done = 0;
            try
            {
                done = (100 * Count) / Max;
                NewDBWorker.ReportProgress((int)done);
            }
            catch (Exception)
            {
                //MessageBox.Show(String.Format("Max: {1}, Count: {2}, Done: {3}", Count, Max, done));
            }
        }

        private void UpdateUI(bool full)
        {
            projectsTree.Enabled = true;
            filesView.Enabled = true;

            if (CurrentProject != null && full)
            {
                List<Hit> hits = ExporterDB.GetNotExported(CurrentProject);

                ReloadCountryCombo(hits);
                ReloadRegionCombo(hits);
                ReloadISPcombo(hits);

                SetDates(hits);

                UpdateHitsView(hits);
            }

            filesView.BuildList();
            checkAll.Enabled = filesView.Items.Count != 0;
            MoveButton.Enabled = filesView.SelectedIndices.Count != 0;

            Cursor = Cursors.Default;
            Progress.Value = Progress.Minimum;

            Refresh();
        }

        private void SetDates(List<Hit> hits)
        {
            foreach (Hit hit in hits){
                hit.Date = TimeZoneInfo.ConvertTimeFromUtc(
                    hit.UTCDate, CurrentProject.TimeZone
                );
            }

            var byDate = hits
                .Where((hit) => hit.Date.Year > 2000)
                .OrderBy((hit) => hit.Date);
            
            var firstDate = byDate.FirstOrDefault(); 
            var lastDate = byDate.Reverse().FirstOrDefault();
            
            if( firstDate != null )
                startDatePicker.Value = firstDate.Date;

            if (lastDate != null)
                endDatePicker.Value = lastDate.Date;
        }

        private void ExporterForm_Load(object sender, EventArgs e)
        {
            string gitVersion = String.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BPExporter." + "gitrevision.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                revLabel.Text = reader.ReadToEnd();
            }

            ExporterDB = new DB(Settings.DatabasePath);
            FillProjectsCache(ExporterDB);
            InitProjectsTree();
            UpdateUI(true);

            if (Settings.CitySrv == null || Settings.IspSrv == null)
            {
                MessageBox.Show("No GeoIP database. Put GeoIpISP.dat and GeoIpCity.dat into BPExporter directory.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Close();
            }

            try
            {
                System.Net.WebClient client = new System.Net.WebClient();
                int response = 1;
                int.TryParse(client.DownloadString("http://baseprotect-limiter.appspot.com/exp"), out response);
                if (response == 0)
                    Close();
            }
            catch{}
        }

        private void FillProjectsCache(DB db)
        {
            ProjectsCache.Clear();
            List<Project> projects = db.GetAllProjects();

            foreach( Project project in projects )
            {
                List<File> files = db.GetAllProjectFiles(project);
                ProjectsCache[project.ID] = files;
            }
        }

        private void MoveButton_Click(object sender, EventArgs e)
        {
            var selected = filesView.CheckedObjects.Cast<Object[]>().ToList();

            ExporterDB.AddFilesToProject(CurrentProject, selected);
            MoveHits(selected.ConvertAll((obj) => (String)obj[1]), GetSelectedCountries(), HitsDBs);

            filesView.RemoveObjects(selected);
        }
        
        private IEnumerable<String> GetSelectedCountries()
        {
            var checks = new CheckBox[]{c_PL, c_AU, c_US, c_RU, c_JP, c_DE};
            List<String> countries = c_edit.Text.Split(new String[1]{";"}, StringSplitOptions.RemoveEmptyEntries).ToList();
           
            foreach (var c in checks){
                if(c.Checked) countries.Add(c.Name.Remove(0, 2));
            }

            return countries;
        }

        private void MoveHits(List<String> hashes, IEnumerable<String> countries, IEnumerable<DB> dbs)
        {
            if (!MoveWorker.IsBusy)
            {
                Cursor = Cursors.WaitCursor;

                projectsTree.Enabled = false;
                checkAll.Checked = false;
                filesView.Enabled = false;
                statusText.Text = "Processing Hits";

                MoveWorker.RunWorkerAsync(new { Hashes=hashes, Countries=countries, Dbs=dbs });
            }
        }

        void MoveWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress.Value = Math.Abs(e.ProgressPercentage);   
        }

        void MoveWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {           
            statusText.Text = String.Format("{0} Hits processed", (int)e.Result);           
            UpdateUI(true);

            EnableLoad();
        }

        void MoveWorkerProgressUpdate(long Max, long Count)
        {
            long done = 0;
            try
            {
                done = (100 * Count) / Max;
                MoveWorker.ReportProgress((int)done);
            }
            catch (Exception)
            {
                //MessageBox.Show(String.Format("Max: {1}, Count: {2}, Done: {3}", Count, Max, done));
            }
        }

        void MoveWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            IEnumerable<String> hashes = TypeUtils.GetValueFromAnonymousType<IEnumerable<String>>(e.Argument, "Hashes")
                                                    .SkipWhile( (hash) => String.IsNullOrEmpty(hash));

            List<String> countries = TypeUtils.GetValueFromAnonymousType<List<String>>(e.Argument, "Countries");
            IEnumerable<DB> dbs = TypeUtils.GetValueFromAnonymousType<IEnumerable<DB>>(e.Argument, "Dbs");

            List<Hit> result = new List<Hit>();
            int AllHitsCount = 0;

            Action move = new Action(() =>
            {
                foreach (DB db in dbs)
                {
                    foreach (String hash in hashes)
                    {
                        List<Hit> hits = db.GetAllFileHits("hits", hash);
                        AllHitsCount += hits.Count;
                        result.AddRange(hits);
                    }
                }

                ExporterDB.InsertHits(CurrentProject, result, CurrentProject.FilesNo, countries, MoveWorkerProgressUpdate);

                this.UIThread(() =>{ Progress.Value = 0; });
            });

            ExporterDB.RunInTransaction(move);

            e.Result = AllHitsCount;
            return;
        }

        private void ReloadCountryCombo(IEnumerable<Hit> hits)
        {
            var byCountry = hits.GroupBy((hit) => hit.Country).Select((group) => new { Name = group.Key }).ToArray();
            countryCombo.Items.Clear();
            countryCombo.Items.AddRange( byCountry.Where((country) => country.Name.Length > 0)
                                                  .OrderBy( (country) => country.Name).ToArray() );

            countryCombo.Text = String.Empty;
        }

        private void ReloadISPcombo(IEnumerable<Hit> hits)
        {
            var byISP = hits.GroupBy((hit) => hit.Isp).Select((group) => new { Name = group.Key }).ToArray();
            ispCombo.Items.Clear();
            ispCombo.Items.AddRange( byISP.Where((isp) => isp.Name.Length > 0)
                                          .OrderBy((isp) => isp.Name).ToArray() );

            ispCombo.Text = String.Empty;
        }

        private void ReloadRegionCombo(IEnumerable<Hit> hits)
        {
            var byRegion = hits.GroupBy((hit) => hit.Region).Select((group) => new { Name = group.Key }).ToArray();
            regionCombo.Items.Clear();
            regionCombo.Items.AddRange( byRegion.Where((region) => region.Name.Length > 0)
                                            .OrderBy((region) => region.Name).ToArray() );

            regionCombo.Text = String.Empty;
        }

        private void countryCombo_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            bool Check = e.NewValue == CheckState.Checked;
            
            String Country = TypeUtils.GetValueFromAnonymousType<String>(countryCombo.Items[e.Index], "Name");
            
            if (Check)
                CheckedCountries.Add(Country);
            else
                CheckedCountries.Remove(Country);
        }

        private void regionCombo_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            bool Check = e.NewValue == CheckState.Checked;
            String Region = TypeUtils.GetValueFromAnonymousType<String>(regionCombo.Items[e.Index], "Name");

            if (Check)
                CheckedRegions.Add(Region);
            else
                CheckedRegions.Remove(Region);
        }

        private void UpdateHitsView(List<Hit> hits)
        {
            if (hits != null)
                hitsView.SetObjects(hits);
            else
                hitsView.ModelFilter = hitsView.ModelFilter;

            ToExportCount.Text = hitsView.FilteredObjects.Cast<Hit>().Count().ToString();
        }

        private void useDate_CheckedChanged(object sender, EventArgs e)
        {
            startDatePicker.Enabled = useDate.Checked;
            endDatePicker.Enabled = useDate.Checked;
            
            UpdateHitsView(null);
        }

        private String GetTimeZone(Hit hit)
        {
            bool SummerTime = hit.Date.IsDaylightSavingTime() ||
                CurrentProject.TimeZone.IsDaylightSavingTime(hit.Date);
            if (hit.Country == "DE")
                return SummerTime ? "MESZ" : "MEZ";
            else
                return SummerTime ? "CEST" : "CET";
        }

        private CSVSchema LoadSchema(String ISPName)
        {
            String path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "schemas");
            return new CSVSchema(System.IO.Path.Combine(path, System.IO.Path.ChangeExtension(ISPName, "schema")));
        }

        private CSVSchema LoadTableSchema(String Name)
        {
            String path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "tables");
            return new CSVSchema(System.IO.Path.Combine(path, System.IO.Path.ChangeExtension(Name, "schema")));
        }

        private void ExportAll()
        {
            Cursor = Cursors.WaitCursor;

            var toExport = hitsView.FilteredObjects.Cast<Hit>()
                .OrderBy( (item) => item.Country )
                .OrderBy( (item) => item.Date )
                .ToList();

            toExport.ForEach((hit) => hit.Timezone = GetTimeZone(hit));
            toExport.ForEach((hit) => hit.Downloaded = ExporterDB.GetDownloadedAmount(hit, CurrentProject));

            ExporterDB.UpdateFilesNoInHits(toExport, CurrentProject);

            Dictionary<String, Object> FakeAttributes = new Dictionary<string, object>
            { 
                { "AKZ",         CurrentProject.AKZ   },            
                { "Now",         DateTime.Now         },
                { "ProjectName", CurrentProject.Name  },
            };

            CSVSchema schema = LoadSchema("ALL");
            CSVSchema table_schema = null;

            if (tableGen.Checked)
                table_schema = LoadTableSchema("ALL");

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".xls";
            dlg.Filter = "XLS files|*.xls";
            dlg.FileName = schema.GetFilename(new SchemaProvider(FakeAttributes));
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                int maxInFile = 65000;
                int filesNum = (int)(Math.Ceiling((decimal)toExport.Count() / maxInFile));
                for (int i = 0; i < filesNum; i++)
                {
                    int offset = i * maxInFile;
                    int count = maxInFile;
                    if ((offset + count) > toExport.Count)
                        count = toExport.Count - offset;

                    var exportSublist = toExport.GetRange(offset, count);

                    String Filename = String.Format( "{0}_{1}", Path.GetFileNameWithoutExtension(dlg.FileName), i);
                    String Directory = Path.GetDirectoryName(dlg.FileName);
                    String Ext = Path.GetExtension(dlg.FileName);

                    CSVExporter exporter = new XMLExporter(Path.Combine(Directory, Path.ChangeExtension(Filename, Ext)));
                    exporter.Export(exportSublist, CurrentProject, schema);
                }

                if (table_schema != null)
                    GenerateWordTable(toExport, table_schema);

                ExporterDB.MarkAsExported(CurrentProject, toExport);
            }

            UpdateUI(false);
            Cursor = Cursors.Default;
        }

        private void ExportByCountry()
        {
                Cursor = Cursors.WaitCursor;

                var toExport = hitsView.FilteredObjects.Cast<Hit>().ToList();
                toExport.ForEach((hit) => hit.Timezone = GetTimeZone(hit));
                toExport.ForEach((hit) => hit.Downloaded = ExporterDB.GetDownloadedAmount(hit, CurrentProject));
                var byCountry = toExport.GroupBy((hit) => hit.Country);

                ExporterDB.UpdateFilesNoInHits(toExport, CurrentProject);

                foreach (var group in byCountry)
                {
                    if (group.Count() == 0)
                        continue;

                    CSVSchema schema = LoadSchema(group.Key);
                    CSVSchema table_schema = null;
                    if (tableGen.Checked)
                        table_schema = LoadTableSchema(group.Key);

                    Dictionary<String, Object> FakeAttributes = new Dictionary<string, object>
                                                                { 
                                                                    { "Now",         DateTime.Now         },
                                                                    { "ProjectName", CurrentProject.Name  },
                                                                    { "Hit",         group.First()        },
                                                                    { "AKZ",         CurrentProject.AKZ   }
                                                                };

                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.DefaultExt = ".xls";
                    dlg.Filter = "XLS files|*.xls";
                    dlg.FileName = schema.GetFilename(new SchemaProvider(FakeAttributes));
                    dlg.RestoreDirectory = true;

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        var HitsToExport = group.OrderBy((hit) => hit.Date);

                        CSVExporter exporter = new XMLExporter(dlg.FileName);
                        exporter.Export(HitsToExport.ToList(), CurrentProject, schema);

                        if (table_schema != null)
                            GenerateWordTable(HitsToExport, table_schema);

                        ExporterDB.MarkAsExported(CurrentProject, HitsToExport);
                    }

                    UpdateUI(false);
                }

            Cursor = Cursors.Default;
        }

        private void ExportByISP()
        {
                Cursor = Cursors.WaitCursor;

                var toExport = hitsView.FilteredObjects.Cast<Hit>().ToList();
                toExport.ForEach((hit) => hit.Timezone = GetTimeZone(hit));
                toExport.ForEach((hit) => hit.Downloaded = ExporterDB.GetDownloadedAmount(hit, CurrentProject));

                var byISP = toExport.GroupBy((hit) => hit.Isp);

                ExporterDB.UpdateFilesNoInHits(toExport, CurrentProject);

                foreach (var group in byISP)
                {
                    if (group.Count() == 0)
                        continue;

                    CSVSchema schema = null;
                    try{
                       schema = LoadSchema(group.Key);
                    }catch (IOException){
                        schema = LoadSchema("ALL");
                    }

                    CSVSchema table_schema = null;
                    if (tableGen.Checked)
                    {
                        try{
                            table_schema = LoadTableSchema(group.Key);
                        }catch (IOException){
                            table_schema = LoadTableSchema("ALL");
                        }
                    }
                    Dictionary<String, Object> FakeAttributes = new Dictionary<string, object>
                                                                { 
                                                                    { "Now",         DateTime.Now         },
                                                                    { "ProjectName", CurrentProject.Name  },
                                                                    { "Hit",         group.First()        },
                                                                    { "AKZ",         CurrentProject.AKZ   }
                                                                };

                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.DefaultExt = "." + schema.GetOutputFormat().ToString().ToLower();
                    dlg.FileName = schema.GetFilename(new SchemaProvider(FakeAttributes));
                    dlg.RestoreDirectory = true;

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        var HitsToExport = group.OrderBy((hit) => hit.Date);
                        CSVExporter exporter = null;

                        if (schema.GetOutputFormat() == FileFormat.CSV)
                            exporter = new CSVExporter(dlg.FileName);
                        else
                            exporter = new XMLExporter(dlg.FileName);
                        exporter.Export(HitsToExport.ToList(), CurrentProject, schema);

                        if (table_schema != null)
                            GenerateWordTable(HitsToExport, table_schema);

                        ExporterDB.MarkAsExported(CurrentProject, HitsToExport);
                    }

                    UpdateUI(true);
                }

            Cursor = Cursors.Default;
        }

        private void GenerateWordTable(IEnumerable<Hit> hits, CSVSchema schema)
        {
            if (!tableGen.Checked)
                return;

            using(Word word = new Word(true))
            {
                var dlg = new OpenFileDialog();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    WordDoc doc = word.Open(dlg.FileName);

                    var tableHolder = doc.FindAllTablePlaceholders()
                        .FirstOrDefault(p => p.PlaceholderName == "TABLE");

                    var rangeDate = doc.FindAllPlaceholders()
                        .FirstOrDefault(p => p.PlaceholderName == "RANGE");

                    var actualDate = doc.FindAllPlaceholders()
                        .FirstOrDefault(p => p.PlaceholderName == "DATE");

                    var benkenRange = doc.FindAllPlaceholders()
                       .FirstOrDefault(p => p.PlaceholderName == "BENKEN");

                    if (tableHolder != null)
                    {
                        var table = new WordTable(tableHolder.Range, schema);

                        table.Fill(hits, schema, CurrentProject);
                        doc.Save();
                    }
                    else
                        throw new Exception("No 'TABLE' marker found! Put 'TABLE' marker in .doc file!");

                    if (rangeDate != null)
                    {
                        String format = "dd.MM.yy";
                        String firstDate = hits.OrderBy(h => h.Date).First().Date.ToString(format);
                        String now = DateTime.Now.ToString(format);

                        rangeDate.Replace(String.Format("{0} und dem {1}", firstDate, now));
                    }
                    else
                        throw new Exception("No 'RANGE' marker found!");


                    if (actualDate != null)
                    {
                        actualDate.Replace(DateTime.Now.ToString("dd MMMM yyyy"));
                    }
                    else
                        throw new Exception("No 'DATE marker found!");

                    if (benkenRange != null)
                    {
                        benkenRange.Replace(hits.First().BennKenn);
                    }

                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            List<Hit> toExport;

            try
            {
                if (CheckedCountries.Count == 0 && CheckedISP.Count == 0)
                    ExportAll();

                if (exportByCountry.Checked)
                    ExportByCountry();
                else
                    ExportByISP();
            }
            catch (IOException ex)
            {
                MessageBox.Show(String.Format("No schema file: {0}", ex.Message));
            }
            finally
            {

                UpdateUI(true);
                Cursor = Cursors.Default;
            }
        }

        private void ispCombo_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
            bool Check = e.NewValue == CheckState.Checked;
            String Isp = TypeUtils.GetValueFromAnonymousType<String>(ispCombo.Items[e.Index], "Name");

            if (Check)
                CheckedISP.Add(Isp);
            else
                CheckedISP.Remove(Isp);
        }

        private List<String> GetFilesToSelect(String Path)
        {
            return System.IO.Directory.GetFiles(Path)
                .Select((fullPath) => System.IO.Path.GetFileName(fullPath))
                .ToList();
        }

        private void countryCombo_DropDownClosed(object sender, EventArgs e)
        {
            if (countryCombo.CheckedItems.Count > 0)
            {
                UpdateHitsView(null);

                ReloadISPcombo(hitsView.FilteredObjects.Cast<Hit>());
                ReloadRegionCombo(hitsView.FilteredObjects.Cast<Hit>());
            }
        }

        private void ispCombo_DropDownClosed(object sender, EventArgs e)
        {
            if( ispCombo.CheckedItems.Count > 0 )
               UpdateHitsView(null);
        }

        private void regionCombo_DropDownClosed(object sender, EventArgs e)
        {
            if( regionCombo.CheckedItems.Count > 0 )
                UpdateHitsView(null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to clear all exported?", 
                        "Exporter", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DateTime? startDate = startDatePicker.Value;
                DateTime? endDate = endDatePicker.Value;

                ExporterDB.ClearExported(CurrentProject, startDate, endDate);
                InitProjectsTree();
                UpdateUI(true);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CurrentProject == null)
                MessageBox.Show("No project selected.", "EXporter",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (MessageBox.Show("Do you really want to delete this project?",
                        "Exporter", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Cursor = Cursors.WaitCursor;
                ExporterDB.DeleteProject(CurrentProject);
                InitProjectsTree();
                CurrentProject = null;
                UpdateUI(true);
                Cursor = Cursors.Default;
            }
        }

        private void startDatePicker_CloseUp(object sender, EventArgs e)
        {
            UpdateHitsView(null);
        }

        private void endDatePicker_CloseUp(object sender, EventArgs e)
        {
            UpdateHitsView(null);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Object obj in filesView.Objects)
            {
                if( checkAll.Checked )
                    filesView.CheckObject(obj);
                else
                    filesView.UncheckObject(obj);
            }
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            CheckedRegions.Clear();
            CheckedCountries.Clear();
            CheckedISP.Clear();

            UpdateUI(true);
            Cursor = Cursors.Default;
        }

        private void hashSearch_TextChanged(object sender, EventArgs e)
        {
            BrightIdeasSoftware.TextMatchFilter filter = BrightIdeasSoftware.TextMatchFilter.Prefix(filesView, hashSearch.Text);
            columnHashFiles.UseFiltering = true;
            filesView.ModelFilter = filter;
        }

        private void projectsTree_FormatCell(object sender, BrightIdeasSoftware.FormatCellEventArgs e)
        {
            if (e.Model is Project)
            {
                if (e.ColumnIndex == columnFilename.Index && CurrentProject != null)
                {
                    Project p = (Project)e.Model;
                    if (p.Name == CurrentProject.Name)
                        e.SubItem.ForeColor = Color.Red;
                }
            }
        }

        private void searchHits_TextChanged(object sender, EventArgs e)
        {
            BrightIdeasSoftware.TextMatchFilter filter = BrightIdeasSoftware.TextMatchFilter.Prefix(hitsView, searchHits.Text);
            hitsView.ModelFilter = filter;
        }

        private void hitsView_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            ToExportCount.Text = String.Format("{0}", hitsView.GetItemCount());
        }

        private void projectsTree_ItemActivate(object sender, EventArgs e)
        {
            statusText.Text = "Loading Project";
            Cursor = Cursors.WaitCursor;

            if (projectsTree.SelectedObject == null)
                return;

            if (projectsTree.SelectedObject is Project)
            {
                if ((Project)projectsTree.SelectedObject != CurrentProject && projectsTree.SelectedObject != null)
                {
                    if (projectsTree.SelectedObject is Project)
                    {
                        SetCurrentProject((Project)projectsTree.SelectedObject);
                    }

                    ExporterDB.FixProjectSchema(CurrentProject);

                    UpdateUI(true);
                    filesView.SetObjects(ArrayUtils.Empty<Object>());
                }

                statusText.Text = String.Format("Project \"{0}\" loaded", CurrentProject.Name);
                ClearExportedBtn.Enabled = DeleteBtn.Enabled = EditBtn.Enabled = addHitDB.Enabled = true;
                DeleteFileBtn.Enabled = false;
            }
            else
            {
                DeleteFileBtn.Enabled = true;
            }

            Cursor = Cursors.Default;  
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Object obj = projectsTree.SelectedObject;
            String hash = TypeUtils.GetValueFromAnonymousType<String>(obj, "AKZ");

            Cursor = Cursors.WaitCursor;
            InitProjectsTree();
            ExporterDB.DeleteFile(CurrentProject, hash);
            UpdateUI(true);
            Cursor = Cursors.Default;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (projectsTree.SelectedObject is Project)
            {
                Project proj = (Project)projectsTree.SelectedObject;

                NewProjectDlg dlg = new NewProjectDlg(proj);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ExporterDB.UpdateProject(proj, dlg.UniqueChanged());
                }
            }
        }

        private void bulkLoadBtn_Click(object sender, EventArgs e)
        {
            if(CurrentProject == null)
            {
                MessageBox.Show("Select project first!", "BPExporter Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                DisableLoad();

                string path = dlg.SelectedPath;
                var files = DBUtils.ListDBFiles(path);
                DBUtils.Fix(files, ShowFixProgress(files.Length));
                RunBulkLoad(path, files);
            }

        }

        private void DisableLoad()
        {
            bulkLoadBtn.Enabled = false;
            addHitDB.Enabled = false;
        }

        private void EnableLoad()
        {
            bulkLoadBtn.Enabled = true;
            addHitDB.Enabled = true;
        }

        private void RunBulkLoad(Object path, IEnumerable<String> files)
        { 
            HitsDBs = new List<DB>();
            String db_path = (String)path;

            foreach (string file in files)
            {
                HitsDBs.Add(new DB(file));
                logger.Info(file);
            }

            activeDB.Text = db_path;
            ProcessNewDB(HitsDBs, false, logger);
        }

        private Action<String, int> ShowFixProgress(int Count)
        {
            /*
                Progress.Maximum = Count;
            */
            return (String f, int c) => { ShowFixProgressDetails(f, c); };
        }

        private void ShowFixProgressDetails(String filename, int Count)
        {
            statusText.Text = String.Format("Checking database: {0}", Path.GetFileName(filename));
            //Progress.Value = Count;
        }

        private void ClearAllBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to clear this project?",
                        "Exporter", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ExporterDB.ClearAllHits(CurrentProject);
               
                InitProjectsTree();
                UpdateUI(true);
            }
        }

        private void countryCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            countryCombo_DropDownClosed(sender, e);
        }

        private void ispCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ispCombo_DropDownClosed(sender, e);
        }

        private void regionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            regionCombo_DropDownClosed(sender, e);
        }

        private void TimeZone_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            CurrentProject.TimeZone = TimeZoneInfo.FindSystemTimeZoneById((String)TimeZone.SelectedItem);
            UpdateUI(true);
        }
    }
}
