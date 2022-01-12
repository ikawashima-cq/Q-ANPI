package jp.co.nec.personinfoqrGenelator;

/**
 * Created by ysaijo on 2018/06/07.
 * blowfish.cpp処理呼出を簡易にするためのクラス。
 */
public class CBlowfish {

    // ライブラリ名
    private static final String LIB_NAME = "jniBlowfish";
    // NativeContext
    private long mNativeContext;

    static{
        System.loadLibrary(LIB_NAME);
    }

    // コンストラクタ
    public CBlowfish(){
        Instance();
    }

    // デストラクタ
    public void Destractor(){
        Destroy();
    }

    public native void Instance();
    public native void Destroy();
    public native void Initialize(byte[] key, int keybytes);
    public native byte[] Encode(byte[] pInput, byte[] pOutput, long lSize);
    public native byte[] Decode(byte[] pInput, byte[] pOutput, long lSize);
}
