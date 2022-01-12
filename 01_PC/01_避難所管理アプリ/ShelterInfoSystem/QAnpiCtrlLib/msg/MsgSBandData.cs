/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;

using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.log;

namespace QAnpiCtrlLib.msg
{
    /// <summary>
    /// Ｓ帯モニタデータ共通部
    /// 各TypeのＳ帯モニタデータは本クラスを継承して利用すること
    /// 共通フィールドは以下
    /// {@link #msgType}
    /// {@link #msgSize}
    /// としType別Ｓ帯モニタデータは各Type用の子クラスで処理を実装すること
    /// 
    /// @see EncodeManager
    /// @see DecodeManager
    /// </summary>
    public abstract class MsgSBandData : EncodeManager
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "MsgSBandData";

        /// <summary>
        /// ヘッダ部 開始コードのサイズ(bit)
        /// </summary>
        public const int SIZE_HEAD_START_CODE = 4 * BYTE_BIT_SIZE;

        /// <summary>
        /// ヘッダ部 データサイズのサイズ(bit)
        /// </summary>
        public const int SIZE_HEAD_DATA_SIZE = 4 * BYTE_BIT_SIZE;

        /// <summary>
        /// ヘッダ部 データIDのサイズ(bit)
        /// </summary>
        public const int SIZE_HEAD_DATA_ID = 4 * BYTE_BIT_SIZE;

        /// <summary>
        /// データ部 装置ID サイズ(bit)
        /// </summary>
        public const int SIZE_DATA_EQ_ID = 4 * BYTE_BIT_SIZE;

        /// <summary>
        /// データ部 予約のサイズ(bit)
        /// </summary>
        public const int SIZE_DATA_RESERVE = 4 * BYTE_BIT_SIZE;

        /// <summary>
        /// ポストアンブル部 終了コードのサイズ(bit)
        /// </summary>
        public const int SIZE_POST_END_CODE = 4 * BYTE_BIT_SIZE;


        /// <summary>
        /// エンコード後データの格納エリア
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
	    public byte[] encodedData;

        /// <summary>
        /// Ｓ帯モニタデータのデータ種別。
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public int msgType;

        /// <summary>
        /// Ｓ帯モニタデータのデータサイズ。(bit)
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public int msgSize;

        /// <summary>
        /// Ｓ帯モニタデータのデータサイズ。(byte)
        /// </summary>
        public int dataSize;

        /// <summary>
        /// Ｓ帯モニタデータのデータ種別、データサイズ、データ名の組み合わせ
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public static object[][] sbandDataDef = new object[9][]
        {
            new object[] { EncDecConst.VAL_MSG_SBAND_EQU_REQ,  EncDecConst.SIZE_MSG_SBAND_EQU_REQ, Properties.Resources.W_CTF202_dat_nam_01 },
            new object[] { EncDecConst.VAL_MSG_SBAND_EQU_RSP,  EncDecConst.SIZE_MSG_SBAND_EQU_RSP, Properties.Resources.W_CTF202_dat_nam_02 },
            new object[] { EncDecConst.VAL_MSG_SBAND_TST_REQ,  EncDecConst.SIZE_MSG_SBAND_TST_REQ, Properties.Resources.W_CTF202_dat_nam_03 },
            new object[] { EncDecConst.VAL_MSG_SBAND_TST_RSP,  EncDecConst.SIZE_MSG_SBAND_TST_RSP, Properties.Resources.W_CTF202_dat_nam_04 },
            new object[] { EncDecConst.VAL_MSG_SBAND_FWD_REQ,  EncDecConst.SIZE_MSG_SBAND_FWD_REQ, Properties.Resources.W_CTF202_dat_nam_05 },
            new object[] { EncDecConst.VAL_MSG_SBAND_FWD_RSP,  EncDecConst.SIZE_MSG_SBAND_FWD_RSP, Properties.Resources.W_CTF202_dat_nam_06 },
            new object[] { EncDecConst.VAL_MSG_SBAND_RTN_REQ,  EncDecConst.SIZE_MSG_SBAND_RTN_REQ, Properties.Resources.W_CTF202_dat_nam_07 },
            new object[] { EncDecConst.VAL_MSG_SBAND_RTN_RSP,  EncDecConst.SIZE_MSG_SBAND_RTN_RSP, Properties.Resources.W_CTF202_dat_nam_08 },
            new object[] { EncDecConst.VAL_MSG_SBAND_DEV_RSP,  EncDecConst.SIZE_MSG_SBAND_DEV_RSP, Properties.Resources.W_CTF202_dat_nam_09 },
        };

        /// <summary>
        /// sbandDataDefのデータ種別のカラム番号
        /// </summary>
        public const int COL_SBANDDATA_TYPE = 0;

