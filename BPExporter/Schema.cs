using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;

namespace BPExporter
{
    public class DB
    {
        private SQLiteConnection connection;
        private String path;
        public List<Hit> ListCache;
        public Dictionary<String, List<Hit>> HitsCache;

        public DB(String pth)
        {
            path = pth;
            string connStr = String.Format("Data Source={0};Version=3;", path);
            connection = new SQLiteConnection(connStr);
            connection.Open();

            FixSchema();
        }

        public void Close()
        {
            connection.Close();
        }

        public void CreateNewProject(Project project)
        {
            string Sql = @"INSERT INTO Project(name, files_no, date, prefix, sufix, unique_ip) values(@name, @files_no, @date, @prefix, @sufix, @unique_ip)";
            SQLiteCommand cmd = new SQLiteCommand( Sql, connection );
            
            String nameWithProduct = String.Format("{0}#{1}", 
                project.Name.Replace(" ", ""), project.Product.Replace(" ", ""));

            cmd.Parameters.Add(new SQLiteParameter( "name", nameWithProduct));
            cmd.Parameters.Add( new SQLiteParameter( "files_no", project.FilesNo ) );
            cmd.Parameters.Add( new SQLiteParameter( "date", project.CreationDate ) );
            cmd.Parameters.Add(new SQLiteParameter( "prefix", project.Prefix));
            cmd.Parameters.Add(new SQLiteParameter( "sufix", project.Sufix));
            cmd.Parameters.Add(new SQLiteParameter( "unique_ip", project.UniqueIP ? 1 : 0));

            CreateProjectTable(project);

            cmd.ExecuteNonQuery();

            project.ID = GetLastInsertRowId();
        }

        public void UpdateProject(Project project, bool uniqueChanged)
        {
            string Sql = @"UPDATE Project set prefix=@prefix, sufix=@sufix, files_no=@files_no, unique_ip=@unique_ip where id = @id";
            SQLiteCommand cmd = new SQLiteCommand(Sql, connection);

            cmd.Parameters.Add(new SQLiteParameter("prefix", project.Prefix));
            cmd.Parameters.Add(new SQLiteParameter("sufix", project.Sufix));
            cmd.Parameters.Add(new SQLiteParameter("files_no", project.FilesNo));
            cmd.Parameters.Add(new SQLiteParameter("unique_ip", project.UniqueIP));
            cmd.Parameters.Add(new SQLiteParameter("id", project.ID));

            cmd.ExecuteNonQuery();

            if (uniqueChanged)
                RemoveUniqueFromProject(project);
        }

        private void CreateProjectTable(Project project)
        {
            String tableName = project.TableName;
            String Sql = @"CREATE TABLE {0}(
                 [guid] varchar(255) UNIQUE ON CONFLICT IGNORE DEFAULT ''
                ,[url] text DEFAULT ''
                ,[city] varchar(50) DEFAULT ''
                ,[country] varchar(5) DEFAULT ''
                ,[filename] varchar(500) DEFAULT ''
                ,[ip] varchar(20) DEFAULT ''
                ,[our_ip] varchar(20) DEFAULT ''
                ,[port] integer DEFAULT 0
                ,[our_port] integer DEFAULT 0
                ,[timezone] varchar(255) DEFAULT ''
                ,[type] integer DEFAULT 0
                ,[client] varchar(255) DEFAULT ''
                ,[date] datetime DEFAULT ''
                ,[hash] varchar(255) DEFAULT ''
                ,[piece_hash] varchar(255) DEFAULT ''
                ,[piece_number] integer DEFAULT 0
                ,[piece_size] integer DEFAULT 0
                ,[block_size] integer DEFAULT 0
                ,[block_hash] varchar(50) DEFAULT ''
                ,[block_number] integer DEFAULT 0
                ,[id] integer PRIMARY KEY ASC ON CONFLICT Rollback AUTOINCREMENT UNIQUE NOT NULL
                ,[exported] int DEFAULT 0
                ,[isp] varchar(255) DEFAULT ''
                ,[bennkenn] varchar(255) DEFAULT ''
                ,[time] datetime
                ,[size] int DEFAULT 0
                ,[region] varchar(255) DEFAULT ''
                ,[disconnect_time] datetime DEFAULT NULL
                ,[sig] varchar(255) DEFAULT NULL
                ,[downloaded] integer DEFAULT 0";

            if( !project.UniqueIP )
                Sql +=  ",CONSTRAINT [hit_unique] UNIQUE([ip],[date]) ON CONFLICT IGNORE);";
            else
                Sql +=  ",CONSTRAINT [hit_ip_unique] UNIQUE([ip]) ON CONFLICT IGNORE);";

            SQLiteCommand cmd = new SQLiteCommand(String.Format(Sql, tableName), connection);
            cmd.ExecuteNonQuery();

            String indexSQL = 
            @"CREATE INDEX [exported_index_{0}] On [{0}] ([exported] Collate NOCASE ASC);           
            CREATE INDEX [isp_{0}] On [{0}] ([isp] Collate NOCASE ASC);
            CREATE INDEX [hash_{0}] On [{0}] ([hash] Collate NOCASE ASC);
            CREATE INDEX [country_{0}] On [{0}] ([country] Collate NOCASE ASC);
            CREATE INDEX [filename_{0}] On [{0}] ([filename] Collate NOCASE ASC);
            CREATE INDEX [date_{0}] On [{0}] ([date] Collate NOCASE ASC);";
            
            cmd = new SQLiteCommand(String.Format(indexSQL, tableName), connection);
            cmd.ExecuteNonQuery();
        }

