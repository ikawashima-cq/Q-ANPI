package jp.co.nec.personinfoqrGenelator;

import android.util.Base64;
import android.util.Log;

import java.io.UnsupportedEncodingException;
import java.nio.charset.Charset;

/**
 * Created by omiya on 2018/02/01.
 */

public class CipherManager {
    private final static String CLASS_NAME = new Object(){}.getClass().getEnclosingClass().getName();
    // 暗号化したbyte配列は、文字に変換できないコードを含むため、暗号・復号には使用しない。
    private final static String KEY = "zRcRMrWYdU2i3J4z";
    private static CBlowfish mCBlowfish = new CBlowfish();

    static{
        byte[] bKey = KEY.getBytes(Charset.forName("UTF-8"));
        //kojima
//        mCBlowfish.Initialize(bKey,bKey.length * Byte.SIZE);
        mCBlowfish.Initialize(bKey,bKey.length);
        //^^^^^
    }

    /**
     * 暗号化
     * @param source
     * @return
     */
    public static String encrypt(String source) {
        String sEnc = "";
        byte[] bEncrypt = encrypt_cpp(source);
        if (bEncrypt != null) {
            sEnc = Base64.encodeToString(bEncrypt, Base64.DEFAULT);
        } else {
            // TODO：確認
            Log.e(CLASS_NAME, "暗号化に失敗");
        }
        return sEnc;
    }

    /**
     * 復号化
     * @param encryptSource
     * @return
     */
    public static String decrypt(String encryptSource) {
        // 復号処理
        byte[] bDec = decrypt_cpp(encryptSource);
        String sDec = "";
        if(bDec != null) {
            try {
                sDec = new String(bDec, "SHIFT_JIS");
            } catch (UnsupportedEncodingException e) {
                e.printStackTrace();
            }
        }else{
            // TODO：確認
            Log.e(CLASS_NAME, "復号に失敗");
        }
        return sDec;
    }

    private static byte[] encrypt_cpp(String str){
        byte[] src = new byte[0];
        try {
            src = str.getBytes("SHIFT_JIS");
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
            return null;
        }
        byte[] out = new byte[src.length];

        return mCBlowfish.Encode(src,out,src.length);
    }

    private static byte[] decrypt_cpp(String str) {
        try {
            // Stringの変換
            byte[] src = Base64.decode(str,Base64.DEFAULT);
            // 復号処理
            // cpp
            byte[] bOut = new byte[src.length + 1];
            // kojimka
//            byte[] bDec = mCBlowfish.Decode(src,bOut,bKey.length);
            byte[] bDec = mCBlowfish.Decode(src,bOut,src.length);
            //^^^^^

            return bDec;

        }catch (Exception e){
            e.printStackTrace();
        }

        return null;
    }
}