        /// <summary>
        /// sbandDataDefのデータサイズのカラム番号
        /// </summary>
        public const int COL_SBANDDATA_SIZE = 1;

        /// <summary>
        /// sbandDataDefのデータ名のカラム番号
        /// </summary>
        public const int COL_SBANDDATA_NAME = 2;

        /// <summary>
        /// スタートコード
        /// </summary>
        public int startCode = CommonConst.SBAND_MSG_START_CODE;

        /// <summary>
        /// データID
        /// XML出力用
        /// </summary>
        [System.Xml.Serialization.XmlElement("dataId")]
        public int dId;

        /// <summary>
        /// エンドコード
        /// </summary>
        public int endCode = CommonConst.SBAND_MSG_END_CODE;

        /// <summary>
        /// 装置ID（固定データ送信用）
        /// </summary>
        public const int FIXED_EQ_ID = 0x77;


        //  共通データ応答結果コード

        /// <summary>
        /// 応答結果コード：OK
        /// </summary>
        public const int RSP_RESULT_OK = 0;
        /// <summary>
        /// 応答結果コード：データサイズ不正
        /// </summary>
        public const int RSP_RESULT_NG_DATA_SIZE = -1;
        /// <summary>
        /// 応答結果コード：セッション接続リジェクト
        /// </summary>
        public const int RSP_RESULT_NG_SESSION_OVER = -2;
        /// <summary>
        /// 応答結果コード：データID不正
        /// </summary>
        public const int RSP_RESULT_NG_DATA_ID = -3;
        /// <summary>
        /// 応答結果コード：装置ID不正
        /// </summary>
        public const int RSP_RESULT_NG_EQ_ID = -4;
        /// <summary>
        /// 応答結果コード：ポストアンブル部不正
        /// </summary>
        public const int RSP_RESULT_NG_POSTANBLE = -5;
        /// <summary>
        /// 応答結果コード：処理タイムアウト
        /// </summary>
        public const int RSP_RESULT_NG_TIMEOUT = -6;
        /// <summary>
        /// 応答結果コード：データ内容不正ト
        /// </summary>
        public const int RSP_RESULT_NG_DATA_CONTENT = 1;
        /// <summary>
        /// 応答結果コード：制御モード不一致
        /// </summary>
        public const int RSP_RESULT_NG_CTRL_MODE = 2;
        /// <summary>
        /// 応答結果コード：装置リブート
        /// </summary>
        public const int RSP_RESULT_NG_REBOOT = 10;

        /// <summary>
        /// 共通データ応答結果コードのリスト
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public int[] COMMON_RSP_RESULT_LIST = {   // 共通データ応答結果コード
                                                  RSP_RESULT_OK,
                                                  RSP_RESULT_NG_DATA_SIZE,
                                                  RSP_RESULT_NG_SESSION_OVER,
                                                  RSP_RESULT_NG_DATA_ID,
                                                  RSP_RESULT_NG_EQ_ID,
                                                  RSP_RESULT_NG_POSTANBLE,
                                                  RSP_RESULT_NG_TIMEOUT,
                                                  RSP_RESULT_NG_DATA_CONTENT,
                                                  RSP_RESULT_NG_CTRL_MODE,
                                                  RSP_RESULT_NG_REBOOT,
                                              };


        /// <summary>
        /// コンストラクタ
        ///	encodedDataのインスタンスを生成する
        /// </summary>
        public MsgSBandData()
        {

        }

        /// <summary>
        /// Ｓ帯モニタデータのヘッダ部をエンコードする
        /// エンコードデータは{@link #encodedData}に格納される
        /// 【制約事項】実行前に{@link #msgType}を設定すること
        /// </summary>
        /// <returns>エンコードしたビット数</returns>
	    public virtual int encodeHeader()
        {
	        int dataId;
            int enc_data;
            int enc_size;
	        int startPos = 0;

            // データID（下三桁取得）
            dataId = msgType % EncDecConst.CALC_DATAID_DENOMI;

            // 開始コード
            enc_data = startCode;
            enc_size = SIZE_HEAD_START_CODE;
            encode(enc_data, enc_size, encodedData, startPos);
	        startPos += enc_size;

            // データサイズ(byte)
            enc_data = dataSize;
            enc_size = SIZE_HEAD_DATA_SIZE;
            encode(enc_data, enc_size, encodedData, startPos);
	        startPos += enc_size;

            // データID
            enc_data = dataId;
            enc_size = SIZE_HEAD_DATA_ID;
            encode(enc_data, enc_size, encodedData, startPos);
	        startPos += enc_size;

	        return startPos;
	    }

