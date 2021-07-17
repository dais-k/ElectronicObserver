
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
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dataGridView_ShipTypes = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShipType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelAVG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelMAX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExpSum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.SaveCSV_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Reload_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chart_ShipTypes = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabControl_ShipType = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ShipTypes)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_ShipTypes)).BeginInit();
            this.tabControl_ShipType.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            this.dataGridView_ShipTypes.Size = new System.Drawing.Size(662, 446);
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
            this.label1.Location = new System.Drawing.Point(10, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "艦種別(Lv1の艦を除く)";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveCSV_ToolStripMenuItem,
            this.Reload_ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(700, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // SaveCSV_ToolStripMenuItem
            // 
            this.SaveCSV_ToolStripMenuItem.Name = "SaveCSV_ToolStripMenuItem";
            this.SaveCSV_ToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.SaveCSV_ToolStripMenuItem.Text = "CSV保存(&S)";
            this.SaveCSV_ToolStripMenuItem.Click += new System.EventHandler(this.SaveCSV_ToolStripMenuItem_Click);
            // 
            // Reload_ToolStripMenuItem
            // 
            this.Reload_ToolStripMenuItem.Name = "Reload_ToolStripMenuItem";
            this.Reload_ToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.Reload_ToolStripMenuItem.Text = "更新(&R)";
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
            legend3.Name = "Legend1";
            this.chart_ShipTypes.Legends.Add(legend3);
            this.chart_ShipTypes.Location = new System.Drawing.Point(6, 6);
            this.chart_ShipTypes.Name = "chart_ShipTypes";
            series7.BorderColor = System.Drawing.Color.Red;
            series7.BorderWidth = 3;
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
            series7.Color = System.Drawing.Color.Transparent;
            series7.Legend = "Legend1";
            series7.Name = "最高Lv";
            series8.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            series8.BorderWidth = 3;
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
            series8.Color = System.Drawing.Color.Transparent;
            series8.Legend = "Legend1";
            series8.Name = "平均Lv";
            series9.BorderColor = System.Drawing.Color.Aqua;
            series9.BorderWidth = 3;
            series9.ChartArea = "ChartArea1";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
            series9.Color = System.Drawing.Color.Transparent;
            series9.Legend = "Legend1";
            series9.Name = "最小Lv";
            this.chart_ShipTypes.Series.Add(series7);
            this.chart_ShipTypes.Series.Add(series8);
            this.chart_ShipTypes.Series.Add(series9);
            this.chart_ShipTypes.Size = new System.Drawing.Size(656, 440);
            this.chart_ShipTypes.TabIndex = 4;
            this.chart_ShipTypes.Text = "艦種別(Lv1の艦を除く)";
            // 
            // tabControl_ShipType
            // 
            this.tabControl_ShipType.Controls.Add(this.tabPage1);
            this.tabControl_ShipType.Controls.Add(this.tabPage2);
            this.tabControl_ShipType.Location = new System.Drawing.Point(12, 39);
            this.tabControl_ShipType.Name = "tabControl_ShipType";
            this.tabControl_ShipType.SelectedIndex = 0;
            this.tabControl_ShipType.Size = new System.Drawing.Size(676, 478);
            this.tabControl_ShipType.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView_ShipTypes);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(668, 452);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "データ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chart_ShipTypes);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(668, 452);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "レーダーチャート";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // DialogFleetAnalysis
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(700, 529);
            this.Controls.Add(this.tabControl_ShipType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogFleetAnalysis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "艦隊分析";
            this.Load += new System.EventHandler(this.DialogFleetAnalysis_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ShipTypes)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_ShipTypes)).EndInit();
            this.tabControl_ShipType.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
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
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem SaveCSV_ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem Reload_ToolStripMenuItem;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart_ShipTypes;
		private System.Windows.Forms.TabControl tabControl_ShipType;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
	}
}