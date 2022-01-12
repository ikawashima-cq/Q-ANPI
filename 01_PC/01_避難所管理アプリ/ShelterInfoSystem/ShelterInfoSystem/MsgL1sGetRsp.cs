/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2017. All rights reserved.
 */
/**
 * @file    MsgL1sGetReq.cs
 * @brief   L1S取得応答、メッセージ取得クラス
 */
using System;
using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.msg;

namespace ShelterInfoSystem
{
    /// <summary>
    /// Ｓ帯モニタデータ：Ｓ帯L1Sデータ取得応答 メッセージ管理
    /// 本メッセージのエンコード／デコードを実行する。
    /// </summary>
    /**
     * @class MsgL1sGetRsp
     * @brief L1Sデータ取得応答 メッセージ管理。エンコード／デコードを実行する。
     * ※Ｓ帯モニタデータではないが共通処理のためMsgSBandDataを継承
     */
    public class MsgL1sGetRsp : MsgSBandData
    {
        /**
         * @brief Log出力時に使用する文字列
         */
        private const String TAG = "MsgSBandL1sGetRsp";

        /**
         * @brief このクラスのメッセージタイプ
         */
        private const int MSG_TYPE = EncDecConst.VAL_MSG_L1S_RSP;

        /**
         * @brief このクラスのメッセージサイズ(bit)
         */
        private const int MSG_SIZE = EncDecConst.SIZE_MSG_L1S_RSP;
        
        /**
         * @brief データ部 L1S受信情報
         */
        [System.Xml.Serialization.XmlElement("sbandL1sReciveInfo")]
        public L1sRcvInfo l1sRcvInfo;

        /**
         * @brief コンストラクタ
         */
        public MsgL1sGetRsp()
        {
            msgType = MSG_TYPE;
            msgSize = MSG_SIZE;

            setDataIdAndDataSize(msgType, msgSize);

            int len = sizeToLength(msgSize);
            encodedData = new byte[len];

            l1sRcvInfo = new L1sRcvInfo();
	    }

        /**
         * @brief エンコード
         * 【制限事項】
         * 以下のパラメータを、エンコード要求前に設定しておくこと。
         * {@link l1sRcvInfo}
         * 【備考】
         * エンコードデータは以下に格納される。
         * {@link #encodedData}
         * @param True：有効値パラメータチェック実施 / False：有効値パラメータチェック未実施
         * @return エンコード結果。
         * {@link EncDecConst#OK}
         * {@link EncDecConst#ENC_VAL_NG}
         * {@link EncDecConst#ENC_SIZE_NG}
         */
        public override int encode(Boolean paramCheck) 
        {
            int result = EncDecConst.OK;
            int startPos = 0;


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

            // Ｓ帯L1S受信情報
            startPos = l1sRcvInfo.encode(encodedData, startPos);
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

        /**
         * @brief デコード
         * 【制限事項】
         * 以下のパラメータにデコード対象のデータを設定しておくこと。
         * {@link #encodedData}
         * 【備考】
         * デコードデータは以下に格納。
         * {@link l1sRcvInfo}
         * @param True：有効値パラメータチェック実施 / False：有効値パラメータチェック未実施
         * @return デコード結果。
         * {@link EncDecConst#OK}
         * {@link EncDecConst#DEC_GETTYPE_NG}
         * {@link EncDecConst#DEC_VAL_NG}
         */
        public override int decode(Boolean paramCheck)
        {
            int result = EncDecConst.OK;
	        int startPos;


	        // ヘッダ部のデコード
            startPos = decodeHeader();
            if (startPos == 0)
            {
                return EncDecConst.DEC_VAL_NG;
            }

            // データ部のデコード

            // L1S受信情報
            startPos = l1sRcvInfo.decode(encodedData, startPos);
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
        /**
         * @brief Typeに依存するパラメータをチェックする
         * @return 有効値パラメータチェック結果
         * {@link EncDecConst#OK}
         * {@link EncDecConst#NG}
         */
        protected override int checkParam()
        {
	        int result = EncDecConst.OK;

            // L1S受信情報の値チェック
            int chk_equ = l1sRcvInfo.checkParam();
            if (chk_equ != EncDecConst.OK)
            {
                result = chk_equ;
            }

	        return result;
	    }
    }
}