        /// <summary>
        /// Ｓ帯モニタデータのポストアンブル部をエンコードする
        /// エンコードデータは{@link #encodedData}に格納される
        /// </summary>
        /// <returns>エンコードしたビット数</returns>
        public virtual int encodePostanble(int startPos)
        {
            int enc_data;
            int enc_size;

            // 終了コード
            enc_data = endCode;
            enc_size = SIZE_POST_END_CODE;
            encode(enc_data, enc_size, encodedData, startPos);
	        startPos += enc_size;

	        return startPos;
	    }


        /// <summary>
        /// Ｓ帯モニタデータのヘッダ部をデコードする
        /// デコードしたデータは以下に格納される
        /// {@link #msgType}
        /// {@link #msgSize}
        /// 【制約事項】実行前に{@link #encodedData}にエンコード後のデータ（受信データ）を設定しておくこと
        /// </summary>
        /// <returns>デコードしたビット数</returns>
	    public virtual int decodeHeader() 
        {
            int dec_pos = 0;

            dec_pos = decodeSBandDataHeader(encodedData, this, out msgSize);
            if (dec_pos == 0)
            {
                LogMng.AplLogError(TAG + "decodeHeader : decode error.");
            }

            return dec_pos;
	    }

        /// <summary>
        /// Ｓ帯モニタデータのポストアンブル部をデコードする
        /// </summary>
        /// <returns>デコードしたビット数</returns>
        public virtual int decodePostanble()
        {
            int dec_data;
            int dec_size;
	        int startPos = 0;

            // 終了コードのデコード
            dec_size = SIZE_POST_END_CODE;
            startPos = msgSize - dec_size;
            dec_data = decodeInt(encodedData, dec_size, startPos);
            if (dec_data != CommonConst.SBAND_MSG_END_CODE)
            {
                LogMng.AplLogError(TAG + "decodePostanble : end code is invalid : " + dec_data);
                return 0;
            }
            endCode = dec_data;
            startPos += dec_size;

	        return startPos;
	    }

        /// <summary>
        /// エンコード要求
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
        public abstract int encode(Boolean paramCheck);

        /// <summary>
        /// デコード要求
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
        public abstract int decode(Boolean paramCheck);

        /// <summary>
        /// 有効値パラメータチェック(エンコード用)
        /// Typeに依存しないパラメータはこのメソッドにチェック処理を実装する
        /// </summary>
        /// <param name="type">正解のType値</param>
        /// <returns>
        /// 有効値パラメータチェック結果
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#ENC_VAL_NG}
        /// {@link EncDecConst#ENC_SIZE_NG}
        /// </returns>
        protected virtual int checkEncParam(int type)
        {
            int result = EncDecConst.OK;

            // Type別パラメータ
            if (checkParam() == EncDecConst.NG)
            {
                // checkParam()内でエラーログを出すのでここではログ出力不要
                result = EncDecConst.ENC_VAL_NG;
            }

            // Type(範囲チェックはしない)
            if (msgType != type)
            {
                LogMng.AplLogError(TAG + "checkEncParam : msgType is invalid : msgType = "
                                + msgType);
                result = EncDecConst.ENC_VAL_NG;
            }

            return result;
        }

        /// <summary>
        /// 有効値パラメータチェック(デコード用)
        /// Typeに依存しないパラメータはこのメソッドにチェック処理を実装する
        /// </summary>
        /// <param name="type">正解のType値</param>
        /// <returns>
        /// 有効値パラメータチェック結果
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#DEC_GETTYPE_NG}
        /// {@link EncDecConst#DEC_VAL_NG}
        /// </returns>
        protected virtual int checkDecParam(int type)
        {
            int result = EncDecConst.OK;


            // Type別パラメータ
            if (checkParam() == EncDecConst.NG)
            {
                // checkParam()内でエラーログを出すのでここではログ出力不要
                result = EncDecConst.DEC_VAL_NG;
            }

            // Type(範囲チェックはしない)
            // DEC_GETTYPE_NGの判定
            if (msgType != type)
            {
                LogMng.AplLogError(TAG + "checkDecParam : msgType is invalid : msgType = "
                                + msgType);
                result = EncDecConst.DEC_GETTYPE_NG;
            }


            return result;
        }

        /// <summary>
        /// 有効値パラメータチェック
        /// Typeに依存するパラメータをチェックする
        /// </summary>
        /// <returns>
        /// 有効値パラメータチェック結果
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#NG}
        /// </returns>
        protected abstract int checkParam();

        /// <summary>
        /// S帯モニタデータのデータIDとデータサイズを設定する
        /// XML出力用
        /// </summary>
        /// <param name="msgType">メッセージ種別</param>
        /// <param name="msgSize">メッセージサイズ(bit)</param>
        protected void setDataIdAndDataSize(int msgType, int msgSize)
        {
            dId = msgType % EncDecConst.CALC_DATAID_DENOMI;
            dataSize = msgSize / BYTE_BIT_SIZE; 
        }
        
    }
}
