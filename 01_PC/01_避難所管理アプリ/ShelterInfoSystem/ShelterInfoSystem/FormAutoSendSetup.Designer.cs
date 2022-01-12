namespace ShelterInfoSystem
{
    partial class FormAutoSendSetup
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxMonth = new System.Windows.Forms.ComboBox();
            this.comboBoxDate = new System.Windows.Forms.ComboBox();
            this.comboBoxHour = new System.Windows.Forms.ComboBox();
            this.comboBoxMinute = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRegist = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.radioButtonMonthly = new System.Windows.Forms.RadioButton();
            this.radioButtonDaily = new System.Windows.Forms.RadioButton();
            this.radioButtonHourly = new System.Windows.Forms.RadioButton();
            this.radioButtonTimeSpan = new System.Windows.Forms.RadioButton();
            this.radioButtonUnspecified = new System.Windows.Forms.RadioButton();
            this.comboBoxHour2 = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBoxMin2 = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.radioEnable = new System.Windows.Forms.RadioButton();
            this.radioDisable = new System.Windows.Forms.RadioButton();
            this.label15 = new System.Windows.Forms.Label();
            this.checkTerminalInfo = new System.Windows.Forms.CheckBox();
            this.checkTerminalDetail = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 20);
            this.label1.TabIndex = 20;
            this.label1.Text = "自動送信の送信時間を ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(213, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.TabIndex = 23;
            this.label2.Text = "します。 ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(174, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 20);
            this.label3.TabIndex = 22;
            this.label3.Text = "設定 ";
            // 
            // comboBoxMonth
            // 
            this.comboBoxMonth.DropDownHeight = 200;
            this.comboBoxMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMonth.FormattingEnabled = true;
            this.comboBoxMonth.IntegralHeight = false;
            this.comboBoxMonth.Items.AddRange(new object[] {
            "-",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.comboBoxMonth.Location = new System.Drawing.Point(149, 117);
            this.comboBoxMonth.Name = "comboBoxMonth";
            this.comboBoxMonth.Size = new System.Drawing.Size(45, 20);
            this.comboBoxMonth.TabIndex = 1;
            this.comboBoxMonth.SelectedIndexChanged += new System.EventHandler(this.comboBoxMonth_SelectedIndexChanged);
            // 
            // comboBoxDate
            // 
            this.comboBoxDate.DropDownHeight = 200;
            this.comboBoxDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDate.FormattingEnabled = true;
            this.comboBoxDate.IntegralHeight = false;
            this.comboBoxDate.Items.AddRange(new object[] {
            "-",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.comboBoxDate.Location = new System.Drawing.Point(230, 117);
            this.comboBoxDate.Name = "comboBoxDate";
            this.comboBoxDate.Size = new System.Drawing.Size(45, 20);
            this.comboBoxDate.TabIndex = 2;
            // 
            // comboBoxHour
            // 
            this.comboBoxHour.DropDownHeight = 200;
            this.comboBoxHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHour.FormattingEnabled = true;
            this.comboBoxHour.IntegralHeight = false;
            this.comboBoxHour.Items.AddRange(new object[] {
            "-",
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.comboBoxHour.Location = new System.Drawing.Point(311, 117);
            this.comboBoxHour.Name = "comboBoxHour";
            this.comboBoxHour.Size = new System.Drawing.Size(45, 20);
            this.comboBoxHour.TabIndex = 3;
            // 
            // comboBoxMinute
            // 
            this.comboBoxMinute.DropDownHeight = 200;
            this.comboBoxMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMinute.FormattingEnabled = true;
            this.comboBoxMinute.IntegralHeight = false;
            this.comboBoxMinute.Items.AddRange(new object[] {
            "-",
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59"});
            this.comboBoxMinute.Location = new System.Drawing.Point(392, 117);
            this.comboBoxMinute.Name = "comboBoxMinute";
            this.comboBoxMinute.Size = new System.Drawing.Size(45, 20);
            this.comboBoxMinute.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnCancel.Font = new System.Drawing.Font("Meiryo UI", 15.75F);
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCancel.Location = new System.Drawing.Point(379, 409);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 38);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRegist
            // 
            this.btnRegist.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnRegist.Font = new System.Drawing.Font("Meiryo UI", 15.75F);
            this.btnRegist.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnRegist.Location = new System.Drawing.Point(251, 409);
            this.btnRegist.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRegist.Name = "btnRegist";
            this.btnRegist.Size = new System.Drawing.Size(120, 38);
            this.btnRegist.TabIndex = 14;
            this.btnRegist.Text = "設定";
            this.btnRegist.UseVisualStyleBackColor = false;
            this.btnRegist.Click += new System.EventHandler(this.btnRegist_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(443, 118);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 19);
            this.label7.TabIndex = 30;
            this.label7.Text = "分";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(362, 118);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 19);
            this.label6.TabIndex = 30;
            this.label6.Text = "時";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(281, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 19);
            this.label5.TabIndex = 29;
            this.label5.Text = "日";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(200, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 19);
            this.label4.TabIndex = 29;
            this.label4.Text = "月";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(28, 89);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(248, 17);
            this.label8.TabIndex = 20;
            this.label8.Text = "①自動送信を行う開始時刻を入力してください";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(53, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 17);
            this.label9.TabIndex = 20;
            this.label9.Text = "送信開始日時";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(28, 151);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(320, 17);
            this.label10.TabIndex = 20;
            this.label10.Text = "②送信開始日時から自動送信を行う間隔を入力してください";
            // 
            // radioButtonMonthly
            // 
            this.radioButtonMonthly.AutoSize = true;
            this.radioButtonMonthly.Font = new System.Drawing.Font("Meiryo UI", 9.75F);
            this.radioButtonMonthly.Location = new System.Drawing.Point(56, 203);
            this.radioButtonMonthly.Name = "radioButtonMonthly";
            this.radioButtonMonthly.Size = new System.Drawing.Size(301, 21);
            this.radioButtonMonthly.TabIndex = 6;
            this.radioButtonMonthly.TabStop = true;
            this.radioButtonMonthly.Text = "毎月送信：送信日時で指定した日時分で毎月送信";
            this.radioButtonMonthly.UseVisualStyleBackColor = true;
            this.radioButtonMonthly.CheckedChanged += new System.EventHandler(this.radioButtonTimeSpan_CheckedChanged);
            // 
            // radioButtonDaily
            // 
            this.radioButtonDaily.AutoSize = true;
            this.radioButtonDaily.Font = new System.Drawing.Font("Meiryo UI", 9.75F);
            this.radioButtonDaily.Location = new System.Drawing.Point(56, 230);
            this.radioButtonDaily.Name = "radioButtonDaily";
            this.radioButtonDaily.Size = new System.Drawing.Size(288, 21);
            this.radioButtonDaily.TabIndex = 7;
            this.radioButtonDaily.TabStop = true;
            this.radioButtonDaily.Text = "毎日送信：送信日時で指定した時分で毎日送信";
            this.radioButtonDaily.UseVisualStyleBackColor = true;
            this.radioButtonDaily.CheckedChanged += new System.EventHandler(this.radioButtonTimeSpan_CheckedChanged);
            // 
            // radioButtonHourly
            // 
            this.radioButtonHourly.AutoSize = true;
            this.radioButtonHourly.Font = new System.Drawing.Font("Meiryo UI", 9.75F);
            this.radioButtonHourly.Location = new System.Drawing.Point(56, 257);
            this.radioButtonHourly.Name = "radioButtonHourly";
            this.radioButtonHourly.Size = new System.Drawing.Size(275, 21);
            this.radioButtonHourly.TabIndex = 8;
            this.radioButtonHourly.TabStop = true;
            this.radioButtonHourly.Text = "毎時送信：送信日時で指定した分で毎時送信";
            this.radioButtonHourly.UseVisualStyleBackColor = true;
            this.radioButtonHourly.CheckedChanged += new System.EventHandler(this.radioButtonTimeSpan_CheckedChanged);
            // 
            // radioButtonTimeSpan
            // 
            this.radioButtonTimeSpan.AutoSize = true;
            this.radioButtonTimeSpan.Font = new System.Drawing.Font("Meiryo UI", 9.75F);
            this.radioButtonTimeSpan.Location = new System.Drawing.Point(56, 284);
            this.radioButtonTimeSpan.Name = "radioButtonTimeSpan";
            this.radioButtonTimeSpan.Size = new System.Drawing.Size(271, 21);
            this.radioButtonTimeSpan.TabIndex = 9;
            this.radioButtonTimeSpan.TabStop = true;
            this.radioButtonTimeSpan.Text = "指定時間送信：送信日時で指定した時間から";
            this.radioButtonTimeSpan.UseVisualStyleBackColor = true;
            this.radioButtonTimeSpan.CheckedChanged += new System.EventHandler(this.radioButtonTimeSpan_CheckedChanged);
            // 
            // radioButtonUnspecified
            // 
            this.radioButtonUnspecified.AutoSize = true;
            this.radioButtonUnspecified.Font = new System.Drawing.Font("Meiryo UI", 9.75F);
            this.radioButtonUnspecified.Location = new System.Drawing.Point(56, 176);
            this.radioButtonUnspecified.Name = "radioButtonUnspecified";
            this.radioButtonUnspecified.Size = new System.Drawing.Size(294, 21);
            this.radioButtonUnspecified.TabIndex = 5;
            this.radioButtonUnspecified.TabStop = true;
            this.radioButtonUnspecified.Text = "指定なし：送信開始日時で指定した時間のみ送信";
            this.radioButtonUnspecified.UseVisualStyleBackColor = true;
            this.radioButtonUnspecified.CheckedChanged += new System.EventHandler(this.radioButtonTimeSpan_CheckedChanged);
            // 
            // comboBoxHour2
            // 
            this.comboBoxHour2.DropDownHeight = 200;
            this.comboBoxHour2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxHour2.FormattingEnabled = true;
            this.comboBoxHour2.IntegralHeight = false;
            this.comboBoxHour2.Items.AddRange(new object[] {
            "-",
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.comboBoxHour2.Location = new System.Drawing.Point(169, 310);
            this.comboBoxHour2.Name = "comboBoxHour2";
            this.comboBoxHour2.Size = new System.Drawing.Size(45, 20);
            this.comboBoxHour2.TabIndex = 10;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label11.Location = new System.Drawing.Point(220, 311);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 19);
            this.label11.TabIndex = 30;
            this.label11.Text = "時間";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label12.Location = new System.Drawing.Point(316, 311);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(24, 19);
            this.label12.TabIndex = 30;
            this.label12.Text = "分";
            // 
            // comboBoxMin2
            // 
            this.comboBoxMin2.DropDownHeight = 200;
            this.comboBoxMin2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMin2.FormattingEnabled = true;
            this.comboBoxMin2.IntegralHeight = false;
            this.comboBoxMin2.Items.AddRange(new object[] {
            "-",
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59"});
            this.comboBoxMin2.Location = new System.Drawing.Point(265, 310);
            this.comboBoxMin2.Name = "comboBoxMin2";
            this.comboBoxMin2.Size = new System.Drawing.Size(45, 20);
            this.comboBoxMin2.TabIndex = 11;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label13.Location = new System.Drawing.Point(346, 313);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 17);
            this.label13.TabIndex = 20;
            this.label13.Text = "ごとに送信";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label14.Location = new System.Drawing.Point(27, 53);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(121, 20);
            this.label14.TabIndex = 31;
            this.label14.Text = "自動送信設定：";
            // 
            // radioEnable
            // 
            this.radioEnable.AutoSize = true;
            this.radioEnable.Location = new System.Drawing.Point(35, 25);
            this.radioEnable.Name = "radioEnable";
            this.radioEnable.Size = new System.Drawing.Size(47, 16);
            this.radioEnable.TabIndex = 0;
            this.radioEnable.Text = "有効";
            this.radioEnable.UseVisualStyleBackColor = true;
            this.radioEnable.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioDisable
            // 
            this.radioDisable.AutoSize = true;
            this.radioDisable.Location = new System.Drawing.Point(109, 25);
            this.radioDisable.Name = "radioDisable";
            this.radioDisable.Size = new System.Drawing.Size(47, 16);
            this.radioDisable.TabIndex = 0;
            this.radioDisable.Text = "無効";
            this.radioDisable.UseVisualStyleBackColor = true;
            this.radioDisable.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label15.Location = new System.Drawing.Point(28, 342);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(228, 17);
            this.label15.TabIndex = 20;
            this.label15.Text = "③自動送信の対象項目を選択してください";
            // 
            // checkTerminalInfo
            // 
            this.checkTerminalInfo.AutoSize = true;
            this.checkTerminalInfo.Checked = true;
            this.checkTerminalInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkTerminalInfo.Location = new System.Drawing.Point(56, 370);
            this.checkTerminalInfo.Name = "checkTerminalInfo";
            this.checkTerminalInfo.Size = new System.Drawing.Size(72, 16);
            this.checkTerminalInfo.TabIndex = 12;
            this.checkTerminalInfo.Text = "避難者数";
            this.checkTerminalInfo.UseVisualStyleBackColor = true;
            // 
            // checkTerminalDetail
            // 
            this.checkTerminalDetail.AutoSize = true;
            this.checkTerminalDetail.Checked = true;
            this.checkTerminalDetail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkTerminalDetail.Location = new System.Drawing.Point(169, 370);
            this.checkTerminalDetail.Name = "checkTerminalDetail";
            this.checkTerminalDetail.Size = new System.Drawing.Size(108, 16);
            this.checkTerminalDetail.TabIndex = 13;
            this.checkTerminalDetail.Text = "避難所詳細情報";
            this.checkTerminalDetail.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioDisable);
            this.groupBox1.Controls.Add(this.radioEnable);
            this.groupBox1.Location = new System.Drawing.Point(148, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(196, 56);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // FormAutoSendSetup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(512, 459);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkTerminalDetail);
            this.Controls.Add(this.checkTerminalInfo);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.comboBoxHour2);
            this.Controls.Add(this.radioButtonUnspecified);
            this.Controls.Add(this.radioButtonTimeSpan);
            this.Controls.Add(this.radioButtonHourly);
            this.Controls.Add(this.radioButtonDaily);
            this.Controls.Add(this.radioButtonMonthly);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRegist);
            this.Controls.Add(this.comboBoxMin2);
            this.Controls.Add(this.comboBoxMinute);
            this.Controls.Add(this.comboBoxHour);
            this.Controls.Add(this.comboBoxDate);
            this.Controls.Add(this.comboBoxMonth);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormAutoSendSetup";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "避難所情報システム  －自動送信設定";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxMonth;
        private System.Windows.Forms.ComboBox comboBoxDate;
        private System.Windows.Forms.ComboBox comboBoxHour;
        private System.Windows.Forms.ComboBox comboBoxMinute;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRegist;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RadioButton radioButtonMonthly;
        private System.Windows.Forms.RadioButton radioButtonDaily;
        private System.Windows.Forms.RadioButton radioButtonHourly;
        private System.Windows.Forms.RadioButton radioButtonTimeSpan;
        private System.Windows.Forms.RadioButton radioButtonUnspecified;
        private System.Windows.Forms.ComboBox comboBoxHour2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboBoxMin2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.RadioButton radioEnable;
        private System.Windows.Forms.RadioButton radioDisable;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox checkTerminalInfo;
        private System.Windows.Forms.CheckBox checkTerminalDetail;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}