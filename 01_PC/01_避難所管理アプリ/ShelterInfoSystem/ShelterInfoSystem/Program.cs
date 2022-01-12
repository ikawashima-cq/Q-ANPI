using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;

namespace ShelterInfoSystem
{
    static class Program
    {
        // データベースアクセス用クラス
// step2 L1S解析処理追加のためDBクラス改修
        public static DbAccessStep2 m_objDbAccess = null;
        // QZSS設定
        // public static QzssConfig m_objQzssConfig = null;
        // SHELTER設定
        public static ShelterConfig m_objShelterAppConfig = null;
        // ログ用スレッド
        public static LogThread m_thLog = null;
        // 現在アクティブな避難所
        public static DbAccess.TerminalInfo m_objActiveTermial;
        // メインフォーム 
        public static FormShelterInfo m_mainForm = null; 
        // 2016/04/21
        // ログ出力パス　
        public static　string m_sCurrentLogPath = "";

// step2 TCP/SubGHz 切り替え 1:TCP 2:SubG
        public static int m_ConnectMethod = 2;

// step2 TCP接続設定追加
        public static String m_TcpIpAddr = "";
        public static TcpConfig m_objTcpConfig = null;
        public static L1sReceiver m_L1sRecv;
        public static FwdReceiver m_FwdRecv;
        public static TermInfo m_EquStat;
        public static RtnThread m_RtnThreadSend;

        // 自動送信処理
        public static AutoSendThread m_AutoThreadSend;

// step2 L1S解析処理追加
        public static L1sBitsToMsg m_BitsToMsg = null;
        public static L1sFormat m_L1sConv = null;
// step2 SubGiga設定追加
        public static SubGHzConfig m_SubGHzConfig = null;
        public static SubGHz m_SubGHz = null;

        public static bool m_statRecv = false;

        // 送信フラグ：送信中でない
        public static readonly int NOT_SENDING = 0;
        // 送信フラグ：個人安否送信中
        public static readonly int SENDING_PERSON_INFO = 1;
        // 送信フラグ：避難所情報送信中
        public static readonly int SENDING_SHELTER_INFO = 2;
        // 送信フラグ：開設/閉鎖/端末情報送信中
        public static readonly int SENDING_TERMINAL_INFO = 3;
        // 送信フラグ：初回疎通通信送信中
        public static readonly int SENDING_INITIALIZE_INFO = 4;
        // 送信フラグ
        public static int m_SendFlag = NOT_SENDING;


        // 時刻同期要求用送信時刻保持
        public static DateTime m_sendTime;

        // 自動送信時刻設定保持
        public static List<AutoSendThread.AutoSendSetting> m_AutoSendSetting;

        // 受信状態設定用
        //public static FormReceiveMessageView m_ReceiveMessageView;

        // 仮Q-ANPIターミナルID
        public const string TEMP_QANPI_ID = "######";

        // 仮避難所ID
        public const string TEMP_SHELTER_ID = "XXXYYYZZZ";

        // 時刻同期用　現在の年保持
        public static int m_nowYear = 0;

        // 時刻同期用　現在の月       //2020.03.25 Add
        public static int m_nowMonth = 0;

        struct GidCnv
        {
            public char ch;
            public string sVal;

            public GidCnv(char chSrc, string sValSrc)
            {
                ch = chSrc;
                sVal = sValSrc;
            }
        }

        private static GidCnv[] m_GidCnvList = 
        {
            new GidCnv( 'A', "0000" ),
            new GidCnv( 'B', "0001" ),
            new GidCnv( 'C', "0010" ),
            new GidCnv( 'D', "0011" ),
            new GidCnv( 'E', "0100" ),
            new GidCnv( 'F', "0101" ),
            new GidCnv( 'G', "0110" ),
            new GidCnv( 'H', "0111" ),
            new GidCnv( 'J', "1000" ),
            new GidCnv( 'K', "1001" ),
            new GidCnv( 'L', "1010" ),
            new GidCnv( 'M', "1011" ),
            new GidCnv( 'N', "1100" ),
            new GidCnv( 'P', "1101" ),
            new GidCnv( 'Q', "1110" ),
            new GidCnv( 'R', "1111" ),
        };


        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 2016/04/21
            // ログ出力パス　
            m_sCurrentLogPath = System.IO.Directory.GetCurrentDirectory() + "\\Log";

