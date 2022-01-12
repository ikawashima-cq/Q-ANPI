/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QAnpiCtrlLib.consts;
using QAnpiCtrlLib.utils;

using QAnpiCtrlLib.log;

namespace QAnpiCtrlLib.msg
{
    /// <summary>
    /// Type100：平常時送達確認
    /// Type100エンコード・デコードクラス
    /// Type100メッセージのエンコードを実行する。
    /// Type100メッセージのデコードを実行する。
    /// </summary>
    public class MsgType100 : MsgToTerminal
    {
        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "MsgType100";
        /// <summary>
        /// このクラスのメッセージタイプ
        /// </summary>
        private const int MSG_TYPE = EncDecConst.VAL_MSG_TYPE100;

        // Type 別端末宛メッセージ
        /// <summary>
        /// 安否情報送達確認数。範囲{@value #ACKNUM_MIN}～{@value #ACKNUM_MAX}。有効bitは右詰
        /// {@value #ACKNUM_SIZE}bitとなる。
        /// </summary>
        public int ackNum;
        /// <summary>
        /// {@link #ackNum}の最小値
        /// </summary>
        public const int ACKNUM_MIN = 1;
        /// <summary>
        /// {@link #ackNum}の最大値
        /// </summary>
        public const int ACKNUM_MAX = 88;
        /// <summary>
        /// {@link #ackNum}の有効ビット数
        /// </summary>
        public const int ACKNUM_SIZE = 8;

        /// <summary>
        /// 送達確認
        /// </summary>
        public class Ack
        {
            /// <summary>
            /// 衛星通信端末の識別子
            /// </summary>
            public int cid;
            /// <summary>
            /// {@link #cid}の最小値
            /// </summary>
            public const int CID_MIN = consts.CommonConst.CID_MIN;
            /// <summary>
            /// {@link #cid}の最大値
            /// </summary>
            public const int CID_MAX = consts.CommonConst.CID_MAX;
            /// <summary>
            /// {@link #cid}の有効ビット数
            /// </summary>
            public const int CID_SIZE = 25;

            /// <summary>
            /// Slot番号
            /// </summary>
            public int slotNum;
            /// <summary>
            /// {@link #slotNum}の最小値
            /// </summary>
            public const int SLOTNUM_MIN = 0;
            /// <summary>
            /// {@link #slotNum}の最大値
            /// </summary>
            public const int SLOTNUM_MAX = 2;
            /// <summary>
            /// {@link #slotNum}の有効ビット数
            /// </summary>
            public const int SLOTNUM_SIZE = 2;

            /// <summary>
            /// SubSlotビットマップ
            /// </summary>
            public int subslotBitmap;
            /// <summary>
            /// {@link #subslotBitmap}の最小値
            /// </summary>
            public const int SUBSLOTBITMAP_MIN = 0;
            /// <summary>
            /// {@link #subslotBitmap}の最大値
            /// </summary>
            public const int SUBSLOTBITMAP_MAX = 1023;
            /// <summary>
            /// {@link #subslotBitmap}の有効ビット数
            /// </summary>
            public const int SUBSLOTBITMAP_SIZE = 10;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public Ack()
            {
                // 端末宛メッセージのデータ部が仕様変更後に不要となった.
            }
            /// <summary>
            /// 有効値パラメータチェック
            /// </summary>
            /// <returns>
            /// 有効値パラメータチェック結果
            /// {@link EncDecConst#OK}
            /// {@link EncDecConst#NG}
            /// </returns>
            public int checkParam()
            {
                int res = EncDecConst.OK;

                // 衛星通信端末の識別子
                if (CommonUtils.isOutOfRange(cid, CID_MIN, CID_MAX))
                {
                    LogMng.AplLogError(TAG + "checkParam : cid is invalid : cid = "
                            + cid);
                    res = EncDecConst.NG;
                }

                // Slot番号
                if (CommonUtils.isOutOfRange(slotNum, SLOTNUM_MIN, SLOTNUM_MAX))
                {
                    LogMng.AplLogError(TAG + "checkParam : slotNum is invalid : slotNum = "
                            + slotNum);
                    res = EncDecConst.NG;
                }


                // SubSlotビットマップ
                if (CommonUtils.isOutOfRange(subslotBitmap, SUBSLOTBITMAP_MIN, SUBSLOTBITMAP_MAX))
                {
                    LogMng.AplLogError(TAG + "checkParam : subslotBitmap is invalid : subslotBitmap = "
                            + subslotBitmap);
                    res = EncDecConst.NG;
                }

                return res;
            }

#if false
            /// <summary>
            /// 端末発データ部を渡されたバイト型配列に設定する
            /// </summary>
            /// <param name="dataToSet">データを詰めるバイト型配列</param>
            private void setFromTermMsg(byte[] dataToSet)
            {
                EncodeManager em = new EncodeManager();

                int pos = 0;
                int size = 0;
                int data = 0;

                //メッセージ種別
                size = MSGTYPE_SIZE;
                data = msgType;
                em.encode(data, size, dataToSet, pos);
                pos += size;

                //グループID
                size = GID_SIZE;
                data = gId;
                em.encode(data, size, dataToSet, pos);
                pos += size;

                //data part
                size = DATA_PART_SIZE;
                em.encode(dataPart, size, dataToSet, pos);
            }
#endif

        }

