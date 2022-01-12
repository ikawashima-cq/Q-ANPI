using System.Windows.Forms;
using System.Security.Permissions;
using System.ComponentModel;

namespace ShelterInfoSystem
{
    /// <summary>
    /// 任意の避難者数入力欄のコピペ制御専用クラス
    /// </summary>
    class FormEditPersonCnt : TextBox
    {
        protected override void WndProc(ref Message m)
        {
            // 任意の避難者数入力テキストボックスに貼り付け操作がされたとき
            // 数字以外を含む文字列の場合は貼り付け不可にする
            const int WM_PASTE = 0x302;
            if (m.Msg == WM_PASTE)
            {
                IDataObject iData = Clipboard.GetDataObject();
                //文字列がクリップボードにあるか
                if (iData != null && iData.GetDataPresent(DataFormats.Text))
                {
                    string clipStr = (string)iData.GetData(DataFormats.Text);
                    //クリップボードの文字列が数字のみか調べる
                    if (!System.Text.RegularExpressions.Regex.IsMatch(
                    clipStr,
                    @"^[0-9]+$"))
                    {
                        return;
                    }
                }
            }
            base.WndProc(ref m);
        }
    }

    partial class FormShelterInfo
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
            this.txtShelterID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtShelterName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtShelterPos = new System.Windows.Forms.TextBox();
            this.lblShelterStat = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnNewPersonal = new System.Windows.Forms.Button();
            this.listPersonal = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnEntryListPersonal = new System.Windows.Forms.Button();
            this.btnSendListPersonal = new System.Windows.Forms.Button();
            this.btnDeletePersonal = new System.Windows.Forms.Button();
            this.btnEditPersonal = new System.Windows.Forms.Button();
            this.btnSendPersonal = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.cbTextOnly = new System.Windows.Forms.CheckBox();
            this.textShelterInfo = new System.Windows.Forms.TextBox();
            this.btnSendTotalization = new System.Windows.Forms.Button();
            this.btnSendListTotalization = new System.Windows.Forms.Button();
            this.listTotalizationDetail = new System.Windows.Forms.ListView();
            this.columnHeader23 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader24 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader25 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader26 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblTextInfo = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.checkPersonCnt = new System.Windows.Forms.CheckBox();
            this.SendShelterMenuItem = new System.Windows.Forms.Button();
            this.listTotalization = new System.Windows.Forms.ListView();
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label17 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnType3 = new System.Windows.Forms.Button();
            this.btnOutputBinary = new System.Windows.Forms.Button();
            this.textMsg = new System.Windows.Forms.TextBox();
            this.listMsg = new System.Windows.Forms.ListView();
            this.columnHeader19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader20 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader21 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader22 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblShelterStatDate = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ShelterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RegisterShelterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditShelterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InfoClearMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditLocationInfoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SwitchShelterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChangeShelterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.開設状態ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenShelterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseShelterMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectSettingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnectTestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TcpSettingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AutoSendSettingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TermStatusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReceiveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InportFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImportQRCodeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogOffMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblUpdateDate = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.IdcConnect = new System.Windows.Forms.Panel();
            this.IdcTerm = new System.Windows.Forms.Panel();
            this.IdcTdma = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.IdcMessage = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.IdcAlart = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbl_RspCnt = new System.Windows.Forms.Label();
            this.lbl_ReqCnt = new System.Windows.Forms.Label();
            this.lbl_SndCnt = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label11 = new System.Windows.Forms.Label();
            this.lblShelterStatus = new System.Windows.Forms.Label();
            this.pnlShelterStatus = new System.Windows.Forms.Panel();
            this.txtMemo = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.selectShelterName = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbl_LastSendShelterName = new System.Windows.Forms.Label();
            this.textPersonCnt = new ShelterInfoSystem.FormEditPersonCnt();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlShelterStatus.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "避難所ID";
            // 
            // txtShelterID
            // 
            this.txtShelterID.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtShelterID.Location = new System.Drawing.Point(89, 32);
            this.txtShelterID.Margin = new System.Windows.Forms.Padding(4);
            this.txtShelterID.Name = "txtShelterID";
            this.txtShelterID.ReadOnly = true;
            this.txtShelterID.Size = new System.Drawing.Size(145, 27);
            this.txtShelterID.TabIndex = 1;
            this.txtShelterID.TabStop = false;
            this.txtShelterID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(270, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 19);
            this.label2.TabIndex = 0;
            this.label2.Text = "避難所名";
            // 
            // txtShelterName
            // 
            this.txtShelterName.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtShelterName.Location = new System.Drawing.Point(349, 33);
            this.txtShelterName.Margin = new System.Windows.Forms.Padding(4);
            this.txtShelterName.Name = "txtShelterName";
            this.txtShelterName.ReadOnly = true;
            this.txtShelterName.Size = new System.Drawing.Size(255, 27);
            this.txtShelterName.TabIndex = 2;
            this.txtShelterName.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(611, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 19);
            this.label3.TabIndex = 0;
            this.label3.Text = "緯度, 経度";
            // 
            // txtShelterPos
            // 
            this.txtShelterPos.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtShelterPos.Location = new System.Drawing.Point(698, 32);
            this.txtShelterPos.Margin = new System.Windows.Forms.Padding(4);
            this.txtShelterPos.Name = "txtShelterPos";
            this.txtShelterPos.ReadOnly = true;
            this.txtShelterPos.Size = new System.Drawing.Size(176, 27);
            this.txtShelterPos.TabIndex = 3;
            this.txtShelterPos.TabStop = false;
            // 
            // lblShelterStat
            // 
            this.lblShelterStat.AutoSize = true;
            this.lblShelterStat.BackColor = System.Drawing.Color.Transparent;
            this.lblShelterStat.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblShelterStat.Location = new System.Drawing.Point(3, 2);
            this.lblShelterStat.Name = "lblShelterStat";
            this.lblShelterStat.Size = new System.Drawing.Size(94, 19);
            this.lblShelterStat.TabIndex = 0;
            this.lblShelterStat.Text = "開設日時　：";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tabControl1.Location = new System.Drawing.Point(5, 112);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1217, 475);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 6;
            this.tabControl1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.btnNewPersonal);
            this.tabPage1.Controls.Add(this.listPersonal);
            this.tabPage1.Controls.Add(this.btnEntryListPersonal);
            this.tabPage1.Controls.Add(this.btnSendListPersonal);
            this.tabPage1.Controls.Add(this.btnDeletePersonal);
            this.tabPage1.Controls.Add(this.btnEditPersonal);
            this.tabPage1.Controls.Add(this.btnSendPersonal);
            this.tabPage1.Location = new System.Drawing.Point(4, 33);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1209, 438);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "個人安否情報";
            // 
            // btnNewPersonal
            // 
            this.btnNewPersonal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNewPersonal.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnNewPersonal.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnNewPersonal.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnNewPersonal.Location = new System.Drawing.Point(7, 395);
            this.btnNewPersonal.Margin = new System.Windows.Forms.Padding(4);
            this.btnNewPersonal.Name = "btnNewPersonal";
            this.btnNewPersonal.Size = new System.Drawing.Size(120, 38);
            this.btnNewPersonal.TabIndex = 1;
            this.btnNewPersonal.Text = "新規登録";
            this.btnNewPersonal.UseVisualStyleBackColor = false;
            this.btnNewPersonal.Click += new System.EventHandler(this.btnNewPersonal_Click);
            // 
            // listPersonal
            // 
            this.listPersonal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listPersonal.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader16,
            this.columnHeader17,
            this.columnHeader18,
            this.columnHeader15});
            this.listPersonal.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listPersonal.FullRowSelect = true;
            this.listPersonal.GridLines = true;
            this.listPersonal.Location = new System.Drawing.Point(3, 6);
            this.listPersonal.MultiSelect = false;
            this.listPersonal.Name = "listPersonal";
            this.listPersonal.OwnerDraw = true;
            this.listPersonal.Size = new System.Drawing.Size(1204, 385);
            this.listPersonal.TabIndex = 0;
            this.listPersonal.UseCompatibleStateImageBehavior = false;
            this.listPersonal.View = System.Windows.Forms.View.Details;
            this.listPersonal.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listPersonal_ColumnClick);
            this.listPersonal.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listPersonal_DrawColumnHeader);
            this.listPersonal.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listPersonal_DrawSubItem);
            this.listPersonal.SelectedIndexChanged += new System.EventHandler(this.listPersonal_SelectedIndexChanged);
            this.listPersonal.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listPersonal_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "No";
            this.columnHeader1.Width = 0;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "送信";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 70;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "氏名";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 140;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "電話番号";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 140;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "年齢";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "性別";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "入/退";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "公表";
            this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader8.Width = 51;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "住所";
            this.columnHeader9.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader9.Width = 260;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "ケガ";
            this.columnHeader10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "介護";
            this.columnHeader16.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "障がい";
            this.columnHeader17.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "妊産婦";
            this.columnHeader18.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "避難所内外";
            this.columnHeader15.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader15.Width = 100;
            // 
            // btnEntryListPersonal
            // 
            this.btnEntryListPersonal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEntryListPersonal.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnEntryListPersonal.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnEntryListPersonal.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnEntryListPersonal.Location = new System.Drawing.Point(392, 395);
            this.btnEntryListPersonal.Margin = new System.Windows.Forms.Padding(4);
            this.btnEntryListPersonal.Name = "btnEntryListPersonal";
            this.btnEntryListPersonal.Size = new System.Drawing.Size(120, 38);
            this.btnEntryListPersonal.TabIndex = 4;
            this.btnEntryListPersonal.Text = "登録履歴";
            this.btnEntryListPersonal.UseVisualStyleBackColor = false;
            this.btnEntryListPersonal.Click += new System.EventHandler(this.btnEntryListPersonal_Click);
            // 
            // btnSendListPersonal
            // 
            this.btnSendListPersonal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendListPersonal.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSendListPersonal.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSendListPersonal.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSendListPersonal.Location = new System.Drawing.Point(1086, 395);
            this.btnSendListPersonal.Margin = new System.Windows.Forms.Padding(4);
            this.btnSendListPersonal.Name = "btnSendListPersonal";
            this.btnSendListPersonal.Size = new System.Drawing.Size(120, 38);
            this.btnSendListPersonal.TabIndex = 6;
            this.btnSendListPersonal.Text = "送信履歴";
            this.btnSendListPersonal.UseVisualStyleBackColor = false;
            this.btnSendListPersonal.Click += new System.EventHandler(this.btnSendListPersonal_Click);
            // 
            // btnDeletePersonal
            // 
            this.btnDeletePersonal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeletePersonal.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnDeletePersonal.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDeletePersonal.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDeletePersonal.Location = new System.Drawing.Point(136, 395);
            this.btnDeletePersonal.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeletePersonal.Name = "btnDeletePersonal";
            this.btnDeletePersonal.Size = new System.Drawing.Size(120, 38);
            this.btnDeletePersonal.TabIndex = 2;
            this.btnDeletePersonal.Text = "削除";
            this.btnDeletePersonal.UseVisualStyleBackColor = false;
            this.btnDeletePersonal.Click += new System.EventHandler(this.btnDeletePersonal_Click);
            // 
            // btnEditPersonal
            // 
            this.btnEditPersonal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditPersonal.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnEditPersonal.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnEditPersonal.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnEditPersonal.Location = new System.Drawing.Point(264, 395);
            this.btnEditPersonal.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditPersonal.Name = "btnEditPersonal";
            this.btnEditPersonal.Size = new System.Drawing.Size(120, 38);
            this.btnEditPersonal.TabIndex = 3;
            this.btnEditPersonal.Text = "編集";
            this.btnEditPersonal.UseVisualStyleBackColor = false;
            this.btnEditPersonal.Click += new System.EventHandler(this.btnEditPersonal_Click);
            // 
            // btnSendPersonal
            // 
            this.btnSendPersonal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendPersonal.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSendPersonal.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSendPersonal.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSendPersonal.Location = new System.Drawing.Point(958, 395);
            this.btnSendPersonal.Margin = new System.Windows.Forms.Padding(4);
            this.btnSendPersonal.Name = "btnSendPersonal";
            this.btnSendPersonal.Size = new System.Drawing.Size(120, 38);
            this.btnSendPersonal.TabIndex = 5;
            this.btnSendPersonal.Text = "送信";
            this.btnSendPersonal.UseVisualStyleBackColor = false;
            this.btnSendPersonal.Click += new System.EventHandler(this.btnSendPersonal_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 33);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1209, 438);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "避難所情報";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.label18);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.label16);
            this.panel2.Controls.Add(this.cbTextOnly);
            this.panel2.Controls.Add(this.textShelterInfo);
            this.panel2.Controls.Add(this.btnSendTotalization);
            this.panel2.Controls.Add(this.btnSendListTotalization);
            this.panel2.Controls.Add(this.listTotalizationDetail);
            this.panel2.Controls.Add(this.lblTextInfo);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Location = new System.Drawing.Point(0, 129);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1209, 308);
            this.panel2.TabIndex = 27;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label18.Location = new System.Drawing.Point(410, 70);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(371, 34);
            this.label18.TabIndex = 25;
            this.label18.Text = "※各集計項目の集計値は避難者数と在宅者数を合計した数値です。\r\n※テキスト情報のみ送信する場合は最大22文字送信できます。";
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label14.Location = new System.Drawing.Point(7, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(1193, 2);
            this.label14.TabIndex = 24;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label16.Location = new System.Drawing.Point(409, 30);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(556, 40);
            this.label16.TabIndex = 25;
            this.label16.Text = "各集計項目の集計値と避難所状況に入力したテキスト情報を送信します。\r\nテキスト情報のみを送信する場合は「テキスト情報のみを送信する」をチェックして下さい。";
            // 
            // cbTextOnly
            // 
            this.cbTextOnly.AutoSize = true;
            this.cbTextOnly.Checked = true;
            this.cbTextOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTextOnly.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.cbTextOnly.Location = new System.Drawing.Point(795, 130);
            this.cbTextOnly.Name = "cbTextOnly";
            this.cbTextOnly.Size = new System.Drawing.Size(179, 24);
            this.cbTextOnly.TabIndex = 2;
            this.cbTextOnly.Text = "テキスト情報のみを送信";
            this.cbTextOnly.UseVisualStyleBackColor = true;
            this.cbTextOnly.CheckedChanged += new System.EventHandler(this.cbTextOnly_CheckedChanged);
            // 
            // textShelterInfo
            // 
            this.textShelterInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textShelterInfo.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textShelterInfo.Location = new System.Drawing.Point(414, 154);
            this.textShelterInfo.MaxLength = 13;
            this.textShelterInfo.Multiline = true;
            this.textShelterInfo.Name = "textShelterInfo";
            this.textShelterInfo.Size = new System.Drawing.Size(560, 103);
            this.textShelterInfo.TabIndex = 1;
            this.textShelterInfo.Text = "避難所詳細メッセージ　13文字";
            this.textShelterInfo.TextChanged += new System.EventHandler(this.textShelterInfo_TextChanged);
            // 
            // btnSendTotalization
            // 
            this.btnSendTotalization.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendTotalization.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSendTotalization.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSendTotalization.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSendTotalization.Location = new System.Drawing.Point(845, 264);
            this.btnSendTotalization.Margin = new System.Windows.Forms.Padding(4);
            this.btnSendTotalization.Name = "btnSendTotalization";
            this.btnSendTotalization.Size = new System.Drawing.Size(229, 38);
            this.btnSendTotalization.TabIndex = 3;
            this.btnSendTotalization.Text = "避難所詳細情報送信";
            this.btnSendTotalization.UseVisualStyleBackColor = false;
            this.btnSendTotalization.Click += new System.EventHandler(this.btnSendTotalization_Click);
            // 
            // btnSendListTotalization
            // 
            this.btnSendListTotalization.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendListTotalization.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSendListTotalization.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSendListTotalization.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSendListTotalization.Location = new System.Drawing.Point(1082, 264);
            this.btnSendListTotalization.Margin = new System.Windows.Forms.Padding(4);
            this.btnSendListTotalization.Name = "btnSendListTotalization";
            this.btnSendListTotalization.Size = new System.Drawing.Size(120, 38);
            this.btnSendListTotalization.TabIndex = 4;
            this.btnSendListTotalization.Text = "送信履歴";
            this.btnSendListTotalization.UseVisualStyleBackColor = false;
            this.btnSendListTotalization.Click += new System.EventHandler(this.btnSendListTotalization_Click);
            // 
            // listTotalizationDetail
            // 
            this.listTotalizationDetail.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader23,
            this.columnHeader24,
            this.columnHeader25,
            this.columnHeader26});
            this.listTotalizationDetail.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listTotalizationDetail.FullRowSelect = true;
            this.listTotalizationDetail.GridLines = true;
            this.listTotalizationDetail.Location = new System.Drawing.Point(21, 30);
            this.listTotalizationDetail.MultiSelect = false;
            this.listTotalizationDetail.Name = "listTotalizationDetail";
            this.listTotalizationDetail.OwnerDraw = true;
            this.listTotalizationDetail.Size = new System.Drawing.Size(366, 273);
            this.listTotalizationDetail.TabIndex = 0;
            this.listTotalizationDetail.UseCompatibleStateImageBehavior = false;
            this.listTotalizationDetail.View = System.Windows.Forms.View.Details;
            this.listTotalizationDetail.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listTotalization_DrawColumnHeader);
            this.listTotalizationDetail.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listTotalization_DrawSubItem);
            // 
            // columnHeader23
            // 
            this.columnHeader23.Text = "No";
            this.columnHeader23.Width = 0;
            // 
            // columnHeader24
            // 
            this.columnHeader24.Text = "送信";
            this.columnHeader24.Width = 0;
            // 
            // columnHeader25
            // 
            this.columnHeader25.Text = "集計項目";
            this.columnHeader25.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader25.Width = 180;
            // 
            // columnHeader26
            // 
            this.columnHeader26.Text = "集計";
            this.columnHeader26.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader26.Width = 180;
            // 
            // lblTextInfo
            // 
            this.lblTextInfo.AutoSize = true;
            this.lblTextInfo.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTextInfo.Location = new System.Drawing.Point(411, 131);
            this.lblTextInfo.Name = "lblTextInfo";
            this.lblTextInfo.Size = new System.Drawing.Size(197, 20);
            this.lblTextInfo.TabIndex = 0;
            this.lblTextInfo.Text = "避難所状況  (最大13文字)";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label15.Location = new System.Drawing.Point(17, 7);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(121, 20);
            this.label15.TabIndex = 0;
            this.label15.Text = "避難所詳細情報";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.checkPersonCnt);
            this.panel1.Controls.Add(this.SendShelterMenuItem);
            this.panel1.Controls.Add(this.listTotalization);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.textPersonCnt);
            this.panel1.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1208, 133);
            this.panel1.TabIndex = 26;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label13.Location = new System.Drawing.Point(757, 96);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(25, 20);
            this.label13.TabIndex = 23;
            this.label13.Text = "人";
            // 
            // checkPersonCnt
            // 
            this.checkPersonCnt.AutoSize = true;
            this.checkPersonCnt.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.checkPersonCnt.Location = new System.Drawing.Point(414, 95);
            this.checkPersonCnt.Name = "checkPersonCnt";
            this.checkPersonCnt.Size = new System.Drawing.Size(226, 24);
            this.checkPersonCnt.TabIndex = 1;
            this.checkPersonCnt.Text = "避難者数を任意の数値で送信";
            this.checkPersonCnt.UseVisualStyleBackColor = true;
            this.checkPersonCnt.CheckedChanged += new System.EventHandler(this.checkPersonCnt_CheckedChanged);
            // 
            // SendShelterMenuItem
            // 
            this.SendShelterMenuItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SendShelterMenuItem.BackColor = System.Drawing.Color.RoyalBlue;
            this.SendShelterMenuItem.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.SendShelterMenuItem.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.SendShelterMenuItem.Location = new System.Drawing.Point(1037, 83);
            this.SendShelterMenuItem.Margin = new System.Windows.Forms.Padding(4);
            this.SendShelterMenuItem.Name = "SendShelterMenuItem";
            this.SendShelterMenuItem.Size = new System.Drawing.Size(165, 38);
            this.SendShelterMenuItem.TabIndex = 3;
            this.SendShelterMenuItem.Text = "避難者数送信";
            this.SendShelterMenuItem.UseVisualStyleBackColor = false;
            this.SendShelterMenuItem.Click += new System.EventHandler(this.SendShelterMenuItem_Click);
            // 
            // listTotalization
            // 
            this.listTotalization.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14});
            this.listTotalization.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listTotalization.FullRowSelect = true;
            this.listTotalization.GridLines = true;
            this.listTotalization.Location = new System.Drawing.Point(21, 27);
            this.listTotalization.MultiSelect = false;
            this.listTotalization.Name = "listTotalization";
            this.listTotalization.OwnerDraw = true;
            this.listTotalization.Scrollable = false;
            this.listTotalization.Size = new System.Drawing.Size(366, 81);
            this.listTotalization.TabIndex = 0;
            this.listTotalization.UseCompatibleStateImageBehavior = false;
            this.listTotalization.View = System.Windows.Forms.View.Details;
            this.listTotalization.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.listTotalization_DrawColumnHeader);
            this.listTotalization.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listTotalization_DrawSubItem);
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "No";
            this.columnHeader11.Width = 0;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "送信";
            this.columnHeader12.Width = 0;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "集計項目";
            this.columnHeader13.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader13.Width = 180;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "集計";
            this.columnHeader14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader14.Width = 180;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label17.Location = new System.Drawing.Point(410, 27);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(482, 57);
            this.label17.TabIndex = 0;
            this.label17.Text = "集計項目の避難者数を送信します。\r\n任意の数値で送信する場合（在宅者数も含めて送信する場合など）は\r\n「避難者数を任意の数値で送信」をチェックしてテキストに数値を" +
    "入力して下さい。";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(17, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(153, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "避難者数＋在宅者数";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.flowLayoutPanel2);
            this.tabPage3.Controls.Add(this.textMsg);
            this.tabPage3.Controls.Add(this.listMsg);
            this.tabPage3.Location = new System.Drawing.Point(4, 33);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1209, 438);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "救助支援情報";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnType3);
            this.flowLayoutPanel2.Controls.Add(this.btnOutputBinary);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 385);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(1203, 50);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // btnType3
            // 
            this.btnType3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnType3.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnType3.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnType3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnType3.Location = new System.Drawing.Point(1079, 5);
            this.btnType3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnType3.Name = "btnType3";
            this.btnType3.Size = new System.Drawing.Size(120, 38);
            this.btnType3.TabIndex = 5;
            this.btnType3.Text = "新着確認";
            this.btnType3.UseVisualStyleBackColor = false;
            this.btnType3.Click += new System.EventHandler(this.btnType3_Click);
            // 
            // btnOutputBinary
            // 
            this.btnOutputBinary.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnOutputBinary.Font = new System.Drawing.Font("Meiryo UI", 15.75F);
            this.btnOutputBinary.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnOutputBinary.Location = new System.Drawing.Point(941, 5);
            this.btnOutputBinary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOutputBinary.Name = "btnOutputBinary";
            this.btnOutputBinary.Size = new System.Drawing.Size(130, 38);
            this.btnOutputBinary.TabIndex = 4;
            this.btnOutputBinary.Text = "バイナリ保存(現在未使用)";
            this.btnOutputBinary.UseVisualStyleBackColor = false;
            this.btnOutputBinary.Visible = false;
            this.btnOutputBinary.Click += new System.EventHandler(this.btnOutputBinary_Click);
            // 
            // textMsg
            // 
            this.textMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textMsg.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.textMsg.Location = new System.Drawing.Point(7, 212);
            this.textMsg.Multiline = true;
            this.textMsg.Name = "textMsg";
            this.textMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textMsg.Size = new System.Drawing.Size(1199, 169);
            this.textMsg.TabIndex = 2;
            // 
            // listMsg
            // 
            this.listMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listMsg.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader19,
            this.columnHeader20,
            this.columnHeader21,
            this.columnHeader22});
            this.listMsg.Cursor = System.Windows.Forms.Cursors.Default;
            this.listMsg.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listMsg.FullRowSelect = true;
            this.listMsg.GridLines = true;
            this.listMsg.Location = new System.Drawing.Point(5, 5);
            this.listMsg.MinimumSize = new System.Drawing.Size(200, 130);
            this.listMsg.Name = "listMsg";
            this.listMsg.Size = new System.Drawing.Size(1203, 201);
            this.listMsg.TabIndex = 1;
            this.listMsg.UseCompatibleStateImageBehavior = false;
            this.listMsg.View = System.Windows.Forms.View.Details;
            this.listMsg.SelectedIndexChanged += new System.EventHandler(this.listMsg_SelectedIndexChanged);
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "ID";
            // 
            // columnHeader20
            // 
            this.columnHeader20.Text = "日付";
            this.columnHeader20.Width = 212;
            // 
            // columnHeader21
            // 
            this.columnHeader21.Text = "種類";
            // 
            // columnHeader22
            // 
            this.columnHeader22.Text = "受信メッセージ";
            this.columnHeader22.Width = 867;
            // 
            // lblShelterStatDate
            // 
            this.lblShelterStatDate.AutoSize = true;
            this.lblShelterStatDate.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblShelterStatDate.Location = new System.Drawing.Point(103, 2);
            this.lblShelterStatDate.Name = "lblShelterStatDate";
            this.lblShelterStatDate.Size = new System.Drawing.Size(159, 19);
            this.lblShelterStatDate.TabIndex = 0;
            this.lblShelterStatDate.Text = "yyyy/mm/dd hh:mm";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShelterMenuItem,
            this.開設状態ToolStripMenuItem,
            this.ConnectSettingsMenuItem,
            this.TermStatusMenuItem,
            this.ReceiveMenuItem,
            this.ImportMenuItem,
            this.ExportMenuItem,
            this.LogOffMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(1240, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ShelterMenuItem
            // 
            this.ShelterMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RegisterShelterMenuItem,
            this.EditShelterMenuItem,
            this.InfoClearMenuItem,
            this.EditLocationInfoMenuItem,
            this.SwitchShelterMenuItem,
            this.ChangeShelterMenuItem});
            this.ShelterMenuItem.Name = "ShelterMenuItem";
            this.ShelterMenuItem.Size = new System.Drawing.Size(85, 21);
            this.ShelterMenuItem.Text = "避難所情報";
            // 
            // RegisterShelterMenuItem
            // 
            this.RegisterShelterMenuItem.Name = "RegisterShelterMenuItem";
            this.RegisterShelterMenuItem.Size = new System.Drawing.Size(180, 22);
            this.RegisterShelterMenuItem.Text = "登録";
            this.RegisterShelterMenuItem.Click += new System.EventHandler(this.RegisterShelterMenuItem_Click);
            // 
            // EditShelterMenuItem
            // 
            this.EditShelterMenuItem.Name = "EditShelterMenuItem";
            this.EditShelterMenuItem.Size = new System.Drawing.Size(180, 22);
            this.EditShelterMenuItem.Text = "編集";
            this.EditShelterMenuItem.Click += new System.EventHandler(this.EditShelterMenuItem_Click);
            // 
            // InfoClearMenuItem
            // 
            this.InfoClearMenuItem.Name = "InfoClearMenuItem";
            this.InfoClearMenuItem.Size = new System.Drawing.Size(180, 22);
            this.InfoClearMenuItem.Text = "情報クリア";
            this.InfoClearMenuItem.Click += new System.EventHandler(this.InfoClearMenuItem_Click);
            // 
            // EditLocationInfoMenuItem
            // 
            this.EditLocationInfoMenuItem.Name = "EditLocationInfoMenuItem";
            this.EditLocationInfoMenuItem.Size = new System.Drawing.Size(180, 22);
            this.EditLocationInfoMenuItem.Text = "位置情報更新";
            this.EditLocationInfoMenuItem.Click += new System.EventHandler(this.EditLocationInfoMenuItem_Click);
            // 
            // SwitchShelterMenuItem
            // 
            this.SwitchShelterMenuItem.Name = "SwitchShelterMenuItem";
            this.SwitchShelterMenuItem.Size = new System.Drawing.Size(180, 22);
            this.SwitchShelterMenuItem.Text = "避難所切替";
            this.SwitchShelterMenuItem.Click += new System.EventHandler(this.SwitchShelterMenuItem_Click);
            // 
            // ChangeShelterMenuItem
            // 
            this.ChangeShelterMenuItem.Name = "ChangeShelterMenuItem";
            this.ChangeShelterMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ChangeShelterMenuItem.Text = "個人安否情報修正";
            this.ChangeShelterMenuItem.Click += new System.EventHandler(this.ChangeShelterMenuItem_Click);
            // 
            // 開設状態ToolStripMenuItem
            // 
            this.開設状態ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenShelterMenuItem,
            this.CloseShelterMenuItem});
            this.開設状態ToolStripMenuItem.Name = "開設状態ToolStripMenuItem";
            this.開設状態ToolStripMenuItem.Size = new System.Drawing.Size(72, 21);
            this.開設状態ToolStripMenuItem.Text = "開設状態";
            // 
            // OpenShelterMenuItem
            // 
            this.OpenShelterMenuItem.Name = "OpenShelterMenuItem";
            this.OpenShelterMenuItem.Size = new System.Drawing.Size(102, 22);
            this.OpenShelterMenuItem.Text = "開設";
            this.OpenShelterMenuItem.Click += new System.EventHandler(this.OpenShelterMenuItem_Click);
            // 
            // CloseShelterMenuItem
            // 
            this.CloseShelterMenuItem.Name = "CloseShelterMenuItem";
            this.CloseShelterMenuItem.Size = new System.Drawing.Size(102, 22);
            this.CloseShelterMenuItem.Text = "閉鎖";
            this.CloseShelterMenuItem.Click += new System.EventHandler(this.CloseShelterMenuItem_Click);
            // 
            // ConnectSettingsMenuItem
            // 
            this.ConnectSettingsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConnectSettingMenuItem,
            this.ConnectTestMenuItem,
            this.TcpSettingMenuItem,
            this.AutoSendSettingMenuItem});
            this.ConnectSettingsMenuItem.Name = "ConnectSettingsMenuItem";
            this.ConnectSettingsMenuItem.Size = new System.Drawing.Size(72, 21);
            this.ConnectSettingsMenuItem.Text = "通信設定";
            // 
            // ConnectSettingMenuItem
            // 
            this.ConnectSettingMenuItem.Name = "ConnectSettingMenuItem";
            this.ConnectSettingMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ConnectSettingMenuItem.Text = "通信端末接続";
            this.ConnectSettingMenuItem.Click += new System.EventHandler(this.ConnectSettingMenuItem_Click);
            // 
            // ConnectTestMenuItem
            // 
            this.ConnectTestMenuItem.Name = "ConnectTestMenuItem";
            this.ConnectTestMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ConnectTestMenuItem.Text = "送受信テスト";
            this.ConnectTestMenuItem.Click += new System.EventHandler(this.ConnectTestMenuItem_Click);
            // 
            // TcpSettingMenuItem
            // 
            this.TcpSettingMenuItem.Name = "TcpSettingMenuItem";
            this.TcpSettingMenuItem.Size = new System.Drawing.Size(180, 22);
            this.TcpSettingMenuItem.Text = "安否情報受信設定";
            this.TcpSettingMenuItem.Click += new System.EventHandler(this.TcpSettingMenuItem_Click);
            // 
            // AutoSendSettingMenuItem
            // 
            this.AutoSendSettingMenuItem.Name = "AutoSendSettingMenuItem";
            this.AutoSendSettingMenuItem.Size = new System.Drawing.Size(180, 22);
            this.AutoSendSettingMenuItem.Text = "自動送信設定";
            this.AutoSendSettingMenuItem.Click += new System.EventHandler(this.AutoSendSettingMenuItem_Click);
            // 
            // TermStatusMenuItem
            // 
            this.TermStatusMenuItem.Name = "TermStatusMenuItem";
            this.TermStatusMenuItem.Size = new System.Drawing.Size(72, 21);
            this.TermStatusMenuItem.Text = "装置状態";
            this.TermStatusMenuItem.Click += new System.EventHandler(this.TermStatusMenuItem_Click);
            // 
            // ReceiveMenuItem
            // 
            this.ReceiveMenuItem.Name = "ReceiveMenuItem";
            this.ReceiveMenuItem.Size = new System.Drawing.Size(72, 21);
            this.ReceiveMenuItem.Text = "受信情報";
            this.ReceiveMenuItem.Visible = false;
            this.ReceiveMenuItem.Click += new System.EventHandler(this.ReceiveMenuItem_Click);
            // 
            // ImportMenuItem
            // 
            this.ImportMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.InportFileMenuItem,
            this.ImportQRCodeMenuItem});
            this.ImportMenuItem.Name = "ImportMenuItem";
            this.ImportMenuItem.Size = new System.Drawing.Size(70, 21);
            this.ImportMenuItem.Text = "インポート";
            // 
            // InportFileMenuItem
            // 
            this.InportFileMenuItem.Name = "InportFileMenuItem";
            this.InportFileMenuItem.Size = new System.Drawing.Size(159, 22);
            this.InportFileMenuItem.Text = "ファイル選択";
            this.InportFileMenuItem.Click += new System.EventHandler(this.ImportMenuItem_Click);
            // 
            // ImportQRCodeMenuItem
            // 
            this.ImportQRCodeMenuItem.Name = "ImportQRCodeMenuItem";
            this.ImportQRCodeMenuItem.Size = new System.Drawing.Size(159, 22);
            this.ImportQRCodeMenuItem.Text = "受信データ確認";
            this.ImportQRCodeMenuItem.Click += new System.EventHandler(this.ImportQRCodeMenuItem_Click);
            // 
            // ExportMenuItem
            // 
            this.ExportMenuItem.Name = "ExportMenuItem";
            this.ExportMenuItem.Size = new System.Drawing.Size(80, 21);
            this.ExportMenuItem.Text = "エクスポート";
            this.ExportMenuItem.Click += new System.EventHandler(this.ExportMenuItemClick);
            // 
            // LogOffMenuItem
            // 
            this.LogOffMenuItem.Name = "LogOffMenuItem";
            this.LogOffMenuItem.Size = new System.Drawing.Size(68, 21);
            this.LogOffMenuItem.Text = "ログアウト";
            this.LogOffMenuItem.Click += new System.EventHandler(this.LogOffMenuItem_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnUpdate.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnUpdate.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnUpdate.Location = new System.Drawing.Point(1093, 94);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(120, 38);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.Text = "最新";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblUpdateDate
            // 
            this.lblUpdateDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUpdateDate.AutoSize = true;
            this.lblUpdateDate.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUpdateDate.Location = new System.Drawing.Point(880, 113);
            this.lblUpdateDate.Name = "lblUpdateDate";
            this.lblUpdateDate.Size = new System.Drawing.Size(208, 15);
            this.lblUpdateDate.TabIndex = 0;
            this.lblUpdateDate.Text = "更新日時：yyyy/mm/dd hh:mm:ss";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel8,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5,
            this.toolStripStatusLabel6,
            this.toolStripStatusLabel7});
            this.statusStrip1.Location = new System.Drawing.Point(0, 622);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1240, 23);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Size = new System.Drawing.Size(89, 18);
            this.toolStripStatusLabel8.Text = "時刻未同期    |";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(134, 18);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 18);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(0, 18);
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(0, 18);
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(0, 18);
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(0, 18);
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(0, 18);
            // 
            // IdcConnect
            // 
            this.IdcConnect.BackColor = System.Drawing.Color.Lime;
            this.IdcConnect.Location = new System.Drawing.Point(7, 3);
            this.IdcConnect.Margin = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.IdcConnect.Name = "IdcConnect";
            this.IdcConnect.Size = new System.Drawing.Size(30, 20);
            this.IdcConnect.TabIndex = 22;
            // 
            // IdcTerm
            // 
            this.IdcTerm.BackColor = System.Drawing.Color.Lime;
            this.IdcTerm.Location = new System.Drawing.Point(139, 3);
            this.IdcTerm.Margin = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.IdcTerm.Name = "IdcTerm";
            this.IdcTerm.Size = new System.Drawing.Size(30, 20);
            this.IdcTerm.TabIndex = 21;
            // 
            // IdcTdma
            // 
            this.IdcTdma.BackColor = System.Drawing.Color.Lime;
            this.IdcTdma.Location = new System.Drawing.Point(51, 3);
            this.IdcTdma.Margin = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.IdcTdma.Name = "IdcTdma";
            this.IdcTdma.Size = new System.Drawing.Size(30, 20);
            this.IdcTdma.TabIndex = 21;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(139, 0);
            this.label10.Margin = new System.Windows.Forms.Padding(7, 0, 8, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 12);
            this.label10.TabIndex = 4;
            this.label10.Text = "装置";
            // 
            // IdcMessage
            // 
            this.IdcMessage.BackColor = System.Drawing.Color.RoyalBlue;
            this.IdcMessage.Location = new System.Drawing.Point(95, 3);
            this.IdcMessage.Margin = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.IdcMessage.Name = "IdcMessage";
            this.IdcMessage.Size = new System.Drawing.Size(30, 20);
            this.IdcMessage.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(978, 70);
            this.label9.Margin = new System.Windows.Forms.Padding(7, 0, 8, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 3;
            this.label9.Text = "災危";
            this.label9.Visible = false;
            // 
            // IdcAlart
            // 
            this.IdcAlart.BackColor = System.Drawing.Color.RoyalBlue;
            this.IdcAlart.Location = new System.Drawing.Point(977, 45);
            this.IdcAlart.Margin = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.IdcAlart.Name = "IdcAlart";
            this.IdcAlart.Size = new System.Drawing.Size(30, 20);
            this.IdcAlart.TabIndex = 5;
            this.IdcAlart.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(95, 0);
            this.label8.Margin = new System.Windows.Forms.Padding(7, 0, 8, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "情報";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(51, 0);
            this.label7.Margin = new System.Windows.Forms.Padding(7, 0, 8, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "同期";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(7, 0);
            this.label6.Margin = new System.Windows.Forms.Padding(7, 0, 8, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "接続";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "送信状況：";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 147F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 147F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 500F));
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_RspCnt, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_ReqCnt, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_SndCnt, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 602);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(869, 18);
            this.tableLayoutPanel1.TabIndex = 21;
            // 
            // lbl_RspCnt
            // 
            this.lbl_RspCnt.AutoSize = true;
            this.lbl_RspCnt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_RspCnt.Location = new System.Drawing.Point(372, 0);
            this.lbl_RspCnt.Name = "lbl_RspCnt";
            this.lbl_RspCnt.Size = new System.Drawing.Size(420, 15);
            this.lbl_RspCnt.TabIndex = 3;
            this.lbl_RspCnt.Text = "rsp:(000000/000000) 受信:(000000) 未受信:(000000) 受信合計:(000000)";
            // 
            // lbl_ReqCnt
            // 
            this.lbl_ReqCnt.AutoSize = true;
            this.lbl_ReqCnt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_ReqCnt.Location = new System.Drawing.Point(78, 0);
            this.lbl_ReqCnt.Name = "lbl_ReqCnt";
            this.lbl_ReqCnt.Size = new System.Drawing.Size(137, 15);
            this.lbl_ReqCnt.TabIndex = 1;
            this.lbl_ReqCnt.Text = "req:(000000/0000000)";
            // 
            // lbl_SndCnt
            // 
            this.lbl_SndCnt.AutoSize = true;
            this.lbl_SndCnt.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_SndCnt.Location = new System.Drawing.Point(225, 0);
            this.lbl_SndCnt.Name = "lbl_SndCnt";
            this.lbl_SndCnt.Size = new System.Drawing.Size(131, 15);
            this.lbl_SndCnt.TabIndex = 2;
            this.lbl_SndCnt.Text = "snd:(000000/000000)";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "csv";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label11.Location = new System.Drawing.Point(3, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 19);
            this.label11.TabIndex = 0;
            this.label11.Text = "開設状態　：";
            // 
            // lblShelterStatus
            // 
            this.lblShelterStatus.AutoSize = true;
            this.lblShelterStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblShelterStatus.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblShelterStatus.Location = new System.Drawing.Point(103, 22);
            this.lblShelterStatus.Name = "lblShelterStatus";
            this.lblShelterStatus.Size = new System.Drawing.Size(128, 19);
            this.lblShelterStatus.TabIndex = 0;
            this.lblShelterStatus.Text = "開設/閉鎖/未開設";
            // 
            // pnlShelterStatus
            // 
            this.pnlShelterStatus.BackColor = System.Drawing.Color.Orange;
            this.pnlShelterStatus.Controls.Add(this.label11);
            this.pnlShelterStatus.Controls.Add(this.lblShelterStat);
            this.pnlShelterStatus.Controls.Add(this.lblShelterStatDate);
            this.pnlShelterStatus.Controls.Add(this.lblShelterStatus);
            this.pnlShelterStatus.Location = new System.Drawing.Point(9, 61);
            this.pnlShelterStatus.Name = "pnlShelterStatus";
            this.pnlShelterStatus.Size = new System.Drawing.Size(278, 45);
            this.pnlShelterStatus.TabIndex = 23;
            // 
            // txtMemo
            // 
            this.txtMemo.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtMemo.Location = new System.Drawing.Point(349, 63);
            this.txtMemo.MaxLength = 200;
            this.txtMemo.Multiline = true;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.ReadOnly = true;
            this.txtMemo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMemo.Size = new System.Drawing.Size(525, 43);
            this.txtMemo.TabIndex = 4;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label12.Location = new System.Drawing.Point(309, 63);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(30, 19);
            this.label12.TabIndex = 0;
            this.label12.Text = "メモ";
            // 
            // selectShelterName
            // 
            this.selectShelterName.DropDownHeight = 400;
            this.selectShelterName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selectShelterName.Font = new System.Drawing.Font("Meiryo UI", 11.25F);
            this.selectShelterName.FormattingEnabled = true;
            this.selectShelterName.IntegralHeight = false;
            this.selectShelterName.Location = new System.Drawing.Point(349, 33);
            this.selectShelterName.Name = "selectShelterName";
            this.selectShelterName.Size = new System.Drawing.Size(255, 27);
            this.selectShelterName.TabIndex = 2;
            this.selectShelterName.Visible = false;
            this.selectShelterName.SelectedIndexChanged += new System.EventHandler(this.selectShelterName_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.IdcConnect);
            this.flowLayoutPanel1.Controls.Add(this.IdcTdma);
            this.flowLayoutPanel1.Controls.Add(this.IdcMessage);
            this.flowLayoutPanel1.Controls.Add(this.IdcTerm);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(183, 27);
            this.flowLayoutPanel1.TabIndex = 25;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.label6);
            this.flowLayoutPanel3.Controls.Add(this.label7);
            this.flowLayoutPanel3.Controls.Add(this.label8);
            this.flowLayoutPanel3.Controls.Add(this.label10);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 36);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(183, 12);
            this.flowLayoutPanel3.TabIndex = 25;
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel4.Controls.Add(this.flowLayoutPanel1);
            this.flowLayoutPanel4.Controls.Add(this.flowLayoutPanel3);
            this.flowLayoutPanel4.Location = new System.Drawing.Point(1, 1);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(190, 51);
            this.flowLayoutPanel4.TabIndex = 25;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel3.Controls.Add(this.flowLayoutPanel4);
            this.panel3.Location = new System.Drawing.Point(1030, 35);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(192, 53);
            this.panel3.TabIndex = 26;
            // 
            // lbl_LastSendShelterName
            // 
            this.lbl_LastSendShelterName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_LastSendShelterName.AutoSize = true;
            this.lbl_LastSendShelterName.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lbl_LastSendShelterName.Location = new System.Drawing.Point(880, 602);
            this.lbl_LastSendShelterName.Name = "lbl_LastSendShelterName";
            this.lbl_LastSendShelterName.Size = new System.Drawing.Size(188, 15);
            this.lbl_LastSendShelterName.TabIndex = 0;
            this.lbl_LastSendShelterName.Text = "(デバッグ用)最後に送信した避難所名";
            this.lbl_LastSendShelterName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl_LastSendShelterName.Visible = false;
            // 
            // textPersonCnt
            // 
            this.textPersonCnt.Enabled = false;
            this.textPersonCnt.Location = new System.Drawing.Point(646, 94);
            this.textPersonCnt.MaxLength = 6;
            this.textPersonCnt.Name = "textPersonCnt";
            this.textPersonCnt.Size = new System.Drawing.Size(105, 27);
            this.textPersonCnt.TabIndex = 2;
            this.textPersonCnt.TextChanged += new System.EventHandler(this.textShelterInfo_TextChanged);
            this.textPersonCnt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textPersonCnt_KeyPress);
            // 
            // FormShelterInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1240, 645);
            this.ControlBox = false;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.txtMemo);
            this.Controls.Add(this.IdcAlart);
            this.Controls.Add(this.pnlShelterStatus);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.selectShelterName);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lbl_LastSendShelterName);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.txtShelterPos);
            this.Controls.Add(this.txtShelterName);
            this.Controls.Add(this.txtShelterID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblUpdateDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1157, 651);
            this.Name = "FormShelterInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "避難所情報システム  －避難所管理";
            this.Load += new System.EventHandler(this.FormShelterInfo_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.pnlShelterStatus.ResumeLayout(false);
            this.pnlShelterStatus.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtShelterID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtShelterName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtShelterPos;
        private System.Windows.Forms.Label lblShelterStat;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView listPersonal;
        private System.Windows.Forms.Button btnEntryListPersonal;
        private System.Windows.Forms.Button btnSendListPersonal;
        private System.Windows.Forms.Button btnDeletePersonal;
        private System.Windows.Forms.Button btnEditPersonal;
        private System.Windows.Forms.Button btnSendPersonal;
        private System.Windows.Forms.Label lblTextInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textShelterInfo;
        private System.Windows.Forms.Button btnSendTotalization;
        private System.Windows.Forms.Button btnSendListTotalization;
        private System.Windows.Forms.ListView listTotalization;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.ColumnHeader columnHeader17;
        private System.Windows.Forms.ColumnHeader columnHeader18;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.Label lblShelterStatDate;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ShelterMenuItem;
        private System.Windows.Forms.ToolStripMenuItem InfoClearMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LogOffMenuItem;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lblUpdateDate;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btnNewPersonal;
        private System.Windows.Forms.ToolStripMenuItem ImportMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConnectSettingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReceiveMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Panel IdcConnect;
        private System.Windows.Forms.Panel IdcTdma;
        private System.Windows.Forms.Panel IdcMessage;
        private System.Windows.Forms.Panel IdcAlart;
        private System.Windows.Forms.Panel IdcTerm;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ToolStripMenuItem TermStatusMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripMenuItem ConnectSettingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConnectTestMenuItem;
        private System.Windows.Forms.CheckBox cbTextOnly;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lbl_ReqCnt;
        private System.Windows.Forms.Label lbl_SndCnt;
        private System.Windows.Forms.Label lbl_RspCnt;
        private System.Windows.Forms.ToolStripMenuItem ExportMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ToolStripMenuItem TcpSettingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RegisterShelterMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditShelterMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AutoSendSettingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditLocationInfoMenuItem;
        private System.Windows.Forms.ToolStripMenuItem InportFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImportQRCodeMenuItem;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblShelterStatus;
        private System.Windows.Forms.Panel pnlShelterStatus;
        private System.Windows.Forms.TextBox txtMemo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ToolStripMenuItem SwitchShelterMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 開設状態ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenShelterMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CloseShelterMenuItem;
        private System.Windows.Forms.ComboBox selectShelterName;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView listMsg;
        private System.Windows.Forms.ColumnHeader columnHeader19;
        private System.Windows.Forms.ColumnHeader columnHeader20;
        private System.Windows.Forms.ColumnHeader columnHeader21;
        private System.Windows.Forms.ColumnHeader columnHeader22;
        private System.Windows.Forms.TextBox textMsg;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnOutputBinary;
        private System.Windows.Forms.Button btnType3;
        private System.Windows.Forms.Button SendShelterMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ChangeShelterMenuItem;
        private System.Windows.Forms.CheckBox checkPersonCnt;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ListView listTotalizationDetail;
        private System.Windows.Forms.ColumnHeader columnHeader23;
        private System.Windows.Forms.ColumnHeader columnHeader24;
        private System.Windows.Forms.ColumnHeader columnHeader25;
        private System.Windows.Forms.ColumnHeader columnHeader26;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private FormEditPersonCnt textPersonCnt;
        private Label lbl_LastSendShelterName;
        private ToolStripStatusLabel toolStripStatusLabel8;
    }
}