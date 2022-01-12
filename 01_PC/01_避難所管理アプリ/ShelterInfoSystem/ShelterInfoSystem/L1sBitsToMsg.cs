/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2017. All rights reserved.
 */
/**
 * @file    L1sBitsToMsg.cs
 * @brief   災害危機通報表示メッセージ変換クラス
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ShelterInfoSystem
{
    /**
     * @class L1sBitsToMsg
     * @brief 災害危機通報表示メッセージ変換クラス
     */
    public class L1sBitsToMsg
    {
        private static L1sParser m_Parser;

        /**
         * @brief 初期化　./format.csvファイル読み込み
         */
        public void init()
        {
            //  テストデータ
            string csvText = "";
            string filename = "./parser/l1s_parser.csv";
            string csvKeys = "";
            string csvBits = "";

            m_Parser = new L1sParser();

            try
            {
                StreamReader sr = new StreamReader(
                    filename, Encoding.GetEncoding("Shift_JIS"));

                csvText = sr.ReadToEnd();

                sr.Close();
            }
            catch (Exception ex)
            {
                // err
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "L1sBitsToMsg", "init1 " + ex.Message);
            }

            try
            {
                csvText = csvText.Replace("\r", "");
                csvText = csvText.Replace("\"", "");
                string[] csvTexts = csvText.Split('\n');
                for (int line = 0; line < csvTexts.Length - 1; line += 2)
                {
                    csvKeys = csvTexts[line];
                    csvBits = csvTexts[line + 1];
                    int firstspliter = csvKeys.IndexOf(",");

                    if (firstspliter >= 0)
                    {
                        string fname = csvKeys.Substring(0, firstspliter);
                        m_Parser.setFormat(fname, csvKeys, csvBits); // 2行ごとにくりかえす
                    }
                }
            }
            catch (Exception ex)
            {
                // err
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "L1sBitsToMsg", "init2 " + ex.Message);
            }
        }

        /**
         * @brief ビットデータから災害危機通報メッセージを作成する
         * @param バイナリ情報
         * @return 災危通報メッセージ
         */
        public string getMsg(byte[] bits)
        {
            string msgText = "";

            m_Parser.setBinary(bits);

            // ヘッダからタイプ
            string fname = m_Parser.getTypeRc(bits);
            fname = "./template/" + fname + ".tpl";

            // tpl
            string tpl = getTpl(fname);

            // 置き換え数字・文字取得
            int pos = 0;
            int oldpos = 0;
            while (true)
            {
                int keyposstart = tpl.IndexOf("%%", pos);
                if (keyposstart >= 0)
                {
                    msgText += tpl.Substring(pos, keyposstart - pos);
                    int keyposend = tpl.IndexOf("%%", keyposstart+1);

                    string key = tpl.Substring(keyposstart+2,keyposend-keyposstart-2);

                    // %%key%% が Ev か t43_1_Ev.Ev か
                    string parserkey = key;
                    if (key.IndexOf('.') <= 0)
                    {
                        // Evのみ　数字そのまま
                    }
                    else
                    {
                        // t43_1_Ev.Ev
                        string[] pa = key.Split('.');
                        parserkey = pa[0];
                        //key = pa[0];
                    }

                    // 特殊な処理
                    if (key == "mt43_1_Pl") {
                        string chng = make43_1_PlMessage(bits);
                        msgText += chng;
                    }
                    else if (key == "mt43_4_Te")
                    {
                        string chng = make43_4_TeMessage(bits);
                        msgText += chng;
                    }
                    else if (key.StartsWith("mt43_5_Ta_"))
                    {
                        string str = "";
                        string num = key.Substring(10); // 後ろの数字
                        string chkD = m_Parser.getValue("TaD_" + num);
                        string chkH = m_Parser.getValue("TaH_" + num);
                        string chkM = m_Parser.getValue("TaM_" + num);
                        if (chkH == "31" && chkM == "63")
                        {
                            str = "津波到達有";
                        }
                        else
                        {
                            if (chkD == "1")
                            {
                                str += "（翌日）";
                            }
                            else
                            {
                                str += "（同日）";
                            }
                            str += chkH + ":" + chkM + "\r\n";
                        }
                        msgText += str;
                    }
                    else if (key.IndexOf("mt43_11_Pl") > 0)
                    {
                        string strVal = m_Parser.getValue(parserkey);
                        string chng = Program.m_L1sConv.getName(key, strVal);
                        if (strVal == "" || strVal == chng)
                        {
                            // 未定義
                            // 「○○県の河川(コード番号:%d)」
                            string strCode = "(コード番号:" + strVal + ")";
                            if (strVal.Length == 11)
                            {
                                strVal = strVal.Substring(0, 1);
                            }
                            else if (strVal.Length > 2)
                            {
                                strVal = strVal.Substring(0, 2);
                            }
                            else
                            {
                                // N/A
                            }

                            string chng2 = Program.m_L1sConv.getName("Pl.mt43_11_Pl_Unknown", strVal);
                            if (chng == strVal)
                            {
                                chng = "";
                            }
                            chng += chng2;

                            chng = chng2 + strCode;
                        }
                        msgText += chng;
                    }
                    else
                    {

                        string strVal = m_Parser.getValue(parserkey);

                        // 変換するものはして置き換え
                        string chng = Program.m_L1sConv.getName(key, strVal);

                        chng = chng.Replace("\\n", "\r\n");     // 変換後文字列の改行を設定する

                        msgText += chng;
                    }

                    pos = keyposend+2;
                }
                else
                {
                    msgText += tpl.Substring(oldpos);
                    break;
                }

                oldpos = pos;
            }

            return msgText;
    	}

        /**
         * @brief バイナリ情報からL1S情報取得
         * @param バイナリ情報
         * @return L1S情報
         */
        public DbAccessStep2.RecvMsgInfo getL1sInfo(byte[] bits)
        {
            DbAccessStep2.RecvMsgInfo info = new DbAccessStep2.RecvMsgInfo();

            string[] header = m_Parser.getHeader(bits);
            info.id = Program.m_objDbAccess.GetRecvMsgNextId().ToString();
            info.MT = header[1];
            info.Dc = header[3];
            info.Rc = header[2];
            info.rdate = DateTime.Now.ToString("yyyyMMddHHmmss");       // 端末の送信時刻を設定する場合は上書きすること

            info.bitdata = bits;

            return info;
        }

        /**
         * @brief 表示テンプレートを取得する
         * @param テンプレートファイル名
         * @return テンプレート文字列
         */
        private string getTpl(string filename){
            string tpl = "";

            try
            {
                StreamReader sr = new StreamReader(
                    filename, Encoding.GetEncoding("Shift_JIS"));

                tpl = sr.ReadToEnd();

                sr.Close();
            }
            catch(Exception ex)
            {
                // err
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "L1sBitsToMsg", "getTpl " + ex.Message);
            }
            return tpl;
        }

        // 特殊処理

        // 80あるうちbitたってる地方をすべて表示
        private string make43_1_PlMessage(byte[] bits)
        {
            string ret = "";
            byte[] data = new byte[32];

            m_Parser.reverse(bits, data);

            // data[16]の下6bitsからdata[26]の途中まで
            int idx = 1;
            byte mask = 0x20;
            int aaidx = 16;
            byte aa = data[aaidx];
            string stidxmax = m_Parser.getFormat( "mt43_1", "Pl");
            int idxmax = 80;
            try
            {
                idxmax = int.Parse(stidxmax);
            }
            catch (Exception ex)
            {
                // err
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "L1sBitsToMsg", "make43_1_PlMessage " + ex.Message);
            }

            for (idx = 1; idx <= idxmax; idx++)
            {
                if ((aa & mask) > 0)
                {
                    // bit たってる県は全部表示
                    string chng = Program.m_L1sConv.getName("Pl.mt43_1_Pl", idx.ToString());
                    ret += chng + "\r\n";
                }
                mask >>= 1;
                if (mask == 0)
                {
                    mask = 0x80;
                    aaidx++;
                    aa = data[aaidx];
                }
            }
            return ret;
        }

        // UTF-8で
        private string make43_4_TeMessage(byte[] bits)
        {
            string ret = "";
            byte[] data = new byte[32];

            m_Parser.reverse(bits, data);

            // 表示情報（情報1）から（情報18）まで
            for (int i = 0; i < 18; i++)
            {
                data[i+7] <<= 1;
                data[i+7] |= (byte)((data[i+7+1] >> 7) & 0x01);
            }
            byte[] moji = new byte[18];
            Array.Copy(data, 7, moji, 0, 18);

            ret = System.Text.Encoding.UTF8.GetString(moji);

            return ret;
        }
    }
    
}

