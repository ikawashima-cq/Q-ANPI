using System.Windows.Forms;
using System.Security.Permissions;
using System.ComponentModel;

namespace ShelterInfoSystem
{
    /// <summary>
    /// 電話番号欄のコピペ制御専用クラス
    /// </summary>
    class FormEditPersonalPhoneNumber : TextBox
    {
        protected override void WndProc(ref Message m)
        {
            // 電話番号テキストボックスに貼り付け操作がされたとき
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

    /// <summary>
    /// 氏名欄のコピペ制御専用クラス
    /// </summary>
    class FormEditPersonalName : TextBox
    {
        protected override void WndProc(ref Message m)
        {
            // 氏名テキストボックスに貼り付け操作がされたとき
            // 空白文字(タブ文字)が入力されたときに張り付け不可にする
            const int WM_PASTE = 0x302;
            if (m.Msg == WM_PASTE)
            {
                IDataObject iData = Clipboard.GetDataObject();
                //文字列がクリップボードにあるか
                if (iData != null && iData.GetDataPresent(DataFormats.Text))
                {
                    // 空白文字を含むものは貼り付け不可
                    string clipStr = (string)iData.GetData(DataFormats.Text);
                    if (clipStr.Contains(" ") || clipStr.Contains("　") || clipStr.Contains("\t"))
                    {
                        return;
                    }
                }
            }
            base.WndProc(ref m);
        }
    }


    partial class FormEditPersonal
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
            this.label4 = new System.Windows.Forms.Label();
            this.txtName1 = new ShelterInfoSystem.FormEditPersonalName();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEntry = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.rdbEnter = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdbHome = new System.Windows.Forms.RadioButton();
            this.rdbRelese = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdbWomen = new System.Windows.Forms.RadioButton();
            this.rdbMan = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdbNoInjuries = new System.Windows.Forms.RadioButton();
            this.rdbInjuries = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rdbPrivate = new System.Windows.Forms.RadioButton();
            this.rdbPublic = new System.Windows.Forms.RadioButton();
            this.radioNonCare = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.radioCare = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.label12 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.txtName2 = new ShelterInfoSystem.FormEditPersonalName();
            this.label9 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.comboYear = new System.Windows.Forms.ComboBox();
            this.comboMonth = new System.Windows.Forms.ComboBox();
            this.comboDay = new System.Windows.Forms.ComboBox();
            this.txtTel = new ShelterInfoSystem.FormEditPersonalPhoneNumber();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(63, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "氏名：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtName1
            // 
            this.txtName1.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtName1.Location = new System.Drawing.Point(126, 39);
            this.txtName1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtName1.MaxLength = 12;
            this.txtName1.Name = "txtName1";
            this.txtName1.Size = new System.Drawing.Size(100, 28);
            this.txtName1.TabIndex = 1;
            this.txtName1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName1_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(31, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "電話番号：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(24, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 20);
            this.label6.TabIndex = 15;
            this.label6.Text = "入所/退所：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(31, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 20);
            this.label7.TabIndex = 5;
            this.label7.Text = "生年月日：";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtAddress
            // 
            this.txtAddress.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtAddress.Location = new System.Drawing.Point(126, 232);
            this.txtAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAddress.MaxLength = 64;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(303, 28);
            this.txtAddress.TabIndex = 9;
            this.txtAddress.Text = "○○県○○市○○1-1-1";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnCancel.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCancel.Location = new System.Drawing.Point(239, 441);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 38);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnEntry
            // 
            this.btnEntry.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnEntry.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnEntry.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnEntry.Location = new System.Drawing.Point(97, 441);
            this.btnEntry.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnEntry.Name = "btnEntry";
            this.btnEntry.Size = new System.Drawing.Size(120, 38);
            this.btnEntry.TabIndex = 14;
            this.btnEntry.Text = "登録";
            this.btnEntry.UseVisualStyleBackColor = false;
            this.btnEntry.Click += new System.EventHandler(this.btnEntry_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(63, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "性別：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(63, 236);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 20);
            this.label2.TabIndex = 19;
            this.label2.Text = "住所：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(24, 280);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 20);
            this.label3.TabIndex = 21;
            this.label3.Text = "ケガの有無：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(18, 193);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(102, 20);
            this.label8.TabIndex = 17;
            this.label8.Text = "公表の可否：";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // rdbEnter
            // 
            this.rdbEnter.AutoSize = true;
            this.rdbEnter.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdbEnter.Location = new System.Drawing.Point(3, 8);
            this.rdbEnter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdbEnter.Name = "rdbEnter";
            this.rdbEnter.Size = new System.Drawing.Size(59, 24);
            this.rdbEnter.TabIndex = 0;
            this.rdbEnter.TabStop = true;
            this.rdbEnter.Text = "入所";
            this.rdbEnter.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdbHome);
            this.panel1.Controls.Add(this.rdbRelese);
            this.panel1.Controls.Add(this.rdbEnter);
            this.panel1.Location = new System.Drawing.Point(126, 143);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(282, 41);
            this.panel1.TabIndex = 7;
            // 
            // rdbHome
            // 
            this.rdbHome.AutoSize = true;
            this.rdbHome.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdbHome.Location = new System.Drawing.Point(203, 8);
            this.rdbHome.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdbHome.Name = "rdbHome";
            this.rdbHome.Size = new System.Drawing.Size(59, 24);
            this.rdbHome.TabIndex = 2;
            this.rdbHome.TabStop = true;
            this.rdbHome.Text = "在宅";
            this.rdbHome.UseVisualStyleBackColor = true;
            // 
            // rdbRelese
            // 
            this.rdbRelese.AutoSize = true;
            this.rdbRelese.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdbRelese.Location = new System.Drawing.Point(103, 8);
            this.rdbRelese.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdbRelese.Name = "rdbRelese";
            this.rdbRelese.Size = new System.Drawing.Size(59, 24);
            this.rdbRelese.TabIndex = 1;
            this.rdbRelese.TabStop = true;
            this.rdbRelese.Text = "退所";
            this.rdbRelese.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rdbWomen);
            this.panel2.Controls.Add(this.rdbMan);
            this.panel2.Location = new System.Drawing.Point(126, 104);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(237, 41);
            this.panel2.TabIndex = 6;
            // 
            // rdbWomen
            // 
            this.rdbWomen.AutoSize = true;
            this.rdbWomen.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdbWomen.Location = new System.Drawing.Point(103, 7);
            this.rdbWomen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdbWomen.Name = "rdbWomen";
            this.rdbWomen.Size = new System.Drawing.Size(43, 24);
            this.rdbWomen.TabIndex = 1;
            this.rdbWomen.TabStop = true;
            this.rdbWomen.Text = "女";
            this.rdbWomen.UseVisualStyleBackColor = true;
            // 
            // rdbMan
            // 
            this.rdbMan.AutoSize = true;
            this.rdbMan.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdbMan.Location = new System.Drawing.Point(3, 7);
            this.rdbMan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdbMan.Name = "rdbMan";
            this.rdbMan.Size = new System.Drawing.Size(43, 24);
            this.rdbMan.TabIndex = 0;
            this.rdbMan.TabStop = true;
            this.rdbMan.Text = "男";
            this.rdbMan.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rdbNoInjuries);
            this.panel3.Controls.Add(this.rdbInjuries);
            this.panel3.Location = new System.Drawing.Point(126, 268);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(237, 42);
            this.panel3.TabIndex = 10;
            // 
            // rdbNoInjuries
            // 
            this.rdbNoInjuries.AutoSize = true;
            this.rdbNoInjuries.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdbNoInjuries.Location = new System.Drawing.Point(103, 10);
            this.rdbNoInjuries.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdbNoInjuries.Name = "rdbNoInjuries";
            this.rdbNoInjuries.Size = new System.Drawing.Size(55, 24);
            this.rdbNoInjuries.TabIndex = 1;
            this.rdbNoInjuries.TabStop = true;
            this.rdbNoInjuries.Text = "無し";
            this.rdbNoInjuries.UseVisualStyleBackColor = true;
            // 
            // rdbInjuries
            // 
            this.rdbInjuries.AutoSize = true;
            this.rdbInjuries.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdbInjuries.Location = new System.Drawing.Point(3, 10);
            this.rdbInjuries.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdbInjuries.Name = "rdbInjuries";
            this.rdbInjuries.Size = new System.Drawing.Size(53, 24);
            this.rdbInjuries.TabIndex = 0;
            this.rdbInjuries.TabStop = true;
            this.rdbInjuries.Text = "有り";
            this.rdbInjuries.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.rdbPrivate);
            this.panel4.Controls.Add(this.rdbPublic);
            this.panel4.Location = new System.Drawing.Point(126, 183);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(237, 42);
            this.panel4.TabIndex = 8;
            // 
            // rdbPrivate
            // 
            this.rdbPrivate.AutoSize = true;
            this.rdbPrivate.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdbPrivate.Location = new System.Drawing.Point(103, 9);
            this.rdbPrivate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdbPrivate.Name = "rdbPrivate";
            this.rdbPrivate.Size = new System.Drawing.Size(75, 24);
            this.rdbPrivate.TabIndex = 1;
            this.rdbPrivate.TabStop = true;
            this.rdbPrivate.Text = "非公表";
            this.rdbPrivate.UseVisualStyleBackColor = true;
            // 
            // rdbPublic
            // 
            this.rdbPublic.AutoSize = true;
            this.rdbPublic.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdbPublic.Location = new System.Drawing.Point(3, 9);
            this.rdbPublic.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdbPublic.Name = "rdbPublic";
            this.rdbPublic.Size = new System.Drawing.Size(59, 24);
            this.rdbPublic.TabIndex = 0;
            this.rdbPublic.TabStop = true;
            this.rdbPublic.Text = "公表";
            this.rdbPublic.UseVisualStyleBackColor = true;
            // 
            // radioNonCare
            // 
            this.radioNonCare.AutoSize = true;
            this.radioNonCare.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radioNonCare.Location = new System.Drawing.Point(103, 9);
            this.radioNonCare.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioNonCare.Name = "radioNonCare";
            this.radioNonCare.Size = new System.Drawing.Size(59, 24);
            this.radioNonCare.TabIndex = 1;
            this.radioNonCare.TabStop = true;
            this.radioNonCare.Text = "不要";
            this.radioNonCare.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(18, 321);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 20);
            this.label10.TabIndex = 23;
            this.label10.Text = "介護の要否：";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.radioNonCare);
            this.panel6.Controls.Add(this.radioCare);
            this.panel6.Location = new System.Drawing.Point(126, 310);
            this.panel6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(237, 42);
            this.panel6.TabIndex = 11;
            // 
            // radioCare
            // 
            this.radioCare.AutoSize = true;
            this.radioCare.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radioCare.Location = new System.Drawing.Point(3, 9);
            this.radioCare.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioCare.Name = "radioCare";
            this.radioCare.Size = new System.Drawing.Size(59, 24);
            this.radioCare.TabIndex = 0;
            this.radioCare.TabStop = true;
            this.radioCare.Text = "必要";
            this.radioCare.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radioButton1.Location = new System.Drawing.Point(3, 9);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(53, 24);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "有り";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label11.Location = new System.Drawing.Point(8, 363);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 20);
            this.label11.TabIndex = 25;
            this.label11.Text = "障がいの有無：";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.radioButton2);
            this.panel7.Controls.Add(this.radioButton1);
            this.panel7.Location = new System.Drawing.Point(126, 352);
            this.panel7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(237, 42);
            this.panel7.TabIndex = 12;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radioButton2.Location = new System.Drawing.Point(103, 9);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(55, 24);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "無し";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radioButton3.Location = new System.Drawing.Point(103, 9);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(66, 24);
            this.radioButton3.TabIndex = 1;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "いいえ";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label12.Location = new System.Drawing.Point(47, 405);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(73, 20);
            this.label12.TabIndex = 27;
            this.label12.Text = "妊産婦：";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.radioButton3);
            this.panel8.Controls.Add(this.radioButton4);
            this.panel8.Location = new System.Drawing.Point(126, 394);
            this.panel8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(237, 42);
            this.panel8.TabIndex = 13;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radioButton4.Location = new System.Drawing.Point(3, 9);
            this.radioButton4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(54, 24);
            this.radioButton4.TabIndex = 0;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "はい";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // txtName2
            // 
            this.txtName2.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtName2.Location = new System.Drawing.Point(232, 39);
            this.txtName2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtName2.MaxLength = 13;
            this.txtName2.Name = "txtName2";
            this.txtName2.Size = new System.Drawing.Size(100, 28);
            this.txtName2.TabIndex = 2;
            this.txtName2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName1_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(122, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 20);
            this.label9.TabIndex = 6;
            this.label9.Text = "西暦";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(240, 76);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(25, 20);
            this.label13.TabIndex = 8;
            this.label13.Text = "年";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(322, 76);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(25, 20);
            this.label14.TabIndex = 10;
            this.label14.Text = "月";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(404, 76);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(25, 20);
            this.label15.TabIndex = 12;
            this.label15.Text = "日";
            // 
            // comboYear
            // 
            this.comboYear.DropDownHeight = 300;
            this.comboYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboYear.FormattingEnabled = true;
            this.comboYear.IntegralHeight = false;
            this.comboYear.Location = new System.Drawing.Point(169, 73);
            this.comboYear.Name = "comboYear";
            this.comboYear.Size = new System.Drawing.Size(65, 28);
            this.comboYear.TabIndex = 3;
            this.comboYear.SelectedIndexChanged += new System.EventHandler(this.comboYear_SelectedIndexChanged);
            // 
            // comboMonth
            // 
            this.comboMonth.DropDownHeight = 400;
            this.comboMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMonth.FormattingEnabled = true;
            this.comboMonth.IntegralHeight = false;
            this.comboMonth.Location = new System.Drawing.Point(271, 73);
            this.comboMonth.Name = "comboMonth";
            this.comboMonth.Size = new System.Drawing.Size(45, 28);
            this.comboMonth.TabIndex = 4;
            this.comboMonth.SelectedIndexChanged += new System.EventHandler(this.comboMonth_SelectedIndexChanged);
            // 
            // comboDay
            // 
            this.comboDay.DropDownHeight = 300;
            this.comboDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDay.FormattingEnabled = true;
            this.comboDay.IntegralHeight = false;
            this.comboDay.Location = new System.Drawing.Point(353, 73);
            this.comboDay.Name = "comboDay";
            this.comboDay.Size = new System.Drawing.Size(45, 28);
            this.comboDay.TabIndex = 5;
            // 
            // txtTel
            // 
            this.txtTel.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtTel.Location = new System.Drawing.Point(126, 7);
            this.txtTel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTel.MaxLength = 12;
            this.txtTel.Name = "txtTel";
            this.txtTel.Size = new System.Drawing.Size(206, 28);
            this.txtTel.TabIndex = 0;
            this.txtTel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTel_KeyPress);
            // 
            // FormEditPersonal
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(444, 489);
            this.ControlBox = false;
            this.Controls.Add(this.comboDay);
            this.Controls.Add(this.comboMonth);
            this.Controls.Add(this.comboYear);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtName2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnEntry);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.txtTel);
            this.Controls.Add(this.txtName1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormEditPersonal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "避難所情報システム  － 個人情報編集";
            this.Load += new System.EventHandler(this.FormEditPersonal_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnEntry;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton rdbEnter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdbRelese;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rdbWomen;
        private System.Windows.Forms.RadioButton rdbMan;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rdbNoInjuries;
        private System.Windows.Forms.RadioButton rdbInjuries;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton rdbPrivate;
        private System.Windows.Forms.RadioButton rdbPublic;
        private System.Windows.Forms.RadioButton radioNonCare;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.RadioButton radioCare;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton rdbHome;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox comboYear;
        private System.Windows.Forms.ComboBox comboMonth;
        private System.Windows.Forms.ComboBox comboDay;
        private FormEditPersonalPhoneNumber txtTel;
        private FormEditPersonalName txtName1;
        private FormEditPersonalName txtName2;
    }
}