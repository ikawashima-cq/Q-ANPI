/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2017. All rights reserved.
 */
/**
 * @file    L1sReceiver.cs
 * @brief   L1Sの受信クラス
 */
using System;
using System.Net;
using System.IO;
using System.Diagnostics;       // Debug.WriteLine用

using QAnpiCtrlLib.log;
using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.msg;

using ShelterInfoSystem.control;

namespace ShelterInfoSystem
{
    /**
     * @class TcpL1sReceiver
     * @brief L1S/CIDのTcp受信クラス
     */
    public class L1sReceiver 
    {
        /**
         * @brief 結果定義
         */
        public enum Result
        {
            OK = 0,
            NG,
            NOT_OK,
            NOT_NG,
        };

        public enum Mode
        {
            TCPIP = 0,
            SUBGHZ,
        };

        public Mode m_Mode = Mode.TCPIP;

        // TcpとSubGHz
        TcpL1sReceiver m_TcpL1s = new TcpL1sReceiver();
        SubGHzL1sReceiver m_SGL1s = new SubGHzL1sReceiver();

        // L1Sメッセージ取得要求
        public const int MSG_TYPE_L1S = 40;

        // 端末ID取得要求
        public const int MSG_TYPE_CID = 41;

        // 接続情報
        //private static byte[] connectedIp = {0,0,0,0};
        //private static int connectedPort = 0;
        public bool isSaved = true;

        /// <summary>
        /// L1Sデータ（L1Sデータ送信要求）
        /// </summary>
        private MsgL1sGetReq m_L1sGetReq = new MsgL1sGetReq(MSG_TYPE_L1S);


        private MsgSendBase msb = new MsgSendBase();
        private L1sParser m_Parse = new L1sParser();

        /**
         * @brief L1S応答デリゲート
         */
        public delegate void EventL1sDelegate(object sender, int code, byte[] msg, string outVal);
        public event EventL1sDelegate EventL1sResp
        {
            add
            {
                if (m_Mode == Mode.TCPIP)
                {
                    m_TcpL1s.EventL1sResp += value;
                }
                else
                {
                    // SubGHZ
                    m_SGL1s.EventL1sResp += value;
                    Program.m_SubGHz.rcvL1sDataEvent += new SubGHz.EventL1sDelegate(m_SGL1s.rcvL1sDataEvent);
                }
            }
            remove
            {
                if (m_Mode == Mode.TCPIP)
                {
                    m_TcpL1s.EventL1sResp -= value;
                }
                else
                {
                    // SubGHZ
                    m_SGL1s.EventL1sResp -= value;
                    Program.m_SubGHz.rcvL1sDataEvent -= new SubGHz.EventL1sDelegate(m_SGL1s.rcvL1sDataEvent);
                }
            }
        }

        /**
         * @brief 接続処理(文字列指定)
         * @param 接続先IPアドレス
         * @param 接続ポート
         * @return 接続結果
         */
        public Result connect(string ipAddr, int port)
        {
            if (m_Mode == Mode.TCPIP)
            {
                return m_TcpL1s.connect(ipAddr, port);
            }
            else
            {
                return m_SGL1s.connect(ipAddr, port);
            }
            
        }

        /**
         * @brief 接続処理(Byte配列指定)
         * @param 接続先IPアドレス
         * @param 接続ポート
         * @return 接続結果
         */
        public Result connect(byte[] ipBytes, int port)
        {
            if (m_Mode == Mode.TCPIP)
            {
                return m_TcpL1s.connect(ipBytes, port);
            }
            else
            {
                return Result.NG;
            }
        }

        public bool isConnected()
        {
            if (m_Mode == Mode.TCPIP)
            {
                return m_TcpL1s.isConnected();
            }
            else
            {
                return m_SGL1s.isConnected();
            }
        }

        /// <summary>
        /// データ取得要求 (L1S以外の汎用)
        /// </summary>
        public void sendReq(int reqcode)
        {
            if (m_Mode == Mode.TCPIP)
            {
                m_TcpL1s.sendReq(reqcode);
            }
            else
            {
                // SubGHZ
                m_SGL1s.sendReq(reqcode);
            }
        }

        /**
         * @brief 端末ID、利用機関ID、緯度経度デコード
         */
        public static void readCidInfo(byte[] encodedData)
        {
            int startPos = 44; // ヘッダを飛ばす
            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "L1sReceiver", "readCidInfo", "read start");

            // 端末ID
            char[] qcid = new char[8];
            for (int i = 0; i < qcid.Length; i++)
            {
                qcid[i] = (char)encodedData[startPos + i];
            }
            startPos += qcid.Length + 1;