        /// <summary>
        /// 送達確認
        /// リソースはコンストラクタで確保する。->端末発メッセージデータ部の確保は不要になった
        /// </summary>
        [System.Xml.Serialization.XmlArray("acks")]
        [System.Xml.Serialization.XmlArrayItem("ack", typeof(Ack))]
        public Ack[] acks;

	    /// <summary>
	    /// Padding。有効bitは右詰{@value #PADDING_SIZE}bitとなる
	    /// </summary>
        [System.Xml.Serialization.XmlIgnore]
	    public byte[] padding;
	    /// <summary>
	    /// {@link #padding}の有効ビット数
	    /// </summary>
        public const int PADDING_SIZE = 16;

        /// <summary>
        /// 巡回冗長検査
        /// </summary>
        public int crc;
        /// <summary>
        ///{@link crc}の最小値
        /// </summary>
        public const int CRC_MIN = 0x00000000;
        /// <summary>
        ///{@link crc}の最大値
        /// </summary>
        public const int CRC_MAX = 0x0000FFFF;
        /// <summary>
        ///{@link crc}の有効ビット数
        /// </summary>
        public const int CRC_SIZE = 16;

	    /// <summary>
	    /// コンストラクタ
	    /// </summary>
	    public MsgType100() {
            msgType = MSG_TYPE;
            acks = new Ack[ACKNUM_MAX];
            for (int i = 0; i < ACKNUM_MAX; i++)
            {
                acks[i] = new Ack();
            }
            padding = new byte[sizeToLength(PADDING_SIZE)];
	    }

	    /// <summary>
	    /// 4.2.8.17 Type100エンコード要求
	    /// 【制限事項
	    /// Type100エンコードクラスの以下のパラメータを、エンコード要求前に設定しておくこと。
	    /// 設定する定義値については、3.6共通定義を参照
	    /// {@link MsgToTerminal#sysInfo}
	    /// {@link #userIdNum}
	    /// {@link #userId}
	    /// {@link #padding}
	    /// 
	    /// 【備考】
	    /// エンコードデータは以下に格納。
	    /// {@link #encodedData}
        /// 左詰め{@link MsgFromTerminal#SIZE}bitが有効値となる。
	    /// </summary>
        /// <param name="paramCheck">
        /// True：有効値パラメータチェック実施
        /// False：有効値パラメータチェック未実施
        /// ※型変換時のサイズチェックは実施。bit範囲に収まっている値かどうか。
        /// </param>
        /// <returns>
        /// エンコード結果。結果の定義は、3.6章共通定義を参照
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#ENC_VAL_NG}
        /// {@link EncDecConst#ENC_SIZE_NG}
        /// </returns>
	    public override int encode(Boolean paramCheck) {
	        // 有効値パラメータチェック
	        if (paramCheck) {
                int result = checkEncParam(MSG_TYPE);
	            if (result != EncDecConst.OK) {
	                return result;
	            }
	        }

	        // 共通フィールドのエンコード
	        int startPos = encodeCommonField();
	        int data;
	        int size;

	        // 送達確認数
	        data = ackNum;
	        size = ACKNUM_SIZE;
	        encode(data, size, encodedData, startPos);
	        startPos += size;

	        // 安否情報送達確認
	        for (int i = 0; i < ACKNUM_MAX; i++) {
                size = encode(acks[i], startPos);
                startPos += size;
	        }

	        // Padding
	        size = PADDING_SIZE;
	        encode(padding, size, encodedData, startPos);
	        startPos += size;

            // crc
            data = crc;
            size = CRC_SIZE;
            encode(data, size, encodedData, startPos);
            startPos += size;


            // エンコードサイズチェック
            if (startPos != SIZE)
            {
                return EncDecConst.ENC_SIZE_NG;
            }

	        return EncDecConst.OK;
	    }

        /// <summary>
        /// 送達確認のエンコード
        /// </summary>
        /// <param name="ack">送達確認</param>
        /// <param name="startPos">エンコード開始位置（ビット数）</param>
        /// <returns>エンコードしたビット数</returns>
        private int encode(Ack ack, int startPos)
        {
            int pos = startPos;
            int data;
            int size;

            //衛星通信端末の識別子
            data = ack.cid;
            size = Ack.CID_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            //Slot番号
            data = ack.slotNum;
            size = Ack.SLOTNUM_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            //SubSlotビットマップ
            data = ack.subslotBitmap;
            size = Ack.SUBSLOTBITMAP_SIZE;
            encode(data, size, encodedData, pos);
            pos += size;

            return pos - startPos;
        }

