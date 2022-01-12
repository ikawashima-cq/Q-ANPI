/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.utils;
using QAnpiCtrlLib.log;

namespace QAnpiCtrlLib.msg
{
    /// <summary>
    /// Ｓ帯モニタデータ：Ｓ帯RTNデータ送信応答 メッセージ管理
    /// 本メッセージのエンコード／デコードを実行する。
    /// </summary>
    public class MsgSBandRtnSendRsp : MsgSBandData
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "MsgSBandRtnSendRsp";

        /// <summary>
        /// このクラスのメッセージタイプ
        /// </summary>
        private const int MSG_TYPE = EncDecConst.VAL_MSG_SBAND_RTN_RSP;

        /// <summary>
        /// このクラスのメッセージサイズ(bit)
        /// </summary>
        private const int MSG_SIZE = EncDecConst.SIZE_MSG_SBAND_RTN_RSP;

        /// <summary>
        /// データ部 応答結果コード
        /// </summary>
        [System.Xml.Serialization.XmlElement("resultCode")]
        public int rspResult;

        /// <summary>
        /// データ部 Ｓ帯RTN送信情報
        /// </summary>
        [System.Xml.Serialization.XmlElement("sbandRtnSendInfo")]
        public SBandRtnSendInfo rtnSendInfo;

        /// <summary>
        /// データ部 応答結果コード サイズ(bit)
        /// </summary>
        public const int SIZE_DATA_RSP_RESULT = 4 * BYTE_BIT_SIZE;


        //  装置固有結果コード（S帯RTN）

        /// <summary>
        /// 応答結果コード：送信時刻不正
        /// </summary>
        public const int RSP_RESULT_NG_SENDTIME = 8001;
        /// <summary>
        /// 応答結果コード：送信PN符号不正
        /// </summary>
        public const int RSP_RESULT_NG_PN = 8002;
        /// <summary>
        /// 応答結果コード：送信周波数不正
        /// </summary>
        public const int RSP_RESULT_NG_FREQ = 8003;
        /// <summary>
        /// 応答結果コード：要求受付数超過 
        /// </summary>
        public const int RSP_RESULT_NG_REQ_OVER = 8004;
        /// <summary>
        /// 応答結果コード：送信NG（TDMA同期外れ） 
        /// </summary>
        public const int RSP_RESULT_NG_TDMA = 8005;
        /// <summary>
        /// 応答結果コード：送信NG（システム情報不正） 
        /// </summary>
        public const int RSP_RESULT_NG_SYSINFO = 8006;
        /// <summary>
        /// 応答結果コード：送信NG（温度異常）
        /// </summary>
        public const int RSP_RESULT_NG_THERMAL = 8007;
        /// <summary>
        /// 応答結果コード：送信NG（FPGA更新中） 
        /// </summary>
        public const int RSP_RESULT_NG_FPGA_UPDATE = 8008;
        /// <summary>
        /// 応答結果コード：送信NG（モデム通信不可）
        /// </summary>
        public const int RSP_RESULT_NG_MODEM_COM = 8009;
        /// <summary>
        /// 応答結果コード：送信NG（GPS 時刻未取得） 
        /// </summary>
        public const int RSP_RESULT_NG_GPS_NO_TIME = 8010;
        /// <summary>
        /// 応答結果コード：送信NG（GPS 現在地情報未取得） 
        /// </summary>
        public const int RSP_RESULT_NG_GPS_NO_LOCATON = 8011;
        /// <summary>
        /// 応答結果コード：送信NG（送信PLLアラーム検出） 
        /// </summary>
        public const int RSP_RESULT_NG_PLL_ALARM = 8012;
        /// <summary>
        /// 応答結果コード：NG（その他） 
        /// </summary>
        public const int RSP_RESULT_NG_ETC = 8100;

        /// <summary>
        /// 装置固有応答結果コードのリスト
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public int[] DEV_RSP_RESULT_LIST = {   //  装置固有結果コード（S帯RTN）
                                               RSP_RESULT_NG_SENDTIME,
                                               RSP_RESULT_NG_PN,
                                               RSP_RESULT_NG_FREQ,
                                               RSP_RESULT_NG_REQ_OVER,
                                               RSP_RESULT_NG_TDMA,
                                               RSP_RESULT_NG_SYSINFO,
                                               RSP_RESULT_NG_THERMAL,
                                               RSP_RESULT_NG_FPGA_UPDATE,
                                               RSP_RESULT_NG_MODEM_COM,
                                               RSP_RESULT_NG_GPS_NO_TIME,
                                               RSP_RESULT_NG_GPS_NO_LOCATON,
                                               RSP_RESULT_NG_PLL_ALARM,
                                               RSP_RESULT_NG_ETC,
                                           };

	    /// <summary>
	    /// コンストラクタ
	    /// </summary>
        public MsgSBandRtnSendRsp()
        {
            msgType = MSG_TYPE;
            msgSize = MSG_SIZE;

            setDataIdAndDataSize(msgType, msgSize);

            int len = sizeToLength(msgSize);
            encodedData = new byte[len];

            rtnSendInfo = new SBandRtnSendInfo();
	    }

