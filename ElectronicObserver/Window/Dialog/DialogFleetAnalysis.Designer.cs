
namespace ElectronicObserver.Window.Dialog
{
	partial class DialogFleetAnalysis
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.dataGridView_ShipTypes = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShipType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelAVG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelMAX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExpSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.FleetAnalysis_menuStrip = new System.Windows.Forms.MenuStrip();
            this.SaveCSV_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReadCSV_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Reload_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chart_ShipTypes = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabControl_ShipType = new System.Windows.Forms.TabControl();
            this.tabPage_ShipType_Data = new System.Windows.Forms.TabPage();
            this.tabPage_ShipType_Chart = new System.Windows.Forms.TabPage();
            this.checkBox_LvMin = new System.Windows.Forms.CheckBox();
            this.checkBox_LvAvg = new System.Windows.Forms.CheckBox();
            this.checkBox_LvMax = new System.Windows.Forms.CheckBox();
            this.tabControl_Parent = new System.Windows.Forms.TabControl();
            this.tabPage_ShipType = new System.Windows.Forms.TabPage();
            this.tabPage_Level = new System.Windows.Forms.TabPage();
            this.tabControl_Level = new System.Windows.Forms.TabControl();
            this.tabPage_Level_Data = new System.Windows.Forms.TabPage();
            this.dataGridView_Level = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage_Level_Chart = new System.Windows.Forms.TabPage();
            this.chart_Level = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.labelAllSumExp = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ShipTypes)).BeginInit();
            this.FleetAnalysis_menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_ShipTypes)).BeginInit();
            this.tabControl_ShipType.SuspendLayout();
            this.tabPage_ShipType_Data.SuspendLayout();
            this.tabPage_ShipType_Chart.SuspendLayout();
            this.tabControl_Parent.SuspendLayout();
            this.tabPage_ShipType.SuspendLayout();
            this.tabPage_Level.SuspendLayout();
            this.tabControl_Level.SuspendLayout();
            this.tabPage_Level_Data.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Level)).BeginInit();
            this.tabPage_Level_Chart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Level)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_ShipTypes
            // 
            this.dataGridView_ShipTypes.AllowUserToAddRows = false;
            this.dataGridView_ShipTypes.AllowUserToDeleteRows = false;
            this.dataGridView_ShipTypes.AllowUserToResizeRows = false;
            this.dataGridView_ShipTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_ShipTypes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.ShipType,
            this.Num,
            this.LevelAVG,
            this.LevelMin,
            this.LevelMAX,
            this.ExpSum});
            this.dataGridView_ShipTypes.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_ShipTypes.MultiSelect = false;
            this.dataGridView_ShipTypes.Name = "dataGridView_ShipTypes";
            this.dataGridView_ShipTypes.ReadOnly = true;
            this.dataGridView_ShipTypes.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView_ShipTypes.RowTemplate.Height = 21;
            this.dataGridView_ShipTypes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_ShipTypes.Size = new System.Drawing.Size(637, 464);
            this.dataGridView_ShipTypes.TabIndex = 1;
            // 
            // Index
            // 
            this.Index.FillWeight = 40F;
            this.Index.HeaderText = "通番";
            this.Index.MinimumWidth = 40;
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            this.Index.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Index.Width = 40;
            // 
            // ShipType
            // 
            this.ShipType.HeaderText = "艦種";
            this.ShipType.MinimumWidth = 100;
            this.ShipType.Name = "ShipType";
            this.ShipType.ReadOnly = true;
            this.ShipType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Num
            // 
            this.Num.FillWeight = 50F;
            this.Num.HeaderText = "隻数";
            this.Num.MinimumWidth = 50;
            this.Num.Name = "Num";
            this.Num.ReadOnly = true;
            this.Num.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Num.Width = 50;
            // 
            // LevelAVG
            // 
            this.LevelAVG.FillWeight = 76F;
            this.LevelAVG.HeaderText = "平均Lv";
            this.LevelAVG.MinimumWidth = 76;
            this.LevelAVG.Name = "LevelAVG";
            this.LevelAVG.ReadOnly = true;
            this.LevelAVG.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelAVG.Width = 76;
            // 
            // LevelMin
            // 
            this.LevelMin.FillWeight = 76F;
            this.LevelMin.HeaderText = "最小Lv";
            this.LevelMin.MinimumWidth = 76;
            this.LevelMin.Name = "LevelMin";
            this.LevelMin.ReadOnly = true;
            this.LevelMin.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelMin.Width = 76;
            // 
            // LevelMAX
            // 
            this.LevelMAX.FillWeight = 76F;
            this.LevelMAX.HeaderText = "最大Lv";
            this.LevelMAX.MinimumWidth = 76;
            this.LevelMAX.Name = "LevelMAX";
            this.LevelMAX.ReadOnly = true;
            this.LevelMAX.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.LevelMAX.Width = 76;
            // 
            // ExpSum
            // 
            this.ExpSum.FillWeight = 150F;
            this.ExpSum.HeaderText = "取得経験値";
            this.ExpSum.MinimumWidth = 150;
            this.ExpSum.Name = "ExpSum";
            this.ExpSum.ReadOnly = true;
            this.ExpSum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ExpSum.Width = 150;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(515, 540);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "データはロック済みかつLv2以上の艦";
            // 
            // FleetAnalysis_menuStrip
            // 
            this.FleetAnalysis_menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveCSV_ToolStripMenuItem,
            this.ReadCSV_ToolStripMenuItem,
            this.Reload_ToolStripMenuItem});
            this.FleetAnalysis_menuStrip.Location = new System.Drawing.Point(0, 0);
            this.FleetAnalysis_menuStrip.Name = "FleetAnalysis_menuStrip";
            this.FleetAnalysis_menuStrip.Size = new System.Drawing.Size(700, 24);
            this.FleetAnalysis_menuStrip.TabIndex = 3;
            this.FleetAnalysis_menuStrip.Text = "menuStrip1";
            // 
            // SaveCSV_ToolStripMenuItem
            // 
            this.SaveCSV_ToolStripMenuItem.Name = "SaveCSV_ToolStripMenuItem";
            this.SaveCSV_ToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.SaveCSV_ToolStripMenuItem.Text = "CSV保存(&S)";
            this.SaveCSV_ToolStripMenuItem.ToolTipText = "自艦隊のデータをCSV形式で出力します";
            this.SaveCSV_ToolStripMenuItem.Click += new System.EventHandler(this.SaveCSV_ToolStripMenuItem_Click);
            // 
            // ReadCSV_ToolStripMenuItem
            // 
            this.ReadCSV_ToolStripMenuItem.Name = "ReadCSV_ToolStripMenuItem";
            this.ReadCSV_ToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.ReadCSV_ToolStripMenuItem.Text = "CSV読込(&R)";
            this.ReadCSV_ToolStripMenuItem.ToolTipText = "CSV形式で保存されている艦隊データを読み込みます";
            this.ReadCSV_ToolStripMenuItem.Click += new System.EventHandler(this.ReadCSV_ToolStripMenuItem_Click);
            // 
            // Reload_ToolStripMenuItem
            // 
            this.Reload_ToolStripMenuItem.Name = "Reload_ToolStripMenuItem";
            this.Reload_ToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.Reload_ToolStripMenuItem.Text = "更新(&R)";
            this.Reload_ToolStripMenuItem.ToolTipText = "自艦隊の最新データに更新します";
            this.Reload_ToolStripMenuItem.Click += new System.EventHandler(this.Reload_ToolStripMenuItem_Click);
            // 
            // chart_ShipTypes
            // 
            chartArea3.AxisX.IsLabelAutoFit = false;
            chartArea3.AxisX.LabelStyle.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea3.AxisY.Interval = 25D;
            chartArea3.AxisY.IntervalOffset = 25D;
            chartArea3.AxisY.IsLabelAutoFit = false;
            chartArea3.AxisY.LineColor = System.Drawing.Color.Gainsboro;
            chartArea3.AxisY.MajorGrid.IntervalOffset = 25D;
            chartArea3.AxisY.MajorTickMark.Enabled = false;
            chartArea3.AxisY.Maximum = 175D;
            chartArea3.AxisY.MinorGrid.Enabled = true;
            chartArea3.AxisY.MinorGrid.IntervalOffset = 5D;
            chartArea3.AxisY.MinorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea3.Name = "ChartArea1";
            this.chart_ShipTypes.ChartAreas.Add(chartArea3);
            legend3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend3.Name = "Legend1";
            this.chart_ShipTypes.Legends.Add(legend3);
            this.chart_ShipTypes.Location = new System.Drawing.Point(3, 6);
            this.chart_ShipTypes.Name = "chart_ShipTypes";
            series4.BorderColor = System.Drawing.Color.Red;
            series4.BorderWidth = 3;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
            series4.Color = System.Drawing.Color.Transparent;
            series4.CustomProperties = "AreaDrawingStyle=Polygon";
            series4.Legend = "Legend1";
            series4.MarkerBorderColor = System.Drawing.Color.Red;
            series4.MarkerBorderWidth = 5;
            series4.MarkerColor = System.Drawing.Color.Red;
            series4.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Diamond;
            series4.Name = "最大Lv";
            series5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            series5.BorderWidth = 3;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
            series5.Color = System.Drawing.Color.Transparent;
            series5.CustomProperties = "AreaDrawingStyle=Polygon";
            series5.Legend = "Legend1";
            series5.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            series5.MarkerBorderWidth = 5;
            series5.MarkerColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            series5.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series5.Name = "平均Lv";
            series6.BorderColor = System.Drawing.Color.Aqua;
            series6.BorderWidth = 3;
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
            series6.Color = System.Drawing.Color.Transparent;
            series6.CustomProperties = "AreaDrawingStyle=Polygon";
            series6.Legend = "Legend1";
            series6.MarkerBorderColor = System.Drawing.Color.Aqua;
            series6.MarkerBorderWidth = 5;
            series6.MarkerColor = System.Drawing.Color.Aqua;
            series6.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Square;
            series6.Name = "最小Lv";
            this.chart_ShipTypes.Series.Add(series4);
            this.chart_ShipTypes.Series.Add(series5);
            this.chart_ShipTypes.Series.Add(series6);
            this.chart_ShipTypes.Size = new System.Drawing.Size(637, 461);
            this.chart_ShipTypes.TabIndex = 4;
            this.chart_ShipTypes.Text = "艦種別(Lv1の艦を除く)";
            // 
            // tabControl_ShipType
            // 
            this.tabControl_ShipType.Controls.Add(this.tabPage_ShipType_Data);
            this.tabControl_ShipType.Controls.Add(this.tabPage_ShipType_Chart);
            this.tabControl_ShipType.Location = new System.Drawing.Point(3, 3);
            this.tabControl_ShipType.Name = "tabControl_ShipType";
            this.tabControl_ShipType.SelectedIndex = 0;
            this.tabControl_ShipType.Size = new System.Drawing.Size(644, 496);
            this.tabControl_ShipType.TabIndex = 5;
            // 
            // tabPage_ShipType_Data
            // 
            this.tabPage_ShipType_Data.Controls.Add(this.dataGridView_ShipTypes);
            this.tabPage_ShipType_Data.Location = new System.Drawing.Point(4, 22);
            this.tabPage_ShipType_Data.Name = "tabPage_ShipType_Data";
            this.tabPage_ShipType_Data.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_ShipType_Data.Size = new System.Drawing.Size(636, 470);
            this.tabPage_ShipType_Data.TabIndex = 0;
            this.tabPage_ShipType_Data.Text = "データ";
            this.tabPage_ShipType_Data.UseVisualStyleBackColor = true;
            // 
            // tabPage_ShipType_Chart
            // 
            this.tabPage_ShipType_Chart.BackColor = System.Drawing.Color.White;
            this.tabPage_ShipType_Chart.Controls.Add(this.checkBox_LvMin);
            this.tabPage_ShipType_Chart.Controls.Add(this.checkBox_LvAvg);
            this.tabPage_ShipType_Chart.Controls.Add(this.checkBox_LvMax);
            this.tabPage_ShipType_Chart.Controls.Add(this.chart_ShipTypes);
            this.tabPage_ShipType_Chart.Location = new System.Drawing.Point(4, 22);
            this.tabPage_ShipType_Chart.Name = "tabPage_ShipType_Chart";
            this.tabPage_ShipType_Chart.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_ShipType_Chart.Size = new System.Drawing.Size(636, 470);
            this.tabPage_ShipType_Chart.TabIndex = 1;
            this.tabPage_ShipType_Chart.Text = "レーダーチャート";
            // 
            // checkBox_LvMin
            // 
            this.checkBox_LvMin.AutoSize = true;
            this.checkBox_LvMin.Checked = true;
            this.checkBox_LvMin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_LvMin.Location = new System.Drawing.Point(570, 448);
            this.checkBox_LvMin.Name = "checkBox_LvMin";
            this.checkBox_LvMin.Size = new System.Drawing.Size(60, 16);
            this.checkBox_LvMin.TabIndex = 7;
            this.checkBox_LvMin.Text = "最小Lv";
            this.checkBox_LvMin.UseVisualStyleBackColor = true;
            this.checkBox_LvMin.CheckedChanged += new System.EventHandler(this.checkBox_LvMin_CheckedChanged);
            // 
            // checkBox_LvAvg
            // 
            this.checkBox_LvAvg.AutoSize = true;
            this.checkBox_LvAvg.Checked = true;
            this.checkBox_LvAvg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_LvAvg.Location = new System.Drawing.Point(570, 434);
            this.checkBox_LvAvg.Name = "checkBox_LvAvg";
            this.checkBox_LvAvg.Size = new System.Drawing.Size(60, 16);
            this.checkBox_LvAvg.TabIndex = 6;
            this.checkBox_LvAvg.Text = "平均Lv";
            this.checkBox_LvAvg.UseVisualStyleBackColor = true;
            this.checkBox_LvAvg.CheckedChanged += new System.EventHandler(this.checkBox_LvAvg_CheckedChanged);
            // 
            // checkBox_LvMax
            // 
            this.checkBox_LvMax.AutoSize = true;
            this.checkBox_LvMax.Checked = true;
            this.checkBox_LvMax.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_LvMax.Location = new System.Drawing.Point(570, 420);
            this.checkBox_LvMax.Name = "checkBox_LvMax";
            this.checkBox_LvMax.Size = new System.Drawing.Size(60, 16);
            this.checkBox_LvMax.TabIndex = 5;
            this.checkBox_LvMax.Text = "最大Lv";
            this.checkBox_LvMax.UseVisualStyleBackColor = true;
            this.checkBox_LvMax.CheckedChanged += new System.EventHandler(this.checkBox_LvMax_CheckedChanged);
            // 
            // tabControl_Parent
            // 
            this.tabControl_Parent.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl_Parent.Controls.Add(this.tabPage_ShipType);
            this.tabControl_Parent.Controls.Add(this.tabPage_Level);
            this.tabControl_Parent.Location = new System.Drawing.Point(12, 27);
            this.tabControl_Parent.Multiline = true;
            this.tabControl_Parent.Name = "tabControl_Parent";
            this.tabControl_Parent.SelectedIndex = 0;
            this.tabControl_Parent.Size = new System.Drawing.Size(676, 510);
            this.tabControl_Parent.TabIndex = 6;
            // 
            // tabPage_ShipType
            // 
            this.tabPage_ShipType.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_ShipType.Controls.Add(this.tabControl_ShipType);
            this.tabPage_ShipType.Location = new System.Drawing.Point(22, 4);
            this.tabPage_ShipType.Name = "tabPage_ShipType";
            this.tabPage_ShipType.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_ShipType.Size = new System.Drawing.Size(650, 502);
            this.tabPage_ShipType.TabIndex = 0;
            this.tabPage_ShipType.Text = "艦種別";
            // 
            // tabPage_Level
            // 
            this.tabPage_Level.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage_Level.Controls.Add(this.tabControl_Level);
            this.tabPage_Level.Location = new System.Drawing.Point(22, 4);
            this.tabPage_Level.Name = "tabPage_Level";
            this.tabPage_Level.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Level.Size = new System.Drawing.Size(650, 502);
            this.tabPage_Level.TabIndex = 1;
            this.tabPage_Level.Text = "レベル帯別";
            // 
            // tabControl_Level
            // 
            this.tabControl_Level.Controls.Add(this.tabPage_Level_Data);
            this.tabControl_Level.Controls.Add(this.tabPage_Level_Chart);
            this.tabControl_Level.Location = new System.Drawing.Point(3, 3);
            this.tabControl_Level.Name = "tabControl_Level";
            this.tabControl_Level.SelectedIndex = 0;
            this.tabControl_Level.Size = new System.Drawing.Size(651, 496);
            this.tabControl_Level.TabIndex = 0;
            // 
            // tabPage_Level_Data
            // 
            this.tabPage_Level_Data.Controls.Add(this.dataGridView_Level);
            this.tabPage_Level_Data.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Level_Data.Name = "tabPage_Level_Data";
            this.tabPage_Level_Data.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Level_Data.Size = new System.Drawing.Size(643, 470);
            this.tabPage_Level_Data.TabIndex = 0;
            this.tabPage_Level_Data.Text = "データ";
            this.tabPage_Level_Data.UseVisualStyleBackColor = true;
            // 
            // dataGridView_Level
            // 
            this.dataGridView_Level.AllowUserToAddRows = false;
            this.dataGridView_Level.AllowUserToDeleteRows = false;
            this.dataGridView_Level.AllowUserToResizeRows = false;
            this.dataGridView_Level.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_Level.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
            this.dataGridView_Level.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_Level.MultiSelect = false;
            this.dataGridView_Level.Name = "dataGridView_Level";
            this.dataGridView_Level.ReadOnly = true;
            this.dataGridView_Level.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView_Level.RowTemplate.Height = 21;
            this.dataGridView_Level.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_Level.Size = new System.Drawing.Size(637, 464);
            this.dataGridView_Level.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 40F;
            this.dataGridViewTextBoxColumn1.HeaderText = "区分";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 40;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.Width = 40;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "艦種";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 60F;
            this.dataGridViewTextBoxColumn3.HeaderText = "2-39";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn3.Width = 60;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 60F;
            this.dataGridViewTextBoxColumn4.HeaderText = "40-59";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn4.Width = 60;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.FillWeight = 60F;
            this.dataGridViewTextBoxColumn5.HeaderText = "60-79";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn5.Width = 60;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.FillWeight = 60F;
            this.dataGridViewTextBoxColumn6.HeaderText = "80-99";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn6.Width = 60;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.FillWeight = 60F;
            this.dataGridViewTextBoxColumn7.HeaderText = "100-127";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn7.Width = 60;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.FillWeight = 60F;
            this.dataGridViewTextBoxColumn8.HeaderText = "128-";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn8.Width = 60;
            // 
            // tabPage_Level_Chart
            // 
            this.tabPage_Level_Chart.BackColor = System.Drawing.Color.White;
            this.tabPage_Level_Chart.Controls.Add(this.chart_Level);
            this.tabPage_Level_Chart.Location = new System.Drawing.Point(4, 22);
            this.tabPage_Level_Chart.Name = "tabPage_Level_Chart";
            this.tabPage_Level_Chart.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Level_Chart.Size = new System.Drawing.Size(643, 470);
            this.tabPage_Level_Chart.TabIndex = 1;
            this.tabPage_Level_Chart.Text = "グラフ";
            // 
            // chart_Level
            // 
            chartArea4.AxisX.Title = "レベル帯";
            chartArea4.AxisY.Title = "隻数";
            chartArea4.Name = "ChartArea1";
            this.chart_Level.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chart_Level.Legends.Add(legend4);
            this.chart_Level.Location = new System.Drawing.Point(3, 6);
            this.chart_Level.Name = "chart_Level";
            this.chart_Level.Size = new System.Drawing.Size(637, 461);
            this.chart_Level.TabIndex = 0;
            this.chart_Level.Text = "chart1";
            // 
            // labelAllSumExp
            // 
            this.labelAllSumExp.AutoSize = true;
            this.labelAllSumExp.Location = new System.Drawing.Point(34, 540);
            this.labelAllSumExp.Name = "labelAllSumExp";
            this.labelAllSumExp.Size = new System.Drawing.Size(23, 12);
            this.labelAllSumExp.TabIndex = 7;
            this.labelAllSumExp.Text = "exp";
            // 
            // DialogFleetAnalysis
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(700, 561);
            this.Controls.Add(this.labelAllSumExp);
            this.Controls.Add(this.tabControl_Parent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FleetAnalysis_menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.FleetAnalysis_menuStrip;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogFleetAnalysis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "艦隊分析";
            this.Load += new System.EventHandler(this.DialogFleetAnalysis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ShipTypes)).EndInit();
            this.FleetAnalysis_menuStrip.ResumeLayout(false);
            this.FleetAnalysis_menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_ShipTypes)).EndInit();
            this.tabControl_ShipType.ResumeLayout(false);
            this.tabPage_ShipType_Data.ResumeLayout(false);
            this.tabPage_ShipType_Chart.ResumeLayout(false);
            this.tabPage_ShipType_Chart.PerformLayout();
            this.tabControl_Parent.ResumeLayout(false);
            this.tabPage_ShipType.ResumeLayout(false);
            this.tabPage_Level.ResumeLayout(false);
            this.tabControl_Level.ResumeLayout(false);
            this.tabPage_Level_Data.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Level)).EndInit();
            this.tabPage_Level_Chart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_Level)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView dataGridView_ShipTypes;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Index;
		private System.Windows.Forms.DataGridViewTextBoxColumn ShipType;
		private System.Windows.Forms.DataGridViewTextBoxColumn Num;
		private System.Windows.Forms.DataGridViewTextBoxColumn LevelAVG;
		private System.Windows.Forms.DataGridViewTextBoxColumn LevelMin;
		private System.Windows.Forms.DataGridViewTextBoxColumn LevelMAX;
		private System.Windows.Forms.DataGridViewTextBoxColumn ExpSum;
		private System.Windows.Forms.MenuStrip FleetAnalysis_menuStrip;
		private System.Windows.Forms.ToolStripMenuItem SaveCSV_ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem Reload_ToolStripMenuItem;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart_ShipTypes;
		private System.Windows.Forms.TabControl tabControl_ShipType;
		private System.Windows.Forms.TabPage tabPage_ShipType_Data;
		private System.Windows.Forms.TabPage tabPage_ShipType_Chart;
		private System.Windows.Forms.CheckBox checkBox_LvMin;
		private System.Windows.Forms.CheckBox checkBox_LvAvg;
		private System.Windows.Forms.CheckBox checkBox_LvMax;
		private System.Windows.Forms.TabControl tabControl_Parent;
		private System.Windows.Forms.TabPage tabPage_ShipType;
		private System.Windows.Forms.TabPage tabPage_Level;
		private System.Windows.Forms.TabControl tabControl_Level;
		private System.Windows.Forms.TabPage tabPage_Level_Data;
		private System.Windows.Forms.TabPage tabPage_Level_Chart;
		private System.Windows.Forms.DataGridView dataGridView_Level;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart_Level;
		private System.Windows.Forms.ToolStripMenuItem ReadCSV_ToolStripMenuItem;
		private System.Windows.Forms.Label labelAllSumExp;
	}
}