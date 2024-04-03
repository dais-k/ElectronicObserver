﻿namespace ElectronicObserver.Window
{
	partial class FormJson
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.JsonRawData = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.JsonTreeView = new System.Windows.Forms.TreeView();
            this.TreeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TreeContextMenu_Expand = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeContextMenu_Shrink = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeContextMenu_ShrinkParent = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TreeContextMenu_OutputCSV = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeContextMenu_CopyToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.TreeContextMenu_CopyAsDocument = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ViewJSONContents = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.UpdatesTree = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.AutoUpdateFilter = new System.Windows.Forms.TextBox();
            this.AutoUpdate = new System.Windows.Forms.CheckBox();
            this.TimeCheck = new System.Windows.Forms.CheckBox();
            this.CSVSaver = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.TreeContextMenu.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDrop = true;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(300, 200);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabControl1_DragDrop);
            this.tabControl1.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabControl1_DragEnter);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.JsonRawData);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(292, 172);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Raw";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // JsonRawData
            // 
            this.JsonRawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.JsonRawData.Location = new System.Drawing.Point(3, 3);
            this.JsonRawData.MaxLength = 0;
            this.JsonRawData.Multiline = true;
            this.JsonRawData.Name = "JsonRawData";
            this.JsonRawData.ReadOnly = true;
            this.JsonRawData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.JsonRawData.Size = new System.Drawing.Size(286, 166);
            this.JsonRawData.TabIndex = 0;
            this.JsonRawData.WordWrap = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.JsonTreeView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(292, 174);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tree";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // JsonTreeView
            // 
            this.JsonTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.JsonTreeView.ContextMenuStrip = this.TreeContextMenu;
            this.JsonTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.JsonTreeView.Location = new System.Drawing.Point(3, 3);
            this.JsonTreeView.Margin = new System.Windows.Forms.Padding(0);
            this.JsonTreeView.Name = "JsonTreeView";
            this.JsonTreeView.PathSeparator = ".";
            this.JsonTreeView.ShowNodeToolTips = true;
            this.JsonTreeView.Size = new System.Drawing.Size(286, 168);
            this.JsonTreeView.TabIndex = 0;
            this.JsonTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.JsonTreeView_BeforeExpand);
            this.JsonTreeView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.JsonTreeView_MouseClick);
            // 
            // TreeContextMenu
            // 
            this.TreeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TreeContextMenu_Expand,
            this.TreeContextMenu_Shrink,
            this.TreeContextMenu_ShrinkParent,
            this.toolStripSeparator1,
            this.TreeContextMenu_OutputCSV,
            this.TreeContextMenu_CopyToClipboard,
            this.TreeContextMenu_CopyAsDocument});
            this.TreeContextMenu.Name = "TreeContextMenu";
            this.TreeContextMenu.Size = new System.Drawing.Size(293, 142);
            this.TreeContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.TreeContextMenu_Opening);
            // 
            // TreeContextMenu_Expand
            // 
            this.TreeContextMenu_Expand.Name = "TreeContextMenu_Expand";
            this.TreeContextMenu_Expand.Size = new System.Drawing.Size(292, 22);
            this.TreeContextMenu_Expand.Text = "全て展開";
            this.TreeContextMenu_Expand.Click += new System.EventHandler(this.TreeContextMenu_Expand_Click);
            // 
            // TreeContextMenu_Shrink
            // 
            this.TreeContextMenu_Shrink.Name = "TreeContextMenu_Shrink";
            this.TreeContextMenu_Shrink.Size = new System.Drawing.Size(292, 22);
            this.TreeContextMenu_Shrink.Text = "全て格納";
            this.TreeContextMenu_Shrink.Click += new System.EventHandler(this.TreeContextMenu_Shrink_Click);
            // 
            // TreeContextMenu_ShrinkParent
            // 
            this.TreeContextMenu_ShrinkParent.Name = "TreeContextMenu_ShrinkParent";
            this.TreeContextMenu_ShrinkParent.Size = new System.Drawing.Size(292, 22);
            this.TreeContextMenu_ShrinkParent.Text = "親ノードを格納";
            this.TreeContextMenu_ShrinkParent.Click += new System.EventHandler(this.TreeContextMenu_ShrinkParent_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(289, 6);
            // 
            // TreeContextMenu_OutputCSV
            // 
            this.TreeContextMenu_OutputCSV.Name = "TreeContextMenu_OutputCSV";
            this.TreeContextMenu_OutputCSV.Size = new System.Drawing.Size(292, 22);
            this.TreeContextMenu_OutputCSV.Text = "このノードをCSVに出力...";
            this.TreeContextMenu_OutputCSV.Click += new System.EventHandler(this.TreeContextMenu_OutputCSV_Click);
            // 
            // TreeContextMenu_CopyToClipboard
            // 
            this.TreeContextMenu_CopyToClipboard.Name = "TreeContextMenu_CopyToClipboard";
            this.TreeContextMenu_CopyToClipboard.Size = new System.Drawing.Size(292, 22);
            this.TreeContextMenu_CopyToClipboard.Text = "このノードをクリップボードへコピー";
            this.TreeContextMenu_CopyToClipboard.Click += new System.EventHandler(this.TreeContextMenu_CopyToClipboard_Click);
            // 
            // TreeContextMenu_CopyAsDocument
            // 
            this.TreeContextMenu_CopyAsDocument.Name = "TreeContextMenu_CopyAsDocument";
            this.TreeContextMenu_CopyAsDocument.Size = new System.Drawing.Size(292, 22);
            this.TreeContextMenu_CopyAsDocument.Text = "このノードをドキュメント化してコピー";
            this.TreeContextMenu_CopyAsDocument.Click += new System.EventHandler(this.TreeContextMenu_CopyAsDocument_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.ViewJSONContents);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.UpdatesTree);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.AutoUpdateFilter);
            this.tabPage3.Controls.Add(this.AutoUpdate);
            this.tabPage3.Controls.Add(this.TimeCheck);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(292, 172);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Config";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // ViewJSONContents
            // 
            this.ViewJSONContents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ViewJSONContents.FormattingEnabled = true;
            this.ViewJSONContents.Items.AddRange(new object[] {
            "JSONのデータを全て表示する",
            "APIにアクセスした時間のみを表示する",
            "表示しない"});
            this.ViewJSONContents.Location = new System.Drawing.Point(80, 87);
            this.ViewJSONContents.Name = "ViewJSONContents";
            this.ViewJSONContents.Size = new System.Drawing.Size(204, 23);
            this.ViewJSONContents.TabIndex = 3;
            this.ViewJSONContents.SelectedIndexChanged += new System.EventHandler(this.ViewJSONContents_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(249, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "◆ D&D で保存した json ファイル を読み込めます";
            this.label3.UseMnemonic = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(140, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 44);
            this.label2.TabIndex = 4;
            this.label2.Text = "※自動更新を有効にすると、\r\n　重くなる可能性があります";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UpdatesTree
            // 
            this.UpdatesTree.AutoSize = true;
            this.UpdatesTree.Location = new System.Drawing.Point(9, 32);
            this.UpdatesTree.Name = "UpdatesTree";
            this.UpdatesTree.Size = new System.Drawing.Size(104, 19);
            this.UpdatesTree.TabIndex = 1;
            this.UpdatesTree.Text = "Treeも更新する";
            this.UpdatesTree.UseVisualStyleBackColor = true;
            this.UpdatesTree.CheckedChanged += new System.EventHandler(this.UpdatesTree_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "JSON表示：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "フィルタ：";
            // 
            // AutoUpdateFilter
            // 
            this.AutoUpdateFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AutoUpdateFilter.Location = new System.Drawing.Point(66, 57);
            this.AutoUpdateFilter.Name = "AutoUpdateFilter";
            this.AutoUpdateFilter.Size = new System.Drawing.Size(218, 23);
            this.AutoUpdateFilter.TabIndex = 2;
            this.AutoUpdateFilter.Validated += new System.EventHandler(this.AutoUpdateFilter_Validated);
            // 
            // AutoUpdate
            // 
            this.AutoUpdate.AutoSize = true;
            this.AutoUpdate.Location = new System.Drawing.Point(9, 7);
            this.AutoUpdate.Name = "AutoUpdate";
            this.AutoUpdate.Size = new System.Drawing.Size(93, 19);
            this.AutoUpdate.TabIndex = 0;
            this.AutoUpdate.Text = "自動更新する";
            this.AutoUpdate.UseVisualStyleBackColor = true;
            this.AutoUpdate.CheckedChanged += new System.EventHandler(this.AutoUpdate_CheckedChanged);
            // 
            // TimeCheck
            // 
            this.TimeCheck.AutoSize = true;
            this.TimeCheck.Location = new System.Drawing.Point(9, 114);
            this.TimeCheck.Name = "TimeCheck";
            this.TimeCheck.Size = new System.Drawing.Size(138, 19);
            this.TimeCheck.TabIndex = 4;
            this.TimeCheck.Text = "非稼働時間を表示する";
            this.TimeCheck.UseVisualStyleBackColor = true;
            this.TimeCheck.CheckedChanged += new System.EventHandler(this.TimeCheck_CheckedChanged);
            // 
            // CSVSaver
            // 
            this.CSVSaver.Filter = "CSV|*.csv|File|*";
            this.CSVSaver.Title = "ノードを CSV に出力";
            // 
            // FormJson
            // 
            this.AutoHidePortion = 150D;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Name = "FormJson";
            this.Text = "JSON";
            this.Load += new System.EventHandler(this.FormJson_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.TreeContextMenu.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TreeView JsonTreeView;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TextBox JsonRawData;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox AutoUpdateFilter;
		private System.Windows.Forms.CheckBox AutoUpdate;
		private System.Windows.Forms.CheckBox UpdatesTree;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ContextMenuStrip TreeContextMenu;
		private System.Windows.Forms.ToolStripMenuItem TreeContextMenu_Expand;
		private System.Windows.Forms.ToolStripMenuItem TreeContextMenu_Shrink;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem TreeContextMenu_OutputCSV;
		private System.Windows.Forms.SaveFileDialog CSVSaver;
		private System.Windows.Forms.ToolStripMenuItem TreeContextMenu_ShrinkParent;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ToolStripMenuItem TreeContextMenu_CopyToClipboard;
		private System.Windows.Forms.ToolStripMenuItem TreeContextMenu_CopyAsDocument;
		private System.Windows.Forms.CheckBox TimeCheck;
		private System.Windows.Forms.ComboBox ViewJSONContents;
		private System.Windows.Forms.Label label4;
	}
}