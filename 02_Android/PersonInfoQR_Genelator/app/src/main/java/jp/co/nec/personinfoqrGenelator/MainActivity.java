package jp.co.nec.personinfoqrGenelator;

import android.app.AlertDialog;
import android.app.Dialog;
import android.app.DialogFragment;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.net.Uri;
import android.net.wifi.WifiManager;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.text.InputFilter;
import android.text.Spanned;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.inputmethod.InputMethodManager;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.NumberPicker;
import android.widget.RadioGroup;
import android.widget.ScrollView;
import android.widget.TextView;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;

import static jp.co.nec.personinfoqrGenelator.CommonDialogFragment.DIALOG_INPUT_ERROR;
import static jp.co.nec.personinfoqrGenelator.CommonDialogFragment.FIELD_LABEL_POSITIVE;
import static jp.co.nec.personinfoqrGenelator.CommonDialogFragment.FIELD_MESSAGE;
import static jp.co.nec.personinfoqrGenelator.CommonDialogFragment.FIELD_TITLE;
import static jp.co.nec.personinfoqrGenelator.ParseParam.MAX_LENGTH_ADDRESS;
import static jp.co.nec.personinfoqrGenelator.ParseParam.MAX_LENGTH_FIRST_NAME;

public class MainActivity extends AppCompatActivity implements CommonDialogFragment.CommonDialogInterface{
    public static final int REQUEST_CODE_GENERATE = 1;
    public static final int RESULT_GENERATE_ERROR = 1000;

    private ParseParam mParam;
    Button btn_next;
    Button btm_last_registered_info;
    boolean mViewNoEssential;

    // 前回登録情報
    public static final String SHARED_PREF_FILENAME = "LastRegisterd";
    private static final String SHARED_PREF_DATE = "date";
    private static final String SHARED_PREF_ID = "id";
    private static final String SHARED_PREF_FAMILY_NAME = "family_name";
    private static final String SHARED_PREF_GIVEN_NAME = "given_name";
    private static final String SHARED_PREF_BIRTHDAY = "birthday";
    private static final String SHARED_PREF_SEX = "sex";
    private static final String SHARED_PREF_ENTER = "enter";
    private static final String SHARED_PREF_PUBLISH = "publish";
    private static final String SHARED_PREF_ADDRESS = "address";
    private static final String SHARED_PREF_INJURY = "injury";
    private static final String SHARED_PREF_CARE = "care";
    private static final String SHARED_PREF_DISABILITY = "disability";
    private static final String SHARED_PREF_EXPECTANT = "expectant";
    private static final String SHARED_PREF_IS_ENCRYPTION = "is_encryption";
    public static final String SHARED_PREF_SEND_IP = "send_ip";
    public static final String SHARED_PREF_SEND_PORT = "send_port";

    private static final String PRIVACY_POLICY_URI = "http://qzss.go.jp/technical/q-anpi/privacy-policy.html\n";

    private RecieveThread recieveThread = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        final InputFilter[] nameFilter = new InputFilter[] {new EditTextFilter(), new InputFilter.LengthFilter(MAX_LENGTH_FIRST_NAME)};
        final InputFilter[] addressFilter = new InputFilter[] {new EditTextFilter(), new InputFilter.LengthFilter(MAX_LENGTH_ADDRESS)};

        // EditTextにフィルターを追加
        EditText et = (EditText) findViewById(R.id.input_txt_family_name);
        et.setFilters(nameFilter);
        et = (EditText) findViewById(R.id.input_txt_given_name);
        et.setFilters(nameFilter);
        et = (EditText) findViewById(R.id.input_txt_address);
        et.setFilters(addressFilter);

        mParam = new ParseParam();
        btn_next = (Button)findViewById(R.id.btn_next);
        btm_last_registered_info = (Button)findViewById(R.id.btn_last_registered_info);
        btm_last_registered_info.setEnabled(isLastRegisteredInfo());
        clearField();

