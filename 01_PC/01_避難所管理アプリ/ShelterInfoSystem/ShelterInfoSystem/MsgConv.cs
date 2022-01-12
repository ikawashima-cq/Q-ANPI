using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ShelterInfoSystem
{
    public static class MsgConv
    {
        // string "00110100" -> バイナリー byte[]
        public static byte[] ConvStringToBytes(string sndVal)
        {
            byte[] binary = new byte[sndVal.Length / 8 + 1];
            int num = 0;

            string fromStr = sndVal;
            string toStr = "";
            int count = 0;
            int i16 = 0;

            for (int i = 0; i < fromStr.Length; i++)
            {
                i16 *= 2;
                string tt = fromStr.Substring(i, 1);
                if ("1".Equals(tt))
                {
                    i16 += 1;
                }

                count++;
                if (count >= 4)
                {
                    count = 0;
                    toStr += Convert.ToString(i16, 16);

                    if (num % 2 == 1)
                    {
                        binary[num / 2] *= 16;
                        binary[num / 2] += (byte)i16;

                    }
                    else
                    {
                        binary[num / 2] = (byte)i16;
                    }
                    num++;
                    i16 = 0;
                }
            }

            return binary;
        }

        // Type 3
        public static string ConvType3ToSendString()
        {
            string sRet = "";

            //Type	
            sRet += "11";

            //GID
            sRet += Program.ConvGidSendData();

            // 救助支援情報ID数 2
            sRet += "00";

            // 救助支援情報ID1 6
            sRet += "000000";
            
            // 救助支援情報ID2 6
            sRet += "000000";
            
            // 救助支援情報ID3 6
            sRet += "000000";

            // 救助支援情報要求 2 
            sRet += "11"; // 3:送信要求

            // reserved 26
            sRet += new string('0', 26);

            //CRC 16
            sRet += "0000000000000000";

            //Tail 6
            sRet += "000000";

            return sRet;
        }

        // Type 3 ID数、ID1、ID2、ID3
        public static string ConvType3ToSendString(string idnum, string id1, string id2, string id3, string req)
        {
            string sRet = "";

            //Type	
            sRet += "11";

            //GID
            sRet += Program.ConvGidSendData();

            // 救助支援情報ID数 2
            sRet += idnum; // "00";

            // 救助支援情報ID1 6
            sRet += id1; // "000000";

            // 救助支援情報ID2 6
            sRet += id2; // "000000";

            // 救助支援情報ID3 6
            sRet += id3; // "000000";

            // 救助支援情報要求 2 
            sRet += req; // "11"; // 3:送信要求

            // reserved 26
            sRet += new string('0', 26);

            //CRC 16
            sRet += "0000000000000000";

            //Tail 6
            sRet += "000000";

            return sRet;
        }

        // Type 0 (FormShelterInfo.csから移動)
        public static string ConvTerminalInfoToSendString(bool bOpen, DbAccess.TerminalInfo info, int entrynum, int subtype)
        {
            string sRet = "";

            // Type 0 サブタイプ１
            if (subtype == 1)
            {
                //Type  2	
                sRet += "00";
                //GID   12
                sRet += Program.ConvGidSendData();

                //経度		19
                double dWork = double.Parse(info.lon);
                dWork -= 110;
                int nWork = (int)(dWork * 10000);
                string sWork = Convert.ToString(nWork, 2);
                if (sWork.Length < 19)
                {
                    int nIdx;

                    for (nIdx = 0; nIdx < 19; nIdx++)
                    {
                        if (sWork.Length >= 19)
                        {
                            break;
                        }
                        sWork = "0" + sWork;
                    }
                }
                sRet += sWork;

                //緯度		18
                dWork = double.Parse(info.lat);
                dWork -= 20;
                nWork = (int)(dWork * 10000);
                sWork = Convert.ToString(nWork, 2);
                if (sWork.Length < 18)
                {
                    int nIdx;

                    for (nIdx = 0; nIdx < 19; nIdx++)
                    {
                        if (sWork.Length >= 18)
                        {
                            break;
                        }
                        sWork = "0" + sWork;
                    }
                }
                sRet += sWork;

                // 避難所管理ID(SMID)    8
                string sendSid = Convert.ToString(int.Parse(info.smid), 2).PadLeft(8, '0');
                sRet += sendSid;

                // サブタイプ    3
                sRet += "001";

                //CRC		16
                sRet += "0000000000000000";

                //Tail		6
                sRet += "000000";
            }
            // Type0 サブタイプ２
            else if (subtype == 2)
            {
                //Type  2
                sRet += "00";
                //GID   12
                sRet += Program.ConvGidSendData();

                // 避難所開設状況 開設／閉鎖	1:開／0:閉	1
                if (bOpen)
                {
                    sRet += "1";
                }
                else
                {
                    sRet += "0";
                }

                // 避難所管理ID(SMID)    8
                string sendSid = Convert.ToString(int.Parse(info.smid), 2).PadLeft(8, '0');
                sRet += sendSid;

                // 未使用領域    19
                sRet += new string('0', 19);

                // 避難者数     17
                if (entrynum >= 131071)
                {
                    sRet += new string('1', 17);
                }
                else
                {
                    string sEntrynum = Convert.ToString(entrynum, 2).PadLeft(17, '0');
                    sRet += sEntrynum;
                }

                // サブタイプ    3
                sRet += "010";

                //CRC		16
                sRet += "0000000000000000";

                //Tail		6
                sRet += "000000";
            }
            else
            {
                // 想定外のサブタイプ
            }

            return sRet;
        }

        /// <summary>
        /// 個人安否情報衛星通信向け メッセージType1
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string ConvPersonInfoToSendString(DbAccess.PersonInfo info)
        {
            string sRet = "";

            //Type	
            sRet += "01";
            //GID
            sRet += Program.ConvGidSendData();

            //個人ID(電話番号)
            long nId = long.Parse(info.id);
            string sId = Convert.ToString(nId, 2);
            if (sId.Length < 40)
            {
                int nIdx;

                for (nIdx = 0; nIdx < 40; nIdx++)
                {
                    if (sId.Length >= 40)
                    {
                        break;
                    }
                    sId = "0" + sId;
                }
            }
            sRet += sId;

            // 公表  
            // DBデータinfo.sel03：  0:Private 1:Public
            // フォーマット：        0:公開可 1:公開不可
            switch (info.sel03)
            {
                case "0":
                    sRet += "1";
                    break;
                case "1":
                    sRet += "0";
                    break;
                default:
                    sRet += "1";
                    break;
            }

            //// -----安否状況(5bit)-----
            //// 怪我の有無
            //// DBデータinfo.sel04：0:無 1:有 2:未使用 3:未選択
            //// フォーマット：      0:無し、1:有り
            //switch (info.sel04)
            //{
            //    case "0":
            //        sRet += "0";
            //        break;
            //    case "1":
            //        sRet += "1";
            //        break;
            //    case "3":
            //        sRet += "0";
            //        break;
            //    default:
            //        sRet += "0";
            //        break;
            //}

            //// 要介護・要介助
            //// フォーマット：0:否(「介護」が"不要"かつ「障がい」が"無し")
            ////               1:要(「介護」が"必要"または「障がい」が"有り")
            //// 介護
            //// DBデータinfo.sel05：0:否 1:要 2:未選択
            //// 障がい
            //// DBデータinfo.sel06：0:無 1:有 2:未選択 
            //if ((info.sel05 == "1") || (info.sel06 == "1"))
            //{
            //    sRet += "1";
            //}
            //else
            //{
            //    sRet += "0";
            //}

            //// 高齢者
            //// フォーマット：      0:いいえ(年齢が65歳未満)、1:はい(年齢が65歳以上)
            //// DBデータinfo.txt02：生年月日(yyyy-mm-dd) 
            //// 現在の日付から生年月日を差し引いて年齢を求める
            //string[] birthDayStr = info.txt02.Split('-');
            //DateTime birthDay =
            //    new DateTime(int.Parse(birthDayStr[0]), int.Parse(birthDayStr[1]), int.Parse(birthDayStr[2]));
            //DateTime.TryParse(info.txt02,out birthDay);
            //int age = DateTime.Today.Year - birthDay.Year;
            //if (age < 65)
            //{
            //    sRet += "0";
            //}
            //else
            //{
            //    sRet += "1";
            //}

            //// 妊産婦
            //// DBデータinfo.sel07：0:いいえ 1:はい 2:未選択
            //// フォーマット：      0:いいえ、1:はい
            //switch (info.sel07)
            //{
            //    case "0":
            //        sRet += "0";
            //        break;
            //    case "1":
            //        sRet += "1";
            //        break;
            //    case "2":
            //        sRet += "0";
            //        break;
            //    default:
            //        sRet += "0";
            //        break;
            //}

            //// 乳児
            //// フォーマット：      0:いいえ(年齢が1歳以上)、1:はい(年齢が1歳未満)
            //// DBデータinfo.txt02：生年月日(yyyy-mm-dd) 
            //// 現在の日付から生年月日を差し引いて年齢を求める
            //if (age >= 1)
            //{
            //    sRet += "0";
            //}
            //else
            //{
            //    sRet += "1";
            //}
            //// -----安否状況(5bit)-----

            // -----安否状況(4bit + 空1bit)-----
            // 怪我の有無
            // DBデータinfo.sel04：0:無 1:有 2:未使用 3:未選択
            // フォーマット：      0:無し、1:有り
            switch (info.sel04)
            {
                case "0":
                    sRet += "0";
                    break;
                case "1":
                    sRet += "1";
                    break;
                case "3":
                    sRet += "0";
                    break;
                default:
                    sRet += "0";
                    break;
            }

            // 要介護・要介助
            // フォーマット：0:否(「介護」が"不要"かつ「障がい」が"無し")
            //               1:要(「介護」が"必要"または「障がい」が"有り")
            // 介護
            // DBデータinfo.sel05：0:否 1:要 2:未選択
            // 障がい
            // DBデータinfo.sel06：0:無 1:有 2:未選択 
            if ((info.sel05 == "1") || (info.sel06 == "1"))
            {
                sRet += "1";
            }
            else
            {
                sRet += "0";
            }

            // 送信ステータス判定
            // 01:高齢者
            // 10:妊産婦
            // 11:乳児
            // 00:どちらでもない
            
            //年齢取得
            string[] birthDayStr2 = info.txt02.Split('-');
            DateTime birthDay2 =
                new DateTime(int.Parse(birthDayStr2[0]), int.Parse(birthDayStr2[1]), int.Parse(birthDayStr2[2]));
            DateTime.TryParse(info.txt02, out birthDay2);
            int age2 = DateTime.Today.Year - birthDay2.Year;

            // 妊産婦取得
            // DBデータinfo.sel07：0:いいえ 1:はい 2:未選択
            bool preg = false;
            if (info.sel07 == "1")
            {
                preg = true;
            }
            else
            {
                preg = false;
            }

            //　送信ステータス判定(2bit)
            if (age2 >= 65)
            {
                // 高齢者
                sRet += "01";
            }
            else if (age2 < 1)
            {
                // 乳児
                sRet += "11";
            }
            else if (preg)
            {
                // 妊産婦
                sRet += "10";
            }
            else
            {
                // どちらでもない
                sRet += "00";
            }

            // 予備 (1bit)
            sRet += "0";

            // -----安否状況(4bit + 空1bit)-----



            // -----安否補足情報（2bit）-----
            // 避難所内外
            // DBデータinfo.sel08：0:内、1:外
            // フォーマット：      0:内、1:外
            switch (info.sel08)
            {
                case "0":
                    sRet += "0";
                    break;
                case "1":
                    sRet += "1";
                    break;
                default:
                    sRet += "0";
                    break;
            }
            
            // 予備(現在未使用)
            sRet += "0";
            // -----安否補足情報（2bit）-----

            //CRC	
            sRet += "0000000000000000";

            //Tail	
            sRet += "000000";

            return sRet;
        }

        #if false
        //// type 1(FormShelterInfo.csから移動)
        //public static string ConvPersonInfoToSendString(DbAccess.PersonInfo info)
        //{
        //    string sRet = "";

        //    //Type	
        //    sRet += "01";
        //    //GID
        //    sRet += Program.ConvGidSendData();

        //    //個人ID
        //    long nId = long.Parse(info.id);
        //    string sId = Convert.ToString(nId, 2);
        //    if (sId.Length < 40)
        //    {
        //        int nIdx;

        //        for (nIdx = 0; nIdx < 40; nIdx++)
        //        {
        //            if (sId.Length >= 40)
        //            {
        //                break;
        //            }
        //            sId = "0" + sId;
        //        }
        //    }
        //    sRet += sId;

        //    // 公表  
        //    // DBデータinfo.sel03：  0:Private 1:Public
        //    // フォーマット：        0:公開可 1:公開不可
        //    switch (info.sel03)
        //    {
        //        case "0":
        //            sRet += "1";
        //            break;
        //        case "1": 
        //            sRet += "0";
        //            break;
        //        default:
        //            sRet += "1";
        //            break;
        //    }

        //    //安否状況
        //    // DBデータinfo.sel01： 0:man, 1:woman
        //    // フォーマット：       0:男性 1:女性
        //    if (info.sel01 == "0")
        //    {
        //        sRet += "0";
        //    }
        //    else
        //    {
        //        sRet += "1";
        //    }

        //    // 怪我の有無
        //    // DBデータinfo.sel04：0:無 1:有 2:未使用 3:未選択
        //    // フォーマット：      0:無し、1:有り
        //    switch (info.sel04)
        //    {
        //        case "0":
        //            sRet += "0";
        //            break;
        //        case "1":
        //            sRet += "1";
        //            break;
        //        case "3":
        //            sRet += "0";
        //            break;
        //        default:
        //            sRet += "0";
        //            break;
        //    }

        //    // 介護
        //    // DBデータinfo.sel05：0:否 1:要 2:未選択
        //    // フォーマット：      0:否、1:要
        //    switch (info.sel05)
        //    {
        //        case "0":
        //            sRet += "0";
        //            break;
        //        case "1":
        //            sRet += "1";
        //            break;
        //        case "2":
        //            sRet += "0";
        //            break;
        //        default:
        //            sRet += "0";
        //            break;
        //    }

        //    // 障がい
        //    // DBデータinfo.sel06：0:無 1:有 2:未選択 
        //    // フォーマット：      0:有り、1:無し
        //    switch (info.sel06)
        //    {
        //        case "0":
        //            sRet += "0";
        //            break;
        //        case "1":
        //            sRet += "1";
        //            break;
        //        case "2":
        //            sRet += "0";
        //            break;
        //        default:
        //            sRet += "0";
        //            break;
        //    }

        //    // 妊産婦
        //    // DBデータinfo.sel07：0:いいえ 1:はい 2:未選択
        //    // フォーマット：      0:いいえ、1:はい
        //    switch (info.sel07)
        //    {
        //        case "0":
        //            sRet += "0";
        //            break;
        //        case "1":
        //            sRet += "1";
        //            break;
        //        case "2":
        //            sRet += "0";
        //            break;
        //        default:
        //            sRet += "0";
        //            break;
        //    }

        //    //    予備
        //    sRet += "00";

        //    //CRC	
        //    sRet += "0000000000000000";

        //    //Tail	
        //    sRet += "000000";

        //    return sRet;
        //}
        #endif


        // type 2(FormShelterInfo.csから移動)
        public static string[] ConvTotalSendLogToSendString(DbAccess.TotalSendLog log, bool bTextOnly)
        {
            string[] sRet = new string[9];

            int nIdx;
            for (nIdx = 0; nIdx < sRet.Length; nIdx++)
            {
                sRet[nIdx] = "";

                //Type	
                sRet[nIdx] += "10";
                //GID
                sRet[nIdx] += Program.ConvGidSendData();

                //Msg先頭フラグ
                if (nIdx == 0)
                {
                    sRet[nIdx] += "1";
                }
                else
                {
                    sRet[nIdx] += "0";
                }
                //Msg最終フラグ      // 暫定フォーマットではテキスト長固定のため9分割の末尾を最終とする
                if (nIdx == (sRet.Length - 1))
                {
                    sRet[nIdx] += "1";
                }
                else
                {
                    sRet[nIdx] += "0";
                }
                //シーケンス番号
                string sSeq = Convert.ToString(nIdx, 2);
                int nLen;
                for (nLen = 0; nLen < 5; nLen++)
                {
                    if (sSeq.Length >= 5)
                    {
                        break;
                    }
                    sSeq = "0" + sSeq;
                }

                sRet[nIdx] += sSeq;

                // 避難所情報
                if (bTextOnly)
                {
                    sRet[nIdx] += ConvTotalSendTextData(log, nIdx);
                }
                else
                {
                    switch (nIdx)
                    {
                        case 0:
                            sRet[nIdx] += ConvTotalSendLog01Data(log);
                            break;
                        case 1:
                            sRet[nIdx] += ConvTotalSendLog02Data(log);
                            break;
                        case 2:
                            sRet[nIdx] += ConvTotalSendLog03Data(log);
                            break;
                        case 3:
                            sRet[nIdx] += ConvTotalSendLog04Data(log);
                            break;
                        case 4:
                            sRet[nIdx] += ConvTotalSendLog05Data(log);
                            break;
                        case 5:
                            sRet[nIdx] += ConvTotalSendLog06Data(log);
                            break;
                        case 6:
                            sRet[nIdx] += ConvTotalSendLog07Data(log);
                            break;
                        case 7:
                            sRet[nIdx] += ConvTotalSendLog08Data(log);
                            break;
                        case 8:
                            sRet[nIdx] += ConvTotalSendLog09Data(log);
                            break;
                        default:
                            break;
                    }
                }

                //避難所詳細情報種別 (1=避難所名登録(別関数),0=避難所詳細情報登録)
                sRet[nIdx] += "0";

                //CRC	
                sRet[nIdx] += "0000000000000000";

                //Tail	
                sRet[nIdx] += "000000";
            }

            return sRet;
        }

        // テキストモード（送信データ作成）
        private static string ConvTotalSendTextData(DbAccess.TotalSendLog log, int idx)
        {
            string sRet = "";

            // 避難所メッセージ (32bit + 40bit*8 = 44byte = 22文字)
            string sWork = ConvTextToByteText(log.txt01.PadRight(22, ' '), 44);

            // 先頭データのみ、データ種別、データサイズをセットする
            if (idx == 0)
            {
                // データ種別(1bit) 0:バイナリ 1:テキスト
                sRet += "1";

                // データサイズ 7bit
                sRet += "0101101";

                // テキスト 32bit
                sWork = sWork.Substring(0, 32);
            }
            else
            {
                // テキスト 40bit
                sWork = sWork.Substring(idx * 40 - 8, 40);
            }
            sRet += sWork;

            return sRet;
        }

        // バイナリモード（送信データ作成）
        private static string ConvTotalSendLog01Data(DbAccess.TotalSendLog log)
        {
            string sRet = "";

            // データ種別(1bit)　0:バイナリ 1:テキスト
            sRet += "0";

            int nWork;
            string sWork;
            int nLen;

            // データ長　避難所詳細情報のサイズ(byte)(7bit)
            // 暫定フォーマットではテキスト長固定のためデータ長も45byte固定とする
            sRet += "0101101";

            // Reserved（4bit）
            sRet += "0000";

            // 避難者数	(12bit)
            nWork = int.Parse(log.num01);
            sWork = Convert.ToString(nWork, 2);
            for (nLen = 0; nLen < 12; nLen++)
            {
                if (sWork.Length >= 12)
                {
                    break;
                }
                sWork = "0" + sWork;
            }
            sRet += sWork;

            // 在宅数(12bit)
            nWork = int.Parse(log.num02);
            sWork = Convert.ToString(nWork, 2);
            for (nLen = 0; nLen < 12; nLen++)
            {
                if (sWork.Length >= 12)
                {
                    break;
                }
                sWork = "0" + sWork;
            }
            sRet += sWork;

            // 男性(12bit)(上位4bit)
            nWork = int.Parse(log.num03);
            sWork = Convert.ToString(nWork, 2);
            for (nLen = 0; nLen < 12; nLen++)
            {
                if (sWork.Length >= 12)
                {
                    break;
                }
                sWork = "0" + sWork;
            }
            sWork = sWork.Substring(0, 4);
            sRet += sWork;

            return sRet;
        }

        private static string ConvTotalSendLog02Data(DbAccess.TotalSendLog log)
        {
            string sRet = "";

            int nWork;
            string sWork;
            int nLen;
            // 男性(12bit)(下位8bit)
            nWork = int.Parse(log.num03);
            sWork = Convert.ToString(nWork, 2);
            for (nLen = 0; nLen < 12; nLen++)
            {
                if (sWork.Length >= 12)
                {
                    break;
                }
                sWork = "0" + sWork;
            }
            sWork = sWork.Substring(4, 8);
            sRet += sWork;

            // 女性(12bit)
            nWork = int.Parse(log.num04);
            sWork = Convert.ToString(nWork, 2);
            for (nLen = 0; nLen < 12; nLen++)
            {
                if (sWork.Length >= 12)
                {
                    break;
                }
                sWork = "0" + sWork;
            }
            sRet += sWork;

            // 重傷(12bit)
            nWork = int.Parse("0");
            sWork = Convert.ToString(nWork, 2);
            for (nLen = 0; nLen < 12; nLen++)
            {
                if (sWork.Length >= 12)
                {
                    break;
                }
                sWork = "0" + sWork;
            }
            sRet += sWork;

            // 軽傷(12bit)(上位8bit)
            nWork = int.Parse(log.num05);
            sWork = Convert.ToString(nWork, 2);
            for (nLen = 0; nLen < 12; nLen++)
            {
                if (sWork.Length >= 12)
                {
                    break;
                }
                sWork = "0" + sWork;
            }
            sWork = sWork.Substring(0, 8);
            sRet += sWork;

            return sRet;
        }

        private static string ConvTotalSendLog03Data(DbAccess.TotalSendLog log)
        {
            string sRet = "";

            int nWork;
            string sWork;
            int nLen;

            // 軽傷(12bit)(下位4bit)
            nWork = int.Parse(log.num05);
            sWork = Convert.ToString(nWork, 2);
            for (nLen = 0; nLen < 12; nLen++)
            {
                if (sWork.Length >= 12)
                {
                    break;
                }
                sWork = "0" + sWork;
            }
            sWork = sWork.Substring(8, 4);
            sRet += sWork;


            // 要介護 (12bit)
            nWork = int.Parse(log.num06);
            sWork = Convert.ToString(nWork, 2);
            for (nLen = 0; nLen < 12; nLen++)
            {
                if (sWork.Length >= 12)
                {
                    break;
                }
                sWork = "0" + sWork;
            }
            sRet += sWork;

            // 障がい者 (12bit)
            nWork = int.Parse(log.num07);
            sWork = Convert.ToString(nWork, 2);
            for (nLen = 0; nLen < 12; nLen++)
            {
                if (sWork.Length >= 12)
                {
                    break;
                }
                sWork = "0" + sWork;
            }
            sRet += sWork;

            // 妊産婦 (12bit)
            nWork = int.Parse(log.num08);
            sWork = Convert.ToString(nWork, 2);
            for (nLen = 0; nLen < 12; nLen++)
            {
                if (sWork.Length >= 12)
                {
                    break;
                }
                sWork = "0" + sWork;
            }
            sRet += sWork;

            return sRet;
        }

        private static string ConvTotalSendLog04Data(DbAccess.TotalSendLog log)
        {
            string sRet = "";

            int nWork;
            string sWork;
            int nLen;

            // 高齢者(12bit)
            nWork = int.Parse(log.num09);
            sWork = Convert.ToString(nWork, 2);
            for (nLen = 0; nLen < 12; nLen++)
            {
                if (sWork.Length >= 12)
                {
                    break;
                }
                sWork = "0" + sWork;
            }
            sRet += sWork;

            // 乳児(12bit)
            nWork = int.Parse(log.num10);
            sWork = Convert.ToString(nWork, 2);
            for (nLen = 0; nLen < 12; nLen++)
            {
                if (sWork.Length >= 12)
                {
                    break;
                }
                sWork = "0" + sWork;
            }
            sRet += sWork;

            //2020.03.16 Del
            //// Reserved(4bit)
            //sRet += "0000";

            // テキスト長(8bit)
            // 暫定フォーマットでは208bit固定(13文字)とする
            sRet += "11010000";

            // 避難所メッセージ (26byte(0～))(8bit)
            sWork = ConvTextToByteText(log.txt01, 26);
            sWork = sWork.Substring(0, 8);      //2020.03.16 <- (0, 4)
            sRet += sWork;

            return sRet;
        }

        private static string ConvTotalSendLog05Data(DbAccess.TotalSendLog log)
        {
            string sRet = "";

            // 避難所メッセージ (26byte(1～))(40bit)
            string sWork = ConvTextToByteText(log.txt01, 26);
            sWork = sWork.Substring(8, 40);     //2020.03.16 <- (4, 40)
            sRet += sWork;
            
            return sRet;
        }

        private static string ConvTotalSendLog06Data(DbAccess.TotalSendLog log)
        {
            string sRet = "";

            // 避難所メッセージ (26byte(6～))(40bit)
            string sWork = ConvTextToByteText(log.txt01, 26);
            sWork = sWork.Substring(48, 40);    //2020.03.16 <- (44, 40)
            sRet += sWork;

            return sRet;
        }

        private static string ConvTotalSendLog07Data(DbAccess.TotalSendLog log)
        {
            string sRet = "";

            // 避難所メッセージ (26byte(11～))(40bit)
            string sWork = ConvTextToByteText(log.txt01, 26);
            sWork = sWork.Substring(88, 40);    //2020.03.16 <- (84, 40)
            sRet += sWork;

            return sRet;
        }

        private static string ConvTotalSendLog08Data(DbAccess.TotalSendLog log)
        {
            string sRet = "";

            // 避難所メッセージ (26byte(16～))(40bit)
            string sWork = ConvTextToByteText(log.txt01, 26);
            sWork = sWork.Substring(128, 40);    //2020.03.16 <- (124, 40)
            sRet += sWork;

            return sRet;
        }

        private static string ConvTotalSendLog09Data(DbAccess.TotalSendLog log)
        {
            string sRet = "";

            // 避難所メッセージ (26byte(21～))(40bit)
            string sWork = ConvTextToByteText(log.txt01, 26);
            sWork = sWork.Substring(168, 40);    //2020.03.16 <- (164, 40)
            sRet += sWork;

            return sRet;
        }
#if false
        private static string ConvTotalSendLog10Data(DbAccess.TotalSendLog log)
        {
            string sRet = "";

            // 避難所メッセージ 	40
            string sWork;
            int nLen;
            int nIdx;
            int nStartIdx = 25;
            byte[] txt01 = ConvTxt01(log.txt01);
            for (nIdx = nStartIdx; nIdx < 60; nIdx++)
            {
                sWork = Convert.ToString(txt01[nIdx], 2);
                for (nLen = 0; nLen < 8; nLen++)
                {
                    if (sWork.Length >= 8)
                    {
                        break;
                    }
                    sWork = "0" + sWork;
                }
                if (nIdx == nStartIdx)
                {
                    sWork = sWork.Substring(4, 4);
                }
                else if (nIdx == (nStartIdx + 5))
                {
                    sWork = sWork.Substring(0, 4);
                }
                sRet += sWork;

                if (sRet.Length >= 40)
                {
                    break;
                }
            }

            return sRet;
        }

        private static string ConvTotalSendLog11Data(DbAccess.TotalSendLog log)
        {
            string sRet = "";

            // 避難所メッセージ 	40
            string sWork;
            int nLen;
            int nIdx;
            int nStartIdx = 30;
            byte[] txt01 = ConvTxt01(log.txt01);
            for (nIdx = nStartIdx; nIdx < 60; nIdx++)
            {
                sWork = Convert.ToString(txt01[nIdx], 2);
                for (nLen = 0; nLen < 8; nLen++)
                {
                    if (sWork.Length >= 8)
                    {
                        break;
                    }
                    sWork = "0" + sWork;
                }
                if (nIdx == nStartIdx)
                {
                    sWork = sWork.Substring(4, 4);
                }
                else if (nIdx == (nStartIdx + 5))
                {
                    sWork = sWork.Substring(0, 4);
                }
                sRet += sWork;

                if (sRet.Length >= 40)
                {
                    break;
                }
            }

            return sRet;
        }

        private static string ConvTotalSendLog12Data(DbAccess.TotalSendLog log)
        {
            string sRet = "";

            // 避難所メッセージ 	40
            string sWork;
            int nLen;
            int nIdx;
            int nStartIdx = 35;
            byte[] txt01 = ConvTxt01(log.txt01);
            for (nIdx = nStartIdx; nIdx < 60; nIdx++)
            {
                sWork = Convert.ToString(txt01[nIdx], 2);
                for (nLen = 0; nLen < 8; nLen++)
                {
                    if (sWork.Length >= 8)
                    {
                        break;
                    }
                    sWork = "0" + sWork;
                }
                if (nIdx == nStartIdx)
                {
                    sWork = sWork.Substring(4, 4);
                }
                else if (nIdx == (nStartIdx + 5))
                {
                    sWork = sWork.Substring(0, 4);
                }
                sRet += sWork;

                if (sRet.Length >= 40)
                {
                    break;
                }
            }

            return sRet;
        }

        private static string ConvTotalSendLog13Data(DbAccess.TotalSendLog log)
        {
            string sRet = "";

            // 避難所メッセージ 	40
            string sWork;
            int nLen;
            int nIdx;
            int nStartIdx = 40;
            byte[] txt01 = ConvTxt01(log.txt01);
            for (nIdx = nStartIdx; nIdx < 60; nIdx++)
            {
                sWork = Convert.ToString(txt01[nIdx], 2);
                for (nLen = 0; nLen < 8; nLen++)
                {
                    if (sWork.Length >= 8)
                    {
                        break;
                    }
                    sWork = "0" + sWork;
                }
                if (nIdx == nStartIdx)
                {
                    sWork = sWork.Substring(4, 4);
                }
                else if (nIdx == (nStartIdx + 5))
                {
                    sWork = sWork.Substring(0, 4);
                }
                sRet += sWork;

                if (sRet.Length >= 40)
                {
                    break;
                }
            }

            return sRet;
        }

        private static string ConvTotalSendLog14Data(DbAccess.TotalSendLog log)
        {
            string sRet = "";

            // 避難所メッセージ 	40
            string sWork;
            int nLen;
            int nIdx;
            int nStartIdx = 45;
            byte[] txt01 = ConvTxt01(log.txt01);
            for (nIdx = nStartIdx; nIdx < 60; nIdx++)
            {
                sWork = Convert.ToString(txt01[nIdx], 2);
                for (nLen = 0; nLen < 8; nLen++)
                {
                    if (sWork.Length >= 8)
                    {
                        break;
                    }
                    sWork = "0" + sWork;
                }
                if (nIdx == nStartIdx)
                {
                    sWork = sWork.Substring(4, 4);
                }
                else if (nIdx == (nStartIdx + 5))
                {
                    sWork = sWork.Substring(0, 4);
                }
                sRet += sWork;

                if (sRet.Length >= 40)
                {
                    break;
                }
            }

            return sRet;
        }

        private static string ConvTotalSendLog15Data(DbAccess.TotalSendLog log)
        {
            string sRet = "";

            // 避難所メッセージ 	40
            string sWork;
            int nLen;
            int nIdx;
            int nStartIdx = 50;
            byte[] txt01 = ConvTxt01(log.txt01);
            for (nIdx = nStartIdx; nIdx < 60; nIdx++)
            {
                sWork = Convert.ToString(txt01[nIdx], 2);
                for (nLen = 0; nLen < 8; nLen++)
                {
                    if (sWork.Length >= 8)
                    {
                        break;
                    }
                    sWork = "0" + sWork;
                }
                if (nIdx == nStartIdx)
                {
                    sWork = sWork.Substring(4, 4);
                }
                else if (nIdx == (nStartIdx + 5))
                {
                    sWork = sWork.Substring(0, 4);
                }
                sRet += sWork;

                if (sRet.Length >= 40)
                {
                    break;
                }
            }

            return sRet;
        }

        private static string ConvTotalSendLog16Data(DbAccess.TotalSendLog log)
        {
            string sRet = "";

            // 避難所メッセージ 	40
            string sWork;
            int nLen;
            int nIdx;
            int nStartIdx = 55;
            byte[] txt01 = ConvTxt01(log.txt01);
            for (nIdx = nStartIdx; nIdx < 60; nIdx++)
            {
                sWork = Convert.ToString(txt01[nIdx], 2);
                for (nLen = 0; nLen < 8; nLen++)
                {
                    if (sWork.Length >= 8)
                    {
                        break;
                    }
                    sWork = "0" + sWork;
                }
                if (nIdx == nStartIdx)
                {
                    sWork = sWork.Substring(4, 4);
                }
                else if (nIdx == (nStartIdx + 5))
                {
                    sWork = sWork.Substring(0, 4);
                }
                sRet += sWork;

                if (sRet.Length >= 40)
                {
                    break;
                }
            }

            sRet += "0000";

            return sRet;
        }
#endif

        /// <summary>
        /// 避難所名新規登録メッセージ作成
        /// </summary>
        /// <param name="newShelterName"></param>
        /// <returns></returns>
        public static string[] ConvRegisterNewShelterToSendString(string gid, string smid, string newShelterName)
        {
            string[] sRet = new string[9];

            int nIdx;
            for (nIdx = 0; nIdx < sRet.Length; nIdx++)
            {
                sRet[nIdx] = "";

                //Type (メッセージタイプ2)	    2bit
                sRet[nIdx] += "10";

                //GID                           12bit
                sRet[nIdx] += Program.ConvGidSendData();

                //Msg先頭フラグ                  1bit
                if (nIdx == 0)
                {
                    sRet[nIdx] += "1";
                }
                else
                {
                    sRet[nIdx] += "0";
                }
                //Msg最終フラグ                  1bit
                // 暫定フォーマットではテキスト長固定のため9分割の末尾を最終とする
                if (nIdx == (sRet.Length - 1))
                {
                    sRet[nIdx] += "1";
                }
                else
                {
                    sRet[nIdx] += "0";
                }
                //シーケンス番号               5bit
                string sSeq = Convert.ToString(nIdx, 2);
                int nLen;
                for (nLen = 0; nLen < 5; nLen++)
                {
                    if (sSeq.Length >= 5)
                    {
                        break;
                    }
                    sSeq = "0" + sSeq;
                }

                sRet[nIdx] += sSeq;

                // 分割避難所詳細情報(避難所名)          40bit
                if (nIdx == 0)
                {
                    sRet[nIdx] += ConvShelterNameData01(newShelterName, smid);
                }
                else
                {
                    sRet[nIdx] += ConvShelterNameData2_9(newShelterName, nIdx+1);
                }

                //避難所詳細情報種別 (1=避難所名登録,0=避難所詳細情報登録(別関数))     1bit
                sRet[nIdx] += "1";

                //CRC	                                 16bit
                sRet[nIdx] += "0000000000000000";

                //Tail	                                6bit
                sRet[nIdx] += "000000";
            }

            return sRet;
        }

        /// <summary>
        /// 避難所名送信シーケンス1
        /// </summary>
        /// <param name="NewShelterName"></param>
        /// <param name="smid"></param>
        /// <returns></returns>
        /// 1シーケンスにつき40bitまで
        private static string ConvShelterNameData01(string NewShelterName, string smid)
        {
            string sRet = "";

            // 避難所管理情報種別(8bit)　1:避難所名登録 その他:Reserved
            sRet += "00000001";

            // 避難所管理ID(8bit)　避難所IDの追番部分を設定
            //smidを2進数に変換
            string sendSmid = Convert.ToString(int.Parse(smid),2).PadLeft(8,'0');
            sRet += sendSmid;

            // 送信データのバイト長(40byte/320bit固定)(8bit)
            sRet += "00101000";

            // 避難所名(320bit=40byteのうち16bit分を格納)(16bit)
            string sWork = ConvTextToByteText(NewShelterName, 40);
            sWork = sWork.Substring(0, 16);
            sRet += sWork;           
            
            return sRet;
        }

        /// <summary>
        /// 避難所名送信シーケンス2～9
        /// </summary>
        /// <param name="NewShelterName"></param>
        /// <param name="smid"></param>
        /// <returns></returns>
        /// 1シーケンスにつき40bitまで
        private static string ConvShelterNameData2_9(string NewShelterName, int sequenceNum)
        {
            string sRet = "";
            int startPos = 0;
            int getLen = 0;

            // シーケンス毎に送信位置を設定
            switch (sequenceNum)
            {
                case 2:
                    startPos = 16;
                    getLen = 40;
                    break;
                case 3:
                    startPos = 56;
                    getLen = 40;
                    break;
                case 4:
                    startPos = 96;
                    getLen = 40;
                    break;
                case 5:
                    startPos = 136;
                    getLen = 40;
                    break;
                case 6:
                    startPos = 176;
                    getLen = 40;
                    break;
                case 7:
                    startPos = 216;
                    getLen = 40;
                    break;
                case 8:
                    startPos = 256;
                    getLen = 40;
                    break;
                case 9:
                    startPos = 296;
                    getLen = 24;
                    break;
                default:
                    // seaquenceNumが2～9以外の場合、何も値を返さずに終了
                    return "";
            }

            // 避難所名([getLen]bit分を格納)
            string sWork = ConvTextToByteText(NewShelterName, 40);
            sWork = sWork.Substring(startPos, getLen);
            sRet += sWork;

            // シーケンス9のみReserved(16bit)が入る
            if (sequenceNum == 9)
            {
                // Reserved(16bit)
                sRet += "0000000000000000";
            }

            return sRet;
        }


        private static byte[] ConvTxt01(string txt01)
        {
            char[] bufList = txt01.ToCharArray();

            byte[] retList = new byte[60];

            int nIdx;
            int nCharIdx = 0;
            for (nIdx = 0; nIdx < retList.Length; nIdx++)
            {
                if (bufList.Length > nCharIdx)
                {
                    if ((nIdx % 2) == 0)
                    {
                        retList[nIdx] = (byte)(((int)bufList[nCharIdx] & 0x0000ff00) >> 8);
                    }
                    else
                    {
                        retList[nIdx] = (byte)(((int)bufList[nCharIdx] & 0x000000ff) >> 0);
                        nCharIdx++;
                    }
                }
                else
                {
                    retList[nIdx] = 0;
                }
            }

            return retList;
        }

        /**
         * @brief テキスト->byte値の文字列
         * @param txt:文字列
         * @param byteNum:取得するbyte数
         */
        private static string ConvTextToByteText(string txt, int byteNum)
        {
            string sRet = "";
            string sWork;
            int nLen;
            int nIdx;
            byte[] byteText = ConvTxt01(txt);

            for (nIdx = 0; nIdx < byteNum; nIdx++)
            {
                if (nIdx < byteText.Length)
                {
                    sWork = Convert.ToString(byteText[nIdx], 2);
                    for (nLen = 0; nLen < 8; nLen++)
                    {
                        if (sWork.Length >= 8)
                        {
                            break;
                        }
                        sWork = "0" + sWork;
                    }
                }
                else
                {
                    sWork = "00000000";
                }
                sRet += sWork;
            }
            return sRet;
        }

    }
}
