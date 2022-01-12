namespace ShelterInfoSystem
{
    partial class FormSubGHzConnectSettings
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
            this.txtDstId = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDstId = new System.Windows.Forms.Label();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDeviceSearch = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cmbComPort = new System.Windows.Forms.ComboBox();
            this.btnSetComPort = new System.Windows.Forms.Button();
            this.btnDisableComPort = new System.Windows.Forms.Button();
            this.cmbChannel = new System.Windows.Forms.ComboBox();
            this.btnSetChannel = new System.Windows.Forms.Button();
            this.txtSrcId = new System.Windows.Forms.TextBox();
            this.btnGetSrcId = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblComPort = new System.Windows.Forms.Label();
            this.lblChannel = new System.Windows.Forms.Label();
            this.lblSrcId = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel8 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClear = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtTestResult = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tipsConnectSettings = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.flowLayoutPanel7.SuspendLayout();
            this.flowLayoutPanel6.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.flowLayoutPanel8.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSaveSettings);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 279);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(817, 48);
            this.panel1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnClose.Font = new System.Drawing.Font("Meiryo UI", 15.75F);
            this.btnClose.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnClose.Location = new System.Drawing.Point(691, 5);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 38);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSettings.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSaveSettings.Font = new System.Drawing.Font("Meiryo UI", 15.75F);
            this.btnSaveSettings.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSaveSettings.Location = new System.Drawing.Point(557, 5);
            this.btnSaveSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(120, 38);
            this.btnSaveSettings.TabIndex = 0;
            this.btnSaveSettings.Text = "設定保存";
            this.btnSaveSettings.UseVisualStyleBackColor = false;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(474, 279);
            this.panel2.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(464, 269);
            this.groupBox1.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.flowLayoutPanel7);
            this.groupBox4.Controls.Add(this.flowLayoutPanel6);
            this.groupBox4.Controls.Add(this.flowLayoutPanel4);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(0, 149);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(464, 121);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "接続先設定";
            // 
            // flowLayoutPanel7
            // 
            this.flowLayoutPanel7.Controls.Add(this.txtDstId);
            this.flowLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel7.Location = new System.Drawing.Point(137, 24);
            this.flowLayoutPanel7.Name = "flowLayoutPanel7";
            this.flowLayoutPanel7.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel7.Size = new System.Drawing.Size(324, 42);
            this.flowLayoutPanel7.TabIndex = 0;
            // 
            // txtDstId
            // 
            this.txtDstId.Location = new System.Drawing.Point(8, 8);
            this.txtDstId.MaximumSize = new System.Drawing.Size(380, 28);
            this.txtDstId.MaxLength = 32;
            this.txtDstId.MinimumSize = new System.Drawing.Size(200, 28);
            this.txtDstId.Name = "txtDstId";
            this.txtDstId.Size = new System.Drawing.Size(200, 28);
            this.txtDstId.TabIndex = 0;
            this.tipsConnectSettings.SetToolTip(this.txtDstId, "接続先モジュールのデバイスIDを入力してください");
            this.txtDstId.TextChanged += new System.EventHandler(this.txtDstId_TextChanged);
            // 
            // flowLayoutPanel6
            // 
            this.flowLayoutPanel6.Controls.Add(this.lblDstId);
            this.flowLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel6.Location = new System.Drawing.Point(3, 24);
            this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.flowLayoutPanel6.Name = "flowLayoutPanel6";
            this.flowLayoutPanel6.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel6.Size = new System.Drawing.Size(128, 42);
            this.flowLayoutPanel6.TabIndex = 0;
            // 
            // lblDstId
            // 
            this.lblDstId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDstId.AutoSize = true;
            this.lblDstId.Location = new System.Drawing.Point(8, 8);
            this.lblDstId.Margin = new System.Windows.Forms.Padding(3, 3, 3, 7);
            this.lblDstId.Name = "lblDstId";
            this.lblDstId.Size = new System.Drawing.Size(78, 20);
            this.lblDstId.TabIndex = 8;
            this.lblDstId.Text = "デバイスID";
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Controls.Add(this.btnDisconnect);
            this.flowLayoutPanel4.Controls.Add(this.btnConnect);
            this.flowLayoutPanel4.Controls.Add(this.btnDeviceSearch);
            this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel4.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(3, 66);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(458, 52);
            this.flowLayoutPanel4.TabIndex = 2;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnDisconnect.Font = new System.Drawing.Font("Meiryo UI", 15.75F);
            this.btnDisconnect.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDisconnect.Location = new System.Drawing.Point(334, 5);
            this.btnDisconnect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(120, 38);
            this.btnDisconnect.TabIndex = 2;
            this.btnDisconnect.Text = "切断";
            this.btnDisconnect.UseVisualStyleBackColor = false;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnConnect.Font = new System.Drawing.Font("Meiryo UI", 15.75F);
            this.btnConnect.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnConnect.Location = new System.Drawing.Point(206, 5);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(120, 38);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "接続";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDeviceSearch
            // 
            this.btnDeviceSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeviceSearch.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnDeviceSearch.Font = new System.Drawing.Font("Meiryo UI", 15.75F);
            this.btnDeviceSearch.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDeviceSearch.Location = new System.Drawing.Point(78, 5);
            this.btnDeviceSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDeviceSearch.Name = "btnDeviceSearch";
            this.btnDeviceSearch.Size = new System.Drawing.Size(120, 38);
            this.btnDeviceSearch.TabIndex = 0;
            this.btnDeviceSearch.Text = "端末検索";
            this.btnDeviceSearch.UseVisualStyleBackColor = false;
            this.btnDeviceSearch.Click += new System.EventHandler(this.btnDeviceSearch_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.flowLayoutPanel1);
            this.groupBox3.Controls.Add(this.flowLayoutPanel2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(464, 149);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "無線ドングル設定";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.cmbComPort);
            this.flowLayoutPanel1.Controls.Add(this.btnSetComPort);
            this.flowLayoutPanel1.Controls.Add(this.btnDisableComPort);
            this.flowLayoutPanel1.Controls.Add(this.cmbChannel);
            this.flowLayoutPanel1.Controls.Add(this.btnSetChannel);
            this.flowLayoutPanel1.Controls.Add(this.txtSrcId);
            this.flowLayoutPanel1.Controls.Add(this.btnGetSrcId);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(137, 24);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(324, 122);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // cmbComPort
            // 
            this.cmbComPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbComPort.FormattingEnabled = true;
            this.cmbComPort.Location = new System.Drawing.Point(8, 8);
            this.cmbComPort.Name = "cmbComPort";
            this.cmbComPort.Size = new System.Drawing.Size(200, 28);
            this.cmbComPort.TabIndex = 0;
            this.tipsConnectSettings.SetToolTip(this.cmbComPort, "通信モジュールが接続されているポート番号を選択してください");
            this.cmbComPort.SelectedIndexChanged += new System.EventHandler(this.cmbComPort_SelectedIndexChanged);
            // 
            // btnSetComPort
            // 
            this.btnSetComPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetComPort.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSetComPort.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSetComPort.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSetComPort.Location = new System.Drawing.Point(215, 10);
            this.btnSetComPort.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSetComPort.Name = "btnSetComPort";
            this.btnSetComPort.Size = new System.Drawing.Size(87, 26);
            this.btnSetComPort.TabIndex = 1;
            this.btnSetComPort.Text = "接続";
            this.btnSetComPort.UseVisualStyleBackColor = false;
            this.btnSetComPort.Click += new System.EventHandler(this.btnSetComPort_Click);
            // 
            // btnDisableComPort
            // 
            this.btnDisableComPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisableComPort.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnDisableComPort.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDisableComPort.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDisableComPort.Location = new System.Drawing.Point(9, 46);
            this.btnDisableComPort.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDisableComPort.Name = "btnDisableComPort";
            this.btnDisableComPort.Size = new System.Drawing.Size(87, 26);
            this.btnDisableComPort.TabIndex = 2;
            this.btnDisableComPort.Text = "切断";
            this.btnDisableComPort.UseVisualStyleBackColor = false;
            this.btnDisableComPort.Visible = false;
            this.btnDisableComPort.Click += new System.EventHandler(this.btnDisableComPort_Click);
            // 
            // cmbChannel
            // 
            this.cmbChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChannel.FormattingEnabled = true;
            this.cmbChannel.Location = new System.Drawing.Point(103, 44);
            this.cmbChannel.Name = "cmbChannel";
            this.cmbChannel.Size = new System.Drawing.Size(200, 28);
            this.cmbChannel.TabIndex = 3;
            this.tipsConnectSettings.SetToolTip(this.cmbChannel, "使用するチャネル番号を選択してください");
            this.cmbChannel.SelectedIndexChanged += new System.EventHandler(this.cmbChannel_SelectedIndexChanged);
            // 
            // btnSetChannel
            // 
            this.btnSetChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetChannel.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnSetChannel.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSetChannel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSetChannel.Location = new System.Drawing.Point(9, 82);
            this.btnSetChannel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSetChannel.Name = "btnSetChannel";
            this.btnSetChannel.Size = new System.Drawing.Size(87, 26);
            this.btnSetChannel.TabIndex = 4;
            this.btnSetChannel.Text = "設定";
            this.btnSetChannel.UseVisualStyleBackColor = false;
            this.btnSetChannel.Click += new System.EventHandler(this.btnSetChannel_Click);
            // 
            // txtSrcId
            // 
            this.txtSrcId.Location = new System.Drawing.Point(103, 80);
            this.txtSrcId.MaximumSize = new System.Drawing.Size(380, 28);
            this.txtSrcId.MaxLength = 32;
            this.txtSrcId.MinimumSize = new System.Drawing.Size(200, 28);
            this.txtSrcId.Name = "txtSrcId";
            this.txtSrcId.ReadOnly = true;
            this.txtSrcId.Size = new System.Drawing.Size(200, 28);
            this.txtSrcId.TabIndex = 5;
            this.tipsConnectSettings.SetToolTip(this.txtSrcId, "接続後、接続先モジュールのIDが表示されます");
            // 
            // btnGetSrcId
            // 
            this.btnGetSrcId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetSrcId.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnGetSrcId.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnGetSrcId.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnGetSrcId.Location = new System.Drawing.Point(9, 118);
            this.btnGetSrcId.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGetSrcId.Name = "btnGetSrcId";
            this.btnGetSrcId.Size = new System.Drawing.Size(87, 26);
            this.btnGetSrcId.TabIndex = 6;
            this.btnGetSrcId.Text = "取得";
            this.btnGetSrcId.UseVisualStyleBackColor = false;
            this.btnGetSrcId.Click += new System.EventHandler(this.btnGetSrcId_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.lblComPort);
            this.flowLayoutPanel2.Controls.Add(this.lblChannel);
            this.flowLayoutPanel2.Controls.Add(this.lblSrcId);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 24);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel2.Size = new System.Drawing.Size(128, 122);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // lblComPort
            // 
            this.lblComPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblComPort.AutoSize = true;
            this.lblComPort.Location = new System.Drawing.Point(8, 12);
            this.lblComPort.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.lblComPort.Name = "lblComPort";
            this.lblComPort.Size = new System.Drawing.Size(82, 20);
            this.lblComPort.TabIndex = 4;
            this.lblComPort.Text = "COMポート";
            // 
            // lblChannel
            // 
            this.lblChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblChannel.AutoSize = true;
            this.lblChannel.Location = new System.Drawing.Point(8, 46);
            this.lblChannel.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.lblChannel.Name = "lblChannel";
            this.lblChannel.Size = new System.Drawing.Size(91, 20);
            this.lblChannel.TabIndex = 6;
            this.lblChannel.Text = "チャネル番号";
            // 
            // lblSrcId
            // 
            this.lblSrcId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSrcId.AutoSize = true;
            this.lblSrcId.Location = new System.Drawing.Point(8, 80);
            this.lblSrcId.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.lblSrcId.Name = "lblSrcId";
            this.lblSrcId.Size = new System.Drawing.Size(78, 20);
            this.lblSrcId.TabIndex = 7;
            this.lblSrcId.Text = "デバイスID";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.flowLayoutPanel8);
            this.groupBox2.Controls.Add(this.panel6);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(5, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(333, 269);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "出力";
            // 
            // flowLayoutPanel8
            // 
            this.flowLayoutPanel8.Controls.Add(this.btnClear);
            this.flowLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel8.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel8.Location = new System.Drawing.Point(3, 219);
            this.flowLayoutPanel8.Name = "flowLayoutPanel8";
            this.flowLayoutPanel8.Size = new System.Drawing.Size(327, 47);
            this.flowLayoutPanel8.TabIndex = 9;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnClear.Font = new System.Drawing.Font("Meiryo UI", 15.75F);
            this.btnClear.ForeColor = System.Drawing.SystemColors.Control;
            this.btnClear.Location = new System.Drawing.Point(203, 5);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 38);
            this.btnClear.TabIndex = 0;
            this.btnClear.Text = "クリア";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.txtTestResult);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 24);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(5);
            this.panel6.Size = new System.Drawing.Size(327, 242);
            this.panel6.TabIndex = 2;
            // 
            // txtTestResult
            // 
            this.txtTestResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTestResult.BackColor = System.Drawing.SystemColors.Window;
            this.txtTestResult.Location = new System.Drawing.Point(5, 5);
            this.txtTestResult.Multiline = true;
            this.txtTestResult.Name = "txtTestResult";
            this.txtTestResult.ReadOnly = true;
            this.txtTestResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTestResult.Size = new System.Drawing.Size(317, 190);
            this.txtTestResult.TabIndex = 20;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(474, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(5);
            this.panel3.Size = new System.Drawing.Size(343, 279);
            this.panel3.TabIndex = 3;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 327);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(817, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // FormSubGHzConnectSettings
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(817, 349);
            this.ControlBox = false;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FormSubGHzConnectSettings";
            this.Text = "通信設定";
            this.Load += new System.EventHandler(this.FormSubGHzConnectSettings_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.flowLayoutPanel7.ResumeLayout(false);
            this.flowLayoutPanel7.PerformLayout();
            this.flowLayoutPanel6.ResumeLayout(false);
            this.flowLayoutPanel6.PerformLayout();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.flowLayoutPanel8.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ComboBox cmbComPort;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblComPort;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox txtTestResult;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label lblChannel;
        private System.Windows.Forms.ToolTip tipsConnectSettings;
        private System.Windows.Forms.ComboBox cmbChannel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Label lblSrcId;
        private System.Windows.Forms.TextBox txtSrcId;
        private System.Windows.Forms.Button btnSetComPort;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel7;
        private System.Windows.Forms.TextBox txtDstId;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
        private System.Windows.Forms.Label lblDstId;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnDeviceSearch;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel8;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSetChannel;
        private System.Windows.Forms.Button btnGetSrcId;
        private System.Windows.Forms.Button btnDisableComPort;
    }
}