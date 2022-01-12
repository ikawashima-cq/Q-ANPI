package jp.co.nec.personinfoqrGenelator;

import android.util.Log;

import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;

public class ParseParam {
    // 定数
    public static final int NUM_ALL_PARAM = 12;  // 項目数
    public static final int MAX_LENGTH_ID = 12;  // ID最大文字数
    public static final int MAX_LENGTH_FIRST_NAME = 12;  // 姓最大文字数
    public static final int MAX_LENGTH_GIVEN_NAME = 12; // 名最大文字数
    public static final int MAX_LENGTH_FULL_NAME = 25;  // 氏名最大文字数
    public static final int MAX_LENGTH_ADDRESS = 64;  // 住所最大文字数
    public static final int LENGTH_BIRHTDAY = 8;    // 生年月日文字数
    public static final char FLAG_CIPHER_TRUE = '1';    // 文字列暗号フラグON
    public static final char FLAG_CIPHER_FALSE = '0';    // 文字列暗号化フラグOFF

    // メンバ
    private String mId;     // 電話番号(個人ID)
    private String mName;   // 氏名
    private String mFamilyName;   // 姓
    private String mGivenName;   // 名
    private String mBirthday;  // 生年月日
    private String mTxt01;  // 住所
    private String mSel01;  // 性別(0:男性 1:女性)
    private String mSel02;  // 入所状況(0:入所 1:退所 2:在宅)
    private String mSel03;  // 公表(0:しない 1:する)
    private String mSel04;  // 怪我の有無(2:未選択 0:無 1:有)
    private String mSel05;  // 介護 (2:未選択 0:否  1:要)
    private String mSel06;  // 障がい (2:未選択 0:無 1:有)
    private String mSel07;  // 妊産婦 (2:未選択 0:いいえ 1:はい)
    private String mSel08;  // 避難所内外フラグ (0:内 1:外)
    private String mEncodeParam;    // 暗号フラグ無しCSV形式文字列 平文
    private String mEncodeParamWithFlag;    // 暗号フラグ有りCSV形式文字列
    private String mDate;  // 安否登録日時
    private boolean mIsEncryption;  // 暗号化有無 (true:暗号化する　false：暗号化しない)

    public ParseParam()
    {
        init();
    }

    public void init()
    {
        mId = "";
        mName = "";
        mFamilyName = "";
        mGivenName = "";
        mBirthday = "";
        mTxt01 = "";
        mSel01 = "";
        mSel02 = "";
        mSel03 = "";
        mSel04 = "";
        mSel05 = "";
        mSel06 = "";
        mSel07 = "";
        mSel08 = "";
        mEncodeParam = "";
        mEncodeParamWithFlag = "";
        mDate = "";
    }

    /**
     * エンコード文字列の入力
     * @param param エンコード文字列
     */
    public void setEncodeParamWithFlag( String param )
    {
        mEncodeParamWithFlag = param;
    }

    /**
     * エンコード文字列の出力
     * @return mEncodeParamWithFlag
     */
    public String getmEncodeParamWithFlag()
    {
        return mEncodeParamWithFlag;
    }