            m_thLog = new LogThread();
            // ログスレッド起動
            m_thLog.Start();

            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "Main", "", "避難所管理アプリ開始");


            // QZSS設定
            // m_objQzssConfig = new QzssConfig();
            // m_objQzssConfig .Load();

            // SHELTER設定
            m_objShelterAppConfig = new ShelterConfig();
            m_objShelterAppConfig.Load();

            // 生成と接続
//            m_objDbAccess = new DbAccess();
//            m_objDbAccess.Connect();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

// step2 L1S解析処理追加
            m_BitsToMsg = new L1sBitsToMsg();
            m_BitsToMsg.init();

            m_L1sConv = new L1sFormat();
            m_L1sConv.load();

            m_EquStat = new TermInfo();
            m_L1sRecv = new L1sReceiver();
            m_FwdRecv = new FwdReceiver();
            m_RtnThreadSend = new RtnThread();

            // 自動送信処理
            m_AutoThreadSend = new AutoSendThread();

// step2 TCP接続設定追加
// step2 SubGHz 設定追加
            m_SubGHzConfig = new SubGHzConfig();
            m_SubGHzConfig.Load();
            if (m_SubGHzConfig.Enable != null && m_SubGHzConfig.Enable.ToUpper() == "FALSE")
            {
                m_objTcpConfig = new TcpConfig();
                m_objTcpConfig.Load();
                m_ConnectMethod = 1; // TCPモード
                m_EquStat.m_Mode = TermInfo.Mode.TCPIP;
                m_L1sRecv.m_Mode = L1sReceiver.Mode.TCPIP;
                m_FwdRecv.m_Mode = FwdReceiver.Mode.TCPIP;
                m_RtnThreadSend.m_Mode = RtnThread.Mode.TCPIP;
            }
            else
            {
                m_ConnectMethod = 2; // サブギガモード
                m_EquStat.m_Mode = TermInfo.Mode.SUBGHZ;
                m_L1sRecv.m_Mode = L1sReceiver.Mode.SUBGHZ;
                m_FwdRecv.m_Mode = FwdReceiver.Mode.SUBGHZ;
                m_RtnThreadSend.m_Mode = RtnThread.Mode.SUBGHZ;
            }

            m_SubGHz = SubGHz.GetInstance();
            m_SubGHz.Start();

            Application.Run(new FormLogin());

            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "Main", "", "避難所管理アプリ終了");

            // ログスレッド終了
            m_thLog.Exit();

