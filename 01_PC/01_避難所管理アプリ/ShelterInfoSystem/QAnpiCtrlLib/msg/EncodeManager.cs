/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
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
    ///  2.1.8 メッセージエンコード・デコード
    /// エンコード管理クラス
    /// エンコード処理の実行処理
    /// 各Typeのエンコード・デコードクラスはこのクラスを継承してエンコードを実施する。
    /// @see DecodeManager
    /// </summary>
    public class EncodeManager : DecodeManager
    {
        /// <summary>
        /// Log出力用の文字列
        /// </summary>
        private const String TAG = "EncodeManager";

        /// <summary>
        /// 4.2.8.1 Intデータエンコード要求
        /// 【制約事項】dataはLSBから有効ビットとなる。
        /// </summary>
        /// <param name="data">エンコードするデータ。LSBから有効ビットとなる</param>
        /// <param name="size">格納するサイズ(bit数)。右詰めで有効エリアとなる</param>
        /// <param name="encodedData">エンコードデータ格納エリア。入力データを上書きする</param>
        /// <param name="startPos">エンコードデータエリアの保存開始位置(bit数)</param>
	    public void encode(int data, int size, byte[] encodedData, int startPos) {
	        // invalid size
            if (size < 0 || size > INT_BIT_SIZE)
            {
                LogMng.AplLogError(TAG + ":encode(int) invalid size : " + size);
                throw (new System.ArgumentOutOfRangeException("sizeが範囲外です"));
	        }
	        // invalid encodedData
	        if (encodedData == null) {
                LogMng.AplLogError(TAG + ":encode(int) invalid encodedData : null");
                throw (new System.ArgumentNullException("encodedDataがNULLです"));
            }
	        // invalid startPos
	        int remain = (encodedData.Length * BYTE_BIT_SIZE) - startPos; // 残バッファbit数
	        if (startPos < 0 || remain < 0) {
                LogMng.AplLogError(TAG + ":encode(int) invalid startPos : " + startPos);
                throw (new System.ArgumentOutOfRangeException("startPosが範囲外です"));
	        }
	        // buffer overflow
	        if (size > remain) {
	            LogMng.AplLogError(TAG +": " +
	                    String.Format(
	                            "encode(int) buffer overflow : startPos={0}, size={1}, remain={2}, encodedData.length={3}",
	                            startPos, size, remain, encodedData.Length));
                throw (new System.ArgumentOutOfRangeException("encodedDataのサイズが足りません"));
	        }

	        int mask = makeBitMask(0, size); // 最下位bitからlength分のビットマスク

	        // 不要なデータを削除
	        // ex) data=(00 00 00 ff), length=4 -> temp=(00 00 00 0f)
	        int temp = data & mask;
	        // データを左詰め(big endianでバイト配列変換するため)
	        // ex) -> temp=(f0 00 00 00)
	        temp = temp << (INT_BIT_SIZE - size);

	        // バイト配列変換
	        // ex) -> dataArray={0xf0, 0x00, 0x00, 0x00}
	        byte[] dataArray = toBytes(temp);

	        // 既存メソッドで処理する
	        encode(dataArray, size, encodedData, startPos);
	    }

        /// <summary>
        /// 4.2.8.29 longデータエンコード要求
        /// 【制約事項】dataはLSBから有効ビットとなる。
        /// </summary>
        /// <param name="data">エンコードするデータ。LSBから有効ビットとなる。</param>
        /// <param name="size">格納するサイズ(bit数)。右詰めで有効エリアとなる。</param>
        /// <param name="encodedData">エンコードデータ格納エリア。入力データを上書きする。</param>
        /// <param name="startPos">エンコードデータエリアの保存開始位置(bit数)</param>
        public void encode(long data, int size, byte[] encodedData, int startPos)
        {
            // invalid size
            if (size < 0 || size > LONG_BIT_SIZE)
            {
                LogMng.AplLogError(TAG + "encode(long) invalid size : " + size);
                throw (new System.ArgumentOutOfRangeException("sizeが範囲外です"));
            }
            // invalid encodedData
            if (encodedData == null)
            {
                LogMng.AplLogError(TAG + "encode(long) invalid encodedData : null");
                throw (new System.ArgumentNullException("encodedDataがNULLです"));
            }
            // invalid startPos
            int remain = (encodedData.Length * BYTE_BIT_SIZE) - startPos; // 残バッファbit数
            if (startPos < 0 || remain < 0)
            {
                LogMng.AplLogError(TAG + "encode(long) invalid startPos : " + startPos);
                throw (new System.ArgumentOutOfRangeException("startPosが範囲外です"));
            }
            // buffer overflow
            if (size > remain)
            {
                LogMng.AplLogError(TAG + 
                                String.Format(
                                        "encode(long) buffer overflow : startPos={0}, size={1}, remain={2}, encodedData.length={3}",
                                        startPos, size, remain, encodedData.Length));
                throw (new System.ArgumentOutOfRangeException("encodedDataのサイズが足りません"));
            }

            long mask = makeBitMaskLong(0, size); // 最下位bitからlength分のビットマスク

            // 不要なデータを削除
            // ex) data=(00 00 00 ff), length=4 -> temp=(00 00 00 0f)
            long temp = data & mask;
            // データを左詰め(big endianでバイト配列変換するため)
            // ex) -> temp=(f0 00 00 00)
            temp = temp << (LONG_BIT_SIZE - size);

            // バイト配列変換
            // ex) -> dataArray={0xf0, 0x00, 0x00, 0x00}
            byte[] dataArray = toBytes(temp);

            // 既存メソッドで処理する
            encode(dataArray, size, encodedData, startPos);
        }


        /// <summary>
        /// 4.2.8.2 byteデータエンコード要求
        /// 【制約事項】dataはbig endianの左詰めデータで、上位ビット(ビット)が有効ビットとなる
        /// </summary>
        /// <param name="data">エンコードするデータ。MSBから有効ビットとなる</param>
        /// <param name="size">格納するサイズ(bit数)</param>
        /// <param name="encodedData">エンコードデータ格納エリア 入力データを上書きする</param>
        /// <param name="startPos">エンコードデータエリアの保存開始位置(bit数)</param>
	    public void encode(byte[] data, int size, byte[] encodedData, int startPos) {
	        // invalid size
	        if (size < 0 || size > (data.Length * BYTE_BIT_SIZE)) {
                LogMng.AplLogError(TAG + ":encode(byte[]) invalid size : " + size);
                throw (new System.ArgumentOutOfRangeException("sizeが範囲外です"));
	        }
	        // invalid encodedData
	        if (encodedData == null) {
                LogMng.AplLogError(TAG + ":encode(byte[]) invalid encodedData : null");
                throw (new System.ArgumentNullException("encodedDataがNULLです"));
	        }
	        // invalid startPos
	        int remain = (encodedData.Length * BYTE_BIT_SIZE) - startPos; // 残バッファbit数
	        if (startPos < 0 || remain < 0) {
                LogMng.AplLogError(TAG + ":encode(byte[]) invalid startPos : " + startPos);
                throw (new System.ArgumentOutOfRangeException("startPosが範囲外です"));
	        }
	        // buffer overflow
	        if (size > remain) {
	            LogMng.AplLogError(TAG + ":" +
	                    String.Format(
	                            "encode(byte[]) buffer overflow : startPos={0}, size={1}, remain={2}, encodedData.length={3}",
	                            startPos, size, remain, encodedData.Length));
                throw (new System.ArgumentOutOfRangeException("encodedDataのサイズが足りません"));
	        }

	        int pos = startPos;
	        int remainSize = size; // 残りの処理bit数
	        int buffLen = data.Length;
	        for (int i = 0; remainSize > 0 && i < buffLen; i++) {
	            int encSize = 0;
	            if (remainSize > BYTE_BIT_SIZE) {
                    encSize = BYTE_BIT_SIZE;
	            } else {
	                encSize = remainSize;
	            }

	            encode(data[i], encSize, encodedData, pos);
	            remainSize -= encSize;
	            pos += encSize;
	        }
	    }

        /// <summary>
        /// dataの左先頭(7bit目)からsizeビット分encodedDataのstartPosから書き込む
        /// 【制約事項】dataは上位ビット(MSB)が有効ビットとなる。
        /// </summary>
        /// <param name="data">エンコードするデータ(左詰め)。MSBから有効ビットとなる</param>
        /// <param name="size">格納するサイズ(bit数)</param>
        /// <param name="encodedData">エンコードデータ格納エリア。入力データを上書きする</param>
        /// <param name="startPos">エンコードデータエリアの保存開始位置(bit数)</param>
	    private void encode(byte data, int size, byte[] encodedData, int startPos) {
	        // invalid size
	        if (size < 0 || size > BYTE_BIT_SIZE) {
	            LogMng.AplLogError(TAG + "encode(byte) invalid size : " + size);
	            throw (new System.ArgumentOutOfRangeException("sizeが範囲外です"));
	        }
	        // invalid encodedData
	        if (encodedData == null) {
	            LogMng.AplLogError(TAG + "encode(byte) invalid encodedData : null");
	            throw (new System.ArgumentNullException("encodedDataがNULLです"));
	        }
	        // invalid startPos
	        int remain = (encodedData.Length * BYTE_BIT_SIZE) - startPos; // 残バッファbit数
	        if (startPos < 0 || remain < 0) {
	            LogMng.AplLogError(TAG + "encode(byte) invalid startPos : " + startPos);
	            throw (new System.ArgumentOutOfRangeException("startPosが範囲外です"));
	        }
	        // buffer overflow
	        if (size > remain) {
	            LogMng.AplLogError(TAG + ": " +
	                    String.Format(
	                            "encode(byte) buffer overflow : startPos={0}, size={1}, remain={2}, encodedData.length={3}",
	                            startPos, size, remain, encodedData.Length));
	            throw (new System.ArgumentOutOfRangeException("encodedDataのサイズが足りません"));
	        }

	        int indexByte = startPos / BYTE_BIT_SIZE;
	        int indexBits = startPos % BYTE_BIT_SIZE; // 先頭ビット(7bit)からのビット数

	        // dataは左詰めされている
	        // 既に書き込まれているデータ分右シフト
	        // ex) data=0x80 indexBits=4 -> temp=0x08
	        byte temp = (byte) ((data & 0xff) >> indexBits);

	        // startPos以降は0埋めされている前提
	        // indexBits分はデータ書き込み済みなのでOR演算でデータを保持する
	        encodedData[indexByte] |= temp;

	        // 書き込めなかったビットを次のバッファに書き込む
	        int encodedSize = BYTE_BIT_SIZE - indexBits;
	        int remainSize = size - encodedSize; // 残りの処理bit数
	        if (remainSize > 0) {
	            encode((byte) (data << encodedSize), remainSize, encodedData,
	                    (startPos + encodedSize));
	        }
	    }
    }
}
