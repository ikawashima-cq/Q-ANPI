using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ShelterInfoSystem
{
    public static class Qcid
    {
        /**
         * @brief CID取得
         * @param : qcid : 端末ID（QCID） 
         */
        public static int convCID(string qcid)
        {
            int ret = -1;

            // 端末IDから CID に変換 
            // BF 基本周波数ID BC 基本PN符号（0-399）
            if (qcid == null || qcid == "")
            {
                // まだきていない or ない
                // エラー
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                    "Qcid", "getCID", "端末ID未設定エラー");
                return ret;
            }

            qcid = qcid.Replace("-", "");
            if (qcid.Length != 8)
            {
                // Length不正
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                    "Qcid", "getCID", "端末ID桁数エラー");
                return ret;
            }

            // 文字をBFの数字に変換
            // BF_n=N_1×24^3+N_2×24^2+N_3×24+N_4
            string bf_moji = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            int N1 = bf_moji.IndexOf(qcid.Substring(0, 1));
            int N2 = bf_moji.IndexOf(qcid.Substring(1, 1));
            int N3 = bf_moji.IndexOf(qcid.Substring(2, 1));
            int N4 = bf_moji.IndexOf(qcid.Substring(3, 1));

            int bf = N1 * (24 * 24 * 24) + N2 * (24 * 24) + N3 * (24) + N4;

            // BCの数字を
            string strBC = qcid.Substring(4, 3);
            int bc = 0;
            try
            {
                bc = int.Parse(strBC);
            }
            catch (Exception)
            {
                // 数値にできない
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                    "TcpFwdThread_Event", "checkType100", "端末ID(BC)Not数値エラー");
                return ret;
            }

            // 前16:bf 後9:bc
            int cid = (bf << 9) + bc;

            ret = cid;
            return ret;
        }

        /**
         * @brief GID取得
         * @param : qcid : 端末ID（QCID） 
         * @param : startFreq : 開始周波数
         * @param : endFreq : 終了周波数
         */
        public static int convGID(string qcid, int startFreq, int endFreq)
        {
            int ret = 0;
            // 端末IDから CID に変換 
            // BF 基本周波数ID BC 基本PN符号（0-399）
            if (qcid == null || qcid == "")
            {
                // まだきていない or ない
                // エラー
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                    "Qcid", "getGID", "端末ID未設定エラー");
                return ret;
            }

            qcid = qcid.Replace("-", "");
            if (qcid.Length != 8)
            {
                // Length不正
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                    "Qcid", "getGID", "端末ID桁数エラー");
                return ret;
            }

            // 文字を基本周波数ID（BF：Base Frequency ID）の数字に変換
            // BF_n=N_1×24^3+N_2×24^2+N_3×24+N_4
            string bf_moji = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            int N1 = bf_moji.IndexOf(qcid.Substring(0, 1));
            int N2 = bf_moji.IndexOf(qcid.Substring(1, 1));
            int N3 = bf_moji.IndexOf(qcid.Substring(2, 1));
            int N4 = bf_moji.IndexOf(qcid.Substring(3, 1));

            int bf = N1 * (24 * 24 * 24) + N2 * (24 * 24) + N3 * (24) + N4;

            // 
            if (endFreq - startFreq + 1 == 0)
            {
                // 0で割れない
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                    "Qcid", "getGID", "endFreq - startFreq + 1 == 0エラー");
                return ret;
            }
            else if (endFreq - startFreq + 1 < 0)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR,
                    "Qcid", "getGID", "endFreq - startFreq + 1 < 0エラー");
                return ret;
            }
            else
            {
                // OK
            }

            ret = (int)(bf / (endFreq - startFreq + 1));

            return ret;
        }

    }
}