        public void RemoveUniqueFromProject(Project project)
        {
            Project cpy = project.Copy();
  
            CreateProjectTable(cpy);

            String copyStm = @"INSERT INTO {1} SELECT * FROM {0};
                               DROP TABLE {0};
                               ALTER TABLE {1} RENAME TO {0}";

            SQLiteCommand cmd = new SQLiteCommand(String.Format(copyStm, project.TableName, cpy.TableName), connection);
            cmd.ExecuteNonQuery();
        }

        public List<Object[]> GetAllCities(IEnumerable<Hit> Source)
        {
            return GetAllGroupedByFromCache(new String[] { "City" }, "City", Source);
        }

        public List<Object[]> GetAllFilesNamesFromHits(IEnumerable<Hit> Source)
        {
            return GetAllGroupedByFromCache(new String[] { "Filename", "Hash", "FormatedSize", "FileSize" }, "Hash", Source);
        }

        public List<Object[]> GetAllCountries(IEnumerable<Hit> Source)
        {
            return GetAllGroupedByFromCache(new String[] { "Country" }, "Country", Source);
        }

        public List<Object[]> GetAllISP(IEnumerable<Hit> Source)
        {
            return GetAllGroupedByFromCache(new String[] { "isp" }, "isp", Source);
        }

        public List<String[]> GetAllGroupedBy(String[] Columns, String GroupBy)
        {
            string ColumnClause = String.Join( ",", Columns );
            string Sql = String.Format( @"SELECT {0} FROM hits GROUP BY {1}", ColumnClause, GroupBy );
            
            SQLiteCommand cmd = new SQLiteCommand(Sql, connection);
 
            List<String[]> names = new List<String[]>();
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                String[] values = new String[Columns.Length];
                for (int i = 0; i < Columns.Length; i++)
                    values[i] = reader[i].ToString();
                names.Add( values );
            }

            return names;
        }

        public List<Object[]> GetAllGroupedByFromCache(String[] Columns, String GroupBy, IEnumerable<Hit> Source)
        {
            List<Object[]> names = new List<Object[]>();
            var grouped = Source.GroupBy((item) => new Munger(GroupBy).GetValue(item));

            foreach( var group in grouped )
            {
                var Key = group.Key;
                var Item = group.First();

                Object[] values = new Object[Columns.Length+1];
                for (int i = 0; i < Columns.Length; i++)
                    values[i] = new Munger(Columns[i]).GetValue(Item).ToString();
                values[values.Length - 1] = group.Count();

                names.Add(values);
            }

            return names;
        }

        public List<File> GetAllProjectFiles(Project project) 
        {
            string Sql = @"SELECT * FROM file f JOIN file_to_project p on f.id = p.id_file WHERE id_project = @project_id";
            SQLiteCommand cmd = new SQLiteCommand(Sql, connection);

            cmd.Parameters.Add( new SQLiteParameter( "project_id", project.ID ) );

            List<File> files = new List<File>();
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                files.Add( new File( reader ) );
            }

