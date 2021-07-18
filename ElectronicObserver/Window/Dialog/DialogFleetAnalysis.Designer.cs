
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            this.checkBox_LvMax = new System.Windows.Forms.CheckBox();
            this.checkBox_LvAvg = new System.Windows.Forms.CheckBox();
            this.checkBox_LvMin = new System.Windows.Forms.CheckBox();
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
            chartArea4.AxisX.IsLabelAutoFit = false;
            chartArea4.AxisX.LabelStyle.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea4.AxisY.Interval = 25D;
            chartArea4.AxisY.IntervalOffset = 25D;
            chartArea4.AxisY.IsLabelAutoFit = false;
            chartArea4.AxisY.LineColor = System.Drawing.Color.Gainsboro;
            chartArea4.AxisY.MajorGrid.IntervalOffset = 25D;
            chartArea4.AxisY.MajorTickMark.Enabled = false;
            chartArea4.AxisY.Maximum = 175D;
            chartArea4.AxisY.MinorGrid.Enabled = true;
            chartArea4.AxisY.MinorGrid.IntervalOffset = 5D;
            chartArea4.AxisY.MinorGrid.LineColor = System.Drawing.Color.Gainsboro;
            chartArea4.Name = "ChartArea1";
            this.chart_ShipTypes.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chart_ShipTypes.Legends.Add(legend4);
            this.chart_ShipTypes.Location = new System.Drawing.Point(6, 6);
            this.chart_ShipTypes.Name = "chart_ShipTypes";
            series10.BorderColor = System.Drawing.Color.Red;
            series10.BorderWidth = 3;
            series10.ChartArea = "ChartArea1";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
            series10.Color = System.Drawing.Color.Transparent;
            series10.CustomProperties = "AreaDrawingStyle=Polygon";
            series10.Legend = "Legend1";
            series10.MarkerBorderColor = System.Drawing.Color.Red;
            series10.MarkerBorderWidth = 5;
            series10.MarkerColor = System.Drawing.Color.Red;
            series10.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Diamond;
            series10.Name = "最大Lv";
            series11.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            series11.BorderWidth = 3;
            series11.ChartArea = "ChartArea1";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
            series11.Color = System.Drawing.Color.Transparent;
            series11.CustomProperties = "AreaDrawingStyle=Polygon";
            series11.Legend = "Legend1";
            series11.MarkerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            series11.MarkerBorderWidth = 5;
            series11.MarkerColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            series11.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series11.Name = "平均Lv";
            series12.BorderColor = System.Drawing.Color.Aqua;
            series12.BorderWidth = 3;
            series12.ChartArea = "ChartArea1";
            series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
            series12.Color = System.Drawing.Color.Transparent;
            series12.CustomProperties = "AreaDrawingStyle=Polygon";
            series12.Legend = "Legend1";
            series12.MarkerBorderColor = System.Drawing.Color.Aqua;
            series12.MarkerBorderWidth = 5;
            series12.MarkerColor = System.Drawing.Color.Aqua;
            series12.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Square;
            series12.Name = "最小Lv";
            this.chart_ShipTypes.Series.Add(series10);
            this.chart_ShipTypes.Series.Add(series11);
            this.chart_ShipTypes.Series.Add(series12);
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
            this.tabPage2.Controls.Add(this.checkBox_LvMin);
            this.tabPage2.Controls.Add(this.checkBox_LvAvg);
            this.tabPage2.Controls.Add(this.checkBox_LvMax);
            this.tabPage2.Controls.Add(this.chart_ShipTypes);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(668, 452);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "レーダーチャート";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBox_LvMax
            // 
            this.checkBox_LvMax.AutoSize = true;
            this.checkBox_LvMax.Checked = true;
            this.checkBox_LvMax.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_LvMax.Location = new System.Drawing.Point(602, 405);
            this.checkBox_LvMax.Name = "checkBox_LvMax";
            this.checkBox_LvMax.Size = new System.Drawing.Size(60, 16);
            this.checkBox_LvMax.TabIndex = 5;
            this.checkBox_LvMax.Text = "最大Lv";
            this.checkBox_LvMax.UseVisualStyleBackColor = true;
            this.checkBox_LvMax.CheckedChanged += new System.EventHandler(this.checkBox_LvMax_CheckedChanged);
            // 
            // checkBox_LvAvg
            // 
            this.checkBox_LvAvg.AutoSize = true;
            this.checkBox_LvAvg.Checked = true;
            this.checkBox_LvAvg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_LvAvg.Location = new System.Drawing.Point(602, 419);
            this.checkBox_LvAvg.Name = "checkBox_LvAvg";
            this.checkBox_LvAvg.Size = new System.Drawing.Size(60, 16);
            this.checkBox_LvAvg.TabIndex = 6;
            this.checkBox_LvAvg.Text = "平均Lv";
            this.checkBox_LvAvg.UseVisualStyleBackColor = true;
            this.checkBox_LvAvg.CheckedChanged += new System.EventHandler(this.checkBox_LvAvg_CheckedChanged);
            // 
            // checkBox_LvMin
            // 
            this.checkBox_LvMin.AutoSize = true;
            this.checkBox_LvMin.Checked = true;
            this.checkBox_LvMin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_LvMin.Location = new System.Drawing.Point(602, 433);
            this.checkBox_LvMin.Name = "checkBox_LvMin";
            this.checkBox_LvMin.Size = new System.Drawing.Size(60, 16);
            this.checkBox_LvMin.TabIndex = 7;
            this.checkBox_LvMin.Text = "最小Lv";
            this.checkBox_LvMin.UseVisualStyleBackColor = true;
            this.checkBox_LvMin.CheckedChanged += new System.EventHandler(this.checkBox_LvMin_CheckedChanged);
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
            this.tabPage2.PerformLayout();
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
		private System.Windows.Forms.CheckBox checkBox_LvMin;
		private System.Windows.Forms.CheckBox checkBox_LvAvg;
		private System.Windows.Forms.CheckBox checkBox_LvMax;
	}
}