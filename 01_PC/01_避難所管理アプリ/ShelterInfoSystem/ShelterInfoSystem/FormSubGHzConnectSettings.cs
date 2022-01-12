using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace ShelterInfoSystem
{
    public partial class FormSubGHzConnectSettings : Form
    {
        /**
         * @brief 通信接続状態定義
         */
        enum statConnected
        {
            Disconnected = 0,   /* 未接続状態 */
            Connecting,         /* 接続中状態 */
            ModuleConnect,      /* 無線ドングル接続状態 */
            Connected,          /* 通信端末接続状態 */
            Disconnecting       /* 切断中状態 */
        }
        /**
         * @brief 送受信テスト状態定義
         */
        enum statTest
        {
            NotTesting = 0,        /* テスト実施していない */
            SendTesting,           /* 送信テスト実施中 */
            RecvTesting            /* 受信テスト実施中 */
        }

        enum statCheck
        {
            SUBGHZ_OK = 0,
            SUBGHZ_NG
        }

        private statConnected m_StatConnect;
        private Boolean m_IsSaved = true;
        private FormWait m_FrmWait = new FormWait();

        private bool m_DeviceSerching = false;  // 端末検索状態
        private int m_SearchDeviceMode = 1; // 再送完了あり（複数検索）

        private SubGHz m_SubG;

        // 再送試行回数最大
        private const int SEND_RETRY_MAX = 5;

        // 端末情報取得要求応答タイムアウト時間(秒)
        private int GET_QANPI_ID_TIME_OUT = 10;


        public FormSubGHzConnectSettings()
        {
            InitializeComponent();
        }

        /**
         * @brief フォーム画面読み込み時メソッド
         */
        private void FormSubGHzConnectSettings_Load(object sender, EventArgs e)
        {
            bool isSave = true;
            // 初期表示にする
            this.btnSaveSettings.Enabled = false;
            this.toolStripStatusLabel1.Text = "";
            this.txtTestResult.Text = "";
            foreach (string s in SerialPort.GetPortNames())
            {
                cmbComPort.Items.Add(s);
            }

            // チャネル初期表示
            for (int i = 24; i <= 37; i++)
            {
                if (i == 32)
                {
                    continue;
                }
                cmbChannel.Items.Add("" + i + " ch");
            }

            // 設定ファイルを反映
            if (Program.m_SubGHzConfig.COMPort != null && m_IsSaved == true)
            {
                this.cmbComPort.SelectedItem = Program.m_SubGHzConfig.COMPort;
                this.txtSrcId.Text = Program.m_SubGHzConfig.Src_ID;
                this.cmbChannel.Text = Program.m_SubGHzConfig.Channel;
                this.txtDstId.Text = Program.m_SubGHzConfig.Dst_ID;
            }
            else
            {
                // 設定ファイルがない場合
                cmbComPort.SelectedText = "";
            }

            // [デモ用仕様]ドングル名のCOMポートを指定する
//            String demoComport = getDevicePort("ATEN USB to Serial Bridge");
            String demoComport = getDevicePort("USB Serial Port");
            if (demoComport != null)
            {
                this.cmbComPort.SelectedIndex = this.cmbComPort.FindStringExact(demoComport);
            }

            // サブギガ通信スレッドを取得
            m_SubG = SubGHz.GetInstance();

            // 通信接続状態によって表示を切り替える
            m_StatConnect = statConnected.Disconnected;

            if (m_SubG.GetStatSubGHz() == SubGHz.statSubGHz.Disconnected)
            {
                // 未接続（デフォルト）
            }
            else
            {
                // 接続中
                m_StatConnect = statConnected.Connected;
                if (Program.m_SubGHzConfig.Channel != m_SubG.GetChannel().ToString() + " ch")
                {
                    cmbChannel.Text = m_SubG.GetChannel().ToString() + " ch";
                    isSave = false;
                }
                if (Program.m_SubGHzConfig.COMPort != m_SubG.GetPortName())
                {
                    cmbComPort.Text = m_SubG.GetPortName();
                    isSave = false;
                }
            }

            ConnectChanged(m_StatConnect);

            // 保存済みか未保存か
            if (isSave)
            {
                m_IsSaved = true;
                this.btnSaveSettings.Enabled = false;
            }
            else
            {
                m_IsSaved = false;
                this.btnSaveSettings.Enabled = true;
            }

            m_SubG.MessageEvent += new SubGHz.EventMessageDelegate(eventConnecting);
        }


        /**
         * @brief 閉じるボタン押下メソッド
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

            m_FrmWait = null;

            m_SubG.MessageEvent -= new SubGHz.EventMessageDelegate(eventConnecting);

            Close();
        }

         
       /**
         * @brief 入力チェック
         */
        private statCheck checkSubGHzSettings()
        {

            return statCheck.SUBGHZ_OK;
        }

        /**
         * @brief 保存ボタン押下メソッド
         */
        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            // 設定値保存処理
            // COMPort, Channel
            // ---
            statCheck result = checkSubGHzSettings();
            if (result == statCheck.SUBGHZ_NG)
            {
                return;
            }

            Program.m_SubGHzConfig.COMPort = cmbComPort.Text;
            Program.m_SubGHzConfig.Src_ID = txtSrcId.Text;
            Program.m_SubGHzConfig.Dst_ID = txtDstId.Text;
            Program.m_SubGHzConfig.Channel = cmbChannel.Text;

            Program.m_SubGHzConfig.Save();

            m_IsSaved = true;
            this.btnSaveSettings.Enabled = false;
            this.toolStripStatusLabel1.Text = "保存しました";
        }

        /**
         * @brief 切断ボタン押下メソッド
         */
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (m_StatConnect == statConnected.Connected)
            {
                DialogResult result = MessageBox.Show("通信を切断しますがよろしいですか？", "切断確認",
                                                        MessageBoxButtons.OKCancel,
                                                        MessageBoxIcon.Exclamation,
                                                        MessageBoxDefaultButton.Button2);
                if (result == DialogResult.OK)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "FormSubGHzConnectSettings", "btnDisconnect_Click", "通信切断開始");
                    //m_FrmWait.Show(this);
                    ConnectChanged(statConnected.Disconnecting);

                    Program.m_RtnThreadSend.ClearSendQue();
                    // AnpiSS通信切断
                    //Program.m_L1sRecv.disconnect();
                    Program.m_EquStat.disconnect();
                    //Program.m_FwdRecv.disconnect();

                    // SubGHz通信切断処理開始
                    // ---
                    Program.m_SubGHz.DisconnectSubGHz();
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
         * @brief 接続ボタン押下メソッド
         */
        private void btnConnect_Click(object sender, EventArgs e)
        {
            // 初回疎通確認の為、通信中状態にする（他機能のQ-ANPIターミナル送信を不可にする）
            Program.m_SendFlag = Program.SENDING_INITIALIZE_INFO;
            Program.m_mainForm.ShelterStatusView();

            // 避難所管理画面のステータスバーの表記をクリア
            Program.m_mainForm.setToolStripStatusLabel(FormShelterInfo.LABEL.ALL, "");

            string s = txtDstId.Text;
            if (string.IsNullOrEmpty(s))
            { 
                return;
            }

            // デバイスID入力チェック
            if (s.Length != 8)
            {
                // 桁数エラー
                toolStripStatusLabel1.Text = "接続先のデバイスIDが正しく入力されていません";
                return;
            }

            int er = 0;
            foreach (char c in s)
            {
                if (!Uri.IsHexDigit(c))
                {
                    er = 1;
                    break;
                }
            }
            if (er == 1)
            {
                // 16進数以外
                toolStripStatusLabel1.Text = "接続先のデバイスIDが正しく入力されていません";
                return;
            }

            // 接続先確定
            Program.m_SubGHzConfig.Dst_ID = s;

            // サブギガ通信接続処理開始
            if (m_StatConnect == statConnected.ModuleConnect || m_StatConnect == statConnected.Connecting)
            {
                // ウェイトダイアログは2つ表示しない
                if (!m_FrmWait.Visible)
                {
                    m_FrmWait.Show(this);
                }

                ConnectChanged(statConnected.Connecting);
                //Application.DoEvents();

                // 接続処理(初期化処理)
                // ---
                Program.m_SubGHz.ConnectSubGHz(cmbComPort.Text, cmbChannel.Text);
            }
        }

        /**
         * @brief 通信接続処理時表示デリゲート用イベント
         * @param[in] sendor 呼び出し元オブジェクト
         * @param[in] code   処理コード
         * @param[in] msg    処理メッセージ
         * @param[in] outVal 処理番号 0:接続 1:切断 2:検索
         * @return    void
         */
        public void eventConnecting(object sender, int code, string msg, int outVal)
        {
            // 接続処理
            if (outVal == SubGHz.CONNECTING_CONNECT)
            {
                try
                {
                    m_FrmWait.Hide();
                }
                catch
                {
                }
                this.toolStripStatusLabel1.Text = msg;
                if (code == 0 || code == -1)
                {
                    // 接続失敗
                    ConnectChanged(statConnected.Disconnected);
                }
                else if (code == 1)
                {
                    // 初期化時、DEVICE_ID取得
                    txtSrcId.Text = msg;
                    Program.m_SubGHzConfig.Src_ID = msg;
                    ConnectChanged(statConnected.ModuleConnect);
                }
                else if (code == 2)
                {
                    // 初期化時、回線確認応答
                    // ************ 接続 ************
                    // 装置ステータス
                    Program.m_SubGHz.setSendInterval(80);
                    Program.m_EquStat.connect("", 3005);
                    Program.m_EquStat.sendReq(true);
                    System.Threading.Thread.Sleep(200);
                    // L1S/CID・UID
                    Program.m_L1sRecv.connect("", 3610);

                    // 端末情報取得要求
                    GetQANPIInfoThread();
                    //Program.m_L1sRecv.sendReq(40);    // 災害危機通報取得要求をコメントアウト
#if true
                    // FWD要求（通信量低減する場合OFFも可能）
                    Program.m_FwdRecv.sendReq();
#endif
                    ConnectChanged(statConnected.Connected);
                }
                else if (code == 3)
                {
                    // チャネルセットのみ
                    this.toolStripStatusLabel1.Text = msg;
                }
                else if (code == 4)
                {
                    // デバイス検索
                    this.toolStripStatusLabel1.Text = msg;
                }
                else if (code == 5)
                {
                    // UART設定完了
                    ConnectChanged(statConnected.ModuleConnect);
                }
                else
                {
                    // N/A 
                    this.toolStripStatusLabel1.Text = "メッセージエラー:" + msg;

                }
            }
            // 切断処理
            else if (outVal == SubGHz.CONNECTING_DISCONNECT)
            {
                //m_FrmWait.Hide();
                this.toolStripStatusLabel1.Text = msg;
                if (code == 0)
                {
                    // 切断失敗
                    ConnectChanged(statConnected.Connected);
                }
                else if (code == 1)
                {
                    // サブギガ通信切断完了時表示
                    m_DeviceSerching = false;
                    ConnectChanged(statConnected.Disconnected);
                }
                else
                {
                    // N/A 
                    this.toolStripStatusLabel1.Text = "メッセージエラー5";
                }
            }
            // 衛星通信端末検索処理
            else if (outVal == SubGHz.CONNECTING_SEARCHDEVICE)
            {
                if (code == 0)
                {
                    // 検索失敗
                    txtTestResult.AppendText("デバイス検索エラー");
                    m_DeviceSerching = false;
                }
                else if( code == SubGHz.MSGID_SEARCH_10 )
                {
                    // デバイスID１つめ表示
                    if (txtDstId.Text.Length == 0) { 
                        int idx = msg.IndexOf("DeviceID:");
                        if (idx == 0 && msg.Length > 17)
                        {
                            string dst_id = msg.Substring(9, 8);
                            txtDstId.Text = dst_id;
                            Program.m_SubGHzConfig.Dst_ID = dst_id;
                        }
                    }
                    // デバイス検索結果表示
                    txtTestResult.AppendText(msg);
                }
                else if (code == SubGHz.MSGID_RESEND_12)
                {
                    // 再送完了
                    txtTestResult.AppendText("-------- 端末検索終了 --------\r\n");
                    m_DeviceSerching = false;
                }
                else if (code == -1 && m_DeviceSerching)
                {
                    // タイムアウト通知により終了
                    this.toolStripStatusLabel1.Text = msg;
                    txtTestResult.AppendText("-------- 端末検索終了 --------\r\n");
                    m_DeviceSerching = false;
                }
                else
                {
                    txtTestResult.AppendText("デバイス検索エラー2");
                    m_DeviceSerching = false;
                }

                // 端末検索終了時に表示を切り替える
                if (m_DeviceSerching == false)
                {
                    ConnectChanged(m_StatConnect);
                }
            }
            // 自デバイスID取得
            else if (outVal == SubGHz.CONNECTING_GETDEVICE)
            {
                // DEVICE_ID 取得結果
                if (msg.Length == 8)
                {
                    txtSrcId.Text = msg;
                    Program.m_SubGHzConfig.Src_ID = msg;
                }
                this.toolStripStatusLabel1.Text = "デフォルト設定（DEVICE_ID : " + msg + "）";
            }
            else
            {
                this.toolStripStatusLabel1.Text = "メッセージエラー6";
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
                this.btnSetComPort.Visible = false;
                this.btnDisableComPort.Visible = true;
                this.btnConnect.Visible = false;
                this.btnDisconnect.Visible = true;

                // 設定項目活性/非活性
                this.lblComPort.ForeColor = SystemColors.ControlDark;
                this.cmbComPort.Enabled = false;
                this.btnSetComPort.Enabled = false;
                this.btnDisableComPort.Enabled = false;

                this.lblChannel.ForeColor = SystemColors.ControlDark;
                this.cmbChannel.Enabled = false;
                this.btnSetChannel.Enabled = false;

                this.lblSrcId.ForeColor = SystemColors.ControlText;
                this.txtSrcId.Enabled = true;
                this.btnGetSrcId.Enabled = true;

                this.lblDstId.ForeColor = SystemColors.ControlDark;
                this.txtDstId.Enabled = false;
                this.btnDeviceSearch.Enabled = true;
                this.btnConnect.Enabled = false;
                this.btnDisconnect.Enabled = true;

                if (m_DeviceSerching)
                {
                    // 端末検索中の場合
                    this.btnDeviceSearch.Enabled = false;
                    this.btnGetSrcId.Enabled = false;
                }

            }
            // 無線ドングル接続状態の表示
            else if (stat == statConnected.ModuleConnect)
            {
                this.Enabled = true;
                this.btnSetComPort.Visible = false;
                this.btnDisableComPort.Visible = true;
                this.btnConnect.Visible = true;
                this.btnDisconnect.Visible = false;

                // 設定項目活性/非活性
                this.lblComPort.ForeColor = SystemColors.ControlDark;
                this.cmbComPort.Enabled = false;
                this.btnSetComPort.Enabled = false;
                this.btnDisableComPort.Enabled = true;

                this.lblChannel.ForeColor = SystemColors.ControlText;
                this.cmbChannel.Enabled = true;
                this.btnSetChannel.Enabled = true;

                this.lblSrcId.ForeColor = SystemColors.ControlText;
                this.txtSrcId.Enabled = true;
                this.btnGetSrcId.Enabled = true;

                this.lblDstId.ForeColor = SystemColors.ControlText;
                this.txtDstId.Enabled = true;
                this.btnDeviceSearch.Enabled = true;
                if (this.txtDstId.Text.Length == 0)
                {
                    this.btnConnect.Enabled = false;        // DstIdが空欄であれば接続ボタン非活性
                }
                else
                {
                    this.btnConnect.Enabled = true;
                }
                if (m_DeviceSerching)
                {
                    // 端末検索中の場合
                    this.btnDisableComPort.Enabled = false;
                    this.btnSetChannel.Enabled = false;
                    this.cmbChannel.Enabled = false;
                    this.btnGetSrcId.Enabled = false;
                    this.btnDeviceSearch.Enabled = false;
                    this.btnConnect.Enabled = false;
                }

                // 装置ステータス
                Program.m_EquStat.disconnect();
                // L1S/CID・UID
                Program.m_L1sRecv.disconnect();
                   
            }
            // 未接続状態の表示
            else if (stat == statConnected.Disconnected)
            {
                this.Enabled = true;
                this.btnSetComPort.Visible = true;
                this.btnDisableComPort.Visible = false;
                this.btnConnect.Visible = true;
                this.btnDisconnect.Visible = false;

                // 設定項目活性/非活性
                this.lblComPort.ForeColor = SystemColors.ControlText;
                this.cmbComPort.Enabled = true;
                this.btnSetComPort.Enabled = true;
                this.btnDisableComPort.Enabled = false;

                this.lblChannel.ForeColor = SystemColors.ControlDark;
                this.cmbChannel.Enabled = false;
                this.btnSetChannel.Enabled = false;

                this.lblSrcId.ForeColor = SystemColors.ControlDark;
                this.txtSrcId.Enabled = false;
                this.btnGetSrcId.Enabled = false;

                this.lblDstId.ForeColor = SystemColors.ControlDark;
                this.txtDstId.Enabled = false;
                this.btnDeviceSearch.Enabled = false;
                this.btnConnect.Enabled = false;

                // 装置ステータス
                Program.m_EquStat.disconnect();
                // L1S/CID・UID
                Program.m_L1sRecv.disconnect();
            }
            // 接続中状態の表示
            else if (stat == statConnected.Connecting)
            {
                this.toolStripStatusLabel1.Text = "接続中のため操作できません。";
            }
            // 切断中状態の表示
            else if (stat == statConnected.Disconnecting)
            {
                this.toolStripStatusLabel1.Text = "切断中のため操作できません。";
            }
            else
            {
                // N/A
            }
            m_StatConnect = stat;
        }

        /**
        * @brief COMポート設定値変更時処理
        */
        private void cmbComPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_IsSaved = false;
            this.btnSaveSettings.Enabled = true;
        }

        /**
         * @brief Channel設定変更時処理
         */
        private void cmbChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_IsSaved = false;
            this.btnSaveSettings.Enabled = true;
        }

        /**
         * @brief SrcId取得ボタン押下時処理
         */
        private void btnGetSrcId_Click(object sender, EventArgs e)
        {
            // 設定COMポートのモジュールよりDeviceIDを取得する
            // ---
            if (m_StatConnect == statConnected.Disconnected)
            {
                // 未接続
                this.toolStripStatusLabel1.Text = "未接続です";
                return;
            }

            //txtTestResult.AppendText("デフォルト設定を取得します\r\n");
            Program.m_SubGHz.GetDeviceID();
        }

        private void btnDeviceSearch_Click(object sender, EventArgs e)
        {
            if (m_StatConnect == statConnected.Disconnected)
            {
                // 未接続
                this.toolStripStatusLabel1.Text = "未接続です";
                return;
            }
            // 端末検索状態
            m_DeviceSerching = true;
            ConnectChanged(m_StatConnect);

            // モジュール接続の場合、検索IDを反映するためデバイスIDをクリア
            if (m_StatConnect == statConnected.ModuleConnect)
            {
                txtDstId.Text = "";
            }

            txtTestResult.Text = "";
            txtTestResult.AppendText( "-------- 端末検索開始 --------\r\n");

            Program.m_SubGHz.SearchDevice(m_SearchDeviceMode);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtTestResult.Text = "";
        }

        /**
         * @brief COMポート設定ボタン押下
         */
        private void btnSetComPort_Click(object sender, EventArgs e)
        {
            // 無線モジュール接続処理
            bool bRet = false;
            if (this.cmbComPort.SelectedItem == null)
            {
                // 未選択
                return;
            }

            bRet = Program.m_SubGHz.OpenSerialSubGHz(this.cmbComPort.SelectedItem.ToString());
            if (bRet == false)
            {
                // OPEN 失敗
                return;
            }
            ConnectChanged(statConnected.ModuleConnect);
            ConnectChanged(statConnected.Disconnected);
        }

        /**
         * @brief COMポート設定解除ボタン押下
         */
        private void btnDisableComPort_Click(object sender, EventArgs e)
        {
            // 無線モジュール切断処理
            Program.m_SubGHz.DisconnectSubGHz();
        }

        /**
         * @brief チャネル設定ボタン押下
         */
        private void btnSetChannel_Click(object sender, EventArgs e)
        {
            if (cmbChannel.SelectedItem == null)
            {
                // 未選択
                return;
            }
            // チャネル設定処理
            bool bRet = Program.m_SubGHz.SetChannelSubGHz(cmbChannel.SelectedItem.ToString());
        }

        /**
         * @brief 接続先デバイスID変更時
         */
        private void txtDstId_TextChanged(object sender, EventArgs e)
        {
            m_IsSaved = false;
            this.btnSaveSettings.Enabled = true;

            // テキストボックスが空欄の場合は接続ボタンを非活性にする
            if (!m_DeviceSerching)
            {
                if (this.txtDstId.TextLength == 0)
                {
                    this.btnConnect.Enabled = false;
                }
                else
                {
                    this.btnConnect.Enabled = true;
                }
            }
        }

        /**
         * @brief 指定文字列のデバイスが接続されているポートを取得する
         */
        private string getDevicePort(string deviceName)
        {
            var deviceNameList = new System.Collections.ArrayList();
            var check = new System.Text.RegularExpressions.Regex("COM[1-9][0-9]?[0-9]?");

            ManagementClass mcPnPEntity = new ManagementClass("Win32_PnPEntity");
            ManagementObjectCollection manageObjCol = mcPnPEntity.GetInstances();

            //全てのPnPデバイスを探索しシリアル通信が行われるデバイスを随時追加する
            foreach (ManagementObject manageObj in manageObjCol)
            {
                //Nameプロパティを取得
                var namePropertyValue = manageObj.GetPropertyValue("Name");
                if (namePropertyValue == null)
                {
                    continue;
                }
                
                string name = namePropertyValue.ToString();
                if (name.StartsWith(deviceName))
                {
                    // 該当デバイスがあったためポート名を返す
                    var res = check.Match(name);
                    if(res.Success)
                    {
                        return res.Value;
                    }
                }
            }

            // 該当デバイスなし
            return null;
        }

        /// <summary>
        /// 端末情報取得要求送信(リトライ有り)
        /// </summary>
        private void GetQANPIInfoThread()
        {
            // 避難所管理画面のステータスバーに「端末情報取得中」を表示する
            Program.m_mainForm.setToolStripStatusLabel(FormShelterInfo.LABEL.L1S, "端末情報取得中   ");
            Program.m_mainForm.m_isTerminalDeviceInfoOK = false;

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {

                FormWait frmWait = new FormWait();
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    frmWait.Text = "端末情報取得中";
                    if (Program.m_mainForm.m_frmSubGhzSettings != null)
                    {
                        frmWait.ShowDialog(Program.m_mainForm.m_frmSubGhzSettings);
                    }
                    else if (Program.m_mainForm.m_frmSubGhzTest != null)
                    {
                        frmWait.ShowDialog(Program.m_mainForm.m_frmSubGhzTest);
                    }

                    //frmWait.TopMost = true;
                }
                );

                int getTyrCount = 0;
                int TyrCount = 0;
                int tryTime = GET_QANPI_ID_TIME_OUT;

                // 端末情報取得要求送信(1回目)
                Program.m_L1sRecv.sendReq(41);

                while (true)
                {
                    // Q-ANPIターミナルのIDが取得出来たら終了
                    if ((Program.m_mainForm.m_isTerminalDeviceInfoOK))
                    {
                        Program.m_mainForm.setToolStripStatusLabel(FormShelterInfo.LABEL.ALL, "");
                        break;
                    }

                    // タイムアウト時間を超えたら再度初回接続を送信する
                    if (getTyrCount > tryTime)
                    {
                        // 端末情報取得要求送信
                        Program.m_L1sRecv.sendReq(41);

                        // 次の送信時間をセット
                        tryTime += GET_QANPI_ID_TIME_OUT;
                        TyrCount++;
                    }

                    // 1秒ごとにチェック
                    System.Threading.Thread.Sleep(1000);
                    getTyrCount++;

                    // 切断されたら終了
                    if (!Program.m_EquStat.isConnected())
                    {
                        Program.m_mainForm.setToolStripStatusLabel(FormShelterInfo.LABEL.ALL, "");
                        break;
                    }

                    // TRY上限回数を超えたら切断し、エラーメッセージを表示する
                    if (TyrCount > SEND_RETRY_MAX)
                    {
                        Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "SubGHz", "SendInitData", "端末情報取得出来ない為切断");
                        Program.m_RtnThreadSend.ClearSendQue();
                        Program.m_EquStat.disconnect();
                        Program.m_SubGHz.DisconnectSubGHz();

                        MessageBox.Show("端末の情報が取得できませんでした。\n端末の状態を確認し、再接続してください。", "避難所情報システム",
                                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        break;
                    }

                    // 避難所管理画面のステータスバーに「端末情報取得中」を表示する
                    string str = "";
                    if ((getTyrCount % 3) == 0)
                    {
                        str = "端末情報取得中.  ";
                    }
                    else if ((getTyrCount % 3) == 1)
                    {
                        str = "端末情報取得中.. ";
                    }
                    else
                    {
                        str = "端末情報取得中...";
                    }
                    Program.m_mainForm.setToolStripStatusLabel(FormShelterInfo.LABEL.L1S, str);
                }
                frmWait.Hide();
            });
        }
    }
}
