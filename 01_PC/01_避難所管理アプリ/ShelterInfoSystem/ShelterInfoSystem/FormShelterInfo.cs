using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections;
using System.Diagnostics;        // Debug.WriteLine用
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using System.Net.Sockets;

// using System.Runtime.Serialization.Json を使用 するためには [プロジェクト]-[参照の追加]  [.Net]タブにて
// System.Runtime.Serializeation、System.Runtime.Serialization.Json を追加

using QAnpiCtrlLib.msg;

namespace ShelterInfoSystem
{
    public partial class FormShelterInfo : Form
    {
        // Q-ANPI端末メッセージ送信コマンド
        public enum SendMessageCommand
        {
            COMMAND_NONE = 0,
            OPEN_SHELTER,
            CLOSE_SHELTER,
            EDIT_SHELTER,
            EDIT_SHELTER_NO_POSITION,
            UPDATE_LOCATION,
            SEND_PERSON_NUM,
            SEND_PERSON,
            SEND_SHELTER_INFO,
            CHECK_NEW_MESSAGE,
            AUTO_SEND_NUM,
            AUTO_SEND_DETAIL,
            AUTO_SEND_NUM_DETAIL
        }

        // 避難所開設状態
        public enum SHELTER_STATUS
        {
            NO_OPEN = 0,
            OPEN = 1,
            CLOSE = 2
        }

        // メッセージ受信状態管理
        enum RCV_STATE
        {
            DISCONNECT = 0x00,
            CONNECT_L1S = 0x01,
            CONNECT_EQU = 0x02,
            CONNECT_FWD = 0x04,
        }

        // TYPE0,1,2 状態定義
        public enum STATE
        {
            OK = 0,         // 未送信
            SENDING,        // 送信中
            WAITING         // 送達確認待機中
        }

        // 画面下部ラベル表示
        public enum LABEL
        {
            APP = 0,    // アプリ状態の表示
            RTN_SND,    // RTN送信状態の表示
            RTN_RCV,    // RTN送信応答の表示    
            RTN_CHK,    // RTN送達確認の表示
            FWD,        // FWD受信状態の表示
            L1S,        // CID/L1S受信状態の表示
            CONNECT,    // 端末接続状態の表示
            ALL,        // 全Label
            TIME_SYNC,  // 時刻同期状態
        }

        /// 送信履歴用
        private struct SendData
        {
            public bool bEdit;
            public bool bDelete;
            public bool bSendWait;
            public DbAccess.PersonInfo info;

            public void init()
            {
                bEdit = false;
                bDelete = false;
                bSendWait = true;
                info = new DbAccess.PersonInfo();
            }
        }


        // 避難所詳細情報
        private string[,] m_TotalizationList =
        {
            {"1", "", "避難者数", "55"},
            {"2", "", "在宅者数", "0"},
            {"3", "", "男性", "30"},
            {"4", "", "女性", "25"},
            // 2016/04/18 救護を削除：これ以降を前詰
            //{"7", "", "要救護者数", "1"},
            {"5", "", "負傷者数", "0"},
            {"6", "", "要介護者数", "5"},
            {"7", "", "障がい者数", "1"},
            {"8", "", "妊産婦者数", "1"},
            {"9", "", "高齢者数", "0"},
            {"10", "", "乳児数", "3"},
            {"11", "", "避難所内", "3"},
            {"12", "", "避難所外", "3"},
            {"13", "", "避難所状況", ""},
        };

        // ログインフォーム
        public FormLogin m_frmLogin = null;

        // ウェイトフォーム
        private FormWait m_frmWait = new FormWait();

        // 装置状態フォーム
        private FormTermStatus m_frmTermStatus = null; // or null

        // TCP設定フォーム
        private FormTcpConnectSettings m_frmTcpSettings = null;

        // 通信設定フォーム
        public FormSubGHzConnectSettings m_frmSubGhzSettings = null;

        // 送受信テストフォーム
        public FormSubGHzConnectTest m_frmSubGhzTest = null;

        // 災害危機通報フォーム
        private FormReceiveMessageView m_frmReceiveMessage = null;

        // Q-ANPIターミナル通信時の実行コマンド
        public SendMessageCommand m_NowCommand = SendMessageCommand.COMMAND_NONE;

        // タイプ2が詳細情報送信か避難所名送信かを判断する(必ず名前→詳細の順なので、衛星からの送達確認が来たらtrue/falseを切り替える)
        public bool m_isType2subDetail = false;

        // 自動送信実施中フラグ
        public bool m_nowAutoSending = false;

        // 現在の避難所開設状態
        public SHELTER_STATUS m_ShelterStat = SHELTER_STATUS.NO_OPEN;

        // TCP/IP通信終了フラグ
        public Boolean m_bTcpIpStop = false;

        // 連続メッセージ用 送信中フラグ
        public bool m_nowSending = false;

        // 送信が完了した番号
        public int m_nowSendFinNo = 0;

        // 送信待ち番号
        public int m_nowWaitNo = 0;

        // 避難所開設状況メッセージ(Type0)の避難所状態保持用
        public DbAccess.TerminalInfo m_OpenCloseShelterInfo = new DbAccess.TerminalInfo();

        // 災害危機通報を現在受信中か(Q-ANPI端末の仕様上デフォルトONなので、避難所アプリ開始時にOFFにするメッセージを投げる)
        public bool m_isNowReceiving = true;

        // 個人安否情報リスト選択状態保持用
        private int m_SelectRow = 0;

        // 個人安否情報リスト最終表示行保持用
        private int m_ShowLastRow = 0;

        // カラムのオーダー(更新日時順/昇順/降順)制御
        private int m_order = 0;

        // 初期化中フラグ
        private bool m_initialize = true;

        // エクスポートフォルダ保持用
        public string m_ExportDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // インポートフォルダ保持用
        public string m_ImportDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // 監視ステータスチェック間隔　2秒=2000
        private const int EQU_STAT_INTERVAL = 8000; // 4000;

        // 救助支援情報リストデータ
        List<DbAccessStep2.RecvMsgInfo> m_l1sList = new List<DbAccessStep2.RecvMsgInfo>();

        // 救助支援情報リストデータ
        List<DbAccessStep2.RescueMsgInfo> m_RescueList = new List<DbAccessStep2.RescueMsgInfo>();

        // 救助支援情報選択
        private const int LISTVIEW_130 = 0;

        // 災害危機通報選択
        private const int LISTVIEW_L1S = 1;

        // 救助支援情報/災害危機通報選択デフォルト
        private int m_selected = LISTVIEW_130;

        // 表示メッセージ上限値
        private int LISTVIEW_MAX = 20;

        // 130タイマーカウンタ
        private int m_type130TimerCount = 0;

        // 130タイマー(救助支援情報受信待ち)を無効化するフラグ
        private bool m_isType130timerStop = false;

        // 130タイマー(救助支援情報受信待ち)をリセットするフラグ
        private bool m_isType130timerReset = false;

        // Q-ANPIターミナル接続状態
        private RCV_STATE m_rcvState = RCV_STATE.DISCONNECT;

        private const int TYPE2_SEND_STATE_DEFAULT = 0;
        private const int TYPE2_SEND_STATE_SEND = 1;
        private const int TYPE2_SEND_STATE_RESP = 2;

        // Type2 避難所情報
        private const int m_Type2CountMax_0 = 9;
        private STATE m_StateType2_0 = STATE.OK;
        private int m_Type2SendStartCount_0 = 0; // 送信数
        private int m_Type2WaitingCount_0 = 0; // 送達確認数
        private int[] m_CheckType2_0 = Enumerable.Repeat<int>(TYPE2_SEND_STATE_DEFAULT, m_Type2CountMax_0).ToArray();

        // Type2 避難所名
        private const int m_Type2CountMax_1 = 9;
        private STATE m_StateType2_1 = STATE.OK;
        private int m_Type2SendStartCount_1 = 0; // 送信数
        private int m_Type2WaitingCount_1 = 0; // 送達確認数
        private int[] m_CheckType2_1 = Enumerable.Repeat<int>(TYPE2_SEND_STATE_DEFAULT, m_Type2CountMax_1).ToArray();

        // Type1 個人安否情報
        private STATE m_StateType1 = STATE.OK;
        private int m_Type1SendStartCount = 0; // 送信数
        private int m_Type1SendingCount = 0; // 送信応答数
        private int m_Type1WaitingCount = 0; // 送達確認数
        private int m_Type1CountMax = 10;
        private int[] m_CheckType1 = new int[160];

        // Type0 端末情報（開設。閉鎖）
        private STATE m_StateType0 = STATE.OK;
        private int m_Type0SendStartCount = 0; // 送信数
        private int m_Type0SendingCount = 0; // 送信応答数
        private int m_Type0WaitingCount = 0; // 送達確認数
        private int m_Type0CountMax = 2;
        private int m_Type0SubType1ok = 0;

        // Type3 用
        private STATE m_StateType3 = STATE.OK;
        private bool m_Type3Last = false;
        private int m_Type3SendStartCount = 0; // 送信数
        private int m_Type3SendingCount = 0; // 送信応答数
        private int m_Type3WaitingCount = 0; // 送達確認数
        private int m_Type3RecvMsgCount = 0;    // 受信済みメッセージ数
        private int m_Type3NotRecvMsgCount = 0; // 未受信メッセージ数
        private int m_Type3CountMax = 1;
        public bool m_first130Get = false;

        // 共通要
        private int m_commonSendStartCount = 0; // 送信数
        private int m_commonSendingCount = 0; // 送信応答数
        private int m_commonWaitingCount = 0; // 送達確認数
        public int m_commonCountMax = 0;

        // RTN スレッド

        // リンクタイムスロットデータ
        private SlotSubSlot m_SlotSubSlot;

        // 送信データ
        private ArrayList m_sendDataArr = new ArrayList();

        // 送信データ総計
        private int m_nSndTotal = 0;

        // 送信したデータのカウント
        private int m_nSndCnt = 0;

        // 終了処理
        private Boolean m_bProcStop = false;

        // 装置ステータス異常状態管理
        private IndStatus m_TermStatusInd;

        // 送信する個人安否情報のリスト
        private List<SendData> m_PersonSendList = new List<SendData>();

        // 避難所詳細情報ログ(送信したデータのログ保持用）
        //private DbAccess.TotalSendLog m_TotalSendLog;
        private List<DbAccess.TotalSendLog> m_TotalSendLog = new List<DbAccess.TotalSendLog>();

        // 現在設定中の避難所
        private DbAccess.TerminalInfo m_ActiveTarminal_info;

        // 送信時刻保持用(送信時刻を送信ボタンを押した時刻に変更)
        private DateTime m_SendDate = DateTime.MinValue;

        // 画面に表示中の個人安否情報のリスト
        private List<DbAccess.PersonInfo> m_activePersonInfoList = new List<DbAccess.PersonInfo>();

        // 送信中の個人安否情報のリスト
        private List<DbAccess.PersonInfo> m_sendPersonInfoList = new List<DbAccess.PersonInfo>();

        // RTNリクエスト要求中のID
        private long[] m_Rtn_req_id = null;

        // リスト描画スレッド
        private System.Threading.Thread m_ListViewThread = null;

        // 全避難所情報リスト
        private List<DbAccess.TerminalInfo> m_shelterInfoList = new List<DbAccess.TerminalInfo>();

        // 最後に送信した避難所名データ
        public DbAccess.TerminalInfo m_lastSendShelterName = new DbAccess.TerminalInfo();

        // 時刻同期フラグ
        public bool m_isTimeSyncOK = false;

        // 端末情報取得フラグ
        public bool m_isTerminalDeviceInfoOK = false;

        // 送達確認チェック用
        private int m_respCount = 0;

        // 時刻同期再送要求     //2020.03.18 Add
        public bool m_timeSyncRetry = false;

        /// <summary>
        /// 最後に送信した避難所名データを取得する
        /// </summary>
        /// <returns></returns>
        public DbAccess.TerminalInfo GetLastSendShelterNameInfo()
        {
            return m_lastSendShelterName;
        }

        /// <summary>
        /// DBに登録済みの全避難所のリストを取得する
        /// </summary>
        /// <returns></returns>
        public List<DbAccess.TerminalInfo> GetAllTerminalInfoList()
        {
            return m_shelterInfoList;
        }

        /// <summary>
        /// 避難所管理画面初期化処理
        /// </summary>
        public FormShelterInfo()
        {
            InitializeComponent();

            //タブのサイズを固定する
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.ItemSize = new Size(150, 29);

            //TabControlをオーナードローする
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            //DrawItemイベントハンドラを追加
            tabControl1.DrawItem += new DrawItemEventHandler(tabControl1_DrawItem);
            
            // 避難所情報TEXTクリア
            textShelterInfo.Text = "";
            setToolStripStatusLabel(LABEL.ALL, "");
            lblUpdateDate.Text = "";

            // メニュー項目、ボタンの活性/非活性制御
            ShelterStatusView();

            Control.CheckForIllegalCrossThreadCalls = false;

            Program.EnableDoubleBuffering(listPersonal);
            Program.EnableDoubleBuffering(listTotalization);

            // 開設済みの避難所データを取得、メンバーリストに追加する
            // DBから避難所一覧データを取得
            DbAccess.TerminalInfo[] AllTernimalInfo = new DbAccess.TerminalInfo[0];
            Program.m_objDbAccess.GetTerminalInfoAll(ref AllTernimalInfo);

            // 任意の避難者数を入力テキストボックスで右クリックメニューを表示させないようにする
            textPersonCnt.ContextMenu = new ContextMenu();

            // 自動送信スレッド初期化
            Program.m_AutoThreadSend.Initialize(this);

            // 開設状態となっている避難所をリストに追加する
            //UpdateShelterInfoList();

        }

        /// <summary>
        /// 避難所管理画面表示処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormShelterInfo_Load(object sender, EventArgs e)
        {
            // SSL自己証明書対応
            ServicePointManager.ServerCertificateValidationCallback = (senderObj, certificate, chain, sslPolicyErrors) =>
            {
                return true;
            };

            // インジケータ初期化
            SetIndicator(Indicator.Connection, IndStatus.Disconnect);
            SetIndicator(Indicator.Synchronize, IndStatus.Disconnect);
            SetIndicator(Indicator.TermStatus, IndStatus.Disconnect);
            SetIndicator(Indicator.Message, IndStatus.Disconnect);
            m_TermStatusInd = IndStatus.Disconnect;
            Program.m_objDbAccess.EventReadFlg += new DbAccessStep2.EventReadFlgDelegate(CheckNotRead);
            //Program.m_objDbAccess.CheckReadFlgMessage();

            //  RTN用
            Program.m_RtnThreadSend.EventRtnResp += new RtnThread.EventRtnDelegate(RtnThread_Event);
            Program.m_RtnThreadSend.EventSendStart += new RtnThread.EventRtnDelegate(RtnThread_SendStart);
            Program.m_RtnThreadSend.Start();

            //  FWD用
            Program.m_FwdRecv.EventFwdResp += new FwdReceiver.EventFwdDelegate(FwdThread_Event);

            //  監視ステータス用
            Program.m_EquStat.EventEquResp += new TermInfo.EventEquDelegate(EquThread_Event);
            Program.m_EquStat.startTimer(EQU_STAT_INTERVAL); // 2000 = 2秒ごと

            // L1S用
            Program.m_L1sRecv.EventL1sResp += new L1sReceiver.EventL1sDelegate(L1sThread_Event);

            //  時刻同期要求用
            Program.m_RtnThreadSend.EventTimeSync += new RtnThread.EventTimeSyncDelegate(RtnThread_Event);
            //  災害危機通報
            Program.m_RtnThreadSend.EventDisasterReport += new RtnThread.EventDisasterReportDelegate(RtnThread_Event);

            // 端末情報ダイアログ定義
            m_frmTermStatus = new FormTermStatus();

            // 送達確認用
            m_SlotSubSlot = SlotSubSlot.GetInstance();
            m_SlotSubSlot.EventTimeOut += RtnTimeOut;
            m_SlotSubSlot.Start();

            // SubGhz状態取得用
            if (Program.m_ConnectMethod == 2)
            {
                Program.m_SubGHz.MessageEvent += new SubGHz.EventMessageDelegate(SubGhz_Event);
            }

            bool bExist = false;
            string sId = "";
            // DBよりデータを取得(一番上のデータを取得)
            Program.m_objDbAccess.GetTerminalInfo(sId, out bExist, ref m_ActiveTarminal_info);

            if (bExist == true)
            {
                m_ShelterStat = m_ActiveTarminal_info.open_flag;
                if (m_ShelterStat != 0)
                {
                    txtShelterID.Text = m_ActiveTarminal_info.gid;
                    txtShelterName.Text = m_ActiveTarminal_info.name;
                    txtShelterPos.Text = m_ActiveTarminal_info.lat + "," + m_ActiveTarminal_info.lon;
                    Program.m_objActiveTermial = m_ActiveTarminal_info;
                }
                Program.m_objActiveTermial = m_ActiveTarminal_info;
            }
            else
            {
            }

            if (Program.m_ConnectMethod == 2)
            {
                // サブギガ通信設定の場合は送受信テスト有効化
                ConnectTestMenuItem.Enabled = true;
            }
            else
            {
                // 送受信テスト無効化
                ConnectTestMenuItem.Enabled = false;
            }

            ShelterStatusView();
            UpdateDisplay();

            // 個人安否情報 表更新
            //UpdateListData(true);

            // メッセージ受信時の効果音イベント登録
            Program.m_objDbAccess.EventDbAdd += new DbAccessStep2.EventDbAccessDelegate(RecvMessageSound);

            // 避難所状況設定初期化
            cbTextOnly.Checked = true;
            lblTextInfo.Text = "避難所状況  (最大22文字)";
            textShelterInfo.MaxLength = 22;

            // TCP/IP通信開始
            TcpPersonalThread tcpParsonalThread = new TcpPersonalThread();
            tcpParsonalThread.startTcpPersonalThread(this, true);

            // 時刻同期要求送信スレッド機能開始
            // →一定周期で時刻同期要求をQ-QNPI端末に送信する
            sendTimeSyncThread();

            // 通信端末の情報がないときにDBから通信端末情報を取得する
            if ((Program.m_EquStat.mQCID == "") || (Program.m_EquStat.mQCID == null))
            {
                Program.m_objDbAccess.GetRecentlyQANPIDeviceInfo(ref Program.m_EquStat.mQCID);
            }

            // 避難所詳細情報ログ保持リスト初期化
            m_TotalSendLog = new List<DbAccess.TotalSendLog>();

            m_initialize = false;

            if (m_shelterInfoList.Count > 0)
            {
                // DBのselectフラグがONになっている避難所に切替え
                int indexNum = -1;
                bool findSelectFlg = false;
                DbAccess.TerminalInfo[] AllTernimalInfo = new DbAccess.TerminalInfo[0];
                Program.m_objDbAccess.GetTerminalInfoAll(ref AllTernimalInfo);
                foreach (var item in AllTernimalInfo)
                {
                    indexNum++;
                    if (item.select_flag)
                    {
                        findSelectFlg = true;
                        break;
                    }
                }

                if (findSelectFlg)
                {
                    SetActiveShelterInfo(indexNum);
                }
                else
                {
                    SetActiveShelterInfo(0);
                }
            }

            updateActiveShelterInfo();

            if ((m_ActiveTarminal_info.sid != "") && (m_ActiveTarminal_info.sid != null))
            {
                // DBに保存されている救助支援情報を読み込み
                Program.m_objDbAccess.CheckReadFlgMessage();

                // 救助避難情報タブ値取得＆書き込み
                loadList(2);
            }

        }

        //----------------------------------------------------------------------------------------------
        // 個人安否情報
        //----------------------------------------------------------------------------------------------
        // ListViewの列ヘッダ描画
        private void listPersonal_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // 描画はOSに任せる
            e.DrawDefault = true;
        }

        // ListViewのセル描画
        private void listPersonal_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            StringFormat stringFormat = new StringFormat();
            Rectangle objStrRect = e.Bounds;

            Brush br, brTxt;
            br = SystemBrushes.Window;
            brTxt = SystemBrushes.WindowText;
            // 奇数行は背景色を変更して描画
            if ((e.Item.Index % 2) != 0)
            {
                //br = Brushes.LightBlue;
                //br = Brushes.AliceBlue;
                br = Brushes.Azure;

            }

            switch (e.ColumnIndex)
            {
                default:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    objStrRect = e.Bounds;
                    //brTxt = Brushes.Blue;
                    break;
                case 3:    // 電話番号 左詰
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    objStrRect = e.Bounds;
                    brTxt = Brushes.Blue;
                    break;

                case 2:    //氏名 左詰
                case 8:     //住所 左詰
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    objStrRect = e.Bounds;
                    break;

                case 4:    //年齢 右詰
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    objStrRect = e.Bounds;
                    break;

                case 7:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                    // 「電話番号」「公表」「怪我」「介護」「障がい」「妊産婦」「避難所内外」のみの文言表示色を青色に変更
                    //br = Brushes.Gainsboro;
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    objStrRect = e.Bounds;
                    brTxt = Brushes.Blue;
                    break;

            }

            Font fontCell;
            if (e.ColumnIndex == 1)
            {
                if (e.SubItem.Text == "未送信")
                {
                    fontCell = new Font(e.Item.Font.Name, e.Item.Font.Size, FontStyle.Bold);
                }
                else if (e.SubItem.Text == "送信中")
                {
                    brTxt = Brushes.Red;
                    fontCell = new Font(e.Item.Font.Name, e.Item.Font.Size);
                }
                else
                {
                    fontCell = new Font(e.Item.Font.Name, e.Item.Font.Size);
                }

            }
            else
            {
                fontCell = new Font(e.Item.Font.Name, e.Item.Font.Size);
            }

