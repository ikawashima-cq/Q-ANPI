/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using QAnpiCtrlLib.consts;

namespace QAnpiCtrlLib.msg
{
    /// <summary>
    /// Ｓ帯モニタデータ：設備ステータス(要求) メッセージ管理
    /// 本メッセージのエンコード／デコードを実行する。
    /// </summary>
    public class MsgSBandEquStatReq : MsgSBandData
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "MsgSBandEquStatReq";

        /// <summary>
        /// このクラスのメッセージタイプ
        /// </summary>
        private const int MSG_TYPE = EncDecConst.VAL_MSG_SBAND_EQU_REQ;

        /// <summary>
        /// このクラスのメッセージサイズ(bit)
        /// </summary>
        private const int MSG_SIZE = EncDecConst.SIZE_MSG_SBAND_EQU_REQ;

        /// <summary>
        /// データ部 装置ID
        /// </summary>
        [System.Xml.Serialization.XmlElement("equipmentId")]
        public int eqId;


	    /// <summary>
	    /// コンストラクタ
	    /// </summary>
        public MsgSBandEquStatReq()
        {
            msgType = MSG_TYPE;
            msgSize = MSG_SIZE;

            setDataIdAndDataSize(msgType, msgSize);

            int len = sizeToLength(msgSize);
            encodedData = new byte[len];
	    }

	    /// <summary>
	    /// エンコード
	    /// 【制限事項】
	    /// 以下のパラメータを、エンコード要求前に設定しておくこと。
        /// {@link #eqId}
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
            // 装置ID
            enc_data = eqId;
            enc_size = SIZE_DATA_EQ_ID;
            encode(enc_data, enc_size, encodedData, startPos);
            startPos += enc_size;


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
        /// 【制限事項】
        /// 以下のパラメータにデコード対象のデータを設定しておくこと。
        /// {@link #encodedData}
        ///
        /// 【備考】
        /// デコードデータは以下に格納。
        /// {@link eqId}
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


            // データ部のデコード

            // 装置ID
            dec_size = SIZE_DATA_EQ_ID;
            eqId = decodeInt(encodedData, dec_size, startPos);
            startPos += dec_size;


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

            // チェック項目なし

	        return result;
	    }
    }
}
