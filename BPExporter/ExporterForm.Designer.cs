namespace BPExporter
{
    partial class ExporterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExporterForm));
            this.hitsView = new BrightIdeasSoftware.FastObjectListView();
            this.columnIP = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnPort = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnHash = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnCountry = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnCity = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.coulmnISP = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnUrl = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnRegion = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnFileSize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnFile = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnBlock = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.bpIP = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.filesView = new BrightIdeasSoftware.ObjectListView();
            this.columnFilename = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnHashFiles = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnCount = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnSize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.MoveButton = new System.Windows.Forms.Button();
            this.ToExportCount = new System.Windows.Forms.Label();
            this.startDatePicker = new System.Windows.Forms.DateTimePicker();
            this.endDatePicker = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.projectsTree = new BrightIdeasSoftware.TreeListView();
            this.columnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnFiles = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.useDate = new System.Windows.Forms.CheckBox();
            this.ClearExportedBtn = new System.Windows.Forms.Button();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.checkAll = new System.Windows.Forms.CheckBox();
            this.refreshBtn = new System.Windows.Forms.Button();
            this.exportByCountry = new System.Windows.Forms.CheckBox();
            this.hashSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.searchHits = new System.Windows.Forms.TextBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Progress = new System.Windows.Forms.ProgressBar();
            this.statusText = new System.Windows.Forms.Label();
            this.tableGen = new System.Windows.Forms.CheckBox();
            this.DeleteFileBtn = new System.Windows.Forms.Button();
            this.EditBtn = new System.Windows.Forms.Button();
            this.prjName = new System.Windows.Forms.Label();
            this.c_DE = new System.Windows.Forms.CheckBox();
            this.c_US = new System.Windows.Forms.CheckBox();
            this.c_PL = new System.Windows.Forms.CheckBox();
            this.c_RU = new System.Windows.Forms.CheckBox();
            this.c_AU = new System.Windows.Forms.CheckBox();
            this.c_JP = new System.Windows.Forms.CheckBox();
            this.c_edit = new System.Windows.Forms.TextBox();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.activeDB = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.addHitDB = new System.Windows.Forms.Button();
            this.bulkLoadBtn = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.ClearAllBtn = new System.Windows.Forms.Button();
            this.TimeZone = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.revLabel = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.regionCombo = new BPExporter.CheckedComboBox();
            this.ispCombo = new BPExporter.CheckedComboBox();
            this.countryCombo = new BPExporter.CheckedComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.hitsView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filesView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectsTree)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // hitsView
            // 
            this.hitsView.AllColumns.Add(this.columnIP);
            this.hitsView.AllColumns.Add(this.columnPort);
            this.hitsView.AllColumns.Add(this.columnDate);
            this.hitsView.AllColumns.Add(this.columnHash);
            this.hitsView.AllColumns.Add(this.columnCountry);
            this.hitsView.AllColumns.Add(this.columnCity);
            this.hitsView.AllColumns.Add(this.coulmnISP);
            this.hitsView.AllColumns.Add(this.columnUrl);
            this.hitsView.AllColumns.Add(this.columnRegion);
            this.hitsView.AllColumns.Add(this.columnFileSize);
            this.hitsView.AllColumns.Add(this.columnFile);
            this.hitsView.AllColumns.Add(this.columnBlock);
            this.hitsView.AllColumns.Add(this.bpIP);
            this.hitsView.AlternateRowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.hitsView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hitsView.BackColor = System.Drawing.SystemColors.Info;
            this.hitsView.CheckBoxes = false;
            this.hitsView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnIP,
            this.columnPort,
            this.columnDate,
            this.columnHash,
            this.columnCountry,
            this.columnCity,
            this.coulmnISP,
            this.columnUrl,
            this.columnRegion,
            this.columnFileSize,
            this.columnFile,
            this.columnBlock,
            this.bpIP});
            this.hitsView.Cursor = System.Windows.Forms.Cursors.Default;
            this.hitsView.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hitsView.Location = new System.Drawing.Point(16, 481);
            this.hitsView.Name = "hitsView";
            this.hitsView.ShowGroups = false;
            this.hitsView.Size = new System.Drawing.Size(991, 248);
            this.hitsView.TabIndex = 0;
            this.hitsView.UseCompatibleStateImageBehavior = false;
            this.hitsView.UseFiltering = true;
            this.hitsView.UseHotItem = true;
            this.hitsView.UseTranslucentHotItem = true;
            this.hitsView.View = System.Windows.Forms.View.Details;
            this.hitsView.VirtualMode = true;
            this.hitsView.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.hitsView_FormatRow);
            // 
            // columnIP
            // 
            this.columnIP.AspectName = "IP";
            this.columnIP.Groupable = false;
            this.columnIP.Text = "IP";
            this.columnIP.Width = 150;
            // 
            // columnPort
            // 
            this.columnPort.AspectName = "Port";
            this.columnPort.Groupable = false;
            this.columnPort.Text = "Port";
            this.columnPort.Width = 70;
            // 
            // columnDate
            // 
            this.columnDate.AspectName = "FullStrDate";
            this.columnDate.AspectToStringFormat = "";
            this.columnDate.Text = "Date";
            this.columnDate.Width = 150;
            // 
            // columnHash
            // 
            this.columnHash.AspectName = "Hash";
            this.columnHash.Text = "Hash";
            this.columnHash.Width = 270;
            // 
            // columnCountry
            // 
            this.columnCountry.AspectName = "Country";
            this.columnCountry.Text = "Country";
            this.columnCountry.Width = 80;
            // 
            // columnCity
            // 
            this.columnCity.AspectName = "City";
            this.columnCity.Text = "City";
            this.columnCity.Width = 80;
            // 
            // coulmnISP
            // 
            this.coulmnISP.AspectName = "Isp";
            this.coulmnISP.Text = "ISP";
            this.coulmnISP.Width = 100;
            // 
            // columnUrl
            // 
            this.columnUrl.AspectName = "GUID";
            this.columnUrl.Text = "GUID";
            this.columnUrl.Width = 250;
            // 
            // columnRegion
            // 
            this.columnRegion.AspectName = "Region";
            this.columnRegion.Text = "Region/State";
            this.columnRegion.Width = 100;
            // 
            // columnFileSize
            // 
            this.columnFileSize.AspectName = "FormatedSize";
            this.columnFileSize.Text = "Size";
            // 
            // columnFile
            // 
            this.columnFile.AspectName = "Filename";
            this.columnFile.Text = "File";
            // 
            // columnBlock
            // 
            this.columnBlock.AspectName = "PieceHash";
            this.columnBlock.Text = "Piece Hash";
            // 
            // bpIP
            // 
            this.bpIP.AspectName = "BaseprotectIP";
            this.bpIP.Text = "BaseprotectIP";
            // 
            // filesView
            // 
            this.filesView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.filesView.AllColumns.Add(this.columnFilename);
            this.filesView.AllColumns.Add(this.columnHashFiles);
            this.filesView.AllColumns.Add(this.columnCount);
            this.filesView.AllColumns.Add(this.columnSize);
            this.filesView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filesView.BackColor = System.Drawing.SystemColors.Info;
            this.filesView.CheckBoxes = true;
            this.filesView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFilename,
            this.columnHashFiles,
            this.columnCount,
            this.columnSize});
            this.filesView.Cursor = System.Windows.Forms.Cursors.Default;
            this.filesView.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.filesView.FullRowSelect = true;
            this.filesView.Location = new System.Drawing.Point(587, 48);
            this.filesView.Name = "filesView";
            this.filesView.ShowGroups = false;
            this.filesView.Size = new System.Drawing.Size(420, 315);
            this.filesView.SortGroupItemsByPrimaryColumn = false;
            this.filesView.TabIndex = 3;
            this.filesView.UseAlternatingBackColors = true;
            this.filesView.UseCompatibleStateImageBehavior = false;
            this.filesView.UseFiltering = true;
            this.filesView.UseHotItem = true;
            this.filesView.UseTranslucentHotItem = true;
            this.filesView.UseTranslucentSelection = true;
            this.filesView.View = System.Windows.Forms.View.Details;
            // 
            // columnFilename
            // 
            this.columnFilename.AspectName = "Items[0]";
            this.columnFilename.CheckBoxes = true;
            this.columnFilename.Groupable = false;
            this.columnFilename.IsEditable = false;
            this.columnFilename.Text = "Filename";
            this.columnFilename.UseFiltering = false;
            this.columnFilename.Width = 94;
            // 
            // columnHashFiles
            // 
            this.columnHashFiles.AspectName = "Items[1]";
            this.columnHashFiles.AutoCompleteEditor = false;
            this.columnHashFiles.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.columnHashFiles.Groupable = false;
            this.columnHashFiles.IsEditable = false;
            this.columnHashFiles.Text = "Hash";
            this.columnHashFiles.Width = 200;
            // 
            // columnCount
            // 
            this.columnCount.AspectName = "Items[2]";
            this.columnCount.DisplayIndex = 3;
            this.columnCount.IsEditable = false;
            this.columnCount.Searchable = false;
            this.columnCount.Text = "Count";
            this.columnCount.UseFiltering = false;
            // 
            // columnSize
            // 
            this.columnSize.AspectName = "Items[3]";
            this.columnSize.DisplayIndex = 2;
            this.columnSize.Text = "Size";
            // 
            // MoveButton
            // 
            this.MoveButton.Enabled = false;
            this.MoveButton.Image = ((System.Drawing.Image)(resources.GetObject("MoveButton.Image")));
            this.MoveButton.Location = new System.Drawing.Point(534, 256);
            this.MoveButton.Name = "MoveButton";
            this.MoveButton.Size = new System.Drawing.Size(47, 35);
            this.MoveButton.TabIndex = 6;
            this.MoveButton.UseVisualStyleBackColor = true;
            this.MoveButton.Click += new System.EventHandler(this.MoveButton_Click);
            // 
            // ToExportCount
            // 
            this.ToExportCount.AutoSize = true;
            this.ToExportCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ToExportCount.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.ToExportCount.Location = new System.Drawing.Point(508, 372);
            this.ToExportCount.Name = "ToExportCount";
            this.ToExportCount.Size = new System.Drawing.Size(20, 24);
            this.ToExportCount.TabIndex = 14;
            this.ToExportCount.Text = "0";
            // 
            // startDatePicker
            // 
            this.startDatePicker.CalendarMonthBackground = System.Drawing.SystemColors.Info;
            this.startDatePicker.CustomFormat = "yyyy-MM-dd";
            this.startDatePicker.Enabled = false;
            this.startDatePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.startDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startDatePicker.Location = new System.Drawing.Point(114, 374);
            this.startDatePicker.Name = "startDatePicker";
            this.startDatePicker.Size = new System.Drawing.Size(121, 22);
            this.startDatePicker.TabIndex = 15;
            this.startDatePicker.Value = new System.DateTime(2011, 11, 2, 14, 45, 0, 0);
            this.startDatePicker.CloseUp += new System.EventHandler(this.startDatePicker_CloseUp);
            // 
            // endDatePicker
            // 
            this.endDatePicker.CalendarMonthBackground = System.Drawing.SystemColors.Info;
            this.endDatePicker.Enabled = false;
            this.endDatePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.endDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.endDatePicker.Location = new System.Drawing.Point(275, 374);
            this.endDatePicker.Name = "endDatePicker";
            this.endDatePicker.Size = new System.Drawing.Size(118, 22);
            this.endDatePicker.TabIndex = 16;
            this.endDatePicker.Value = new System.DateTime(2014, 11, 2, 14, 45, 0, 0);
            this.endDatePicker.CloseUp += new System.EventHandler(this.endDatePicker_CloseUp);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label8.Location = new System.Drawing.Point(239, 374);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 18);
            this.label8.TabIndex = 17;
            this.label8.Text = "to:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label9.Location = new System.Drawing.Point(13, 376);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 18);
            this.label9.TabIndex = 18;
            this.label9.Text = "Date from:";
            // 
            // projectsTree
            // 
            this.projectsTree.AllColumns.Add(this.columnName);
            this.projectsTree.AllColumns.Add(this.columnFiles);
            this.projectsTree.BackColor = System.Drawing.SystemColors.Info;
            this.projectsTree.CheckBoxes = false;
            this.projectsTree.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnFiles});
            this.projectsTree.Cursor = System.Windows.Forms.Cursors.Default;
            this.projectsTree.Font = new System.Drawing.Font("Courier New", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.projectsTree.FullRowSelect = true;
            this.projectsTree.GridLines = true;
            this.projectsTree.Location = new System.Drawing.Point(14, 48);
            this.projectsTree.MultiSelect = false;
            this.projectsTree.Name = "projectsTree";
            this.projectsTree.OwnerDraw = true;
            this.projectsTree.ShowGroups = false;
            this.projectsTree.ShowItemToolTips = true;
            this.projectsTree.Size = new System.Drawing.Size(514, 243);
            this.projectsTree.TabIndex = 20;
            this.projectsTree.UseAlternatingBackColors = true;
            this.projectsTree.UseCompatibleStateImageBehavior = false;
            this.projectsTree.UseFiltering = true;
            this.projectsTree.UseHotItem = true;
            this.projectsTree.UseTranslucentHotItem = true;
            this.projectsTree.View = System.Windows.Forms.View.Details;
            this.projectsTree.VirtualMode = true;
            this.projectsTree.FormatCell += new System.EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(this.projectsTree_FormatCell);
            this.projectsTree.ItemActivate += new System.EventHandler(this.projectsTree_ItemActivate);
            // 
            // columnName
            // 
            this.columnName.AspectName = "Name";
            this.columnName.Text = "Project Name";
            this.columnName.Width = 350;
            // 
            // columnFiles
            // 
            this.columnFiles.AspectName = "AKZ";
            this.columnFiles.Text = "AKZ";
            this.columnFiles.Width = 75;
            // 
            // useDate
            // 
            this.useDate.AutoSize = true;
            this.useDate.Location = new System.Drawing.Point(399, 380);
            this.useDate.Name = "useDate";
            this.useDate.Size = new System.Drawing.Size(15, 14);
            this.useDate.TabIndex = 21;
            this.useDate.UseVisualStyleBackColor = true;
            this.useDate.CheckedChanged += new System.EventHandler(this.useDate_CheckedChanged);
            // 
            // ClearExportedBtn
            // 
            this.ClearExportedBtn.Enabled = false;
            this.ClearExportedBtn.Image = ((System.Drawing.Image)(resources.GetObject("ClearExportedBtn.Image")));
            this.ClearExportedBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ClearExportedBtn.Location = new System.Drawing.Point(74, 333);
            this.ClearExportedBtn.Name = "ClearExportedBtn";
            this.ClearExportedBtn.Size = new System.Drawing.Size(111, 34);
            this.ClearExportedBtn.TabIndex = 24;
            this.ClearExportedBtn.Text = "Clear Exported";
            this.ClearExportedBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ClearExportedBtn.UseVisualStyleBackColor = true;
            this.ClearExportedBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Enabled = false;
            this.DeleteBtn.Image = ((System.Drawing.Image)(resources.GetObject("DeleteBtn.Image")));
            this.DeleteBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.DeleteBtn.Location = new System.Drawing.Point(284, 333);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(78, 34);
            this.DeleteBtn.TabIndex = 25;
            this.DeleteBtn.Text = "Delete";
            this.DeleteBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(828, 405);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(132, 37);
            this.button3.TabIndex = 27;
            this.button3.Text = "Export";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // checkAll
            // 
            this.checkAll.AutoSize = true;
            this.checkAll.Enabled = false;
            this.checkAll.Location = new System.Drawing.Point(587, 369);
            this.checkAll.Name = "checkAll";
            this.checkAll.Size = new System.Drawing.Size(76, 19);
            this.checkAll.TabIndex = 28;
            this.checkAll.Text = "Select all";
            this.checkAll.UseVisualStyleBackColor = true;
            this.checkAll.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // refreshBtn
            // 
            this.refreshBtn.Image = ((System.Drawing.Image)(resources.GetObject("refreshBtn.Image")));
            this.refreshBtn.Location = new System.Drawing.Point(966, 403);
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(40, 37);
            this.refreshBtn.TabIndex = 29;
            this.refreshBtn.UseVisualStyleBackColor = true;
            this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // exportByCountry
            // 
            this.exportByCountry.AutoSize = true;
            this.exportByCountry.Location = new System.Drawing.Point(672, 403);
            this.exportByCountry.Name = "exportByCountry";
            this.exportByCountry.Size = new System.Drawing.Size(121, 19);
            this.exportByCountry.TabIndex = 30;
            this.exportByCountry.Text = "Export By Country";
            this.exportByCountry.UseVisualStyleBackColor = true;
            // 
            // hashSearch
            // 
            this.hashSearch.BackColor = System.Drawing.SystemColors.Info;
            this.hashSearch.Location = new System.Drawing.Point(750, 373);
            this.hashSearch.Name = "hashSearch";
            this.hashSearch.Size = new System.Drawing.Size(256, 21);
            this.hashSearch.TabIndex = 32;
            this.hashSearch.TextChanged += new System.EventHandler(this.hashSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(685, 373);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 15);
            this.label1.TabIndex = 33;
            this.label1.Text = "Search:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(285, 413);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 15);
            this.label2.TabIndex = 34;
            this.label2.Text = "Search:";
            // 
            // searchHits
            // 
            this.searchHits.BackColor = System.Drawing.SystemColors.Info;
            this.searchHits.Location = new System.Drawing.Point(350, 410);
            this.searchHits.Name = "searchHits";
            this.searchHits.Size = new System.Drawing.Size(316, 21);
            this.searchHits.TabIndex = 35;
            this.searchHits.TextChanged += new System.EventHandler(this.searchHits_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 453);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 15);
            this.label3.TabIndex = 37;
            this.label3.Text = "Country:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(236, 453);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 15);
            this.label4.TabIndex = 39;
            this.label4.Text = "ISP:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(669, 453);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 15);
            this.label5.TabIndex = 40;
            this.label5.Text = "State:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.Progress);
            this.panel1.Controls.Add(this.statusText);
            this.panel1.Location = new System.Drawing.Point(16, 731);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1345, 29);
            this.panel1.TabIndex = 42;
            // 
            // Progress
            // 
            this.Progress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Progress.Location = new System.Drawing.Point(346, 3);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(644, 23);
            this.Progress.Step = 1;
            this.Progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.Progress.TabIndex = 1;
            // 
            // statusText
            // 
            this.statusText.AutoSize = true;
            this.statusText.Location = new System.Drawing.Point(3, 5);
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(0, 15);
            this.statusText.TabIndex = 0;
            // 
            // tableGen
            // 
            this.tableGen.AutoSize = true;
            this.tableGen.Checked = true;
            this.tableGen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableGen.Location = new System.Drawing.Point(672, 422);
            this.tableGen.Name = "tableGen";
            this.tableGen.Size = new System.Drawing.Size(111, 19);
            this.tableGen.TabIndex = 48;
            this.tableGen.Text = "Generate Table";
            this.tableGen.UseVisualStyleBackColor = true;
            // 
            // DeleteFileBtn
            // 
            this.DeleteFileBtn.Enabled = false;
            this.DeleteFileBtn.Image = ((System.Drawing.Image)(resources.GetObject("DeleteFileBtn.Image")));
            this.DeleteFileBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.DeleteFileBtn.Location = new System.Drawing.Point(368, 333);
            this.DeleteFileBtn.Name = "DeleteFileBtn";
            this.DeleteFileBtn.Size = new System.Drawing.Size(100, 33);
            this.DeleteFileBtn.TabIndex = 49;
            this.DeleteFileBtn.Text = "Delete file";
            this.DeleteFileBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DeleteFileBtn.UseVisualStyleBackColor = true;
            this.DeleteFileBtn.Click += new System.EventHandler(this.button4_Click);
            // 
            // EditBtn
            // 
            this.EditBtn.Enabled = false;
            this.EditBtn.Image = ((System.Drawing.Image)(resources.GetObject("EditBtn.Image")));
            this.EditBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.EditBtn.Location = new System.Drawing.Point(13, 333);
            this.EditBtn.Name = "EditBtn";
            this.EditBtn.Size = new System.Drawing.Size(55, 34);
            this.EditBtn.TabIndex = 50;
            this.EditBtn.Text = "Edit";
            this.EditBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.EditBtn.UseVisualStyleBackColor = true;
            this.EditBtn.Click += new System.EventHandler(this.button5_Click);
            // 
            // prjName
            // 
            this.prjName.AutoSize = true;
            this.prjName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.prjName.ForeColor = System.Drawing.Color.DarkRed;
            this.prjName.Location = new System.Drawing.Point(124, 300);
            this.prjName.Name = "prjName";
            this.prjName.Size = new System.Drawing.Size(0, 20);
            this.prjName.TabIndex = 51;
            // 
            // c_DE
            // 
            this.c_DE.AutoSize = true;
            this.c_DE.Checked = true;
            this.c_DE.CheckState = System.Windows.Forms.CheckState.Checked;
            this.c_DE.Location = new System.Drawing.Point(534, 48);
            this.c_DE.Name = "c_DE";
            this.c_DE.Size = new System.Drawing.Size(43, 19);
            this.c_DE.TabIndex = 52;
            this.c_DE.Text = "DE";
            this.c_DE.UseVisualStyleBackColor = true;
            // 
            // c_US
            // 
            this.c_US.AutoSize = true;
            this.c_US.Location = new System.Drawing.Point(534, 73);
            this.c_US.Name = "c_US";
            this.c_US.Size = new System.Drawing.Size(43, 19);
            this.c_US.TabIndex = 53;
            this.c_US.Text = "US";
            this.c_US.UseVisualStyleBackColor = true;
            // 
            // c_PL
            // 
            this.c_PL.AutoSize = true;
            this.c_PL.Location = new System.Drawing.Point(534, 98);
            this.c_PL.Name = "c_PL";
            this.c_PL.Size = new System.Drawing.Size(41, 19);
            this.c_PL.TabIndex = 54;
            this.c_PL.Text = "PL";
            this.c_PL.UseVisualStyleBackColor = true;
            // 
            // c_RU
            // 
            this.c_RU.AutoSize = true;
            this.c_RU.Location = new System.Drawing.Point(534, 123);
            this.c_RU.Name = "c_RU";
            this.c_RU.Size = new System.Drawing.Size(44, 19);
            this.c_RU.TabIndex = 55;
            this.c_RU.Text = "RU";
            this.c_RU.UseVisualStyleBackColor = true;
            // 
            // c_AU
            // 
            this.c_AU.AutoSize = true;
            this.c_AU.Location = new System.Drawing.Point(534, 148);
            this.c_AU.Name = "c_AU";
            this.c_AU.Size = new System.Drawing.Size(42, 19);
            this.c_AU.TabIndex = 56;
            this.c_AU.Text = "AU";
            this.c_AU.UseVisualStyleBackColor = true;
            // 
            // c_JP
            // 
            this.c_JP.AutoSize = true;
            this.c_JP.Location = new System.Drawing.Point(534, 173);
            this.c_JP.Name = "c_JP";
            this.c_JP.Size = new System.Drawing.Size(40, 19);
            this.c_JP.TabIndex = 57;
            this.c_JP.Text = "JP";
            this.c_JP.UseVisualStyleBackColor = true;
            // 
            // c_edit
            // 
            this.c_edit.Location = new System.Drawing.Point(534, 198);
            this.c_edit.Name = "c_edit";
            this.c_edit.Size = new System.Drawing.Size(40, 21);
            this.c_edit.TabIndex = 58;
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(150, 175);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(201, 5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 15);
            this.label7.TabIndex = 43;
            this.label7.Text = "Active database:";
            // 
            // activeDB
            // 
            this.activeDB.AutoSize = true;
            this.activeDB.Location = new System.Drawing.Point(302, 5);
            this.activeDB.Name = "activeDB";
            this.activeDB.Size = new System.Drawing.Size(0, 15);
            this.activeDB.TabIndex = 44;
            this.activeDB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button6
            // 
            this.button6.Image = ((System.Drawing.Image)(resources.GetObject("button6.Image")));
            this.button6.Location = new System.Drawing.Point(5, 5);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(43, 37);
            this.button6.TabIndex = 59;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // addHitDB
            // 
            this.addHitDB.Image = ((System.Drawing.Image)(resources.GetObject("addHitDB.Image")));
            this.addHitDB.Location = new System.Drawing.Point(55, 5);
            this.addHitDB.Name = "addHitDB";
            this.addHitDB.Size = new System.Drawing.Size(47, 37);
            this.addHitDB.TabIndex = 60;
            this.addHitDB.UseVisualStyleBackColor = true;
            this.addHitDB.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // bulkLoadBtn
            // 
            this.bulkLoadBtn.Image = ((System.Drawing.Image)(resources.GetObject("bulkLoadBtn.Image")));
            this.bulkLoadBtn.Location = new System.Drawing.Point(108, 5);
            this.bulkLoadBtn.Name = "bulkLoadBtn";
            this.bulkLoadBtn.Size = new System.Drawing.Size(47, 37);
            this.bulkLoadBtn.TabIndex = 61;
            this.bulkLoadBtn.UseVisualStyleBackColor = true;
            this.bulkLoadBtn.Click += new System.EventHandler(this.bulkLoadBtn_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label10.Location = new System.Drawing.Point(10, 300);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(108, 20);
            this.label10.TabIndex = 62;
            this.label10.Text = "Active project:";
            // 
            // ClearAllBtn
            // 
            this.ClearAllBtn.Enabled = false;
            this.ClearAllBtn.Image = ((System.Drawing.Image)(resources.GetObject("ClearAllBtn.Image")));
            this.ClearAllBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ClearAllBtn.Location = new System.Drawing.Point(191, 333);
            this.ClearAllBtn.Name = "ClearAllBtn";
            this.ClearAllBtn.Size = new System.Drawing.Size(87, 34);
            this.ClearAllBtn.TabIndex = 63;
            this.ClearAllBtn.Text = "Clear All";
            this.ClearAllBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ClearAllBtn.UseVisualStyleBackColor = true;
            this.ClearAllBtn.Click += new System.EventHandler(this.ClearAllBtn_Click);
            // 
            // TimeZone
            // 
            this.TimeZone.BackColor = System.Drawing.SystemColors.Info;
            this.TimeZone.FormattingEnabled = true;
            this.TimeZone.Items.AddRange(new object[] {
            "UTC",
            "Central Europe Standard Time"});
            this.TimeZone.Location = new System.Drawing.Point(74, 408);
            this.TimeZone.Name = "TimeZone";
            this.TimeZone.Size = new System.Drawing.Size(205, 23);
            this.TimeZone.TabIndex = 67;
            this.TimeZone.Text = "UTC";
            this.TimeZone.SelectedIndexChanged += new System.EventHandler(this.TimeZone_SelectedIndexChanged_1);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label11.Location = new System.Drawing.Point(420, 376);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 18);
            this.label11.TabIndex = 13;
            this.label11.Text = "Hits:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 413);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 15);
            this.label6.TabIndex = 68;
            this.label6.Text = "TZ:";
            // 
            // revLabel
            // 
            this.revLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.revLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.revLabel.Location = new System.Drawing.Point(301, 23);
            this.revLabel.Name = "revLabel";
            this.revLabel.Size = new System.Drawing.Size(227, 21);
            this.revLabel.TabIndex = 69;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(201, 26);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 15);
            this.label12.TabIndex = 70;
            this.label12.Text = "Version:";
            // 
            // regionCombo
            // 
            this.regionCombo.BackColor = System.Drawing.SystemColors.Info;
            this.regionCombo.CheckOnClick = true;
            this.regionCombo.DisplayMember = "Name";
            this.regionCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.regionCombo.DropDownHeight = 1;
            this.regionCombo.FormattingEnabled = true;
            this.regionCombo.IntegralHeight = false;
            this.regionCombo.Location = new System.Drawing.Point(714, 450);
            this.regionCombo.Name = "regionCombo";
            this.regionCombo.Size = new System.Drawing.Size(292, 22);
            this.regionCombo.TabIndex = 66;
            this.regionCombo.ValueSeparator = ", ";
            this.regionCombo.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.regionCombo_ItemCheck);
            this.regionCombo.SelectedIndexChanged += new System.EventHandler(this.regionCombo_SelectedIndexChanged);
            this.regionCombo.DropDownClosed += new System.EventHandler(this.ispCombo_DropDownClosed);
            // 
            // ispCombo
            // 
            this.ispCombo.BackColor = System.Drawing.SystemColors.Info;
            this.ispCombo.CheckOnClick = true;
            this.ispCombo.DisplayMember = "Name";
            this.ispCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.ispCombo.DropDownHeight = 1;
            this.ispCombo.FormattingEnabled = true;
            this.ispCombo.IntegralHeight = false;
            this.ispCombo.Location = new System.Drawing.Point(271, 450);
            this.ispCombo.Name = "ispCombo";
            this.ispCombo.Size = new System.Drawing.Size(392, 22);
            this.ispCombo.TabIndex = 65;
            this.ispCombo.ValueSeparator = ", ";
            this.ispCombo.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ispCombo_ItemCheck);
            this.ispCombo.SelectedIndexChanged += new System.EventHandler(this.ispCombo_SelectedIndexChanged);
            this.ispCombo.DropDownClosed += new System.EventHandler(this.ispCombo_DropDownClosed);
            // 
            // countryCombo
            // 
            this.countryCombo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.countryCombo.BackColor = System.Drawing.SystemColors.Info;
            this.countryCombo.CheckOnClick = false;
            this.countryCombo.DisplayMember = "Name";
            this.countryCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.countryCombo.DropDownHeight = 1;
            this.countryCombo.FormattingEnabled = true;
            this.countryCombo.IntegralHeight = false;
            this.countryCombo.Location = new System.Drawing.Point(74, 450);
            this.countryCombo.Name = "countryCombo";
            this.countryCombo.Size = new System.Drawing.Size(156, 22);
            this.countryCombo.TabIndex = 64;
            this.countryCombo.ValueSeparator = ", ";
            this.countryCombo.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.countryCombo_ItemCheck);
            this.countryCombo.DropDownClosed += new System.EventHandler(this.countryCombo_DropDownClosed);
            // 
            // ExporterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 759);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.revLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TimeZone);
            this.Controls.Add(this.regionCombo);
            this.Controls.Add(this.ispCombo);
            this.Controls.Add(this.countryCombo);
            this.Controls.Add(this.ClearAllBtn);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.bulkLoadBtn);
            this.Controls.Add(this.addHitDB);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.c_edit);
            this.Controls.Add(this.c_JP);
            this.Controls.Add(this.c_AU);
            this.Controls.Add(this.c_RU);
            this.Controls.Add(this.c_PL);
            this.Controls.Add(this.c_US);
            this.Controls.Add(this.c_DE);
            this.Controls.Add(this.prjName);
            this.Controls.Add(this.EditBtn);
            this.Controls.Add(this.DeleteFileBtn);
            this.Controls.Add(this.tableGen);
            this.Controls.Add(this.activeDB);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.searchHits);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hashSearch);
            this.Controls.Add(this.exportByCountry);
            this.Controls.Add(this.refreshBtn);
            this.Controls.Add(this.checkAll);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.DeleteBtn);
            this.Controls.Add(this.ClearExportedBtn);
            this.Controls.Add(this.useDate);
            this.Controls.Add(this.projectsTree);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.endDatePicker);
            this.Controls.Add(this.startDatePicker);
            this.Controls.Add(this.ToExportCount);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.MoveButton);
            this.Controls.Add(this.filesView);
            this.Controls.Add(this.hitsView);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExporterForm";
            this.Text = "BPExporter by PROSOFT";
            this.Load += new System.EventHandler(this.ExporterForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.hitsView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filesView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectsTree)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView filesView;
        private System.Windows.Forms.Button MoveButton;
        private System.Windows.Forms.Label ToExportCount;
        private System.Windows.Forms.DateTimePicker startDatePicker;
        private System.Windows.Forms.DateTimePicker endDatePicker;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private BrightIdeasSoftware.TreeListView projectsTree;
        private BrightIdeasSoftware.OLVColumn columnName;
        private BrightIdeasSoftware.OLVColumn columnFilename;
        private BrightIdeasSoftware.OLVColumn columnCount;
        private BrightIdeasSoftware.OLVColumn columnIP;
        private BrightIdeasSoftware.OLVColumn columnPort;
        private BrightIdeasSoftware.OLVColumn columnDate;
        private BrightIdeasSoftware.OLVColumn columnHash;
        private BrightIdeasSoftware.OLVColumn columnCountry;
        private BrightIdeasSoftware.OLVColumn columnCity;
        private BrightIdeasSoftware.OLVColumn coulmnISP;
        private BrightIdeasSoftware.OLVColumn columnUrl;
        private System.Windows.Forms.CheckBox useDate;
        private BrightIdeasSoftware.OLVColumn columnFiles;
        private System.Windows.Forms.Button ClearExportedBtn;
        private System.Windows.Forms.Button DeleteBtn;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkAll;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.CheckBox exportByCountry;
        private BrightIdeasSoftware.OLVColumn columnRegion;
        private BrightIdeasSoftware.OLVColumn columnHashFiles;
        private System.Windows.Forms.TextBox hashSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox searchHits;
        private System.IO.Ports.SerialPort serialPort1;
        private BrightIdeasSoftware.FastObjectListView hitsView;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar Progress;
        private System.Windows.Forms.Label statusText;
        private System.Windows.Forms.CheckBox tableGen;
        private System.Windows.Forms.Button DeleteFileBtn;
        private System.Windows.Forms.Button EditBtn;
        private System.Windows.Forms.Label prjName;
        private System.Windows.Forms.CheckBox c_DE;
        private System.Windows.Forms.CheckBox c_US;
        private System.Windows.Forms.CheckBox c_PL;
        private System.Windows.Forms.CheckBox c_RU;
        private System.Windows.Forms.CheckBox c_AU;
        private System.Windows.Forms.CheckBox c_JP;
        private System.Windows.Forms.TextBox c_edit;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label activeDB;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button addHitDB;
        private BrightIdeasSoftware.OLVColumn columnSize;
        private BrightIdeasSoftware.OLVColumn columnFileSize;
        private BrightIdeasSoftware.OLVColumn columnFile;
        private System.Windows.Forms.Button bulkLoadBtn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Button ClearAllBtn;
        private CheckedComboBox countryCombo;
        private CheckedComboBox ispCombo;
        private CheckedComboBox regionCombo;
        private BrightIdeasSoftware.OLVColumn columnBlock;
        private BrightIdeasSoftware.OLVColumn bpIP;
        private System.Windows.Forms.ComboBox TimeZone;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox revLabel;
        private System.Windows.Forms.Label label12;
    }
}