            // 選択されているアイテムの描画
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(
                                SystemBrushes.Highlight,
                                e.Bounds);
                e.Graphics.DrawString(
                                e.SubItem.Text,
                    //e.Item.Font,
                                fontCell,
                                SystemBrushes.HighlightText,
                                objStrRect,
                                stringFormat);
                return;
            }


            e.Graphics.FillRectangle(
                            br,
                            e.Bounds);

            e.Graphics.DrawString(
                            e.SubItem.Text,
                //e.Item.Font,
                            fontCell,
                            brTxt,
                            e.Bounds,
                            stringFormat);
            return;
        }

        private void listPersonal_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        // 20180720 - 生年月日から年齢を計算する
        private int getAge(string strBirthDay)
        {
            int result = 0;
            // 渡された文字列はDateTimeに変換可能か?
            DateTime today = DateTime.Now;
            DateTime dt;
            // 変換できる場合、年齢の計算を行う
            if (DateTime.TryParse(strBirthDay, out dt))
            {
                result = today.Year - dt.Year;

                // 誕生日をまだ迎えてない場合、-1
                if (today.Month < dt.Month
                    || (today.Month == dt.Month && today.Day < dt.Day))
                {
                    result--;
                }
            }
            // 変換できない値を渡された場合とりあえず0を返す
            else
            {
                result = 0;
            }

            return result;
        }


        // 送信履歴ボタン
        private void btnSendListPersonal_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "person", "送信履歴クリック");

            if (listPersonal.SelectedItems.Count > 0)
            {
                int nSelidx = listPersonal.SelectedItems[0].Index;
                FormPersonalSendHistory frmHistory = new FormPersonalSendHistory();

                // リストの選択された項目をm_infoListから検索
                for (int ic = 0; ic <= m_activePersonInfoList.Count - 1; ic++)
                {
                    if ((m_activePersonInfoList[ic].id == listPersonal.SelectedItems[0].SubItems[3].Text)
                        && (m_activePersonInfoList[ic].name == listPersonal.SelectedItems[0].SubItems[2].Text))
                    {
                        nSelidx = ic;
                        break;
                    }
                }
                //2016/04/27 個人送信ログは、選択行のログのみ表示
                frmHistory.init(m_activePersonInfoList[nSelidx]);

                frmHistory.ShowDialog();

            }
        }

        // 登録履歴ボタン
        private void btnEntryListPersonal_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "person", "登録履歴クリック");

            if (listPersonal.SelectedItems.Count > 0)
            {
                int nSelidx = listPersonal.SelectedItems[0].Index;
                FormPersonalEntryHistory frmHistory = new FormPersonalEntryHistory();

                // リストの選択された項目をm_infoListから検索
                for (int ic = 0; ic <= m_activePersonInfoList.Count - 1; ic++)
                {
                    if ((m_activePersonInfoList[ic].id == listPersonal.SelectedItems[0].SubItems[3].Text)
                        && (m_activePersonInfoList[ic].name == listPersonal.SelectedItems[0].SubItems[2].Text))
                    {
                        nSelidx = ic;
                        break;
                    }
                }

                frmHistory.init(m_activePersonInfoList[nSelidx]);

                frmHistory.ShowDialog();
            }
        }

        // 編集ボタン
        private void btnEditPersonal_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "person", "編集クリック");

            if (listPersonal.SelectedItems.Count > 0)
            {
                int nSelidx = listPersonal.SelectedItems[0].Index;
                int nItemCnt = listPersonal.Items.Count;

                DbAccess.PersonInfo beforeInfo = new DbAccess.PersonInfo(); ;
                beforeInfo.init();

                // リストの選択された項目をm_infoListから検索
                for (int ic = 0; ic <= m_activePersonInfoList.Count - 1; ic++)
                {
                    if ((m_activePersonInfoList[ic].id == listPersonal.SelectedItems[0].SubItems[3].Text)
                        && (m_activePersonInfoList[ic].name == listPersonal.SelectedItems[0].SubItems[2].Text))
                    {
                        nSelidx = ic;
                        beforeInfo = m_activePersonInfoList[ic];
                        break;
                    }
                }

                FormEditPersonal frmEdit = new FormEditPersonal(m_activePersonInfoList[nSelidx]);

                frmEdit.init(m_activePersonInfoList[nSelidx], 1);
                frmEdit.Text = "避難所情報システム  － 個人情報編集";
                frmEdit.ShowDialog();

                // 2016/04/16 DB更新処理をShelterInfoSystemに移動
                if (frmEdit.GetDlgResult())
                {
                    DbAccess.PersonInfo info = frmEdit.GetDlgPersonInfo();

                    lock (m_PersonSendList)
                    {
                        // 変更前のデータを削除
                        Program.m_objDbAccess.DeletePersonInfo(beforeInfo.id, beforeInfo.name, beforeInfo.sid);

                        // 変更後のデータを登録
                        Program.m_objDbAccess.UpsertPersonInfo(info);

                        int nListIdx;
                        for (nListIdx = 0; nListIdx < m_PersonSendList.Count; nListIdx++)
                        {
                            if (m_PersonSendList[nListIdx].info == m_activePersonInfoList[nSelidx])
                            {
                                SendData data = new SendData();
                                data.bEdit = true;
                                data.bDelete = m_PersonSendList[nListIdx].bDelete;
                                data.bSendWait = m_PersonSendList[nListIdx].bSendWait;
                                data.info = info;
                                m_PersonSendList[nListIdx] = data;
                                break;
                            }
                        }
                    }

                    // 履歴追加
                    DbAccess.PersonLog logInfo = new DbAccess.PersonLog();
                    logInfo.init();

                    // 履歴情報を設定
                    logInfo.Set(info);

                    Program.m_objDbAccess.InsertPersonLog(logInfo);

                    Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "person", "編集画面 登録操作");
                }

                // 表更新
                UpdateListData(true);
            }

        }

        // 新規登録ボタン
        private void btnNewPersonal_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "person", "新規登録クリック");
            //System.Diagnostics.Process.Start(Program.m_objQzssConfig.LocalURL);

            // 新規登録ダイアログを表示
            DbAccess.PersonInfo personInfo = new DbAccess.PersonInfo();
            personInfo.init();
            personInfo.sid = m_ActiveTarminal_info.sid;
            FormEditPersonal frmEdit = new FormEditPersonal(personInfo);

            frmEdit.init(personInfo, 0);
            frmEdit.Text = "避難所情報システム  － 個人情報登録";
            frmEdit.ShowDialog();

            // 登録ボタン押下
            if (frmEdit.GetDlgResult())
            {
                DbAccess.PersonInfo info = frmEdit.GetDlgPersonInfo();

                lock (m_PersonSendList)
                {
                    // 必須入力チェック
                    /*  性別/公表可否は未選択の場合"0"が設定されるため判別不可 */
                    String strMessage = "";
                    if (info.id == "")
                        strMessage += "電話番号を入力してください。\n";
                    if (info.name == "　")
                        strMessage += "名前を入力してください。\n";
                    if (info.txt02 == "")
                        strMessage += "生年月日を入力してください。\n";
                    if (info.sel01 == "")
                        strMessage += "性別を選択してください。\n";
                    if (info.sel03 == "")
                        strMessage += "公表の可否を選択してください。\n";

                    // 未入力の必須入力項目が存在する
                    if (strMessage != "")
                    {
                        MessageBox.Show(strMessage, "避難所システム", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "person", "登録画面 必須入力未入力で登録失敗");
                        return;
                    }

                    // 重複チェック
                    bool bExist;
                    Program.m_objDbAccess.ExistPersonInfo(info, out bExist);
                    List<KeyValuePair<string, string>> chkInfoPair = new List<KeyValuePair<string, string>>();
                    foreach (var tmpList in m_activePersonInfoList)
                    {
                        KeyValuePair<string, string> tmpPair = new KeyValuePair<string, string>(tmpList.id, tmpList.name);
                        chkInfoPair.Add(tmpPair);
                    }
                    KeyValuePair<string, string> thisPair = new KeyValuePair<string, string>(info.id, info.name);

                    // DBに存在するかつリストに存在する場合、登録エラー
                    if ((bExist) && (chkInfoPair.Contains(thisPair)))
                    {
                        MessageBox.Show("すでに登録されている氏名と電話番号は登録できません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "person", "登録画面 同ID同名で登録失敗");
                        return;
                    }
                    // DBに存在するがリストに存在しない場合、DBを更新しリストに再表示する
                    else if (bExist)
                    {
                        // 更新処理
                        Program.m_objDbAccess.UpdatePersonInfo(info);
                        SendData sendData = new SendData();
                        sendData.bEdit = true;
                        sendData.bDelete = true;
                        sendData.bSendWait = true;
                        sendData.info = info;
                    }
                    // DBにもリストにも存在しない場合、新規登録処理
                    else
                    {
                        // 新規登録
                        Program.m_objDbAccess.InsertPersonInfo(info);
                        SendData sendData = new SendData();
                        sendData.bEdit = false;
                        sendData.bDelete = false;
                        sendData.bSendWait = true;
                        sendData.info = info;
                    }
                }

                // 履歴追加
                DbAccess.PersonLog logInfo = new DbAccess.PersonLog();
                logInfo.init();

                // 履歴情報を設定
                logInfo.Set(info);

                Program.m_objDbAccess.InsertPersonLog(logInfo);
                Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "person", "登録画面 登録操作");
            }
            // 表更新
            UpdateListData(true);
        }

        // 削除ボタン
        private void btnDeletePersonal_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "person", "削除クリック");

            // 個人安否情報リストの項目を選択していた場合、削除処理を実行
            if (listPersonal.SelectedItems.Count > 0)
            {
                // 削除確認ダイアログ表示
                int nSelidx = listPersonal.SelectedItems[0].Index;
                string strname = listPersonal.Items[nSelidx].SubItems[2].Text;
                string strtel = listPersonal.Items[nSelidx].SubItems[3].Text;
                DialogResult result = MessageBox.Show("氏名：" + strname + "\r\n" + "電話番号：" + strtel + "\r\n削除しますか？",
                                                            "避難所情報システム",
                                                            MessageBoxButtons.OKCancel,
                                                            MessageBoxIcon.Question);

                // OKボタン押下時に削除を実施
                if (result == DialogResult.OK)
                {
                    lock (m_PersonSendList)
                    {
                        // レコードを削除
                        Program.m_objDbAccess.DeletePersonInfo(strtel, strname, m_ActiveTarminal_info.sid);

                        // リストの選択された項目をm_infoListから検索
                        for (int ic = 0; ic <= m_activePersonInfoList.Count - 1; ic++)
                        {
                            if ((m_activePersonInfoList[ic].id == listPersonal.SelectedItems[0].SubItems[3].Text)
                                && (m_activePersonInfoList[ic].name == listPersonal.SelectedItems[0].SubItems[2].Text))
                            {
                                nSelidx = ic;
                                break;
                            }
                        }

                        int nListIdx;
                        for (nListIdx = 0; nListIdx < m_PersonSendList.Count; nListIdx++)
                        {
                            if (m_PersonSendList[nListIdx].info == m_activePersonInfoList[nSelidx])
                            {
                                SendData data = new SendData();
                                data.bEdit = m_PersonSendList[nListIdx].bEdit;
                                data.bDelete = true;
                                data.bSendWait = m_PersonSendList[nListIdx].bSendWait;
                                data.info = m_PersonSendList[nListIdx].info;
                                m_PersonSendList[nListIdx] = data;
                                break;
                            }
                        }
                    }

                    UpdateListData(true);

                    Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "person", "削除OK操作");
                }
            }
        }

        // 送信ボタン
        private void btnSendPersonal_Click(object sender, EventArgs e)
        {
            if (Program.m_SendFlag != Program.NOT_SENDING)
            {
                MessageBox.Show("他の機能でメッセージ送信中の為、個人安否情報の送信はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Program.m_EquStat.isConnected() == false)
            {
                MessageBox.Show("Q-ANPIターミナル未接続の為、個人安否情報の送信はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "btnSendPersonal_Click", "送信クリック");

            DialogResult result = DialogResult.None;
            result = MessageBox.Show("個人安否情報（電話番号、公開可否、ケガの有無、介護の要否、" + System.Environment.NewLine +
                                     "高齢者/乳児/妊産婦、避難所内外）を送信しますか？",
                                     "避難所情報システム",
                                     MessageBoxButtons.OKCancel,
                                     MessageBoxIcon.Question);
            if ((result == DialogResult.OK))
            {
                if (Program.m_SendFlag != Program.NOT_SENDING)
                {
                    MessageBox.Show("他の機能でメッセージ送信中の為、個人安否情報の送信はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 個人安否情報送信時のみボタン押下直後に非活性にする
                // （個人安否情報が多いと送信開始の非活性制御までにもう一度ボタンを押せてしまうため）
                Program.m_SendFlag = Program.SENDING_PERSON_INFO;
                ShelterStatusView();
                Program.m_SendFlag = Program.NOT_SENDING;


                // 共通送信数の初期化
                initCommonSendingBuff();
                initSending(true);

                // 実行コマンドをセット
                m_NowCommand = SendMessageCommand.SEND_PERSON;

                // 選択中の避難所で既に避難所名送信のメッセージを送信済みの場合、避難所名情報は送らない
                if (m_lastSendShelterName.sid != m_ActiveTarminal_info.sid)
                {
                    // 避難所名情報を送信する(Type2 ST1)
                    // (直前に送信された避難所に個人安否情報が紐づく、という地上サーバ側の仕様に対応させる)
                    SendShelterNameInfo(m_ActiveTarminal_info);

                    // 避難所名⇒個人安否情報の順番が前後しないようにウェイトを挿入
                    Thread.Sleep(100);
                }

                // 個人安否情報を送信する(Type1)
                sendPersonal(m_ActiveTarminal_info, false);
            }
        }


        /// <summary>
        /// 個人安否情報送信(Type1)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="isAuto"></param>
        public void sendPersonal(DbAccess.TerminalInfo info, bool isAuto)
        {
            if (Program.m_EquStat.isConnected() == false)
            {
                MessageBox.Show("Q-ANPIターミナル未接続の為、個人安否情報の送信はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 避難所のに登録されている個人情報を取得
            List<DbAccess.PersonInfo> personInfo = new List<DbAccess.PersonInfo>();
            Program.m_objDbAccess.GetPersonInfoList(info.sid, ref personInfo);

            // HTTP 送信データ
            int personCount = 0;
            SetPersonDataCount(info, out personCount);

            // 送信待機前に先に最大送信数だけ計算しておく
            m_Type1CountMax = personCount;
            m_commonCountMax += m_Type1CountMax;

            // 送信リクエスト処理は別スレッド
            Task task = Task.Factory.StartNew(() =>
            {
                int timeoutCount = 0;
                bool okSend = true;
                int myWatingNo = m_nowWaitNo;
                m_nowWaitNo += 1;
                while (true)
                {
                    // 現在送信中ではない、かつ自分の送信順が来たら送信開始
                    if ((myWatingNo == (m_nowSendFinNo)) && (!m_nowSending))
                    {
                        break;
                    }
                    // 送信中フラグがOFFになるまで待機
                    System.Threading.Thread.Sleep(100);
                    timeoutCount++;

                    // 送信待機中に送信状態がリセット(解除)されたら、他の送信中に送信失敗したとみなし、送信待機を解除
                    if (Program.m_SendFlag == Program.NOT_SENDING)
                    {
                        okSend = false;
                        break;
                    }

                    // 順番待ち変数がint型の最大値-10を超えていたら送信せず、避難所アプリを再起動するメッセージを出す
                    if (myWatingNo > (int.MaxValue - 10))
                    {
                        Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "sendPersonal", "送信待ち受け数オーバー");
                        MessageBox.Show("メモリが不足しています。避難所アプリを再起動してください。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        okSend = false;
                        break;
                    }
                }

                if (okSend)
                {
                    // HTTP 送信データ
                    SetPersonData(info, personInfo);

                    Program.m_SendFlag = Program.SENDING_PERSON_INFO;         // 0:送信中でない  1:個人安否送信  2:避難所情報送信　3:開設/閉鎖/端末情報送信
                    ShelterStatusView();

                    if (m_nSndTotal > 0)
                    //if (listPersonal.Items.Count > 0)
                    {

                        Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "FormShelterInfo", "person", "送信処理開始");
                        Program.m_SendFlag = Program.SENDING_PERSON_INFO;         // 0:送信中でない  1:個人安否送信  2:避難所情報送信　3:開設/閉鎖/端末情報送信
                        ShelterStatusView();

                        // 送信時刻を保存
                        // DB上の送信時刻は、この送信ボタンを押した時刻で更新する
                        m_SendDate = DateTime.Now;

                        // 避難所情報の送信ボタン使用不可
                        btnSendTotalization.Enabled = false;

                        setToolStripStatusLabel(LABEL.APP, "個人安否情報送信開始  ");

                        // 送達確認用
                        m_CheckType1 = new int[m_nSndTotal]; // 20170525
                        m_StateType1 = STATE.SENDING;
                        m_Type1SendStartCount = 0;
                        m_Type1SendingCount = 0;
                        m_Type1WaitingCount = 0;

                        m_commonSendingCount += m_Type1SendingCount;
                        m_commonSendStartCount += m_Type1SendStartCount;
                        m_commonWaitingCount += m_Type1WaitingCount;

                        string lblmsg = getLabelRtnCount(m_commonSendStartCount, m_commonCountMax);
                        setToolStripStatusLabel(LABEL.RTN_SND, "req:" + lblmsg);
                        lblmsg = getLabelRtnCount(m_commonSendingCount, m_commonCountMax);
                        setToolStripStatusLabel(LABEL.RTN_RCV, "send:" + lblmsg);
                        lblmsg = getLabelRtnCount(m_commonWaitingCount, m_commonCountMax);
                        setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);

                        PostSend();

                        // 一時配列に格納(ListViewのデータを直接参照すると動作が遅いので)
                        List<string[]> personItem = new List<string[]>();
                        for (int ic = 0; ic < listPersonal.Items.Count; ic++)
                        {
                            personItem.Add(new string[14]);
                            for (int kc = 0; kc < 14; kc++)
                            {
                                personItem[ic][kc] = listPersonal.Items[ic].SubItems[kc].Text;
                            }
                        }

                        // 一致するデータを検索
                        string[] strItems = new string[14];
                        for (int ic = 0; ic < personItem.Count; ic++)
                        {
                            for (int nIdx = 0; nIdx < personInfo.Count; nIdx++)
                            {
                                if ((personInfo[nIdx].id == personItem[ic][3]) &&
                                    (personInfo[nIdx].name == personItem[ic][2]))
                                {
                                    strItems = ConvPersonInfoToStringList(personInfo[nIdx]);
                                    listPersonal.Items[ic].SubItems[1].Text = strItems[1];
                                    break;
                                }
                            }
                        }
                    }
                }
            });
        }

        // 個人情報送信データ生成
        private void SetPersonData(DbAccess.TerminalInfo info, List<DbAccess.PersonInfo> sendPersonInfo)
        {
            m_Rtn_req_id = new long[sendPersonInfo.Count];
            m_sendPersonInfoList = sendPersonInfo;

            m_sendDataArr.Clear();
            // 2016/04/18 送信中対応のための排他処理追加
            lock (m_PersonSendList)
            {
                m_PersonSendList.Clear();

                m_nSndCnt = 0;
                m_nSndTotal = 0;

                int nIdx;
                for (nIdx = 0; nIdx < sendPersonInfo.Count; nIdx++)
                {
                    DateTime sendDay = DateTime.MinValue;
                    DateTime.TryParse(sendPersonInfo[nIdx].send_datetime, out sendDay);

                    DateTime upDay = DateTime.MinValue;
                    DateTime.TryParse(sendPersonInfo[nIdx].update_datetime, out upDay);

                    if (sendDay < upDay)
                    {
                        // 2016/04/18 送信中対応
                        SendData data = new SendData();
                        data.init();
                        data.info = sendPersonInfo[nIdx];
                        m_PersonSendList.Add(data);

                        string sSendData = MsgConv.ConvPersonInfoToSendString(sendPersonInfo[nIdx]);
                        Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "FormShelterInfo", "SetPersonData", sSendData);
                        m_sendDataArr.Add(sSendData);

                        // 確認のためのIDを保持
                        if (data.info.id != null)
                        {
                            m_Rtn_req_id[m_nSndTotal] = long.Parse(data.info.id);
                        }
                        else
                        {
                            Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "SetPersonData ID err ");

                        }
                        // dbg
                        if (m_nSndTotal == 1)
                        {
                            Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "FormShelterInfo", "SetPersonData 1 ", data.info.id);
                        }

                        m_nSndTotal++;
                    }
                }
            }
        }

        // 個人情報送信データ生成
        private void SetPersonDataCount(DbAccess.TerminalInfo info, out int personCount)
        {
            personCount = 0;

            // 2016/04/18 送信中対応のための排他処理追加
            lock (m_PersonSendList)
            {
                // 避難所のに登録されている個人情報を取得
                List<DbAccess.PersonInfo> personInfo = new List<DbAccess.PersonInfo>();
                Program.m_objDbAccess.GetPersonInfoList(info.sid, ref personInfo);

                int nIdx;
                for (nIdx = 0; nIdx < personInfo.Count; nIdx++)
                {
                    DateTime sendDay = DateTime.MinValue;
                    DateTime.TryParse(personInfo[nIdx].send_datetime, out sendDay);

                    DateTime upDay = DateTime.MinValue;
                    DateTime.TryParse(personInfo[nIdx].update_datetime, out upDay);

                    if (sendDay < upDay)
                    {
                        personCount++;
                    }
                }
            }
        }


        //----------------------------------------------------------------------------------------------
        // 避難所情報
        //----------------------------------------------------------------------------------------------
        // ListViewの列ヘッダ描画
        private void listTotalization_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // 描画はOSに任せる
            e.DrawDefault = true;
        }

        // ListViewのセル描画
        private void listTotalization_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            StringFormat stringFormat = new StringFormat();
            Rectangle objStrRect = e.Bounds;

            switch (e.ColumnIndex)
            {
                default:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    objStrRect = e.Bounds;
                    break;
            }

            // 選択されているアイテムの描画
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(
                                SystemBrushes.Highlight,
                                e.Bounds);
                e.Graphics.DrawString(
                                e.SubItem.Text,
                                e.Item.Font,
                                SystemBrushes.HighlightText,
                                objStrRect,
                                stringFormat);
                return;
            }

            // 奇数行は背景色を変更して描画
            if ((e.Item.Index % 2) != 0)
            {
                SolidBrush brushes = new SolidBrush(Color.AliceBlue);

                e.Graphics.FillRectangle(
                                brushes,
                                e.Bounds);

                e.Graphics.DrawString(
                                e.SubItem.Text,
                                e.Item.Font,
                                SystemBrushes.WindowText,
                                e.Bounds,
                                stringFormat);
                return;
            }
            else
            {
                e.Graphics.FillRectangle(
                                SystemBrushes.Window,
                                e.Bounds);
                e.Graphics.DrawString(
                                e.SubItem.Text,
                                e.Item.Font,
                                SystemBrushes.WindowText,
                                objStrRect,
                                stringFormat);
            }
            return;
        }

        // 送信履歴
        private void btnSendListTotalization_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "total", "送信履歴クリック");

            FormTotalizationSendHistory frmHistory = new FormTotalizationSendHistory();

            frmHistory.ShowDialog();
        }

        // 送信ボタン
        private void btnSendTotalization_Click(object sender, EventArgs e)
        {
            if (Program.m_SendFlag != Program.NOT_SENDING)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "SendShelterNameInfo", "送信中です");
                MessageBox.Show("他の機能でメッセージ送信中の為、避難所詳細情報の送信はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            if (Program.m_EquStat.isConnected() == false)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "SendShelterNameInfo", "未接続です");
                MessageBox.Show("Q-ANPIターミナル未接続の為、避難所詳細情報の送信はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DialogResult result = DialogResult.None;
            result = MessageBox.Show("避難所情報を送信しますか？",
                                                    "避難所情報システム",
                                                    MessageBoxButtons.OKCancel,
                                                    MessageBoxIcon.Question);
            if ((result == DialogResult.OK))
            {
                if (Program.m_EquStat.isConnected() == false)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "SendShelterNameInfo", "未接続です");
                    MessageBox.Show("Q-ANPIターミナル未接続の為、避難所詳細情報の送信はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Program.m_SendFlag != Program.NOT_SENDING)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "SendShelterNameInfo", "送信中です");
                    MessageBox.Show("他の機能でメッセージ送信中の為、避難所詳細情報の送信はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 共通送信数の初期化
                initCommonSendingBuff();
                initSending(true);

                // 実行コマンドをセット
                m_NowCommand = SendMessageCommand.SEND_SHELTER_INFO;

                // 選択中の避難所で既に避難所名送信のメッセージを送信済みの場合、避難所名情報は送らない
                if (m_lastSendShelterName.sid != m_ActiveTarminal_info.sid)
                {
                    // 避難所名情報を送信する(Type2 ST1)
                    // (直前に送信された避難所に個人安否情報が紐づく、という地上サーバ側の仕様に対応させる)
                    SendShelterNameInfo(m_ActiveTarminal_info);

                    // 避難所名⇒避難所詳細情報の順番が前後しないようにウェイトを挿入
                    Thread.Sleep(100);
                }

                // 避難所詳細情報を送信する(Type2 ST1)
                SendShelter(m_ActiveTarminal_info, false);
            }
        }

        /// <summary>
        /// 避難所詳細情報送信(Type2,ST1)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="isAuto"></param>
        public void SendShelter(DbAccess.TerminalInfo info, bool isAuto = false, int autoMaxCount = 0)
        {
            // 避難所の全個人安否情報を取得
            List<DbAccess.PersonInfo> sendPersonList = new List<DbAccess.PersonInfo>();
            Program.m_objDbAccess.GetPersonInfoList(info.sid, ref sendPersonList);

            // 避難所集計情報の取得
            string[,] sendTotalSendList =
                {
                    {"1", "", "避難者数", "55"},
                    {"2", "", "在宅者数", "0"},
                    {"3", "", "男性", "30"},
                    {"4", "", "女性", "25"},
                    {"5", "", "負傷者数", "0"},
                    {"6", "", "要介護者数", "5"},
                    {"7", "", "障がい者数", "1"},
                    {"8", "", "妊産婦者数", "1"},
                    {"9", "", "高齢者数", "0"},
                    {"10", "", "乳児数", "3"},
                    {"11", "", "避難所内", "3"},
                    {"12", "", "避難所外", "3"},
                    {"13", "", "避難所状況", ""},
                };

            TotalizationPersonInfo(info, sendPersonList, ref sendTotalSendList);

            if (sendTotalSendList.Length > 0)
            {
                // 詳細情報送信ログデータを生成
                DbAccess.TotalSendLog log = new DbAccess.TotalSendLog();
                SetTotalSendLog(sendTotalSendList, info, out log);

                // 先に送信数をカウントしておく
                if (isAuto)
                {
                    m_commonCountMax = autoMaxCount;
                }
                else
                {
                    m_commonCountMax += m_Type2CountMax_0;
                }

                int myWatingNo = m_nowWaitNo;
                m_nowWaitNo++;

                // 送信リクエスト処理は別スレッド
                Task task = Task.Factory.StartNew(() =>
                {
                    int timeoutCount = 0;
                    bool okSend = true;
                    //int myWatingNo = m_nowWaitNo;
                    //m_nowWaitNo++;

                    Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "SendShelter(TaskID：" + Task.CurrentId + ")", "避難所詳細情報送信(" + info.name + ")", "myWatingNo = " + myWatingNo + ", m_nowWaitNo = " + m_nowWaitNo);

                    while (true)
                    {
                        // 現在送信中ではない、かつ自分の送信順が来たら送信開始
                        if ((myWatingNo == (m_nowSendFinNo)) && (!m_nowSending))
                        {
                            break;
                        }
                        // 送信中フラグがOFFになるまで待機
                        System.Threading.Thread.Sleep(100);
                        timeoutCount++;

                        // 送信待機中に送信状態がリセット(解除)されたら、他の送信中に送信失敗したとみなし、送信待機を解除
                        if (Program.m_SendFlag == Program.NOT_SENDING)
                        {
                            okSend = false;
                            break;
                        }

                        // 順番待ち変数がint型の最大値-10を超えていたら送信せず、避難所アプリを再起動するメッセージを出す
                        if (myWatingNo > (int.MaxValue - 10))
                        {
                            Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "SendShelter", "送信待ち受け数オーバー");
                            MessageBox.Show("メモリが不足しています。避難所アプリを再起動してください。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            okSend = false;
                            break;
                        }
                    }
                    if (okSend)
                    {

                        Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "FormShelterInfo", "total", "避難所情報送信処理開始");

                        Program.m_SendFlag = Program.SENDING_SHELTER_INFO;         // 0:送信中でない  1:個人安否送信  2:避難所情報送信　3:開設/閉鎖/端末情報送信
                        Program.m_mainForm.m_isType2subDetail = true;

                        ShelterStatusView();

                        // データ設定
                        //SetCalcData(sendTotalSendList, info);
                        SetCalcData(log, info);

                        setToolStripStatusLabel(LABEL.APP, "避難所詳細情報送信開始  ");

                        // 送達確認用
                        m_CheckType2_0 = Enumerable.Repeat<int>(TYPE2_SEND_STATE_DEFAULT, m_Type2CountMax_0).ToArray();
                        m_StateType2_0 = STATE.SENDING;
                        m_Type2SendStartCount_0 = 0;
                        m_Type2WaitingCount_0 = 0;


                        // ステータスバーに現在の送信状況を表示
                        string lblmsg = getLabelRtnCount(m_commonSendStartCount, m_commonCountMax);
                        setToolStripStatusLabel(LABEL.RTN_SND, "req:" + lblmsg);
                        lblmsg = getLabelRtnCount(m_commonSendingCount, m_commonCountMax);
                        setToolStripStatusLabel(LABEL.RTN_RCV, "send:" + lblmsg);
                        lblmsg = getLabelRtnCount(m_commonWaitingCount, m_commonCountMax);
                        setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);

                        // 送信中フラグをONにし、送信処理
                        m_nowSending = true;
                        string sndVal2 = (string)m_sendDataArr[0];
                        byte[] binary2 = MsgConv.ConvStringToBytes(sndVal2);
                        Program.m_RtnThreadSend.AddSendQue(binary2);
                    }
                });
            }
        }

        // 送信データ作成
        //private void SetCalcData(string[,] sendTotalList, DbAccess.TerminalInfo info)
        private void SetCalcData(DbAccess.TotalSendLog log, DbAccess.TerminalInfo info)
        {
            m_sendDataArr.Clear();

            m_nSndCnt = 0;
            m_nSndTotal = 0;

            // 避難所毎の避難所状況(テキスト)のみ送信チェック状態取得
            bool textOnty = false;
            if (info.text_flag == "1")
            {
                textOnty = true;
            }
            else
            {
                textOnty = false;
            }
            //string[] sDataList = MsgConv.ConvTotalSendLogToSendString(m_TotalSendLog, textOnty);
            string[] sDataList = MsgConv.ConvTotalSendLogToSendString(log, textOnty);

            int nIdx;
            for (nIdx = 0; nIdx < sDataList.Length; nIdx++)
            {
                m_sendDataArr.Add(sDataList[nIdx]);
                Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "FormShelterInfo", "SetCalcData", sDataList[nIdx]);
                m_nSndTotal++;
            }
        }

        /// <summary>
        /// 避難所情報から詳細情報送信ログを生成する
        /// </summary>
        /// <param name="sendTotalList"></param>
        /// <param name="info"></param>
        private void SetTotalSendLog(string[,] sendTotalList, DbAccess.TerminalInfo info, out DbAccess.TotalSendLog sendTotalSendLog)
        {
            // 送信履歴情報一時保持用変数宣言/初期化
            sendTotalSendLog = new DbAccess.TotalSendLog();
            sendTotalSendLog.init();

            // 送信データ保存
            sendTotalSendLog.sid = info.sid;                  // 避難所ID
            sendTotalSendLog.num01 = sendTotalList[0, 3];     // 入所数
            sendTotalSendLog.num02 = sendTotalList[1, 3];     // 在宅数
            sendTotalSendLog.num03 = sendTotalList[2, 3];     // 男性
            sendTotalSendLog.num04 = sendTotalList[3, 3];     // 女性
            sendTotalSendLog.num05 = sendTotalList[4, 3];     // 軽傷者数(負傷者数は軽傷者としてカウント)
            sendTotalSendLog.num06 = sendTotalList[5, 3];     // 要介護者数
            sendTotalSendLog.num07 = sendTotalList[6, 3];     // 障がい者数
            sendTotalSendLog.num08 = sendTotalList[7, 3];     // 妊産婦数
            sendTotalSendLog.num09 = sendTotalList[8, 3];     // 高齢者数
            sendTotalSendLog.num10 = sendTotalList[9, 3];     // 乳児数
            sendTotalSendLog.num11 = sendTotalList[10, 3];    // 避難所内
            sendTotalSendLog.num12 = sendTotalList[11, 3];    // 避難所外
            sendTotalSendLog.txt01 = sendTotalList[12, 3];    // 避難所状況テキストボックス内テキスト
            sendTotalSendLog.sendresult = "1";                // 送信成功に設定
            sendTotalSendLog.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            // 送信履歴保持リストに保存
            if (!m_TotalSendLog.Contains(sendTotalSendLog))
            {
                m_TotalSendLog.Add(sendTotalSendLog);
            }
        }

        /// <summary>
        /// 詳細情報送信ログをDBに保存する
        /// </summary>
        /// <param name="sendLog"></param>
        /// <param name="result"></param>
        public void SaveTotalSendLog(DbAccess.TotalSendLog sendLog, bool result)
        {
            // 基本的な情報は詳細情報送信の処理(SetCalcData)でセット済み

            // 結果をセット
            if (!result)
            {
                sendLog.sendresult = "0";    // 送信失敗に設定
            }
            else
            {
                sendLog.sendresult = "1";    // 送信成功に設定
            }

            // 避難所IDとログ出力日時をセット
            sendLog.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            // DBに書き込み
            Program.m_objDbAccess.InsertTotalSendLog(sendLog);
        }

        /// <summary>
        /// 避難所登録用送信データ作成
        /// </summary>
        /// <param name="sendDataArr"></param>
        private void SetCalcData_ShelterName(string gid, string smid, string newShelterName)
        {
            string[] sDataList = MsgConv.ConvRegisterNewShelterToSendString(gid, smid, newShelterName);

            int nIdx;
            for (nIdx = 0; nIdx < sDataList.Length; nIdx++)
            {
                m_sendDataArr.Add(sDataList[nIdx]);
                Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "FormShelterInfo", "SetCalcData", sDataList[nIdx]);
                m_nSndTotal++;
            }
        }


        //----------------------------------------------------------------------------------------------
        // 共通部
        //----------------------------------------------------------------------------------------------
        // TAB描画
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {

            //対象のTabControlを取得
            TabControl tab = (TabControl)sender;
            //タブページのテキストを取得
            string txt = tab.TabPages[e.Index].Text;

            //タブのテキストと背景を描画するためのブラシを決定する
            Brush foreBrush, backBrush;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                //選択されているタブのテキストを赤、背景を青とする
                foreBrush = Brushes.White;
                backBrush = Brushes.Navy;
            }
            else
            {
                //選択されていないタブのテキストは灰色、背景を白とする
                foreBrush = Brushes.Black;
                //backBrush = Brushes.LightGray;
                backBrush = Brushes.Snow;
            }

            //StringFormatを作成
            StringFormat sf = new StringFormat();
            //中央に表示する
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            //背景の描画
            e.Graphics.FillRectangle(backBrush, e.Bounds);
            //Textの描画
            e.Graphics.DrawString(txt, e.Font, foreBrush, e.Bounds, sf);
        }

        // メニュー 開設
        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            // 現在未使用