    /**
     * 各パラメータ->mEncodeParamのエンコード処理
     * 各メンバ変数が以下条件を満たしているかチェックを行う
     * ・文字数制限
     * ・必須項目が空でない
     * ・数字であれば指定範囲内である
     * 条件を満たしている場合はメンバ変数にCSV形式文字列をセットする
     * @return true:成功 false:失敗
     */
    public ArrayList<Boolean> encode()
    {
        ArrayList<Boolean> result = new ArrayList<Boolean>();
        String encode = "";
        try {
            // 電話番号(個人ID)
            if (mId.isEmpty() || mId.length() > MAX_LENGTH_ID) {
                Log.e(this.getClass().getName(), "encode:mId error:" + mId);
                result.add(false);
            } else {
                encode = mId + ",";
                result.add(true);
            }

            // 氏名
            if (mFamilyName.isEmpty() || mFamilyName.length() > MAX_LENGTH_FIRST_NAME || !Util.isSJIS(mFamilyName)) {
                Log.e(this.getClass().getName(), "encode:mName error:" + mFamilyName);
                result.add(false);
            } else if (mGivenName.isEmpty() || mGivenName.length() > MAX_LENGTH_GIVEN_NAME || !Util.isSJIS(mGivenName)) {
                Log.e(this.getClass().getName(), "encode:mName error:" + mGivenName);
                result.add(false);
            }else{
                mName = mFamilyName + "　" + mGivenName;
                encode += mName + ",";
                result.add(true);
            }

            // 生年月日
            boolean isIllegalDate = false;  // 不正な日付である場合true
            try{
                // 不正な日付が入力された場合は例外を投げる
                DateFormat df = new SimpleDateFormat("yyyyMMdd");
                df.setLenient(false);
                df.parse(mBirthday);
            }catch (ParseException e){
                isIllegalDate = true;
            }
            if (isIllegalDate) {
                Log.e(this.getClass().getName(),"encode:mBirthday error:" + mBirthday);
                result.add(false);
            } else {
                encode += mBirthday + ",";
                result.add(true);
            }

            // 性別
            if (mSel01.isEmpty() || ( Integer.decode(mSel01) < 0 && Integer.decode(mSel01) > 1)) {
                Log.e(this.getClass().getName(),"encode:mSel01 error:" + mSel01);
                result.add(false);
            } else {
                encode += mSel01 + ",";
                result.add(true);
            }

            // 入退所
            if (mSel02.isEmpty() || ( Integer.decode(mSel02) < 0 && Integer.decode(mSel02) > 2)) {
                Log.e(this.getClass().getName(),"encode:mSel02 error:" + mSel02);
                result.add(false);
            } else {
                encode += mSel02 + ",";
                result.add(true);
            }

            // 公表
            if (mSel03.isEmpty() || ( Integer.decode(mSel03) < 0 && Integer.decode(mSel03) > 1)) {
                Log.e(this.getClass().getName(),"encode:mSel03 error:" + mSel03);
                result.add(false);
            } else {
                encode += mSel03 + ",";
                result.add(true);
            }

            // 住所
            if (mTxt01.length() > 64 || !Util.isSJIS(mTxt01)) {
                Log.e(this.getClass().getName(),"encode:mTxt01 error:" + mTxt01);
                result.add(false);
            } else {
                encode += mTxt01 + ",";
                result.add(true);
            }

            // 怪我
            if (!mSel04.isEmpty() && Integer.decode(mSel04) < 0 && Integer.decode(mSel04) > 2) {
                Log.e(this.getClass().getName(),"encode:mSel04 error:" + mSel04);
                result.add(false);
            } else {
                encode += mSel04 + ",";
                result.add(true);
            }

            // 要介護
            if (!mSel05.isEmpty() && Integer.decode(mSel05) < 0 && Integer.decode(mSel05) > 2) {
                Log.e(this.getClass().getName(),"encode:mSel05 error:" + mSel05);
                result.add(false);
            } else {
                encode += mSel05 + ",";
                result.add(true);
            }

            // 障がい
            if (!mSel06.isEmpty() && Integer.decode(mSel06) < 0 && Integer.decode(mSel06) > 2) {
                Log.e(this.getClass().getName(),"encode:mSel06 error:" + mSel06);
                result.add(false);
            } else {
                encode += mSel06 + ",";
                result.add(true);
            }

            // 妊産婦
            if (!mSel07.isEmpty() && Integer.decode(mSel07) < 0 && Integer.decode(mSel07) > 2) {
                Log.e(this.getClass().getName(),"encode:mSel07 error:" + mSel07);
                result.add(false);
            } else {
                encode += mSel07 + ",";
                result.add(true);
            }

            // 避難所内外フラグ(内(0)固定)
            mSel08 = "0";
            encode += mSel08;

            if( result.indexOf(false) == -1 ) {
                mEncodeParam = encode;
            }
            // 暗号化フラグを付与
            if (mIsEncryption) {
                encode = CipherManager.encrypt(encode);
                mEncodeParamWithFlag = "1" + encode;
            } else {
                mEncodeParamWithFlag = "0" + encode;
            }
        }
        catch (Exception e)
        {
            Log.e(this.getClass().getName(),"encode:exception:" + e);
            return null;
        }
        return result;
    }

