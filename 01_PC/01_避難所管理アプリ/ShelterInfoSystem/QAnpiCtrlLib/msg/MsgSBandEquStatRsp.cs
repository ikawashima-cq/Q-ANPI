/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using QAnpiCtrlLib.consts;

namespace QAnpiCtrlLib.msg
{
    /// <summary>
    /// Ｓ帯モニタデータ：設備ステータス応答 メッセージ管理
    /// 本メッセージのエンコード／デコードを実行する。
    /// </summary>
    public class MsgSBandEquStatRsp : MsgSBandData
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "MsgSBandEquStatRsp";

        /// <summary>
        /// このクラスのメッセージタイプ
        /// </summary>
        private const int MSG_TYPE = EncDecConst.VAL_MSG_SBAND_EQU_RSP;

        /// <summary>
        /// このクラスのメッセージサイズ(bit)
        /// </summary>
        private const int MSG_SIZE = EncDecConst.SIZE_MSG_SBAND_EQU_RSP;

        /// <summary>
        /// データ部 装置ID
        /// </summary>
        [System.Xml.Serialization.XmlElement("equipmentId")]
        public int eqId;

        /// <summary>
        /// データ部 データ時刻
        /// </summary>
        public TimeUsec dataTime;

        /// <summary>
        /// データ部 シーケンスカウンタ
        /// </summary>
        [System.Xml.Serialization.XmlElement("sequenceCounter")]
        public int seqNum;

        /// <summary>
        /// データ部 設備ステータス情報
        /// </summary>
        [System.Xml.Serialization.XmlElement("equipmentStatusInfo")]
        public EquStatInfo equStatInfo;

        /// <summary>
        /// データ部 シーケンスカウンタ サイズ(bit)
        /// </summary>
        public const int SIZE_SEQ_NUM = 4 * BYTE_BIT_SIZE;


	    /// <summary>
	    /// コンストラクタ
	    /// </summary>
        public MsgSBandEquStatRsp()
        {
            msgType = MSG_TYPE;
            msgSize = MSG_SIZE;

            setDataIdAndDataSize(msgType, msgSize);

            int len = sizeToLength(msgSize);
            encodedData = new byte[len];

            dataTime = new TimeUsec();
            equStatInfo = new EquStatInfo();
	    }

	    /// <summary>
	    /// エンコード
        /// 
	    /// 【制限事項】
	    /// 以下のパラメータを、エンコード要求前に設定しておくこと。
        /// {@link eqId}
        /// {@link dataTime}
        /// {@link seqNum}
        /// {@link equStatInfo}
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

            // データ時刻
            startPos = dataTime.encode(encodedData, startPos);
            if (startPos == 0)
            {
                return EncDecConst.ENC_SIZE_NG;
            }

            // シーケンスカウンタ
            enc_data = seqNum;
            enc_size = SIZE_SEQ_NUM;
            encode(enc_data, enc_size, encodedData, startPos);
            startPos += enc_size;

            // 予約×４
            startPos += SIZE_DATA_RESERVE * 4;

            // 設備ステータス情報
            startPos = equStatInfo.encode(encodedData, startPos);
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
        /// {@link eqId}
        /// {@link dataTime}
        /// {@link seqNum}
        /// {@link equStatInfo}
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
	        int dec_size = 0;
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

            // データ時刻
            startPos = dataTime.decode(encodedData, startPos);
            if (startPos == 0)
            {
                return EncDecConst.DEC_VAL_NG;
            }

            // シーケンスカウンタ
            dec_size = SIZE_SEQ_NUM;
            seqNum = decodeInt(encodedData, dec_size, startPos);
            startPos += dec_size;

            // 予約×４
            startPos += SIZE_DATA_RESERVE * 4;

            // 設備ステータス情報
            startPos = equStatInfo.decode(encodedData, startPos);
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

            // データ時刻の値チェック
            int chk_time = dataTime.checkParam();
            if (chk_time != EncDecConst.OK)
            {
                result = chk_time;
            }

            // 設備ステータス情報の値チェック
            int chk_equ = equStatInfo.checkParam();
            if (chk_equ != EncDecConst.OK)
            {
                result = chk_equ;
            }

	        return result;
	    }


    }
}