        startRecieveThread();

        mViewNoEssential = false;
    }

    @Override
    protected void onResume() {
        super.onResume();
        Util util = new Util();
        if( util.isTablet(this) ) {
            util.setBarVisible(this, R.id.layout);
            util.setActivitySizeFullScreen(this, R.id.content);
        } else {
            util.setActivitySize(this, R.id.content);
        }
        btm_last_registered_info.setEnabled(isLastRegisteredInfo());
        mParam.init();
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        switch (requestCode) {
            //SecondActivityから戻ってきた場合
            case (REQUEST_CODE_GENERATE):
                if (resultCode == RESULT_OK) {
                    // 画面の先頭にスクロール
                    final ScrollView scrollView = (ScrollView) findViewById(R.id.input_view);
                    scrollView.post(new Runnable() {
                        public void run() {
                            scrollView.fullScroll(ScrollView.FOCUS_UP);
                        }
                    });
                    Log.e(this.getClass().getName(), "Routes that do not exist. ResultCode:" + resultCode);
                } else if (resultCode == RESULT_CANCELED) {
                    // 端末の戻るボタン押下
                } else if (resultCode == RESULT_GENERATE_ERROR) {
                    // 本来存在しないはずのルート
                    Bundle args = new Bundle();
                    args.putInt(FIELD_TITLE, R.string.input_error_dialog_title);
                    args.putString(FIELD_MESSAGE, "入力エラー。");
                    args.putInt(FIELD_LABEL_POSITIVE, R.string.input_error_dialog_positive_btn);
                    CommonDialogFragment dialog = new CommonDialogFragment();
                    dialog.setArguments(args);
                    dialog.show(getSupportFragmentManager(), DIALOG_INPUT_ERROR);
                } else {
                    // 存在しないルート
                    Log.e(this.getClass().getName(), "Routes that do not exist. ResultCode:" + resultCode);
                }
                break;
            default:
                break;
        }
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();

        // ブロードキャスト受信スレッドを終了
        finishRecieveThread();
    }

    // テキスト設定
    public void setText(String txt, int id)
    {
        TextView view;

        if( id == R.id.input_txt_id ||
                id == R.id.input_txt_family_name ||
                id == R.id.input_txt_given_name ||
                id == R.id.input_num_birth_year ||
                id == R.id.input_num_birth_month ||
                id == R.id.input_num_birth_day ||
                id == R.id.input_txt_address)
        {
            view = (TextView)findViewById(id);
        } else {
            return;
        }
        view.setText(txt);
    }

    // 選択設定
    public void clearSelect(int id)
    {
        RadioGroup group;
        if( id == R.id.input_select_sex ||
                id == R.id.input_select_publish ||
                id == R.id.input_select_enter ||
                id == R.id.input_select_injury ||
                id == R.id.input_select_care ||
                id == R.id.input_select_disability ||
                id == R.id.input_select_expectant)
        {
            group = (RadioGroup)findViewById(id);
        } else {
            return;
        }
        if(id == R.id.input_select_enter){
            group.check(R.id.input_selbtn_enter_0);
        } else {
            group.clearCheck();
        }

    }

    // チェックボックスクリア
    private void clearCheck (int id) {
        if (id == R.id.is_encryption) {
            CheckBox checkBox = (CheckBox) findViewById(R.id.is_encryption);
            checkBox.setChecked(false);
        }
    }

    // 入力テキスト、選択状態のクリア
    public void clearField()
    {
        EditText txt_id = (EditText)findViewById(R.id.input_txt_id);

        setText("", R.id.input_txt_id);
        setText("", R.id.input_txt_family_name);
        setText("", R.id.input_txt_given_name);
        setText("", R.id.input_num_birth_year);
        setText("", R.id.input_num_birth_month);
        setText("", R.id.input_num_birth_day);
        setText("", R.id.input_txt_address);
        setText("", R.id.input_num_birth_year);
        setText("", R.id.input_num_birth_month);
        setText("", R.id.input_num_birth_day);
        txt_id.requestFocus();

        clearSelect(R.id.input_select_sex);
        clearSelect(R.id.input_select_publish);
        clearSelect(R.id.input_select_enter);
        clearSelect(R.id.input_select_injury);
        clearSelect(R.id.input_select_care);
        clearSelect(R.id.input_select_disability);
        clearSelect(R.id.input_select_expectant);

        clearCheck(R.id.is_encryption);
        mParam.init();
    }

    // ボタンクリックイベント定義
    public void onButtonClick(View view){
        switch (view.getId()) {
            case R.id.btn_clear:
                clearField();
                break;
            case R.id.btn_last_registered_info:
                loadLastRegisteredInfo();
                callQRGenelate();
                break;
            case R.id.btn_next:
                onNextButtonClick();
                break;
            case R.id.input_btn_essential:
                setNoEssential();
                break;
            case R.id.input_num_birth_year:
                final DateFormat yearDf = new SimpleDateFormat("yyyy");
                int year = Integer.parseInt(Util.getNowDate(yearDf));
                callNumberDialog(year - 120, year, year, "生年月日（年）", R.id.input_num_birth_year);
                break;
            case R.id.input_num_birth_month:
                final DateFormat monthDf = new SimpleDateFormat("M");
                int month = Integer.parseInt(Util.getNowDate(monthDf));
                callNumberDialog(1, 12, month,"生年月日（月）", R.id.input_num_birth_month);
                break;
            case R.id.input_num_birth_day:
                final DateFormat dayDf = new SimpleDateFormat("d");
                int day = Integer.parseInt(Util.getNowDate(dayDf));
                callNumberDialog(1, 31, day,"生年月日（日）", R.id.input_num_birth_day);
                break;
            default:
                break;
        }
    }

    public void callQRGenelate()
    {
        ArrayList<Boolean> encodeResult = mParam.encode();
        if(encodeResult == null || encodeResult.indexOf(false) != -1) {
            // エンコード失敗
            callErrorDialog(encodeResult);
            return;
        }

        Intent intent = new Intent(MainActivity.this, GenelateActivity.class);
        intent.putExtra("data", mParam.getmEncodeParamWithFlag());   // 入力データ
        intent.putExtra("date", mParam.getRegisteredDate()); // 日付
        // 次画面のアクティビティ起動
        startActivityForResult(intent, REQUEST_CODE_GENERATE);
    }

    public void setNoEssential()
    {
        // キーボードを隠す
        if (this.getCurrentFocus() != null) {
            InputMethodManager imm = (InputMethodManager) getSystemService(Context.INPUT_METHOD_SERVICE);
            imm.hideSoftInputFromWindow(this.getCurrentFocus().getWindowToken(), InputMethodManager.HIDE_NOT_ALWAYS);
        }

        LinearLayout address = (LinearLayout)findViewById(R.id.layout_address);
        LinearLayout injury = (LinearLayout)findViewById(R.id.layout_injury);
        LinearLayout care = (LinearLayout)findViewById(R.id.layout_care);
        LinearLayout disability = (LinearLayout)findViewById(R.id.layout_disability);
        LinearLayout expectant = (LinearLayout)findViewById(R.id.layout_expectant);
        Button noEssential = (Button)findViewById(R.id.input_btn_essential);

        if(mViewNoEssential) {
            address.setVisibility(View.GONE);
            injury.setVisibility(View.GONE);
            care.setVisibility(View.GONE);
            disability.setVisibility(View.GONE);
            expectant.setVisibility(View.GONE);
            noEssential.setText("▼");
        }
        else
        {
            address.setVisibility(View.VISIBLE);
            injury.setVisibility(View.VISIBLE);
            care.setVisibility(View.VISIBLE);
            disability.setVisibility(View.VISIBLE);
            expectant.setVisibility(View.VISIBLE);
            noEssential.setText("▲");
        }
        mViewNoEssential = !mViewNoEssential;
    }


    /**
     * 内容確認ボタン押下時処理
     */
    private void onNextButtonClick(){
        setmParam();
        // メンバ変数から保存値を取得しているため、setmParam()の後に実行
        saveLastRegisteredInfo();
        callQRGenelate();
    }

    /**
     * ParseParamオブジェクトに値を設定する
     */
    private void setmParam(){
        // テキスト入力項目を取得
        EditText idText = (EditText) findViewById(R.id.input_txt_id);
        EditText familyNameText = (EditText) findViewById(R.id.input_txt_family_name);
        EditText givenNameText = (EditText) findViewById(R.id.input_txt_given_name);
        EditText addressText = (EditText) findViewById(R.id.input_txt_address);
        TextView birthYearText = (TextView) findViewById(R.id.input_num_birth_year);
        TextView birthMonthText = (TextView) findViewById(R.id.input_num_birth_month);
        TextView birthDayText = (TextView) findViewById(R.id.input_num_birth_day);

        // 生年月日文字列の整形
        String strYear = birthYearText.getText().toString();
        String strMonth = birthMonthText.getText().toString();
        String strDay = birthDayText.getText().toString();
        String birthday = strYear + String.format("%2s", strMonth).replace(" ", "0") + String.format("%2s", strDay).replace(" ", "0");

        // テキスト入力項目をオブジェクトに設定
        mParam.setPersonalId(idText.getText().toString());
        mParam.setPersonalFamilyName(familyNameText.getText().toString());
        mParam.setPersonalGivenName(givenNameText.getText().toString());
        mParam.setPersonalBirthday(birthday);
        mParam.setPersonalAddress(addressText.getText().toString());

        // 選択項目を取得
        RadioGroup rgSex = (RadioGroup)findViewById(R.id.input_select_sex);
        RadioGroup rgEnter = (RadioGroup)findViewById(R.id.input_select_enter);
        RadioGroup rgPublish = (RadioGroup)findViewById(R.id.input_select_publish);
        RadioGroup rgInjury = (RadioGroup)findViewById(R.id.input_select_injury);
        RadioGroup rgCare = (RadioGroup)findViewById(R.id.input_select_care);
        RadioGroup rgDisability = (RadioGroup)findViewById(R.id.input_select_disability);
        RadioGroup rgExpectant = (RadioGroup)findViewById(R.id.input_select_expectant);

        // 選択項目をオブジェクトに設定
        setmParamRadioButton(rgSex.getCheckedRadioButtonId());
        setmParamRadioButton(rgEnter.getCheckedRadioButtonId());
        setmParamRadioButton(rgPublish.getCheckedRadioButtonId());
        setmParamRadioButton(rgInjury.getCheckedRadioButtonId());
        setmParamRadioButton(rgCare.getCheckedRadioButtonId());
        setmParamRadioButton(rgDisability.getCheckedRadioButtonId());
        setmParamRadioButton(rgExpectant.getCheckedRadioButtonId());

        // 暗号化有無を取得
        CheckBox cbIsEncryption = (CheckBox) findViewById(R.id.is_encryption);

        // 暗号化有無をオブジェクトに設定
        mParam.setIsEncryption(cbIsEncryption.isChecked());
    }

    /**
     * ラジオボタンViewのidからParseParamオブジェクトに値を設定する
     * @param id
     */
    public void setmParamRadioButton(int id){
        switch (id) {
            case R.id.input_selbtn_sex_0:
                mParam.setPersonalSex(0);
                break;
            case R.id.input_selbtn_sex_1:
                mParam.setPersonalSex(1);
                break;
            case R.id.input_selbtn_enter_0:
                mParam.setPersonalEnter(0);
                break;
            case R.id.input_selbtn_enter_1:
                mParam.setPersonalEnter(1);
                break;
            case R.id.input_selbtn_enter_2:
                mParam.setPersonalEnter(2);
                break;
            case R.id.input_selbtn_publish_0:
                mParam.setPersonalPublish(0);
                break;
            case R.id.input_selbtn_publish_1:
                mParam.setPersonalPublish(1);
                break;
            case R.id.input_selbtn_injury_0:
                mParam.setPersonalInjury(0);
                break;
            case R.id.input_selbtn_injury_1:
                mParam.setPersonalInjury(1);
                break;
            case R.id.input_selbtn_care_0:
                mParam.setPersonalCare(0);
                break;
            case R.id.input_selbtn_care_1:
                mParam.setPersonalCare(1);
                break;
            case R.id.input_selbtn_disability_0:
                mParam.setPersonalDisability(0);
                break;
            case R.id.input_selbtn_disability_1:
                mParam.setPersonalDisability(1);
                break;
            case R.id.input_selbtn_expectant_0:
                mParam.setPersonalExpectant(0);
                break;
            case R.id.input_selbtn_expectant_1:
                mParam.setPersonalExpectant(1);
                break;
            default:
                break;
        }
    }

    /**
     * 前回登録情報有無
     */
    private boolean isLastRegisteredInfo(){
        SharedPreferences data = getSharedPreferences(SHARED_PREF_FILENAME, Context.MODE_PRIVATE);
        // IDは必須入力であるため、IDの登録有無を前回登録情報有無とする
        return data.contains(SHARED_PREF_ID);
    }

    /**
     * 前回登録情報の読込み
     */
    private void loadLastRegisteredInfo(){
        SharedPreferences data = getSharedPreferences(SHARED_PREF_FILENAME, Context.MODE_PRIVATE);

        String date = data.getString(SHARED_PREF_DATE, "");
        String id = data.getString(SHARED_PREF_ID, "");
        String familyName = data.getString(SHARED_PREF_FAMILY_NAME, "");
        String givenName = data.getString(SHARED_PREF_GIVEN_NAME, "");
        String day = data.getString(SHARED_PREF_BIRTHDAY, "");
        int sex = data.getInt(SHARED_PREF_SEX, 0);
        int enter = data.getInt(SHARED_PREF_ENTER, 0);
        int publish = data.getInt(SHARED_PREF_PUBLISH, 0);

        String address = data.getString(SHARED_PREF_ADDRESS, "");

        // 空の場合-2、失敗時-1が格納される
        int injury = data.getInt(SHARED_PREF_INJURY, 0);
        int care = data.getInt(SHARED_PREF_CARE, 0);
        int disability = data.getInt(SHARED_PREF_DISABILITY, 0);
        int expectant = data.getInt(SHARED_PREF_EXPECTANT, 0);

        boolean isEncryption = data.getBoolean(SHARED_PREF_IS_ENCRYPTION ,true);

        mParam.setRegisteredDate(date);
        mParam.setPersonalId(id);
        mParam.setPersonalFamilyName(familyName);
        mParam.setPersonalGivenName(givenName);
        mParam.setPersonalBirthday(day);
        mParam.setPersonalSex(sex);
        mParam.setPersonalEnter(enter);
        mParam.setPersonalPublish(publish);
        mParam.setPersonalAddress(address);
        if(injury != -2){
            mParam.setPersonalInjury(injury);
        }
        if(care != -2){
            mParam.setPersonalCare(care);
        }
        if(disability != -2){
            mParam.setPersonalDisability(disability);
        }
        if(expectant != -2){
            mParam.setPersonalExpectant(expectant);
        }
        mParam.setIsEncryption(isEncryption);
    }

    /**
     * 前回登録情報の保存
     */
    private void saveLastRegisteredInfo(){
        SharedPreferences data = getSharedPreferences(SHARED_PREF_FILENAME, Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = data.edit();

        final DateFormat df = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        String date = Util.getNowDate(df);

        String id = mParam.getPersonalId();
        String familyName = mParam.getPersonalFamilyName();
        String givenName = mParam.getPersonalGivenName();
        String birthday = mParam.getPersonalBirthday();

        int sex = mParam.getPersonalSexNum();
        int enter = mParam.getPersonalEnterNum();
        int publish = mParam.getPersonalPublishNum();

        String address = mParam.getPersonalAddress();

        int injury = mParam.getPersonalInjuryNum();
        int care = mParam.getPersonalCareNum();
        int disability = mParam.getPersonalDisabilityNum();
        int expectant = mParam.getPersonalExpectantNum();

        boolean isEncryption = mParam.getIsEncryption();

        if(!mParam.isSufficientRequiredInput()){
            // 必須項目が不十分のため値取得失敗
            return;
        }

        editor.putString(SHARED_PREF_DATE, date);
        editor.putString(SHARED_PREF_ID, id);
        editor.putString(SHARED_PREF_FAMILY_NAME, familyName);
        editor.putString(SHARED_PREF_GIVEN_NAME, givenName);
        editor.putString(SHARED_PREF_BIRTHDAY, birthday);
        editor.putInt(SHARED_PREF_SEX, sex);
        editor.putInt(SHARED_PREF_ENTER, enter);
        editor.putInt(SHARED_PREF_PUBLISH, publish);
        editor.putString(SHARED_PREF_ADDRESS, address);
        editor.putInt(SHARED_PREF_INJURY, injury);
        editor.putInt(SHARED_PREF_CARE, care);
        editor.putInt(SHARED_PREF_DISABILITY, disability);
        editor.putInt(SHARED_PREF_EXPECTANT, expectant);
        editor.putBoolean(SHARED_PREF_IS_ENCRYPTION, isEncryption);
        editor.commit();
    }

    /**
     * エラーダイアログ表示
     * @param encodeResult エンコード成功/失敗リスト
     */
    public void callErrorDialog(ArrayList<Boolean> encodeResult){
        String viewMessage[] = {"電話番号を入力してください。\n","名前を入力してください。\n","生年月日を正しく入力してください。\n",
                "性別を選択してください。\n", "入所状況を選択してください。\n", "公表可否を選択してください。\n","住所を正しく入力してください。\n",
                "怪我の有無を正しく選択してください。\n", "介護要否を正しく選択してください。\n", "障がいの有無を正しく選択してください。\n", "妊産婦を正しく選択してください。\n"};
        String message = "";

        // encode()でのException対策
        if(encodeResult == null) {
            message = "エンコードに失敗しました。\n";
        } else {
            for(int i = 0; i < encodeResult.size(); i++) {
                if (!encodeResult.get(i)) {
                    message += viewMessage[i];
                }
            }
        }

        Bundle args = new Bundle();
        args.putInt(FIELD_TITLE, R.string.input_error_dialog_title);
        args.putString(FIELD_MESSAGE, message);
        args.putInt(FIELD_LABEL_POSITIVE, R.string.input_error_dialog_positive_btn);
        CommonDialogFragment dialog = new CommonDialogFragment();
        dialog.setArguments(args);
        dialog.show(getSupportFragmentManager(), DIALOG_INPUT_ERROR);
    }


    /**
     * 単一数字選択ダイアログの表示
     * @param min 選択可能な数字の最小値
     * @param max 選択可能な数字の最大値
     * @param setnum 初期選択数字
     * @param title ダイアログタイトル
     * @param resid OKボタン押下時に選択している数字を表示するTextView
     */
    public void callNumberDialog(int min, int max,int setnum,  String title, int resid){
        Bundle bundle = new Bundle();
        bundle.putInt(NumberDialogFragment.MIN, min);
        bundle.putInt(NumberDialogFragment.MAX, max);
        bundle.putInt(NumberDialogFragment.SET, setnum);
        bundle.putString(NumberDialogFragment.TITLE, title);
        bundle.putInt(NumberDialogFragment.VIEW, resid);
        NumberDialogFragment dialog = new NumberDialogFragment();
        dialog.setArguments(bundle);
        dialog.show(getFragmentManager(), "number_dialog");
    }

    @Override
    public void onDialogButtonClick(String tag, Dialog dialog, int whitch) {

    }

    @Override
    public void onDialogShow(String tag, Dialog dialog) {

    }

    @Override
    public void onDialogDismiss(String tag, Dialog dialog) {

    }

    /**
     * 数字選択ダイアログクラス
     */
    public static class NumberDialogFragment extends DialogFragment {
        public static final String MIN = "min";
        public static final String MAX = "max";
        public static final String SET = "set";
        public static final String TITLE = "title";
        public static final String VIEW = "view";

        @Override
        public Dialog onCreateDialog(Bundle savedInstanceState) {
            LayoutInflater inflater = getActivity().getLayoutInflater();
            final View view = inflater.inflate(R.layout.number_picker_dialog, null);
            NumberPicker numberPicker = (NumberPicker) view.findViewById(R.id.numberPicker);

            int min = getArguments().getInt(MIN);
            int max = getArguments().getInt(MAX);
            int set = getArguments().getInt(SET);
            String title = getArguments().getString(TITLE);
            int resid = getArguments().getInt(VIEW);
            final TextView textView = (TextView) getActivity().findViewById(resid);
            String textDate = textView.getText().toString();

            numberPicker.setMinValue(min);
            numberPicker.setMaxValue(max);

            // TextViewに値が入力されていた場合はその値を初期選択値とする
            if(textDate.isEmpty()){
                numberPicker.setValue(set);
            }else{
                numberPicker.setValue(Integer.parseInt(textDate));
            }

            AlertDialog.Builder builder = new AlertDialog.Builder(getActivity(), R.style.Theme_AppCompat_Light_Dialog_Alert);
            builder.setTitle(title);
            builder.setPositiveButton("OK", new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    NumberPicker numberPicker = (NumberPicker) view.findViewById(R.id.numberPicker);
                    textView.setText(String.valueOf(numberPicker.getValue()));
                }
            });
            builder.setNegativeButton("Cancel", null);
            builder.setView(view);
            return builder.create();
        }
    }

    /**
     * EditTextのフィルター
     * 「,」入力時に確定できない
     */
    class EditTextFilter implements InputFilter {
        final String FILTER_REGEX = "^.*,.*";

        @Override
        public CharSequence filter(CharSequence source, int start, int end, Spanned dest, int dstart, int dend) {
            if (source.toString().matches(FILTER_REGEX)) {
                return "";
            } else {
                return source;
            }
        }
    }

    /**
     *  ブロードキャスト受信スレッドを開始
     */
    private void startRecieveThread() {

        // 受信スレッドが開始していない場合または受信スレッドが終了済の場合
        if(recieveThread == null ||
                Thread.State.TERMINATED == recieveThread.getState()){

            WifiManager wifi = (WifiManager) getApplicationContext().getSystemService(Context.WIFI_SERVICE);
            SharedPreferences data = getSharedPreferences(SHARED_PREF_FILENAME, Context.MODE_PRIVATE);

            recieveThread = new RecieveThread(data, wifi);
            recieveThread.start();
        }
    }

    /**
     *  ブロードキャスト受信スレッドを終了
     */
    private  void finishRecieveThread() {
        recieveThread.finish();
    }

    /**
     *  プライバシーポリシーをクリックしたときのイベント
     *  ブラウザを起動し、プライバシーポリシーを表示する。
     */
    public void onPrivacyPolicyText(View view){
        Intent intent = new Intent(Intent.ACTION_VIEW, Uri.parse(PRIVACY_POLICY_URI));
        startActivity(intent);
    }

}
