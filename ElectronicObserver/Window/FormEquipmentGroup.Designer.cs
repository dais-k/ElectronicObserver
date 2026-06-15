using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ElectronicObserver.Window
{
	partial class FormEquipmentGroup
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			this.EquipView = new System.Windows.Forms.DataGridView();
			this.EquipView_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EquipView_Icon = new System.Windows.Forms.DataGridViewImageColumn();
			this.EquipView_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EquipView_Category1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EquipView_Category2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EquipView_ImproveShips = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EquipView_Range = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EquipView_Firepower = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EquipView_Accuracy = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EquipView_Evasion = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EquipView_Bomber = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EquipView_Torpedo = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EquipView_LOS = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EquipView_ASW = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EquipView_AA = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EquipView_Armor = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EquipView_Radius = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EquipView_EquipedShips = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.MenuMember = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuMember_AddToGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMember_CreateGroup = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMember_Exclude = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuMember_Filter = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMember_ColumnFilter = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuMember_SortOrder = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuMember_CSVOutput = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.MenuGroup_Add = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGroup_Copy = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGroup_Rename = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGroup_Delete = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.MenuGroup_AutoUpdate = new System.Windows.Forms.ToolStripMenuItem();
			this.MenuGroup_ShowStatusBar = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.TabPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.StatusBar = new System.Windows.Forms.StatusStrip();
			this.Status_Total = new System.Windows.Forms.ToolStripStatusLabel();
			this.Status_ByLevel = new System.Windows.Forms.ToolStripStatusLabel();
			this.Status_ByAircraftLevel = new System.Windows.Forms.ToolStripStatusLabel();
			this.SaveCSVDialog = new System.Windows.Forms.SaveFileDialog();
			this.MenuMember_CopyName = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.EquipView)).BeginInit();
			this.MenuMember.SuspendLayout();
			this.MenuGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.StatusBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// EquipView
			// 
			this.EquipView.AllowUserToAddRows = false;
			this.EquipView.AllowUserToDeleteRows = false;
			this.EquipView.AllowUserToOrderColumns = true;
			this.EquipView.AllowUserToResizeRows = false;
			this.EquipView.BackgroundColor = System.Drawing.SystemColors.Control;
			this.EquipView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.EquipView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
			this.EquipView_ID,
			this.EquipView_Icon,
			this.EquipView_Name,
			this.EquipView_Category1,
			this.EquipView_Category2,
			this.EquipView_ImproveShips,
			this.EquipView_Range,
			this.EquipView_Firepower,
			this.EquipView_Accuracy,
			this.EquipView_Evasion,
			this.EquipView_Bomber,
			this.EquipView_Torpedo,
			this.EquipView_LOS,
			this.EquipView_ASW,
			this.EquipView_AA,
			this.EquipView_Armor,
			this.EquipView_Radius,
			this.EquipView_EquipedShips});
			this.EquipView.ContextMenuStrip = this.MenuMember;
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle7.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.EquipView.DefaultCellStyle = dataGridViewCellStyle7;
			this.EquipView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.EquipView.Location = new System.Drawing.Point(0, 0);
			this.EquipView.Name = "EquipView";
			this.EquipView.ReadOnly = true;
			this.EquipView.RowHeadersVisible = false;
			this.EquipView.RowTemplate.Height = 21;
			this.EquipView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.EquipView.Size = new System.Drawing.Size(300, 134);
			this.EquipView.TabIndex = 0;
			this.EquipView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.EquipView_CellFormatting);
			this.EquipView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.EquipView_CellMouseDoubleClick);
			this.EquipView.ColumnDisplayIndexChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.EquipView_ColumnDisplayIndexChanged);
			this.EquipView.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.EquipView_ColumnWidthChanged);
			this.EquipView.SelectionChanged += new System.EventHandler(this.EquipView_SelectionChanged);
			this.EquipView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.EquipView_SortCompare);
			this.EquipView.Sorted += new System.EventHandler(this.EquipView_Sorted);
			// 
			// EquipView_ID
			// 
			this.EquipView_ID.HeaderText = "ID";
			this.EquipView_ID.Name = "EquipView_ID";
			this.EquipView_ID.ReadOnly = true;
			this.EquipView_ID.Width = 60;
			// 
			// EquipView_Icon
			// 
			this.EquipView_Icon.HeaderText = "";
			this.EquipView_Icon.MinimumWidth = 2;
			this.EquipView_Icon.Name = "EquipView_Icon";
			this.EquipView_Icon.ReadOnly = true;
			this.EquipView_Icon.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.EquipView_Icon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.EquipView_Icon.Width = 2;
			// 
			// EquipView_Name
			// 
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.EquipView_Name.DefaultCellStyle = dataGridViewCellStyle1;
			this.EquipView_Name.HeaderText = "装備名";
			this.EquipView_Name.Name = "EquipView_Name";
			this.EquipView_Name.ReadOnly = true;
			this.EquipView_Name.Width = 130;
			// 
			// EquipView_Category1
			// 
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.EquipView_Category1.DefaultCellStyle = dataGridViewCellStyle2;
			this.EquipView_Category1.HeaderText = "カテゴリ";
			this.EquipView_Category1.Name = "EquipView_Category1";
			this.EquipView_Category1.ReadOnly = true;
			// 
			// EquipView_Category2
			// 
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.EquipView_Category2.DefaultCellStyle = dataGridViewCellStyle3;
			this.EquipView_Category2.HeaderText = "カテゴリ2";
			this.EquipView_Category2.Name = "EquipView_Category2";
			this.EquipView_Category2.ReadOnly = true;
			// 
			// EquipView_ImproveShips
			// 
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.EquipView_ImproveShips.DefaultCellStyle = dataGridViewCellStyle4;
			this.EquipView_ImproveShips.HeaderText = "改修担当艦";
			this.EquipView_ImproveShips.Name = "EquipView_ImproveShips";
			this.EquipView_ImproveShips.ReadOnly = true;
			this.EquipView_ImproveShips.Width = 150;
			// 
			// EquipView_Range
			// 
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.EquipView_Range.DefaultCellStyle = dataGridViewCellStyle5;
			this.EquipView_Range.HeaderText = "射程";
			this.EquipView_Range.Name = "EquipView_Range";
			this.EquipView_Range.ReadOnly = true;
			this.EquipView_Range.Width = 42;
			// 
			// EquipView_Firepower
			// 
			this.EquipView_Firepower.HeaderText = "火力";
			this.EquipView_Firepower.Name = "EquipView_Firepower";
			this.EquipView_Firepower.ReadOnly = true;
			this.EquipView_Firepower.Width = 40;
			// 
			// EquipView_Accuracy
			// 
			this.EquipView_Accuracy.HeaderText = "命中/対爆";
			this.EquipView_Accuracy.Name = "EquipView_Accuracy";
			this.EquipView_Accuracy.ReadOnly = true;
			this.EquipView_Accuracy.Width = 40;
			// 
			// EquipView_Evasion
			// 
			this.EquipView_Evasion.HeaderText = "回避/迎撃";
			this.EquipView_Evasion.Name = "EquipView_Evasion";
			this.EquipView_Evasion.ReadOnly = true;
			this.EquipView_Evasion.Width = 40;
			// 
			// EquipView_Bomber
			// 
			this.EquipView_Bomber.HeaderText = "爆装";
			this.EquipView_Bomber.Name = "EquipView_Bomber";
			this.EquipView_Bomber.ReadOnly = true;
			this.EquipView_Bomber.Width = 40;
			// 
			// EquipView_Torpedo
			// 
			this.EquipView_Torpedo.HeaderText = "雷装";
			this.EquipView_Torpedo.Name = "EquipView_Torpedo";
			this.EquipView_Torpedo.ReadOnly = true;
			this.EquipView_Torpedo.Width = 40;
			// 
			// EquipView_LOS
			// 
			this.EquipView_LOS.HeaderText = "索敵";
			this.EquipView_LOS.Name = "EquipView_LOS";
			this.EquipView_LOS.ReadOnly = true;
			this.EquipView_LOS.Width = 40;
			// 
			// EquipView_ASW
			// 
			this.EquipView_ASW.HeaderText = "対潜";
			this.EquipView_ASW.Name = "EquipView_ASW";
			this.EquipView_ASW.ReadOnly = true;
			this.EquipView_ASW.Width = 40;
			// 
			// EquipView_AA
			// 
			this.EquipView_AA.HeaderText = "対空";
			this.EquipView_AA.Name = "EquipView_AA";
			this.EquipView_AA.ReadOnly = true;
			this.EquipView_AA.Width = 40;
			// 
			// EquipView_Armor
			// 
			this.EquipView_Armor.HeaderText = "装甲";
			this.EquipView_Armor.Name = "EquipView_Armor";
			this.EquipView_Armor.ReadOnly = true;
			this.EquipView_Armor.Width = 40;
			// 
			// EquipView_Radius
			// 
			this.EquipView_Radius.HeaderText = "半径";
			this.EquipView_Radius.Name = "EquipView_Radius";
			this.EquipView_Radius.ReadOnly = true;
			this.EquipView_Radius.Width = 40;
			// 
			// EquipView_EquipedShips
			// 
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.EquipView_EquipedShips.DefaultCellStyle = dataGridViewCellStyle6;
			this.EquipView_EquipedShips.HeaderText = "装備中の艦娘";
			this.EquipView_EquipedShips.Name = "EquipView_EquipedShips";
			this.EquipView_EquipedShips.ReadOnly = true;
			this.EquipView_EquipedShips.Width = 120;
			// 
			// MenuMember
			// 
			this.MenuMember.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.MenuMember_AddToGroup,
			this.MenuMember_CreateGroup,
			this.MenuMember_Exclude,
			this.toolStripSeparator2,
			this.MenuMember_Filter,
			this.MenuMember_ColumnFilter,
			this.MenuMember_SortOrder,
			this.toolStripSeparator3,
			this.MenuMember_CSVOutput,
			this.MenuMember_CopyName});
			this.MenuMember.Name = "MenuMember";
			this.MenuMember.Size = new System.Drawing.Size(216, 214);
			this.MenuMember.Opening += new System.ComponentModel.CancelEventHandler(this.MenuMember_Opening);
			// 
			// MenuMember_AddToGroup
			// 
			this.MenuMember_AddToGroup.Name = "MenuMember_AddToGroup";
			this.MenuMember_AddToGroup.Size = new System.Drawing.Size(215, 22);
			this.MenuMember_AddToGroup.Text = "グループへ追加(&A)...";
			this.MenuMember_AddToGroup.Click += new System.EventHandler(this.MenuMember_AddToGroup_Click);
			// 
			// MenuMember_CreateGroup
			// 
			this.MenuMember_CreateGroup.Name = "MenuMember_CreateGroup";
			this.MenuMember_CreateGroup.Size = new System.Drawing.Size(215, 22);
			this.MenuMember_CreateGroup.Text = "新規グループの作成(&N)...";
			this.MenuMember_CreateGroup.Click += new System.EventHandler(this.MenuMember_CreateGroup_Click);
			// 
			// MenuMember_Exclude
			// 
			this.MenuMember_Exclude.Name = "MenuMember_Exclude";
			this.MenuMember_Exclude.Size = new System.Drawing.Size(215, 22);
			this.MenuMember_Exclude.Text = "除外(&E)";
			this.MenuMember_Exclude.Click += new System.EventHandler(this.MenuMember_Exclude_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(212, 6);
			// 
			// MenuMember_Filter
			// 
			this.MenuMember_Filter.Name = "MenuMember_Filter";
			this.MenuMember_Filter.Size = new System.Drawing.Size(215, 22);
			this.MenuMember_Filter.Text = "フィルタ設定(&F)...";
			this.MenuMember_Filter.Click += new System.EventHandler(this.MenuMember_Filter_Click);
			// 
			// MenuMember_ColumnFilter
			// 
			this.MenuMember_ColumnFilter.Name = "MenuMember_ColumnFilter";
			this.MenuMember_ColumnFilter.Size = new System.Drawing.Size(215, 22);
			this.MenuMember_ColumnFilter.Text = "列の表示設定(&C)...";
			this.MenuMember_ColumnFilter.Click += new System.EventHandler(this.MenuMember_ColumnFilter_Click);
			// 
			// MenuMember_SortOrder
			// 
			this.MenuMember_SortOrder.Name = "MenuMember_SortOrder";
			this.MenuMember_SortOrder.Size = new System.Drawing.Size(215, 22);
			this.MenuMember_SortOrder.Text = "自動ソート設定(&S)...";
			this.MenuMember_SortOrder.Click += new System.EventHandler(this.MenuMember_SortOrder_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(212, 6);
			// 
			// MenuMember_CSVOutput
			// 
			this.MenuMember_CSVOutput.Name = "MenuMember_CSVOutput";
			this.MenuMember_CSVOutput.Size = new System.Drawing.Size(215, 22);
			this.MenuMember_CSVOutput.Text = "グループのCSV出力(&O)...";
			this.MenuMember_CSVOutput.Click += new System.EventHandler(this.MenuMember_CSVOutput_Click);
			// 
			// MenuMember_CopyName
			// 
			this.MenuMember_CopyName.Name = "MenuMember_CopyName";
			this.MenuMember_CopyName.Size = new System.Drawing.Size(215, 22);
			this.MenuMember_CopyName.Text = "装備名をコピー";
			this.MenuMember_CopyName.Click += new System.EventHandler(this.MenuMember_CopyName_Click);
			this.MenuMember_CopyName.Visible = true;
			// 
			// MenuGroup
			// 
			this.MenuGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.MenuGroup_Add,
			this.MenuGroup_Copy,
			this.MenuGroup_Rename,
			this.MenuGroup_Delete,
			this.toolStripSeparator4,
			this.MenuGroup_AutoUpdate,
			this.MenuGroup_ShowStatusBar});
			this.MenuGroup.Name = "MenuGroup";
			this.MenuGroup.Size = new System.Drawing.Size(221, 142);
			this.MenuGroup.Opening += new System.ComponentModel.CancelEventHandler(this.MenuGroup_Opening);
			// 
			// MenuGroup_Add
			// 
			this.MenuGroup_Add.Name = "MenuGroup_Add";
			this.MenuGroup_Add.Size = new System.Drawing.Size(220, 22);
			this.MenuGroup_Add.Text = "グループを追加(&A)";
			this.MenuGroup_Add.Click += new System.EventHandler(this.MenuGroup_Add_Click);
			// 
			// MenuGroup_Copy
			// 
			this.MenuGroup_Copy.Name = "MenuGroup_Copy";
			this.MenuGroup_Copy.Size = new System.Drawing.Size(220, 22);
			this.MenuGroup_Copy.Text = "グループをコピー(&C)";
			this.MenuGroup_Copy.Click += new System.EventHandler(this.MenuGroup_Copy_Click);
			// 
			// MenuGroup_Rename
			// 
			this.MenuGroup_Rename.Name = "MenuGroup_Rename";
			this.MenuGroup_Rename.Size = new System.Drawing.Size(220, 22);
			this.MenuGroup_Rename.Text = "グループ名の変更(&R)...";
			this.MenuGroup_Rename.Click += new System.EventHandler(this.MenuGroup_Rename_Click);
			// 
			// MenuGroup_Delete
			// 
			this.MenuGroup_Delete.Name = "MenuGroup_Delete";
			this.MenuGroup_Delete.Size = new System.Drawing.Size(220, 22);
			this.MenuGroup_Delete.Text = "グループを削除(&D)";
			this.MenuGroup_Delete.Click += new System.EventHandler(this.MenuGroup_Delete_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(217, 6);
			// 
			// MenuGroup_AutoUpdate
			// 
			this.MenuGroup_AutoUpdate.CheckOnClick = true;
			this.MenuGroup_AutoUpdate.Name = "MenuGroup_AutoUpdate";
			this.MenuGroup_AutoUpdate.Size = new System.Drawing.Size(220, 22);
			this.MenuGroup_AutoUpdate.Text = "自動更新する";
			// 
			// MenuGroup_ShowStatusBar
			// 
			this.MenuGroup_ShowStatusBar.Checked = true;
			this.MenuGroup_ShowStatusBar.CheckOnClick = true;
			this.MenuGroup_ShowStatusBar.CheckState = System.Windows.Forms.CheckState.Checked;
			this.MenuGroup_ShowStatusBar.Name = "MenuGroup_ShowStatusBar";
			this.MenuGroup_ShowStatusBar.Size = new System.Drawing.Size(220, 22);
			this.MenuGroup_ShowStatusBar.Text = "ステータスバーを表示する";
			this.MenuGroup_ShowStatusBar.CheckedChanged += new System.EventHandler(this.MenuGroup_ShowStatusBar_CheckedChanged);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.TabPanel);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.EquipView);
			this.splitContainer1.Panel2.Controls.Add(this.StatusBar);
			this.splitContainer1.Size = new System.Drawing.Size(300, 200);
			this.splitContainer1.SplitterDistance = 40;
			this.splitContainer1.TabIndex = 1;
			// 
			// TabPanel
			// 
			this.TabPanel.AllowDrop = true;
			this.TabPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
			| System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
			this.TabPanel.AutoScroll = true;
			this.TabPanel.ContextMenuStrip = this.MenuGroup;
			this.TabPanel.Location = new System.Drawing.Point(0, 0);
			this.TabPanel.Name = "TabPanel";
			this.TabPanel.Size = new System.Drawing.Size(300, 40);
			this.TabPanel.TabIndex = 0;
			this.TabPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.TabPanel_DragDrop);
			this.TabPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.TabPanel_DragEnter);
			this.TabPanel.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.TabPanel_QueryContinueDrag);
			this.TabPanel.DoubleClick += new System.EventHandler(this.TabPanel_DoubleClick);
			// 
			// StatusBar
			// 
			this.StatusBar.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.Status_Total,
			this.Status_ByLevel,
			this.Status_ByAircraftLevel});
			this.StatusBar.Location = new System.Drawing.Point(0, 134);
			this.StatusBar.Name = "StatusBar";
			this.StatusBar.Size = new System.Drawing.Size(300, 22);
			this.StatusBar.TabIndex = 1;
			// 
			// Status_Total
			// 
			this.Status_Total.Name = "Status_Total";
			this.Status_Total.Size = new System.Drawing.Size(0, 17);
			// 
			// Status_ByLevel
			// 
			this.Status_ByLevel.Name = "Status_ByLevel";
			this.Status_ByLevel.Size = new System.Drawing.Size(0, 17);
			// 
			// Status_ByAircraftLevel
			// 
			this.Status_ByAircraftLevel.Name = "Status_ByAircraftLevel";
			this.Status_ByAircraftLevel.Size = new System.Drawing.Size(0, 17);
			// 
			// SaveCSVDialog
			// 
			this.SaveCSVDialog.Filter = "CSV|*.csv|File|*";
			this.SaveCSVDialog.Title = "CSVに出力";
			// 
			// FormEquipmentGroup
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.splitContainer1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormEquipmentGroup";
			this.Text = "装備グループ";
			this.Load += new System.EventHandler(this.FormEquipmentGroup_Load);
			((System.ComponentModel.ISupportInitialize)(this.EquipView)).EndInit();
			this.MenuMember.ResumeLayout(false);
			this.MenuGroup.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.StatusBar.ResumeLayout(false);
			this.StatusBar.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView EquipView;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.FlowLayoutPanel TabPanel;
		private System.Windows.Forms.ContextMenuStrip MenuGroup;
		private System.Windows.Forms.ToolStripMenuItem MenuGroup_Add;
		private System.Windows.Forms.ToolStripMenuItem MenuGroup_Delete;
		private System.Windows.Forms.ContextMenuStrip MenuMember;
		private System.Windows.Forms.ToolStripMenuItem MenuGroup_Rename;
		private System.Windows.Forms.ToolStripMenuItem MenuMember_ColumnFilter;
		private System.Windows.Forms.ToolStripMenuItem MenuMember_CSVOutput;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem MenuGroup_AutoUpdate;
		private System.Windows.Forms.StatusStrip StatusBar;
		private System.Windows.Forms.ToolStripStatusLabel Status_Total;
		private System.Windows.Forms.ToolStripStatusLabel Status_ByLevel;
		private System.Windows.Forms.ToolStripStatusLabel Status_ByAircraftLevel;
		private System.Windows.Forms.ToolStripMenuItem MenuGroup_ShowStatusBar;
		private System.Windows.Forms.ToolStripMenuItem MenuMember_Filter;
		private System.Windows.Forms.ToolStripMenuItem MenuMember_SortOrder;
		private System.Windows.Forms.ToolStripMenuItem MenuGroup_Copy;
		private System.Windows.Forms.ToolStripMenuItem MenuMember_AddToGroup;
		private System.Windows.Forms.ToolStripMenuItem MenuMember_CreateGroup;
		private System.Windows.Forms.ToolStripMenuItem MenuMember_Exclude;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.SaveFileDialog SaveCSVDialog;
		private DataGridViewTextBoxColumn EquipView_ID;
		private DataGridViewImageColumn EquipView_Icon;
		private DataGridViewTextBoxColumn EquipView_Name;
		private DataGridViewTextBoxColumn EquipView_Category1;
		private DataGridViewTextBoxColumn EquipView_Category2;
		private DataGridViewTextBoxColumn EquipView_ImproveShips;
		private DataGridViewTextBoxColumn EquipView_Range;
		private DataGridViewTextBoxColumn EquipView_Firepower;
		private DataGridViewTextBoxColumn EquipView_Accuracy;
		private DataGridViewTextBoxColumn EquipView_Evasion;
		private DataGridViewTextBoxColumn EquipView_Bomber;
		private DataGridViewTextBoxColumn EquipView_Torpedo;
		private DataGridViewTextBoxColumn EquipView_LOS;
		private DataGridViewTextBoxColumn EquipView_ASW;
		private DataGridViewTextBoxColumn EquipView_AA;
		private DataGridViewTextBoxColumn EquipView_Armor;
		private DataGridViewTextBoxColumn EquipView_Radius;
		private DataGridViewTextBoxColumn EquipView_EquipedShips;
		private ToolStripMenuItem MenuMember_CopyName;
	}
}