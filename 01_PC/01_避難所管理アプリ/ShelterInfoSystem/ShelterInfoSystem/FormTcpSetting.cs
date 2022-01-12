using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace ShelterInfoSystem
{
    public partial class FormTcpSetting : Form
    {
        private FormShelterInfo m_mainForm;
        public FormTcpSetting(FormShelterInfo mainForm)
        {
            InitializeComponent();
            m_mainForm = mainForm;

            // 右クリックメニューを非表示にする
            txtIpAddr.ContextMenu = new ContextMenu();
            txtPort.ContextMenu = new ContextMenu();
        }

        /// <summary>
        /// 閉じるボタン押下事処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTCPSave_Click(object sender, EventArgs e)
        {
            bool chkAddress = true;
            bool chkIpNo = true;

            // IPアドレスの整合性チェック
            string[] ipAddressSplit;
            try
            {
                // カンマ「.」区切りで4つの数字が取得できない場合エラー
                ipAddressSplit = txtIpAddr.Text.Replace(" ", "").Split('.');
                if (ipAddressSplit.Length != 4)
                {
                    chkAddress = false;
                }
                else
                {
                    foreach (string splitStr in ipAddressSplit)
                    {
                        if (splitStr == "")
                        {
                            // .(ドット)間の数値が入力されていないときエラー
                            chkAddress = false;
                            continue;
                        }
                        if ((int.Parse(splitStr) > 255) || (int.Parse(splitStr) < 0))
                        {
                            // .(ドット)間の数値が0～255以外の時エラー
                            chkAddress = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                chkAddress = false;
            }

            if (!chkAddress)
            {
                MessageBox.Show("使用不可能なIPアドレスが設定されました。" + Environment.NewLine +
                                "IPアドレスは0.0.0.0～255.255.255.255の範囲内で設定してください。", "避難所システム", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "btnTCPSave_Click", "IPアドレスに使用不可能な値が設定される");
            }
            else
            {
                if (txtPort.Text == "")
                {
                    chkIpNo = false;
                    MessageBox.Show("ポート番号を1～65535の範囲内で設定してください。", "避難所システム", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "btnTCPSave_Click", "ポート番号未設定");
                }
                // ポート番号の整合性チェック(ポート番号が1～65535以外の時エラー)
                else if ((int.Parse(txtPort.Text) > 65535) || (int.Parse(txtPort.Text) < 1))
                {
                    chkIpNo = false;
                    MessageBox.Show("使用不可能なポート番号が設定されました。" + Environment.NewLine +
                                    "ポート番号は1～65535の範囲内で設定してください。", "避難所システム", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "btnTCPSave_Click", "ポート番号に使用不可能な値が設定される");
                }

            }

            // チェックがOKの時のみ、IPアドレスとポート番号を更新する
            if (chkAddress && chkIpNo)
            {
                // TCP/IP通信を一旦終了
                m_mainForm.m_bTcpIpStop = true;
                System.Threading.Thread.Sleep(1100);

                // 新しいIPアドレス、ポート番号を設定
                Program.m_objShelterAppConfig.IPAddress = txtIpAddr.Text.Replace(" ", "");
                Program.m_objShelterAppConfig.PortNo = txtPort.Text.Replace(" ", "");

                // TCP/IP通信を再開
                m_mainForm.m_bTcpIpStop = false;
                TcpPersonalThread tcpParsonalThread = new TcpPersonalThread();
                tcpParsonalThread.startTcpPersonalThread(m_mainForm, false);
                Close();
            }
            else
            {
                FormTcpSetting_Load(sender, e);
            }
        }

        /// <summary>
        /// 避難所情報システム - 個人安否情報受信設定ダイアログ　ロード時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormTcpSetting_Load(object sender, EventArgs e)
        {
            // IPアドレス、ポート番号をセットする
            txtIpAddr.Text = Program.m_objShelterAppConfig.IPAddress;
            txtPort.Text = Program.m_objShelterAppConfig.PortNo;
        }

        /// <summary>
        /// ポート番号入力欄に番号以外の入力を不可能にする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 制御文字は入力可
            if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            // 入力可能文字は0～9のみ
            else if ('0' <= e.KeyChar && e.KeyChar <= '9')
            {
                e.Handled = false;
            }
            // バックスペース(文字の削除)を許可
            else if (e.KeyChar == '\b')
            {
                e.Handled = false;
            }
            // 上記以外は入力不可
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// IPアドレス入力欄に番号、「.」以外の入力を不可能にする
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtIpAddr_KeyPress(object sender, KeyPressEventArgs e)
        {    
            // 制御文字は入力可
            if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            // 入力可能文字は0～9のみ
            else if ('0' <= e.KeyChar && e.KeyChar <= '9')
            {
            }
            // バックスペース(文字の削除)を許可
            else if (e.KeyChar == '\b')
            {
            }
            // 「.」(ドット)文字を許可
            else if (e.KeyChar == '.')
            {
            }
            // 上記以外は入力不可
            else
            {
                e.Handled = true;
            }
        }
    }
}
