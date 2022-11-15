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
            this.checkBoxFleet1 = new System.Windows.Forms.CheckBox();
            this.checkBoxFleet2 = new System.Windows.Forms.CheckBox();
            this.checkBoxFleet3 = new System.Windows.Forms.CheckBox();
            this.checkBoxFleet4 = new System.Windows.Forms.CheckBox();
            this.groupBoxFleet = new System.Windows.Forms.GroupBox();
            this.labelAirBase = new System.Windows.Forms.Label();
            this.groupBoxFleet.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnCancel
            // 
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(85, 101);
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
            this.BtnOK.Location = new System.Drawing.Point(166, 101);
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
            this.ComboBox_ID.Location = new System.Drawing.Point(12, 75);
            this.ComboBox_ID.Name = "ComboBox_ID";
            this.ComboBox_ID.Size = new System.Drawing.Size(229, 20);
            this.ComboBox_ID.TabIndex = 2;
            // 
            // checkBoxFleet1
            // 
            this.checkBoxFleet1.AutoSize = true;
            this.checkBoxFleet1.Checked = true;
            this.checkBoxFleet1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFleet1.Location = new System.Drawing.Point(6, 18);
            this.checkBoxFleet1.Name = "checkBoxFleet1";
            this.checkBoxFleet1.Size = new System.Drawing.Size(36, 16);
            this.checkBoxFleet1.TabIndex = 3;
            this.checkBoxFleet1.Text = "#1";
            this.checkBoxFleet1.UseVisualStyleBackColor = true;
            // 
            // checkBoxFleet2
            // 
            this.checkBoxFleet2.AutoSize = true;
            this.checkBoxFleet2.Checked = true;
            this.checkBoxFleet2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFleet2.Location = new System.Drawing.Point(48, 18);
            this.checkBoxFleet2.Name = "checkBoxFleet2";
            this.checkBoxFleet2.Size = new System.Drawing.Size(36, 16);
            this.checkBoxFleet2.TabIndex = 4;
            this.checkBoxFleet2.Text = "#2";
            this.checkBoxFleet2.UseVisualStyleBackColor = true;
            // 
            // checkBoxFleet3
            // 
            this.checkBoxFleet3.AutoSize = true;
            this.checkBoxFleet3.Checked = true;
            this.checkBoxFleet3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFleet3.Location = new System.Drawing.Point(90, 18);
            this.checkBoxFleet3.Name = "checkBoxFleet3";
            this.checkBoxFleet3.Size = new System.Drawing.Size(36, 16);
            this.checkBoxFleet3.TabIndex = 5;
            this.checkBoxFleet3.Text = "#3";
            this.checkBoxFleet3.UseVisualStyleBackColor = true;
            // 
            // checkBoxFleet4
            // 
            this.checkBoxFleet4.AutoSize = true;
            this.checkBoxFleet4.Checked = true;
            this.checkBoxFleet4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFleet4.Location = new System.Drawing.Point(132, 18);
            this.checkBoxFleet4.Name = "checkBoxFleet4";
            this.checkBoxFleet4.Size = new System.Drawing.Size(36, 16);
            this.checkBoxFleet4.TabIndex = 6;
            this.checkBoxFleet4.Text = "#4";
            this.checkBoxFleet4.UseVisualStyleBackColor = true;
            // 
            // groupBoxFleet
            // 
            this.groupBoxFleet.Controls.Add(this.checkBoxFleet1);
            this.groupBoxFleet.Controls.Add(this.checkBoxFleet4);
            this.groupBoxFleet.Controls.Add(this.checkBoxFleet2);
            this.groupBoxFleet.Controls.Add(this.checkBoxFleet3);
            this.groupBoxFleet.Location = new System.Drawing.Point(12, 11);
            this.groupBoxFleet.Name = "groupBoxFleet";
            this.groupBoxFleet.Size = new System.Drawing.Size(225, 46);
            this.groupBoxFleet.TabIndex = 7;
            this.groupBoxFleet.TabStop = false;
            this.groupBoxFleet.Text = "艦隊";
            // 
            // labelAirBase
            // 
            this.labelAirBase.AutoSize = true;
            this.labelAirBase.Location = new System.Drawing.Point(16, 60);
            this.labelAirBase.Name = "labelAirBase";
            this.labelAirBase.Size = new System.Drawing.Size(65, 12);
            this.labelAirBase.TabIndex = 8;
            this.labelAirBase.Text = "基地航空隊";
            // 
            // DialogChooseAirBase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(249, 132);
            this.Controls.Add(this.labelAirBase);
            this.Controls.Add(this.groupBoxFleet);
            this.Controls.Add(this.ComboBox_ID);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.BtnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogChooseAirBase";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "艦隊と基地航空隊の選択";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.DialogChooseAirBase_Shown);
            this.groupBoxFleet.ResumeLayout(false);
            this.groupBoxFleet.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.Button BtnOK;
		private System.Windows.Forms.ComboBox ComboBox_ID;
		private System.Windows.Forms.CheckBox checkBoxFleet1;
		private System.Windows.Forms.CheckBox checkBoxFleet2;
		private System.Windows.Forms.CheckBox checkBoxFleet3;
		private System.Windows.Forms.CheckBox checkBoxFleet4;
		private System.Windows.Forms.GroupBox groupBoxFleet;
		private System.Windows.Forms.Label labelAirBase;
	}
}