    /**
     * mEncodeParam->各パラメータのデコード処理
     * 各項目が以下条件を満たしているかチェックを行う
     * ・文字数制限
     * ・必須項目が空でない
     * ・数字であれば指定範囲内である
     * 条件を満たしている場合はメンバ変数に各項目をセットする
     * @return true:成功 false:失敗
     */
    public boolean decode()
    {
        // 暗号化フラグをチェック
        if (!mEncodeParamWithFlag.isEmpty()) {
            if (mEncodeParamWithFlag.charAt(0) == FLAG_CIPHER_TRUE) {
                try {
                    // 暗号
                    mEncodeParam = CipherManager.decrypt(mEncodeParamWithFlag.substring(1));
                } catch (Exception e) {
                    Log.e(this.getClass().getName(), "decode:decrypt error:" + mEncodeParamWithFlag);
                    Log.e(this.getClass().getName(), "Exception:" + e.toString());
                    return false;
                }
            } else if (mEncodeParamWithFlag.charAt(0) == FLAG_CIPHER_FALSE) {
                // 平文
                mEncodeParam = mEncodeParamWithFlag.substring(1);
            } else {
                // それ以外
                Log.e(this.getClass().getName(), "decode:decrypt error:" + mEncodeParamWithFlag);
                return false;
            }
        }

        if( mEncodeParam.isEmpty() ) {
            return false;
        }

        try {
            String[] splitParam = mEncodeParam.split(",", -1);      // 空欄も配列に含むこと

            if( splitParam.length != NUM_ALL_PARAM) {
                Log.e(this.getClass().getName(),"decode:splitNum error:" + splitParam.length);
                return false;
            }

            // 電話番号(個人ID)
            if (splitParam[0].isEmpty() || splitParam[0].length() > MAX_LENGTH_ID) {
                Log.e(this.getClass().getName(),"decode:splitParam[0](mId) error:" + splitParam[0]);
                return false;
            }

            // 氏名
            if (splitParam[1].isEmpty() || splitParam[1].length() > MAX_LENGTH_FULL_NAME) {
                Log.e(this.getClass().getName(),"decode:splitParam[1](mName) error:" + splitParam[1]);
                return false;
            }

            // 生年月日
            if (splitParam[2].isEmpty() || splitParam[2].length() != LENGTH_BIRHTDAY) {
                Log.e(this.getClass().getName(),"decode:splitParam[2](mBirthday) error:" + splitParam[2]);
                return false;
            }

            // 性別
            if (splitParam[3].isEmpty() || ( Integer.decode(splitParam[3]) < 0 && Integer.decode(splitParam[3]) > 1)) {
                Log.e(this.getClass().getName(),"decode:splitParam[3](mSel01) error:" + splitParam[3]);
                return false;
            }

            // 入退所
            if (splitParam[4].isEmpty() || ( Integer.decode(splitParam[4]) < 0 && Integer.decode(splitParam[4]) > 2)) {
                Log.e(this.getClass().getName(),"decode:splitParam[4](mSel02) error:" + splitParam[4]);
                return false;
            }

            // 公表
            if (splitParam[5].isEmpty() || ( Integer.decode(splitParam[5]) < 0 && Integer.decode(splitParam[5]) > 1)) {
                Log.e(this.getClass().getName(),"decode:splitParam[5](mSel03) error:" + splitParam[5]);
                return false;
            }

            // 住所
            if (splitParam[6].isEmpty()) {
                splitParam[6] = "";
            } else {
                if (splitParam[6].length() > MAX_LENGTH_ADDRESS) {
                    Log.e(this.getClass().getName(), "decode:splitParam[6](mTxt01) error:" + splitParam[6]);
                    return false;
                }
            }

            // 怪我
            if( splitParam[7].isEmpty() ) {
                splitParam[7] = "";
            } else {
                if (Integer.decode(splitParam[7]) < 0 && Integer.decode(splitParam[7]) > 1) {
                    Log.e(this.getClass().getName(), "decode:splitParam[7](mSel04) error:" + splitParam[7]);
                    return false;
                }
            }

            // 要介護
            if( splitParam[8].isEmpty() ) {
                splitParam[8] = "";
            } else {
                if (Integer.decode(splitParam[8]) < 0 && Integer.decode(splitParam[8]) > 1) {
                    Log.e(this.getClass().getName(), "decode:splitParam[8](mSel05) error:" + splitParam[8]);
                    return false;
                }
            }

            // 障がい
            if( splitParam[9].isEmpty() ) {
                splitParam[9] = "";
            } else {
                if (Integer.decode(splitParam[9]) < 0 && Integer.decode(splitParam[9]) > 1) {
                    Log.e(this.getClass().getName(), "decode:splitParam[9](mSel06) error:" + splitParam[9]);
                    return false;
                }
            }

            // 妊産婦
            if( splitParam[10].isEmpty() ) {
                splitParam[10] = "";
            } else {
                if (Integer.decode(splitParam[10]) < 0 && Integer.decode(splitParam[10]) > 1) {
                    Log.e(this.getClass().getName(), "decode:splitParam[10](mSel07) error:" + splitParam[10]);
                    return false;
                }
            }

            // 避難所内外フラグ
            if( splitParam[11].isEmpty() ) {
                splitParam[11] = "0"; // 「0:内」で固定
            } else {
                if (Integer.decode(splitParam[11]) < 0 && Integer.decode(splitParam[11]) > 1) {
                    Log.e(this.getClass().getName(), "decode:splitParam[11](mSel08) error:" + splitParam[11]);
                    return false;
                }
            }

            mId = splitParam[0];
            mName = splitParam[1];
            mBirthday = splitParam[2];
            mSel01 = splitParam[3];
            mSel02 = splitParam[4];
            mSel03 = splitParam[5];
            mTxt01 = splitParam[6];
            mSel04 = splitParam[7];
            mSel05 = splitParam[8];
            mSel06 = splitParam[9];
            mSel07 = splitParam[10];
            mSel08 = splitParam[11];
        }
        catch (Exception e)
        {
            Log.e(this.getClass().getName(),"decode:Exception:" + e);
            return false;
        }

        return true;
    }