#if false
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FormShelterInfo", "", "開設クリック");

            if (Program.m_EquStat.isConnected() == false)
            {
                MessageBox.Show("Q-ANPIターミナル未接続の為、避難所の開設はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FormOpenShelter frmOpen = new FormOpenShelter();
            frmOpen.ShowDialog();
            if (frmOpen.m_RetBtn == 1)
            {
                if (Program.m_EquStat.isConnected() == false)
                {
                    MessageBox.Show("Q-ANPIターミナル未接続の為、避難所の開設はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FormShelterInfo", "", "開設処理開始");
                setToolStripStatusLabel(LABEL.APP, "避難所情報送信開始  ");

                sendDataArr.Clear();
                m_nSndTotal = 0;
                m_nSndCnt = 0;
                m_StateType0 = STATE.SENDING;
                m_Type0SendingCount = 0;
                m_Type0SendStartCount = 0;
                m_Type0WaitingCount = 0;

                m_commonCountMax += m_Type0CountMax;
                m_commonSendStartCount += m_Type0SendStartCount;
                m_commonSendingCount += m_Type0SendingCount;
                m_commonWaitingCount += m_Type0WaitingCount;

                //string lblmsg = getLabelRtnCount(m_Type0SendStartCount, m_Type0CountMax);
                //setToolStripStatusLabel(LABEL.RTN_SND, "req:" + lblmsg);
                //lblmsg = getLabelRtnCount(m_Type0SendingCount, m_Type0CountMax);
                //setToolStripStatusLabel(LABEL.RTN_RCV, "send:" + lblmsg);
                //lblmsg = getLabelRtnCount(m_Type0WaitingCount, m_Type0CountMax);
                //setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);

                string lblmsg = getLabelRtnCount(m_commonSendStartCount, m_commonCountMax);
                setToolStripStatusLabel(LABEL.RTN_SND, "req:" + lblmsg);
                lblmsg = getLabelRtnCount(m_commonSendingCount, m_commonCountMax);
                setToolStripStatusLabel(LABEL.RTN_RCV, "send:" + lblmsg);
                lblmsg = getLabelRtnCount(m_commonWaitingCount, m_commonCountMax);
                setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);

                DbAccess.TerminalInfo info = Program.m_objActiveTermial;
                if (String.IsNullOrEmpty(info.sid))
                {
                    if (String.IsNullOrEmpty(m_ActiveTarminal_info.sid))
                    {
                        info.sid = info.gid;
                    }
                    else
                    {
                        info.sid = m_ActiveTarminal_info.gid;
                    }
                }
                // 20170626 Type0 サブタイプ追加
                for (int i = 1; i < 3; i++)
                {
                    string sData = MsgConv.ConvTerminalInfoToSendString(true, info, m_PersonCountTotal, i);

                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FormShelterInfo", "OpenMenuItem_Click", sData);

                    m_nSndTotal++;
                    m_Type0SendStartCount++;
                    m_commonSendStartCount++;

                    Program.m_SendFlag = Program.SENDING_TERMINAL_INFO;         // 0:送信中でない  1:個人安否送信  2:避難所情報送信　3:開設/閉鎖/端末情報送信
                    //                setToolStripStatusLabel(1, "端末情報情報送信中  " + (m_nSndCnt + 1).ToString() + "/" + m_nSndTotal.ToString();

                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS,
                        "SendShelterMenuItem_Click", "RTN", "送信 " + m_Type0SendStartCount + " " + m_nSndCnt);

                    byte[] binary = MsgConv.ConvStringToBytes(sData);
                    Program.m_RtnThreadSend.AddSendQue(this, binary);
                }
            }
            // 201604/14 送信成功後に更新するように移動
            ShelterStatusView();

#endif
        }

        // メニュー 閉鎖
        private void CloseMenuItem_Click(object sender, EventArgs e)
        {
            // 現在未使用

#if false
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FormShelterInfo", "", "閉鎖クリック");

            if (Program.m_EquStat.isConnected() == false)
            {
                MessageBox.Show("Q-ANPIターミナル未接続の為、避難所の閉鎖はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FormCloseShelter frmClose = new FormCloseShelter();
            frmClose.ShowDialog();
            if (frmClose.m_RetBtn == 1)
            {
                if (Program.m_EquStat.isConnected() == false)
                {
                    MessageBox.Show("Q-ANPIターミナル未接続の為、避難所の閉鎖はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FormShelterInfo", "", "閉鎖処理開始");
                setToolStripStatusLabel(LABEL.APP, "避難所情報送信開始  ");

                sendDataArr.Clear();
                m_nSndTotal = 0;
                m_nSndCnt = 0;
                m_StateType0 = STATE.SENDING;
                m_Type0SendingCount = 0;
                m_Type0SendStartCount = 0;
                m_Type0WaitingCount = 0;

                m_commonCountMax += m_Type0CountMax;
                m_commonSendStartCount += m_Type0SendStartCount;
                m_commonSendingCount += m_Type0SendingCount;
                m_commonWaitingCount += m_Type0WaitingCount;

                string lblmsg = getLabelRtnCount(m_commonSendStartCount, m_commonCountMax);
                setToolStripStatusLabel(LABEL.RTN_SND, "req:" + lblmsg);
                lblmsg = getLabelRtnCount(m_commonSendingCount, m_commonCountMax);
                setToolStripStatusLabel(LABEL.RTN_RCV, "send:" + lblmsg);
                lblmsg = getLabelRtnCount(m_commonWaitingCount, m_commonCountMax);
                setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);

                //string lblmsg = getLabelRtnCount(m_Type0SendStartCount, m_Type0CountMax);
                //setToolStripStatusLabel(LABEL.RTN_SND, "req:" + lblmsg);
                //lblmsg = getLabelRtnCount(m_Type0SendingCount, m_Type0CountMax);
                //setToolStripStatusLabel(LABEL.RTN_RCV, "send:" + lblmsg);
                //lblmsg = getLabelRtnCount(m_Type0WaitingCount, m_Type0CountMax);
                //setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);

                DbAccess.TerminalInfo info = frmClose.GetTerminalInfo();
                m_ActiveTarminal_info = info;
                m_ShelterStat = int.Parse(info.open_flag);

                // 20170626 Type0 サブタイプ追加
                for (int i = 1; i < 3; i++)
                {
                    string sData = MsgConv.ConvTerminalInfoToSendString(false, info, m_PersonCountTotal, i);
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FormShelterInfo", "CloseMenuItem_Click", sData);

                    m_nSndTotal++;
                    m_Type0SendStartCount++;
                    m_commonSendStartCount++;

                    Program.m_SendFlag = Program.SENDING_TERMINAL_INFO;         // 0:送信中でない  1:個人安否送信  2:避難所情報送信　3:開設/閉鎖/端末情報送信

                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS,
                        "SendShelterMenuItem_Click", "RTN", "送信 " + m_Type0SendStartCount + " " + m_nSndCnt);

                    byte[] binary = MsgConv.ConvStringToBytes(sData);
                    Program.m_RtnThreadSend.AddSendQue(this, binary);
                }
            }
            ShelterStatusView();
#endif

        }

        /**
         * @brief 端末情報送信メニュー押下時
         */
        private void SendShelterMenuItem_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "SendShelterMenuItem_Click", "情報端末情報送信 クリック");

            // 避難者数任意入力をチェックONにしたのに人数未入力の時、エラーを出して終了
            if ((textPersonCnt.Text == "") && (checkPersonCnt.Checked))
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "SendShelterMenuItem_Click", "人数未入力");
                MessageBox.Show("避難者数を入力して下さい。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 最大人数チェック
            int inputPersonNum = 0;
            bool inputOK = false;
            inputOK = int.TryParse(textPersonCnt.Text, out inputPersonNum);
            if ((checkPersonCnt.Checked) && (!inputOK || (inputPersonNum > 131071)))
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "SendShelterMenuItem_Click", "人数オーバー");
                MessageBox.Show("任意の避難者数は131071人以下に設定してください。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 避難者数セット(入所/退所/在宅が「入所」の人のみカウント)
            List<DbAccess.PersonInfo> personInfo = new List<DbAccess.PersonInfo>();
            Program.m_objDbAccess.GetPersonInfoList(m_ActiveTarminal_info.sid, ref personInfo);

            int personCount = 0;
            // 「避難者数を任意の数で送信」のチェックがONの場合、テキストボックス内の入力値を避難者数として設定
            if ((m_ActiveTarminal_info.dummy_num_flag) && (int.TryParse(m_ActiveTarminal_info.dummy_num, out personCount)))
            {
                personCount = int.Parse(m_ActiveTarminal_info.dummy_num);
            }
            // 「避難者数を任意の数で送信」のチェックがOFFの場合、避難所に登録されている個人安否情報から
            // 「入所」の人のみをカウントしたものを避難者数として設定
            else
            {
                // 避難所内の「入所(=0)」の人数を計算
                foreach (var person in personInfo)
                {
                    if (person.sel02 == "0") personCount++;
                }
            }

            // 開設状態送信ダイアログ（避難者数送信）を表示
            FormSendShelterInfo frmSendShelterInfo;
            if ((checkPersonCnt.Checked) && (textPersonCnt.Text != ""))
            {
                frmSendShelterInfo = new FormSendShelterInfo(int.Parse(textPersonCnt.Text));
            }
            else
            {
                frmSendShelterInfo = new FormSendShelterInfo();
            }
            frmSendShelterInfo.StartPosition = FormStartPosition.CenterParent;
            frmSendShelterInfo.SetEntryNum(personCount); // 避難者数セット
            frmSendShelterInfo.SetOpenDate(lblShelterStatDate.Text);
            frmSendShelterInfo.ShowDialog();

            if (frmSendShelterInfo.GetDlgResult())
            {
                if (Program.m_EquStat.isConnected() == false)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "SendShelterMenuItem_Click", "未接続です");
                    MessageBox.Show("Q-ANPIターミナル未接続の為、避難者数の送信はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Program.m_SendFlag != Program.NOT_SENDING)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "SendShelterMenuItem_Click", "送信中です");
                    MessageBox.Show("他の機能でメッセージ送信中の為、避難者数の送信はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 共通送信数の初期化
                initCommonSendingBuff();
                initSending(true);

                // 実行コマンドをセット
                m_NowCommand = SendMessageCommand.SEND_PERSON_NUM;

                // 開設情報送信(避難者数送信)
                SendShelterOpenCloseInfo(frmSendShelterInfo.GetDlgShelterInfo());
            }

            ShelterStatusView();
        }

        // メニュー 情報クリア
        private void InfoClearMenuItem_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "", "情報クリア クリック");

            FormClearShelter frmClear = new FormClearShelter(m_shelterInfoList, Program.m_objActiveTermial.name);
            frmClear.ShowDialog();
            if (frmClear.GetDlgResult())
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "", "情報クリア処理開始");
                setToolStripStatusLabel(LABEL.APP, "情報クリア開始  ");

                // 避難所情報のクリア
                ClearShelterInfo(frmClear.GetDlgShelterInfo());

                // 避難所情報の読み直し用にsid退避    //2020.03.26 Add
                string sid = m_ActiveTarminal_info.sid;
                m_ActiveTarminal_info.sid = "";         // 画面項目の初期化によりDB登録が行われないよう、クリア

                // 現在の避難所情報をすべてクリア
                m_ShelterStat = 0;
                m_shelterInfoList.Clear();
                txtShelterPos.Text = "0,0";
                textPersonCnt.Text = "";        //2020.03.25 Add
                checkPersonCnt.Checked = false; //2020.03.25 Add
                textShelterInfo.Text = "";      //2020.03.25 Add
                cbTextOnly.Checked = true;      //2020.03.25 Add

                // 避難所情報を読み直す。  //2020.03.26 Add
                bool bExist = false;
                Program.m_objDbAccess.GetTerminalInfo(sid, out bExist, ref m_ActiveTarminal_info);

                // アプリ起動時の初期化と同じように、すべてのタブ内の情報を更新する
                m_initialize = true;
                UpdateDisplay();
                m_initialize = false;

                // 画面表示を更新(活性化状態など)
                ShelterStatusView();

                setToolStripStatusLabel(LABEL.APP, "情報クリア完了  ");
                System.Threading.Thread.Sleep(1000);

                DialogResult res = MessageBox.Show("避難所の集計情報が削除されました。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 避難所情報をクリア
        /// </summary>
        /// <param name="clearInfo"></param>
        public void ClearShelterInfo(DbAccess.TerminalInfo clearInfo)
        {
            // 選択した避難所IDのレコードを持つデータをすべて削除
            // person_logテーブルの全削除
            Program.m_objDbAccess.DeleteShelterPersonLog(clearInfo);

            // person_infoテーブルの全削除
            Program.m_objDbAccess.DeleteShelterPersonInfo(clearInfo);

            // person_send_logテーブルの全削除
            Program.m_objDbAccess.DeleteShelterPersonSendLog(clearInfo);

            // total_send_logテーブルの全削除
            Program.m_objDbAccess.DeleteShelterTotalSendLog(clearInfo);

            // 救助支援情報の個別配信メッセージを削除する
            Program.m_objDbAccess.DeleteRescueMsgInfo(clearInfo.gid, clearInfo.smid);

            // 選択された避難所を未開設状態に戻す
            Program.m_objDbAccess.ClearTerminalInfo(clearInfo);
        }

        // メニュー ログオフ
        private void LogOffMenuItem_Click(object sender, EventArgs e)
        {

            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "", "ログオフ クリック");

            DialogResult result = MessageBox.Show("終了しますか？",
                                                    "避難所情報システム",
                                                    MessageBoxButtons.OKCancel,
                                                    MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "", "ログオフ処理開始");
                // 2016/04/19
                m_bProcStop = true;

                // TCP通信スレッドの終了
                m_bTcpIpStop = true;

                //Program.m_tcpThreadSend.Exit();
                //m_frmLogin.Show();
                m_frmLogin.Close();
                Close();
                Application.Exit();
            }

        }

        /// <summary>
        /// メニュー　インポート　ファイル選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportMenuItem_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "", "インポート ファイル選択 クリック");

            FormImport import = new FormImport(this);
            DialogResult result = import.ShowDialog();
            if (result == DialogResult.OK)
            {
                // 画面更新処理
                UpdateDisplay();
                setToolStripStatusLabel(LABEL.APP, "インポート完了  ");
            }
            import.Dispose();
        }

        /// <summary>
        /// メニュー　インポート　受信データ確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportQRCodeMenuItem_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "", "インポート 受信データ確認 クリック");

            // 避難所IDが空となっている個人安否情報を取得
            List<DbAccess.PersonInfo> personInfo = new List<DbAccess.PersonInfo>();
            Program.m_objDbAccess.GetPersonInfoList(Program.TEMP_SHELTER_ID, ref personInfo);

            bool findEmptySid = false;
            foreach (var item in personInfo)
            {
                if (item.sid == Program.TEMP_SHELTER_ID)
                {
                    findEmptySid = true;
                    break;
                }
            }

            if (findEmptySid)
            {
                FormImportQR importQR = new FormImportQR(Program.m_objActiveTermial, personInfo, this);
                DialogResult result = importQR.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // 画面更新処理
                    UpdateDisplay();
                    setToolStripStatusLabel(LABEL.APP, "インポート完了  ");
                }
                importQR.Dispose();
            }
            else
            {
                DialogResult result = MessageBox.Show("受信データはありません。", "避難所情報システム",
                                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        // メニュー エクスポート
        private void ExportMenuItemClick(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "", "エクスポートクリック");

            // ファイルチューザー表示
            SaveFileDialog saveDlg = this.saveFileDialog1;

            // 初期表示フォルダを設定(初回はドキュメントフォルダ、2回目以降は前回表示フォルダ)
            saveDlg.InitialDirectory = m_ExportDir;
            saveDlg.FileName = txtShelterID.Text + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            saveDlg.Filter = "CSV（カンマ区切り）（*.csv）|*.csv";

            DialogResult result = saveDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                // WriteFile
                writeCSV(saveDlg.FileName);
                setToolStripStatusLabel(LABEL.APP, "エクスポート完了 ");

                // 表示したフォルダを記憶(次回エクスポートダイアログ表示時に同じフォルダを表示する)
                m_ExportDir = System.IO.Path.GetDirectoryName(saveDlg.FileName);
            }
            saveDlg.Dispose();
        }
        private void writeCSV(String strPath)
        {
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("shift_jis");

            try
            {
                // 書き込むファイルを開く                   ////
                System.IO.StreamWriter w = new System.IO.StreamWriter(strPath, false, enc);

                // ヘッダー（カラム名）
                String header = "電話番号,氏名,生年月日,性別,入所/退所,公表,住所,ケガ,介護,障がい,妊産婦,避難所内外";

                w.Write(header);
                w.Write("\r\n");

                // 個人安否情報 取得
                List<DbAccess.PersonInfo> infoList = new List<DbAccess.PersonInfo>();
                Program.m_objDbAccess.GetPersonInfoList(Program.m_objActiveTermial.sid, ref infoList);

                for (int i = 0; i < infoList.Count; i++)
                {
                    String data = "";
                    DbAccess.PersonInfo info = infoList[i];
                    // 電話番号
                    data += info.id + ",";
                    // 氏名
                    data += info.name + ",";
                    // 2018/07/20
                    // 年齢 -> 生年月日
                    //data += info.num01 + ",";
                    data += info.txt02 + ",";
                    // 性別(必須入力・未選択は考慮しない)
                    // 0: 男
                    // 1: 女
                    data += (info.sel01 == "0") ? "男," : "女,";
                    // 入所/退所
                    // 0:入所
                    // 1:退所
                    // 2:在宅
                    switch (info.sel02)
                    {
                        case "0":
                            data += "入所,";
                            break;
                        case "1":
                            data += "退所,";
                            break;
                        case "2":
                            data += "在宅,";
                            break;
                        default:
                            break;
                    }
                    // 公表(必須入力・未選択は考慮しない)
                    // 0: 拒否
                    // 1: 許可
                    data += (info.sel03 == "0") ? "拒否," : "許可,";
                    // 住所
                    data += info.txt01 + ",";
                    // ケガ
                    // 0: 無し
                    // 1: 有り
                    // それ以外: 未選択
                    //data += (info.sel04 == "0") ? "無し," : "有り,";
                    if (info.sel04 == "0")
                        data += "無し,";
                    else if (info.sel04 == "1")
                        data += "有り,";
                    else
                        data += ",";
                    // 介護
                    // 0: 不要
                    // 1: 必要
                    // それ以外: 未選択
                    //data += (info.sel05 == "0") ? "不要," : "必要,";
                    if (info.sel05 == "0")
                        data += "不要,";
                    else if (info.sel05 == "1")
                        data += "必要,";
                    else
                        data += ",";
                    // 障害
                    // 0: 無し
                    // 1: 有り
                    // それ以外: 未選択
                    //data += (info.sel06 == "0") ? "無し," : "有り,";
                    if (info.sel06 == "0")
                        data += "無し,";
                    else if (info.sel06 == "1")
                        data += "有り,";
                    else
                        data += ",";
                    // 妊産婦
                    // 0: いいえ
                    // 1: はい
                    // それ以外: 未選択
                    //data += (info.sel07 == "0") ? "いいえ," : "はい,";
                    if (info.sel07 == "0")
                        data += "いいえ,";
                    else if (info.sel07 == "1")
                        data += "はい,";
                    else
                        data += ",";

                    // 避難所内外
                    // 入退所が「入」の時「内」
                    //          「退」もしくは「在」の時「外」
                    if (info.sel08 == "0")
                        data += "内,";
                    else if (info.sel08 == "1")
                        data += "外,";
                    else
                        data += ",";


                    w.Write(data);
                    w.Write("\r\n");

                }
                w.Close();
            }
            catch (Exception e)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelter", "writeCSV", e.ToString());
                MessageBox.Show("エクスポートに失敗しました。" + System.Environment.NewLine + "指定したフォルダに書き込み権限があるか確認してください。",
                                "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // 端末状態メニュー、開設閉鎖日時状態の変更
        // 通信端末に情報を送信後、状態を変更する
        public void ShelterStatusView()
        {
            try
            {
                string OpenStatus = "";
                string SendStatus = "";

                if (m_ShelterStat == SHELTER_STATUS.NO_OPEN)
                {
                    OpenStatus = "未開設";

                    //メニュー
                    RegisterShelterMenuItem.Enabled = true;    //避難所登録
                    EditShelterMenuItem.Enabled = true;        //避難所編集
                    OpenShelterMenuItem.Enabled = true;        //開設(新)
                    CloseShelterMenuItem.Enabled = false;       //閉鎖(新)
                    InfoClearMenuItem.Enabled = false;          //情報クリア
                    SendShelterMenuItem.Enabled = false;        //端末情報送信
                    EditLocationInfoMenuItem.Enabled = false;   //位置情報更新
                    ImportMenuItem.Enabled = false;             //インポート
                    ExportMenuItem.Enabled = false;             //エクスポート

                    //メイン画面ボタン
                    btnNewPersonal.Enabled = false;             //個人情報新規
                    btnDeletePersonal.Enabled = false;          //個人情報削除
                    btnEditPersonal.Enabled = false;            //個人情報編集
                    btnEntryListPersonal.Enabled = false;       //個人情報履歴
                    btnSendPersonal.Enabled = false;            //個人情報送信
                    btnSendListPersonal.Enabled = false;        //個人情報送信履歴
                    btnSendTotalization.Enabled = false;        //避難所情報送信
                    btnSendListTotalization.Enabled = false;    //避難所情報送信履歴
                    btnUpdate.Enabled = false;                  //更新ボタン

                    //メイン画面ラベル
                    lblShelterStat.Text = "開設日時　：";        //開設日時
                    pnlShelterStatus.BackColor = Color.FromKnownColor(KnownColor.LightBlue);
                    lblShelterStatus.Text = "未開設";
                    lblShelterStatDate.Text = "";               //開設日時(日付)
                    lblUpdateDate.Text = "";                    //更新日時

                    btnType3.Enabled = true;                    //新着確認
                }
                else if (m_ShelterStat == SHELTER_STATUS.OPEN)
                {
                    OpenStatus = "開設";

                    //メニュー
                    RegisterShelterMenuItem.Enabled = true;    //避難所登録
                    EditShelterMenuItem.Enabled = true;        //避難所編集
                    OpenShelterMenuItem.Enabled = false;        //開設(新)
                    CloseShelterMenuItem.Enabled = true;       //閉鎖(新)
                    InfoClearMenuItem.Enabled = false;          //情報クリア
                    SendShelterMenuItem.Enabled = true;        //端末情報送信
                    EditLocationInfoMenuItem.Enabled = true;   //位置情報更新
                    ImportMenuItem.Enabled = true;             //インポート
                    ExportMenuItem.Enabled = true;             //エクスポート

                    //メイン画面ボタン
                    btnNewPersonal.Enabled = true;             //個人情報新規
                    btnDeletePersonal.Enabled = true;          //個人情報削除
                    btnEditPersonal.Enabled = true;            //個人情報編集
                    btnEntryListPersonal.Enabled = true;       //個人情報履歴
                    btnSendPersonal.Enabled = true;            //個人情報送信
                    btnSendListPersonal.Enabled = true;        //個人情報送信履歴
                    btnSendTotalization.Enabled = true;        //避難所情報送信
                    btnSendListTotalization.Enabled = true;    //避難所情報送信履歴
                    btnUpdate.Enabled = true;                  //更新ボタン

                    //メイン画面ラベル
                    lblShelterStat.Text = "開設日時　：";        //開設日時
                    pnlShelterStatus.BackColor = Color.GreenYellow;
                    lblShelterStatus.Text = "開設";
                    lblShelterStatDate.Text = m_ActiveTarminal_info.open_datetime;//開設日時(日付)

                    btnType3.Enabled = true;                   //新着確認

                }
                else if (m_ShelterStat == SHELTER_STATUS.CLOSE)
                {
                    OpenStatus = "閉鎖";

                    //メニュー
                    RegisterShelterMenuItem.Enabled = true;    //避難所登録
                    EditShelterMenuItem.Enabled = true;        //避難所編集
                    OpenShelterMenuItem.Enabled = true;        //開設(新)
                    CloseShelterMenuItem.Enabled = false;       //閉鎖(新)
                    InfoClearMenuItem.Enabled = true;          //情報クリア
                    SendShelterMenuItem.Enabled = false;        //端末情報送信
                    EditLocationInfoMenuItem.Enabled = false;   //位置情報更新
                    ImportMenuItem.Enabled = false;             //インポート
                    ExportMenuItem.Enabled = false;             //エクスポート

                    //メイン画面ボタン
                    btnNewPersonal.Enabled = false;             //個人情報新規
                    btnDeletePersonal.Enabled = false;          //個人情報削除
                    btnEditPersonal.Enabled = false;            //個人情報編集
                    btnEntryListPersonal.Enabled = false;       //個人情報履歴
                    btnSendPersonal.Enabled = false;            //個人情報送信
                    btnSendListPersonal.Enabled = true;         //個人情報送信履歴
                    SendShelterMenuItem.Enabled = false;        //避難者数送信
                    btnSendTotalization.Enabled = false;        //避難所情報送信
                    btnSendListTotalization.Enabled = true;     //避難所情報送信履歴
                    btnUpdate.Enabled = true;                   //更新ボタン

                    //メイン画面ラベル
                    lblShelterStat.Text = "閉鎖日時　：";        //閉鎖日時
                    pnlShelterStatus.BackColor = Color.FromKnownColor(KnownColor.LightCoral);
                    lblShelterStatus.Text = "閉鎖";
                    lblShelterStatDate.Text = m_ActiveTarminal_info.close_datetime;//開設日時(日付)

                    btnType3.Enabled = true;                   //新着確認
                }

                // 表示中の避難所がない場合、各ボタンを非活性にする
                if (selectShelterName.Text == "")
                {
                    OpenStatus = "避難所なし";

                    btnNewPersonal.Enabled = false;             //個人情報新規
                    btnDeletePersonal.Enabled = false;          //個人情報削除
                    btnEditPersonal.Enabled = false;            //個人情報編集
                    btnEntryListPersonal.Enabled = false;       //個人情報履歴
                    btnSendPersonal.Enabled = false;            //個人情報送信
                    btnSendListPersonal.Enabled = false;        //個人情報送信履歴
                    btnSendTotalization.Enabled = false;        //避難所情報送信
                    btnSendListTotalization.Enabled = false;    //避難所情報送信履歴
                    btnUpdate.Enabled = false;                  //更新ボタン
                    SendShelterMenuItem.Enabled = false;        //避難者数送信
                    EditShelterMenuItem.Enabled = false;        //避難所編集
                    OpenShelterMenuItem.Enabled = false;        //開設
                    CloseShelterMenuItem.Enabled = false;       //閉鎖
                    InfoClearMenuItem.Enabled = false;          //情報クリア
                    SendShelterMenuItem.Enabled = false;        //情報送信
                    EditLocationInfoMenuItem.Enabled = false;   //位置情報更新

                    btnType3.Enabled = false;                   //新着確認
                }
                else
                {
                    // 救助支援情報リストを更新
                    loadList(2);
                }


                //送信状態によって活性化/非活性化を切り替える
                // 0:送信中でない  1:個人安否送信  2:避難所情報送信　3:開設/閉鎖/端末情報送信

                //何かしらの情報送信中は「登録」「編集」「開設」「閉鎖」「情報クリア」
                //「避難所情報送信」「位置情報更新」「送信」「避難所切替」を無効化する
                if (Program.m_SendFlag != Program.NOT_SENDING)
                {
                    SendStatus = "送信中";

                    RegisterShelterMenuItem.Enabled = false;    //登録
                    EditShelterMenuItem.Enabled = false;        //編集
                    OpenShelterMenuItem.Enabled = false;        //開設(新)
                    CloseShelterMenuItem.Enabled = false;       //閉鎖(新)
                    SendShelterMenuItem.Enabled = false;        //避難所情報送信
                    EditLocationInfoMenuItem.Enabled = false;   //位置情報更新
                    InfoClearMenuItem.Enabled = false;          //情報クリア 
                    SendShelterMenuItem.Enabled = false;        //避難者数送信
                    btnSendPersonal.Enabled = false;            //個人安否情報-送信
                    btnSendTotalization.Enabled = false;        //避難所集計情報-送信
                    btnType3.Enabled = false;                   //新着確認

                    selectShelterName.Enabled = false;          //避難所切替プルダウン
                }
                else
                {
                    SendStatus = "送信中ではない";
                    selectShelterName.Enabled = true;
                    m_respCount = 0;

                    // 避難所詳細情報送信後、タイムアウトや切断などで失敗ログが出力できていなかった場合
                    // 送信中解除のタイミングで失敗ログを出力
                    if (m_TotalSendLog != null)
                    {
                        foreach (DbAccess.TotalSendLog sendLog in m_TotalSendLog)
                        {
                            SaveTotalSendLog(sendLog, false);
                        }
                        m_TotalSendLog.Clear();
                    }

                    // 個人安否情報送信後、タイムアウトや切断などで失敗ログが出力できていなかった場合
                    // 送信中解除のタイミングで失敗ログを出力
                    if (m_PersonSendList != null)
                    {
                        foreach (var item in m_PersonSendList)
                        {
                            // 送信待ちになっている個人安否情報を失敗として出力
                            if (item.bSendWait)
                            {
                                DbAccess.PersonSendLog log = new DbAccess.PersonSendLog();
                                log.Set(item.info);
                                log.sendresult = "0";
                                log.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                Program.m_objDbAccess.InsertPersonSendLog(log);
                            }
                        }
                        m_PersonSendList.Clear();
                    }
                }

                // 避難所切替プルダウンは、登録避難所が0件の場合、非活性化する
                if ((selectShelterName.Items.Count < 1))
                {
                    selectShelterName.Enabled = false;
                }

                // 個人安否情報に「未送信」のものが存在しない場合、「送信」ボタンを非活性化
                if (btnSendPersonal.Enabled)
                {
                    btnSendPersonal.Enabled = false;
                    for (int ic = 0; ic < listPersonal.Items.Count; ic++)
                    {
                        if (listPersonal.Items[ic].SubItems[1].Text == "未送信")
                        {
                            btnSendPersonal.Enabled = true;
                            break;
                        }
                    }
                }

#if DEBUG
                // デバッグ（避難所名未送信の時に更新日時がオレンジ色になる）
                lbl_LastSendShelterName.Visible = true;

                if (m_lastSendShelterName.sid != Program.m_objActiveTermial.sid)
                {
                    lbl_LastSendShelterName.BackColor = System.Drawing.Color.Orange;
                    if ((m_lastSendShelterName.sid == "")||(m_lastSendShelterName.sid == null))
                    {
                        lbl_LastSendShelterName.Text = "最終送信避難所名：[避難所名未送信]";
                    }
                    else
                    {
                        lbl_LastSendShelterName.Text = "最終送信避難所名：[" + m_lastSendShelterName.sid + "]" + m_lastSendShelterName.name;
                    }
                }
                else
                {
                    lbl_LastSendShelterName.BackColor = System.Drawing.Color.Empty;
                    lbl_LastSendShelterName.Text = "最終送信避難所名：[" + Program.m_objActiveTermial.sid + "]" + Program.m_objActiveTermial.name;
                }
#else
                lbl_LastSendShelterName.Visible = false;
#endif

                // 現在の活性化状態をログに出力
                Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "ShelterStatusView", "活性化状態変更", "開設状態：" + OpenStatus + " 送信状態：" + SendStatus);
            }
            catch (Exception e)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "ShelterStatusView", "活性化状態変更エラー", e.Message);
            }
        }

        //更新ボタン
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "person", "更新クリック");

            // ソートフラグをリセット(更新日時順に設定)
            m_order = 0;
            listPersonal.ListViewItemSorter =
                new ListViewItemComparer(1, m_order);

            // 画面更新処理
            UpdateDisplay();
        }

        //
        public void UpdateDisplay()
        {
            if (m_frmWait.Visible == false)
            {
                m_frmWait.Show(this);
            }
            else
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "FormShelterInfo", "", "QZSS取得処理中のため処理しない");
                return;
            }
            UpdateListData(true);

            UpdateShelterInfoList();
        }

        // 個人安否・避難所情報データ表示
        private void UpdateListThread()
        {
            if (m_frmWait.Visible == false)
            {
                m_frmWait.Show(this);
            }
            if (m_ListViewThread != null)
            {
                return;
            }
            // スレッド作成
            m_ListViewThread = new System.Threading.Thread(UpdateListData);

            // スレッドを開始
            m_ListViewThread.Start();
        }

        /**
         * @brief EquStatデータ取得要求(スレッド確認)
         */
        public void UpdateListData(bool checkThread)
        {
            if (checkThread)
            {
                if (m_ListViewThread != null)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "FormShelterInfo", "UpdateListData Error");
                    return;
                }
            }
            UpdateListData();
        }

        /**
         * @brief EquStatデータ取得要求(スレッド)
         */
        public void UpdateListData()
        {
            if (m_bProcStop == true)
            {
                if (m_ListViewThread != null)
                {
                    m_ListViewThread = null;
                }
                return;
            }
            try
            {
                lblUpdateDate.Text = "更新日時：" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                // 避難所が登録されていない場合は処理を実施しない
                if (Program.m_objActiveTermial.sid != null)
                {
                    // 個人安否情報 取得
                    Program.m_objDbAccess.GetPersonInfoList(Program.m_objActiveTermial.sid, ref m_activePersonInfoList);

                    // 更新日時でソート
                    m_activePersonInfoList.Sort((b, a) => a.update_datetime.CompareTo(b.update_datetime));

                    // 行データの値を設定
                    string[] strItems = new string[15];
                    int ItemLen = m_activePersonInfoList.Count;

                    // 表領域の画面更新停止(画面上で項目が削除/追加されると処理が遅い)
                    listPersonal.BeginUpdate();

                    // 表領域の初期化
                    listPersonal.Items.Clear();

                    // ---行データの追加---
                    ListViewItem[] addDatas = null;
                    int nIdx;
                    for (nIdx = 0; nIdx < ItemLen; nIdx++)
                    {
                        // person_infoをstring配列に変換
                        strItems = ConvPersonInfoToStringList(m_activePersonInfoList[nIdx]);

                        // 一時配列に格納
                        Array.Resize(ref addDatas, nIdx + 1);
                        addDatas[nIdx] = new ListViewItem(strItems);
                    }

                    if (addDatas != null)
                    {
                        // リストにセット
                        listPersonal.Items.AddRange(addDatas);
                    }

                    // 表領域の画面更新再開
                    listPersonal.EndUpdate();

                    // ---行データの追加---

                    //4/4
                    if (ItemLen > 0)
                    {
                        if (listPersonal.Items.Count - 1 < m_SelectRow)
                        {
                            m_SelectRow = listPersonal.Items.Count - 1;
                        }
                        SetSelRowItemNo();

                        // 開設時のみ使用可能
                        ShelterStatusView();
                    }
                }

                // 集計
                // タブが避難所情報になっている時のみ避難所情報のリストを更新する(アプリ起動時は必ず更新する)
                if ((tabControl1.SelectedIndex == 1) || (m_initialize))
                {
                    TotalizationPersonInfo(Program.m_objActiveTermial, m_activePersonInfoList, ref m_TotalizationList);
                }
                listTotalization.Items.Clear();
                listTotalizationDetail.Items.Clear();
                {
                    // 行データの値を設定
                    string[] strItems = new string[4];

                    // 行データの追加
                    int nIdx;
                    for (nIdx = 0; nIdx < 12; nIdx++)
                    {
                        if ((m_TotalizationList.Length / strItems.Length) > nIdx)
                        {
                            int nColIdx;
                            for (nColIdx = 0; nColIdx < strItems.Length; nColIdx++)
                            {
                                strItems[nColIdx] = m_TotalizationList[nIdx, nColIdx];
                            }
                        }
                        else
                        {
                            int nColIdx;
                            for (nColIdx = 0; nColIdx < strItems.Length; nColIdx++)
                            {
                                strItems[nColIdx] = "";
                            }
                        }
                        if (nIdx < 2)
                        {
                            listTotalization.Items.Add(new ListViewItem(strItems));
                        }
                        else
                        {
                            listTotalizationDetail.Items.Add(new ListViewItem(strItems));
                        }
                    }
                }
                //2020.03.24




                // 救助支援情報リストの更新
                loadList(2);
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "UpdateList Error -", ex.ToString());
            }
            finally
            {
                if (m_frmWait.Visible == true)
                {
                    m_frmWait.Hide();
                }

                if (m_ListViewThread != null)
                {
                    m_ListViewThread = null;
                }
            }
        }

        private string[] ConvPersonInfoToStringList(DbAccess.PersonInfo info)
        {
            string[] strItems = new string[14];
            int nIdx;
            bool bSendWait = false;
            // 初期化
            for (nIdx = 0; nIdx < strItems.Length; nIdx++)
            {
                strItems[nIdx] = "";
            }

            int nStrListIdx = 0;
            // No
            strItems[nStrListIdx] = "0";
            nStrListIdx++;

            // 済 送信日時 > 更新日時  送信済みとする。
            DateTime sendDay = DateTime.MinValue;
            DateTime.TryParse(info.send_datetime, out sendDay);

            DateTime upDay = DateTime.MinValue;
            DateTime.TryParse(info.update_datetime, out upDay);

            // 送信日時が先の場合は送信済み
            if (sendDay > upDay)
            {
                //                strItems[nStrListIdx] = "済";
                strItems[nStrListIdx] = "送信済";
            }
            else
            {
                // 2016/04/18 送信中表示の対応
                // 送信日時が後の場合、送信リストに存在するかを確認
                int nListIdx;
                for (nListIdx = 0; nListIdx < m_PersonSendList.Count; nListIdx++)
                {
                    //2016/04/19
                    if (m_PersonSendList[nListIdx].info.id == info.id && m_PersonSendList[nListIdx].info.name == info.name)
                    {
                        bSendWait = m_PersonSendList[nListIdx].bSendWait;
                    }

                    if (m_PersonSendList[nListIdx].info == info)
                    {
                        // 一致しても更新されていたら未送信とする
                        if (m_PersonSendList[nListIdx].bEdit || m_PersonSendList[nListIdx].bDelete)
                        {
                            bSendWait = true;
                        }
                        break;
                    }
                }

                // リストに存在していなければ未送信 & 送信中でない場合も未送信
                if ((bSendWait) && (Program.m_SendFlag != Program.NOT_SENDING))
                {
                    //strItems[nStrListIdx] = "中";
                    strItems[nStrListIdx] = "送信中";
                }
                // リストに存在していれば送信中
                else
                {
                    //strItems[nStrListIdx] = "未";
                    strItems[nStrListIdx] = "未送信";
                }

            }
            nStrListIdx++;

            // 氏名
            strItems[nStrListIdx] = info.name;
            nStrListIdx++;

            // 電話番号
            strItems[nStrListIdx] = info.id;
            nStrListIdx++;

            // 年齢 -> 生年月日(生年月日から年齢を計算する)
            int age = getAge(info.txt02);
            if (age <= 0)
            {
                // 年齢が0歳未満になってしまう場合(入力された生年月日がPCシステム時刻より未来の場合)、0歳で表示する
                age = 0;
            }
            strItems[nStrListIdx] = age.ToString();
            nStrListIdx++;

            // 性別       0:男性 1:女性
            strItems[nStrListIdx] = Program.ConvSel01(info.sel01);
            nStrListIdx++;

            // 入/退所  0:入所 1:退所 2:在宅
            strItems[nStrListIdx] = Program.ConvSel02(info.sel02);
            nStrListIdx++;

            // 公表       0:しない 1:する
            strItems[nStrListIdx] = Program.ConvSel03(info.sel03);
            nStrListIdx++;

            // 住所
            strItems[nStrListIdx] = info.txt01;
            nStrListIdx++;

            // 怪我       0:無 1:有 2:未使用 3:未選択
            strItems[nStrListIdx] = Program.ConvSel04(info.sel04);
            nStrListIdx++;

            // 2016/04/18 救護を削除：これ以降を前詰
            //// 救護       2:未選択 0:否  1:要
            //strItems[nStrListIdx] = Program.ConvSel05(info.sel05);
            //nStrListIdx++;

            // 介護       0:否 1:要 2:未選択
            strItems[nStrListIdx] = Program.ConvSel05(info.sel05);
            nStrListIdx++;

            // 障がい    0:無 1:有 2:未選択
            strItems[nStrListIdx] = Program.ConvSel06(info.sel06);
            nStrListIdx++;

            // 妊産婦    0:いいえ 1:はい 2:未選択
            strItems[nStrListIdx] = Program.ConvSel07(info.sel07);
            nStrListIdx++;

            // 避難所内外   入/退が「入」の場合は「内」それ以外の場合は「外」
            strItems[nStrListIdx] = Program.ConvSel08(info.sel08);
            nStrListIdx++;

            return strItems;
        }

        // 集計
        private void TotalizationPersonInfo(DbAccess.TerminalInfo terminalInfo, List<DbAccess.PersonInfo> personInfo, ref string[,] totalList)
        {
            int nIdx;

            int nNum01 = 0;
            int nNum02 = 0;
            int nNum03 = 0;
            int nNum04 = 0;
            int nNum05 = 0;
            int nNum06 = 0;
            int nNum07 = 0;
            int nNum08 = 0;
            int nNum09 = 0;
            int nNum10 = 0;
            int nNum11 = 0;
            int nNum12 = 0;

            for (nIdx = 0; nIdx < personInfo.Count; nIdx++)
            {
                //{"1", "", "避難者数", "55"},
                if (personInfo[nIdx].sel02 == "0")
                {
                    nNum01++;
                }

                //{"2", "", "在宅者数", "0"},
                if (personInfo[nIdx].sel02 == "2")
                {
                    nNum02++;
                }

                // 2016/04/19 退所者は、集計に加算しない
                if (personInfo[nIdx].sel02 != "1")
                {

                    //{"3", "", "男性", "30"},
                    if (personInfo[nIdx].sel01 == "0")
                    {
                        nNum03++;
                    }

                    //{"4", "", "女性", "25"},
                    if (personInfo[nIdx].sel01 == "1")
                    {
                        nNum04++;
                    }

                    //{"5", "", "負傷者", "0"},
                    if (personInfo[nIdx].sel04 == "1")
                    {
                        nNum05++;
                    }


                    // 2016/04/18 救護を削除：これ以降を前詰
                    ////{"7", "", "要救護者数", "1"},
                    //if (m_infoList[nIdx].sel05 == "1")
                    //{
                    //    nNum07++;
                    //}

                    //{"6", "", "要介護者数", "5"},
                    if (personInfo[nIdx].sel05 == "1")
                    {
                        nNum06++;
                    }

                    //{"7", "", "障がい者数", "1"},
                    if (personInfo[nIdx].sel06 == "1")
                    {
                        nNum07++;
                    }

                    //{"8", "", "妊産婦者数", "1"},
                    if (personInfo[nIdx].sel07 == "1")
                    {
                        nNum08++;
                    }

                    // ●4/14
                    //{"9", "", "高齢者児数", "0"},
                    //if (int.Parse(m_infoList[nIdx].num01) >= 65)
                    //{
                    //    nNum09++;
                    //}
                    //{"10", "", "乳児数", "3"},
                    //                if (int.Parse(m_infoList[nIdx].num01) < 7)
                    //if (int.Parse(m_infoList[nIdx].num01) < 1)
                    //{
                    //    nNum10++;
                    //}

                    // 20180720 年齢 -> 生年月日
                    //{"9", "", "高齢者数", "0"},
                    if (getAge(personInfo[nIdx].txt02) >= 65)
                    {
                        nNum09++;
                    }
                    //{"10", "", "乳児数", "3"},
                    if (getAge(personInfo[nIdx].txt02) < 1)
                    {
                        nNum10++;
                    }

                    //{"11", "", "避難所内", "55"},
                    if (personInfo[nIdx].sel08 == "0")
                    {
                        nNum11++;
                    }

                    //{"12", "", "避難所外", "55"},
                    if (personInfo[nIdx].sel08 == "1")
                    {
                        nNum12++;
                    }
                }
            }

            totalList[0, 3] = nNum01.ToString();        // 入所者数
            totalList[1, 3] = nNum02.ToString();        // 在宅者数
            totalList[2, 3] = nNum03.ToString();        // 男性
            totalList[3, 3] = nNum04.ToString();        // 女性
            totalList[4, 3] = nNum05.ToString();        // 負傷者
            totalList[5, 3] = nNum06.ToString();        // 要介護者
            totalList[6, 3] = nNum07.ToString();        // 障がい者
            totalList[7, 3] = nNum08.ToString();        // 妊産婦
            totalList[8, 3] = nNum09.ToString();        // 高齢者
            totalList[9, 3] = nNum10.ToString();        // 乳児
            totalList[10, 3] = nNum11.ToString();       // 避難所内
            totalList[11, 3] = nNum12.ToString();       // 避難所外
            totalList[12, 3] = terminalInfo.status;     // 避難所状況
        }

        /****************************  ***************************/
        /*  Step 2 */
        /****************************  ***************************/

        //----------------------------------------------------------------------------------------------
        // 災害危機通報　L1S
        //----------------------------------------------------------------------------------------------

        // L1S取得要求に対する応答が来た
        public void L1sThread_Event(object sender, int code, byte[] msgdata, string msg2)
        {
            m_rcvState |= RCV_STATE.CONNECT_L1S;
            if (msgdata.Length == 0)
            {
                // 切断
                m_rcvState ^= RCV_STATE.CONNECT_L1S;
                EventDisconnect(msg2);
                return;
            }

            // L1S要求応答装置固有結果コード
            if (msgdata.Length == 20)
            {
                String rspmsg = "L1S取得要求:";
                DecodeManager dm = new DecodeManager();
                int l1srsp = dm.decodeInt(msgdata, (2 * QAnpiCtrlLib.consts.EncDecConst.BYTE_BIT_SIZE), (14 * QAnpiCtrlLib.consts.EncDecConst.BYTE_BIT_SIZE));
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "L1sThread_Event", "L1SGetReqRsp", "ID" + l1srsp);
                // L1S応答がGPS時刻未取得(7010)だった場合
                if (l1srsp == 7010)
                {
                    // 要求受信時、GPS初期化NGのため災危通報を取得できない
                    rspmsg += "GPS時刻未取得のため接続不可(" + l1srsp + ")";
                    m_rcvState ^= RCV_STATE.CONNECT_L1S;
                    EventDisconnect(rspmsg);
                }
                // L1S応答がその他(7100)だった場合
                else if (l1srsp == 7100)
                {
                    // N/A
                    rspmsg += "その他の要因(" + l1srsp + ")";
                }
                else
                {
                    // N/A
                }
                setToolStripStatusLabel(LABEL.L1S, rspmsg);
                return;
            }
            setToolStripStatusLabel(LABEL.L1S, msg2);
        }

        //----------------------------------------------------------------------------------------------
        // EQU 監視ステータス
        //----------------------------------------------------------------------------------------------
        // 監視ステータス要求に対する応答が来た
        public void EquThread_Event(object sender, int code, byte[] msgdata, string msg2)
        {
            m_rcvState |= RCV_STATE.CONNECT_EQU;
            if (msgdata.Length == 0)
            {
                // 切断
                m_rcvState ^= RCV_STATE.CONNECT_EQU;
                EventDisconnect(msg2);
                return;
            }

            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FormShelterInfo", "EquThread_Event", msg2);
            // デコード
            MsgSBandEquStatRsp msg3005 = new MsgSBandEquStatRsp();
            msg3005.encodedData = msgdata;
            msg3005.decode(false);

            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FormShelterInfo", "EquThread_Event dataTime"
                , "" + msg3005.dataTime.hour + ":" + msg3005.dataTime.min
                + ":" + msg3005.dataTime.sec + "." + msg3005.dataTime.msec.ToString("000"));
            // インジケータ
            // および装置状態

            // 接続
            SetIndicator(Indicator.Connection, IndStatus.Normal);
            if (m_frmTermStatus != null)
            {
                m_frmTermStatus.SetConnectStats(FormTermStatus.ConnectStat.Connect);
            }

            // 同期状態
            if (msg3005.equStatInfo.opeStat == 4)
            {
                SetIndicator(Indicator.Synchronize, IndStatus.Normal);
                if (m_frmTermStatus != null)
                {
                    m_frmTermStatus.SetModemStat((FormTermStatus.ModemStat)msg3005.equStatInfo.opeStat);
                }
            }
            else
            {
                SetIndicator(Indicator.Synchronize, IndStatus.Disconnect);
                if (m_frmTermStatus != null)
                {
                    m_frmTermStatus.SetModemStat((FormTermStatus.ModemStat)msg3005.equStatInfo.opeStat);
                }
            }

            // 電圧状態
            if (msg3005.equStatInfo.voltStat == 0)
            {
                if (m_frmTermStatus != null)
                {
                    m_frmTermStatus.SetVoltStat(FormTermStatus.VoltStat.Normal);
                }
            }
            else
            {
                if (m_frmTermStatus != null)
                {
                    m_frmTermStatus.SetVoltStat(FormTermStatus.VoltStat.Exception);
                }
            }

            // 装置状態
            if (msg3005.equStatInfo.voltStat == 0
                && (msg3005.equStatInfo.gpsStat == 0
                    || msg3005.equStatInfo.gpsStat == 1
                    || msg3005.equStatInfo.gpsStat == 3
                    || msg3005.equStatInfo.gpsStat == 4
                    || msg3005.equStatInfo.gpsStat == 5)
                && msg3005.equStatInfo.tmpStat == 0
                && msg3005.equStatInfo.sndrcvStat == 0
                && (msg3005.equStatInfo.opeStat == 1
                    || msg3005.equStatInfo.opeStat == 3
                    || msg3005.equStatInfo.opeStat == 4
                    || msg3005.equStatInfo.opeStat == 5)
                && msg3005.equStatInfo.swAlarm == 0
                )
            {
                SetIndicator(Indicator.TermStatus, IndStatus.Normal);
                m_TermStatusInd = IndStatus.Normal;
            }
            else
            {
                SetIndicator(Indicator.TermStatus, IndStatus.Exception);

                // 異常状態になったらエラー音鳴動する
                if (m_TermStatusInd != IndStatus.Exception)
                {
                    m_TermStatusInd = IndStatus.Exception;
                    System.Media.SystemSounds.Exclamation.Play();
                }
            }

            if (msg3005.equStatInfo.lanStat == 1) // lanStat = SubGHz_State = SUBGHZ_UNINITIALIZED = 1
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "EquThread_Event", "SUBGHZ", "サブギガ未初期化");
            }
            else if (msg3005.equStatInfo.lanStat == 2) // lanStat = SubGHz_State = SUBGHZ_INITIALIZED_NG = 2
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "EquThread_Event", "SUBGHZ", "サブギガ初期化NG");
            }
            else
            {
                // lanStat = SubGHz_State = SUBGHZ_INITIALIZED_OK = 0
            }


            // 装置状態フォームのみ
            if (m_frmTermStatus != null)
            {
                // GPS
                m_frmTermStatus.SetGpsStat((FormTermStatus.GpsStat)msg3005.equStatInfo.gpsStat);
                // 温度
                m_frmTermStatus.SetTempStat((FormTermStatus.TempStat)msg3005.equStatInfo.tmpStat);
                // Ｓ帯送受信状態
                m_frmTermStatus.SetSBandStats(msg3005.equStatInfo.sndrcvStat);
                // SWアラーム
                m_frmTermStatus.SetSWAlarmStats(msg3005.equStatInfo.swAlarm);

            }
        }

        //----------------------------------------------------------------------------------------------
        // RTN
        //----------------------------------------------------------------------------------------------
        private void PostSend()
        {
            // 2016/04/19
            //if (m_bProcStop == true)
            {
                // 全て送信したとして終了する
                //    m_nSndCnt = m_nSndTotal;
            }

            if (m_nSndCnt < m_nSndTotal)
            {
                if (m_Type1SendStartCount < m_sendDataArr.Count)
                {

                    // 二進数文字列取得
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS,
                        "PostSend", "RTN", "送信 " + m_Type1SendStartCount + " " + m_nSndCnt);
                    string sndVal = (string)m_sendDataArr[m_Type1SendStartCount];
                    byte[] binary = MsgConv.ConvStringToBytes(sndVal);

                    //Program.m_tcpThreadSend.TcpProc(binary);

                    // 送信中フラグをONにし、送信処理
                    m_nowSending = true;

                    Program.m_RtnThreadSend.AddSendQue(binary);
                }
            }
        }

        // RTN要求を送った（応答はまだ受信していない）
        public void RtnThread_SendStart(object sender, int code, byte[] msgdata, string msg2)
        {
            // [test]確認用に応答の送信時間を登録
            setToolStripStatusLabel(LABEL.CONNECT, "RTN送信要求:" + msg2 + "");
            Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "RtnThread_SendStart", "RTN", "送信 " + msg2);
            MsgSBandRtnSendRsp msg3091 = new MsgSBandRtnSendRsp();
            msg3091.encodedData = msgdata;
            msg3091.decode(false);

            int dayOfYear = msg3091.rtnSendInfo.sendTime.dayOfYear;
            int hour = msg3091.rtnSendInfo.sendTime.hour;
            int min = msg3091.rtnSendInfo.sendTime.min;
            int sec = msg3091.rtnSendInfo.sendTime.sec;
            int msec = msg3091.rtnSendInfo.sendTime.msec;
            string sendTimeDecode = dayOfYear + "日 " + hour + "時 " + min + "分 " + sec + "秒 " + msec + "ms";
            Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "RtnThread_SendStart", "RTN送信要求:", "送信希望時刻:" + sendTimeDecode);
            Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "RtnThread_SendStart", "RTN送信要求:", "送信内容:" + GetByteArrayString(msgdata));

            int mt = msg3091.rtnSendInfo.rtnDataMsgType;

            // 個人安否情報
            if (mt == 1)
            {
                m_Type1SendStartCount++; // 送信済み（送達確認まだ）カウント
                m_commonSendStartCount++;

                // 結果表示
                //string lblmsg = getLabelRtnCount(m_Type1SendStartCount, m_Type1CountMax);
                string lblmsg = getLabelRtnCount(m_commonSendStartCount, m_commonCountMax);

                setToolStripStatusLabel(LABEL.RTN_SND, "req:" + lblmsg);

                if (m_Type1SendStartCount >= m_Type1CountMax)
                {
                    // 全部送信
                }
                else
                {
                    PostSend();
                }
            }

            // 避難所情報
            else if (mt == 2)
            {
                // 避難所詳細 or 避難所名　判別
                bool isShelterName = false;
                byte[] detailData = msg3091.encodedData.Skip(28).Take(12).ToArray();

                // 2進数変換
                string binStr = "";
                for (int ic = 0; ic < detailData.Length; ic++)
                {
                    string tmp = Convert.ToString((int)detailData[ic], 2);
                    if (tmp.Length < 8)
                    {
                        tmp = tmp.PadLeft(8, '0');
                    }
                    binStr += tmp;
                }
                // type2メッセージS帯RTNデータの62bit目(避難所種別)が「1」の場合、避難所名登録
                if (binStr[61] == '1') isShelterName = true;

                if (isShelterName)
                {
                    if (m_StateType2_1 == STATE.SENDING)
                    {
                        m_Type2SendStartCount_1++; // 送信済み（送達確認まだ）カウント
                        m_commonSendStartCount++;

                        // 結果表示         
                        string lblmsg = getLabelRtnCount(m_commonSendStartCount, m_commonCountMax);

                        setToolStripStatusLabel(LABEL.RTN_SND, "req:" + lblmsg);

                        if (m_Type2SendStartCount_1 >= m_Type2CountMax_1)
                        {
                            // 全部送信
                        }
                        else
                        {
                            // 送信中フラグをONにし、送信処理
                            m_nowSending = true;
                            string sndVal = (string)m_sendDataArr[m_Type2SendStartCount_1];
                            byte[] binary = MsgConv.ConvStringToBytes(sndVal);
                            Program.m_RtnThreadSend.AddSendQue(binary);
                        }
                    }
                }
                else
                {
                    if (m_StateType2_0 == STATE.SENDING)
                    {
                        m_Type2SendStartCount_0++; // 送信済み（送達確認まだ）カウント
                        m_commonSendStartCount++;

                        // 結果表示         
                        string lblmsg = getLabelRtnCount(m_commonSendStartCount, m_commonCountMax);

                        setToolStripStatusLabel(LABEL.RTN_SND, "req:" + lblmsg);

                        if (m_Type2SendStartCount_0 >= m_Type2CountMax_0)
                        {
                            // 全部送信
                        }
                        else
                        {
                            // 送信中フラグをONにし、送信処理
                            m_nowSending = true;
                            string sndVal = (string)m_sendDataArr[m_Type2SendStartCount_0];
                            byte[] binary = MsgConv.ConvStringToBytes(sndVal);
                            Program.m_RtnThreadSend.AddSendQue(binary);
                        }
                    }
                }
            }

            // 開設など
            else if (mt == 0)
            {
                m_commonSendStartCount++;

                // 開設・閉鎖・端末情報送信は2通連続送信する
                string lblmsg = getLabelRtnCount(m_commonSendStartCount, m_commonCountMax);
                setToolStripStatusLabel(LABEL.RTN_SND, "req:" + lblmsg);
            }
            else if (mt == 3)
            {
                // 要求/送信応答/送達確認/受信済みメッセージ/未受信メッセージ数
                m_Type3SendStartCount++;
                string lblmsg = getLabelRtnCount(m_Type3SendStartCount, m_Type3CountMax);
                setToolStripStatusLabel(LABEL.RTN_SND, "req:" + lblmsg);
            }
            else
            {
                // N/A
            }
        }

        // RTN要求に対する応答が来た
        public void RtnThread_Event(object sender, int code, byte[] msgdata, string msg2)
        {
            // [test]確認用に応答の送信時間を登録
            setToolStripStatusLabel(LABEL.CONNECT, "RTN応答:" + msg2 + "");
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "RtnThread_Event", "RTN", "RTN応答 " + msg2);

            Program.m_RtnThreadSend.setWaiting(false);

            // 送信制限状態でのRTN要求時イベント
            if (code == RtnThread.RESULT_SEND_RESTRICT || code == RtnThread.RESULT_FREQ_RESTRICT)
            {
                // カウント初期化
                if (m_StateType1 == STATE.SENDING)
                {
                    setToolStripStatusLabel(LABEL.APP, "個人安否情報送信失敗");
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "RtnThread_Event", "RTN", "個人安否情報送信失敗");

                    lock (m_PersonSendList)
                    {
                        m_PersonSendList.Clear();
                    }
                }
                if (m_StateType2_0 == STATE.SENDING)
                {
                    setToolStripStatusLabel(LABEL.APP, "避難所詳細情報送信失敗");
                    // 失敗で送信履歴登録
                    // 避難所詳細情報を送信するときのみ、詳細情報送信ログを出力
                    if (m_isType2subDetail)
                    {
                        foreach (DbAccess.TotalSendLog sendLog in m_TotalSendLog)
                        {
                            SaveTotalSendLog(sendLog, false);
                        }
                        m_TotalSendLog.Clear();
                    }
                }
                if (m_StateType2_1 == STATE.SENDING)
                {
                    setToolStripStatusLabel(LABEL.APP, "避難所名情報送信失敗");
                }
                if (m_StateType0 == STATE.SENDING)
                {
                    setToolStripStatusLabel(LABEL.APP, "端末情報送信失敗");
                }
                if (m_StateType3 == STATE.SENDING)
                {
                    setToolStripStatusLabel(LABEL.APP, "救助支援要求送信失敗");
                }
                // 送信状態リセット
                initSending(true);

                return;
            }

            if (code == RtnThread.RESULT_TIMEOUT || msgdata == null)
            {
                // カウント初期化
                if (m_StateType1 == STATE.SENDING)
                {
                    setToolStripStatusLabel(LABEL.APP, "個人安否情報送信タイムアウト");
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "RtnThread_Event", "RTN", "個人安否情報送信タイムアウト");

                    // 個人安否情報送信失敗で送信履歴登録
                    foreach (var item in m_PersonSendList)
                    {
                        DbAccess.PersonSendLog log = new DbAccess.PersonSendLog();
                        log.Set(item.info);
                        log.sendresult = "0";
                        log.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        lock (Program.m_objDbAccess)
                        {
                            Program.m_objDbAccess.InsertPersonSendLog(log);
                        }
                    }

                    lock (m_PersonSendList)
                    {
                        m_PersonSendList.Clear();
                    }
                }
                if (m_StateType2_0 == STATE.SENDING)
                {
                    setToolStripStatusLabel(LABEL.APP, "避難所詳細情報送信タイムアウト");
                    // 失敗で送信履歴登録
                    // 避難所詳細情報を送信するときのみ、詳細情報送信ログを出力
                    if (m_isType2subDetail)
                    {
                        foreach (DbAccess.TotalSendLog sendLog in m_TotalSendLog)
                        {
                            SaveTotalSendLog(sendLog, false);
                        }
                        m_TotalSendLog.Clear();
                    }
                }
                if (m_StateType2_1 == STATE.SENDING)
                {
                    setToolStripStatusLabel(LABEL.APP, "避難所名情報送信タイムアウト");
                }
                if (m_StateType0 == STATE.SENDING)
                {
                    setToolStripStatusLabel(LABEL.APP, "端末情報送信タイムアウト");
                }
                if (m_StateType3 == STATE.SENDING)
                {
                    setToolStripStatusLabel(LABEL.APP, "救助支援要求送信タイムアウト");
                }
                // 送信状態リセット
                initSending(true);

                // 活性化状態をリセット
                ShelterStatusView();

