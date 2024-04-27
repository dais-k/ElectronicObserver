namespace ElectronicObserver.Window
{
	partial class FormAccessTime
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
			this.TimeViewer = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// TimeViewer
			// 
			this.TimeViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TimeViewer.Location = new System.Drawing.Point(0, 0);
			this.TimeViewer.MaxLength = 0;
			this.TimeViewer.Multiline = true;
			this.TimeViewer.Name = "TimeViewer";
			this.TimeViewer.ReadOnly = true;
			this.TimeViewer.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.TimeViewer.Size = new System.Drawing.Size(300, 200);
			this.TimeViewer.TabIndex = 1;
			this.TimeViewer.WordWrap = false;
			// 
			// FormAccessTime
			// 
			this.AutoHidePortion = 150D;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(300, 200);
			this.Controls.Add(this.TimeViewer);
			this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.HideOnClose = true;
			this.Name = "FormAccessTime";
			this.Text = "稼働時間";
			this.Load += new System.EventHandler(this.FormAccessTime_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox TimeViewer;
	}
}