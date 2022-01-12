/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.msg;
using QAnpiCtrlLib.log;

namespace QAnpiCtrlLib.utils
{
    /// <summary>
    /// 共通で使用するメソッドを登録する。
    /// </summary>
    public class CommonUtils
    {
        /// <summary>
        /// パラメータ範囲チェック
        /// </summary>
        /// <param name="value">パラメータ</param>
        /// <param name="min">パラメータの最小値</param>
        /// <param name="max">パラメータの最大値</param>
        /// <returns>チェック結果 true:範囲外 false:範囲内</returns>
        public static Boolean isOutOfRange(int value, int min, int max)
        {
            return isOutOfRange((long)value, (long)min, (long)max);
        }

        /// <summary>
        /// パラメータ範囲チェック
        /// </summary>
        /// <param name="value">パラメータ</param>
        /// <param name="min">パラメータの最小値</param>
        /// <param name="max">パラメータの最大値</param>
        /// <returns>チェック結果 true:範囲外 false:範囲内</returns>
        public static Boolean isOutOfRange(long value, long min, long max)
        {
            Boolean result = false;
            if (value < min || value > max)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// string型に格納されている16進数の値をbyte[]型に変換する
        /// </summary>
        /// <param name="strByteArray">16進数の値</param>
        /// <returns></returns>
        public static  byte[] ToByteArray(string strByteArray)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            if ((strByteArray.Length % 2) != 0)
            {
                throw (new System.ArgumentException("messageのレングス不正"));
            }

            ByteConverter bcoverter = new ByteConverter();
            List<byte> listbyte = new List<byte>();
            for (int i = 0; i < strByteArray.Length; i = i + 2)
            {
                try
                {
                    listbyte.Add((byte)bcoverter.ConvertFromString("0x" + strByteArray.Substring(i, 2)));
                }
                catch (System.Exception e)
                {
                    LogMng.AplLogError(e.Message);
                    LogMng.AplLogError(e.StackTrace);
                    throw (e);
                }

            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            return listbyte.ToArray();
        }

        /// <summary>
        /// crc16の計算
        /// </summary>
        /// <param name="msg">計算対象のデータ</param>
        /// <returns>crcの計算結果</returns>
        public static int calcutateCrc16(byte[] msg)
        {
            // CRC16のテーブル情報
            UInt16[] CRC16Table = { 0x0000, 0x1021, 0x2042, 0x3063,
            0x4084, 0x50A5, 0x60C6, 0x70E7, 0x8108, 0x9129, 0xA14A, 0xB16B,
            0xC18C, 0xD1AD, 0xE1CE, 0xF1EF, 0x1231, 0x0210, 0x3273, 0x2252,
            0x52B5, 0x4294, 0x72F7, 0x62D6, 0x9339, 0x8318, 0xB37B, 0xA35A,
            0xD3BD, 0xC39C, 0xF3FF, 0xE3DE, 0x2462, 0x3443, 0x0420, 0x1401,
            0x64E6, 0x74C7, 0x44A4, 0x5485, 0xA56A, 0xB54B, 0x8528, 0x9509,
            0xE5EE, 0xF5CF, 0xC5AC, 0xD58D, 0x3653, 0x2672, 0x1611, 0x0630,
            0x76D7, 0x66F6, 0x5695, 0x46B4, 0xB75B, 0xA77A, 0x9719, 0x8738,
            0xF7DF, 0xE7FE, 0xD79D, 0xC7BC,

            0x48C4, 0x58E5, 0x6886, 0x78A7, 0x0840, 0x1861, 0x2802, 0x3823,
            0xC9CC, 0xD9ED, 0xE98E, 0xF9AF, 0x8948, 0x9969, 0xA90A, 0xB92B,
            0x5AF5, 0x4AD4, 0x7AB7, 0x6A96, 0x1A71, 0x0A50, 0x3A33, 0x2A12,
            0xDBFD, 0xCBDC, 0xFBBF, 0xEB9E, 0x9B79, 0x8B58, 0xBB3B, 0xAB1A,
            0x6CA6, 0x7C87, 0x4CE4, 0x5CC5, 0x2C22, 0x3C03, 0x0C60, 0x1C41,
            0xEDAE, 0xFD8F, 0xCDEC, 0xDDCD, 0xAD2A, 0xBD0B, 0x8D68, 0x9D49,
            0x7E97, 0x6EB6, 0x5ED5, 0x4EF4, 0x3E13, 0x2E32, 0x1E51, 0x0E70,
            0xFF9F, 0xEFBE, 0xDFDD, 0xCFFC, 0xBF1B, 0xAF3A, 0x9F59, 0x8F78,

            0x9188, 0x81A9, 0xB1CA, 0xA1EB, 0xD10C, 0xC12D, 0xF14E, 0xE16F,
            0x1080, 0x00A1, 0x30C2, 0x20E3, 0x5004, 0x4025, 0x7046, 0x6067,
            0x83B9, 0x9398, 0xA3FB, 0xB3DA, 0xC33D, 0xD31C, 0xE37F, 0xF35E,
            0x02B1, 0x1290, 0x22F3, 0x32D2, 0x4235, 0x5214, 0x6277, 0x7256,
            0xB5EA, 0xA5CB, 0x95A8, 0x8589, 0xF56E, 0xE54F, 0xD52C, 0xC50D,
            0x34E2, 0x24C3, 0x14A0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405,
            0xA7DB, 0xB7FA, 0x8799, 0x97B8, 0xE75F, 0xF77E, 0xC71D, 0xD73C,
            0x26D3, 0x36F2, 0x0691, 0x16B0, 0x6657, 0x7676, 0x4615, 0x5634,

            0xD94C, 0xC96D, 0xF90E, 0xE92F, 0x99C8, 0x89E9, 0xB98A, 0xA9AB,
            0x5844, 0x4865, 0x7806, 0x6827, 0x18C0, 0x08E1, 0x3882, 0x28A3,
            0xCB7D, 0xDB5C, 0xEB3F, 0xFB1E, 0x8BF9, 0x9BD8, 0xABBB, 0xBB9A,
            0x4A75, 0x5A54, 0x6A37, 0x7A16, 0x0AF1, 0x1AD0, 0x2AB3, 0x3A92,
            0xFD2E, 0xED0F, 0xDD6C, 0xCD4D, 0xBDAA, 0xAD8B, 0x9DE8, 0x8DC9,
            0x7C26, 0x6C07, 0x5C64, 0x4C45, 0x3CA2, 0x2C83, 0x1CE0, 0x0CC1,
            0xEF1F, 0xFF3E, 0xCF5D, 0xDF7C, 0xAF9B, 0xBFBA, 0x8FD9, 0x9FF8,
            0x6E17, 0x7E36, 0x4E55, 0x5E74, 0x2E93, 0x3EB2, 0x0ED1, 0x1EF0, };

            int encodedData = 0;
            Int32 size = msg.Length - consts.CommonConst.CRC16_SIZE / consts.EncDecConst.BYTE_BIT_SIZE;
            UInt16 crc16 = 0xffff;
            while (size != 0) {
                crc16 = (UInt16)(CRC16Table[(crc16 >> (consts.CommonConst.CRC16_SIZE - consts.EncDecConst.BYTE_BIT_SIZE)) ^ msg[encodedData]] ^ (crc16 << consts.EncDecConst.BYTE_BIT_SIZE));
                encodedData++;
                size--;
            }
            return (Int32)crc16;
        }

        /// <summary>
        /// double型のデータをacsii(16byte)のデータに変換
        /// </summary>
        /// <param name="ddata">double型のデータ</param>
        /// <returns>変換後のbyte配列</returns>
        public static byte[] doubleToBytes(double ddata)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            byte[] tmpBytes = BitConverter.GetBytes(ddata);
            Array.Reverse(tmpBytes);
            StringBuilder sb = new StringBuilder();
            foreach(byte b in tmpBytes)
            {
                sb.Append(b.ToString("X2"));
            }
            byte[] retBytes = System.Text.Encoding.GetEncoding("ascii").GetBytes(sb.ToString());
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return retBytes;
        }

        /// <summary>
        /// バイト配列を文字列に変換する
        /// </summary>
        /// <param name="data">バイト配列</param>
        /// <returns>文字列</returns>
        public static string bytesToString(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            foreach(byte b in data)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// ビット反転数値変換
        /// </summary>
        /// <param name="decInt">MSB or LSB数値</param>
        /// <returns>ビット反転数値</returns>
        public static int bitReverse(int decInt)
        {
            string binStr;
            char[] binStrChars;
            string binStr_rev;
            int decInt_rev;

            // 2進数文字列に変換（32桁に合わせる）
            binStr = Convert.ToString(decInt, 2).PadLeft(32, '0');

            // 文字列反転
            binStrChars = binStr.ToCharArray();
            Array.Reverse(binStrChars);
            binStr_rev = new string(binStrChars);
            
            // 反転2進数文字列を10進数に変換
            decInt_rev = Convert.ToInt32(binStr_rev, 2);

            return decInt_rev;
        }

    }
}
