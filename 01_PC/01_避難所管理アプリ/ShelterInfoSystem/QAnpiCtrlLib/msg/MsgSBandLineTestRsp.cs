/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using QAnpiCtrlLib.consts;

namespace QAnpiCtrlLib.msg
{
    /// <summary>
    /// Ｓ帯モニタデータ：回線試験データ応答 メッセージ管理
    /// 本メッセージのエンコード／デコードを実行する。
    /// </summary>
    public class MsgSBandLineTestRsp : MsgSBandData
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "MsgSBandLineTestRsp";

        /// <summary>
        /// このクラスのメッセージタイプ
        /// </summary>
        private const int MSG_TYPE = EncDecConst.VAL_MSG_SBAND_TST_RSP;

        /// <summary>
        /// このクラスのメッセージサイズ(bit)
        /// </summary>
        private const int MSG_SIZE = EncDecConst.SIZE_MSG_SBAND_TST_RSP;


	    /// <summary>
	    /// コンストラクタ
	    /// </summary>
        public MsgSBandLineTestRsp()
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
        /// ・なし
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

            // 予約×６
            startPos += SIZE_DATA_RESERVE * 6;


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
        /// ・なし
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
	        int startPos;


	        // ヘッダ部のデコード
            startPos = decodeHeader();
            if (startPos == 0)
            {
                return EncDecConst.DEC_VAL_NG;
            }


            // データ部のデコード

            // 予約×６
            startPos += SIZE_DATA_RESERVE * 6;


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