#if false
                // RTN応答のタイムアウトで切断する必要はない
                // 通信切断処理
                if (Program.m_ConnectMethod == 2)
                {
                    SubGHz m_SubG = SubGHz.GetInstance();
                    m_SubG.DisconnectSubGHz();
                }
                // タイムアウト or 切断
                EventDisconnect("RTN TIMEOUT");
#endif

                return;
            }

            // デコード
            MsgSBandRtnSendRsp msg3091 = new MsgSBandRtnSendRsp();
            msg3091.encodedData = msgdata;
            msg3091.decode(false);

            // 送信時刻
            int hour = msg3091.rtnSendInfo.sendTime.hour;
            int min = msg3091.rtnSendInfo.sendTime.min;
            int sec = msg3091.rtnSendInfo.sendTime.sec;
            int msec = msg3091.rtnSendInfo.sendTime.msec;

            int dayOfYear = msg3091.rtnSendInfo.sendTime.dayOfYear;
            string sendTimeDecode = dayOfYear + "日 " + hour + "時 " + min + "分 " + sec + "秒 " + msec + "ms";
            Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "RtnThread_Event", "RTN送信応答:", "実際の送信時刻(Q-ANPIターミナル):" + sendTimeDecode);
            Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "RtnThread_SendStart", "RTN送信応答:", "応答内容:" + GetByteArrayString(msgdata));


            // スロットサブスロットの計算
            int slot = m_SlotSubSlot.calcSlot(min, sec, msec);
            int subslot = m_SlotSubSlot.calcSubSlot(min, sec, msec);

            int mt = msg3091.rtnSendInfo.rtnDataMsgType; // メッセージタイプ（2 など）
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "RtnThread_Event", "RTN", "TYPE" + mt + " RTN応答 ");
            int rcode = msg3091.rspResult; // リターンコード（8005 8013 など）

            // 送信成功　code==0
            if (code == 0)
            {
                // type3 は別個に処理
                if (mt == 3)
                {
                    // スロットサブスロット登録
                    m_SlotSubSlot.setRTNReqSlots(3, m_Type1SendingCount, min, sec, msec);

                    // 要求/送信応答/送達確認/受信済みメッセージ/未受信メッセージ数
                    m_Type3SendingCount++;
                    //m_Type3CountMax++;
                    string lblmsg = getLabelRtnCount(m_Type3SendingCount, m_Type3CountMax);
                    setToolStripStatusLabel(LABEL.RTN_RCV, "send:" + lblmsg);
                    if (m_Type3Last == false)
                    {
                        // type130 120秒タイマー開始
                        Task.Factory.StartNew(() =>
                        {
                            m_type130TimerCount = 0;
                            m_isType130timerStop = false;
                            while (true)
                            {
                                // type130を正常に受信出来たらタイマー停止
                                if (m_isType130timerStop)
                                {
                                    break;
                                }

                                // タイムアウトは120秒
                                if (m_type130TimerCount > 120)
                                {
                                    // タイムアウト処理
                                    OnType130Timer();
                                    break;
                                }

                                // type130を正常に受信出来たらタイマー停止
                                if (m_isType130timerReset)
                                {
                                    m_type130TimerCount = 0;
                                    m_isType130timerReset = false;
                                }

                                // 1秒待機
                                Thread.Sleep(1000);
                                m_type130TimerCount++;
                            }
                        });
                    }
                    else
                    {
                        m_isType130timerStop = true;
                        m_StateType3 = STATE.OK;
                    }
                    return;
                }

                // 送信終了後、未送信状態でRTN応答を受けた場合はログのみ表示
                if (Program.m_SendFlag == Program.NOT_SENDING)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "RtnThread_Event", "RTN", "未送信でRTN応答エラー");
                }
                // 個人安否情報
                else if (mt == 1)
                {
                    // slot subslot -> TYPE1 SEQUENCE 
                    // 
                    MsgType1 msg01 = new MsgType1();
                    if (msgdata.Length == 59)
                    {
                        byte[] msgdata2 = new byte[59 - 28];
                        Array.Copy(msgdata, 28, msgdata2, 0, 59 - 28);
                        msg01.encodedData = msgdata2;
                    }
                    else if (msgdata.Length == 44)
                    {
                        byte[] msgdata3 = new byte[44 - 28];
                        Array.Copy(msgdata, 28, msgdata3, 0, 44 - 28);
                        msg01.encodedData = msgdata3;
                    }
                    else
                    {
                        msg01.encodedData = msgdata;
                    }
                    msg01.decode(false);

                    // スロットサブスロット登録
                    m_SlotSubSlot.setRTNReqSlots(1, m_Type1SendingCount, min, sec, msec);

                    long id = msg01.personalId;
                    if (id == m_Rtn_req_id[m_Type1SendingCount])
                    {
                        // OK
                    }
                    else
                    {
                        // RTN応答が順番通りでない
                        Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "RtnThread_Event", "SetPersonData RTN応答 ID err "
                            + id + "!=" + m_Rtn_req_id[m_Type1SendingCount]);
                        EventType1(-1, "RTN REQ", "");
                        return;
                    }

                    m_Type1SendingCount++; // 送信済み（送達確認まだ）カウント
                    m_commonSendingCount++;

                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "RtnThread_Event", "RTN", "TYPE1 RTN応答 ");

                    // 
                    EventType1(code, "RTN REQ", "");
                }

                // 避難所情報
                else if (mt == 2)
                {
                    // slot subslot -> TYPE2 SEQUENCE 
                    // 分割前情報のシーケンス番号
                    MsgType2 msg02 = new MsgType2();
                    msg02.encodedData = msgdata;
                    msg02.decode(false);

                    // 避難所詳細 or 避難所名　判別
                    bool isShelterName = false;
                    byte[] detailData = msg02.encodedData.Skip(28).Take(12).ToArray();

                    // 2進数変換
                    string binStr = "";
                    for (int ic = 0; ic < detailData.Length; ic++)
                    {
                        string tmp = Convert.ToString((int)detailData[ic], 2);
                        if (tmp.Length < 8)
                        {
                            tmp = tmp.PadLeft(8, '0');
                        }
                        binStr += tmp;
                    }
                    // type2メッセージS帯RTNデータの62bit目(避難所種別)が「1」の場合、避難所名登録
                    if (binStr[61] == '1') isShelterName = true;

                    // スロットサブスロット登録処理
                    // シーケンス番号を取得
                    int sequence = Convert.ToInt32(binStr.Substring(16, 5), 2);

                    // 避難所名の場合の処理
                    if (isShelterName)
                    {
                        if (sequence < m_Type2CountMax_1)
                        {
                            m_CheckType2_1[sequence] = TYPE2_SEND_STATE_SEND;

                            // TYPE2 の当該シーケンス番号でスロットサブスロット登録
                            m_SlotSubSlot.setRTNReqSlots(21, sequence, min, sec, msec);

                            m_commonSendingCount++;

                            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "RtnThread_Event", "RTN", "TYPE2_1(避難所名) RTN応答 seq" + sequence);

                            EventType2(code, isShelterName, "RTN REQ", "");
                        }
                    }
                    // 避難所詳細の場合の処理
                    else
                    {
                        if (sequence < m_Type2CountMax_0)
                        {
                            m_CheckType2_0[sequence] = TYPE2_SEND_STATE_SEND;

                            // TYPE2 の当該シーケンス番号でスロットサブスロット登録
                            m_SlotSubSlot.setRTNReqSlots(20, sequence, min, sec, msec);

                            m_commonSendingCount++;

                            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "RtnThread_Event", "RTN", "TYPE2_0(避難所詳細) RTN応答 seq" + sequence);

                            EventType2(code, isShelterName, "RTN REQ", "");
                        }
                    }
                }

                // 開設など
                else if (mt == 0)
                {
                    m_Type0SendingCount++;
                    m_commonSendingCount++;
                    // サブタイプをシーケンス番号に
                    // (type0はsubtype1,subtype2を連続で送信するので、サブタイプの番号が送信済みシーケンスに一致する)
                    int subtype = m_Type0SendingCount;
                    // TYPE0 でスロットサブスロット登録
                    m_SlotSubSlot.setRTNReqSlots(0, subtype, min, sec, msec);
                    EventType0(code, "RTN REQ", "");
                }
                else
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "RtnThread_Event", "RTN", "エラー type:" + mt + " ");
                }
            }
            // RTNのリターンコードでエラーの場合
            else
            {
                // 個人安否情報
                if (mt == 1)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "RtnThread_Event"
                        , "RTN", "TYPE1 RTN応答ERR code " + rcode);
                    Program.m_SendFlag = Program.NOT_SENDING;
                    EventType1(code, "RTN REQ", "");
                    ShelterStatusView();
                }

                // 避難所情報
                else if (mt == 2)
                {
                    // error
                    Program.m_SendFlag = Program.NOT_SENDING;
                    ShelterStatusView();

                    setToolStripStatusLabel(LABEL.APP, "避難所情報送信エラー" + rcode);
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "RtnThread_Event"
                        , "RTN", "TYPE2 RTN応答ERR code " + rcode);
                }
                // 開設など
                else if (mt == 0)
                {
                    // 開設閉鎖NGメッセージを表示
                    setToolStripStatusLabel(LABEL.APP, "端末情報送信エラー" + rcode);
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "RtnThread_Event", "RTN", "TYPE0 RTN応答ERR code " + code);

                    Program.m_SendFlag = Program.NOT_SENDING;
                    ShelterStatusView();
                }
                else if (mt == 3)
                {
                    // 救助支援要求NGメッセージを表示
                    setToolStripStatusLabel(LABEL.APP, "救助支援要求送信エラー" + rcode);
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "RtnThread_Event", "RTN", "TYPE3 RTN応答ERR code " + code);

                    Program.m_SendFlag = Program.NOT_SENDING;
                    ShelterStatusView();

                    initSending(true);
                }
                else
                {
                    setToolStripStatusLabel(LABEL.APP, "送信エラー" + rcode + " type" + mt);
                }
            }
        }

        // 個人安否情報送信 RTN応答時（HttpThread1_Event改変）
        private void EventType1(int code, string msg, string opval)
        {
            m_nSndCnt++;

            // 結果表示
            string lblmsg = getLabelRtnCount(m_commonSendingCount, m_commonCountMax);
            // 個人情報送信中
            setToolStripStatusLabel(LABEL.RTN_RCV, "send:" + lblmsg);

            //2016/04/20 通信失敗の場合は次フレーム処理はしない
            if (code != 0)
            {
                // エラー
                // 送信中状態解除
                Program.m_SendFlag = Program.NOT_SENDING;
                ShelterStatusView();

                if (m_PersonSendList.Count == 0)
                {
                    // すでにエラーでクリアされた
                    Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG,
                        "EventType1", "m_PersonSendList.Count == 0" + msg);
                }
                else
                {
                    // 個人安否情報送信エラーログ
                    DbAccess.PersonSendLog log = new DbAccess.PersonSendLog();
                    log.Set(m_PersonSendList[m_nSndCnt - 1].info);
                    log.sendresult = "0";
                    log.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    Program.m_objDbAccess.InsertPersonSendLog(log);

                    lock (m_PersonSendList)
                    {
                        Program.m_RtnThreadSend.ClearSendQue();
                        m_PersonSendList.Clear();
                    }

                    lock (m_SlotSubSlot)
                    {
                        m_SlotSubSlot.clear();
                    }
                    setToolStripStatusLabel(LABEL.APP, "通信失敗 code:" + code + " msg:" + msg);
                    UpdateListThread();
                }
            }
            else
            {
                // 2016/04/18 送信中対応のための排他処理追加
                lock (m_PersonSendList)
                {
                    // まだ何もしていない
                }

                if (m_Type1SendingCount >= m_Type1CountMax)
                {
                    // 全部送信完了 (送達確認はまだ到着していない)

                    // 送信完了
                    m_nowSending = false;
                    m_nowSendFinNo++;
                }
                else
                {
                    // 送信件数に応答件数が達していないため、タイマーセット
                    Program.m_RtnThreadSend.setTimeOut(RtnThread.RTN_TIMEOUT_SEC);
                }
            }
        }


        // 避難所情報送信 RTN応答時（HttpThread1_Event改変）
        private void EventType2(int code, bool isShelterName, string msg, string opval)
        {
            m_nSndCnt++;

            // 結果表示
            string lblmsg = getLabelRtnCount(m_commonSendingCount, m_commonCountMax);
            setToolStripStatusLabel(LABEL.RTN_RCV, "send:" + lblmsg);

            // エラーの場合、続きの情報を送信してもデータ欠けとなるため送信しない
            if (code != 0)
            {
                // 詳細情報送信の時のみ
                if (m_isType2subDetail)
                {
                    // 避難所詳細情報送信エラーログ
                    foreach (DbAccess.TotalSendLog sendLog in m_TotalSendLog)
                    {
                        SaveTotalSendLog(sendLog, false);
                    }
                    m_TotalSendLog.Clear();
                }

                // 送信中状態解除
                Program.m_SendFlag = Program.NOT_SENDING;
                ShelterStatusView();

                setToolStripStatusLabel(LABEL.APP, "通信失敗 code:" + code + " msg:" + msg);
            }
            else
            {
                // 避難所詳細 or 避難所名で参照するカウンタが違う
                int sendCnt = 0;    // 送信済みカウンタ
                int cntMax = 0;     // 最大分割数

                // 避難所名の場合
                if (isShelterName)
                {
                    sendCnt = m_CheckType2_1.Count(val => !val.Equals(TYPE2_SEND_STATE_DEFAULT));
                    cntMax = m_Type2CountMax_1;
                }
                // 避難所詳細の場合
                else
                {
                    sendCnt = m_CheckType2_0.Count(val => !val.Equals(TYPE2_SEND_STATE_DEFAULT));
                    cntMax = m_Type2CountMax_0;
                }

                // 全部送信応答あった
                if (sendCnt.Equals(cntMax))
                {
                    // 自動送信中の避難所情報送信の場合、次の自動送信メッセージの送信のために一度自動送信フラグをOFFにする
                    if ((m_NowCommand == SendMessageCommand.AUTO_SEND_DETAIL) ||
                        (m_NowCommand == SendMessageCommand.AUTO_SEND_NUM) ||
                        (m_NowCommand == SendMessageCommand.AUTO_SEND_NUM_DETAIL)
                        )
                    {
                        if (m_isType2subDetail)
                        {
                            // 自動送信の時はType2が今何かの判断をsend:の送達確認内で行う
                            m_isType2subDetail = false;

                            // 自動送信で避難所名と避難者数を送信する場合、Type2の後に必ずType0を送信するので、この位置でm_nowAutoSendingをfalseにしない
                            if (m_NowCommand != SendMessageCommand.AUTO_SEND_NUM_DETAIL)
                            {
                                m_nowAutoSending = false;
                            }
                        }
                        else
                        {
                            // Type2の送信確認が来たとき、m_isType2subDetailがtrueでなかったらそれは避難所名送信の送信確認とみなし、
                            // 次に送るType2は詳細情報とする
                            m_isType2subDetail = true;
                        }
                    }

                    // 送信完了
                    m_nowSending = false;
                    m_nowSendFinNo++;
                }
                else
                {
                    // 送信件数に応答件数が達していないため、タイマーセット
                    Program.m_RtnThreadSend.setTimeOut(RtnThread.RTN_TIMEOUT_SEC);
                }
            }
        }

        // 端末情報送信 RTN応答時（HttpThread1_Event改変）
        private void EventType0(int code, string msg, string opval)
        {
            m_nSndCnt++;

            // 結果表示
            //string lblmsg = getLabelRtnCount(m_Type0SendingCount, m_Type0CountMax);
            string lblmsg = getLabelRtnCount(m_commonSendingCount, m_commonCountMax);
            setToolStripStatusLabel(LABEL.RTN_RCV, "send:" + lblmsg);

            // 端末情報送信
            if (code != 0)
            {
                Program.m_SendFlag = Program.NOT_SENDING;
                ShelterStatusView();
                setToolStripStatusLabel(LABEL.APP, "通信失敗 code:" + code + " msg:" + msg);
            }
            else
            {
                if (m_Type0SendingCount >= m_Type0CountMax) // 全部送信応答あった
                {
                    // 自動送信中の避難所情報送信の場合、次の自動送信メッセージの送信のために一度自動送信フラグをOFFにする
                    if (
                        (m_NowCommand == SendMessageCommand.AUTO_SEND_DETAIL) ||
                        (m_NowCommand == SendMessageCommand.AUTO_SEND_NUM) ||
                        (m_NowCommand == SendMessageCommand.AUTO_SEND_NUM_DETAIL)
                        )
                    {
                        m_nowAutoSending = false;
                    }

                    // 送信完了
                    m_nowSending = false;
                    m_nowSendFinNo++;
                }
                else
                {
                    // 送信件数に応答件数が達していないため、タイマーセット
                    Program.m_RtnThreadSend.setTimeOut(RtnThread.RTN_TIMEOUT_SEC);
                }
            }
        }

        public String getLabelRtnCount(int count, int max)
        {
            // 結果表示
            string lblmsg = "";

            // カウント
            lblmsg += count;
            lblmsg += "/";
            lblmsg += max;
            lblmsg += " ";

            return lblmsg;
        }

        // RTN送達確認が来ない
        public void RtnTimeOut(object sender, int type)
        {
            if (type == SlotSubSlot.TYPE0)
            {
                Program.m_SendFlag = Program.NOT_SENDING;
                ShelterStatusView();
                setToolStripStatusLabel(LABEL.APP, "端末情報送達確認タイムアウト");
            }
            else if (type == SlotSubSlot.TYPE1)
            {
                Program.m_SendFlag = Program.NOT_SENDING;
                btnSendTotalization.Enabled = true;
                DbAccess.PersonSendLog log = new DbAccess.PersonSendLog();
                if (m_PersonSendList.Count > 0)
                {
                    for (int ic = m_Type1WaitingCount; ic < m_PersonSendList.Count; ic++)
                    {
                        log.Set(m_PersonSendList[ic].info);
                        log.sendresult = "0";
                        log.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        Program.m_objDbAccess.InsertPersonSendLog(log);
                    }
                }
                lock (m_PersonSendList)
                {
                    m_PersonSendList.Clear();
                }

                UpdateListThread();

                setToolStripStatusLabel(LABEL.APP, "個人安否送達確認タイムアウト");
                initSending(true);
            }
            else if (type == SlotSubSlot.TYPE2)
            {
                // 避難所詳細情報を送信するときのみ、詳細情報送信ログを出力
                if (m_isType2subDetail)
                {
                    foreach (DbAccess.TotalSendLog sendLog in m_TotalSendLog)
                    {
                        SaveTotalSendLog(sendLog, false);
                    }
                    m_TotalSendLog.Clear();
                }

                Program.m_SendFlag = Program.NOT_SENDING;
                ShelterStatusView();

                setToolStripStatusLabel(LABEL.APP, "避難所詳細情報送達確認タイムアウト");
                initSending(true);
            }
            else if (type == SlotSubSlot.TYPE2_0)
            {
                // 避難所詳細情報を送信するときのみ、詳細情報送信ログを出力
                if (m_isType2subDetail)
                {
                    foreach (DbAccess.TotalSendLog sendLog in m_TotalSendLog)
                    {
                        SaveTotalSendLog(sendLog, false);
                    }
                    m_TotalSendLog.Clear();
                }

                Program.m_SendFlag = Program.NOT_SENDING;
                ShelterStatusView();

                setToolStripStatusLabel(LABEL.APP, "避難所名情報送達確認タイムアウト");
                initSending(true);
            }
            else if (type == SlotSubSlot.TYPE2_1)
            {
                Program.m_SendFlag = Program.NOT_SENDING;
                ShelterStatusView();

                setToolStripStatusLabel(LABEL.APP, "避難所情報送達確認タイムアウト");
                initSending(true);
            }
            else if (type == SlotSubSlot.TYPE3)
            {
                setToolStripStatusLabel(LABEL.APP, "救助支援送達確認タイムアウト");
                // もし送信中が解除されていなかったら解除する
                if (Program.m_SendFlag != Program.NOT_SENDING)
                {
                    Program.m_SendFlag = Program.NOT_SENDING;
                }
                if (m_frmReceiveMessage != null)
                {
                    m_frmReceiveMessage.setBtnEnable(true);
                }
                initSending(true);
            }
            else
            {
                // 
                setToolStripStatusLabel(LABEL.APP, "送達確認タイムアウト");
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                    "FormShelterInfo", "RtnTimeOut", "タイムアウト type:" + type);

                Program.m_SendFlag = Program.NOT_SENDING;
                ShelterStatusView();
                initSending(true);
            }
            m_nowSending = false;
            ShelterStatusView();
        }

        // type 130 タイムアウト

        /// <summary>
        /// Type130(救助支援情報)受信待機タイムアウト
        /// </summary>
        public void OnType130Timer()
        {
            // もし送信中が解除されていなかったら解除する
            if (Program.m_SendFlag != Program.NOT_SENDING)
            {
                Program.m_SendFlag = Program.NOT_SENDING;
            }

            if (m_frmReceiveMessage != null)
            {
                m_frmReceiveMessage.setBtnEnable(true);
            }
            setToolStripStatusLabel(LABEL.APP, "救助支援情報 メッセージ取得タイムアウト");
            //m_waitingType130 = false;
            //type130timer.Stop();
            m_isType130timerStop = true;
            m_StateType3 = STATE.OK;

            // 活性化状態を更新
            ShelterStatusView();
        }


        //----------------------------------------------------------------------------------------------
        // FWD
        //----------------------------------------------------------------------------------------------

        // FWD要求に対する応答が来た
        public void FwdThread_Event(object sender, int code, byte[] msgdata, string msg2)
        {
            m_rcvState |= RCV_STATE.CONNECT_FWD;

            // サイズチェック
            if (msgdata.Length == 0)
            {
                // 切断
                m_rcvState ^= RCV_STATE.CONNECT_FWD;
                EventDisconnect(msg2);
                return;
            }

            if (msgdata.Length < 430)
            {
                // ほかのデータサイズ err
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                    "FwdThread_Event", "FWD", "Data Size " + msgdata.Length);

                // FWD要求応答装置固有結果コード
                if (msgdata.Length == 20)
                {
                    String rspmsg = "FWD取得要求:";
                    DecodeManager dm = new DecodeManager();
                    int fwdrsp = dm.decodeInt(msgdata, (2 * QAnpiCtrlLib.consts.EncDecConst.BYTE_BIT_SIZE), (14 * QAnpiCtrlLib.consts.EncDecConst.BYTE_BIT_SIZE));
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FwdThread_Event", "FWDGetReqRsp", "ID" + fwdrsp);
                    // FWD応答がGPS時刻未取得(7010)だった場合
                    if (fwdrsp == 7010)
                    {
                        // S帯FWDデータ取得要求受信時、GPS時刻未取得のためS帯FWDデータ取得応答が送信できない
                        rspmsg += "GPS時刻未取得のため接続不可(" + fwdrsp + ")";
                        m_rcvState ^= RCV_STATE.CONNECT_FWD;
                        EventDisconnect(rspmsg);
                    }
                    // FWD応答がS帯キャリア送受信停止中(7011)だった場合
                    else if (fwdrsp == 7011)
                    {
                        rspmsg += "S帯キャリア送受信停止中(" + fwdrsp + ")";
                    }
                    // FWD応答がその他(7100)だった場合
                    else if (fwdrsp == 7100)
                    {
                        rspmsg += "その他の要因(" + fwdrsp + ")";
                    }
                    else
                    {
                        // N/A
                    }
                    setToolStripStatusLabel(LABEL.FWD, rspmsg);
                    return;
                }
            }

            // FWDメッセージ状態表示
            string toolmsg = "" + msg2
                + " MR:" + Program.m_FwdRecv.m_sysInfo.sysSendRestriction
                + " TSG:" + Program.m_FwdRecv.m_sysInfo.sysGroupNum
                + " RSTSG:" + Program.m_FwdRecv.m_sysInfo.sysRandomSelectBand;
            setToolStripStatusLabel(LABEL.FWD, toolmsg);

            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS,
                    "FormShelterInfo", "FwdThread_Event", toolmsg + " FTS:" + Program.m_FwdRecv.m_sysInfo.sysFwdTimeSlot);

            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS,
                "FormShelterInfo", "FwdThread_Event SystemInfo",
                "x:" + Program.m_FwdRecv.m_sysInfo.sysSatellitePosX
                + " y:" + Program.m_FwdRecv.m_sysInfo.sysSatellitePosY
                + " z:" + Program.m_FwdRecv.m_sysInfo.sysSatellitePosZ
                + " dt:" + Program.m_FwdRecv.m_sysInfo.sysDelayTime
                + " bt:" + Program.m_FwdRecv.m_sysInfo.sysBaseTime
                + " stId:" + Program.m_FwdRecv.m_sysInfo.sysStartFreqId
                + " edId:" + Program.m_FwdRecv.m_sysInfo.sysEndFreqId
                );


            // デコード
            MsgSBandFwdGetRsp msg3090 = new MsgSBandFwdGetRsp();
            msg3090.encodedData = msgdata;
            try
            {
                int retSysInfo = Program.m_FwdRecv.m_sysInfo.checkParam();
                if (retSysInfo == QAnpiCtrlLib.consts.EncDecConst.ENC_VAL_NG)
                {
                    // SystemInfo 範囲チェックエラー
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                                "FwdThread_Event", "FWD", "err：SystemInfo Param ERROR");
                }
                msg3090.decode(false);
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                                "FwdThread_Event", "FWD", "err " + ex.Message);
            }

            // Type 100
            if (msg3090.fwdRcvInfo.fwdDataMsgType == 100)
            {
                // 送達確認
                // 37bit * 88
                byte[] databu = msg3090.fwdRcvInfo.fwdData; // 430バイト固定
                try
                {
                    checkType100(databu);
                }
                catch (Exception e)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                    "FwdThread_Event", "checkType100(databu)", "エラー" + e.Message);
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                    "FwdThread_Event", e.StackTrace);
                }
                // sdata[x]は37ビットの文字列
            }
            // Type 130
            else if (msg3090.fwdRcvInfo.fwdDataMsgType == 130)
            {
                // 救助支援情報
                // 未受信まだあればtype3送信（メッセージ欄に表示）
                try
                {
                    bool dohoFlg = false;
                    string[] recvedIDs = Program.m_objDbAccess.submitType130(msg3090.fwdRcvInfo.fwdData, out dohoFlg);
                    if (recvedIDs != null)
                    {
                        m_Type3RecvMsgCount += recvedIDs.Length;
                        string lblmsg = getLabelRtnCount(m_Type3WaitingCount, m_Type3CountMax);

                        MsgType130 msg130 = new MsgType130();
                        msg130.encodedData = msg3090.fwdRcvInfo.fwdData;
                        msg130.decode(false, false);

                        string sSIR = "11"; // 支援情報要求
                        int infonum = msg130.rescueSupportInfoNum;
                        for (int i = 0; i < infonum; i++)
                        {
                            int cid = msg130.rescueSupportInfos[i].cid;
                            int unreceivedMsgNum = msg130.rescueSupportInfos[i].unreceivedMsgNum;

                            // 自分宛のメッセージの未受信件数を保持する（ログ用）
                            if (cid == Program.m_EquStat.mCID)
                            {
                                m_Type3NotRecvMsgCount = unreceivedMsgNum;
                            }

                            lblmsg += " 受信中:(" + recvedIDs.Length + ") 未受信:(" + m_Type3NotRecvMsgCount + ") 受信合計:(" + m_Type3RecvMsgCount + ")";
                            setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);

                            // 未受信数＝受信数
                            if (cid == Program.m_EquStat.mCID && unreceivedMsgNum == recvedIDs.Length)
                            {
                                // すべて受信終了
                                sSIR = "00";
                                break;
                            }

                            // 未受信数＝０
                            if (cid == Program.m_EquStat.mCID && unreceivedMsgNum == 0)
                            {
                                // 何も送らず終了
                                this.setToolStripStatusLabel(LABEL.APP, "救助支援情報 新着メッセージなし");
                                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FormShelterInfo", "FWD", "Type130 未受信数０");
                                if (m_frmReceiveMessage != null)
                                {
                                    m_frmReceiveMessage.setBtnEnable(true);
                                }
                                // もし送信中が解除されていなかったら解除する
                                if (Program.m_SendFlag != Program.NOT_SENDING)
                                {
                                    Program.m_SendFlag = Program.NOT_SENDING;
                                }

                                // type130待機タイマー停止
                                m_isType130timerStop = true;
                                ShelterStatusView();
                                return;
                            }

                            // 未受信数＜受信数　エラー
                            if (cid == Program.m_EquStat.mCID && unreceivedMsgNum < recvedIDs.Length)
                            {
                                this.setToolStripStatusLabel(LABEL.APP, "救助支援情報 未受信数エラー");
                                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "FWD err", "Type130 未受信数＜受信数");
                                if (m_frmReceiveMessage != null)
                                {
                                    m_frmReceiveMessage.setBtnEnable(true);
                                }
                                // もし送信中が解除されていなかったら解除する
                                if (Program.m_SendFlag != Program.NOT_SENDING)
                                {
                                    Program.m_SendFlag = Program.NOT_SENDING;
                                }

                                // type130ループ解除
                                m_isType130timerStop = true;
                                ShelterStatusView();
                                return;
                            }
                        }
                        // 未読ありフラグ
                        int inum = recvedIDs.Length;
                        string idnum = Convert.ToString(inum, 2);
                        idnum = idnum.PadLeft(2, '0');          // 救助支援ID数 2bit
                        string[] ids = new string[3];

                        for (int i = 0; i < 3; i++)
                        {
                            if (i >= inum)
                            {
                                ids[i] = "000000";
                            }
                            else
                            {
                                ids[i] = recvedIDs[i];
                            }
                        }

                        // Type 3 応答、問い合わせ
                        string sndVal = MsgConv.ConvType3ToSendString(idnum, ids[0], ids[1], ids[2], sSIR);

                        Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FormShelterInfo", "FWD"
                            , "Type3 自動送信 idnum:" + idnum + " id0:" + ids[0] + " id1:" + ids[1] + " id2:" + ids[2]
                            + " SIR:" + sSIR);

                        setToolStripStatusLabel(LABEL.APP, "救助支援情報 メッセージ受信中");
                        if (m_first130Get)
                        {
                            int allRcvCountMax = (m_Type3NotRecvMsgCount - recvedIDs.Length) + (m_Type3RecvMsgCount - recvedIDs.Length) + recvedIDs.Length;
                            m_Type3CountMax = allRcvCountMax / 3;
                            if ((allRcvCountMax % 3) != 0) m_Type3CountMax++;
                            m_Type3CountMax++;
                            m_first130Get = false;
                        }
                        //m_Type3CountMax++;
                        lblmsg = getLabelRtnCount(m_Type3SendStartCount, m_Type3CountMax);
                        setToolStripStatusLabel(LABEL.RTN_SND, "req:" + lblmsg);
                        lblmsg = getLabelRtnCount(m_Type3SendingCount, m_Type3CountMax);
                        setToolStripStatusLabel(LABEL.RTN_RCV, "send:" + lblmsg);
                        lblmsg = getLabelRtnCount(m_Type3WaitingCount, m_Type3CountMax);
                        //setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);
                        lblmsg += " 受信中:(" + recvedIDs.Length + ") 未受信:(" + m_Type3NotRecvMsgCount + ") 受信合計:(" + m_Type3RecvMsgCount + ")";
                        setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);

                        // byte配列へ
                        byte[] binary = MsgConv.ConvStringToBytes(sndVal);

                        // 130タイムアウトカウントをリセット
                        m_type130TimerCount = 0;

                        // 送信中フラグをONにし、送信処理
                        m_nowSending = true;

                        m_isType130timerReset = true;

                        Program.m_RtnThreadSend.AddSendQue(binary);

                        // aSIR=="00"なら最終、送達確認で無事終了
                        if (sSIR == "00")
                        {
                            m_Type3Last = true;
                        }
                        else
                        {
                            m_Type3Last = false;
                        }
                    }
                    else
                    {
                        if (dohoFlg)
                        {
                            // 同報配信受信
                            this.setToolStripStatusLabel(LABEL.APP, "救助支援情報 同報配信受信完了");
                            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FormShelterInfo", "FWD", "Type130 同報配信受信");
                        }
                        else
                        {
                            // 自分宛も同報もなし
                            this.setToolStripStatusLabel(LABEL.APP, "救助支援情報 新着メッセージなし");
                            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FormShelterInfo", "FWD", "Type130 メッセージ無し or メッセージ読み込みエラー（要ログチェック）");
                        }

                        // 何も送らず終了
                        if (m_frmReceiveMessage != null)
                        {
                            m_frmReceiveMessage.setBtnEnable(true);
                        }

                        // 同報配信でない場合、最後に送信中を解除する
                        if (!dohoFlg)
                        {
                            // もし送信中が解除されていなかったら解除する
                            if (Program.m_SendFlag != Program.NOT_SENDING)
                            {
                                Program.m_SendFlag = Program.NOT_SENDING;
                            }
                        }

                        // type130待機タイマー停止
                        m_isType130timerStop = true;

                        ShelterStatusView();
                        return;
                    }
                }
                catch (Exception e)
                {
                    string test = e.Message;
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FwdThread_Event", "TYPE130", test);
                }
            }
            else
            {
                // N/A
            }
        }

        // Type100 送信確認中→送信完了
        private void checkType100(byte[] bits)
        {
            byte num = bits[18];

            // 端末IDから CID に変換 
            // BF 基本周波数ID BC 基本PN符号（0-399）
            string strTermID = Program.m_EquStat.mQCID;

            int cid = Qcid.convCID(strTermID);

            MsgType100 msg100 = new MsgType100();
            msg100.encodedData = bits;
            msg100.decode(false, false);

#if true // 確認用ログ
            string oplog = "";
            if (msg100.ackNum > 0)
            {
                // for (int i = 0; i < msg100.ackNum; i++)
                for (int i = 0; i < 1; i++)
                {
                    if (msg100.acks[i].cid != 0)
                    {
                        oplog += " cid[" + i + "]=" + msg100.acks[i].cid;

                        int slot = msg100.acks[i].slotNum;
                        int subslotbits = msg100.acks[i].subslotBitmap;

                        oplog += " slot=" + slot + " sub=" + subslotbits;
                    }
                }

                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FormShelterInfo", "checkType100",
                    "送達確認数:" + msg100.ackNum + oplog);
            }