	    /// <summary>
	    /// エンコード
        /// 
	    /// 【制限事項】
	    /// 以下のパラメータを、エンコード要求前に設定しておくこと。
        /// {@link rspResult}
        /// {@link rtnSendInfo}
	    /// 
	    /// 【備考】
	    /// エンコードデータは以下に格納される。
	    /// {@link #encodedData}
	    /// </summary>
        /// <param name="paramCheck">
        /// True：有効値パラメータチェック実施
        /// False：有効値パラメータチェック未実施
        /// </param>
        /// <returns>
        /// エンコード結果。
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#ENC_VAL_NG}
        /// {@link EncDecConst#ENC_SIZE_NG}
        /// </returns>
	    public override int encode(Boolean paramCheck) 
        {
            int result = EncDecConst.OK;
            int startPos = 0;
            int enc_data;
            int enc_size;


	        // 有効値パラメータチェック
	        if (paramCheck)
            {
                int chk = checkEncParam(MSG_TYPE);
                if (chk != EncDecConst.OK)
                {
                    return chk;
	            }
	        }


	        // ヘッダ部のエンコード
            startPos = encodeHeader();


	        // データ部のエンコード

            // 応答結果コード
            enc_data = rspResult;
            enc_size = SIZE_DATA_RSP_RESULT;
            encode(enc_data, enc_size, encodedData, startPos);
            startPos += enc_size;

            // Ｓ帯RTN送信情報
            startPos = rtnSendInfo.encode(encodedData, startPos);
            if (startPos == 0)
            {
                return EncDecConst.ENC_SIZE_NG;
            }


            // ポストアンブル部のエンコード
            startPos = encodePostanble(startPos);


            // エンコードサイズチェック
            if (startPos != MSG_SIZE)
            {
               result = EncDecConst.ENC_SIZE_NG;
            }


            return result;
	    }

	    /// <summary>
        /// デコード
        /// 
        /// 【制限事項】
        /// 以下のパラメータにデコード対象のデータを設定しておくこと。
        /// {@link #encodedData}
        ///
        /// 【備考】
        /// デコードデータは以下に格納。
        /// {@link rspResult}
        /// {@link rtnSendInfo}
        /// 
        /// </summary>
        /// <param name="paramCheck">
        /// True：有効値パラメータチェック実施
        /// False：有効値パラメータチェック未実施
        /// </param>
        /// <returns>
        /// デコード結果。
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#DEC_GETTYPE_NG}
        /// {@link EncDecConst#DEC_VAL_NG}
        /// </returns>
	    public override int decode(Boolean paramCheck)
        {
            int result = EncDecConst.OK;
            int dec_size;
            int startPos;


	        // ヘッダ部のデコード
            startPos = decodeHeader();
            if (startPos == 0)
            {
                return EncDecConst.DEC_VAL_NG;
            }

            // ヘッダ部だけではサイズが同一なので要求と判定されるので応答に更新
            if ((EncDecConst.SIZE_MSG_SBAND_RTN_RSP == EncDecConst.SIZE_MSG_SBAND_RTN_REQ) &&
                (msgType == EncDecConst.VAL_MSG_SBAND_RTN_REQ))
            {
                msgType = EncDecConst.VAL_MSG_SBAND_RTN_RSP;
            }
            else
            {
                return EncDecConst.DEC_VAL_NG;
            }


            // データ部のデコード

            // 応答結果コード
            dec_size = SIZE_DATA_RSP_RESULT;
            rspResult = decodeInt(encodedData, dec_size, startPos);
            startPos += dec_size;

            // Ｓ帯RTN送信情報
            startPos = rtnSendInfo.decode(encodedData, startPos);
            if (startPos == 0)
            {
                return EncDecConst.DEC_VAL_NG;
            }


            // ポストアンブル部のデコード
            startPos = decodePostanble();
            if (startPos == 0)
            {
                return EncDecConst.DEC_VAL_NG;
            }


	        // 有効値パラメータチェック
	        if (paramCheck)
            {
                result = checkDecParam(MSG_TYPE);
	        }


            return result;
	    }

	    /// <summary>
	    /// 有効値有効値パラメータチェック
        /// Typeに依存するパラメータをチェックする
        /// </summary>
        /// <returns>
	    /// 有効値パラメータチェック結果
	    /// {@link EncDecConst#OK}
	    /// {@link EncDecConst#NG}
	    /// </returns>
	    protected override int checkParam()
        {
	        int result = EncDecConst.OK;
            int chk_code = EncDecConst.NG;

            // 装置固有応答結果コードの値チェック
            for (int i = 0; i < DEV_RSP_RESULT_LIST.Length; i++)
            {
                if (rspResult == DEV_RSP_RESULT_LIST[i])
                {
                    chk_code = EncDecConst.OK;
                    break;
                }
            }

            if (chk_code == EncDecConst.NG)
            {
                // 共通応答結果コードの値チェック
                for (int i = 0; i < COMMON_RSP_RESULT_LIST.Length; i++)
                {
                    if (rspResult == COMMON_RSP_RESULT_LIST[i])
                    {
                        chk_code = EncDecConst.OK;
                        break;
                    }
                }
            }

            if (chk_code == EncDecConst.NG)
            {
                result = EncDecConst.NG;
                LogMng.AplLogError(TAG + "checkParam : rspResult is invalid : " + rspResult);
            }

            // Ｓ帯RTN送信情報の値チェック
            int chk_equ = rtnSendInfo.checkParam();
            if (chk_equ != EncDecConst.OK)
            {
                result = EncDecConst.NG;
            }

	        return result;
	    }
    }
}
