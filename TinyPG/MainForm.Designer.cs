﻿namespace TinyPG
{
    partial class MainForm
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
                checker.Dispose();
                marker.Dispose();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.regexToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.outputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.parsetreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.expressionEvaluatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.parseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuToolsGenerate = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.viewParserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewScannerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewParseTreeCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutTinyParserGeneratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.examplesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusLine = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusCol = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusPos = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
			this.splitterBottom = new System.Windows.Forms.Splitter();
			this.splitterRight = new System.Windows.Forms.Splitter();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.panelOutput = new System.Windows.Forms.Panel();
			this.tabOutput = new TinyPG.Controls.TabControlEx();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.textOutput = new System.Windows.Forms.RichTextBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tvParsetree = new System.Windows.Forms.TreeView();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.regExControl = new TinyPG.Controls.RegExControl();
			this.headerOutput = new TinyPG.Controls.HeaderLabel();
			this.panelInput = new System.Windows.Forms.Panel();
			this.textInput = new System.Windows.Forms.RichTextBox();
			this.headerEvaluator = new TinyPG.Controls.HeaderLabel();
			this.textEditor = new TinyPG.Controls.RichTextBoxEx();
			this.menuStrip.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.panelOutput.SuspendLayout();
			this.tabOutput.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.panelInput.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip
			// 
			this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(1383, 28);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "menuStrip";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator1,
            this.findToolStripMenuItem,
            this.toolStripSeparator3,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newToolStripMenuItem.Size = new System.Drawing.Size(190, 26);
			this.newToolStripMenuItem.Text = "&New";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(190, 26);
			this.openToolStripMenuItem.Text = "&Open...";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(187, 6);
			// 
			// findToolStripMenuItem
			// 
			this.findToolStripMenuItem.Name = "findToolStripMenuItem";
			this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.findToolStripMenuItem.Size = new System.Drawing.Size(190, 26);
			this.findToolStripMenuItem.Text = "&Find...";
			this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(187, 6);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(190, 26);
			this.saveToolStripMenuItem.Text = "&Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(190, 26);
			this.saveAsToolStripMenuItem.Text = "Save &As...";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(187, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(190, 26);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.regexToolToolStripMenuItem,
            this.outputToolStripMenuItem,
            this.parsetreeToolStripMenuItem,
            this.expressionEvaluatorToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
			this.viewToolStripMenuItem.Text = "&View";
			// 
			// regexToolToolStripMenuItem
			// 
			this.regexToolToolStripMenuItem.Name = "regexToolToolStripMenuItem";
			this.regexToolToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
			this.regexToolToolStripMenuItem.Size = new System.Drawing.Size(278, 26);
			this.regexToolToolStripMenuItem.Text = "Regex tool";
			this.regexToolToolStripMenuItem.Click += new System.EventHandler(this.regexToolToolStripMenuItem_Click);
			// 
			// outputToolStripMenuItem
			// 
			this.outputToolStripMenuItem.Name = "outputToolStripMenuItem";
			this.outputToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.outputToolStripMenuItem.Size = new System.Drawing.Size(278, 26);
			this.outputToolStripMenuItem.Text = "&Output";
			this.outputToolStripMenuItem.Click += new System.EventHandler(this.outputToolStripMenuItem_Click);
			// 
			// parsetreeToolStripMenuItem
			// 
			this.parsetreeToolStripMenuItem.Name = "parsetreeToolStripMenuItem";
			this.parsetreeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
			this.parsetreeToolStripMenuItem.Size = new System.Drawing.Size(278, 26);
			this.parsetreeToolStripMenuItem.Text = "Parse &tree";
			this.parsetreeToolStripMenuItem.Click += new System.EventHandler(this.parsetreeToolStripMenuItem_Click);
			// 
			// expressionEvaluatorToolStripMenuItem
			// 
			this.expressionEvaluatorToolStripMenuItem.Name = "expressionEvaluatorToolStripMenuItem";
			this.expressionEvaluatorToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
			this.expressionEvaluatorToolStripMenuItem.Size = new System.Drawing.Size(278, 26);
			this.expressionEvaluatorToolStripMenuItem.Text = "&Expression evaluator";
			this.expressionEvaluatorToolStripMenuItem.Click += new System.EventHandler(this.expressionEvaluatorToolStripMenuItem_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.parseToolStripMenuItem,
            this.menuToolsGenerate,
            this.toolStripMenuItem1,
            this.viewParserToolStripMenuItem,
            this.viewScannerToolStripMenuItem,
            this.viewParseTreeCodeToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
			this.toolsToolStripMenuItem.Text = "&Build";
			// 
			// parseToolStripMenuItem
			// 
			this.parseToolStripMenuItem.Name = "parseToolStripMenuItem";
			this.parseToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.parseToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
			this.parseToolStripMenuItem.Text = "Generate && &Run";
			this.parseToolStripMenuItem.Click += new System.EventHandler(this.parseToolStripMenuItem_Click);
			// 
			// menuToolsGenerate
			// 
			this.menuToolsGenerate.Name = "menuToolsGenerate";
			this.menuToolsGenerate.ShortcutKeys = System.Windows.Forms.Keys.F6;
			this.menuToolsGenerate.Size = new System.Drawing.Size(227, 26);
			this.menuToolsGenerate.Text = "&Generate";
			this.menuToolsGenerate.Click += new System.EventHandler(this.menuToolsGenerate_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(224, 6);
			// 
			// viewParserToolStripMenuItem
			// 
			this.viewParserToolStripMenuItem.Name = "viewParserToolStripMenuItem";
			this.viewParserToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
			this.viewParserToolStripMenuItem.Text = "View &Parser code";
			this.viewParserToolStripMenuItem.Click += new System.EventHandler(this.viewParserToolStripMenuItem_Click);
			// 
			// viewScannerToolStripMenuItem
			// 
			this.viewScannerToolStripMenuItem.Name = "viewScannerToolStripMenuItem";
			this.viewScannerToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
			this.viewScannerToolStripMenuItem.Text = "View &Scanner code";
			this.viewScannerToolStripMenuItem.Click += new System.EventHandler(this.viewScannerToolStripMenuItem_Click);
			// 
			// viewParseTreeCodeToolStripMenuItem
			// 
			this.viewParseTreeCodeToolStripMenuItem.Name = "viewParseTreeCodeToolStripMenuItem";
			this.viewParseTreeCodeToolStripMenuItem.Size = new System.Drawing.Size(227, 26);
			this.viewParseTreeCodeToolStripMenuItem.Text = "View Parse&Tree code";
			this.viewParseTreeCodeToolStripMenuItem.Click += new System.EventHandler(this.viewParseTreeCodeToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutTinyParserGeneratorToolStripMenuItem,
            this.examplesToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// aboutTinyParserGeneratorToolStripMenuItem
			// 
			this.aboutTinyParserGeneratorToolStripMenuItem.Name = "aboutTinyParserGeneratorToolStripMenuItem";
			this.aboutTinyParserGeneratorToolStripMenuItem.Size = new System.Drawing.Size(277, 26);
			this.aboutTinyParserGeneratorToolStripMenuItem.Text = "&About Tiny Parser Generator";
			this.aboutTinyParserGeneratorToolStripMenuItem.Click += new System.EventHandler(this.aboutTinyParserGeneratorToolStripMenuItem_Click);
			// 
			// examplesToolStripMenuItem
			// 
			this.examplesToolStripMenuItem.Name = "examplesToolStripMenuItem";
			this.examplesToolStripMenuItem.Size = new System.Drawing.Size(277, 26);
			this.examplesToolStripMenuItem.Text = "&Examples";
			// 
			// statusStrip
			// 
			this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.toolStripStatusLabel1,
            this.statusLine,
            this.toolStripStatusLabel2,
            this.statusCol,
            this.toolStripStatusLabel4,
            this.statusPos,
            this.toolStripStatusLabel3});
			this.statusStrip.Location = new System.Drawing.Point(0, 769);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
			this.statusStrip.Size = new System.Drawing.Size(1383, 26);
			this.statusStrip.TabIndex = 1;
			this.statusStrip.Text = "statusStrip1";
			// 
			// statusLabel
			// 
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(1077, 20);
			this.statusLabel.Spring = true;
			this.statusLabel.Text = "Ready";
			this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(24, 20);
			this.toolStripStatusLabel1.Text = "Ln";
			this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// statusLine
			// 
			this.statusLine.AutoSize = false;
			this.statusLine.Name = "statusLine";
			this.statusLine.Size = new System.Drawing.Size(50, 20);
			this.statusLine.Text = "-";
			this.statusLine.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Size = new System.Drawing.Size(31, 20);
			this.toolStripStatusLabel2.Text = "Col";
			this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// statusCol
			// 
			this.statusCol.AutoSize = false;
			this.statusCol.Name = "statusCol";
			this.statusCol.Size = new System.Drawing.Size(50, 20);
			this.statusCol.Text = "-";
			this.statusCol.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripStatusLabel4
			// 
			this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
			this.toolStripStatusLabel4.Size = new System.Drawing.Size(31, 20);
			this.toolStripStatusLabel4.Text = "Pos";
			// 
			// statusPos
			// 
			this.statusPos.AutoSize = false;
			this.statusPos.Name = "statusPos";
			this.statusPos.Size = new System.Drawing.Size(50, 20);
			this.statusPos.Text = "-";
			this.statusPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripStatusLabel3
			// 
			this.toolStripStatusLabel3.AutoSize = false;
			this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
			this.toolStripStatusLabel3.Size = new System.Drawing.Size(50, 20);
			this.toolStripStatusLabel3.Text = "INS";
			// 
			// splitterBottom
			// 
			this.splitterBottom.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.splitterBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitterBottom.Location = new System.Drawing.Point(0, 544);
			this.splitterBottom.Margin = new System.Windows.Forms.Padding(4);
			this.splitterBottom.Name = "splitterBottom";
			this.splitterBottom.Size = new System.Drawing.Size(949, 6);
			this.splitterBottom.TabIndex = 5;
			this.splitterBottom.TabStop = false;
			// 
			// splitterRight
			// 
			this.splitterRight.BackColor = System.Drawing.SystemColors.InactiveCaption;
			this.splitterRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.splitterRight.Location = new System.Drawing.Point(949, 28);
			this.splitterRight.Margin = new System.Windows.Forms.Padding(4);
			this.splitterRight.Name = "splitterRight";
			this.splitterRight.Size = new System.Drawing.Size(7, 741);
			this.splitterRight.TabIndex = 7;
			this.splitterRight.TabStop = false;
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "tpg";
			this.openFileDialog.Filter = "Grammar files|*.tpg|All files|*.*";
			this.openFileDialog.Title = "Open Grammar File";
			// 
			// folderBrowserDialog
			// 
			this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.Favorites;
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.Filter = "Grammar files|*.tpg|All files|*.*";
			this.saveFileDialog.Title = "Save Grammar File As";
			// 
			// panelOutput
			// 
			this.panelOutput.Controls.Add(this.tabOutput);
			this.panelOutput.Controls.Add(this.headerOutput);
			this.panelOutput.Dock = System.Windows.Forms.DockStyle.Right;
			this.panelOutput.Location = new System.Drawing.Point(956, 28);
			this.panelOutput.Margin = new System.Windows.Forms.Padding(4);
			this.panelOutput.Name = "panelOutput";
			this.panelOutput.Size = new System.Drawing.Size(427, 741);
			this.panelOutput.TabIndex = 8;
			// 
			// tabOutput
			// 
			this.tabOutput.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.tabOutput.Controls.Add(this.tabPage1);
			this.tabOutput.Controls.Add(this.tabPage2);
			this.tabOutput.Controls.Add(this.tabPage3);
			this.tabOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabOutput.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabOutput.Location = new System.Drawing.Point(0, 25);
			this.tabOutput.Margin = new System.Windows.Forms.Padding(4);
			this.tabOutput.Name = "tabOutput";
			this.tabOutput.Padding = new System.Drawing.Point(10, 3);
			this.tabOutput.SelectedIndex = 0;
			this.tabOutput.Size = new System.Drawing.Size(427, 716);
			this.tabOutput.TabIndex = 6;
			this.tabOutput.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabOutput_Selected);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.textOutput);
			this.tabPage1.Location = new System.Drawing.Point(4, 4);
			this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
			this.tabPage1.Size = new System.Drawing.Size(419, 687);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Output";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// textOutput
			// 
			this.textOutput.BackColor = System.Drawing.SystemColors.Window;
			this.textOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textOutput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textOutput.Location = new System.Drawing.Point(4, 4);
			this.textOutput.Margin = new System.Windows.Forms.Padding(4);
			this.textOutput.Name = "textOutput";
			this.textOutput.ReadOnly = true;
			this.textOutput.Size = new System.Drawing.Size(411, 679);
			this.textOutput.TabIndex = 6;
			this.textOutput.Text = "";
			this.textOutput.WordWrap = false;
			this.textOutput.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.textOutput_LinkClicked);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.tvParsetree);
			this.tabPage2.Location = new System.Drawing.Point(4, 4);
			this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
			this.tabPage2.Size = new System.Drawing.Size(419, 685);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Parse tree";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tvParsetree
			// 
			this.tvParsetree.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tvParsetree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvParsetree.Location = new System.Drawing.Point(4, 4);
			this.tvParsetree.Margin = new System.Windows.Forms.Padding(4);
			this.tvParsetree.Name = "tvParsetree";
			this.tvParsetree.Size = new System.Drawing.Size(411, 677);
			this.tvParsetree.TabIndex = 0;
			this.tvParsetree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvParsetree_AfterSelect);
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.regExControl);
			this.tabPage3.Location = new System.Drawing.Point(4, 4);
			this.tabPage3.Margin = new System.Windows.Forms.Padding(4);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(419, 685);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Regex tool";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// regExControl
			// 
			this.regExControl.BackColor = System.Drawing.SystemColors.Control;
			this.regExControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.regExControl.Location = new System.Drawing.Point(0, 0);
			this.regExControl.Margin = new System.Windows.Forms.Padding(5);
			this.regExControl.Name = "regExControl";
			this.regExControl.Size = new System.Drawing.Size(419, 685);
			this.regExControl.TabIndex = 12;
			// 
			// headerOutput
			// 
			this.headerOutput.Dock = System.Windows.Forms.DockStyle.Top;
			this.headerOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.headerOutput.ForeColor = System.Drawing.SystemColors.GrayText;
			this.headerOutput.Location = new System.Drawing.Point(0, 0);
			this.headerOutput.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.headerOutput.Name = "headerOutput";
			this.headerOutput.Size = new System.Drawing.Size(427, 25);
			this.headerOutput.TabIndex = 7;
			this.headerOutput.Text = "Output";
			this.headerOutput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panelInput
			// 
			this.panelInput.Controls.Add(this.textInput);
			this.panelInput.Controls.Add(this.headerEvaluator);
			this.panelInput.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelInput.Location = new System.Drawing.Point(0, 550);
			this.panelInput.Margin = new System.Windows.Forms.Padding(0);
			this.panelInput.Name = "panelInput";
			this.panelInput.Size = new System.Drawing.Size(949, 219);
			this.panelInput.TabIndex = 9;
			// 
			// textInput
			// 
			this.textInput.AcceptsTab = true;
			this.textInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textInput.DetectUrls = false;
			this.textInput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textInput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textInput.HideSelection = false;
			this.textInput.Location = new System.Drawing.Point(0, 25);
			this.textInput.Margin = new System.Windows.Forms.Padding(4);
			this.textInput.Name = "textInput";
			this.textInput.Size = new System.Drawing.Size(949, 194);
			this.textInput.TabIndex = 2;
			this.textInput.Text = "Enter the text to parse here";
			this.textInput.WordWrap = false;
			this.textInput.SelectionChanged += new System.EventHandler(this.textInput_SelectionChanged);
			this.textInput.Enter += new System.EventHandler(this.textInput_Enter);
			this.textInput.Leave += new System.EventHandler(this.textInput_Leave);
			// 
			// headerEvaluator
			// 
			this.headerEvaluator.Dock = System.Windows.Forms.DockStyle.Top;
			this.headerEvaluator.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.headerEvaluator.ForeColor = System.Drawing.SystemColors.GrayText;
			this.headerEvaluator.Location = new System.Drawing.Point(0, 0);
			this.headerEvaluator.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.headerEvaluator.Name = "headerEvaluator";
			this.headerEvaluator.Size = new System.Drawing.Size(949, 25);
			this.headerEvaluator.TabIndex = 3;
			this.headerEvaluator.Text = "Expression Evaluator";
			this.headerEvaluator.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textEditor
			// 
			this.textEditor.AcceptsTab = true;
			this.textEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textEditor.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textEditor.HideSelection = false;
			this.textEditor.Location = new System.Drawing.Point(0, 28);
			this.textEditor.Margin = new System.Windows.Forms.Padding(4);
			this.textEditor.Name = "textEditor";
			this.textEditor.NumberAlignment = System.Drawing.StringAlignment.Center;
			this.textEditor.NumberBackground1 = System.Drawing.SystemColors.ControlLight;
			this.textEditor.NumberBackground2 = System.Drawing.SystemColors.Window;
			this.textEditor.NumberBorder = System.Drawing.SystemColors.ControlDark;
			this.textEditor.NumberBorderThickness = 1F;
			this.textEditor.NumberColor = System.Drawing.Color.DarkGray;
			this.textEditor.NumberFont = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textEditor.NumberLeadingZeroes = false;
			this.textEditor.NumberLineCounting = TinyPG.Controls.RichTextBoxEx.LineCounting.CRLF;
			this.textEditor.NumberPadding = 2;
			this.textEditor.ShowLineNumbers = true;
			this.textEditor.Size = new System.Drawing.Size(949, 516);
			this.textEditor.TabIndex = 3;
			this.textEditor.Text = "";
			this.textEditor.WordWrap = false;
			this.textEditor.SelectionChanged += new System.EventHandler(this.textEditor_SelectionChanged);
			this.textEditor.TextChanged += new System.EventHandler(this.textEditor_TextChanged);
			this.textEditor.Enter += new System.EventHandler(this.textEditor_Enter);
			this.textEditor.Leave += new System.EventHandler(this.textEditor_Leave);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1383, 795);
			this.Controls.Add(this.textEditor);
			this.Controls.Add(this.splitterBottom);
			this.Controls.Add(this.panelInput);
			this.Controls.Add(this.splitterRight);
			this.Controls.Add(this.panelOutput);
			this.Controls.Add(this.menuStrip);
			this.Controls.Add(this.statusStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "MainForm";
			this.Text = "Tiny Parser Generator .Net";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.panelOutput.ResumeLayout(false);
			this.tabOutput.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.panelInput.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutTinyParserGeneratorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuToolsGenerate;
        private System.Windows.Forms.Splitter splitterBottom;
        private System.Windows.Forms.ToolStripMenuItem parseToolStripMenuItem;
        private System.Windows.Forms.Splitter splitterRight;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem examplesToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Panel panelOutput;
        private TinyPG.Controls.TabControlEx tabOutput;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.RichTextBox textOutput;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TreeView tvParsetree;
        private System.Windows.Forms.TabPage tabPage3;
        private TinyPG.Controls.RegExControl regExControl;
        private TinyPG.Controls.HeaderLabel headerOutput;
        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.RichTextBox textInput;
        private TinyPG.Controls.HeaderLabel headerEvaluator;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expressionEvaluatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem parsetreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem regexToolToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel statusLine;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel statusCol;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel statusPos;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem viewParserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewScannerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewParseTreeCodeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private Controls.RichTextBoxEx textEditor;
	}
}