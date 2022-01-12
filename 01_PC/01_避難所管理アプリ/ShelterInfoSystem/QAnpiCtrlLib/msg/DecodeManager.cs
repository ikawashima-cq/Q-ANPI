/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2017. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QAnpiCtrlLib.log;

namespace QAnpiCtrlLib.msg
{

    /// <summary>
    /// 2.1.8 1 メッセージエンコード・デコード
    /// デコード管理クラス
    /// デコード処理の実行処理
    /// 各Typeのエンコード・デコードクラスはこのクラスを継承してデコードを実施する。 
    /// </summary>
    public class DecodeManager
    {
        /// <summary>
        /// intのビット長
        /// </summary>
        protected const int INT_BIT_SIZE = 32;

        //C# 移植時に追加
        /// <summary>
        /// byteのビット長
        /// </summary>
        protected const int BYTE_BIT_SIZE = QAnpiCtrlLib.consts.EncDecConst.BYTE_BIT_SIZE;

        /// <summary>
        /// longのビット長
        /// </summary>
        protected const int LONG_BIT_SIZE = 64;

        /// <summary>
        /// Log出力時に使用する文字列
        /// </summary>
        private const String TAG = "DecodeManager";



        /// <summary>
        /// 4.2.8.3 Intデータデコード要求
        /// </summary>
        /// <param name="encodedData">デコードするエンコードされたデータ</param>
        /// <param name="size">エンコードデータから抜き出すサイズ(bit数)</param>
        /// <param name="startPos">エンコードデータエリアのデコード開始位置(bit数)</param>
        /// <returns>デコード結果</returns>
        public int decodeInt(byte[] encodedData, int size, int startPos) {
           
            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            // invalid size  
            if (size < 0 || size > INT_BIT_SIZE) {
                LogMng.AplLogError(TAG +": decodeInt invalid size : " + size);
                throw(new System.ArgumentOutOfRangeException("sizeが範囲外です") );
            }
            // invalid encodedData
            if (encodedData == null) {
                LogMng.AplLogError(TAG +": decodeInt invalid encodedData : null");
                throw(new System.ArgumentNullException("encodedDataがNULLです") );
            }
            // invalid startPos
            int remain = (encodedData.Length * BYTE_BIT_SIZE) - startPos; // 残バッファbit数
            if (startPos < 0 || remain < 0) {
                LogMng.AplLogError(TAG +": decodeInt invalid startPos : " + startPos);
                throw(new System.ArgumentOutOfRangeException("startPosが範囲外です") );
            }
            // buffer overrun
            if (size > remain) {
                LogMng.AplLogError(TAG + ": " +
                        String.Format(
                                "decodeInt buffer overrun : startPos={0}, size={1}, remain={2}, encodedData.length={3}",
                                startPos, size, remain, encodedData.Length));
                throw (new System.ArgumentOutOfRangeException("size + startPos が encodedData のサイズより大きいです"));
            }

            int buffLen = sizeToLength(size);
            byte[] buff = new byte[buffLen];
            decodeByteArray(encodedData, size, startPos, buff);
            int result = 0;
            int remainSize = size; // 残りの処理bit数
            for (int i = 0; remainSize > 0 && i < buffLen; i++) {
                int decodeSize = 0;
                if (remainSize > BYTE_BIT_SIZE) {
                    decodeSize = BYTE_BIT_SIZE;
                } else {
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

        /// <summary>
        /// 4.2.8.30 longデータデコード要求
        /// </summary>
        /// <param name="encodedData">デコードするエンコードされたデータ</param>
        /// <param name="size">エンコードデータから抜き出すサイズ(bit数)。</param>
        /// <param name="startPos">エンコードデータエリアのデコード開始位置(bit数)</param>
        /// <returns>デコード後のデータ</returns>
        public long decodeLong(byte[] encodedData, int size, int startPos) {
            // invalid size
            if (size < 0 || size > LONG_BIT_SIZE) {
                LogMng.AplLogError(TAG + "decodeLong invalid size : " + size);
                throw (new System.ArgumentOutOfRangeException("sizeが範囲外です"));
            }
            // invalid encodedData
            if (encodedData == null) {
                LogMng.AplLogError(TAG + "decodeLong invalid encodedData : null");
                throw (new System.ArgumentNullException("encodedDataがNULLです"));
            }
            // invalid startPos
            int remain = (encodedData.Length * BYTE_BIT_SIZE) - startPos; // 残バッファbit数
            if (startPos < 0 || remain < 0) {
                LogMng.AplLogError(TAG + "decodeLong invalid startPos : " + startPos);
                throw (new System.ArgumentOutOfRangeException("startPosが範囲外です"));
            }
            // buffer overrun
            if (size > remain) {
                LogMng.AplLogError(TAG +
                                String.Format(
                                        "decodeLong buffer overrun : startPos={0}, size={1}, remain={2}, encodedData.length={3}",
                                        startPos, size, remain, encodedData.Length));
                throw (new System.ArgumentOutOfRangeException("size + startPos が encodedData のサイズより大きいです"));
            }

            int buffLen = sizeToLength(size);
            byte[] buff = new byte[buffLen];
            decodeByteArray(encodedData, size, startPos, buff);
            ulong result = 0;
            int remainSize = size; // 残りの処理bit数
            for (int i = 0; remainSize > 0 && i < buffLen; i++) {
                int decodeSize = 0;
                if (remainSize > BYTE_BIT_SIZE) {
                    decodeSize = BYTE_BIT_SIZE;
                } else {
                    decodeSize = remainSize;
                }
                result =
                        (result << decodeSize)
                                | ((buff[i] & 0xffUL) >> (BYTE_BIT_SIZE - decodeSize));
                remainSize -= decodeSize;
            }

            return (long)result;
        }

        /// <summary>
        /// 4.2.8.4 byteデータデコード要求
        /// </summary>
        /// <param name="encodedData">デコードするエンコードされたデータ</param>
        /// <param name="size">エンコードデータから抜き出すサイズ(bit数)</param>
        /// <param name="startPos">エンコードデータエリアのデコード開始位置(bit数)</param>
        /// <param name="decodedData">デコード後のデータを格納する。左詰めで有効エリアとなる。</param>
        public void decodeByteArray(byte[] encodedData, int size, int startPos,
                byte[] decodedData) {
            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            // invalid size
            if (size < 0 || size > (decodedData.Length * BYTE_BIT_SIZE)){
                LogMng.AplLogError(TAG  + ": decodeByteArray invalid size : " + size);
                throw (new System.ArgumentOutOfRangeException("sizeが範囲外です"));
            }
            // invalid encodedData
            if (encodedData == null) {
                LogMng.AplLogError(TAG + ":decodeInt invalid encodedData : null");
                throw (new System.ArgumentNullException("encodedDataがNULLです"));
            }
            // buffer overrun
            int remain = (encodedData.Length * BYTE_BIT_SIZE) - startPos; // 残バッファbit数
            if (size > remain) {
                LogMng.AplLogError(TAG + ": " + 
                        String.Format(
                                "decodeByteArray buffer overrun : startPos={0}, size={1}, remain={2}, encodedData.length={3}",
                                startPos, size, remain, encodedData.Length));
                throw (new System.ArgumentOutOfRangeException("size + startPos が encodedData のサイズより大きいです"));
            }

            int pos = startPos;
            int remainSize = size; // 残りの処理bit数
            int buffSize = decodedData.Length;
            for (int i = 0; remainSize > 0 && i < buffSize; i++) {
                int decSize = 0;
                if (remainSize > BYTE_BIT_SIZE) {
                    decSize = BYTE_BIT_SIZE;
                } else {
                    decSize = remainSize;
                }

                decodedData[i] = decodeByte(encodedData, decSize, pos);
                remainSize -= decSize;
                pos += decSize;
            }
            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

        }

        /// <summary>
        /// 4.2.8.5 メッセージType取得/システム情報デコード要求
        /// </summary>
        /// <param name="encodedData">デコードするエンコードされたデータ</param>
        /// <param name="dataSize">
        /// エンコードデータの有効サイズ
        /// 下記以外は不正値として扱う
        /// MsgFromTerminal.SIZE
        /// MsgToTerminal.SIZE 
        /// </param>
        /// <param name="sysInfoDecode">
        /// true：システム情報デコード実施
        /// false：システム情報デコード未実施
        /// パラメータによらずシステム情報のデコードは実施されない
        /// </param>
        /// <returns>メッセージTypeの通知と、システム情報のデコード結果TypeAndSystemInfoを返す</returns>
        public TypeAndSystemInfo decodeTypeAndSystemInfo(byte[] encodedData,
                int dataSize, Boolean sysInfoDecode) {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            // invalid encodedData
            if (encodedData == null) {
                LogMng.AplLogError(TAG + ": decodeTypeAndSystemInfo invalid encodedData : null");
                throw (new System.ArgumentNullException("encodedDataがNULLです"));
            }
            // check message type from dataSize
            Boolean isMsgFromTerminal;
            if (dataSize == MsgFromTerminal.SIZE) {
                isMsgFromTerminal = true;
            } else if (dataSize == MsgToTerminal.SIZE) {
                isMsgFromTerminal = false;
            } else {
                // invalid dataSize
                LogMng.AplLogError(TAG + ": decodeTypeAndSystemInfo invalid dataSize : " + dataSize);
                throw (new System.ArgumentException("dataSizeが無効値です"));
            }
            // buffer overrun
            if (dataSize > (encodedData.Length * BYTE_BIT_SIZE)) {
                LogMng.AplLogError(TAG + ": " +
                        String.Format(
                                "decodeTypeAndSystemInfo buffer overrun : encodedData.length={0}, dataSize={1}",
                                encodedData.Length, dataSize));
                throw (new System.ArgumentException("encodedDataが小さすぎます"));
            }
            // sysInfoDecodeはチェック不要

            TypeAndSystemInfo result = new TypeAndSystemInfo();
            int pos = 0;
            int size;

            if (isMsgFromTerminal) {
                // 端末発メッセージのデコード処理

                // Type(2bit)デコード
                size = TypeAndSystemInfo.MSG_TYPE_FROM_TERMINAL_SIZE;
                result.msgType = decodeInt(encodedData, size, pos);
                pos += size;

            } else {

                // Type(8bit)デコード
                size = TypeAndSystemInfo.MSG_TYPE_TO_TERMINAL_SIZE;
                result.msgType = decodeInt(encodedData, size, pos);
                pos += size;

                // システム情報(136bit)デコード
                if (sysInfoDecode) {
                    result.sysInfo = decodeSystemInfo(encodedData, pos);
                }
            }

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return result;
        }

        /// <summary>
        /// byteデータデコード要求
        /// </summary>
        /// <param name="encodedData">デコードするエンコードされたデータ</param>
        /// <param name="size">エンコードデータから抜き出すサイズ(bit数)</param>
        /// <param name="startPos">エンコードデータエリアのデコード開始位置(bit数) byte内ではMSB起点とする</param>
        /// <returns>デコード後のデータ</returns>
	    private byte decodeByte(byte[] encodedData, int size, int startPos) {
            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

	        // invalid size
	        if (size < 0 || size > BYTE_BIT_SIZE) {
	            LogMng.AplLogError(TAG + ": decodeByte invalid size : " + size);
	            throw (new System.ArgumentOutOfRangeException("sizeが範囲外です"));
	        }
	        // invalid encodedData
	        if (encodedData == null) {
	            LogMng.AplLogError(TAG + ":decodeByte invalid encodedData : null");
	            throw (new System.ArgumentNullException("encodedDataがNULLです"));
	        }
	        // buffer overrun
	        int remain = (encodedData.Length * BYTE_BIT_SIZE) - startPos; // 残バッファbit数
	        if (size > remain) {
	            LogMng.AplLogError(TAG + " :" + 
	                    String.Format(
	                            "decodeByte buffer overrun : startPos={0}, size={1}, remain={2}, encodedData.length={3}",
	                            startPos, size, remain, encodedData.Length));
                throw (new System.ArgumentOutOfRangeException("size + startPos が encodedData のサイズより大きいです"));
	        }

	        int indexByte = startPos / BYTE_BIT_SIZE;
	        int indexBits = startPos % BYTE_BIT_SIZE; // MSBからのビット数
	        int readSize = BYTE_BIT_SIZE - indexBits; // encodedData[indexByte]から読み出すbit数
	        if (readSize > size) {
	            readSize = size;
	        }

	        // encodedData[indexByte]から読み出し
	        byte result = decodeByte(encodedData[indexByte], readSize, indexBits);

	        // 読み込めなかったビットを次のバッファから読み込む
	        int remainSize = size - readSize; // 残りの未処理bit数
	        if (remainSize > 0) {
	            byte temp = decodeByte(encodedData[indexByte + 1], remainSize, 0);
	            result = (byte) (result | ((temp & 0xff) >> readSize)); // 不足分のbitを連結
	        }

            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
	        return result;
	    }

        /// <summary>
        /// byteデータデコード要求
        /// encodedDataのMSBからstartPosビットからsizeビット分左詰めbyteを作成する
        /// {@link Byte#SIZE} = startPos + size + 残りの下位bit
        /// この時点でencodedDataのstartPosと残りの下位bitには不要なデータが入っている
        /// startPosは左シフトで削除し、残りの下位bitはマスキングで削除する
        /// 
        /// 【制約事項】
        /// パラメータの妥当性は呼び元で担保すること
        /// 不正なパラメータによる動作は保証しない
        /// </summary>
        /// <param name="encodedData">デコードするエンコードされたデータ</param>
        /// <param name="size">
        /// エンコードデータから抜き出すサイズ(bit数)
        /// [1 ～ {@link Byte#SIZE}]で指定
        /// </param>
        /// <param name="startPos">
        /// エンコードデータエリアのデコード開始位置(bit数)
        /// MSBを起点とする
        /// size + startPosで{@link Byte#SIZE}を超えてはならない
        /// </param>
        /// <returns>デコードデータ</returns>
        private byte decodeByte(byte encodedData, int size, int startPos) {
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

        /// <summary>
        /// システム情報デコード要求
        /// startPosから{@link SystemInfo#SYSTEM_INFO_SIZE}
        /// bit長の領域を読み出し、システム情報としてデコードする
        /// </summary>
        /// <param name="encodedData">デコードするエンコードされたデータ</param>
        /// <param name="startPos">
        /// エンコードデータエリアのデコード開始位置(bit数)
        /// MSBを起点とする
        /// </param>
        /// <returns>システム情報</returns>
        public SystemInfo decodeSystemInfo(byte[] encodedData, int startPos)
        {
            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            // invalid encodedData
            if (encodedData == null) {
                LogMng.AplLogError(TAG + ": decodeSystemInfo invalid encodedData : null");
                throw (new System.ArgumentNullException("encodedDataがNULLです"));
            }
            // buffer overrun
            int remain = (encodedData.Length * BYTE_BIT_SIZE) - startPos; // 残バッファbit数
            if (SystemInfo.SYSTEM_INFO_SIZE > remain)
            {
                 LogMng.AplLogError(TAG + ": " +
                        String.Format(
                                "decodeSystemInfo buffer overrun : startPos={0}, remain={1}, encodedData.length={2}",
                                startPos, remain, encodedData.Length));
                throw (new System.ArgumentOutOfRangeException("startPosが範囲外です"));
            }

            int size;
            int pos = startPos;
            SystemInfo result = new SystemInfo();

            // システム情報デコード開始
            // FWDリンクタイムスロット(6bit)
            size = SystemInfo.SYS_FWD_TIME_SLOT_SIZE;
            result.sysFwdTimeSlot = decodeInt(encodedData, size, pos);
            pos += size;

            // 静止衛星位置軌道情報のX座標(20bit)
            size = SystemInfo.SYS_SATELLITE_POS_X_SIZE;
            result.sysSatellitePosX = decodeInt(encodedData, size, pos);
            pos += size;

            // 静止衛星位置軌道情報のY座標(20bit)
            size = SystemInfo.SYS_SATELLITE_POS_Y_SIZE;
            result.sysSatellitePosY = decodeInt(encodedData, size, pos);
            pos += size;

            // 静止衛星位置軌道情報のZ座標(20bit)
            size = SystemInfo.SYS_SATELLITE_POS_Z_SIZE;
            result.sysSatellitePosZ = decodeInt(encodedData, size, pos);
            pos += size;

            // 地上局・衛星出力端遅延時間(21bit)
            size = SystemInfo.SYS_DELAY_TIME_SIZE;
            result.sysDelayTime = decodeInt(encodedData, size, pos);
            pos += size;

            // メッセージ送信制限(1bit)
            size = SystemInfo.SYS_SEND_RESTRICTION_SIZE;
            result.sysSendRestriction = decodeInt(encodedData, size, pos);
            pos += size;

            // アクセス制御基準時刻(25bit)
            size = SystemInfo.SYS_BASE_TIME_SIZE;
            result.sysBaseTime = decodeInt(encodedData, size, pos);
            pos += size;

            // 送信グループ数(12bit)
            size = SystemInfo.SYS_GROUP_NUM_SIZE;
            result.sysGroupNum = decodeInt(encodedData, size, pos);
            pos += size;

            // 送信スロットランダム選択幅(3bit)
            size = SystemInfo.SYS_RANDOM_SELECT_BAND_SIZE;
            result.sysRandomSelectBand = decodeInt(encodedData, size, pos);
            pos += size;

            // 開始周波数ID(4bit)
            size = SystemInfo.SYS_START_FREQ_ID_SIZE;
            result.sysStartFreqId = decodeInt(encodedData, size, pos);
            pos += size;

            // 最終周波数ID(4bit)
            size = SystemInfo.SYS_END_FREQ_ID_SIZE;
            result.sysEndFreqId = decodeInt(encodedData, size, pos);
            pos += size;
            // システム情報デコード終了

            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return result;
        }

        /// <summary>
        /// 指定ビットから任意のビットまでのビットマスクを作成する。
        /// </summary>
        /// <param name="index">
        /// ビットマスク開始位置(0 origin)
        /// 0 - ({@link Integer#SIZE}-1)で指定する
        /// </param>
        /// <param name="size">
        /// ビット長。1 - {@link Integer#SIZE}で指定する
        /// index + sizeで{@link Integer#SIZE}を超えてはならない
        /// </param>
        /// <returns>ビットマスク</returns>
	    protected int makeBitMask(int index, int size) {

            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            uint result = 0;

	        if ((index < 0) || (index >= INT_BIT_SIZE)) {
                LogMng.AplLogError(TAG + ": decodeSystemInfo invalid index :" + index);
                throw (new System.ArgumentOutOfRangeException("indexが範囲外です"));
	        }
	        if ((size <= 0) || (size > INT_BIT_SIZE)) {
                LogMng.AplLogError(TAG + ": decodeSystemInfo invalid size :" + size);
                throw (new System.ArgumentOutOfRangeException("sizeが範囲外です"));
	        }
	        // 上位側の不要なビット
	        int remain = INT_BIT_SIZE - (index + size);
	        if (remain < 0) {
                throw (new System.ArgumentOutOfRangeException("index + sizeが範囲外です"));
	        }

	        result = uint.MaxValue;

            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

	        // 上位側remainビット削除 下位側indexビット削除 位置合わせ
	        return (int)(((result << remain) >> (remain + index)) << index);
	    }

        /// <summary>
        /// 指定ビットから任意のビットまでのビットマスクを作成する。
        /// </summary>
        /// <param name="index">
        /// ビットマスク開始位置(0 origin)。
        /// 0 - ({@link Long#SIZE}-1)で指定する。
        /// </param>
        /// <param name="size">
        /// ビット長。1 - {@link Long#SIZE}で指定する。
        /// index + sizeで{@link Long#SIZE}を超えてはならない。
        /// </param>
        /// <returns>ビットマスク。</returns>
        protected long makeBitMaskLong(int index, int size) {
            
            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            ulong result = 0;

            if ((index < 0) || (index >= LONG_BIT_SIZE)) {
                LogMng.AplLogError(TAG + ": decodeSystemInfo invalid index :" + index);
                throw (new System.ArgumentOutOfRangeException("indexが範囲外です"));
            }
            if ((size <= 0) || (size > LONG_BIT_SIZE))
            {
                LogMng.AplLogError(TAG + ": decodeSystemInfo invalid size :" + size);
                throw (new System.ArgumentOutOfRangeException("sizeが範囲外です"));
            }
            // 上位側の不要なビット
            int remain = LONG_BIT_SIZE - (index + size);
            if (remain < 0) {
                throw (new System.ArgumentOutOfRangeException("index + sizeが範囲外です"));
            }

            result = ~0x00UL;
            
            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

            // 上位側remainビット削除 下位側indexビット削除 位置合わせ
            return (long)(((result << remain) >> (remain + index)) << index);
        }

        /// <summary>
        /// int値をバイト配列にする。
        /// </summary>
        /// <param name="value">変換元のint値</param>
        /// <returns>変換したバイト配列 (big endian)</returns>
        protected byte[] toBytes(int value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            List<byte> list = new List<byte>();
            list.AddRange(buffer);
            list.Reverse();
            return list.ToArray();
        }

        /// <summary>
        /// long値をバイト配列にする。
        /// </summary>
        /// <param name="value">変換元のlong値</param>
        /// <returns>変換したバイト配列 (big endian)</returns>
        protected byte[] toBytes(long value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            List<byte> list = new List<byte>();
            list.AddRange(buffer);
            list.Reverse();
            return list.ToArray();
        }

        /// <summary>
        /// ビット数を格納するのに必要なバイト数を算出する
        /// </summary>
        /// <param name="size">ビット数</param>
        /// <returns>size格納するのに必要なバイト数</returns>
        public static int sizeToLength(int size) {
            return (size + BYTE_BIT_SIZE - 1) / BYTE_BIT_SIZE;
        }
        

        /// <summary>
        /// Ｓ帯モニタデータのヘッダ部をデコードする
        /// </summary>
        /// <param name="data">
        /// デコード対象
        /// </param>
        /// <param name="msd">
        /// デコード結果：データType,スタートコード、エンドコード
        /// データTypeには
        /// ※Ｓ帯RTNデータ送信要求/応答についてはサイズ、データID共に同じのため
        ///   Ｓ帯RTNデータ送信応答の場合でもＳ帯RTNデータ送信要求が設定される
        /// </param>
        /// <param name="dataSize">
        /// デコード結果：データサイズ
        /// </param>
        /// <returns>
        /// デコードしたビット数（0の場合、デコード失敗）
        /// </returns>
        public int decodeSBandDataHeader(byte[] data, MsgSBandData msd, out int dataSize)
        {
            string dataName = "";
            return decodeSBandDataHeader(data,  msd, out dataSize, out dataName);
        }

        /// <summary>
        /// Ｓ帯モニタデータのヘッダ部をデコードする
        /// </summary>
        /// <param name="data">
        /// デコード対象
        /// </param>
        /// <param name="msd">
        /// デコード結果：データType,スタートコード、エンドコード
        /// データTypeには
        /// ※Ｓ帯RTNデータ送信要求/応答についてはサイズ、データID共に同じのため
        ///   Ｓ帯RTNデータ送信応答の場合でもＳ帯RTNデータ送信要求が設定される
        /// </param>
        /// <param name="dataSize">
        /// デコード結果：データサイズ
        /// </param>
        /// <param name="dataName">データ名</param>
        /// <returns>
        /// デコードしたビット数（0の場合、デコード失敗）
        /// </returns>
        public int decodeSBandDataHeader(byte[] data, MsgSBandData msd, out int dataSize, out string dataName)
        {
            int dec_pos = 0;
            int dec_size;
            int dec_data;
            int dec_dataid;
            int dec_datasize;

            dataSize = 0;
            dataName = "";
            msd.msgType = 0;

            // 開始コードのデコード
            dec_size = MsgSBandData.SIZE_HEAD_START_CODE;
            dec_data = decodeInt(data, dec_size, dec_pos);
            if (dec_data != consts.CommonConst.SBAND_MSG_START_CODE)
            {
                LogMng.AplLogError(TAG + "decodeSBandDataHeader : start code is invalid : dec_data = " + dec_data);
                return 0;
            }
            msd.startCode = dec_data;
            dec_pos += dec_size;

            // データサイズのデコード
            dec_size = MsgSBandData.SIZE_HEAD_DATA_SIZE;
            dec_datasize = decodeInt(data, dec_size, dec_pos);
            dec_pos += dec_size;

            // データIDのデコード
            dec_size = MsgSBandData.SIZE_HEAD_DATA_ID;
            dec_dataid = decodeInt(data, dec_size, dec_pos);
            dec_pos += dec_size;

            // データサイズとデータIDからType決定
            for (int num = 0; num < MsgSBandData.sbandDataDef.Length; num++)
            {
                int defDataSize = (int)MsgSBandData.sbandDataDef[num][MsgSBandData.COL_SBANDDATA_SIZE];
                int defDataId = (int)MsgSBandData.sbandDataDef[num][MsgSBandData.COL_SBANDDATA_TYPE];

                // 両方一致
                if (((dec_datasize * BYTE_BIT_SIZE) == defDataSize) &&
                    (dec_dataid == defDataId % consts.EncDecConst.CALC_DATAID_DENOMI))
                {
                    msd.msgType = defDataId;
                    dataSize = defDataSize;
                    dataName = (string)MsgSBandData.sbandDataDef[num][MsgSBandData.COL_SBANDDATA_NAME];
                    break;
                }
            }

            // Type未決定の場合、戻り値=0、各値は設定）
            if (msd.msgType == 0)
            {
                LogMng.AplLogError(TAG + "decodeSBandDataHeader : data id is invalid : dec_dataid = " + dec_dataid);

                msd.msgType = dec_dataid; // 生ID値を設定
                dataSize = dec_datasize * BYTE_BIT_SIZE;
                dataName = Properties.Resources.W_CTF202_dat_nam_00;

                dec_pos = 0;
            }

            return dec_pos;
        }

        /// <summary>
        /// Ｓ帯モニタデータのヘッダ部をデコードする
        /// </summary>
        /// <param name="data">
        /// デコード対象
        /// </param>
        /// <param name="dataType">
        /// デコード結果：データType,
        /// ※Ｓ帯RTNデータ送信要求/応答についてはサイズ、データID共に同じのため
        ///   Ｓ帯RTNデータ送信応答の場合でもＳ帯RTNデータ送信要求が設定される
        /// </param>
        /// <param name="dataSize">
        /// デコード結果：データサイズ
        /// </param>
        /// <returns>
        /// デコードしたビット数（0の場合、デコード失敗）
        /// </returns>
        public int decodeSBandDataHeader(byte[] data, out int dataType, out int dataSize)
        {
            string dataName = "";
            return decodeSBandDataHeader(data, out dataType, out dataSize, out dataName);
        }

                /// <summary>
        /// Ｓ帯モニタデータのヘッダ部をデコードする
        /// </summary>
        /// <param name="data">
        /// デコード対象
        /// </param>
        /// <param name="dataType">
        /// デコード結果：データType
        /// ※Ｓ帯RTNデータ送信要求/応答についてはサイズ、データID共に同じのため
        ///   Ｓ帯RTNデータ送信応答の場合でもＳ帯RTNデータ送信要求が設定される
        /// </param>
        /// <param name="dataSize">
        /// デコード結果：データサイズ
        /// </param>
        /// <param name="dataName">データ名</param>
        /// <returns>
        /// デコードしたビット数（0の場合、デコード失敗）
        /// </returns>
        public int decodeSBandDataHeader(byte[] data, out int dataType, out int dataSize, out string dataName)
        {
            MsgSBandLineTestReq msg = new MsgSBandLineTestReq();
            msg.msgType = 0;
            int result = decodeSBandDataHeader(data, msg, out dataSize, out dataName);

            dataType = msg.msgType;
            return result;
        }


    }
}