// step2 TCPスレッド処理終了
            //m_L1sRecv.Exit();
            m_EquStat.Exit();
            m_SubGHz.Exit();
            m_RtnThreadSend.Exit();

            if (m_objDbAccess != null)
            {
                m_objDbAccess.Disconnect();
            }
        }

        // 性別       0:男性 1:女性
        public static string ConvSel01(string sSel)
        {
            string sRet = "";
            switch (sSel)
            {
                case "0":
                    sRet = "男";
                    break;
                case "1":
                    sRet = "女";
                    break;
            }
            return sRet;
        }

        // 入/退所  0:入所 1:退所 2:在宅
        public static string ConvSel02(string sSel)
        {
            string sRet = "";
            switch (sSel)
            {
                case "0":
                    sRet = "入";
                    break;
                case "1":
                    sRet = "退";
                    break;
                case "2":
                    sRet = "在";
                    break;
            }
            return sRet;
        }

        // 公表       0:しない 1:する
        public static string ConvSel03(string sSel)
        {
            string sRet = "";
            switch (sSel)
            {
                case "0":
                    sRet = "否";
                    break;
                case "1":
                    sRet = "可";
                    break;
            }
            return sRet;
        }

        // 怪我       0:無し 1:有り 2:未使用 3:未選択
        public static string ConvSel04(string sSel)
        {
            string sRet = "";
            switch (sSel)
            {
                case "0":
                    sRet = "無";
                    break;
                case "1":
                    sRet = "有";
                    break;
                case "3":
                    sRet = "";
                    break;
                default:
                    break;
            }
            return sRet;
        }

        // 2016/04/18 救護を削除：これ以降を前詰
        //// 救護       2:未選択 0:否  1:要
        //public static string ConvSel05(string sSel)
        //{
        //    string sRet = "";
        //    switch (sSel)
        //    {
        //        case "0":
        //            sRet = "否";
        //            break;
        //        case "1":
        //            sRet = "要";
        //            break;
        //        case "2":
        //            sRet = "";
        //            break;
        //    }
        //    return sRet;
        //}

        // 介護       2:未選択 0:否  1:要
        public static string ConvSel05(string sSel)
        {
            string sRet = "";
            switch (sSel)
            {
                case "0":
                    sRet = "否";
                    break;
                case "1":
                    sRet = "要";
                    break;
                case "2":
                    sRet = "";
                    break;
            }
            return sRet;
        }

        // 障がい    2:未選択 0:無  1:有
        public static string ConvSel06(string sSel)
        {
            string sRet = "";
            switch (sSel)
            {
                case "0":
                    sRet = "無";
                    break;
                case "1":
                    sRet = "有";
                    break;
                case "2":
                    sRet = "";
                    break;
            }
            return sRet;
        }

        // 妊産婦    2:未選択 0:いいえ  1:はい
        public static string ConvSel07(string sSel)
        {
            string sRet = "";
            switch (sSel)
            {
                case "0":
                    sRet = "いいえ";
                    break;
                case "1":
                    sRet = "はい";
                    break;
                case "2":
                    sRet = "";
                    break;
            }
            return sRet;
        }

        // 避難所内外    0:内  1:外
        public static string ConvSel08(string sSel)
        {
            string sRet = "";
            switch (sSel)
            {
                case "0":
                    sRet = "内";
                    break;
                case "1":
                    sRet = "外";
                    break;
                default:
                    sRet = "内";
                    break;
            }
            return sRet;
        }

        public static void EnableDoubleBuffering(Control control)
        {
            control.GetType().InvokeMember(
               "DoubleBuffered",
               BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
               null,
               control,
               new object[] { true });
        }

        public static string ConvGidSendData()
        {
            string retGid = "000000000001";
            QAnpiCtrlLib.msg.SystemInfo sysInfo = Program.m_FwdRecv.m_sysInfo;
            // GIDを計算
            string qcid = Program.m_EquStat.mQCID;

            if (qcid == null || sysInfo == null
                || sysInfo.sysRandomSelectBand == 0
                || sysInfo.sysGroupNum == 0)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "ConvGidSendData", "QCID or SystemInfo null");
                // まだFWDを受け取っていない
                return retGid;
            }

            int startFreq = sysInfo.sysStartFreqId;
            int endFreq = sysInfo.sysEndFreqId;
            int gid = Qcid.convGID(qcid, startFreq, endFreq);
            string sGID = "";
            try
            {
                int GID_LENGTH = 12;
                for (int i = 0; i < GID_LENGTH; i++)
                {
                    int bitval = gid & 0x01;
                    if (bitval == 0)
                    {
                        sGID = "0" + sGID;
                    }
                    else
                    {
                        sGID = "1" + sGID;
                    }
                    gid /= 2;
                }
                retGid = sGID;
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS,
                                "Program", "SysInfo", "err " + ex.Message);
            }

            return retGid;

#if false
            string sRet = "";
            //★
            //char[] cGid = Program.m_objShelterConfig.GID.ToCharArray();
            char[] cGid = Program.m_objTermial.gid.ToCharArray();

            int nIdx;
            for (nIdx = 0; nIdx < 3; nIdx++)
            {
                if (cGid.Length > nIdx)
                {
                    int nCnvIdx;
                    for (nCnvIdx = 0; nCnvIdx < m_GidCnvList.Length; nCnvIdx++)
                    {
                        if (m_GidCnvList[nCnvIdx].ch == cGid[nIdx])
                        {
                            sRet += m_GidCnvList[nCnvIdx].sVal;
                            break;
                        }
                    }
                }
            }

            return sRet;
#endif
        }
    }
}
