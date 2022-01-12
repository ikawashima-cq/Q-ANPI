namespace ShelterInfoSystem
{
    partial class FormSendShelterInfo
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
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtShelterLongitude = new System.Windows.Forms.TextBox();
            this.txtShelterLatitude = new System.Windows.Forms.TextBox();
            this.txtShelterName = new System.Windows.Forms.TextBox();
            this.txtShelterID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtOpenClose = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtOpenDate = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtEntryNum = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(13, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "以下の避難所開設情報を ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.ForeColor = System.Drawing.Color.Lime;
            this.label3.Location = new System.Drawing.Point(188, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "送信 ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(226, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "します。 ";
            // 
            // txtShelterLongitude
            // 
            this.txtShelterLongitude.BackColor = System.Drawing.SystemColors.Control;
            this.txtShelterLongitude.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtShelterLongitude.Location = new System.Drawing.Point(113, 167);
            this.txtShelterLongitude.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtShelterLongitude.MaxLength = 10;
            this.txtShelterLongitude.Name = "txtShelterLongitude";
            this.txtShelterLongitude.ReadOnly = true;
            this.txtShelterLongitude.Size = new System.Drawing.Size(128, 28);
            this.txtShelterLongitude.TabIndex = 6;
            this.txtShelterLongitude.Text = "XXX.XXXXX";
            // 
            // txtShelterLatitude
            // 
            this.txtShelterLatitude.BackColor = System.Drawing.SystemColors.Control;
            this.txtShelterLatitude.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtShelterLatitude.Location = new System.Drawing.Point(113, 126);
            this.txtShelterLatitude.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtShelterLatitude.MaxLength = 10;
            this.txtShelterLatitude.Name = "txtShelterLatitude";
            this.txtShelterLatitude.ReadOnly = true;
            this.txtShelterLatitude.Size = new System.Drawing.Size(128, 28);
            this.txtShelterLatitude.TabIndex = 5;
            this.txtShelterLatitude.Text = "XX.XXXXX";
            // 
            // txtShelterName
            // 
            this.txtShelterName.BackColor = System.Drawing.SystemColors.Control;
            this.txtShelterName.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtShelterName.Location = new System.Drawing.Point(113, 86);
            this.txtShelterName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtShelterName.MaxLength = 25;
            this.txtShelterName.Name = "txtShelterName";
            this.txtShelterName.ReadOnly = true;
            this.txtShelterName.Size = new System.Drawing.Size(267, 28);
            this.txtShelterName.TabIndex = 4;
            this.txtShelterName.Text = "○○県○○市○○小学校";
            // 
            // txtShelterID
            // 
            this.txtShelterID.BackColor = System.Drawing.SystemColors.Control;
            this.txtShelterID.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtShelterID.Location = new System.Drawing.Point(113, 48);
            this.txtShelterID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtShelterID.MaxLength = 12;
            this.txtShelterID.Name = "txtShelterID";
            this.txtShelterID.ReadOnly = true;
            this.txtShelterID.Size = new System.Drawing.Size(128, 28);
            this.txtShelterID.TabIndex = 3;
            this.txtShelterID.Text = "999999999";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(29, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "経度";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(29, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 20);
            this.label6.TabIndex = 7;
            this.label6.Text = "緯度";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(29, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "避難所名";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(29, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "避難所ID";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnCancel.Font = new System.Drawing.Font("Meiryo UI", 15.75F);
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCancel.Location = new System.Drawing.Point(275, 340);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 38);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSend.Font = new System.Drawing.Font("Meiryo UI", 15.75F);
            this.btnSend.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSend.Location = new System.Drawing.Point(147, 340);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(120, 38);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "送信";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtOpenClose
            // 
            this.txtOpenClose.BackColor = System.Drawing.SystemColors.Control;
            this.txtOpenClose.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtOpenClose.Location = new System.Drawing.Point(113, 208);
            this.txtOpenClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOpenClose.MaxLength = 10;
            this.txtOpenClose.Name = "txtOpenClose";
            this.txtOpenClose.ReadOnly = true;
            this.txtOpenClose.Size = new System.Drawing.Size(128, 28);
            this.txtOpenClose.TabIndex = 7;
            this.txtOpenClose.Text = "開設";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(29, 211);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 20);
            this.label8.TabIndex = 15;
            this.label8.Text = "開設状態";
            // 
            // txtOpenDate
            // 
            this.txtOpenDate.BackColor = System.Drawing.SystemColors.Control;
            this.txtOpenDate.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtOpenDate.Location = new System.Drawing.Point(113, 248);
            this.txtOpenDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOpenDate.MaxLength = 20;
            this.txtOpenDate.Name = "txtOpenDate";
            this.txtOpenDate.ReadOnly = true;
            this.txtOpenDate.Size = new System.Drawing.Size(267, 28);
            this.txtOpenDate.TabIndex = 8;
            this.txtOpenDate.Text = "yyyy/mm/dd hh:mm";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(29, 251);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 20);
            this.label9.TabIndex = 17;
            this.label9.Text = "開設日時";
            // 
            // txtEntryNum
            // 
            this.txtEntryNum.BackColor = System.Drawing.SystemColors.Control;
            this.txtEntryNum.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtEntryNum.Location = new System.Drawing.Point(113, 290);
            this.txtEntryNum.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEntryNum.MaxLength = 16;
            this.txtEntryNum.Name = "txtEntryNum";
            this.txtEntryNum.ReadOnly = true;
            this.txtEntryNum.Size = new System.Drawing.Size(128, 28);
            this.txtEntryNum.TabIndex = 9;
            this.txtEntryNum.Text = "XXXXXX";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(29, 293);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 20);
            this.label10.TabIndex = 19;
            this.label10.Text = "避難者数";
            // 
            // FormSendShelterInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(409, 392);
            this.Controls.Add(this.txtEntryNum);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtOpenDate);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtOpenClose);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtShelterLongitude);
            this.Controls.Add(this.txtShelterLatitude);
            this.Controls.Add(this.txtShelterName);
            this.Controls.Add(this.txtShelterID);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FormSendShelterInfo";
            this.ShowIcon = false;
            this.Text = "避難所情報システム  －避難所開設情報送信";
            this.Load += new System.EventHandler(this.FormSendShelterInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtShelterLongitude;
        private System.Windows.Forms.TextBox txtShelterLatitude;
        private System.Windows.Forms.TextBox txtShelterName;
        private System.Windows.Forms.TextBox txtShelterID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtOpenClose;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtOpenDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtEntryNum;
        private System.Windows.Forms.Label label10;
    }
}