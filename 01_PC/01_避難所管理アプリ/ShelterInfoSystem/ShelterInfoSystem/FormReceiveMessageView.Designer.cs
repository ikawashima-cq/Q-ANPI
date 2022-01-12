namespace ShelterInfoSystem
{
    partial class FormReceiveMessageView
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("安否支援情報");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("災害危機情報");
            this.listMsgCategory = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listMsg = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textMsg = new System.Windows.Forms.TextBox();
            this.btnOutputBinary = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnType3 = new System.Windows.Forms.Button();
            this.btnIsReceive = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // listMsgCategory
            // 
            this.listMsgCategory.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listMsgCategory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listMsgCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listMsgCategory.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listMsgCategory.FullRowSelect = true;
            this.listMsgCategory.GridLines = true;
            this.listMsgCategory.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listMsgCategory.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.listMsgCategory.Location = new System.Drawing.Point(5, 5);
            this.listMsgCategory.Margin = new System.Windows.Forms.Padding(0);
            this.listMsgCategory.MultiSelect = false;
            this.listMsgCategory.Name = "listMsgCategory";
            this.listMsgCategory.OwnerDraw = true;
            this.listMsgCategory.Size = new System.Drawing.Size(200, 362);
            this.listMsgCategory.TabIndex = 0;
            this.listMsgCategory.UseCompatibleStateImageBehavior = false;
            this.listMsgCategory.View = System.Windows.Forms.View.Details;
            this.listMsgCategory.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listView_DrawColumnHeader);
            this.listMsgCategory.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listView_DrawSubItem);
            this.listMsgCategory.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listMsgCategory_ItemSelectionChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "受信メッセージ分類";
            this.columnHeader1.Width = 140;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnClose.Font = new System.Drawing.Font("Meiryo UI", 15.75F);
            this.btnClose.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnClose.Location = new System.Drawing.Point(712, 5);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 38);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listMsgCategory);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(210, 372);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.listMsg);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(626, 143);
            this.panel2.TabIndex = 4;
            // 
            // listMsg
            // 
            this.listMsg.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader5,
            this.columnHeader4});
            this.listMsg.Cursor = System.Windows.Forms.Cursors.Default;
            this.listMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listMsg.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listMsg.FullRowSelect = true;
            this.listMsg.GridLines = true;
            this.listMsg.Location = new System.Drawing.Point(5, 5);
            this.listMsg.MinimumSize = new System.Drawing.Size(200, 130);
            this.listMsg.Name = "listMsg";
            this.listMsg.Size = new System.Drawing.Size(616, 133);
            this.listMsg.TabIndex = 0;
            this.listMsg.UseCompatibleStateImageBehavior = false;
            this.listMsg.View = System.Windows.Forms.View.Details;
            this.listMsg.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listView_DrawColumnHeader);
            this.listMsg.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listView_DrawSubItem);
            this.listMsg.SelectedIndexChanged += new System.EventHandler(this.listMsg_SelectedIndexChanged);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "ID";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "日付";
            this.columnHeader3.Width = 212;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "種類";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "受信メッセージ";
            this.columnHeader4.Width = 290;
            // 
            // textMsg
            // 
            this.textMsg.BackColor = System.Drawing.SystemColors.Window;
            this.textMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textMsg.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textMsg.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textMsg.Location = new System.Drawing.Point(5, 5);
            this.textMsg.Multiline = true;
            this.textMsg.Name = "textMsg";
            this.textMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textMsg.Size = new System.Drawing.Size(616, 219);
            this.textMsg.TabIndex = 0;
            // 
            // btnOutputBinary
            // 
            this.btnOutputBinary.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnOutputBinary.Font = new System.Drawing.Font("Meiryo UI", 15.75F);
            this.btnOutputBinary.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnOutputBinary.Location = new System.Drawing.Point(574, 5);
            this.btnOutputBinary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOutputBinary.Name = "btnOutputBinary";
            this.btnOutputBinary.Size = new System.Drawing.Size(130, 38);
            this.btnOutputBinary.TabIndex = 4;
            this.btnOutputBinary.Text = "バイナリ保存";
            this.btnOutputBinary.UseVisualStyleBackColor = false;
            this.btnOutputBinary.Visible = false;
            this.btnOutputBinary.Click += new System.EventHandler(this.btnOutputBinary_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnClose);
            this.flowLayoutPanel1.Controls.Add(this.btnOutputBinary);
            this.flowLayoutPanel1.Controls.Add(this.btnType3);
            this.flowLayoutPanel1.Controls.Add(this.btnIsReceive);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(5, 377);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(836, 50);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // btnType3
            // 
            this.btnType3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnType3.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnType3.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnType3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnType3.Location = new System.Drawing.Point(446, 5);
            this.btnType3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnType3.Name = "btnType3";
            this.btnType3.Size = new System.Drawing.Size(120, 38);
            this.btnType3.TabIndex = 5;
            this.btnType3.Text = "新着確認";
            this.btnType3.UseVisualStyleBackColor = false;
            this.btnType3.Click += new System.EventHandler(this.btnType3_Click);
            // 
            // btnIsReceive
            // 
            this.btnIsReceive.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnIsReceive.AutoSize = true;
            this.btnIsReceive.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnIsReceive.Checked = true;
            this.btnIsReceive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnIsReceive.Font = new System.Drawing.Font("Meiryo UI", 15.75F);
            this.btnIsReceive.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnIsReceive.Location = new System.Drawing.Point(180, 53);
            this.btnIsReceive.Margin = new System.Windows.Forms.Padding(3, 5, 550, 3);
            this.btnIsReceive.Name = "btnIsReceive";
            this.btnIsReceive.Size = new System.Drawing.Size(106, 36);
            this.btnIsReceive.TabIndex = 6;
            this.btnIsReceive.Text = "受信停止";
            this.btnIsReceive.UseVisualStyleBackColor = false;
            this.btnIsReceive.Visible = false;
            this.btnIsReceive.CheckedChanged += new System.EventHandler(this.btnIsReceive_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(5, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(836, 372);
            this.panel3.TabIndex = 6;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.panel2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(210, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(626, 372);
            this.panel5.TabIndex = 6;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.textMsg);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 143);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(5);
            this.panel6.Size = new System.Drawing.Size(626, 229);
            this.panel6.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Meiryo UI", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(246, 176);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(355, 81);
            this.label8.TabIndex = 23;
            this.label8.Text = "現在未使用";
            // 
            // FormReceiveMessageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 432);
            this.ControlBox = false;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "FormReceiveMessageView";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "受信情報";
            this.Load += new System.EventHandler(this.FormReceiveMessageView_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listMsgCategory;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListView listMsg;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox textMsg;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button btnOutputBinary;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnType3;
        private System.Windows.Forms.CheckBox btnIsReceive;
        private System.Windows.Forms.Label label8;
    }
}