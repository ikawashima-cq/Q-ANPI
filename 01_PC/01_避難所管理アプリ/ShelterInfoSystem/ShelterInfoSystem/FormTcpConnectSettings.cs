/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2017. All rights reserved.
 */
/**
 * @file    FormTcpConnectSettings.cs
 * @brief   Tcp通信設定画面フォームの定義
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace ShelterInfoSystem
{
    /**
     * @class FormTcpConnectSettings
     * @brief TCP通信設定画面のフォームクラス
     */ 
    public partial class FormTcpConnectSettings : Form
    {
        /**
         * @brief 通信接続状態定義
         */
        enum statConnected
        {
            Disconnected = 0,   /* 未接続状態 */
            Connecting,         /* 接続中状態 */
            Connected,          /* 接続状態 */
            Disconnecting       /* 切断中状態 */
        }
        private Boolean m_IsSaved = true;

        private statConnected m_StatConnect;

        /**
         * @brief コンストラクタ
         */
        public FormTcpConnectSettings()
        {
            InitializeComponent();
            m_StatConnect = statConnected.Disconnected;
            m_IsSaved = true;
        }

        /**
         * @brief フォーム画面読み込み時メソッド
         */
        private void FormTcpConnectSettings_Load(object sender, EventArgs e)
        {

            // 初期表示にする
            this.toolStripStatusLabel1.Text = "";

            if (Program.m_L1sRecv.isConnected() == true
                && Program.m_EquStat.isConnected() == true
                && Program.m_FwdRecv.isConnected() == true)
            {
                m_StatConnect = statConnected.Connected;
            }
            else
            {
                Program.m_L1sRecv.disconnect();
                Program.m_EquStat.disconnect();
                Program.m_FwdRecv.disconnect();
            }


            if (m_StatConnect == statConnected.Connected)
            {
                // 接続済みの場合は接続中のIPアドレス表示
                this.txtIpAddr.Text = Program.m_TcpIpAddr;
                if (Program.m_TcpIpAddr == Program.m_objTcpConfig.IP)
                {
                    m_IsSaved = true;
                    this.btnSave.Enabled = false;
                }
                else
                {
                    m_IsSaved = false;
                    this.btnSave.Enabled = true;
                }
            }
            else
            {
                if (Program.m_objTcpConfig.IP != null)
                {
                    // 未接続の場合は設定ファイルのIPを表示する
                    this.txtIpAddr.Text = Program.m_objTcpConfig.IP;
                }
                else
                {
                    // 無ければ空欄とする
                    this.txtIpAddr.Text = "";
                }
                m_IsSaved = true;
                this.btnSave.Enabled = false;
            }


            // 通信接続状態によって表示を切り替える
            ConnectChanged(m_StatConnect);
        }

        /**
         * @brief 通信接続処理時表示デリゲート用イベント
         * @param[in] sendor 呼び出し元オブジェクト
         * @param[in] stat   処理ステータス
         * @param[in] code   処理コード
         * @param[in] msg    処理メッセージ
         * @return    void
         */
        public void eventConnecting(object sender, int code, string msg, string outVal)
        {
            this.toolStripStatusLabel1.Text = msg;
            if (code == 0)
            {
                // 接続失敗
                ConnectChanged(statConnected.Disconnected);
            }
            else if (code == 1)
            {
                // 接続完了時表示
                ConnectChanged(statConnected.Connected);
            }
            else
            {
                // N/A 
            }
        }

        /**
         * @brief 通信切断処理時表示デリゲート用イベント
         * @param[in] sendor 呼び出し元オブジェクト
         * @param[in] stat   処理ステータス
         * @param[in] code   処理コード
         * @param[in] msg    処理メッセージ
         * @return    void
         */
        public void eventDisconnecting(object sender, int code, string msg, string outVal)
        {
            this.toolStripStatusLabel1.Text = msg;
            if (code == 0)
            {
                // 切断失敗
                ConnectChanged(statConnected.Connected);
            }
            else if (code == 1)
            {
                // 切断完了時表示
                ConnectChanged(statConnected.Disconnected);
            }
            else
            {
                // N/A 
            }
        }

        /**
         * @brief 接続状態表示の切り替え
         */
        private void ConnectChanged(statConnected stat)
        {
            // 通信接続状態表示
            if (stat == statConnected.Connected)
            {
                this.Enabled = true;
                this.btnConnect.Visible = false;
                this.btnDisconnect.Visible = true;

                // 設定項目を変更不可にする
                this.txtIpAddr.Enabled = false;
            }
            // 未接続状態の表示
            else if (stat == statConnected.Disconnected)
            {
                this.Enabled = true;
                this.btnConnect.Visible = true;
                this.btnDisconnect.Visible = false;

                // 設定項目を変更可にする
                this.txtIpAddr.Enabled = true;
            }
            // 接続中状態の表示
            else if (stat == statConnected.Connecting)
            {
                this.Enabled = false;
            }
            // 切断中状態の表示
            else if (stat == statConnected.Disconnecting)
            {
                this.Enabled = false;
            }
            else
            {
                // N/A
            }
            m_StatConnect = stat;
        }

        /* ----------------- ボタン処理 ---------------- */
        /**
         * @brief : 閉じるボタン押下処理
         */
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (!m_IsSaved)
            {
                DialogResult result = MessageBox.Show("保存せずに終了しますがよろしいですか？", "保存確認",
                                        MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Exclamation,
                                        MessageBoxDefaultButton.Button2);
                if (result == DialogResult.OK)
                {
                    //Close();
                }
                else if (result == DialogResult.Cancel)
                {
                    // 何もせず戻る
                    return;
                }
                else
                {
                    // N/A
                }
            }
            Close();
        }

        /**
         * @brief:接続ボタン押下処理
         */
        private void btnConnect_Click(object sender, EventArgs e)
        {
            // 指定したIpAddressでポートに接続する
            if (m_StatConnect == statConnected.Disconnected)
            {
                string ip = txtIpAddr.Text;
                int port;

                // 入力IPアドレス検査
                if (!checkIpAddress(ip))
                {
                    eventConnecting(this, 0, "IPアドレス入力エラー","");
                    return;
                }

                Program.m_TcpIpAddr = ip;

                // L1S
                port = 3610;
                if (Program.m_L1sRecv.connect(ip, port) != L1sReceiver.Result.OK)
                {
                    eventConnecting(this, 0, "TCP/IP接続エラー(" + port + ")","");
                    disconnectAll();
                    return;
                }
                Program.m_L1sRecv.sendReq(L1sReceiver.MSG_TYPE_L1S);
                Program.m_L1sRecv.sendReq(L1sReceiver.MSG_TYPE_CID);

                // EQU
                port = 3005;
                if (Program.m_EquStat.connect(ip, port) != TermInfo.Result.OK)
                {
                    eventConnecting(this, 0, "TCP/IP接続エラー(" + port + ")","");
                    disconnectAll();
                    return;
                }

                // FWD
                port = 3090;
                if (Program.m_FwdRecv.connect(ip, port) != FwdReceiver.Result.OK)
                {
                    eventConnecting(this, 0, "TCP/IP接続エラー(" + port + ")", "");
                    disconnectAll();
                    return;
                }

                // RTN
                if (TcpRtnThread.setIP(ip) == RtnThread.Result.NG)
                {
                    eventConnecting(this, 0, "TCP/IP接続エラー(" + port + ")", "");
                    disconnectAll();
                    return;
                }

                ConnectChanged(statConnected.Connecting);
                eventConnecting(this, 1, "接続しました", "");
            }
        }

        /**
         * @brief:切断ボタン押下処理
         */
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            // Open状態のポートを切断する
            if (m_StatConnect == statConnected.Connected)
            {
                DialogResult result = MessageBox.Show("通信を切断しますがよろしいですか？", "切断確認",
                                                        MessageBoxButtons.OKCancel,
                                                        MessageBoxIcon.Exclamation,
                                                        MessageBoxDefaultButton.Button2);
                if (result == DialogResult.OK)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "FormTcpConnectSettings", "btnDisconnect_Click", "切断開始");
                    Program.m_RtnThreadSend.ClearSendQue();
                    disconnectAll();

                    ConnectChanged(statConnected.Disconnecting);
                    eventDisconnecting(this, 1, "切断しました", "");
                }
                else if (result == DialogResult.Cancel)
                {
                    // 何もせず戻る
                    return;
                }
                else
                {
                    // N/A
                }
            }
        }

        /**
         * @brief:保存ボタン押下処理
         */
        private void btnSave_Click(object sender, EventArgs e)
        {
            // configファイルにIpAddressの値を保存する
            Program.m_objTcpConfig.IP = this.txtIpAddr.Text;

            if (Program.m_objTcpConfig.Save() == true)
            {
                m_IsSaved = true;
                this.btnSave.Enabled = false;
                this.toolStripStatusLabel1.Text = "保存しました";
            }
            else
            {
                this.toolStripStatusLabel1.Text = "保存に失敗しました";
            }
        }


        /**
         * @brief:IPアドレス設定KeyPressイベント
         */
        private void txtIpAddr_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == '\b' || e.KeyChar == '.')
            {
                //押下キーが0～9、.、BackSpaceでない場合はイベントをキャンセル
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        /**
         * @brief:IPアドレス設定値変更時処理
         */
        private void txtIpAddr_TextChanged(object sender, EventArgs e)
        {
            m_IsSaved = false;
            this.btnSave.Enabled = true;
        }


        /**
         * @brief:切断処理
         */
        private void disconnectAll()
        {
            if (Program.m_L1sRecv.isConnected())
            {
                Program.m_L1sRecv.disconnect();
            }
            if (Program.m_EquStat.isConnected())
            {
                Program.m_EquStat.disconnect();
            }
            if (Program.m_FwdRecv.isConnected())
            {
                Program.m_FwdRecv.disconnect();
            }
        }


        /**
         * @brief:IPアドレスフォーマット検査
         */
        private bool checkIpAddress(string ip)
        {
            if (ip == "")
            {
                return false;
            }

            string[] strIpAddr;
            int addr = 0;
            char[] splt = {'.'};

            try
            {
                strIpAddr = ip.Split(splt);
                if (strIpAddr.Length != 4)
                {
                    return false;
                }
                for (int i = 0; i < 4; i++)
                {
                    addr = int.Parse(strIpAddr[i]);
                    if( addr < 0 || addr > 255 )
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
