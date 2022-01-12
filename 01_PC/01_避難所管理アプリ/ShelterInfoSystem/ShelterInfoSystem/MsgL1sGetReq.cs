/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2017. All rights reserved.
 */
/**
 * @file    MsgL1sGetReq.cs
 * @brief   L1S取得要求クラス
 */
using System;
using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.msg;

namespace ShelterInfoSystem
{
    /**
     * @class MsgL1sGetReq
     * @brief L1Sデータ取得要求 メッセージ管理。本メッセージのエンコード／デコードを実行する。
     * ※Ｓ帯モニタデータではないが共通処理のためMsgSBandDataを継承
     */
    public class MsgL1sGetReq : MsgSBandData
    {
        /**
         * @brief Log出力時に使用する文字列
         */
        private const String TAG = "MsgSBandL1sGetReq";

        /**
         * @brief このクラスのメッセージタイプ
         */
        private const int MSG_TYPE = EncDecConst.VAL_MSG_L1S_REQ;

        /**
         * @brief このクラスのメッセージサイズ(bit)
         */
        private const int MSG_SIZE = EncDecConst.SIZE_MSG_L1S_REQ;

        /**
         * @brief データ部 装置ID
         */
        [System.Xml.Serialization.XmlElement("equipmentId")]
        public int eqId;

        /**
         * @brief 予約エリア　4byte×3個
         */
        [System.Xml.Serialization.XmlIgnore]
        public byte[] reserved;

        /**
         * @brief 予約エリア  XML出力用
         */
        [System.Xml.Serialization.XmlElement("reserved")]
        public string reservedStr = "";

        /**
         * @brief コンストラクタ
         */
        public MsgL1sGetReq(int msgtype)
        {
            msgType = msgtype;

            msgSize = MSG_SIZE;

            setDataIdAndDataSize(msgType, msgSize);

            int len = sizeToLength(msgSize);
            encodedData = new byte[len];

            reserved = new byte[sizeToLength(SIZE_DATA_RESERVE * 3)];
	    }

        /**
         * @brief エンコード
         * 【制限事項】
         * 以下のパラメータを、エンコード要求前に設定しておくこと。
         * {@link #eqId}
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

            // 予約×３
            enc_size = SIZE_DATA_RESERVE * 3;
            encode(reserved, enc_size, encodedData, startPos);
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

        /**
         * @brief デコード
         * 【制限事項】
         * 以下のパラメータにデコード対象のデータを設定しておくこと。
         * {@link #encodedData}
         * 【備考】
         * デコードデータは以下に格納。
         * {@link eqId}
         * @param True：有効値パラメータチェック実施 / False：有効値パラメータチェック未実施
         * @return デコード結果。
         * {@link EncDecConst#OK}
         * {@link EncDecConst#DEC_GETTYPE_NG}
         * {@link EncDecConst#DEC_VAL_NG}
         */
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

            // 予約×３
            dec_size = SIZE_DATA_RESERVE * 3;
            decodeByteArray(encodedData, dec_size, startPos, reserved);
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

        /**
         * @brief Typeに依存するパラメータをチェックする
         * @return 有効値パラメータチェック結果
         * {@link EncDecConst#OK}
         * {@link EncDecConst#NG}
         */
	    protected override int checkParam()
        {
	        int result = EncDecConst.OK;

            // チェック項目なし

	        return result;
	    }
    }
}