    /**
     * 電話番号(個人ID)取得
     * @return ID(文字列)
     */
    public String getPersonalId()
    {
        return mId;
    }

    /**
     * 電話番号(個人ID)設定
     * @param set ID(文字列)
     */
    public void setPersonalId(String set)
    {
        mId = set;
    }

    /**
     * 氏名取得
     * @return 氏名(文字列)
     */
    public String getPersonalName()
    {
        return mName;
    }

    /**
     * 姓取得
     * @return 姓
     */
    public String getPersonalFamilyName()
    {
        return mFamilyName;
    }

    /**
     * 姓設定
     * @param set 姓
     */
    public void setPersonalFamilyName(String set)
    {
        mFamilyName = set;
    }

    /**
     * 名取得
     * @return 名
     */
    public String getPersonalGivenName()
    {
        return mGivenName;
    }

    /**
     * 名設定
     * @param set 名
     */
    public void setPersonalGivenName(String set)
    {
        mGivenName = set;
    }

    /**
     * 生年月日取得
     * @return  生年月日(文字列)
     */
    public String getPersonalBirthday()
    {
        return mBirthday;
    }

    /**
     * * 生年月日設定
     * @param set 生年月日(文字列)
     */
    public void setPersonalBirthday(String set)
    {
        mBirthday = set;
    }

    /**
     * 性別取得
     * @return 性別(表示文字列)
     */
    public String getPersonalSex()
    {
        String res;
        if(mSel01.equals("0"))
        {
            res = "男性";
        }
        else if(mSel01.equals("1"))
        {
            res = "女性";
        }
        else
        {
            res = "";
        }
        return res;
    }

    /**
     * 性別値取得
     * @return 性別(数値)
     */
    public int getPersonalSexNum()
    {
        int res;
        try
        {
            if( mSel01.isEmpty() ) {
                return -1;
            } else {
                res = Integer.decode(mSel01);
            }
        }
        catch (Exception e)
        {
            Log.e(this.getClass().getName(),"getPersonalSexNum:Exception:" + e);
            return -1;
        }
        return res;
    }


    /**
     * 性別設定
     * @param set 性別(数値)
     */
    public void setPersonalSex(int set)
    {
        mSel01 = String.valueOf(set);
    }

    /**
     * 入所状況取得
     * @return 入所状況(表示文字列)
     */
    public String getPersonalEnter()
    {
        String res;
        if( mSel02.equals("0") )
        {
            res = "入所";
        }
        else if( mSel02.equals("1") )
        {
            res = "退所";
        }
        else if( mSel02.equals("2") )
        {
            res = "在宅";
        }
        else
        {
            res = "";
        }
        return res;
    }

    /**
     * 入所状況値取得
     * @return 入所状況(数値)
     */
    public int getPersonalEnterNum()
    {
        int res;
        try
        {
            if( mSel02.isEmpty() ) {
                return -1;
            } else {
                res = Integer.decode(mSel02);
            }
        }
        catch (Exception e)
        {
            Log.e(this.getClass().getName(),"getPersonalEnterNum:Exception:" + e);
            return -1;
        }
        return res;
    }

