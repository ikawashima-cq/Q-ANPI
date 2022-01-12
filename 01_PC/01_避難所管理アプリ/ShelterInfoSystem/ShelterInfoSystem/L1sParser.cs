/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2017. All rights reserved.
 */
/**
 * @file    L1sParser.cs
 * @brief   L1Sメッセージ解析クラス
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace ShelterInfoSystem
{
    /**
     * @class L1sParser
     * @brief L1Sメッセージ解析クラス
     */
    public class L1sParser
    {
        /**
         * @brief Intのビット長
         */
        protected const int INT_BIT_SIZE = 32;
        
        /**
         * @brief Longのビット長
         */
        protected const int LONG_BIT_SIZE = 64;
        
        /**
         * @brief Byteのビット長
         */
        protected const int BYTE_BIT_SIZE = 8;

        /**
         * @brief タイトル部(mt=メッセージタイプ)
         */
        protected const string FILE_NAME_PRE = "mt"; // mt43

        private Dictionary<string, string> m_DataMap = new Dictionary<string,string>();
        private Dictionary<string, string[]> m_KeysMap = new Dictionary<string, string[]>();
        private Dictionary<string, string[]> m_BitsMap = new Dictionary<string, string[]>();

        public string m_fileName = "";

        string[] m_HeadKeys = { "PAB", "MT", "Rc", "Dc" };
        int[] m_HeadBits = { 8, 6, 3, 4 };

        public L1sParser()
        {

        }

        /// <summary>
        /// 災害危機通報フォーマットから値を取得して保持する
        /// </summary>
        /// <param name="key">項目名</param>
        /// <returns>セットされた値</returns>
        /**
         * @brief 災害危機通報フォーマットから値を取得して保持する
         * @param フォーマット名
         * @param 項目名データ
         * @param ビット数データ
         */
        public void setFormat(string fname,string csvKeys, string csvBits)
        {
            try
            {
                string[] m_Keys = csvKeys.Split(',');

                m_KeysMap.Add(fname, m_Keys);

                string[] m_Bits = csvBits.Split(',');

                m_BitsMap.Add(fname, m_Bits);
            }
            catch(Exception ex)
            {
                // null 等
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "L1sDecode", "setCsv " + ex.Message );
            }
        }

        /**
         * 災害危機通報フォーマットから値を取得する
         * 
         *  @param 項目名
         *  @return セットされた値
        */
        public string getFormat(string fname, string key)
        {
            string ret = "";
            try
            {
                string[] m_Keys = m_KeysMap[fname];
                string[] m_Bits = m_BitsMap[fname];

                for (int i = 0; i < m_Keys.Length; i++)
                {
                    if (key == m_Keys[i])
                    {
                        ret = m_Bits[i];
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                // err
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS,
                    "L1sBitsToMsg", "getFormat fname:" + fname 
                    + " key:" + key + " errmsg:" + e.Message);
            }
            return ret;
        }

        /**
         * @brief 災害危機通報のヘッダからタイプと種別（t43_2 の形で）取得する
         * @param バイナリ情報
         * @return フォーマット名
         */
        public string getTypeRc(byte[] frombits)
        {
            string fname = FILE_NAME_PRE;// "t"; // 
            byte[] bits = new byte[frombits.Length];
            reverse(frombits, bits);
            //
            int size;
            int pos = 0;
            int val = 0;


            for (int i = 0; i < m_HeadKeys.Length; i++)
            {
                size = m_HeadBits[i];
                val = decodeInt(bits, size, pos);

                try
                {
                    if ("MT".Equals(m_HeadKeys[i]))
                    {
                        // メッセージタイプ
                        fname += val;
                        if (val == 44)
                        {
                            break;
                        }

                    }
                    if ("Dc".Equals(m_HeadKeys[i]))
                    {
                        // 種別
                        fname += "_" + val;
                    }
                }
                catch
                {
                    // 重複
                }
                pos += size;
            }

            return fname; // t43_2 の形
        }

        /**
         * @brief ヘッダ部取得
         * @param バイナリ情報
         * @return ヘッダ部文字列配列
         */
        public string[] getHeader(byte[] frombits)
        {
            string[] header = new string[m_HeadKeys.Length];
            byte[] bits = new byte[frombits.Length];
            reverse(frombits, bits);
            //
            int size;
            int pos = 0;
            int val = 0;


            for (int i = 0; i < m_HeadKeys.Length; i++)
            {
                size = m_HeadBits[i];
                val = decodeInt(bits, size, pos);

                header[i] = val.ToString();

                pos += size;
            }

            return header; // t43_2 の形
        }

        /**
         * @brief 災害危機通報のヘッダからタイプ取得してバイナリを読む
         * @param バイナリ情報
         */
        public void setBinary(byte[] bits)
        {
            string fname = getTypeRc(bits);
            setBinary(bits, fname);
            return;
        }

        /**
         * @brief 災害危機通報のバイナリから値を取得して保持する
         * @param バイナリ情報
         * @param フォーマット名
         */
        public void setBinary(byte[] frombits, string fname)
        {   
            //
            byte[] bits = new byte[frombits.Length];
            reverse(frombits, bits);
            m_DataMap = new Dictionary<string,string>();
            m_fileName = fname;

            int size;
            int pos = 0;
            long val = 0;

            if (m_KeysMap.ContainsKey(fname) == false)
            {
                // keyがない err
                return;
            }

            string[] m_Keys = m_KeysMap[fname];
            string[] m_Bits = m_BitsMap[fname];

            for (int i = 0; i < m_Bits.Length-1; i++)
            {
                if (m_Bits[i + 1].Length == 0 || m_Keys[i + 1].Length == 0)
                {
                    break;
                }
                size = int.Parse(m_Bits[i+1]);
                if (size > 64)
                {
                    val = decodeLong(bits, 64, pos);
                }
                else if (size > 32 && size <= 64)
                {
                    val = decodeLong(bits, size, pos);
                }
                else
                {
                    val = decodeInt(bits, size, pos);
                }

                try
                {
                    m_DataMap.Add(m_Keys[i+1], val.ToString());
                }
                catch
                {
                    // 重複
                }
                pos += size;
            }

			return;
    	}


        /**
         * @brief 4バイト単位でlittle endian をbig endianにする
         */
        public void reverse(byte[] bits, byte[]outb)
        {
            // 4バイト単位でlittle endian をビックに 
            byte[] tmp = { 0, 0, 0, 0 };
            for (int i = 0; i < bits.Length; i+=4)
            {
                
                for (int j = 0; j < 4; j++)
                {
                    tmp[j] = bits[i + j];
                }
                outb[i + 0] = tmp[3];
                outb[i + 1] = tmp[2];
                outb[i + 2] = tmp[1];
                outb[i + 3] = tmp[0];
            }
        }

        /**
         * @brief 災害危機通報の項目名（Rc,Dcなど）からバイナリにセットされた値を取得する
         * @param 項目名
         * @return セットされた値
         */
        public string getValue(string key)
        {
            string value = "";
            if (m_DataMap != null && m_DataMap.ContainsKey(key))
            {
                value = m_DataMap[key];
            }
            return value;
        }

        /**
         * @brief Longデータデコード要求
         */
        public long decodeLong(byte[] encodedData, int size, int startPos)
        {
            // invalid size
            if (size < 0 || size > LONG_BIT_SIZE)
            {
                //LogMng.AplLogError(TAG + "decodeLong invalid size : " + size);
                throw (new System.ArgumentOutOfRangeException("sizeが範囲外です"));
            }
            // invalid encodedData
            if (encodedData == null)
            {
                //LogMng.AplLogError(TAG + "decodeLong invalid encodedData : null");
                throw (new System.ArgumentNullException("encodedDataがNULLです"));
            }
            // invalid startPos
            int remain = (encodedData.Length * BYTE_BIT_SIZE) - startPos; // 残バッファbit数
            if (startPos < 0 || remain < 0)
            {
                //LogMng.AplLogError(TAG + "decodeLong invalid startPos : " + startPos);
                throw (new System.ArgumentOutOfRangeException("startPosが範囲外です"));
            }
            // buffer overrun
            if (size > remain)
            {
                //LogMng.AplLogError(TAG +
                //                String.Format(
                //                        "decodeLong buffer overrun : startPos={0}, size={1}, remain={2}, encodedData.length={3}",
                //                        startPos, size, remain, encodedData.Length));
                throw (new System.ArgumentOutOfRangeException("size + startPos が encodedData のサイズより大きいです"));
            }

            int buffLen = sizeToLength(size);
            byte[] buff = new byte[buffLen];
            decodeByteArray(encodedData, size, startPos, buff);
            ulong result = 0;
            int remainSize = size; // 残りの処理bit数
            for (int i = 0; remainSize > 0 && i < buffLen; i++)
            {
                int decodeSize = 0;
                if (remainSize > BYTE_BIT_SIZE)
                {
                    decodeSize = BYTE_BIT_SIZE;
                }
                else
                {
                    decodeSize = remainSize;
                }
                result =
                        (result << decodeSize)
                                | ((buff[i] & 0xffUL) >> (BYTE_BIT_SIZE - decodeSize));
                remainSize -= decodeSize;
            }

            return (long)result;
        }

        /**
         * @brief Intデータデコード要求
         */
        public int decodeInt(byte[] encodedData, int size, int startPos)
        {
            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            // invalid size  
            if (size < 0 || size > INT_BIT_SIZE)
            {
                //"sizeが範囲外です"
                return -1;
            }
            // invalid encodedData
            if (encodedData == null)
            {
                // "encodedDataがNULLです"
                return -1;
            }
            // invalid startPos
            int remain = (encodedData.Length * 8) - startPos; // 残バッファbit数
            if (startPos < 0 || remain < 0)
            {
                // "startPosが範囲外です"
                return -1;
            }
            // buffer overrun
            if (size > remain)
            {
                // "size + startPos が encodedData のサイズより大きいです"
                return -1;
            }

            int buffLen = sizeToLength(size);
            byte[] buff = new byte[buffLen];
            decodeByteArray(encodedData, size, startPos, buff);
            int result = 0;
            int remainSize = size; // 残りの処理bit数
            for (int i = 0; remainSize > 0 && i < buffLen; i++)
            {
                int decodeSize = 0;
                if (remainSize > 8)
                {
                    decodeSize = 8;
                }
                else
                {
                    decodeSize = remainSize;
                }

                //注　">>>"と">>"に関して
                //C#ではbyteは符号なし整数型なので">>"で論理シフトになります。
                //C#ではJavaのように">>>"で明示的に論理シフトにする必要はありません。(">>>"もない)
                result = (result << decodeSize)
                        | ((buff[i] & 0xff) >> (BYTE_BIT_SIZE - decodeSize));
                remainSize -= decodeSize;
            }

            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return result;
        }

        /**
         * @brief byteデータデコード要求
         */
        public void decodeByteArray(byte[] encodedData, int size, int startPos,
                byte[] decodedData)
        {
            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            // invalid size
            if (size < 0 || size > (decodedData.Length * BYTE_BIT_SIZE))
            {
                throw (new System.ArgumentOutOfRangeException("sizeが範囲外です"));
            }
            // invalid encodedData
            if (encodedData == null)
            {
                throw (new System.ArgumentNullException("encodedDataがNULLです"));
            }
            // buffer overrun
            int remain = (encodedData.Length * BYTE_BIT_SIZE) - startPos; // 残バッファbit数
            if (size > remain)
            {
                throw (new System.ArgumentOutOfRangeException("size + startPos が encodedData のサイズより大きいです"));
            }

            int pos = startPos;
            int remainSize = size; // 残りの処理bit数
            int buffSize = decodedData.Length;
            for (int i = 0; remainSize > 0 && i < buffSize; i++)
            {
                int decSize = 0;
                if (remainSize > BYTE_BIT_SIZE)
                {
                    decSize = BYTE_BIT_SIZE;
                }
                else
                {
                    decSize = remainSize;
                }

                decodedData[i] = decodeByte(encodedData, decSize, pos);
                remainSize -= decSize;
                pos += decSize;
            }
            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

        }

        /**
         * @brief byteデータデコード要求
         */
        private byte decodeByte(byte[] encodedData, int size, int startPos)
        {
            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            // invalid size
            if (size < 0 || size > BYTE_BIT_SIZE)
            {
                throw (new System.ArgumentOutOfRangeException("sizeが範囲外です"));
            }
            // invalid encodedData
            if (encodedData == null)
            {
                throw (new System.ArgumentNullException("encodedDataがNULLです"));
            }
            // buffer overrun
            int remain = (encodedData.Length * BYTE_BIT_SIZE) - startPos; // 残バッファbit数
            if (size > remain)
            {
                throw (new System.ArgumentOutOfRangeException("size + startPos が encodedData のサイズより大きいです"));
            }

            int indexByte = startPos / BYTE_BIT_SIZE;
            int indexBits = startPos % BYTE_BIT_SIZE; // MSBからのビット数
            int readSize = BYTE_BIT_SIZE - indexBits; // encodedData[indexByte]から読み出すbit数
            if (readSize > size)
            {
                readSize = size;
            }

            // encodedData[indexByte]から読み出し
            byte result = decodeByte(encodedData[indexByte], readSize, indexBits);

            // 読み込めなかったビットを次のバッファから読み込む
            int remainSize = size - readSize; // 残りの未処理bit数
            if (remainSize > 0)
            {
                byte temp = decodeByte(encodedData[indexByte + 1], remainSize, 0);
                result = (byte)(result | ((temp & 0xff) >> readSize)); // 不足分のbitを連結
            }

            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return result;
        }

        /**
         * @brief byteデータデコード要求
         */
        private byte decodeByte(byte encodedData, int size, int startPos)
        {
            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            // パラメータの妥当性は呼び元で担保する

            // Byte.SIZE = startPos + size + index
            int index = BYTE_BIT_SIZE - (startPos + size); // 残りの下位bit
            int mask = makeBitMask(index, size);
            byte result = (byte)((encodedData & mask) << startPos); // マスキングして左詰め
            // 引数が不整値だとmask=0x00となるため0x00が返却される

            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return result;
        }

        /**
         * @brief 指定ビットから任意のビットまでのビットマスクを作成する。
         */
        protected int makeBitMask(int index, int size)
        {

            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            uint result = 0;

            if ((index < 0) || (index >= INT_BIT_SIZE))
            {
                throw (new System.ArgumentOutOfRangeException("indexが範囲外です"));
            }
            if ((size <= 0) || (size > INT_BIT_SIZE))
            {
                throw (new System.ArgumentOutOfRangeException("sizeが範囲外です"));
            }
            // 上位側の不要なビット
            int remain = INT_BIT_SIZE - (index + size);
            if (remain < 0)
            {
                throw (new System.ArgumentOutOfRangeException("index + sizeが範囲外です"));
            }

            result = uint.MaxValue;

            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            // 上位側remainビット削除 下位側indexビット削除 位置合わせ
            return (int)(((result << remain) >> (remain + index)) << index);
        }

        /**
         * @brief ビット数を格納するのに必要なバイト数を算出する
         */
        public static int sizeToLength(int size)
        {
            return (size + BYTE_BIT_SIZE - 1) / BYTE_BIT_SIZE;
        }


    }
    
}