            return files;   
        }

        public List<Project> GetAllProjects()
        {
            string Sql = @"SELECT * FROM project";
            SQLiteCommand cmd = new SQLiteCommand(Sql, connection);

            List<Project> projects = new List<Project>();
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                projects.Add( new Project( reader ) );
            }

            return projects;   
        }

        public void InsertNewHit(Project project, Hit hit, String FilesNo, IEnumerable<String> countries)
        {
            String sql = "INSERT INTO {0} " + 
                "(ip, port, date, guid, url, city, country, filename, type, client, hash, isp, timezone, bennkenn, time, region, size, piece_hash, piece_number, disconnect_time, block_hash, block_number, block_size, piece_size, our_ip, our_port, sig) VALUES" +
                "(@ip, @port, @date, @guid, @url, @city, @country, @filename, @net_type, @client, @hash, @isp, @timezone, @bennkenn, @time, @region, @filesize, @piece_hash, @piece_number, @disconnect_time, @block_hash, @block_number, @block_size, @piece_size, @our_ip, @our_port, @sig)";

            SQLiteCommand cmd = new SQLiteCommand(String.Format(sql, project.TableName), connection);

            String Country = hit.GetCountryByIP(hit.IP);
            String Isp = hit.GetISPByIP(hit.IP);
            String City = hit.GetCityByIP(hit.IP);
            String Region = hit.GetRegionByIP(hit.IP);

            if (String.IsNullOrEmpty(Country) || String.IsNullOrEmpty(Isp))
                return;

            if (countries.Count() != 0 && !countries.Contains(Country))
                return; 

            cmd.Parameters.Add( new SQLiteParameter( "ip",      hit.IP ) );
            cmd.Parameters.Add( new SQLiteParameter( "port",    hit.Port ) );
            cmd.Parameters.Add( new SQLiteParameter( "date",    hit.UTCDate ) );
            cmd.Parameters.Add( new SQLiteParameter( "time",    hit.UTCTime));
            cmd.Parameters.Add( new SQLiteParameter( "guid",    hit.GUID) );
            cmd.Parameters.Add( new SQLiteParameter( "url",     hit.URL) );
            cmd.Parameters.Add( new SQLiteParameter( "filename",hit.Filename) );
            cmd.Parameters.Add( new SQLiteParameter( "net_type",hit.NetType) );
            cmd.Parameters.Add( new SQLiteParameter( "client",  hit.Client) );
            cmd.Parameters.Add( new SQLiteParameter( "hash",    hit.Hash) );
            cmd.Parameters.Add( new SQLiteParameter( "filesize",hit.FileSize) );

            cmd.Parameters.Add(new SQLiteParameter("our_ip", hit.BaseprotectIP));
            cmd.Parameters.Add(new SQLiteParameter("our_port", hit.BaseprotectPort));

            cmd.Parameters.Add( new SQLiteParameter( "isp",     Isp));
            cmd.Parameters.Add( new SQLiteParameter( "city",    City));
            cmd.Parameters.Add( new SQLiteParameter( "country", Country));
            cmd.Parameters.Add( new SQLiteParameter( "region",  Region));

            cmd.Parameters.Add( new SQLiteParameter( "timezone", hit.Timezone));
            cmd.Parameters.Add( new SQLiteParameter( "bennkenn", project.AKZ));

            cmd.Parameters.Add(new SQLiteParameter("piece_hash", hit.PieceHash));
            cmd.Parameters.Add(new SQLiteParameter("piece_number", hit.PieceNumber));
            cmd.Parameters.Add(new SQLiteParameter("block_hash", hit.BlockHash));
            cmd.Parameters.Add(new SQLiteParameter("block_number", hit.BlockNumber));
            cmd.Parameters.Add(new SQLiteParameter("piece_size", hit.PieceSize));
            cmd.Parameters.Add(new SQLiteParameter("block_size", hit.BlockSize));

            cmd.Parameters.Add(new SQLiteParameter("disconnect_time", hit.Disconnect));
            cmd.Parameters.Add(new SQLiteParameter("sig", hit.Sig));

            try
            {
                cmd.ExecuteNonQuery();
                hit.ID = GetLastInsertRowId();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void InsertHits(Project project, List<Hit> hits, String FilesNo, IEnumerable<String> countries,  Action<long, long> callback)
        {
            using ( SQLiteTransaction trans = connection.BeginTransaction() )
            {
                long Count = 0;
                foreach (Hit hit in hits)
                {
                    InsertNewHit(project, hit, FilesNo, countries);
                    if(callback != null)
                        callback.Invoke(hits.Count, Count++);
                }

                trans.Commit();
            }
        }

        public File InsertNewFile(String filename, String hash, Project project)
        {
            File file = new File();

            file.Name = filename;
            file.Hash = hash;

            String Sql = @"INSERT INTO file(filename, hash) VALUES(@filename, @hash)";

            SQLiteCommand cmd = new SQLiteCommand(Sql, connection);
            
            cmd.Parameters.Add(new SQLiteParameter("filename", file.Name));
            cmd.Parameters.Add(new SQLiteParameter("hash", file.Hash));



            cmd.ExecuteNonQuery();

            file.ID = GetLastInsertRowId();
            return file;
        }

        public void AddFilesToProject(Project project, List<Object[]> filesData)
        {
            using( SQLiteTransaction transaction = connection.BeginTransaction())
            {
                foreach (Object[] data in filesData)
                {
                    File f = InsertNewFile((String)data[0], (String)data[1], project);
                    String Sql = @"INSERT INTO file_to_project(id_file, id_project) VALUES(@file, @project)";

                    SQLiteCommand cmd = new SQLiteCommand(Sql, connection);
                    cmd.Transaction = transaction;

                    cmd.Parameters.Add(new SQLiteParameter("file", f.ID));
                    cmd.Parameters.Add(new SQLiteParameter("project", project.ID));

                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
            }
        }

        public long Count(String From)
        {
            String SqlCount = String.Format(@"SELECT count(*) as count from {0};", From);
            long max = (long)new SQLiteCommand(SqlCount, connection).ExecuteScalar();
            return max;
        }

        public long GetDownloadedAmount(Hit hit , Project project)
        {
            String SqlCount = String.Format(@"SELECT SUM(block_size) as count from {0} where guid='{1}' and hash='{2}'", project.TableName, hit.GUID, hit.Hash);
            return (long)new SQLiteCommand(SqlCount, connection).ExecuteScalar();
        }

        public void CacheHitsByHashes(String From, Action<long, long> Callback, HashSet<String> Countries)
        {
            if ( HitsCache != null || ListCache != null )
                return;

            HitsCache = new Dictionary<string, List<Hit>>();
            ListCache = new List<Hit>();

            String Sql = String.Format(@"SELECT * from {0};", From);
            SQLiteCommand cmd = new SQLiteCommand(Sql, connection);
            SQLiteDataReader reader = cmd.ExecuteReader();

            long Counter = 0;
            long Max = Count(From);

            while (reader.Read())
            {
                String url = TypeUtils.GetFromReader<String>(reader, "url");

                String hash = String.Empty;
                if(reader.HasColumn("hash"))
                    hash = TypeUtils.GetFromReader<String>(reader, "hash").FixHash(url);
                else if(reader.HasColumn("t_hash"))
                    hash = TypeUtils.GetFromReader<String>(reader, "t_hash").FixHash(url);

                if (!String.IsNullOrEmpty(hash))
                {
                    if (!HitsCache.ContainsKey(hash))
                        HitsCache.Add(hash, new List<Hit>());

                    Hit hit = new Hit(reader, false, true);

                    if (Countries.Count == 0 || Countries.Contains(hit.GetCountryByIP(hit.IP)))
                    {
                        if (hit.IsValid())
                        {
                            HitsCache[hash].Add(hit);
                            ListCache.Add(hit);
                        }
                    }
                }

                if(++Counter % 20 == 0)
                    Callback.Invoke(Max, Counter);
            }
        }

        public List<Hit> GetAllFileHits(String From, String hash)
        {
            try
            {
                return HitsCache[hash];
            }
            catch (Exception e) { return new List<Hit>(); }
        }

        public List<Hit> GetNotExported(Project project)
        {
            List<Hit> hits = new List<Hit>();
            String Sql = @"SELECT id, ip, port, date, guid, url, city, country, filename, type, client, hash, isp, timezone, bennkenn, time, region, size, block_size, piece_number, piece_hash, our_ip, our_port, block_number, block_hash, sig from {0} where exported = 0;";

            SQLiteCommand cmd = new SQLiteCommand(String.Format(Sql, project.TableName), connection);
            cmd.Parameters.Add(new SQLiteParameter("project", project.ID));

            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                hits.Add(new Hit(reader));
            }

            return hits;
        }

        public void MarkAsExported(Project project, IEnumerable<Hit> hits)
        {
            using( SQLiteTransaction transaction = connection.BeginTransaction())
            {
                project.IncrementFilesNo();

                String Sql = @"UPDATE project SET files_no = @files_no where id = @id";
                SQLiteCommand cmd = new SQLiteCommand(Sql, connection);

                cmd.Parameters.Add(new SQLiteParameter("files_no", project.FilesNo));
                cmd.Parameters.Add(new SQLiteParameter("id", project.ID));

                cmd.ExecuteNonQuery();

                foreach (Hit hit in hits)
                {
                    Sql = @"UPDATE {0} SET exported = @date where id = @id";
                    cmd = new SQLiteCommand(String.Format(Sql, project.TableName), connection);

                    cmd.Parameters.Add(new SQLiteParameter("date", DateTime.Now.ToFileTime()));
                    cmd.Parameters.Add(new SQLiteParameter("id", hit.ID));

                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
        }

        private long GetLastInsertRowId()
        {
            SQLiteCommand IDCmd = new SQLiteCommand(@"SELECT last_insert_rowid();", connection);
            return (long)IDCmd.ExecuteScalar();
        }

        public String GetDateRange()
        {
            var Ordered = ListCache.OrderBy(hit => hit.Date);
            String first = String.Format("{0:d}", Ordered.First().Date);
            String last = String.Format("{0:d}", Ordered.Last().Date);
            return String.Format("{0} - {1}", first, last);
        }

        public long ClearAllHits(Project project)
        {
            //Clear all hits from project, but leave all hashes untouched.
            String Sql = @"delete from {0}";

            SQLiteCommand DeleteCmd = new SQLiteCommand(String.Format(Sql, project.TableName), connection);
            DeleteCmd.Parameters.Add(new SQLiteParameter("project_id", project.ID));

            return DeleteCmd.ExecuteNonQuery();
        }

        public long ClearExported(Project project, DateTime? startDate, DateTime? endDate)
        {
            String Sql = @"UPDATE {0} SET exported = 0 where id in (select id from {0} where hash in (select hash from file f join file_to_project p on f.id = p.id_file where id_project=@project_id )) and date between @startDate and @endDate";
            SQLiteCommand UpdateCmd = new SQLiteCommand(String.Format(Sql, project.TableName), connection);
            UpdateCmd.Parameters.Add( new SQLiteParameter( "project_id", project.ID ));

            UpdateCmd.Parameters.Add( new SQLiteParameter( "startDate", startDate));
            UpdateCmd.Parameters.Add( new SQLiteParameter( "endDate", endDate));
            
            return UpdateCmd.ExecuteNonQuery();
        }

        public void DeleteFile(Project prj, String hash)
        {
            SQLiteCommand DeleteCmd = new SQLiteCommand(@"DELETE from file_to_project where id_project = @project_id and id_file = (select id from file where hash = @hash)", connection);
            DeleteCmd.Parameters.Add(new SQLiteParameter("project_id", prj.ID));
            DeleteCmd.Parameters.Add(new SQLiteParameter("hash", hash));

            DeleteCmd.ExecuteNonQuery();

            DeleteCmd = new SQLiteCommand(@"DELETE from file where hash = @hash", connection);
            DeleteCmd.Parameters.Add(new SQLiteParameter("hash", hash));
            DeleteCmd.ExecuteNonQuery();

            DeleteCmd.ExecuteNonQuery();

            DeleteCmd = new SQLiteCommand(String.Format("DELETE from {0} where hash = @hash", prj.TableName), connection);
            DeleteCmd.Parameters.Add(new SQLiteParameter("hash", hash));
            DeleteCmd.ExecuteNonQuery();

            DeleteCmd.ExecuteNonQuery();
        }

        public void DeleteProject(Project project)
        {
            String Sql = @"DELETE from {0} where id in (select id from {0} where hash in (select hash from file f join file_to_project p on f.id = p.id_file where id_project=@project_id ))";
            SQLiteCommand DeleteCmd = new SQLiteCommand(String.Format(Sql, project.TableName), connection);
            DeleteCmd.Parameters.Add(new SQLiteParameter("project_id", project.ID));
            DeleteCmd.ExecuteNonQuery();

            DeleteCmd = new SQLiteCommand(@"DELETE from project where id = @project_id", connection);
            DeleteCmd.Parameters.Add(new SQLiteParameter("project_id", project.ID));
            DeleteCmd.ExecuteNonQuery();

            DeleteCmd = new SQLiteCommand(@"DELETE from file where id in (select id_file from file_to_project where id_project = @project_id)", connection);
            DeleteCmd.Parameters.Add(new SQLiteParameter("project_id", project.ID));
            DeleteCmd.ExecuteNonQuery();

            DeleteCmd = new SQLiteCommand(@"DELETE from file_to_project where id_project = @project_id", connection);
            DeleteCmd.Parameters.Add(new SQLiteParameter("project_id", project.ID));
            DeleteCmd.ExecuteNonQuery();

            DeleteCmd = new SQLiteCommand(string.Format(@"DROP TABLE IF EXISTS {0}", project.TableName), connection);
            DeleteCmd.ExecuteNonQuery();
        }

        public void UpdateFilesNoInHits(List<Hit> hits, Project project)
        {
            foreach (Hit hit in hits){
                hit.BennKenn = project.AKZ;
            }

            String Sql = @"UPDATE {0} SET bennkenn=@bennkenn where id in ({1})";
            SQLiteCommand cmd = new SQLiteCommand(String.Format(Sql, project.TableName, 
                String.Join(",", hits.Select( hit => hit.ID.ToString() ).ToArray() )), connection); 
        
            cmd.Parameters.Add( new SQLiteParameter("bennkenn", project.AKZ));
            cmd.ExecuteNonQuery();
        }

        public void RunInTransaction(Action action)
        {
            using (SQLiteTransaction trans = connection.BeginTransaction())
            {
                action.Invoke();
                trans.Commit();
            }
        }

        public void FixSchema()
        {
            try
            {
                new SQLiteCommand(@"ALTER TABLE hits ADD COLUMN city TEXT;", connection).ExecuteNonQuery(); ;
            }
            catch (Exception) { }

            try
            {
                new SQLiteCommand(@"ALTER TABLE hits ADD COLUMN region TEXT", connection).ExecuteNonQuery();
            }
            catch (Exception) { }

            try
            {
                new SQLiteCommand(@"ALTER TABLE hits ADD COLUMN downloaded INTEGER", connection).ExecuteNonQuery();
            }
            catch (Exception) { }

            try
            {
                new SQLiteCommand(@"ALTER TABLE hits ADD COLUMN piece_number INTEGER", connection).ExecuteNonQuery();
            }
            catch (Exception) { }

            try
            {
                new SQLiteCommand(@"ALTER TABLE hits ADD COLUMN piece_hash INTEGER", connection).ExecuteNonQuery();
            }
            catch (Exception) { }

            try
            {
                new SQLiteCommand(@"ALTER TABLE hits ADD COLUMN our_ip INTEGER", connection).ExecuteNonQuery();
            }
            catch (Exception) { }

            try
            {
                new SQLiteCommand(@"ALTER TABLE hits ADD COLUMN our_port INTEGER", connection).ExecuteNonQuery();
            }
            catch (Exception) { }
            try
            {
                string alterStm = @"ALTER TABLE project ADD unique_ip int DEFAULT 1";
                SQLiteCommand cmd = new SQLiteCommand(alterStm, connection);
                new SQLiteCommand(alterStm, connection)
                        .ExecuteNonQuery();
            }
            catch { }
        }

        public void FixProjectSchema(Project project)
        {
            try
            {
                new SQLiteCommand(String.Format(@"ALTER TABLE {0} ADD COLUMN downloaded INTEGER", project.TableName), connection).ExecuteNonQuery();
            }
            catch (Exception e) { }

            try
            {
                new SQLiteCommand(String.Format(@"ALTER TABLE {0} ADD COLUMN piece_number INTEGER", project.TableName), connection).ExecuteNonQuery();
            }
            catch (Exception) { }

            try
            {
                new SQLiteCommand(String.Format(@"ALTER TABLE {0} ADD COLUMN piece_hash VARCHAR", project.TableName), connection).ExecuteNonQuery();
            }
            catch (Exception) { }

            try
            {
                new SQLiteCommand(String.Format(@"ALTER TABLE {0} ADD COLUMN our_ip VARCHAR", project.TableName), connection).ExecuteNonQuery();
            }
            catch (Exception) { }

            try
            {
                new SQLiteCommand(String.Format(@"ALTER TABLE {0} ADD COLUMN our_port INTEGER", project.TableName), connection).ExecuteNonQuery();
            }
            catch (Exception) { }

            try
            {
                new SQLiteCommand(String.Format(@"ALTER TABLE {0} ADD COLUMN sig INTEGER", project.TableName), connection).ExecuteNonQuery();
            }
            catch (Exception) { }
        }

        public String Path
        {
            get { return path; }
        }
    }
}