    /**
     * 入所状況設定
     * @param set 入所対処(0:退所 1:入所 2:在宅)
     */
    public void setPersonalEnter(int set)
    {
        mSel02 = String.valueOf(set);
    }


    /**
     * 公表取得
     * @return 公表(表示文字列)
     */
    public String getPersonalPublish()
    {
        String res;
        if( mSel03.equals("0") )
        {
            res = "拒否";
        }
        else if( mSel03.equals("1") )
        {
            res = "許可";
        }
        else
        {
            res = "";
        }
        return res;
    }

    /**
     * 公表値取得
     * @return 公表(数値)
     */
    public int getPersonalPublishNum()
    {
        int res;
        try
        {
            if( mSel03.isEmpty()) {
                return -1;
            } else {
                res = Integer.decode(mSel03);
            }
        }
        catch (Exception e)
        {
            Log.e(this.getClass().getName(),"getPersonalPublishNum:Exception:" + e);
            return -1;
        }
        return res;
    }

    /**
     * 公表設定
     * @param set 公表(0:しない 1:する)
     */
    public void setPersonalPublish(int set)
    {
        mSel03 = String.valueOf(set);
    }

    /**
     * 住所取得
     * @return 住所(文字列)
     */
    public String getPersonalAddress()
    {
        return mTxt01;
    }

    /**
     * 住所設定
     * @param set 住所(文字列)
     */
    public void setPersonalAddress(String set)
    {
        mTxt01 = set;
    }

    /**
     * 怪我有無取得
     * @return 怪我(表示文字列)
     */
    public String getPersonalInjury()
    {
        String res;
        if( mSel04.equals("0") )
        {
            res = "無し";
        }
        else if( mSel04.equals("1") )
        {
            res = "有り";
        }
        else
        {
            res = "";
        }
        return res;
    }

    /**
     * 怪我有無値取得
     * @return 怪我(数値)
     */
    public int getPersonalInjuryNum()
    {
        int res;
        try
        {
            if( mSel04.isEmpty() )
            {
                res = -2;
            } else {
                res = Integer.decode(mSel04);
            }
        }
        catch (Exception e)
        {
            Log.e(this.getClass().getName(),"getPersonalInjuryNum:Exception:" + e);
            return -1;
        }
        return res;
    }

    /**
     * 怪我有無設定
     * @param set 怪我の有無(2:未選択 0:無 1:有)
     */
    public void setPersonalInjury(int set)
    {
        mSel04 = String.valueOf(set);
    }

    /**
     * 要介護取得
     * @return 要介護(表示文字列)
     */
    public String getPersonalCare()
    {
        String res;
        if( mSel05.equals("0") )
        {
            res = "不要";
        }
        else if( mSel05.equals("1") )
        {
            res = "必要";
        }
        else
        {
            res = "";
        }
        return res;
    }

    /**
     * 要介護値取得
     * @return 要介護(数値)
     */
    public int getPersonalCareNum()
    {
        int res;
        try
        {
            if( mSel05.isEmpty() ) {
                res = -2;
            } else {
                res = Integer.decode(mSel05);
            }
        }
        catch (Exception e)
        {
            Log.e(this.getClass().getName(),"getPersonalCareNum:Exception:" + e);
            return -1;
        }
        return res;
    }

    /**
     * 要介護設定
     * @param set 介護 (2:未選択 0:否  1:要)
     */
    public void setPersonalCare(int set)
    {
        mSel05 = String.valueOf(set);
    }

    /**
     * 障がい有無取得
     * @return 障がい(表示文字列)
     */
    public String getPersonalDisability()
    {
        String res;
        if( mSel06.equals("0") )
        {
            res = "無し";
        }
        else if( mSel06.equals("1") )
        {
            res = "有り";
        }
        else
        {
            res = "";
        }
        return res;
    }

    /**
     * 障がい有無値取得
     * @return 障がい(数値)
     */
    public int getPersonalDisabilityNum()
    {
        int res;
        try
        {
            if(mSel06.isEmpty()){
                res = -2;
            }else{
                res = Integer.decode(mSel06);
            }
        }
        catch (Exception e)
        {
            Log.e(this.getClass().getName(),"getPersonalDisabilityNum:Exception:" + e);
            return -1;
        }
        return res;
    }

