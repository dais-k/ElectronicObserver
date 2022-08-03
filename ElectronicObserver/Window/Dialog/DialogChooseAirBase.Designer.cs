namespace ElectronicObserver.Window.Dialog
{
	partial class DialogChooseAirBase
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
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnOK = new System.Windows.Forms.Button();
            this.ComboBox_ID = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(85, 38);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 0;
            this.BtnCancel.Text = "キャンセル";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnOK
            // 
            this.BtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnOK.Location = new System.Drawing.Point(166, 38);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 1;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // ComboBox_ID
            // 
            this.ComboBox_ID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_ID.Location = new System.Drawing.Point(12, 12);
            this.ComboBox_ID.Name = "ComboBox_ID";
            this.ComboBox_ID.Size = new System.Drawing.Size(229, 20);
            this.ComboBox_ID.TabIndex = 2;
            // 
            // DialogChooseAirBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 68);
            this.Controls.Add(this.ComboBox_ID);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.BtnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogChooseAirBase";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "基地航空隊の選択";
            this.Shown += new System.EventHandler(this.DialogChooseAirBase_Shown);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.Button BtnOK;
		private System.Windows.Forms.ComboBox ComboBox_ID;
	}
}