            string sQCID = new string(qcid);

            // 端末ID（CID）
            int iCID = Qcid.convCID(sQCID);

            // 利用機関ID_1
            char[] orgid1 = new char[6];
            for (int i = 0; i < orgid1.Length; i++)
            {
                orgid1[i] = (char)encodedData[startPos + i];
            }
            startPos += orgid1.Length + 1;

            // 利用機関ID_2
            char[] orgid2 = new char[6];
            for (int i = 0; i < orgid2.Length; i++)
            {
                orgid2[i] = (char)encodedData[startPos + i];
            }
            startPos += orgid2.Length + 1;

            // 利用機関ID_3
            char[] orgid3 = new char[6];
            for (int i = 0; i < orgid3.Length; i++)
            {
                orgid3[i] = (char)encodedData[startPos + i];
            }
            startPos += orgid3.Length + 1;

            //string sORGID = new string(orgid);
            string[] sORGID = { new string(orgid1), new string(orgid2), new string(orgid3) };

            // 緯度
            byte[] lat = new byte[8];
            for (int i = 0; i < lat.Length; i++)
            {
                lat[i] = (byte)encodedData[startPos + i];
            }
            startPos += lat.Length;

            double dLat = System.BitConverter.ToDouble(lat, 0);

            // 経度
            byte[] lon = new byte[8];
            for (int i = 0; i < lon.Length; i++)
            {
                lon[i] = (byte)encodedData[startPos + i];
            }
            startPos += lon.Length;

            double dLon = System.BitConverter.ToDouble(lon, 0);

            // 上記 sCID, sORGID, dLat, dLon を保持する(とりあえず装置ステータスで)
            Program.m_EquStat.mQCID = sQCID.Replace(" ", "");
            SystemInfo sysInfo = Program.m_FwdRecv.m_sysInfo;

            if (sysInfo == null
                || sysInfo.sysRandomSelectBand == 0
                || sysInfo.sysGroupNum == 0)
            {
                // まだFWDを受け取っていない
            }
            else
            {
                // システム情報の開始終了周波数とQCIDからGIDを計算する
                int startFreq = sysInfo.sysStartFreqId;
                int endFreq = sysInfo.sysEndFreqId;
                int gid = Qcid.convGID(sQCID, startFreq, endFreq);
                Program.m_EquStat.mGID = gid;
            }
            try
            {
                // 利用機関IDは未設定の場合スペース文字(0x20)埋めで送られてくるので、未設定の場合は「0」とする
                if (!int.TryParse(sORGID[0], out Program.m_EquStat.mORGID[0]))
                {
                    Program.m_EquStat.mORGID[0] = 0;
                }
                if (!int.TryParse(sORGID[1], out Program.m_EquStat.mORGID[1]))
                {
                    Program.m_EquStat.mORGID[1] = 0;
                }
                if (!int.TryParse(sORGID[2], out Program.m_EquStat.mORGID[2]))
                {
                    Program.m_EquStat.mORGID[2] = 0;
                }
                Program.m_EquStat.mCID = iCID;
                Program.m_EquStat.mLatitude = Convert.ToDouble(dLat.ToString("0.0000"));        // 小数点4位以下を削除
                Program.m_EquStat.mLongitude = Convert.ToDouble(dLon.ToString("0.0000"));       // 小数点4位以下を削除

                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "L1sReceiver", "readCidInfo",
                    "QCID:" + sQCID + " CID:" + iCID + " ORGID:" + sORGID
                    + " (lat,lon):(" + dLat + "," + dLon + ")");

                // 通信端末のID(QCID)をDBに保持する
                int smid = 0;
                string updateTime = "";
                if (Program.m_objDbAccess.GetRegister_QANPIDeviceInfo(Program.m_EquStat.mQCID, ref smid, ref updateTime))
                {
                    // DBにすでにある場合、更新日時のみ変更
                    Program.m_objDbAccess.UpsertQANPIDeviceInfo(Program.m_EquStat.mQCID, smid);
                }
                else
                {
                    // DBに存在しなかった場合、SMIDは0として新しく追加
                    Program.m_objDbAccess.UpsertQANPIDeviceInfo(Program.m_EquStat.mQCID, 0);
                }

                // DB内に仮IDを使用した避難所が存在する場合、これをQ-ANPIターミナルIDに置き換える
                ReplaceTempID();
                Program.m_mainForm.UpdateShelterInfoList();
                Program.m_mainForm.ShelterStatusView();
                Program.m_mainForm.updateActiveShelterInfo();

