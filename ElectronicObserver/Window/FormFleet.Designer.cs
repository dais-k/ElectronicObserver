namespace ElectronicObserver.Window
{
	partial class FormFleet
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;


		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.TableMember = new System.Windows.Forms.TableLayoutPanel();
            this.TableFleet = new System.Windows.Forms.TableLayoutPanel();
            this.ContextMenuFleet = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextMenuFleet_CopyFleet = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuFleet_CopyFleetDeckBuilder = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuFleet_CopyKanmusuList = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuFleet_CopyToFleetAnalysis = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuFleet_CopyToFleetAnalysisWithID = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ContextMenuFleet_OpenTacticalRoom = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuFleet_OpenAirControlSimulator = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuFleet_OpenDeckBuilder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ContextMenuFleet_CopyToFleetAnalysisEquip = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuFleet_AntiAirDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuFleet_Capture = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuFleet_OutputFleetImage = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.ContextMenuFleet_OpenCompassSimulator = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuFleet.SuspendLayout();
            this.SuspendLayout();
            // 
            // TableMember
            // 
            this.TableMember.AutoSize = true;
            this.TableMember.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.TableMember.ColumnCount = 6;
            this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableMember.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableMember.Location = new System.Drawing.Point(0, 24);
            this.TableMember.Name = "TableMember";
            this.TableMember.RowCount = 1;
            this.TableMember.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.TableMember.Size = new System.Drawing.Size(0, 21);
            this.TableMember.TabIndex = 1;
            this.TableMember.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.TableMember_CellPaint);
            // 
            // TableFleet
            // 
            this.TableFleet.AutoSize = true;
            this.TableFleet.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.TableFleet.ColumnCount = 5;
            this.TableFleet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableFleet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableFleet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableFleet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableFleet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TableFleet.ContextMenuStrip = this.ContextMenuFleet;
            this.TableFleet.Location = new System.Drawing.Point(0, 0);
            this.TableFleet.Name = "TableFleet";
            this.TableFleet.RowCount = 1;
            this.TableFleet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.TableFleet.Size = new System.Drawing.Size(0, 21);
            this.TableFleet.TabIndex = 2;
            // 
            // ContextMenuFleet
            // 
            this.ContextMenuFleet.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.ContextMenuFleet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextMenuFleet_CopyFleet,
            this.ContextMenuFleet_CopyFleetDeckBuilder,
            this.ContextMenuFleet_CopyKanmusuList,
            this.ContextMenuFleet_CopyToFleetAnalysis,
            this.ContextMenuFleet_CopyToFleetAnalysisWithID,
            this.toolStripSeparator1,
            this.ContextMenuFleet_OpenTacticalRoom,
            this.ContextMenuFleet_OpenAirControlSimulator,
            this.ContextMenuFleet_OpenCompassSimulator,
            this.ContextMenuFleet_OpenDeckBuilder,
            this.toolStripSeparator2,
            this.ContextMenuFleet_CopyToFleetAnalysisEquip,
            this.ContextMenuFleet_AntiAirDetails,
            this.ContextMenuFleet_Capture,
            this.ContextMenuFleet_OutputFleetImage});
            this.ContextMenuFleet.Name = "ContextMenuFleet";
            this.ContextMenuFleet.Size = new System.Drawing.Size(374, 324);
            this.ContextMenuFleet.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuFleet_Opening);
            // 
            // ContextMenuFleet_CopyFleet
            // 
            this.ContextMenuFleet_CopyFleet.Name = "ContextMenuFleet_CopyFleet";
            this.ContextMenuFleet_CopyFleet.Size = new System.Drawing.Size(373, 22);
            this.ContextMenuFleet_CopyFleet.Text = "編成テキストをコピー(&C)";
            this.ContextMenuFleet_CopyFleet.Click += new System.EventHandler(this.ContextMenuFleet_CopyFleet_Click);
            // 
            // ContextMenuFleet_CopyFleetDeckBuilder
            // 
            this.ContextMenuFleet_CopyFleetDeckBuilder.Name = "ContextMenuFleet_CopyFleetDeckBuilder";
            this.ContextMenuFleet_CopyFleetDeckBuilder.Size = new System.Drawing.Size(373, 22);
            this.ContextMenuFleet_CopyFleetDeckBuilder.Text = "編成をコピー(デッキビルダーフォーマット)(&D)";
            this.ContextMenuFleet_CopyFleetDeckBuilder.Click += new System.EventHandler(this.ContextMenuFleet_CopyFleetDeckBuilder_Click);
            // 
            // ContextMenuFleet_CopyKanmusuList
            // 
            this.ContextMenuFleet_CopyKanmusuList.Name = "ContextMenuFleet_CopyKanmusuList";
            this.ContextMenuFleet_CopyKanmusuList.Size = new System.Drawing.Size(373, 22);
            this.ContextMenuFleet_CopyKanmusuList.Text = "編成をコピー(艦隊晒しページフォーマット)(&R)";
            this.ContextMenuFleet_CopyKanmusuList.Click += new System.EventHandler(this.ContextMenuFleet_CopyKanmusuList_Click);
            // 
            // ContextMenuFleet_CopyToFleetAnalysis
            // 
            this.ContextMenuFleet_CopyToFleetAnalysis.Name = "ContextMenuFleet_CopyToFleetAnalysis";
            this.ContextMenuFleet_CopyToFleetAnalysis.Size = new System.Drawing.Size(373, 22);
            this.ContextMenuFleet_CopyToFleetAnalysis.Text = "全艦娘をコピー(艦隊分析フォーマット)(&F)";
            this.ContextMenuFleet_CopyToFleetAnalysis.Click += new System.EventHandler(this.ContextMenuFleet_CopyToFleetAnalysis_Click);
            // 
            // ContextMenuFleet_CopyToFleetAnalysisWithID
            // 
            this.ContextMenuFleet_CopyToFleetAnalysisWithID.Name = "ContextMenuFleet_CopyToFleetAnalysisWithID";
            this.ContextMenuFleet_CopyToFleetAnalysisWithID.Size = new System.Drawing.Size(373, 22);
            this.ContextMenuFleet_CopyToFleetAnalysisWithID.Text = "ID付きの全艦娘をコピー(制空権シミュレータV2用)(&T)";
            this.ContextMenuFleet_CopyToFleetAnalysisWithID.Click += new System.EventHandler(this.ContextMenuFleet_CopyToFleetAnalysisWithID_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(370, 6);
            // 
            // ContextMenuFleet_OpenTacticalRoom
            // 
            this.ContextMenuFleet_OpenTacticalRoom.Name = "ContextMenuFleet_OpenTacticalRoom";
            this.ContextMenuFleet_OpenTacticalRoom.Size = new System.Drawing.Size(373, 22);
            this.ContextMenuFleet_OpenTacticalRoom.Text = "編成をコピーし作戦室を開く(&W)";
            this.ContextMenuFleet_OpenTacticalRoom.Click += new System.EventHandler(this.ContextMenuFleet_OpenTacticalRoom_Click);
            // 
            // ContextMenuFleet_OpenAirControlSimulator
            // 
            this.ContextMenuFleet_OpenAirControlSimulator.Name = "ContextMenuFleet_OpenAirControlSimulator";
            this.ContextMenuFleet_OpenAirControlSimulator.Size = new System.Drawing.Size(373, 22);
            this.ContextMenuFleet_OpenAirControlSimulator.Text = "編成をコピーし制空権シミュレータを開く(&V)";
            this.ContextMenuFleet_OpenAirControlSimulator.Click += new System.EventHandler(this.ContextMenuFleet_OpenAirControlSimulator_Click);
            // 
            // ContextMenuFleet_OpenDeckBuilder
            // 
            this.ContextMenuFleet_OpenDeckBuilder.Name = "ContextMenuFleet_OpenDeckBuilder";
            this.ContextMenuFleet_OpenDeckBuilder.Size = new System.Drawing.Size(373, 22);
            this.ContextMenuFleet_OpenDeckBuilder.Text = "デッキビルダーを開く(&X)";
            this.ContextMenuFleet_OpenDeckBuilder.Click += new System.EventHandler(this.ContextMenuFleet_OpenDeckBuilder_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(370, 6);
            // 
            // ContextMenuFleet_CopyToFleetAnalysisEquip
            // 
            this.ContextMenuFleet_CopyToFleetAnalysisEquip.Name = "ContextMenuFleet_CopyToFleetAnalysisEquip";
            this.ContextMenuFleet_CopyToFleetAnalysisEquip.Size = new System.Drawing.Size(373, 22);
            this.ContextMenuFleet_CopyToFleetAnalysisEquip.Text = "所有装備をコピー(艦隊分析フォーマット)(&E)";
            this.ContextMenuFleet_CopyToFleetAnalysisEquip.Click += new System.EventHandler(this.ContextMenuFleet_CopyToFleetAnalysisEquip_Click);
            // 
            // ContextMenuFleet_AntiAirDetails
            // 
            this.ContextMenuFleet_AntiAirDetails.Name = "ContextMenuFleet_AntiAirDetails";
            this.ContextMenuFleet_AntiAirDetails.Size = new System.Drawing.Size(373, 22);
            this.ContextMenuFleet_AntiAirDetails.Text = "対空砲火の詳細表示(&A)";
            this.ContextMenuFleet_AntiAirDetails.Click += new System.EventHandler(this.ContextMenuFleet_AntiAirDetails_Click);
            // 
            // ContextMenuFleet_Capture
            // 
            this.ContextMenuFleet_Capture.Name = "ContextMenuFleet_Capture";
            this.ContextMenuFleet_Capture.Size = new System.Drawing.Size(373, 22);
            this.ContextMenuFleet_Capture.Text = "この画面をキャプチャ(&S)";
            this.ContextMenuFleet_Capture.Click += new System.EventHandler(this.ContextMenuFleet_Capture_Click);
            // 
            // ContextMenuFleet_OutputFleetImage
            // 
            this.ContextMenuFleet_OutputFleetImage.Name = "ContextMenuFleet_OutputFleetImage";
            this.ContextMenuFleet_OutputFleetImage.Size = new System.Drawing.Size(373, 22);
            this.ContextMenuFleet_OutputFleetImage.Text = "編成画像を出力(&I)";
            this.ContextMenuFleet_OutputFleetImage.Click += new System.EventHandler(this.ContextMenuFleet_OutputFleetImage_Click);
            // 
            // ToolTipInfo
            // 
            this.ToolTipInfo.AutoPopDelay = 30000;
            this.ToolTipInfo.InitialDelay = 500;
            this.ToolTipInfo.ReshowDelay = 100;
            this.ToolTipInfo.ShowAlways = true;
            // 
            // ContextMenuFleet_OpenCompassSimulator
            // 
            this.ContextMenuFleet_OpenCompassSimulator.Name = "ContextMenuFleet_OpenCompassSimulator";
            this.ContextMenuFleet_OpenCompassSimulator.Size = new System.Drawing.Size(373, 22);
            this.ContextMenuFleet_OpenCompassSimulator.Text = "編成をコピーし羅針盤シミュレータを開く(&Y)";
            this.ContextMenuFleet_OpenCompassSimulator.Click += new System.EventHandler(this.ContextMenuFleet_OpenCompassSimulator_Click);
            // 
            // FormFleet
            // 
            this.AutoHidePortion = 150D;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Controls.Add(this.TableFleet);
            this.Controls.Add(this.TableMember);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormFleet";
            this.Text = "*not loaded*";
            this.Load += new System.EventHandler(this.FormFleet_Load);
            this.ContextMenuFleet.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel TableMember;
		private System.Windows.Forms.TableLayoutPanel TableFleet;
		private System.Windows.Forms.ToolTip ToolTipInfo;
		private System.Windows.Forms.ContextMenuStrip ContextMenuFleet;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_CopyFleet;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_Capture;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_CopyFleetDeckBuilder;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_CopyKanmusuList;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_AntiAirDetails;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_OutputFleetImage;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_CopyToFleetAnalysis;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_OpenAirControlSimulator;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_OpenTacticalRoom;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_OpenDeckBuilder;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_CopyToFleetAnalysisWithID;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_CopyToFleetAnalysisEquip;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_OpenCompassSimulator;
	}
}