using System.Windows.Forms;
using System.Security.Permissions;
using System.ComponentModel;

namespace ShelterInfoSystem
{
    /// <summary>
    /// IPアドレス欄のコピペ制御専用クラス
    /// </summary>
    class FormEditIPAddr : TextBox
    {
        protected override void WndProc(ref Message m)
        {
            // IPアドレステキストボックスに貼り付け操作がされたとき
            // 数字と「.」(ドット)以外を含む文字列の場合は貼り付け不可にする
            const int WM_PASTE = 0x302;
            if (m.Msg == WM_PASTE)
            {
                IDataObject iData = Clipboard.GetDataObject();
                //文字列がクリップボードにあるか
                if (iData != null && iData.GetDataPresent(DataFormats.Text))
                {
                    string clipStr = (string)iData.GetData(DataFormats.Text);
                    //クリップボードの文字列がIPアドレスのフォーマット(xxx.xxx.xxx.xxx)になっているかチェック
                    if (!System.Text.RegularExpressions.Regex.IsMatch(
                        clipStr,
                        @"\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}\z")
                        )
                    {
                        // なっていない場合は貼り付け不可
                        return;
                    }
                }
            }
            base.WndProc(ref m);
        }
    }

    /// <summary>
    /// ポート番号欄のコピペ制御専用クラス
    /// </summary>
    class FormEditPortNo : TextBox
    {
        protected override void WndProc(ref Message m)
        {
            // ポート番号テキストボックスに貼り付け操作がされたとき
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

    partial class FormTcpSetting
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
            this.btnTCPSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort = new ShelterInfoSystem.FormEditPortNo();
            this.txtIpAddr = new ShelterInfoSystem.FormEditIPAddr();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(33, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "IPアドレス：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnTCPSave
            // 
            this.btnTCPSave.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnTCPSave.Font = new System.Drawing.Font("Meiryo UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnTCPSave.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnTCPSave.Location = new System.Drawing.Point(139, 107);
            this.btnTCPSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnTCPSave.Name = "btnTCPSave";
            this.btnTCPSave.Size = new System.Drawing.Size(120, 38);
            this.btnTCPSave.TabIndex = 2;
            this.btnTCPSave.Text = "閉じる";
            this.btnTCPSave.UseVisualStyleBackColor = false;
            this.btnTCPSave.Click += new System.EventHandler(this.btnTCPSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(27, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "ポート番号：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.txtPort.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txtPort.Location = new System.Drawing.Point(139, 67);
            this.txtPort.MaxLength = 5;
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(178, 28);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "50000";
            this.txtPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPort_KeyPress);
            // 
            // txtIpAddr
            // 
            this.txtIpAddr.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.txtIpAddr.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txtIpAddr.Location = new System.Drawing.Point(139, 27);
            this.txtIpAddr.MaxLength = 15;
            this.txtIpAddr.Name = "txtIpAddr";
            this.txtIpAddr.Size = new System.Drawing.Size(178, 28);
            this.txtIpAddr.TabIndex = 0;
            this.txtIpAddr.Text = "127.0.0.1";
            this.txtIpAddr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIpAddr_KeyPress);
            // 
            // FormTcpSetting
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(386, 159);
            this.ControlBox = false;
            this.Controls.Add(this.txtIpAddr);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTCPSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormTcpSetting";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "避難所情報システム - 個人安否情報受信設定";
            this.Load += new System.EventHandler(this.FormTcpSetting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnTCPSave;
        private System.Windows.Forms.Label label2;
        private FormEditPortNo txtPort;
        private FormEditIPAddr txtIpAddr;
    }
}