/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2017. All rights reserved.
 */
/**
 * @file    L1sRcvInfo.cs
 * @brief   L1S取得要求クラス
 */
using System;
using QAnpiCtrlLib.log;
using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.utils;
using QAnpiCtrlLib.msg;

namespace ShelterInfoSystem
{
    /**
     * @brief L1Sデータ部 L1S受信情報 管理
     */
    public class L1sRcvInfo : EncodeManager
    {
        // データ構造
        // --------------------------------------
        // Offset  Size    項目
        // --------------------------------------
        // --------------------------------------
        //   合計  250 bit (32 byte)

        /**
         * @brief Log出力時に使用する文字列
         */
        private const String TAG = "L1sRcvInfo";

        /**
         * @brief 受信時刻
         */
        [System.Xml.Serialization.XmlElement("reviceTime")]
        public TimeUsec rcvTime;

        /**
         * @brief 端末宛データ
         */
        [System.Xml.Serialization.XmlIgnore]
        public byte[] fwdData;

        /**
         * @brief 端末宛データ：メッセージタイプ
         */
        [System.Xml.Serialization.XmlIgnore]
        public int l1sDataMsgType;


        /**
         * @brief 端末宛データ：メッセージ文字列
         */
        [System.Xml.Serialization.XmlElement("msg")]
        public string fwdDataStr = "";

        /**
         * @brief 確認信号(L1S)のサイズ(bit)
         */
        public const int SIZE_L1S_DATA = 32 * BYTE_BIT_SIZE;

        /**
         * @brief コンストラクタ
         */
        public L1sRcvInfo()
        {
            rcvTime = new TimeUsec();

            int len = sizeToLength(SIZE_L1S_DATA);
            fwdData = new byte[len];
        }

        /**
         * @brief エンコード
         * @param エンコードデータの保存先
         * @param エンコードデータの保存位置
         * @return エンコード終了位置 0の場合は処理失敗
         */
        public int encode(byte[] encodedData, int startPos)
        {
            int result = 0;
            int enc_size;

            try
            {
                // L1S受信情報：受信時刻
                startPos = this.rcvTime.encode(encodedData, startPos);
                if (startPos == 0)
                {
                    return result;
                }

                // L1S受信情報：確認信号(L1S)
                enc_size = SIZE_L1S_DATA;
                encode(this.fwdData, enc_size, encodedData, startPos);
                startPos += enc_size;
                
                result = startPos;
            }
            catch (Exception ex)
            {
                result = 0;
                LogMng.AplLogError("SBandL1sRcvInfo encode() error:" + ex.Message);
            }

            return result;
        }

        /**
         * @brief デコード
         * @param デコード対象のデータ
         * @param デコード開始位置
         * @return デコード終了位置 0の場合は処理失敗
         */
        public int decode(byte[] encodedData, int startPos)
        {
            int result = 0;
            int dec_size = 0;

            try
            {
                // L1S受信情報：受信時刻
                startPos = this.rcvTime.decode(encodedData, startPos);
                if (startPos == 0)
                {
                    return result;
                }

                // L1S受信情報：確認信号(L1S)
                dec_size = SIZE_L1S_DATA;
                decodeByteArray(encodedData, dec_size, startPos, this.fwdData);
                startPos += dec_size;

                result = startPos;

                // Ｓ帯安否確認信号(L1S)の端末宛メッセージのType取得
                DecodeManager dm = new DecodeManager();
                TypeAndSystemInfo tasi = dm.decodeTypeAndSystemInfo(
                    this.fwdData, MsgToTerminal.SIZE, false);
                l1sDataMsgType = tasi.msgType;
            }
            catch (Exception ex)
            {
                result = 0;
                LogMng.AplLogError("SBandL1sRcvInfo decode() error:" + ex.Message);
            }

            return result;
        }

        /**
         * @brief 有効値パラメータチェック
         * @return 有効値パラメータチェック結果
         * {@link EncDecConst#OK}
         * {@link EncDecConst#NG}
         */
        public int checkParam()
        {
            int result = EncDecConst.OK;

            // 受信時刻の値チェック
            int chk_time = rcvTime.checkParam();
            if (chk_time != EncDecConst.OK)
            {
                LogMng.AplLogError(TAG + "checkParam : rcvTime is invalid");
                result = EncDecConst.NG;
            }

            return result;
        }


    }
}