                // DB内の登録済みの避難所で接続したQ-ANPIターミナルIDと同じGIDを持つ避難所がある場合、これの利用機関IDを更新する
                DbAccess.TerminalInfo[] AllTernimalInfo = new DbAccess.TerminalInfo[0];
                Program.m_objDbAccess.GetTerminalInfoAll(ref AllTernimalInfo);
                foreach (var item in AllTernimalInfo)
                {
                    if (item.gid == Program.m_EquStat.mQCID)
                    {
                        DbAccess.TerminalInfo tmpInfo = item;
                        Program.m_objDbAccess.UpsertTerminalInfo(tmpInfo);
                    }
                }
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "L1sReceiver:readCidInfo", "[端末情報取得完了] " +
                                                "Q-ANPIターミナルID：" + Program.m_EquStat.mQCID +
                                                " 緯度：" + Program.m_EquStat.mLatitude +
                                                " 経度：" + Program.m_EquStat.mLongitude +
                                                " 利用機関ID_1：" + Program.m_EquStat.mORGID[0] +
                                                " 利用機関ID_2：" + Program.m_EquStat.mORGID[1] +
                                                " 利用機関ID_3：" + Program.m_EquStat.mORGID[2] +
                                                " 通信ID：" + Program.m_EquStat.mCID
                                                );

                // 端末情報取得時、緯度経度が正常でない値(緯度：0.0～90.0 経度：0.0～180.0の範囲外）の時、メッセージを表示し再接続を促す
                // →緯度経度が有効範囲外の場合、地上局側で送達確認を返さない為、エラーとなる
                if ((Program.m_EquStat.mLatitude < 0.0) || (Program.m_EquStat.mLatitude > 90.0) ||
                    (Program.m_EquStat.mLongitude < 0.0) || (Program.m_EquStat.mLongitude > 180.0))
                {
                    System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "L1sReceiver:readCidInfo", "位置情報取得エラー" +
                                                " 緯度：" + Program.m_EquStat.mLatitude +
                                                " 経度：" + Program.m_EquStat.mLongitude
                                                );
                        System.Windows.Forms.MessageBox.Show("位置情報が取得できませんでした。" + System.Environment.NewLine +
                                                            "端末との接続を切断し、再接続してください。"
                                                            , "避難所情報システム",
                                                            System.Windows.Forms.MessageBoxButtons.OK,
                                                            System.Windows.Forms.MessageBoxIcon.Warning,
                                                            System.Windows.Forms.MessageBoxDefaultButton.Button1,
                                                            System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);
                    });
                }

            }
            catch (Exception ex)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "L1sReceiver:readCidInfo", "CID値エラー" + ex.Message);
            }

            // 初回通信での送信中状態を解除
            Program.m_mainForm.m_isTerminalDeviceInfoOK = true;
            Program.m_SendFlag = Program.NOT_SENDING;
            Program.m_mainForm.ShelterStatusView();
        }

        /**
         * @brief L1Sデータ取得要求作成
         */
        private void makeL1sGetReq(int data11)
        {

            if (data11 == MSG_TYPE_CID)
            {
                m_L1sGetReq = new MsgL1sGetReq(MSG_TYPE_CID);
            }
            else
            {
                m_L1sGetReq = new MsgL1sGetReq(MSG_TYPE_L1S);
            }

            // 装置ID
            m_L1sGetReq.eqId = MsgSBandData.FIXED_EQ_ID; ;
            m_L1sGetReq.encode(false);
        }

        /**
         * @brief 切断処理
         */
        public void disconnect()
        {
            if (m_Mode == Mode.TCPIP)
            {
                m_TcpL1s.disconnect();
            }
            else
            {
                m_SGL1s.disconnect();
            }
            //connectedPort = 0;
        }

        /// <summary>
        /// DB内の仮IDで登録している避難所のIDを置き換える
        /// </summary>
        private static void ReplaceTempID()
        {
            DbAccess.TerminalInfo[] allInfo = new DbAccess.TerminalInfo[0];
            Program.m_objDbAccess.GetTerminalInfoAll(ref allInfo);

            foreach (var item in allInfo)
            {
                if (item.gid == Program.TEMP_QANPI_ID)
                {
                    DbAccess.TerminalInfo newInfo = item;

                    // 仮IDのデータを削除
                    Program.m_objDbAccess.DeleteTerminalInfo(newInfo.sid);

                    // 本IDでデータを再登録
                    newInfo.sid = Program.m_EquStat.mQCID + "-" + newInfo.smid.PadLeft(3, '0');
                    newInfo.gid = Program.m_EquStat.mQCID;
                    Program.m_objDbAccess.Insert_TerminalInfo(newInfo);
                }
            }
        }
    }
}