#endif

            // 送達確認のなかに自cidがあるか
            int[] slots = new int[88];
            string[] subslots = new string[88];
            int count = 0;
            for (int i = 0; i < msg100.ackNum; i++)
            {
                if (msg100.acks[i].cid == cid)
                {
                    // あった
                    int slot = msg100.acks[i].slotNum;
                    int subslotbits = msg100.acks[i].subslotBitmap;
                    string strsubslotbits = m_SlotSubSlot.getSubSlotString(subslotbits);
                    m_SlotSubSlot.setFWDRspSubSlotON(slot, subslotbits);

                    slots[count] = slot;
                    subslots[count] = strsubslotbits;
                    count++;

                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FormShelterInfo", "checkType100",
                        "送達確認 CID[" + i + "]:" + cid + " スロット:" + slot + " サブスロット:" + strsubslotbits);
                }
            }

            if (count == 0)
            {
                // なかった
                return;
            }

            // Slotクラスに登録
            //string strslots = m_SlotSubSlot.setFWDRspSlots(bits);
            //if (strslots == null)
            //{
            //    // なし
            //    return;
            //}

            // 送達確認が来ている
            for (int i = 0; i < count; i++)
            {
                int slot = slots[i];
                if (slot > 2 || subslots[i] == null || subslots[i].Length != 10)
                {
                    // err or testdata
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                    "FwdThread_Event", "checkType100", "スロットサブスロット不正");
                    continue;
                }

                // 来た送達確認のサブスロットすべてについて
                for (int j = 0; j < subslots[i].Length; j++)
                {
                    int subslot = j;

                    // サブスロットのビットが１
                    if ("1".Equals(subslots[i].Substring(j, 1)))
                    {
                        // 来た送達確認のslot subslotからタイプを
                        int type = m_SlotSubSlot.getMsgType(slot, subslot);
                        string dbgmsg = "(" + slot + "," + subslot + ") Type " + type;
                        Program.m_thLog.PutLog(LogThread.P_LOG_TRANS,
                                "FwdThread_Event", "checkType100", "送達確認" + dbgmsg);

                        m_respCount++;
                        Program.m_thLog.PutLog(LogThread.P_LOG_TRANS,
                                "テストログ", "送達確認チェック", "何かの送達確認が来た：" + m_respCount.ToString() +
                                " Type" + type.ToString() +
                                " Slot:" + slot +
                                " SubSlot:" + subslot
                                );
                        // 個人安否
                        if (type == 1)
                        {
                            // シーケンス番号
                            int seq = m_SlotSubSlot.getType1Seq(slot, subslot);

                            // どの個人安否情報か特定
                            if (seq < 0)
                            {
                                // エラー
                                Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                                    "FwdThread_Event", "checkType100", "Type1 送達確認 シーケンス番号エラー");
                            }
                            else if (m_PersonSendList.Count <= seq)
                            {
                                // エラー
                                Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                                    "FwdThread_Event", "checkType100", "Type1 送達確認 個人安否情報なしエラー");
                                if (m_PersonSendList.Count == 0)
                                {
                                    // 送信中にクリアされた
                                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS,
                                        "FwdThread_Event", "checkType100", "m_PersonSendList.Count == 0");
                                }
                            }
                            else
                            {
                                DbAccess.PersonInfo info = m_PersonSendList[seq].info;

                                // if(m_sendingType2[seq] == 1 → 送信した
                                m_CheckType1[seq] = 2; // 送達確認が来た 2
                                m_SlotSubSlot.clearOne(slot, subslot);

                                //m_Type1WaitingCount++;
                                //m_commonWaitingCount++;

                                // 送信待ち状態OFF
                                {
                                    SendData data = new SendData();
                                    data.bEdit = m_PersonSendList[seq].bEdit;
                                    data.bDelete = m_PersonSendList[seq].bDelete;
                                    data.bSendWait = false;
                                    data.info = m_PersonSendList[seq].info;
                                    m_PersonSendList[seq] = data;
                                }

                                info.send_datetime = m_SendDate.ToString("yyyy/MM/dd HH:mm:ss");
                                // 削除されていなければデータを更新
                                if (!m_PersonSendList[seq].bDelete)
                                {
                                    Program.m_objDbAccess.UpdateSendDateTimePersonInfo(info.id, info.name, info);
                                    // 画面用データも未送信から送信済へ
                                    m_sendPersonInfoList[seq] = info;
                                }

                                DbAccess.PersonSendLog log = new DbAccess.PersonSendLog();
                                log.Set(m_PersonSendList[seq].info);
                                log.sendresult = "1";
                                log.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                Program.m_objDbAccess.InsertPersonSendLog(log);

                                if (m_bProcStop == false)
                                {
                                    // 行データの値を設定
                                    string[] strItems = new string[14];

                                    // 一時配列に格納(ListViewのデータを直接参照すると動作が遅いので)
                                    List<string[]> personItem = new List<string[]>();
                                    for (int ic = 0; ic < listPersonal.Items.Count; ic++)
                                    {
                                        personItem.Add(new string[14]);
                                        for (int kc = 0; kc < 14; kc++)
                                        {
                                            personItem[ic][kc] = listPersonal.Items[ic].SubItems[kc].Text;
                                        }
                                    }

                                    // person_infoをstring配列に変換
                                    strItems = ConvPersonInfoToStringList(info);
                                    for (int nIdx = 0; nIdx < m_sendPersonInfoList.Count; nIdx++)
                                    {
                                        if (m_sendPersonInfoList[nIdx] == m_PersonSendList[seq].info)
                                        {
                                            for (int ic = 0; ic < personItem.Count; ic++)
                                            {
                                                // 個人安否情報リストの氏名・電話番号と一致するものを探し、送信された行を検索
                                                if ((personItem[ic][2] == m_sendPersonInfoList[nIdx].name) &&
                                                    (personItem[ic][3] == m_sendPersonInfoList[nIdx].id))
                                                {
                                                    listPersonal.Items[ic].SubItems[1].Text = strItems[1]; // 送信ごとに更新
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }

                                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS,
                                    "FwdThread_Event", "checkType100", "Type1 送達確認 seq" + seq);
                            }
                        }

                        // 避難所詳細
                        else if (type == 20)
                        {
                            // シーケンス番号確認（例：3 = 三番目に送ったやつ）
                            int seq = m_SlotSubSlot.getType2Seq(slot, subslot);
                            // 
                            // if(m_sendingType2[seq] == 1 → 送信した
                            m_CheckType2_0[seq] = TYPE2_SEND_STATE_RESP; // 送達確認が来た 2
                            m_SlotSubSlot.clearOne(slot, subslot);
                            m_Type2WaitingCount_0++;
                            m_commonWaitingCount++;

                            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS,
                                "FwdThread_Event", "checkType100", "Type2_0()避難所詳細 送達確認 seq" + seq);
                        }
                        // 避難所名
                        else if (type == 21)
                        {
                            // シーケンス番号確認（例：3 = 三番目に送ったやつ）
                            int seq = m_SlotSubSlot.getType2Seq(slot, subslot);
                            // 
                            // if(m_sendingType2[seq] == 1 → 送信した
                            m_CheckType2_1[seq] = TYPE2_SEND_STATE_RESP; // 送達確認が来た 2
                            m_SlotSubSlot.clearOne(slot, subslot);
                            m_Type2WaitingCount_1++;
                            m_commonWaitingCount++;

                            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS,
                                "FwdThread_Event", "checkType100", "Type2_1(避難所名) 送達確認 seq" + seq);
                        }

                        // 開設閉鎖端末情報送信通知
                        else if (type == 0)
                        {
                            // サブタイプ別に送達確認を
                            int seq = m_SlotSubSlot.getType0Seq(slot, subslot);
                            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS,
                                "FwdThread_Event", "checkType100", "Type0 送達確認 seq" + seq);


                            // サブタイプ1の送達確認が来た
                            if (seq == 1)
                            {
                                m_Type0SubType1ok = 1;
                                m_Type0WaitingCount++;
                                m_commonWaitingCount++;
                                // 送達確認受信済み数表示
                                //string lblmsg = getLabelRtnCount(m_Type0WaitingCount, m_Type0CountMax);
                                string lblmsg = getLabelRtnCount(m_commonWaitingCount, m_commonCountMax);
                                this.setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);
                            }
                            // サブタイプ2がきて初めて更新（開/閉鎖情報）
                            else if (seq == 2 && m_Type0SubType1ok == 1)
                            {
                                m_Type0SubType1ok = 0;

                                // 避難所名送信の時、かつコマンドが「編集」の時、画面を更新(編集した避難所名を適用)
                                if ((!m_isType2subDetail) &&
                                    ((m_NowCommand == SendMessageCommand.EDIT_SHELTER) || (m_NowCommand == SendMessageCommand.EDIT_SHELTER_NO_POSITION))
                                    )
                                {
                                    if (m_OpenCloseShelterInfo.sid != "")
                                    {
                                        // 編集ダイアログのでの情報を取得
                                        m_ActiveTarminal_info = m_OpenCloseShelterInfo;

                                        // DBの名前情報を更新
                                        Program.m_objDbAccess.UpsertTerminalInfo(m_ActiveTarminal_info);
                                        m_OpenCloseShelterInfo = new DbAccess.TerminalInfo();

                                        // メイン画面の表示を更新
                                        UpdateShelterInfoList();
                                        updateActiveShelterInfo();

                                        // 画面更新処理
                                        UpdateDisplay();
                                    }
                                }
                                else if (
                                    (m_NowCommand == SendMessageCommand.AUTO_SEND_DETAIL) ||
                                    (m_NowCommand == SendMessageCommand.AUTO_SEND_NUM) ||
                                    (m_NowCommand == SendMessageCommand.AUTO_SEND_NUM_DETAIL)
                                    )
                                {
                                    // 自動送信の場合は何もしない
                                }
                                else
                                {
                                    // 開設時処理
                                    m_ActiveTarminal_info = m_OpenCloseShelterInfo;
                                    if (m_ActiveTarminal_info.open_flag == FormShelterInfo.SHELTER_STATUS.OPEN)
                                    {
                                        // 開設時、画面の表示を変更する
                                        //避難所ID
                                        txtShelterID.Text = m_ActiveTarminal_info.gid;
                                        //避難所名
                                        txtShelterName.Text = m_ActiveTarminal_info.name;
                                        //緯度経度
                                        txtShelterPos.Text = m_ActiveTarminal_info.lat + "," + m_ActiveTarminal_info.lon;
                                    }

                                    m_ShelterStat = m_ActiveTarminal_info.open_flag;

                                    // DBの避難所データを更新
                                    Program.m_objDbAccess.UpsertTerminalInfo(m_ActiveTarminal_info);
                                    // 開設した避難所をアクティブ状態にする
                                    SetActiveShelterInfo(selectShelterName.SelectedIndex);
                                    // 避難所情報を更新
                                    UpdateShelterInfoList();
                                }

                                m_Type0WaitingCount++;
                                m_commonWaitingCount++;
                                // 送達確認受信済み数表示
                                //string lblmsg = getLabelRtnCount(m_Type0WaitingCount, m_Type0CountMax);
                                string lblmsg = getLabelRtnCount(m_commonWaitingCount, m_commonCountMax);
                                this.setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);


                                // 自動送信中は送信待ちフラグを確認する
                                // 自動送信中の避難所情報送信の場合、次の自動送信メッセージの送信のために一度自動送信フラグをOFFにする
                                if (
                                    (m_NowCommand == SendMessageCommand.AUTO_SEND_DETAIL) ||
                                    (m_NowCommand == SendMessageCommand.AUTO_SEND_NUM) ||
                                    (m_NowCommand == SendMessageCommand.AUTO_SEND_NUM_DETAIL)
                                    )
                                {

                                    // 送信待機中のメッセージが存在しない場合は送信中状態を解除する
                                    if (m_nowWaitNo == m_nowSendFinNo)
                                    {
                                        Program.m_SendFlag = Program.NOT_SENDING;
                                    }
                                    // まだ送信待機中のメッセージが存在するなら送信状態を継続する
                                    else
                                    {
                                        Program.m_SendFlag = Program.SENDING_SHELTER_INFO;
                                    }
                                }
                                else
                                {
                                    // 送信中状態を解除する
                                    Program.m_SendFlag = Program.NOT_SENDING;
                                }

                                m_StateType0 = STATE.OK;
                                if (Program.m_SendFlag == Program.NOT_SENDING) setToolStripStatusLabel(LABEL.APP, "避難所情報送信完了 ");
                                ShelterStatusView();
                            }
                            // 端末情報送信中にType0 サブタイプ2が先に来た場合や
                            // 避難所情報送信中にType0 サブタイプ1が来た場合などにわかるように
                            else
                            {
                                // ログ出力
                                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS,
                                "FwdThread_Event", "checkType100", " Type0 m_Type0SubType1ok:" + m_Type0SubType1ok);
                            }

                            // 送達確認待ち消しこみ
                            m_SlotSubSlot.clearOne(slot, subslot);
                        }
                        else if (type == 3)
                        {
                            m_Type3WaitingCount++;
                            string lblmsg = getLabelRtnCount(m_Type3WaitingCount, m_Type3CountMax);
                            setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);

                            // 最終データなら終了
                            if (m_Type3Last == true)
                            {
                                setToolStripStatusLabel(LABEL.APP, "救助支援情報 メッセージ受信完了 ");
                                // もし送信中が解除されていなかったら解除する
                                if (Program.m_SendFlag != Program.NOT_SENDING)
                                {
                                    Program.m_SendFlag = Program.NOT_SENDING;
                                }
                                if (m_frmReceiveMessage != null)
                                {
                                    m_frmReceiveMessage.setBtnEnable(true);
                                }

                                // type3メッセージ最大数を初期化
                                m_Type3CountMax = 1;

                                // type130ループ解除
                                m_isType130timerStop = true;

                                m_StateType3 = STATE.OK;
                            }
                            else
                            {
                                // 最終データでない場合はさらにメッセージの受信が行われるので、送信中を解除しない
                                setToolStripStatusLabel(LABEL.APP, "救助支援情報 メッセージ受信中 ");
                            }

                            // 送達確認待ち消しこみ
                            m_SlotSubSlot.clearOne(slot, subslot);
                            ShelterStatusView();
                        }
                        else
                        {
                            // 
                            Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                                "FwdThread_Event", "checkType100", "送達確認：送ってないのにビットがたってる ");
                        }

                    } // if subslot on

                } // subslot check
            }

            // 連続送信ものについて、送達がそろったかどうか
            if (m_StateType1 == STATE.SENDING)
            {
                bool type1ok = true;
                int count1ok = 0;
                for (int i = 0; i < m_Type1CountMax; i++)
                {
                    if (m_CheckType1[i] != 2) // 全部２で送達確認完了
                    {
                        type1ok = false;
                    }
                    else
                    {
                        count1ok++;
                    }
                }
                m_Type1WaitingCount = count1ok;
                int totalCount = m_commonWaitingCount + m_Type1WaitingCount;

                // 送達確認受信済み数表示
                //string lblmsg = getLabelRtnCount(m_Type1WaitingCount, m_Type1CountMax);
                string lblmsg = getLabelRtnCount(totalCount, m_commonCountMax);
                this.setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);

                if (type1ok == true)
                {
                    // 送信中の場合
                    // 送信完了処理

                    // 送信終了したので送信リストをクリアする
                    lock (m_PersonSendList)
                    {
                        m_PersonSendList.Clear();
                    }

                    // 送信中を解除する
                    Program.m_SendFlag = Program.NOT_SENDING;
                    if (Program.m_SendFlag == Program.NOT_SENDING) setToolStripStatusLabel(LABEL.APP, "個人安否情報送信完了  ");

                    ShelterStatusView();

                    UpdateListThread();

                    m_StateType1 = STATE.OK;
                }
                else
                {
                    // 全部そろってない
                    setToolStripStatusLabel(LABEL.APP, "個人安否情報送信中　  ");
                }
            }

            // 避難所名送信中
            if (m_StateType2_1 == STATE.SENDING)
            {
                int sendCnt = m_CheckType2_1.Count(val => val.Equals(TYPE2_SEND_STATE_RESP));

                string lblmsg = getLabelRtnCount(m_commonWaitingCount, m_commonCountMax);
                this.setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);

                if (sendCnt.Equals(m_Type2CountMax_1))
                {
                    // 最後に送信した名前の避難所を記憶
                    m_lastSendShelterName = m_ActiveTarminal_info;
                    // 送信完了
                    m_StateType2_1 = STATE.OK;

                    // 編集(位置情報送信無し)
                    if (m_NowCommand == SendMessageCommand.EDIT_SHELTER_NO_POSITION)
                    {
                        // 新しい名前にDBを更新
                        if (m_OpenCloseShelterInfo.sid != "")
                        {
                            // 編集ダイアログのでの情報を取得
                            m_ActiveTarminal_info = m_OpenCloseShelterInfo;

                            // DBの名前情報を更新
                            Program.m_objDbAccess.UpsertTerminalInfo(m_ActiveTarminal_info);
                            m_OpenCloseShelterInfo = new DbAccess.TerminalInfo();

                            // メイン画面の表示を更新
                            UpdateShelterInfoList();
                            updateActiveShelterInfo();

                            // 画面更新処理
                            UpdateDisplay();
                        }

                        // 送信中を解除
                        Program.m_SendFlag = Program.NOT_SENDING;
                        ShelterStatusView();
                    }
                    // 自動送信中
                    else if ((m_NowCommand == SendMessageCommand.AUTO_SEND_DETAIL) ||
                        (m_NowCommand == SendMessageCommand.AUTO_SEND_NUM) ||
                        (m_NowCommand == SendMessageCommand.AUTO_SEND_NUM_DETAIL))
                    {
                        // 送信待機中のメッセージが存在しない場合は送信中状態を解除する
                        if (m_nowWaitNo == m_nowSendFinNo)
                        {
                            Program.m_SendFlag = Program.NOT_SENDING;
                        }
                        // まだ送信待機中のメッセージが存在するなら送信状態を継続する
                        else
                        {
                            Program.m_SendFlag = Program.SENDING_SHELTER_INFO;
                        }
                    }

                    // 送信完了処理
                    setToolStripStatusLabel(LABEL.APP, "避難所名情報送信完了  ");
                    ShelterStatusView();
                }
                else
                {
                    setToolStripStatusLabel(LABEL.APP, "避難所名情報送信中　  ");
                }
            }

            // 避難所詳細送信中
            if (m_StateType2_0 == STATE.SENDING)
            {
                int sendCnt = m_CheckType2_0.Count(val => val.Equals(TYPE2_SEND_STATE_RESP));

                string lblmsg = getLabelRtnCount(m_commonWaitingCount, m_commonCountMax);
                this.setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);

                if (sendCnt.Equals(m_Type2CountMax_0))
                {
                    // 送信完了
                    m_StateType2_0 = STATE.OK;

                    // ログ出力
                    foreach (DbAccess.TotalSendLog sendLog in m_TotalSendLog)
                    {
                        // 詳細情報ログリストの一番最初のものを最初に送信した詳細情報のログとして出力
                        SaveTotalSendLog(sendLog, true);
                        m_TotalSendLog.Remove(sendLog);
                        break;
                    }

                    if ((m_NowCommand == SendMessageCommand.AUTO_SEND_DETAIL) ||
                        (m_NowCommand == SendMessageCommand.AUTO_SEND_NUM) ||
                        (m_NowCommand == SendMessageCommand.AUTO_SEND_NUM_DETAIL))
                    {
                        // 送信待機中のメッセージが存在しない場合は送信中状態を解除する
                        if (m_nowWaitNo == m_nowSendFinNo)
                        {
                            Program.m_SendFlag = Program.NOT_SENDING;
                        }
                        // まだ送信待機中のメッセージが存在するなら送信状態を継続する
                        else
                        {
                            Program.m_SendFlag = Program.SENDING_SHELTER_INFO;
                        }
                    }
                    // 避難所詳細情報送信
                    else if (m_NowCommand == SendMessageCommand.SEND_SHELTER_INFO)
                    {
                        Program.m_SendFlag = Program.NOT_SENDING;
                    }

                    // 送信完了処理
                    setToolStripStatusLabel(LABEL.APP, "避難所詳細情報送信完了  ");
                    ShelterStatusView();
                }
                else
                {
                    setToolStripStatusLabel(LABEL.APP, "避難所詳細情報送信中　  ");
                }
            }
        }

        // 選択行取得
        private void GetSelRowItemNo()
        {
            if (listPersonal.Items.Count > 0)
            {
                if (listPersonal.SelectedItems.Count > 0)
                {
                    m_SelectRow = listPersonal.SelectedItems[0].Index;

                    //表示最終行を取得
                    int oldTop = listPersonal.TopItem.Index;
                    m_ShowLastRow = oldTop + 13;
                    if (m_ShowLastRow >= listPersonal.Items.Count)
                    {
                        m_ShowLastRow = listPersonal.Items.Count - 1;
                    }
                }
            }
            else
            {
                m_SelectRow = 0;
                m_ShowLastRow = 0;
            }

        }

        // 選択行
        private void SetSelRowItemNo()
        {

            if (m_SelectRow >= listPersonal.Items.Count)
            {
                m_SelectRow = listPersonal.Items.Count - 1;
            }

            if (m_ShowLastRow >= listPersonal.Items.Count)
            {
                m_ShowLastRow = listPersonal.Items.Count - 1;
            }

            if (listPersonal.Items.Count > 0)
            {
                listPersonal.Items[m_SelectRow].Selected = true;
                //listPersonal.EnsureVisible(m_ShowLastRow);
                // リストの表示は最上部(最新)を取得するように変更
                listPersonal.EnsureVisible(0);

            }

        }

        private void listPersonal_MouseClick(object sender, MouseEventArgs e)
        {
            GetSelRowItemNo();
        }


        /**
         * @brief 受信情報メニュー押下時
         */
        private void ReceiveMenuItem_Click(object sender, EventArgs e)
        {
            // 現在未使用
            /*
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "ReceiveMenuItem_Click", "受信情報 クリック");
            
            if (m_frmReceiveMessage == null)
            {
                m_frmReceiveMessage = new FormReceiveMessageView();
            }
            m_frmReceiveMessage.EventRtnRescue += new FormReceiveMessageView.EventRtnRescueDelegate(RtnRescue_Event);
            m_frmReceiveMessage.StartPosition = FormStartPosition.CenterParent;
            m_frmReceiveMessage.ShowDialog();
            m_frmReceiveMessage.EventRtnRescue -= new FormReceiveMessageView.EventRtnRescueDelegate(RtnRescue_Event);
             * */
        }

        /**
         * @brief 新着確認開始押下時
         */
        private void RtnRescue_Event(object sender, string outVal)
        {
            // 共通送信数の初期化
            initCommonSendingBuff();
            initSending(true);

            // 実行コマンドをセット
            m_NowCommand = SendMessageCommand.CHECK_NEW_MESSAGE;

            Program.m_SendFlag = 3;         // 0:送信中でない  1:個人安否送信  2:避難所情報送信　3:開設/閉鎖/端末情報送信
            ShelterStatusView();

            m_StateType3 = STATE.SENDING;
            m_Type3Last = false;
            m_Type3SendStartCount = 0;
            m_Type3SendingCount = 0;
            m_Type3WaitingCount = 0;
            m_Type3RecvMsgCount = 0;
            m_Type3NotRecvMsgCount = 0;


            string lblmsg = getLabelRtnCount(m_Type3SendStartCount, m_Type3CountMax);
            setToolStripStatusLabel(LABEL.RTN_SND, "req:" + lblmsg);
            lblmsg = getLabelRtnCount(m_Type3SendingCount, m_Type3CountMax);
            setToolStripStatusLabel(LABEL.RTN_RCV, "send:" + lblmsg);
            lblmsg = getLabelRtnCount(m_Type3WaitingCount, m_Type3CountMax);
            setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);

            setToolStripStatusLabel(LABEL.APP, outVal);

            // Type 3 問い合わせ
            string sndVal = MsgConv.ConvType3ToSendString();
            // byte配列へ
            byte[] binary = MsgConv.ConvStringToBytes(sndVal);

            // 送信中フラグをONにし、送信処理
            m_nowSending = true;

            Program.m_RtnThreadSend.AddSendQue(binary);

            setToolStripStatusLabel(LABEL.APP, "救助支援情報 新着確認開始 ");

        }


        /**
         * @brief 装置状態メニュー押下時
         */
        private void TermStatusMenuItem_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "TermStatusMenuItem_Click", "装置状態 クリック");

            if (m_frmTermStatus == null)
            {
                m_frmTermStatus = new FormTermStatus();
            }
            m_frmTermStatus.StartPosition = FormStartPosition.CenterParent;
            m_frmTermStatus.ShowDialog();

            //m_frmTermStatus = null;
        }

        //----------------------------------------------------------------------------------------------
        // インジケータ
        //----------------------------------------------------------------------------------------------
        /**
         * @brief インジケータ定義
         */
        public enum Indicator
        {
            Connection = 0,     /* 端末接続状態 */
            Synchronize,        /* TDMA同期状態 */
            Message,            /* 避難所支援情報 */
            Alert,              /* 災害危機通報情報 */
            TermStatus,         /* 装置ステータス */
        }
        public enum IndStatus
        {
            Disconnect = 0,     /* 未接続状態 */
            Normal,             /* 正常(接続中) */
            NoneMessage,        /* メッセージなし */
            NewMessage,         /* 新着メッセージあり */
            Exception,          /* 異常 */
        }

        /**
         * @brief インジケータ表示切替
         * @param 
         * @param
         * @return
         */
        public void SetIndicator(Indicator ind, IndStatus stat)
        {
            switch (ind)
            {
                case Indicator.Connection:
                    if (stat == IndStatus.Disconnect)
                    {
                        IdcConnect.BackColor = SystemColors.ControlDark;    // 端末未接続時
                    }
                    else if (stat == IndStatus.Normal)
                    {
                        IdcConnect.BackColor = Color.Lime;                  // 端末接続時
                    }
                    else
                    {
                        // N/A
                    }
                    break;
                case Indicator.Synchronize:
                    if (stat == IndStatus.Disconnect)
                    {
                        IdcTdma.BackColor = SystemColors.ControlDark;       // TDMA非同期(未接続)時
                    }
                    else if (stat == IndStatus.Normal)
                    {
                        IdcTdma.BackColor = Color.Lime;                     // TDMA同期時
                    }
                    else
                    {
                        // N/A
                    }
                    break;
                case Indicator.Message:
                    if (stat == IndStatus.Disconnect || stat == IndStatus.NoneMessage)
                    {
                        IdcMessage.BackColor = SystemColors.Control;        // 新着メッセージなし(未接続)時
                    }
                    else if (stat == IndStatus.NewMessage)
                    {
                        IdcMessage.BackColor = Color.RoyalBlue;             // 新着メッセージあり時
                    }
                    else
                    {
                        // N/A
                    }
                    break;
                case Indicator.Alert:
                    if (stat == IndStatus.Disconnect || stat == IndStatus.NoneMessage)
                    {
                        IdcAlart.BackColor = SystemColors.Control;         // 新着メッセージなし(未接続)時
                    }
                    else if (stat == IndStatus.NewMessage)
                    {
                        IdcAlart.BackColor = Color.RoyalBlue;              // 新着メッセージあり時
                    }
                    else
                    {
                        // N/A
                    }
                    break;
                case Indicator.TermStatus:
                    if (stat == IndStatus.Disconnect)
                    {
                        IdcTerm.BackColor = SystemColors.Control;          // 未接続時
                    }
                    else if (stat == IndStatus.Normal)
                    {
                        IdcTerm.BackColor = Color.Lime;                   // 正常時
                    }
                    else if (stat == IndStatus.Exception)
                    {
                        IdcTerm.BackColor = Color.Red;                    // 装置異常時
                    }
                    else
                    {
                        // N/A
                    }
                    break;
                default:
                    break;
            }
        }

        /**
         * @brief 未読メッセージ確認処理
         */
        private void CheckNotRead(object sender, int code, bool flg, string outVal)
        {
            // 災害危機通報
            if (code == 0)
            {
                if (flg)
                {
                    // すべて既読
                    SetIndicator(Indicator.Alert, IndStatus.NoneMessage);
                }
                else
                {
                    // 未読あり
                    SetIndicator(Indicator.Alert, IndStatus.NewMessage);
                }
            }
            // 救助支援情報確認
            else if (code == 1)
            {
                if (flg)
                {
                    // すべて既読
                    SetIndicator(Indicator.Message, IndStatus.NoneMessage);
                }
                else
                {
                    // 未読あり
                    SetIndicator(Indicator.Message, IndStatus.NewMessage);
                }
            }
            else
            {
                // N/A
            }
        }

        public void initCommonSendingBuff()
        {
            m_commonCountMax = 0;
            m_commonSendStartCount = 0;
            m_commonSendingCount = 0;
            m_commonWaitingCount = 0;

            m_isType2subDetail = false;
        }

        /**
         * @brief 送信中エラーの初期化
         */
        public void initSending(bool sending)
        {
            if (sending)
            {
                Program.m_SendFlag = Program.NOT_SENDING;
                ShelterStatusView();
            }

            m_nowSendFinNo = 0;
            m_nowSending = false;
            m_nowWaitNo = 0;
            m_StateType1 = STATE.OK;
            m_Type1SendStartCount = 0;
            m_Type1SendingCount = 0;
            m_Type1WaitingCount = 0;
            m_StateType2_0 = STATE.OK;
            m_Type2SendStartCount_0 = 0;
            m_Type2WaitingCount_0 = 0;
            m_CheckType2_0 = Enumerable.Repeat<int>(TYPE2_SEND_STATE_DEFAULT, m_Type2CountMax_0).ToArray();
            m_StateType2_1 = STATE.OK;
            m_Type2SendStartCount_1 = 0;
            m_Type2WaitingCount_1 = 0;
            m_CheckType2_1 = Enumerable.Repeat<int>(TYPE2_SEND_STATE_DEFAULT, m_Type2CountMax_1).ToArray();
            m_StateType0 = STATE.OK;
            m_Type0SendingCount = 0;
            m_Type0SendStartCount = 0;
            m_Type0SubType1ok = 0;
            m_Type0WaitingCount = 0;
            m_StateType3 = STATE.OK;
            m_Type3Last = false;
            m_Type3SendStartCount = 0;
            m_Type3SendingCount = 0;
            m_Type3WaitingCount = 0;
            m_Type3RecvMsgCount = 0;
            m_Type3NotRecvMsgCount = 0;
            m_SlotSubSlot.clear();

            ShelterStatusView(); // type0 メニュー

            try
            {
                // 一時配列に格納(ListViewのデータを直接参照すると動作が遅いので)
                List<string[]> personItem = new List<string[]>();
                for (int ic = 0; ic < listPersonal.Items.Count; ic++)
                {
                    personItem.Add(new string[14]);
                    for (int kc = 0; kc < 14; kc++)
                    {
                        personItem[ic][kc] = listPersonal.Items[ic].SubItems[kc].Text;
                    }
                }

                // 一致するデータを検索
                string[] strItems = new string[14];
                for (int nIdx = 0; nIdx < m_activePersonInfoList.Count; nIdx++)
                {
                    for (int ic = 0; ic < personItem.Count; ic++)
                    {
                        // 個人安否情報リストの氏名・電話番号と一致する行を検索
                        if ((personItem[ic][2] == m_activePersonInfoList[nIdx].name) &&
                            (personItem[ic][3] == m_activePersonInfoList[nIdx].id))
                        {
                            strItems = ConvPersonInfoToStringList(m_activePersonInfoList[nIdx]);
                            listPersonal.Items[nIdx].SubItems[1].Text = strItems[1];
                            break;
                        }
                    }
                }
            }
            catch
            {

            }

            Program.m_RtnThreadSend.setWaiting(false);
            Program.m_RtnThreadSend.ClearSendQue();

            if (m_frmReceiveMessage != null)
            {
                m_frmReceiveMessage.setBtnEnable(true);
            }

            // 実行コマンドを初期化
            m_NowCommand = SendMessageCommand.COMMAND_NONE;
        }

        /**
         * @brief いずれかのポートの回線が切断されたとき
         */
        private void EventDisconnect(string msg)
        {
            // 送信状態リセット
            initSending(true);
            Program.m_SendFlag = Program.NOT_SENDING;
            ShelterStatusView();

            // タイムアウト設定
            m_SlotSubSlot.clear(); // 切断で送達確認待ちをクリアする
            Program.m_RtnThreadSend.setWaiting(false); // 切断でRTN応答待ちをクリアする
            //m_waitingType130 = false; // 切断で130待ちをクリア
            //type130timer.Stop();
            m_isType130timerStop = true;

            if (m_rcvState == RCV_STATE.DISCONNECT)
            {
                // インジケータの表示切替
                SetIndicator(Indicator.Connection, IndStatus.Disconnect);
                SetIndicator(Indicator.Synchronize, IndStatus.Disconnect);
                SetIndicator(Indicator.TermStatus, IndStatus.Disconnect);
                m_TermStatusInd = IndStatus.Disconnect;

                // 表示中フォームへの反映
                if (m_frmTcpSettings != null)
                {
                    m_frmTcpSettings.eventDisconnecting(m_frmTcpSettings, 1, "回線が切断されました", "");
                }

                if (m_frmSubGhzSettings != null)
                {
                    m_frmSubGhzSettings.eventConnecting(m_frmSubGhzSettings, 1, "回線が切断されました", SubGHz.CONNECTING_DISCONNECT);
                }

                if (m_frmTermStatus != null)
                {
                    m_frmTermStatus.SetDisconnect();
                }

                // 未接続状態に使用可能となる
                if (Program.m_ConnectMethod == 2)
                {
                    ConnectTestMenuItem.Enabled = true;
                }
            }

            // 接続されているものを切断する
            if (Program.m_EquStat.isConnected())
            {
                Program.m_EquStat.disconnect();
            }
            if (Program.m_FwdRecv.isConnected())
            {
                Program.m_FwdRecv.disconnect();
            }
            if (Program.m_L1sRecv.isConnected())
            {
                Program.m_L1sRecv.disconnect();
            }

            setToolStripStatusLabel(LABEL.CONNECT, "接続が切れました (" + msg + ")");

            UpdateDisplay();
        }


        /**
         * @brief:メッセージ受信時に効果音を再生する
         */
        public void RecvMessageSound(object sender, int code, string id, string outVal)
        {
            // メッセージ着信音鳴動
            System.Media.SystemSounds.Asterisk.Play();
        }

        private Object lockToolStrip = new Object();
        public void setToolStripStatusLabel(LABEL label, string msg)
        {
            lock (lockToolStrip)
            {
                string log_msg;
                try
                {
                    switch (label)
                    {
                        case LABEL.APP:
                            lock (toolStripStatusLabel1)
                            {
                                log_msg = "LABEL_APP:" + msg;
                                toolStripStatusLabel1.Text = msg + " | ";
                            }
                            break;
                        case LABEL.RTN_SND:
                            lock (lbl_ReqCnt)
                            {
                                log_msg = "LABEL_RTN_SND:" + msg;
                                lbl_ReqCnt.Text = msg;
                            }
                            break;
                        case LABEL.RTN_RCV:
                            lock (lbl_SndCnt)
                            {
                                log_msg = "LABEL_RTN_RCV:" + msg;
                                lbl_SndCnt.Text = msg;
                            }
                            break;
                        case LABEL.RTN_CHK:
                            lock (lbl_RspCnt)
                            {
                                log_msg = "LABEL_RTN_CHK:" + msg;
                                lbl_RspCnt.Text = msg;
                            }
                            break;
                        case LABEL.FWD:
                            lock (toolStripStatusLabel5)
                            {
                                log_msg = "LABEL_FWD:" + msg;
                                toolStripStatusLabel5.Text = msg + " | ";
                            }
                            break;
                        case LABEL.L1S:
                            lock (toolStripStatusLabel6)
                            {
                                log_msg = "LABEL_L1S:" + msg;
                                toolStripStatusLabel6.Text = msg + " | ";
                            }
                            break;
                        case LABEL.CONNECT:
                            lock (toolStripStatusLabel7)
                            {
                                log_msg = "LABEL_CONNECT:" + msg;
                                toolStripStatusLabel7.Text = msg + " | ";
                            }
                            break;
                        case LABEL.ALL:
                            lock (this)
                            {
                                log_msg = "LABEL_ALL:" + msg;
                                toolStripStatusLabel1.Text = msg;
                                lbl_ReqCnt.Text = msg;
                                lbl_SndCnt.Text = msg;
                                lbl_RspCnt.Text = msg;
                                toolStripStatusLabel5.Text = msg;
                                toolStripStatusLabel6.Text = msg;
                                toolStripStatusLabel7.Text = msg;
                            }
                            break;
                        case LABEL.TIME_SYNC:
                            lock (toolStripStatusLabel8)
                            {
                                log_msg = "LABEL_TUME_SYNC:" + msg;
                                toolStripStatusLabel8.Text = msg + "";
                            }
                            break;
                        default:
                            log_msg = msg;
                            break;
                    }

                    //Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG,"setToolStripStatusLabel", log_msg);

                    // ログにはステータスバーの状態をそのまま出力する
                    string statusText = label5.Text + lbl_ReqCnt.Text + lbl_SndCnt.Text + lbl_RspCnt.Text;
                    Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "setToolStripStatusLabel", statusText);

                }
                catch (Exception ex)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                                    "setToolStripStatusLabel", "" + label + ", err:", ex.Message);
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                                    "setToolStripStatusLabel", "" + label + ", err:", ex.StackTrace);
                }
            }
        }


        /**
         * @brief:SubGhz通信状態のイベント発生時
         */
        public void SubGhz_Event(object sender, int code, string msg, int outVal)
        {
            if (outVal == SubGHz.CONNECTING_CONNECT)
            {
                // 接続状態はEquStatの通信状態で管理している。
                setToolStripStatusLabel(LABEL.CONNECT, msg);
            }
            else if (outVal == SubGHz.CONNECTING_DISCONNECT)
            {
                m_rcvState = RCV_STATE.DISCONNECT;
                EventDisconnect("SubGhz通信切断");
            }

            if (code < 0)
            {
                // 接続TIMEOUTはcode -1が
                setToolStripStatusLabel(LABEL.CONNECT, "接続が切れました(SubGhz通信接続)");
            }
        }

        /// <summary>
        /// 通信端末接続ボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectSettingMenuItem_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "ConnectSettingMenuItem_Click", "通信端末接続 クリック");

            if (Program.m_ConnectMethod == 1) // TCP
            {
                // 現在未使用
                /*
                if (m_frmTcpSettings == null)
                {
                    m_frmTcpSettings = new FormTcpConnectSettings();
                }
                m_frmTcpSettings.StartPosition = FormStartPosition.CenterParent;
                m_frmTcpSettings.ShowDialog();

                m_frmTcpSettings = null;
                 * */
            }
            else if (Program.m_ConnectMethod == 2) // SubGHz
            {
                if (m_frmSubGhzSettings == null)
                {
                    m_frmSubGhzSettings = new FormSubGHzConnectSettings();
                }
                m_frmSubGhzSettings.StartPosition = FormStartPosition.CenterParent;
                m_frmSubGhzSettings.ShowDialog();

                m_frmSubGhzSettings = null;

                if (Program.m_EquStat.isConnected())
                {
                    ConnectTestMenuItem.Enabled = false;
                }
                else
                {
                    ConnectTestMenuItem.Enabled = true;
                }
            }

            UpdateShelterInfoList();
            UpdateDisplay();
        }

        /// <summary>
        /// 送受信テスト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectTestMenuItem_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "ConnectTestMenuItem_Click", "送受信テスト クリック");

            if (Program.m_ConnectMethod == 1) // TCP
            {
                // TCP時には使用できない
            }
            else if (Program.m_ConnectMethod == 2) // SubGHz
            {
                if (Program.m_EquStat.isConnected())
                {
                    // サブギガ接続中は使用できない
                }
                else
                {
                    if (m_frmSubGhzTest == null)
                    {
                        m_frmSubGhzTest = new FormSubGHzConnectTest();
                    }
                    m_frmSubGhzTest.StartPosition = FormStartPosition.CenterParent;
                    m_frmSubGhzTest.ShowDialog();

                    m_frmSubGhzTest = null;
                }
            }
        }

        /// <summary>
        /// 避難所状況(テキスト)のみ送信するチェックを操作したときの動作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbTextOnly_CheckedChanged(object sender, EventArgs e)
        {
            // Type2メッセージモード(チェックボックス)の変更

            // 文字数変更
            if (cbTextOnly.Checked)
            {
                lblTextInfo.Text = "避難所状況  (最大22文字)";
                textShelterInfo.MaxLength = 22;
                m_ActiveTarminal_info.text_flag = "1";
            }
            else
            {
                // 13文字以上のとき
                if (textShelterInfo.Text.Length > 13)
                {
                    textShelterInfo.Text = textShelterInfo.Text.Substring(0, 13);
                }
                lblTextInfo.Text = "避難所状況  (最大13文字)";
                textShelterInfo.MaxLength = 13;
                m_ActiveTarminal_info.text_flag = "0";
            }

            // 文字列保存とチェック状態保存
            textShelterInfo_TextChanged(sender, e);
        }

        /// <summary>
        /// 個人安否情報登録(TCP/IP通信用)
        /// </summary>
        /// <param name="info"></param>
        public bool resisterPersonalData(DbAccess.PersonInfo info)
        {
            lock (m_PersonSendList)
            {
                // 必須入力チェック
                /*  性別/公表可否は未選択の場合"0"が設定されるため判別不可 */
                String strMessage = "";
                if (info.id == "")
                    strMessage += "電話番号を入力してください。\n";
                if (info.name == "　")
                    strMessage += "名前を入力してください。\n";
                if (info.txt02 == "")
                    strMessage += "生年月日を入力してください。\n";
                if (info.sel01 == "")
                    strMessage += "性別を選択してください。\n";
                if (info.sel03 == "")
                    strMessage += "公表の可否を選択してください。\n";

                // 未入力の必須入力項目が存在する
                if (strMessage != "")
                {
                    return false;
                }

                // 重複チェック
                bool bExist;
                Program.m_objDbAccess.ExistPersonInfo(info, out bExist);

                // 受信したデータをDBに登録(DBに重複データがある場合は更新)
                if (bExist)
                {
                    // 更新処理
                    Program.m_objDbAccess.UpsertPersonInfo(info);
                    SendData sendData = new SendData();
                    sendData.bEdit = true;
                    sendData.bDelete = true;
                    sendData.bSendWait = true;
                    sendData.info = info;
                    m_PersonSendList.Add(sendData);
                }
                // DBにもリストにも存在しない場合、新規登録処理
                else
                {
                    // 新規登録
                    Program.m_objDbAccess.UpsertPersonInfo(info);
                    SendData sendData = new SendData();
                    sendData.bEdit = false;
                    sendData.bDelete = false;
                    sendData.bSendWait = true;
                    sendData.info = info;
                    m_PersonSendList.Add(sendData);
                }
            }

            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "person", "登録画面 登録操作");

            // 履歴追加
            DbAccess.PersonLog logInfo = new DbAccess.PersonLog();
            logInfo.init();

            // 履歴情報を設定
            logInfo.Set(info);

            Program.m_objDbAccess.InsertPersonLog(logInfo);

            // 表更新
            //UpdateListData(true);

            return true;
        }

        /// <summary>
        /// 安否情報受信設定ダイアログ表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TcpSettingMenuItem_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "TcpSettingMenuItem_Click", "安否情報受信設定 クリック");

            FormTcpSetting tcpSettingForm = new FormTcpSetting(this);
            tcpSettingForm.ShowDialog();
        }

        /// <summary>
        /// リストのカラムクリック時にソートを実施
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listPersonal_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //ListViewItemSorterを指定する
            listPersonal.ListViewItemSorter =
                new ListViewItemComparer(e.Column, m_order);
            //並び替える（ListViewItemSorterを設定するとSortが自動的に呼び出される）

            // ソート順切り替え(昇順⇔降順)
            if (m_order != 1)
            {
                m_order = 1;
            }
            else
            {
                m_order = 2;
            }
        }

        /// <summary>
        /// 避難所切替処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectShelterName_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "selectShelterName_SelectedIndexChanged", "避難所切替処理 クリック");

            //コンボボックスの避難所に対応した情報を表示
            updateActiveShelterInfo();

            //「更新」ボタンの動きを呼び出し
            // (個人安否情報リストの内容を避難所に対応したものに更新)
            btnUpdate_Click(sender, e);

            return;
        }

        /// <summary>
        ///  避難所登録ダイアログ表示処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterShelterMenuItem_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "RegisterShelterMenuItem_Click", "避難所登録 クリック");

            //端末情報保持DB（q_anpi_device_info）の使用避難所管理IDが255に達した場合、新たに登録ができないことを示すメッセージを表示
            DbAccess.TerminalInfo[] allTerminal = new DbAccess.TerminalInfo[0];
            DbAccess.TerminalInfo[] allInfo = new DbAccess.TerminalInfo[0];
            Program.m_objDbAccess.GetTerminalInfoAll(ref allInfo);

            // 接続中の端末IDと同じIDを持つ避難所の数をカウント
            int terminalCount = 0;
            foreach (var item in allInfo)
            {
                if (item.gid == Program.m_EquStat.mQCID)
                {
                    terminalCount++;
                }
            }

            // 同一端末IDを持つ避難所の数が255を超えた場合、新規登録できない
            if (terminalCount >= 255)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "RegisterShelterMenuItem_Click", "避難所の登録数が上限に達したため、新しく避難所を登録することはできません。");
                MessageBox.Show("避難所の登録数が上限に達したため、新しく避難所を登録することはできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 避難所登録ダイアログを生成
            FormRegistShelter frmRegistShelter = new FormRegistShelter(m_shelterInfoList, this);
            // 避難所登録ダイアログを表示
            frmRegistShelter.ShowDialog();

            // 登録した場合
            // DBに避難所情報を登録
            if (frmRegistShelter.GetDlgResult())
            {
                // DBに登録
                Program.m_objDbAccess.Insert_TerminalInfo(frmRegistShelter.GetDlgShelterInfo());

                // 避難所情報を追加
                m_shelterInfoList.Add(frmRegistShelter.GetDlgShelterInfo());

                // 避難所リストを更新
                SetActiveShelterInfo(selectShelterName.SelectedIndex);

                // 避難所情報を更新
                UpdateShelterInfoList();

                // 登録した避難所をアクティブにする
                int indexCount = 0;
                foreach (var item in m_shelterInfoList)
                {
                    if (item.sid == frmRegistShelter.GetDlgShelterInfo().sid)
                    {
                        break;
                    }
                    indexCount++;
                }
                SetActiveShelterInfo(indexCount);

                // メニューの活性化状態を更新
                ShelterStatusView();
            }
        }

        /// <summary>
        /// 避難所編集ダイアログ表示処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditShelterMenuItem_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "EditShelterMenuItem_Click", "避難所編集クリック");

            // 避難所リストを更新
            UpdateShelterInfoList();

            // 避難所編集ダイアログを生成
            FormEditShelter frmEditShelter = new FormEditShelter(m_shelterInfoList, Program.m_objActiveTermial.name);

            // 避難所編集ダイアログを表示
            frmEditShelter.ShowDialog();

            // 更新した場合
            if (frmEditShelter.GetDlgResult())
            {
                DbAccess.TerminalInfo editTarget = frmEditShelter.GetDlgShelterInfo();

                // 位置情報を同時に送信するかの確認ダイアログを表示
                FormConfirmUpdateLocation frmConfirmUpdateLocation = new FormConfirmUpdateLocation();

                // 対象の避難所が開設の場合、ダイアログ表示
                if (editTarget.open_flag == FormShelterInfo.SHELTER_STATUS.OPEN)
                {
                    frmConfirmUpdateLocation.ShowDialog();

                    if (Program.m_EquStat.isConnected() == false)
                    {
                        Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "EditShelterMenuItem_Click", "未接続です");
                        MessageBox.Show("Q-ANPIターミナル未接続の為、避難所編集は行えません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (Program.m_SendFlag != Program.NOT_SENDING)
                    {
                        Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "EditShelterMenuItem_Click", "送信中です");
                        MessageBox.Show("他の機能でメッセージ送信中の為、避難所編集は行えません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }


                    //通信端末に接続中かつ避難所の状態が「開設」の場合は、情報を送信後に送達確認の到着処理にてDBに登録する処理を実施
                    // 共通送信数の初期化
                    initCommonSendingBuff();
                    initSending(true);

                    m_OpenCloseShelterInfo = new DbAccess.TerminalInfo();
                    if (frmEditShelter.GetDlgShelterInfo().open_flag == FormShelterInfo.SHELTER_STATUS.OPEN)
                    {
                        // 実行コマンドをセット
                        m_NowCommand = SendMessageCommand.EDIT_SHELTER_NO_POSITION;
                        SendShelterNameInfo(editTarget);

                        // 避難所名⇒開設情報の順番が前後しないようにウェイトを挿入
                        Thread.Sleep(100);

                    }

                    // 確認ダイアログで位置情報も更新を選択していた場合、更に位置情報も更新する。

                    if (frmConfirmUpdateLocation.isUpdateLocation)
                    {
                        // 実行コマンドをセット
                        m_NowCommand = SendMessageCommand.EDIT_SHELTER;

                        // Q-ANPIターミナルから取得した経度緯度でDBの避難所情報データを更新する

                        editTarget.lat = Program.m_EquStat.mLatitude.ToString();
                        editTarget.lon = Program.m_EquStat.mLongitude.ToString();

                        // 位置情報を送信
                        SendShelterOpenCloseInfo(editTarget);

                        // メイン画面の表示を更新
                        UpdateShelterInfoList();
                        updateActiveShelterInfo();
                    }

                    // メニューの活性化状態を更新
                    ShelterStatusView();
                }

                // 避難所が「開設」以外の場合はそのままDBに登録する
                else
                {
                    // メニューの活性化状態を更新
                    ShelterStatusView();

                    // DBに登録
                    Program.m_objDbAccess.Update_TerminalInfo(editTarget);

                    // 避難所情報を追加
                    m_shelterInfoList.Add(editTarget);

                    // 避難所情報を更新
                    UpdateShelterInfoList();
                }

                // 更新した避難所をアクティブ状態にする
                SetActiveShelterInfo(selectShelterName.SelectedIndex);
            }
            return;
        }

        /// <summary>
        /// 個人安否避難所情報自動送信設定ダイアログ表示処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoSendSettingMenuItem_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "AutoSendSettingMenuItem_Click", "個人安否/避難所情報自動送信設定 クリック");

            // 個人安否避難所情報自動送信設定ダイアログを生成
            FormAutoSendSetup frmAutoSendSetup = new FormAutoSendSetup();
            // 個人安否避難所情報自動送信設定ダイアログを表示
            frmAutoSendSetup.ShowDialog();
        }

        /// <summary>
        /// 位置情報更新ダイアログ表示処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditLocationInfoMenuItem_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "EditLocationInfoMenuItem_Click", "位置情報更新　クリック");

            if (Program.m_EquStat.isConnected() == false)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "OpenShelterMenuItem_Click", "未接続です");
                MessageBox.Show("Q-ANPIターミナル未接続の為、位置情報は取得できません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 選択中の避難所名
            string shelterName = m_ActiveTarminal_info.name;

            // 位置情報更新ダイアログを生成
            FormUpdateLocationInfo frmUpdateLocationInfo = new FormUpdateLocationInfo(m_shelterInfoList, shelterName, this);
            // 位置情報更新ダイアログを表示
            frmUpdateLocationInfo.ShowDialog();

            // 更新した場合
            if (frmUpdateLocationInfo.GetDlgResult())
            {
                if (Program.m_EquStat.isConnected() == false)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "OpenShelterMenuItem_Click", "未接続です");
                    MessageBox.Show("Q-ANPIターミナル未接続の為、位置情報は取得できません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (Program.m_SendFlag != Program.NOT_SENDING)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "OpenShelterMenuItem_Click", "送信中です");
                    MessageBox.Show("他の機能でメッセージ送信中の為、位置情報は取得できません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 最新情報を表示する
                SetActiveShelterInfo(selectShelterName.SelectedIndex);

                // 共通送信数の初期化
                initCommonSendingBuff();
                initSending(true);

                // 実行コマンドをセット
                m_NowCommand = SendMessageCommand.UPDATE_LOCATION;

                m_OpenCloseShelterInfo = frmUpdateLocationInfo.GetDlgShelterInfo();

                // 情報を送信
                SendShelterOpenCloseInfo(m_OpenCloseShelterInfo);

                // 避難所管理画面の活性化状態を更新
                ShelterStatusView();
            }
        }
        
        /// <summary>
        /// 避難所情報更新時の処理(避難所登録時は除く)
        /// </summary>
        /// <param name="shelterName"></param>
        public void SetActiveShelterInfo(int listIndex)
        {
            // DBから避難所情報を再取得
            UpdateShelterInfoList();

            // 避難所名一覧を一度クリア
            selectShelterName.Items.Clear();

            // 避難所名一覧を再生成
            for (int i = 0; i < m_shelterInfoList.Count; i++)
            {
                // 開設済みの避難所のみコンボボックスに追加
                //if(m_shelterInfoList[i].open_flag == "1")
                {
                    selectShelterName.Items.Add(m_shelterInfoList[i].name);
                }
            }

            // 引数に指定されたインデックス番号にリストの状態を設定
            selectShelterName.SelectedIndex = listIndex;

            //メニューアイテムの更新
            ShelterStatusView();

            //アクティブ状態になっている避難所に情報を更新
            updateActiveShelterInfo();

            //画面表示更新
            m_initialize = true;
            btnUpdate_Click(null, null);
            m_initialize = false;

            return;
        }

        /// <summary>
        /// 開設(新)ダイアログ表示処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenShelterMenuItem_Click(object sender, EventArgs e)
        {
            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "OpenShelterMenuItem_Click", "開設クリック");

            if (Program.m_EquStat.isConnected() == false)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "OpenShelterMenuItem_Click", "未接続です");
                MessageBox.Show("Q-ANPIターミナル未接続の為、避難所の開設はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int tryCount = 0;
            while ((Program.m_EquStat.mQCID == "") || (Program.m_EquStat.mQCID == null))
            {
                // Q-ANPIターミナル接続から端末ID取得までラグがある
                // 端末に接続済みかつ端末ID未取得の時、ID取得完了まで待機
                // 10秒以上IDの取得ができない場合は端末未接続エラーにする
                System.Threading.Thread.Sleep(100);
                tryCount++;
                if (tryCount > 100)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "OpenShelterMenuItem_Click", "未接続です");
                    MessageBox.Show("Q-ANPIターミナル未接続の為、避難所の開設はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            // 最新の避難所情報を取得
            UpdateShelterInfoList();

            // 開設(新)ダイアログを生成
            FormOpenShelter2 FrmDummyOpenShelter = new FormOpenShelter2(m_shelterInfoList);
            // 開設(新)ダイアログを表示
            FrmDummyOpenShelter.ShowDialog();

            // 開設した場合
            if (FrmDummyOpenShelter.GetDlgResult())
            {
                DbAccess.TerminalInfo info = FrmDummyOpenShelter.GetDlgShelterInfo();
                m_OpenCloseShelterInfo = info;


#if false // 開設情報送信テストの為、コメントアウト　デバッグモードでも送達確認が受信されないと開設できない
                //DEBUG:一時的に送達確認無しでデータを登録できるようにする(デバッグ時は下記コメントアウトを解除する)---
                // DBの避難所データを更新
                info.open_flag = "1";
                Program.m_objDbAccess.UpsertTerminalInfo(info);

                // 避難所情報を更新
                m_shelterInfoList = FrmDummyOpenShelter.shelterInfoList;

                // 最新情報を表示する
                SetActiveShelterInfo(selectShelterName.SelectedIndex);
                //DEBUG:一時的に送達確認無しでデータを登録できるようにする---
#endif

                //---Q-QNPI端末に開設した避難所の情報を送信---
                //接続チェック
                if (Program.m_EquStat.isConnected() == false)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "OpenShelterMenuItem_Click", "未接続です");
                    MessageBox.Show("Q-ANPIターミナル未接続の為、避難所の開設はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 送信中チェック
                if (Program.m_SendFlag != Program.NOT_SENDING)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "OpenShelterMenuItem_Click", "送信中です");
                    MessageBox.Show("他の機能でメッセージ送信中の為、避難所の開設はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 共通送信数の初期化
                initCommonSendingBuff();
                initSending(true);

                // 実行コマンドをセット
                m_NowCommand = SendMessageCommand.OPEN_SHELTER;

                // 避難所名情報を送信
                SendShelterNameInfo(info);

                // 避難所名⇒開設情報の順番が前後しないようにウェイトを挿入
                Thread.Sleep(100);

                // 開設情報を送信する
                SendShelterOpenCloseInfo(info);

                // 避難所管理画面の活性化状態を更新
                ShelterStatusView();

            }
        }

        /// <summary>
        /// 閉鎖(新)ダイアログ表示処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseShelterMenuItem_Click(object sender, EventArgs e)
        {

            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "FormShelterInfo", "", "閉鎖クリック");

            if (Program.m_EquStat.isConnected() == false)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "CloseShelterMenuItem_Click", "未接続です");
                MessageBox.Show("Q-ANPIターミナル未接続の為、避難所の閉鎖はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int tryCount = 0;
            while ((Program.m_EquStat.mQCID == "") || (Program.m_EquStat.mQCID == null))
            {
                // Q-ANPIターミナル接続から端末ID取得までラグがある
                // 端末に接続済みかつ端末ID未取得の時、ID取得完了まで待機
                // 10秒以上IDの取得ができない場合は端末未接続エラーにする
                System.Threading.Thread.Sleep(100);
                tryCount++;
                if (tryCount > 100)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "CloseShelterMenuItem_Click", "未接続です");
                    MessageBox.Show("Q-ANPIターミナル未接続の為、避難所の閉鎖はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            // 閉鎖(新)ダイアログを生成
            FormCloseShelter2 FrmCloseShelter = new FormCloseShelter2(m_shelterInfoList, Program.m_objActiveTermial.name);

            // 閉鎖(新)ダイアログを表示
            FrmCloseShelter.ShowDialog();

            // 閉鎖場合
            if (FrmCloseShelter.GetDlgResult())
            {

                //接続チェック
                if (Program.m_EquStat.isConnected() == false)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "CloseShelterMenuItem_Click", "未接続です");
                    MessageBox.Show("Q-ANPIターミナル未接続の為、避難所の閉鎖はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 送信中チェック
                if (Program.m_SendFlag != Program.NOT_SENDING)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "CloseShelterMenuItem_Click", "送信中です");
                    MessageBox.Show("他の機能でメッセージ送信中の為、避難所の閉鎖はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ////閉鎖する避難所を取得
                DbAccess.TerminalInfo info = FrmCloseShelter.GetDlgShelterInfo();
                m_OpenCloseShelterInfo = info;

#if false
                //DEBUG:一時的に送達確認無しでデータを登録できるようにする(デバッグ時は下記をコメントアウト解除する)---
                //// DBの避難所データを更新
                m_ShelterStat = info.open_flag;
                Program.m_objDbAccess.UpsertTerminalInfo(info);

                // 避難所情報を更新
                UpdateShelterInfoList();

                // 最新情報を表示する
                SetActiveShelterInfo(selectShelterName.SelectedIndex);

                //DEBUG:一時的に送達確認無しでデータを登録できるようにする---
#endif
                // 共通送信数の初期化
                initCommonSendingBuff();
                initSending(true);

                // 実行コマンドをセット
                m_NowCommand = SendMessageCommand.CLOSE_SHELTER;

                Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "FormShelterInfo", "", "閉鎖処理開始");
                setToolStripStatusLabel(LABEL.APP, "避難所情報送信開始  ");

                // 開設情報を送信する
                SendShelterOpenCloseInfo(info);

                // 避難所管理画面の活性化状態を更新する
                ShelterStatusView();
            }
        }

        /// <summary>
        /// 避難所名情報送信(Type2,ST2)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public bool SendShelterNameInfo(DbAccess.TerminalInfo ShelterNameInfo, bool isAuto = false, int autoMaxCount = 0)
        {
            if (isAuto)
            {
                if ((Program.m_SendFlag != Program.NOT_SENDING))
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "SendShelterNameInfo", "送信中です");
                    return false;
                }

                if (Program.m_EquStat.isConnected() == false)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "SendShelterNameInfo", "未接続です");
                    return false;
                }
            }
            else
            {
                if ((Program.m_SendFlag != Program.NOT_SENDING))
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "SendShelterNameInfo", "送信中です");
                    MessageBox.Show("送信中です", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (Program.m_EquStat.isConnected() == false)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "SendShelterNameInfo", "未接続です");
                    MessageBox.Show("Q-ANPIターミナル未接続の為、避難所名の送信はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            // 送信待機前に先に送信数を計算しておく
            if (isAuto)
            {
                m_commonCountMax = autoMaxCount;
            }
            else
            {
                m_commonCountMax += m_Type2CountMax_1;
            }
            Program.m_SendFlag = Program.SENDING_SHELTER_INFO;         // 0:送信中でない  1:個人安否送信  2:避難所情報送信　3:開設/閉鎖/端末情報送信
            ShelterStatusView();

            int myWatingNo = m_nowWaitNo;
            m_nowWaitNo++;

            // 送信リクエスト処理は別スレッド
            Task task = Task.Factory.StartNew(() =>
            {
                int timeoutCount = 0;
                bool okSend = true;
                //int myWatingNo = m_nowWaitNo;
                //m_nowWaitNo++;

                Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "SendShelterNameInfo(TaskID：" + Task.CurrentId + ")", "避難所名送信(" + ShelterNameInfo.name + ")", "myWatingNo = " + myWatingNo + ", m_nowWaitNo = " + m_nowWaitNo);

                while (true)
                {
                    // 現在送信中ではない、かつ自分の送信順が来たら送信開始
                    if ((myWatingNo == (m_nowSendFinNo)) && (!m_nowSending))
                    {
                        break;
                    }
                    // 送信中フラグがOFFになるまで待機
                    System.Threading.Thread.Sleep(100);
                    timeoutCount++;

                    // 送信待機中に送信状態がリセット(解除)されたら、他の送信中に送信失敗したとみなし、送信待機を解除
                    if (Program.m_SendFlag == Program.NOT_SENDING)
                    {
                        okSend = false;
                        break;
                    }

                    // 順番待ち変数がint型の最大値-10を超えていたら送信せず、避難所アプリを再起動するメッセージを出す
                    if (myWatingNo > (int.MaxValue - 10))
                    {
                        Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "SendShelterNameInfo", "送信待ち受け数オーバー");
                        MessageBox.Show("メモリが不足しています。避難所アプリを再起動してください。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        okSend = false;
                        break;
                    }
                }
                if (okSend)
                {

                    Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "SendShelterNameInfo", "避難所名", "避難所名情報送信処理開始");

                    // 避難所の情報を保持する(送達確認時、DBに保存する為)
                    m_OpenCloseShelterInfo = ShelterNameInfo;

                    //// 送信する避難所の情報を格納(ログ出力用)
                    //m_TotalSendLog.num01 = "0";
                    //m_TotalSendLog.num02 = "0";
                    //m_TotalSendLog.num03 = "0";
                    //m_TotalSendLog.num04 = "0";
                    //m_TotalSendLog.num05 = "0";
                    //m_TotalSendLog.num06 = "0";
                    //m_TotalSendLog.num07 = "0";
                    //m_TotalSendLog.num08 = "0";
                    //m_TotalSendLog.num09 = "0";
                    //m_TotalSendLog.num10 = "0";
                    //m_TotalSendLog.num11 = "0";
                    //m_TotalSendLog.num12 = "0";
                    //m_TotalSendLog.txt01 = "";
                    //m_TotalSendLog.update_datetime = ShelterNameInfo.update_datetime;
                    //m_TotalSendLog.sid = ShelterNameInfo.sid;

                    // 避難所名情報を送信(メッセージタイプ２）　
                    m_sendDataArr.Clear();

                    // データ設定
                    SetCalcData_ShelterName(ShelterNameInfo.gid, ShelterNameInfo.smid, ShelterNameInfo.name);

                    setToolStripStatusLabel(LABEL.APP, "避難所情報送信開始  ");

                    // 送達確認用
                    m_CheckType2_1 = Enumerable.Repeat<int>(TYPE2_SEND_STATE_DEFAULT, m_Type2CountMax_1).ToArray();

                    m_StateType2_1 = STATE.SENDING;
                    m_Type2SendStartCount_1 = 0;
                    //m_Type2SendingCount_1 = 0;
                    m_Type2WaitingCount_1 = 0;



                    // ステータスバーの送信状況を更新
                    string lblmsg = getLabelRtnCount(m_commonSendStartCount, m_commonCountMax);
                    setToolStripStatusLabel(LABEL.RTN_SND, "req:" + lblmsg);
                    lblmsg = getLabelRtnCount(m_commonSendingCount, m_commonCountMax);
                    setToolStripStatusLabel(LABEL.RTN_RCV, "send:" + lblmsg);
                    lblmsg = getLabelRtnCount(m_commonWaitingCount, m_commonCountMax);
                    setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);

                    // 送信中フラグをONにし、送信処理
                    m_nowSending = true;
                    string sndVal2 = (string)m_sendDataArr[0];
                    byte[] binary2 = MsgConv.ConvStringToBytes(sndVal2);
                    Program.m_RtnThreadSend.AddSendQue(binary2);
                }
            });
            return true;
        }


        /// <summary>
        /// 避難所リストを更新
        /// </summary>
        public void UpdateShelterInfoList()
        {
            // 一旦リストをクリア
            m_shelterInfoList.Clear();

            // DBの避難所情報をすべて取得
            DbAccess.TerminalInfo[] AllTernimalInfo = new DbAccess.TerminalInfo[0];
            Program.m_objDbAccess.GetTerminalInfoAll(ref AllTernimalInfo);

            // shelterInfoListを更新する
            foreach (var item in AllTernimalInfo)
            {
                m_shelterInfoList.Add(item);
            }
        }

        /// <summary>
        /// 選択中の避難所選択リスト項目に避難所ID、緯度/経度を合わせる
        /// </summary>
        public void updateActiveShelterInfo()
        {
            bool find = false;
            int listCount = 0;
            DbAccess.TerminalInfo tmpInfo = new DbAccess.TerminalInfo();
            foreach (var info in m_shelterInfoList)
            {
                if (listCount == selectShelterName.SelectedIndex)
                {
                    //アクティブ状態の避難所情報のセット
                    Program.m_objActiveTermial = info;
                    m_ActiveTarminal_info = info;
                    m_ShelterStat = info.open_flag;

                    // 避難所ID
                    txtShelterID.Text = info.sid;

                    // 避難所名
                    txtShelterName.Text = info.name;

                    // 緯度経度
                    txtShelterPos.Text = info.lat + "," + info.lon;

                    // 開設日時
                    lblShelterStatDate.Text = info.open_datetime;

                    // メモ
                    txtMemo.Text = info.memo;

                    // 避難所状況
                    textShelterInfo.Text = info.status;

                    // 「避難所状況のみ送信する」チェックボックス
                    if (info.text_flag == "1")
                    {
                        cbTextOnly.Checked = true;
                    }
                    else
                    {
                        cbTextOnly.Checked = false;
                    }
                    find = true;

                    txtMemo.Text = info.memo;

                    // 避難者数任意送信チェック
                    checkPersonCnt.Checked = info.dummy_num_flag;

                    // 任意避難者数
                    textPersonCnt.Text = info.dummy_num;

                    //避難所を管理するDB(terminal_info)に現在アクティブな避難所の更新日時を更新する
                    Program.m_objDbAccess.UpsertTerminalInfo(info);
                    tmpInfo = info;
                    break;
                }
                listCount++;
            }


            //閉鎖後等でアクティブな避難所がない場合、ID,緯度,経度,更新日時は表示しない
            if (!find)
            {
                txtShelterID.Text = "";
                txtShelterPos.Text = "";
                lblShelterStatDate.Text = "";
                Program.m_objActiveTermial = new DbAccess.TerminalInfo();
                m_ActiveTarminal_info = new DbAccess.TerminalInfo();
                m_ShelterStat = 0;
                textShelterInfo.Text = "";
            }
            else
            {
                updateSelectTerminal(tmpInfo);
            }

            ShelterStatusView();
        }


        /// <summary>
        /// DBの現在アクティブな避難所を表すフラグを更新する
        /// </summary>
        /// <param name="selectTerminal"></param>
        private void updateSelectTerminal(DbAccess.TerminalInfo selectTerminal)
        {
            // DB内の全避難所情報を取得
            DbAccess.TerminalInfo[] AllTernimalInfo = new DbAccess.TerminalInfo[0];
            Program.m_objDbAccess.GetTerminalInfoAll(ref AllTernimalInfo);
            DbAccess.TerminalInfo tmp = new DbAccess.TerminalInfo();
            foreach (var item in AllTernimalInfo)
            {
                tmp = item;
                if (item.sid == selectTerminal.sid)
                {
                    tmp.select_flag = true;
                }
                else
                {
                    tmp.select_flag = false;
                }
                Program.m_objDbAccess.UpsertTerminalInfo(tmp);
            }
        }


        /// <summary>
        /// 一定周期で時刻同期要求をQ-ANPIターミナルに送信する
        /// </summary>
        private void sendTimeSyncThread()
        {
            Task.Factory.StartNew(() =>
            {

                // 1時間間隔でループ
                while (true)
                {
                    int timeoutCount = 0;
                    if (!Program.m_EquStat.isConnected() || (Program.m_SendFlag != Program.NOT_SENDING))
                    {
                        // Q-ANPIターミナル未接続時は1秒間隔で接続を待機
                        // 接続感知後直ちに時刻同期要求を送信
                        System.Threading.Thread.Sleep(1000);
                        continue;
                    }
                    else
                    {
                        // 時刻同期要求送信
                        Program.m_SubGHz.SendSyncTime();
                        m_isTimeSyncOK = false;
                        m_timeSyncRetry = false;    //再送要求フラグOFF    //2020.03.17 Add

                        // SubGHzRtnThread.cs rcvTimeSyncEvent()にて、返答受信時にPCのシステム時刻を変更する
                        // 設定ファイル(qzss.xml)に記載されている値の間隔でループ
                        int sleepCount = 0;
                        int retryTime = 10;
                        string stateStr = "";
                        while (true)
                        {
                            // 時刻同期中
                            if (!m_isTimeSyncOK)
                            {
                                // 10秒経過しても時刻同期の応答がない場合、時刻同期要求を再送する
                                if (sleepCount > retryTime)
                                {
                                    // 時刻同期要求再送信
                                    Program.m_SubGHz.SendSyncTime();

                                    retryTime += 10;
                                    timeoutCount++;
                                }

                                // ステータスバーに「時刻同期中」を表示
                                if ((sleepCount % 3) == 0)
                                {
                                    stateStr = "時刻同期中.   |";
                                }
                                else if ((sleepCount % 3) == 1)
                                {
                                    stateStr = "時刻同期中..  |";
                                }
                                else
                                {
                                    stateStr = "時刻同期中... |";
                                }

                                if (timeoutCount > 5)
                                {
                                    // ステータスバーに「時刻未同期」を表示
                                    stateStr = "時刻未同期    |";
                                    m_isTimeSyncOK = true;
                                    timeoutCount = 0;
                                }
                                setToolStripStatusLabel(LABEL.TIME_SYNC, stateStr);
                            }

                            // 1秒待機
                            System.Threading.Thread.Sleep(1000);
                            sleepCount++;

                            // 次の時刻同期を実施
                            if (sleepCount > Program.m_objShelterAppConfig.TimeSyncCycle)
                            {
                                break;
                            }

                            // 待機中に切断された場合は待機解除
                            if (!(Program.m_EquStat.isConnected()))
                            {
                                break;
                            }
                            // 再送要求がある場合、1秒待機後に再度実施     //2020.03.18 Add
                            // ※1秒待機は上記で実施済み
                            if (m_timeSyncRetry)
                            {
                                break;
                            }

                            // 何も送信していないときに送信Q-ANPI端末送信待ち受け番号をリセットする
                            if ((Program.m_SendFlag == Program.NOT_SENDING) && (!m_nowSending))
                            {
                                ResetSendWaitingNo();
                            }
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 避難所開設情報を送信する(Type0,ST0,1)
        /// </summary>
        /// <param name="info"></param>
        public void SendShelterOpenCloseInfo(DbAccess.TerminalInfo info, bool isAuto = false, int autoMaxCount = 0)
        {
            // 開設と閉鎖でログを分ける
            if (info.open_flag == FormShelterInfo.SHELTER_STATUS.OPEN)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "FormShelterInfo", "", "開設処理開始");
            }
            else
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "FormShelterInfo", "", "閉鎖処理開始");
            }

            setToolStripStatusLabel(LABEL.APP, "避難所情報送信開始  ");

            Program.m_SendFlag = Program.SENDING_TERMINAL_INFO;         // 0:送信中でない  1:個人安否送信  2:避難所情報送信　3:開設/閉鎖/端末情報送信
            ShelterStatusView();

            // 先にカウントしておく
            if (isAuto)
            {
                m_commonCountMax = autoMaxCount;
            }
            else
            {
                m_commonCountMax += m_Type0CountMax;
            }

            int myWatingNo = m_nowWaitNo;
            m_nowWaitNo++;

            // 送信リクエスト処理は別スレッド
            Task task = Task.Factory.StartNew(() =>
            {
                int timeoutCount = 0;
                bool okSend = true;
                //int myWatingNo = m_nowWaitNo;
                //m_nowWaitNo++;

                Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "SendShelterOpenCloseInfo(TaskID：" + Task.CurrentId + ")", "避難者数送信(" + info.name + ")", "myWatingNo = " + myWatingNo + ", m_nowWaitNo = " + m_nowWaitNo);

                while (true)
                {
                    // 現在送信中ではない、かつ自分の送信順が来たら送信開始
                    if ((myWatingNo == (m_nowSendFinNo)) && (!m_nowSending))
                    {
                        break;
                    }
                    // 送信中フラグがOFFになるまで待機
                    System.Threading.Thread.Sleep(100);
                    timeoutCount++;

                    // 送信待機中に送信状態がリセット(解除)されたら、他の送信中に送信失敗したとみなし、送信待機を解除
                    if (Program.m_SendFlag == Program.NOT_SENDING)
                    {
                        okSend = false;
                        break;
                    }

                    // 順番待ち変数がint型の最大値-10を超えていたら送信せず、避難所アプリを再起動するメッセージを出す
                    if (myWatingNo > (int.MaxValue - 10))
                    {
                        Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "SendShelterOpenCloseInfo", "送信待ち受け数オーバー");
                        MessageBox.Show("メモリが不足しています。避難所アプリを再起動してください。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        okSend = false;
                        break;
                    }
                }
                if (okSend)
                {
                    // 避難所情報保持(送達確認受信時にこの情報でDBの更新を行う)
                    m_OpenCloseShelterInfo = info;

                    //送信準備
                    m_sendDataArr.Clear();
                    //m_nSndTotal = 0;
                    m_nSndCnt = 0;
                    m_StateType0 = STATE.SENDING;
                    m_Type0SendingCount = 0;
                    m_Type0SendStartCount = 0;
                    m_Type0WaitingCount = 0;

                    //m_commonCountMax += m_Type0CountMax;
                    m_commonSendStartCount += m_Type0SendStartCount;
                    m_commonSendingCount += m_Type0SendingCount;
                    m_commonWaitingCount += m_Type0WaitingCount;

                    //ステータスバーに表示する文字列をセット
                    string lblmsg = getLabelRtnCount(m_commonSendStartCount, m_commonCountMax);
                    setToolStripStatusLabel(LABEL.RTN_SND, "req:" + lblmsg);
                    lblmsg = getLabelRtnCount(m_commonSendingCount, m_commonCountMax);
                    setToolStripStatusLabel(LABEL.RTN_RCV, "send:" + lblmsg);
                    lblmsg = getLabelRtnCount(m_commonWaitingCount, m_commonCountMax);
                    setToolStripStatusLabel(LABEL.RTN_CHK, "resp:" + lblmsg);

                    if (String.IsNullOrEmpty(info.sid))
                    {
                        if (String.IsNullOrEmpty(m_ActiveTarminal_info.sid))
                        {
                            info.sid = info.gid;
                        }
                        else
                        {
                            info.sid = m_ActiveTarminal_info.gid;
                        }
                    }

                    int personCount = 0;
                    // 避難所情報の任意の避難者数フラグがONの場合、任意の避難者数を送信する避難者数としてセット
                    if ((info.dummy_num_flag) && (int.TryParse(info.dummy_num, out personCount)))
                    {
                        personCount = int.Parse(info.dummy_num);
                    }
                    // 避難所情報の任意の避難者数フラグがOFFの場合、「入所」となっている人数を送信する避難者数としてセット
                    else
                    {
                        // 避難所に登録されている人数を取得
                        List<DbAccess.PersonInfo> personInfo = new List<DbAccess.PersonInfo>();
                        Program.m_objDbAccess.GetPersonInfoList(info.sid, ref personInfo);

                        // 避難所内の「入所」の人数を計算
                        foreach (var person in personInfo)
                        {
                            if (person.sel02 == "0") personCount++;
                        }
                    }

                    // 20170626 Type0 サブタイプ追加
                    for (int i = 1; i < 3; i++)
                    {
                        bool openState = false;
                        if (info.open_flag == SHELTER_STATUS.OPEN) openState = true;
                        string sData = MsgConv.ConvTerminalInfoToSendString(openState, info, personCount, i);

                        Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "FormShelterInfo", "SendShelterOpenCloseInfo", sData);

                        m_nSndTotal++;
                        m_Type0SendStartCount++;

                        Program.m_SendFlag = Program.SENDING_TERMINAL_INFO;         // 0:送信中でない  1:個人安否送信  2:避難所情報送信　3:開設/閉鎖/端末情報送信
                        //                setToolStripStatusLabel(1, "端末情報情報送信中  " + (m_nSndCnt + 1).ToString() + "/" + m_nSndTotal.ToString();
                        ShelterStatusView();

                        Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG,
                            "SendShelterOpenCloseInfo", "RTN", "送信 " + m_Type0SendStartCount + " " + m_nSndCnt);

                        // 送信中フラグをONにし、送信処理
                        m_nowSending = true;

                        byte[] binary = MsgConv.ConvStringToBytes(sData);
                        Program.m_RtnThreadSend.AddSendQue(binary);
                    }
                }
            });
        }

        /// <summary>
        /// 避難所状況テキストボックス内容変更時コールバック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textShelterInfo_TextChanged(object sender, EventArgs e)
        {
            if (m_ActiveTarminal_info.sid != null)
            {
                // 避難所状況テキストボックス内容取得
                m_ActiveTarminal_info.status = textShelterInfo.Text;

                // 任意避難者数ON/OFF状態取得
                m_ActiveTarminal_info.dummy_num_flag = checkPersonCnt.Checked;

                // 任意避難者数取得
                m_ActiveTarminal_info.dummy_num = textPersonCnt.Text;

                Program.m_objDbAccess.UpsertTerminalInfo(m_ActiveTarminal_info);
                UpdateShelterInfoList();
            }
        }

        /// <summary>
        /// 避難所切替ボタン押下時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchShelterMenuItem_Click(object sender, EventArgs e)
        {
            UpdateShelterInfoList();

            FormSwitchShelter formSwitchShelter = new FormSwitchShelter(m_shelterInfoList, txtShelterID.Text);

            formSwitchShelter.ShowDialog();

            // 避難所選択画面で何も選択していない時、何もせずに戻る
            if (formSwitchShelter.getSelectShelterIndex() != -1)
            {
                selectShelterName.SelectedIndex = formSwitchShelter.getSelectShelterIndex();
            }


            //コンボボックスの避難所に対応した情報を表示
            updateActiveShelterInfo();

            //「更新」ボタンの動きを呼び出し
            // (個人安否情報リストの内容を避難所に対応したものに更新)
            m_initialize = true;
            btnUpdate_Click(null, null);
            m_initialize = false;
        }

        /// <summary>
        /// 救助支援情報選択時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listMsg_SelectedIndexChanged(object sender, EventArgs e)
        {
            // バイナリ保存ボタンの活性化制御
            //activateOutputBinary();

            // 選択したメッセージを既読状態にする
            SetMessageRead();
        }

        /// <summary>
        /// 選択したメッセージを既読状態にする
        /// </summary>
        private void SetMessageRead()
        {
            int idx = 0;
            if (listMsg.SelectedItems.Count > 0)
            {
                idx = listMsg.SelectedItems[0].Index;
                if (m_selected == LISTVIEW_L1S)
                {
                    byte[] bitD = m_l1sList[idx].bitdata; // 受信したバイナリデータ
                    string msg = Program.m_BitsToMsg.getMsg(bitD);
                    this.textMsg.Text = msg;

                    // 未読メッセージであれば既読とする
                    if (m_l1sList[idx].readflg == 0)      // 未読
                    {
                        // DBテーブルのReadFlgを変更
                        Program.m_objDbAccess.SetReadFlg(0, m_l1sList[idx].id, m_l1sList[idx].rdate, 1);
                        // 選択リストアイテムのBold表示をRegular表示に変更
                        listMsg.Items[idx].Font = new Font(listMsg.Items[idx].Font, FontStyle.Regular);
                    }
                    else if (m_l1sList[idx].readflg == 1) // 既読
                    {
                        // 何もしない
                    }
                    else
                    {
                        // N/A
                    }

                }
                else if (m_selected == LISTVIEW_130)
                {
                    if (m_RescueList[idx].Dt == 1)    // テキスト表示
                    {
                        this.textMsg.Text = m_RescueList[idx].message;
                        //btnOutputBinary.Visible = false;
                    }
                    else if (m_RescueList[idx].Dt == 0)    // バイナリ表示
                    {
                        this.textMsg.Text = "バイナリデータ" + (m_RescueList[idx].msglength) + "byte";
                        //btnOutputBinary.Visible = true;
                    }
                    else
                    {
                        // N/A
                    }

                    // 未読メッセージであれば既読とする
                    if (m_RescueList[idx].readflg == 0)      // 未読
                    {
                        // DBテーブルのReadFlgを変更
                        Program.m_objDbAccess.SetReadFlg(1, m_RescueList[idx].id, m_RescueList[idx].rdate, 1);
                        // 選択リストアイテムのBold表示をRegular表示に変更
                        listMsg.Items[idx].Font = new Font(listMsg.Items[idx].Font, FontStyle.Regular);
                    }
                    else if (m_RescueList[idx].readflg == 1) // 既読
                    {
                        // 何もしない
                    }
                    else
                    {
                        // N/A
                    }
                }
            }
        }

        /// <summary>
        /// バイナリ保存ボタン活性化制御処理
        /// </summary>
        private void activateOutputBinary()
        {
            int idx = 0;
            if (listMsg.SelectedItems.Count > 0)
            {
                // 選択した行のDtフラグを取得(テキストorバイナリ)
                foreach (var item in m_RescueList)
                {
                    if (item.id == listMsg.SelectedItems[0].SubItems[0].Text)
                    {
                        idx = item.Dt;
                        break;
                    }
                }

                // バイナリの場合、バイナリ保存ボタンを活性化
                switch (idx)
                {
                    case 0:
                        btnOutputBinary.Enabled = true;
                        break;
                    case 1:
                        btnOutputBinary.Enabled = false;
                        break;
                    default:
                        btnOutputBinary.Enabled = false;
                        break;
                }
                textMsg.Text = listMsg.SelectedItems[0].SubItems[3].Text;
            }
            else
            {
                btnOutputBinary.Enabled = false;
            }
        }

        private void btnIsReceive_Click(object sender, EventArgs e)
        {
            /*
            //「受信再開」ボタン押下
            if (btnIsReceive.Checked)
            {
                //Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FormReceiveMessageView", "btnIsReceive_CheckedChanged", "受信状態切り替え 受信停止中→受信中");
                //btnIsReceive.Enabled = false;
                btnIsReceive.Text = "受信停止";
                // 受信再開メッセージをQ-ANPIターミナルに投げる
                //Program.m_SubGHz.SendDisasterReport(true);
            }
            //「受信停止」ボタン押下
            else
            {
                //Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "FormReceiveMessageView", "btnIsReceive_CheckedChanged", "受信状態切り替え 受信中→受信停止中");
                //btnIsReceive.Enabled = false;
                btnIsReceive.Text = "受信再開";
                // 受信停止メッセージをQ-ANPIターミナルに投げる
                //Program.m_SubGHz.SendDisasterReport(false);
            }
            /*
            //タイムアウトの処理
            Task.Factory.StartNew(() =>
            {
                ButtonTimeout();
            });
            */
        }


        /// <summary>
        /// 避難所切替ボタン押下時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeShelterMenuItem_Click(object sender, EventArgs e)
        {
            UpdateShelterInfoList();

            FormModifyPersonalInfo formModifyPersonalInfo = new FormModifyPersonalInfo(m_shelterInfoList, m_ActiveTarminal_info);

            formModifyPersonalInfo.ShowDialog();

            btnUpdate_Click(sender, e);
        }


        /// <summary>
        /// 人数自動集計フラグ操作時処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkPersonCnt_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkPersonCnt.Checked)
            {
                textPersonCnt.Enabled = false;
            }
            else
            {
                textPersonCnt.Enabled = true;
            }
            textShelterInfo_TextChanged(sender, e);
        }


        /// <summary>
        /// 救助支援情報リスト更新
        /// </summary>
        /// <param name="type"></param>
        public void loadList(int type)
        {
            lock (this)
            {
                // 画面のリストクリア
                listMsg.Items.Clear();
                m_l1sList.Clear();
                m_RescueList.Clear();
                int listnum = 0;

                textMsg.Text = "";


                // リスト取得(type 1:災害危機 2:支援情報)
                if (type == 1)
                {
                    Program.m_objDbAccess.GetRecvMsgInfo(ref m_l1sList);
                    if (m_l1sList.Count == 0)
                    {
                        // データなし
                        return;
                    }
                    // リスト件数MAX20件分表示する（表示優先順はGetRecvMsgInfoにて設定)
                    if (m_l1sList.Count >= LISTVIEW_MAX)
                    {
                        listnum = LISTVIEW_MAX;
                    }
                    else
                    {
                        listnum = m_l1sList.Count;
                    }

                    for (int idx = 0; idx < listnum; idx++)
                    {
                        DbAccessStep2.RecvMsgInfo l1sInfo = m_l1sList[idx];

                        string[] strItems = new string[4];
                        strItems[0] = l1sInfo.id;
                        if (l1sInfo.rdate.Length == 14)
                        {
                            strItems[1] = l1sInfo.rdate.Substring(0, 4)
                                + "/" + l1sInfo.rdate.Substring(4, 2)
                                + "/" + l1sInfo.rdate.Substring(6, 2)
                                + " " + l1sInfo.rdate.Substring(8, 2)
                                + ":" + l1sInfo.rdate.Substring(10, 2)
                                + ":" + l1sInfo.rdate.Substring(12, 2);
                        }
                        else
                        {
                            strItems[1] = l1sInfo.rdate;
                        }
                        strItems[2] = l1sInfo.MT;
                        string dc = l1sInfo.Dc;
                        if ("44".Equals(l1sInfo.MT))
                        {
                            dc = "";
                        }

                        strItems[3] = Program.m_L1sConv.getName("MT.mt00", l1sInfo.MT + dc);

                        // 未読項目はBold体で表示する
                        ListViewItem listItem = new ListViewItem(strItems);

                        if (l1sInfo.readflg == 0)
                        {
                            Font nowFont = listMsg.Font;
                            listItem.Font = new Font(nowFont, nowFont.Style | FontStyle.Bold);
                        }

                        // リストアイテムを追加する
                        listMsg.Items.Add(listItem);
                    }
                }
                else if (type == 2)
                {
                    if (Program.m_objActiveTermial.gid != null)
                    {
                        if (!Program.m_objActiveTermial.gid.Equals(""))
                        {
                            if (!Program.m_objDbAccess.GetRescueMsgInfo(Program.m_objActiveTermial.gid, int.Parse(Program.m_objActiveTermial.smid), ref m_RescueList))
                            {
                                // DBエラーにて取得できなかった場合
                                return;
                            }
                            if (m_RescueList.Count == 0)
                            {
                                // データなし
                                return;
                            }
                            // リスト件数MAX20件分表示する（表示優先順はGetRescueMsgInfoにて設定)
                            if (m_RescueList.Count >= LISTVIEW_MAX)
                            {
                                listnum = LISTVIEW_MAX;
                            }
                            else
                            {
                                listnum = m_RescueList.Count;
                            }

                            for (int idx = 0; idx < listnum; idx++)
                            {
                                DbAccessStep2.RescueMsgInfo RescueInfo = m_RescueList[idx];

                                string[] strItems = new string[4];
                                strItems[0] = RescueInfo.id;
                                if (RescueInfo.rdate.Length == 14)
                                {
                                    strItems[1] = RescueInfo.rdate.Substring(0, 4)
                                        + "/" + RescueInfo.rdate.Substring(4, 2)
                                        + "/" + RescueInfo.rdate.Substring(6, 2)
                                        + " " + RescueInfo.rdate.Substring(8, 2)
                                        + ":" + RescueInfo.rdate.Substring(10, 2)
                                        + ":" + RescueInfo.rdate.Substring(12, 2);

                                    // UTCをJST(UTC+9)に変換
                                    TimeZoneInfo jstZoneInfo = System.TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
                                    DateTime jst = System.TimeZoneInfo.ConvertTimeFromUtc(DateTime.Parse(strItems[1]), jstZoneInfo);
                                    strItems[1] = jst.ToString();
                                }
                                else
                                {
                                    strItems[1] = RescueInfo.rdate;
                                }
                                if (RescueInfo.dohoflg == 1)
                                {
                                    strItems[2] = "同報";
                                }
                                else
                                {
                                    strItems[2] = "個別";
                                }

                                if (RescueInfo.Dt == 1)    //テキスト表示
                                {
                                    strItems[3] = RescueInfo.message;
                                }
                                else                      //バイナリ表示
                                {
                                    strItems[3] = "バイナリデータ" + (RescueInfo.msglength) + "byte";
                                }

                                // 未読項目はBold体で表示する
                                ListViewItem listItem = new ListViewItem(strItems);
                                if (RescueInfo.readflg == 0)
                                {
                                    Font nowFont = listMsg.Font;
                                    listItem.Font = new Font(nowFont, nowFont.Style | FontStyle.Bold);
                                }

                                // リストアイテムを追加する
                                listMsg.Items.Add(listItem);
                            }
                        }
                    }
                }
                else
                {
                    // N/A
                }
            }
        }

        /// <summary>
        /// 新着確認ボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnType3_Click(object sender, EventArgs e)
        {
            if (Program.m_EquStat.isConnected() == false)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "btnType3_Click", "未接続です");
                MessageBox.Show("Q-ANPIターミナル未接続の為、救助支援情報の新着確認はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Program.m_SendFlag != Program.NOT_SENDING)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormRegistShelter", "btnType3_Click", "送信中です");
                MessageBox.Show("他の機能でメッセージ送信中の為、救助支援情報の新着確認はできません。", "避難所情報システム", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // type130初回受信フラグを初期化
            m_first130Get = true;

            // type3メッセージ最大数を初期化
            m_Type3CountMax = 1;

            loadList(2);
            RtnRescue_Event(sender, "");
        }
        /// <summary>
        /// バイナリ保存イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutputBinary_Click(object sender, EventArgs e)
        {
            if (m_selected == LISTVIEW_L1S)
            {
                // 救助支援情報のみのため災害危機通報選択中は抜ける
                return;
            }

            // ファイルにデータを保存する
            if (listMsg.SelectedItems.Count > 0)
            {
                int idx = listMsg.SelectedItems[0].Index;
                byte[] bitD = new byte[m_RescueList[idx].msglength];
                bool success = false;

                for (int i = 0; i < bitD.Length; i++)
                {
                    bitD[i] = m_RescueList[idx].bitMessage[i];
                }

                // ファイル名（yyyymmdd_hhmmss.bin）
                string filename = "./"
                    + m_RescueList[idx].rdate.Substring(0, 8)
                    + "_"
                    + m_RescueList[idx].rdate.Substring(8)
                    + ".bin";

                filename = System.IO.Path.GetFullPath(filename);

                try
                {
                    File.WriteAllBytes(filename, bitD);
                    success = true;
                }
                catch (Exception ex)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormReceiveMessageView", "btnOutputBinary_Click", ex.Message);
                }

                if (success)
                {
                    // 保存成功
                    MessageBox.Show("メッセージバイナリを保存しました。\r\n" + filename);
                }
                else
                {
                    // 保存失敗
                    MessageBox.Show("メッセージバイナリの保存に失敗しました");
                }
            }
        }

        /// <summary>
        /// 任意の避難者数入力テキストボックス制御処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textPersonCnt_KeyPress(object sender, KeyPressEventArgs e)
        {
            //0～9と、バックスペース以外の時は、イベントをキャンセルする
            if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
            }

            // 制御文字は入力可
            if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
        }

        /// <summary>
        /// 現在選択中の避難所を取得する
        /// </summary>
        /// <returns></returns>
        public DbAccess.TerminalInfo GetActiveTerminalInfo()
        {
            return m_ActiveTarminal_info;
        }

        /// <summary>
        /// 送信リクエスト番号を初期化
        /// </summary>
        public void ResetSendWaitingNo()
        {
            m_nowSendFinNo = 0;
            m_nowWaitNo = 0;
        }

        /// <summary>
        /// 送信データを文字列化する
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string GetByteArrayString(byte[] data)
        {
            string retStr = "";
            foreach (var item in data)
            {
                retStr = retStr + item.ToString() + " ";
            }

            return retStr;
        }
    }


    /// <summary>
    /// ListViewの項目の並び替えに使用するクラス
    /// </summary>
    public class ListViewItemComparer : IComparer
    {
        private int _column;
        private int _order;

        /// <summary>
        /// ListViewItemComparerクラスのコンストラクタ
        /// </summary>
        /// <param name="col">並び替える列番号</param>
        /// <param name="order">オーダー順(昇順/降順)</param>
        public ListViewItemComparer(int col, int order)
        {
            _column = col;
            _order = order;
        }

        //xがyより小さいときはマイナスの数、大きいときはプラスの数、
        //同じときは0を返す
        public int Compare(object x, object y)
        {
            //ListViewItemの取得
            ListViewItem itemx = (ListViewItem)x;
            ListViewItem itemy = (ListViewItem)y;
            Int64 xInt, yInt;
            // ソート指定カラムが「年齢」の場合、数値としてソート
            if (_column == 4)
            {
                Int64.TryParse(itemx.SubItems[_column].Text, out xInt);
                Int64.TryParse(itemy.SubItems[_column].Text, out yInt);
                //xとyを数値として比較する
                if (_order == 1)
                {
                    // 昇順
                    int retint = 0;
                    if (yInt - xInt > 0)
                    {
                        retint = 1;
                    }
                    else
                    {
                        retint = -1;
                    }
                    return retint;
                }
                else if (_order == 2)
                {
                    // 降順
                    int retint = 0;
                    if (xInt - yInt > 0)
                    {
                        retint = 1;
                    }
                    else
                    {
                        retint = -1;
                    }
                    return retint;
                }
                else
                {
                    // ソート無し
                    return 0;
                }
            }
            // ソート指定カラムが「年齢」以外の場合、文字列としてソート
            else
            {
                //xとyを文字列として比較する
                if (_order == 1)
                {
                    // 昇順
                    return string.Compare(itemx.SubItems[_column].Text,
                        itemy.SubItems[_column].Text);
                }
                else if (_order == 2)
                {
                    // 降順
                    return string.Compare(itemy.SubItems[_column].Text,
                        itemx.SubItems[_column].Text);
                }
                else
                {
                    // ソート無し
                    return 0;
                }
            }
        }
    }
}
