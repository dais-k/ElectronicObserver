
namespace ElectronicObserver.Window
{
	partial class FormSenka
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.labelRecordPrevious = new System.Windows.Forms.Label();
            this.labelRecordDay = new System.Windows.Forms.Label();
            this.labelRecordMonth = new System.Windows.Forms.Label();
            this.labelTitleSenka = new System.Windows.Forms.Label();
            this.labelEOSum = new System.Windows.Forms.Label();
            this.toolTipSenka = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBoxEODetails = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // labelRecordPrevious
            // 
            this.labelRecordPrevious.AutoSize = true;
            this.labelRecordPrevious.Location = new System.Drawing.Point(0, 15);
            this.labelRecordPrevious.Name = "labelRecordPrevious";
            this.labelRecordPrevious.Size = new System.Drawing.Size(36, 15);
            this.labelRecordPrevious.TabIndex = 0;
            this.labelRecordPrevious.Text = "今回:";
            // 
            // labelRecordDay
            // 
            this.labelRecordDay.AutoSize = true;
            this.labelRecordDay.Location = new System.Drawing.Point(0, 30);
            this.labelRecordDay.Name = "labelRecordDay";
            this.labelRecordDay.Size = new System.Drawing.Size(36, 15);
            this.labelRecordDay.TabIndex = 1;
            this.labelRecordDay.Text = "今日:";
            // 
            // labelRecordMonth
            // 
            this.labelRecordMonth.AutoSize = true;
            this.labelRecordMonth.Location = new System.Drawing.Point(0, 45);
            this.labelRecordMonth.Name = "labelRecordMonth";
            this.labelRecordMonth.Size = new System.Drawing.Size(36, 15);
            this.labelRecordMonth.TabIndex = 2;
            this.labelRecordMonth.Text = "今月:";
            // 
            // labelTitleSenka
            // 
            this.labelTitleSenka.AutoSize = true;
            this.labelTitleSenka.Location = new System.Drawing.Point(0, 0);
            this.labelTitleSenka.Name = "labelTitleSenka";
            this.labelTitleSenka.Size = new System.Drawing.Size(41, 15);
            this.labelTitleSenka.TabIndex = 3;
            this.labelTitleSenka.Text = "[戦果]";
            this.toolTipSenka.SetToolTip(this.labelTitleSenka, "母港に戻った時に更新");
            // 
            // labelEOSum
            // 
            this.labelEOSum.AutoSize = true;
            this.labelEOSum.Location = new System.Drawing.Point(0, 60);
            this.labelEOSum.Name = "labelEOSum";
            this.labelEOSum.Size = new System.Drawing.Size(92, 15);
            this.labelEOSum.TabIndex = 4;
            this.labelEOSum.Text = "EO合計: 未取得";
            this.toolTipSenka.SetToolTip(this.labelEOSum, "出撃海域選択画面で更新");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "[EO進捗]";
            this.toolTipSenka.SetToolTip(this.label1, "出撃海域選択画面で更新");
            // 
            // richTextBoxEODetails
            // 
            this.richTextBoxEODetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxEODetails.DetectUrls = false;
            this.richTextBoxEODetails.Location = new System.Drawing.Point(0, 105);
            this.richTextBoxEODetails.Name = "richTextBoxEODetails";
            this.richTextBoxEODetails.ReadOnly = true;
            this.richTextBoxEODetails.Size = new System.Drawing.Size(284, 155);
            this.richTextBoxEODetails.TabIndex = 0;
            this.richTextBoxEODetails.TabStop = false;
            this.richTextBoxEODetails.Text = "";
            // 
            // FormSenka
            // 
            this.AutoHidePortion = 150D;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.richTextBoxEODetails);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelEOSum);
            this.Controls.Add(this.labelTitleSenka);
            this.Controls.Add(this.labelRecordMonth);
            this.Controls.Add(this.labelRecordDay);
            this.Controls.Add(this.labelRecordPrevious);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HideOnClose = true;
            this.Name = "FormSenka";
            this.Text = "戦果";
            this.Load += new System.EventHandler(this.FormSenka_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label labelRecordPrevious;
		private System.Windows.Forms.Label labelRecordDay;
		private System.Windows.Forms.Label labelRecordMonth;
		private System.Windows.Forms.Label labelTitleSenka;
		private System.Windows.Forms.Label labelEOSum;
		private System.Windows.Forms.ToolTip toolTipSenka;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RichTextBox richTextBoxEODetails;
	}
}