    /**
     * 障がい者有無設定
     * @param set 障がい (2:未選択 0:無 1:有)
     */
    public void setPersonalDisability(int set)
    {
        mSel06 = String.valueOf(set);
    }

    /**
     * 妊産婦有無取得
     * @return 妊産婦(文字列)
     */
    public String getPersonalExpectant()
    {
        String res;
        if( mSel07.equals("0") )
        {
            res = "いいえ";
        }
        else if( mSel07.equals("1") )
        {
            res = "はい";
        }
        else
        {
            res = "";
        }
        return res;
    }

    /**
     * 妊産婦有無値取得
     * @return 妊産婦(数値)
     */
    public int getPersonalExpectantNum()
    {
        int res;
        try
        {
            if(mSel07.isEmpty()){
                res = -2;
            }else {
                res = Integer.decode(mSel07);
            }
        }
        catch (Exception e)
        {
            Log.e(this.getClass().getName(),"getPersonalExpectantNum:Exception:" + e);
            return -1;
        }
        return res;
    }

    /**
     * 妊産婦設定
     * @param set 妊産婦 (2:未選択 0:いいえ 1:はい)
     */
    public void setPersonalExpectant(int set)
    {
        mSel07 = String.valueOf(set);
    }

    /**
     * 避難所内外フラグ取得
     * @return 避難所内外フラグ(文字列)
     */
    public String getShelterFlg()
    {
        String res;
        if( mSel08.equals("0") )
        {
            res = "内";
        }
        else if( mSel08.equals("1") )
        {
            res = "外";
        }
        else
        {
            res = "";
        }
        return res;
    }

    /**
     * 避難所内外フラグ取得
     * @return 避難所内外フラグ(数値)
     */
    public int getShelterFlgNum()
    {
        int res;
        try
        {
            if(mSel08.isEmpty()){
                res = -2;
            }else {
                res = Integer.decode(mSel08);
            }
        }
        catch (Exception e)
        {
            Log.e(this.getClass().getName(),"getShelterFlgNum:Exception:" + e);
            return -1;
        }
        return res;
    }

    /**
     * 避難所内外フラグ設定
     * @param set 避難所内外フラグ (0:内 1:外)
     */
    public void setShelterFlg(int set)
    {
        mSel08 = String.valueOf(set);
    }

    /**
     * 登録日時取得
     * @return 登録日時
     */
    public String getRegisteredDate(){
        return mDate;
    }

    /**
     * 登録日時設定
     * @param date 登録日時
     */
    public void setRegisteredDate(String date){
        mDate = date;
    }

    /**
     * 暗号化有無取得
     * @return 登録日時
     */
    public boolean getIsEncryption(){
        return mIsEncryption;
    }

    /**
     * 暗号化有無設定
     * @param isEncryption 暗号化有無 true：暗号化する　false：暗号化しない
     */
    public void setIsEncryption(boolean isEncryption){
        mIsEncryption = isEncryption;
    }

    /**
     * 必須入力項目入力条件を満たしているかを確認する
     * @return true：必須項目入力十分　false：必須項目入力不十分
     */
    public boolean isSufficientRequiredInput(){
        boolean bRtn = true;
        // 電話番号入力条件
        if(mId.isEmpty()){
            bRtn = false;
        }
        // 名前入力条件
        if(mFamilyName.isEmpty() || mFamilyName.length() > MAX_LENGTH_FIRST_NAME || !Util.isSJIS(mFamilyName)){
            bRtn = false;
        }
        if(mGivenName.isEmpty() || mGivenName.length() > MAX_LENGTH_GIVEN_NAME || !Util.isSJIS(mGivenName)){
            bRtn = false;
        }
        // 生年月日入力条件
        try{
            // 不正な日付が入力された場合は例外を投げる
            DateFormat df = new SimpleDateFormat("yyyyMMdd");
            df.setLenient(false);
            df.parse(mBirthday);
        }catch (ParseException e){
            bRtn = false;
        }
        // 性別入力条件
        if(mSel01.isEmpty()){
            bRtn = false;
        }
        // 入所/退所/在宅
        // 選択項目であり、選択が外れる事が無い為チェックなし
        // 安否情報の公表可否
        if(mSel03.isEmpty()){
            bRtn = false;
        }
        // 住所
        if (mTxt01.length() > 64 || !Util.isSJIS(mTxt01)) {
            bRtn = false;
        }

        return bRtn;
    }
}
