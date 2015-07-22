namespace theDirector
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fILEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewAutomationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenAutomationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.runToolStripButtonReset = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.rtf = new System.Windows.Forms.RichTextBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tvw = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.propGrid = new System.Windows.Forms.PropertyGrid();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tvwConditions = new System.Windows.Forms.TreeView();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.alwaysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.screenConditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataConditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataSourceIsEOFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataSourceIsNOTEOFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.variableValueConditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.messageboxToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.datasourcesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDatasourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enterDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fILEToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(896, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fILEToolStripMenuItem
            // 
            this.fILEToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewAutomationToolStripMenuItem,
            this.OpenAutomationToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.fILEToolStripMenuItem.Name = "fILEToolStripMenuItem";
            this.fILEToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
            this.fILEToolStripMenuItem.Text = "FILE";
            // 
            // NewAutomationToolStripMenuItem
            // 
            this.NewAutomationToolStripMenuItem.Name = "NewAutomationToolStripMenuItem";
            this.NewAutomationToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
            this.NewAutomationToolStripMenuItem.Text = "NEW AUTOMATION";
            this.NewAutomationToolStripMenuItem.Click += new System.EventHandler(this.NewAutomationToolStripMenuItem_Click_1);
            // 
            // OpenAutomationToolStripMenuItem
            // 
            this.OpenAutomationToolStripMenuItem.Name = "OpenAutomationToolStripMenuItem";
            this.OpenAutomationToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
            this.OpenAutomationToolStripMenuItem.Text = "OPEN AUTOMATION";
            this.OpenAutomationToolStripMenuItem.Click += new System.EventHandler(this.OpenAutomationToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
            this.ExitToolStripMenuItem.Text = "EXIT";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.toolStripSeparator1,
            this.helpToolStripButton,
            this.toolStripSeparator2,
            this.runToolStripButtonReset});
            this.toolStrip1.Location = new System.Drawing.Point(0, 28);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(896, 27);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.newToolStripButton.Text = "&New";
            this.newToolStripButton.Click += new System.EventHandler(this.newToolStripButton_Click);
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.openToolStripButton.Text = "&Open";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // helpToolStripButton
            // 
            this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripButton.Image")));
            this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpToolStripButton.Name = "helpToolStripButton";
            this.helpToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.helpToolStripButton.Text = "He&lp";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // runToolStripButtonReset
            // 
            this.runToolStripButtonReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.runToolStripButtonReset.Image = ((System.Drawing.Image)(resources.GetObject("runToolStripButtonReset.Image")));
            this.runToolStripButtonReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.runToolStripButtonReset.Name = "runToolStripButtonReset";
            this.runToolStripButtonReset.Size = new System.Drawing.Size(38, 24);
            this.runToolStripButtonReset.Text = "Run";
            this.runToolStripButtonReset.Click += new System.EventHandler(this.runToolStripButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 55);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel2.Controls.Add(this.statusStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(896, 474);
            this.splitContainer1.SplitterDistance = 297;
            this.splitContainer1.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(896, 297);
            this.splitContainer2.SplitterDistance = 434;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(432, 295);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.rtf);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage3.Size = new System.Drawing.Size(424, 269);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Console";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // rtf
            // 
            this.rtf.BackColor = System.Drawing.Color.Black;
            this.rtf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtf.ForeColor = System.Drawing.Color.White;
            this.rtf.Location = new System.Drawing.Point(3, 3);
            this.rtf.Name = "rtf";
            this.rtf.Size = new System.Drawing.Size(418, 263);
            this.rtf.TabIndex = 1;
            this.rtf.Text = "";
            this.rtf.TextChanged += new System.EventHandler(this.rtf_TextChanged);
            // 
            // splitContainer3
            // 
            this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tvw);
            this.splitContainer3.Panel1.Controls.Add(this.toolStrip2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.propGrid);
            this.splitContainer3.Size = new System.Drawing.Size(458, 297);
            this.splitContainer3.SplitterDistance = 153;
            this.splitContainer3.TabIndex = 0;
            // 
            // tvw
            // 
            this.tvw.AllowDrop = true;
            this.tvw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvw.ImageIndex = 0;
            this.tvw.ImageList = this.imageList1;
            this.tvw.Location = new System.Drawing.Point(0, 27);
            this.tvw.Name = "tvw";
            this.tvw.SelectedImageIndex = 0;
            this.tvw.Size = new System.Drawing.Size(456, 124);
            this.tvw.TabIndex = 1;
            this.tvw.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvw_ItemDrag);
            this.tvw.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvw_AfterSelect);
            this.tvw.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvw_NodeMouseClick);
            this.tvw.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvw_DragDrop);
            this.tvw.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvw_DragEnter);
            this.tvw.DragOver += new System.Windows.Forms.DragEventHandler(this.tvw_DragOver);
            this.tvw.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvw_KeyDown);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "CSharpProject_SolutionExplorerNode.png");
            this.imageList1.Images.SetKeyName(1, "Solution_8308.png");
            this.imageList1.Images.SetKeyName(2, "reference_16xLG.png");
            this.imageList1.Images.SetKeyName(3, "folder_Closed_16xSM.png");
            // 
            // toolStrip2
            // 
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton1});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(456, 27);
            this.toolStrip2.TabIndex = 0;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // newToolStripButton1
            // 
            this.newToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton1.Image")));
            this.newToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton1.Name = "newToolStripButton1";
            this.newToolStripButton1.Size = new System.Drawing.Size(24, 24);
            this.newToolStripButton1.Text = "&New Step";
            this.newToolStripButton1.ToolTipText = "New Scene";
            this.newToolStripButton1.Click += new System.EventHandler(this.newToolStripButton1_Click);
            // 
            // propGrid
            // 
            this.propGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propGrid.Location = new System.Drawing.Point(0, 0);
            this.propGrid.Name = "propGrid";
            this.propGrid.Size = new System.Drawing.Size(456, 138);
            this.propGrid.TabIndex = 0;
            this.propGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propGrid_PropertyValueChanged);
            this.propGrid.Click += new System.EventHandler(this.propGrid_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(894, 149);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tvwConditions);
            this.tabPage1.Controls.Add(this.toolStrip3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(886, 123);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Conditions";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tvwConditions
            // 
            this.tvwConditions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwConditions.Location = new System.Drawing.Point(3, 30);
            this.tvwConditions.Name = "tvwConditions";
            this.tvwConditions.Size = new System.Drawing.Size(880, 90);
            this.tvwConditions.TabIndex = 1;
            this.tvwConditions.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwConditions_AfterSelect);
            // 
            // toolStrip3
            // 
            this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton2,
            this.toolStripDropDownButton1});
            this.toolStrip3.Location = new System.Drawing.Point(3, 3);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(880, 27);
            this.toolStrip3.TabIndex = 0;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // newToolStripButton2
            // 
            this.newToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alwaysToolStripMenuItem,
            this.screenConditionToolStripMenuItem,
            this.dataConditionToolStripMenuItem,
            this.variableValueConditionToolStripMenuItem});
            this.newToolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton2.Image")));
            this.newToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton2.Name = "newToolStripButton2";
            this.newToolStripButton2.Size = new System.Drawing.Size(34, 24);
            this.newToolStripButton2.Text = "&New";
            // 
            // alwaysToolStripMenuItem
            // 
            this.alwaysToolStripMenuItem.Name = "alwaysToolStripMenuItem";
            this.alwaysToolStripMenuItem.Size = new System.Drawing.Size(243, 24);
            this.alwaysToolStripMenuItem.Text = "Always";
            this.alwaysToolStripMenuItem.Click += new System.EventHandler(this.alwaysToolStripMenuItem_Click);
            // 
            // screenConditionToolStripMenuItem
            // 
            this.screenConditionToolStripMenuItem.Name = "screenConditionToolStripMenuItem";
            this.screenConditionToolStripMenuItem.Size = new System.Drawing.Size(243, 24);
            this.screenConditionToolStripMenuItem.Text = "Screen Condition";
            this.screenConditionToolStripMenuItem.Click += new System.EventHandler(this.screenConditionToolStripMenuItem_Click);
            // 
            // dataConditionToolStripMenuItem
            // 
            this.dataConditionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataSourceIsEOFToolStripMenuItem,
            this.dataSourceIsNOTEOFToolStripMenuItem});
            this.dataConditionToolStripMenuItem.Name = "dataConditionToolStripMenuItem";
            this.dataConditionToolStripMenuItem.Size = new System.Drawing.Size(243, 24);
            this.dataConditionToolStripMenuItem.Text = "Data Condition";
            // 
            // dataSourceIsEOFToolStripMenuItem
            // 
            this.dataSourceIsEOFToolStripMenuItem.Name = "dataSourceIsEOFToolStripMenuItem";
            this.dataSourceIsEOFToolStripMenuItem.Size = new System.Drawing.Size(237, 24);
            this.dataSourceIsEOFToolStripMenuItem.Text = "Data Source is EOF";
            this.dataSourceIsEOFToolStripMenuItem.Click += new System.EventHandler(this.dataSourceIsEOFToolStripMenuItem_Click);
            // 
            // dataSourceIsNOTEOFToolStripMenuItem
            // 
            this.dataSourceIsNOTEOFToolStripMenuItem.Name = "dataSourceIsNOTEOFToolStripMenuItem";
            this.dataSourceIsNOTEOFToolStripMenuItem.Size = new System.Drawing.Size(237, 24);
            this.dataSourceIsNOTEOFToolStripMenuItem.Text = "Data Source is NOT EOF";
            this.dataSourceIsNOTEOFToolStripMenuItem.Click += new System.EventHandler(this.dataSourceIsNOTEOFToolStripMenuItem_Click);
            // 
            // variableValueConditionToolStripMenuItem
            // 
            this.variableValueConditionToolStripMenuItem.Name = "variableValueConditionToolStripMenuItem";
            this.variableValueConditionToolStripMenuItem.Size = new System.Drawing.Size(243, 24);
            this.variableValueConditionToolStripMenuItem.Text = "Variable Value Condition";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.datasourcesToolStripMenuItem,
            this.connectionToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(34, 24);
            this.toolStripDropDownButton1.Text = "&New";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.messageboxToolStripMenuItem1});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(175, 24);
            this.toolStripMenuItem1.Text = "Notifications";
            // 
            // messageboxToolStripMenuItem1
            // 
            this.messageboxToolStripMenuItem1.Name = "messageboxToolStripMenuItem1";
            this.messageboxToolStripMenuItem1.Size = new System.Drawing.Size(161, 24);
            this.messageboxToolStripMenuItem1.Text = "Messagebox";
            this.messageboxToolStripMenuItem1.Click += new System.EventHandler(this.messageboxToolStripMenuItem1_Click);
            // 
            // datasourcesToolStripMenuItem
            // 
            this.datasourcesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDatasourceToolStripMenuItem,
            this.nextRecordToolStripMenuItem});
            this.datasourcesToolStripMenuItem.Name = "datasourcesToolStripMenuItem";
            this.datasourcesToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.datasourcesToolStripMenuItem.Text = "Datasources";
            // 
            // openDatasourceToolStripMenuItem
            // 
            this.openDatasourceToolStripMenuItem.Name = "openDatasourceToolStripMenuItem";
            this.openDatasourceToolStripMenuItem.Size = new System.Drawing.Size(193, 24);
            this.openDatasourceToolStripMenuItem.Text = "Open Datasource";
            this.openDatasourceToolStripMenuItem.Click += new System.EventHandler(this.openDatasourceToolStripMenuItem_Click);
            // 
            // nextRecordToolStripMenuItem
            // 
            this.nextRecordToolStripMenuItem.Name = "nextRecordToolStripMenuItem";
            this.nextRecordToolStripMenuItem.Size = new System.Drawing.Size(193, 24);
            this.nextRecordToolStripMenuItem.Text = "Next Record";
            this.nextRecordToolStripMenuItem.Click += new System.EventHandler(this.nextRecordToolStripMenuItem_Click);
            // 
            // connectionToolStripMenuItem
            // 
            this.connectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openConnectionToolStripMenuItem,
            this.enterDataToolStripMenuItem});
            this.connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
            this.connectionToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.connectionToolStripMenuItem.Text = "Connection";
            this.connectionToolStripMenuItem.Click += new System.EventHandler(this.connectionToolStripMenuItem_Click);
            // 
            // openConnectionToolStripMenuItem
            // 
            this.openConnectionToolStripMenuItem.Name = "openConnectionToolStripMenuItem";
            this.openConnectionToolStripMenuItem.Size = new System.Drawing.Size(193, 24);
            this.openConnectionToolStripMenuItem.Text = "Open Connection";
            this.openConnectionToolStripMenuItem.Click += new System.EventHandler(this.openConnectionToolStripMenuItem_Click);
            // 
            // enterDataToolStripMenuItem
            // 
            this.enterDataToolStripMenuItem.Name = "enterDataToolStripMenuItem";
            this.enterDataToolStripMenuItem.Size = new System.Drawing.Size(193, 24);
            this.enterDataToolStripMenuItem.Text = "Enter Data";
            this.enterDataToolStripMenuItem.Click += new System.EventHandler(this.enterDataToolStripMenuItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(887, 132);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Output";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 149);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(894, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 529);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "theDirector";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fILEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewAutomationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenAutomationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton helpToolStripButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TreeView tvw;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton newToolStripButton1;
        private System.Windows.Forms.PropertyGrid propGrid;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TreeView tvwConditions;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripDropDownButton newToolStripButton2;
        private System.Windows.Forms.ToolStripMenuItem alwaysToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem screenConditionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataConditionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem variableValueConditionToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton runToolStripButtonReset;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem messageboxToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem datasourcesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDatasourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextRecordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataSourceIsEOFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataSourceIsNOTEOFToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox rtf;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem connectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openConnectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enterDataToolStripMenuItem;

    }
}