	    /// <summary>
        /// 4.2.8.18 Type100デコード要求
        /// 【制限事項】
        /// Type100デコードクラスの以下のパラメータを、デコード要求前に設定しておくこと。
        /// {@link #encodedData}
        ///
        /// 【備考】
        /// デコードデータは以下に格納。
        /// 設定する定義値については、3.6共通定義を参照
        /// {@link MsgToTerminal#sysInfo}
        /// {@link #userIdNum}
        /// {@link #userId}
        /// {@link #padding}
        /// </summary>
        /// <param name="paramCheck">
        /// True：有効値パラメータチェック実施
        /// False：有効値パラメータチェック未実施
        /// </param>
        /// <param name="sysInfoDecode">
        /// True：システム情報デコード実施
        /// False：システム情報デコード未実施
        /// </param>
        /// <returns>
        /// デコード結果。結果の定義は、3.6章共通定義を参照
        /// {@link EncDecConst#OK}
        /// {@link EncDecConst#DEC_GETTYPE_NG}
        /// {@link EncDecConst#DEC_VAL_NG}
        /// </returns>
	    public override int decode(Boolean paramCheck, Boolean sysInfoDecode) {
	        int size;
	        int startPos;

	        // 共通フィールドのデコード
	        startPos = decodeCommonField(sysInfoDecode);

            // 送達確認数
            size = ACKNUM_SIZE;
            ackNum = decodeInt(encodedData, size, startPos);
            startPos += size;

            // 安否情報送達確認
            for (int i = 0; i < ACKNUM_MAX; i++)
            {
                size = decode(acks[i], startPos);
                startPos += size;
            }

            // Padding
            size = PADDING_SIZE;
            decodeByteArray(encodedData,size, startPos, padding);
            startPos += size;

            // crc
            size = CRC_SIZE;
            crc = decodeInt(encodedData, size, startPos);
            startPos += size;

	        // 有効値パラメータチェック
	        if (paramCheck) {
                int result = checkDecParam(sysInfoDecode, MSG_TYPE);
	            if (result != EncDecConst.OK) {
	                return result;
	            }
	        }
	        return EncDecConst.OK;
	    }

        /// <summary>
        /// Ackデコード要求
        /// </summary>
        /// <param name="ack">{@link Ack}</param>
        /// <param name="startPos">デコード開始位置（ビット数）</param>
        /// <returns>デコードしたビット数</returns>
        private int decode(Ack ack, int startPos)
        {
            int pos = startPos;
            int size;

            //衛星通信端末の識別子
            size = Ack.CID_SIZE;
            ack.cid = decodeInt(encodedData, size, pos);
            pos += size;

            //Slot番号
            size = Ack.SLOTNUM_SIZE;
            ack.slotNum = decodeInt(encodedData, size, pos);
            pos += size;

            //SubSlotビットマップ
            size = Ack.SUBSLOTBITMAP_SIZE;
            ack.subslotBitmap = decodeInt(encodedData, size, pos);
            pos += size;

            return pos - startPos;

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
	    protected override int checkParam() {
	        int result = EncDecConst.OK;
            
            // 送達確認数
            int num = ackNum;
            if (CommonUtils.isOutOfRange(ackNum, ACKNUM_MIN, ACKNUM_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : ackNum is invalid : ackNum = "
                                + ackNum);
                result = EncDecConst.NG;
                num = 0; // ackNumが不正ならacks[]の確認はスキップ
            }

            // 送達確認
            // ackNumまでは有効なAck数の確認とパラメータチェック
            for (int i = 0; i < num; i++)
            {
                if (this.acks[i].checkParam() != EncDecConst.OK)
                {
                    LogMng.AplLogError(TAG + "checkParam : acks[] is invalid : acks["
                            + i + "]");
                    result = EncDecConst.NG;
                }
            }

            // Padding
            for (int i = 0; i < padding.Length; i++ )
            {
                if (padding[i] != 0)
                {
                    LogMng.AplLogError(TAG + "checkParam : padding is invalid : padding = "
                            + padding);
                    result = EncDecConst.NG;
                    break;
                }
             }

            //crc
            if (CommonUtils.isOutOfRange(crc, CRC_MIN, CRC_MAX))
            {
                LogMng.AplLogError(TAG +
                        "checkParam : crc is invalid : crc = "
                                + crc);
                result = EncDecConst.NG;
            }
            

	        return result;
	    }
    }
}
