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
            this.ContextMenuFleet_CopyAllShips = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuFleet_CopyAllEquips = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ContextMenuFleet_OpenAirControlSimulator = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuFleet_OpenTacticalRoom = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuFleet_OpenKacColleSupportKai = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuFleet_OpenCompassSimulator = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ContextMenuFleet_AntiAirDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuFleet_Capture = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuFleet_OutputFleetImage = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
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
            this.ContextMenuFleet_OutputFleetImage,
            this.toolStripSeparator3,
            this.ContextMenuFleet_OpenAirControlSimulator,
            this.ContextMenuFleet_OpenTacticalRoom,
            this.ContextMenuFleet_OpenKacColleSupportKai,
            this.ContextMenuFleet_OpenCompassSimulator,
            this.toolStripSeparator1,
            this.ContextMenuFleet_CopyFleetDeckBuilder,
            this.ContextMenuFleet_CopyAllShips,
            this.ContextMenuFleet_CopyAllEquips,
            this.toolStripSeparator2,
            this.ContextMenuFleet_Capture,
            this.ContextMenuFleet_AntiAirDetails});
            this.ContextMenuFleet.Name = "ContextMenuFleet";
            this.ContextMenuFleet.Size = new System.Drawing.Size(324, 286);
            this.ContextMenuFleet.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuFleet_Opening);
            // 
            // ContextMenuFleet_CopyFleet
            // 
            this.ContextMenuFleet_CopyFleet.Name = "ContextMenuFleet_CopyFleet";
            this.ContextMenuFleet_CopyFleet.Size = new System.Drawing.Size(323, 22);
            this.ContextMenuFleet_CopyFleet.Text = "編成テキストをコピー(&C)";
            this.ContextMenuFleet_CopyFleet.Click += new System.EventHandler(this.ContextMenuFleet_CopyFleet_Click);
            // 
            // ContextMenuFleet_CopyFleetDeckBuilder
            // 
            this.ContextMenuFleet_CopyFleetDeckBuilder.Name = "ContextMenuFleet_CopyFleetDeckBuilder";
            this.ContextMenuFleet_CopyFleetDeckBuilder.Size = new System.Drawing.Size(323, 22);
            this.ContextMenuFleet_CopyFleetDeckBuilder.Text = "艦隊編成をコピー(&1)";
            this.ContextMenuFleet_CopyFleetDeckBuilder.Click += new System.EventHandler(this.ContextMenuFleet_CopyFleetDeckBuilder_Click);
            // 
            // ContextMenuFleet_CopyAllShips
            // 
            this.ContextMenuFleet_CopyAllShips.Name = "ContextMenuFleet_CopyAllShips";
            this.ContextMenuFleet_CopyAllShips.Size = new System.Drawing.Size(323, 22);
            this.ContextMenuFleet_CopyAllShips.Text = "全所属艦娘をコピー(&2)";
            this.ContextMenuFleet_CopyAllShips.Click += new System.EventHandler(this.ContextMenuFleet_CopyAllShips_Click);
            // 
            // ContextMenuFleet_CopyAllEquips
            // 
            this.ContextMenuFleet_CopyAllEquips.Name = "ContextMenuFleet_CopyAllEquips";
            this.ContextMenuFleet_CopyAllEquips.Size = new System.Drawing.Size(323, 22);
            this.ContextMenuFleet_CopyAllEquips.Text = "全所有装備をコピー(&3)";
            this.ContextMenuFleet_CopyAllEquips.Click += new System.EventHandler(this.ContextMenuFleet_CopyAllEquips_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(320, 6);
            // 
            // ContextMenuFleet_OpenAirControlSimulator
            // 
            this.ContextMenuFleet_OpenAirControlSimulator.Name = "ContextMenuFleet_OpenAirControlSimulator";
            this.ContextMenuFleet_OpenAirControlSimulator.Size = new System.Drawing.Size(323, 22);
            this.ContextMenuFleet_OpenAirControlSimulator.Text = "制空権シミュレータを開き艦隊編成を反映(&N)";
            this.ContextMenuFleet_OpenAirControlSimulator.Click += new System.EventHandler(this.ContextMenuFleet_OpenAirControlSimulator_Click);
            // 
            // ContextMenuFleet_OpenTacticalRoom
            // 
            this.ContextMenuFleet_OpenTacticalRoom.Name = "ContextMenuFleet_OpenTacticalRoom";
            this.ContextMenuFleet_OpenTacticalRoom.Size = new System.Drawing.Size(323, 22);
            this.ContextMenuFleet_OpenTacticalRoom.Text = "作戦室を開き艦隊編成を反映(&J)";
            this.ContextMenuFleet_OpenTacticalRoom.Click += new System.EventHandler(this.ContextMenuFleet_OpenTacticalRoom_Click);
            // 
            // ContextMenuFleet_OpenKacColleSupportKai
            // 
            this.ContextMenuFleet_OpenKacColleSupportKai.Name = "ContextMenuFleet_OpenKacColleSupportKai";
            this.ContextMenuFleet_OpenKacColleSupportKai.Size = new System.Drawing.Size(323, 22);
            this.ContextMenuFleet_OpenKacColleSupportKai.Text = "編成をコピーしらくらく支援艦隊改を開く(&K)";
            this.ContextMenuFleet_OpenKacColleSupportKai.Click += new System.EventHandler(this.ContextMenuFleet_OpenKacColleSupportKai_Click);
            // 
            // ContextMenuFleet_OpenCompassSimulator
            // 
            this.ContextMenuFleet_OpenCompassSimulator.Name = "ContextMenuFleet_OpenCompassSimulator";
            this.ContextMenuFleet_OpenCompassSimulator.Size = new System.Drawing.Size(323, 22);
            this.ContextMenuFleet_OpenCompassSimulator.Text = "編成をコピーし羅針盤シミュレータを開く(&P)";
            this.ContextMenuFleet_OpenCompassSimulator.Click += new System.EventHandler(this.ContextMenuFleet_OpenCompassSimulator_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(320, 6);
            // 
            // ContextMenuFleet_AntiAirDetails
            // 
            this.ContextMenuFleet_AntiAirDetails.Name = "ContextMenuFleet_AntiAirDetails";
            this.ContextMenuFleet_AntiAirDetails.Size = new System.Drawing.Size(323, 22);
            this.ContextMenuFleet_AntiAirDetails.Text = "対空砲火の詳細表示(&A)";
            this.ContextMenuFleet_AntiAirDetails.Click += new System.EventHandler(this.ContextMenuFleet_AntiAirDetails_Click);
            // 
            // ContextMenuFleet_Capture
            // 
            this.ContextMenuFleet_Capture.Name = "ContextMenuFleet_Capture";
            this.ContextMenuFleet_Capture.Size = new System.Drawing.Size(323, 22);
            this.ContextMenuFleet_Capture.Text = "この画面をキャプチャ(&S)";
            this.ContextMenuFleet_Capture.Click += new System.EventHandler(this.ContextMenuFleet_Capture_Click);
            // 
            // ContextMenuFleet_OutputFleetImage
            // 
            this.ContextMenuFleet_OutputFleetImage.Name = "ContextMenuFleet_OutputFleetImage";
            this.ContextMenuFleet_OutputFleetImage.Size = new System.Drawing.Size(323, 22);
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
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(320, 6);
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
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_AntiAirDetails;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_OutputFleetImage;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_OpenAirControlSimulator;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_OpenTacticalRoom;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_CopyAllShips;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_CopyAllEquips;
		private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_OpenCompassSimulator;
        private System.Windows.Forms.ToolStripMenuItem ContextMenuFleet_OpenKacColleSupportKai;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}