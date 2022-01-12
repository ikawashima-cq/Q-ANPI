package jp.co.nec.personinfoqrGenelator;

import android.app.Activity;
import android.app.Dialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.os.Bundle;
import android.os.Message;
import android.support.v7.app.AppCompatActivity;
import android.util.AndroidRuntimeException;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.google.zxing.BarcodeFormat;
import com.google.zxing.EncodeHintType;
import com.google.zxing.WriterException;
import com.journeyapps.barcodescanner.BarcodeEncoder;

import java.util.EnumMap;
import java.util.Map;

import static jp.co.nec.personinfoqrGenelator.CommonDialogFragment.DIALOG_SEND;
import static jp.co.nec.personinfoqrGenelator.CommonDialogFragment.DIALOG_SEND_COMPLETE;
import static jp.co.nec.personinfoqrGenelator.CommonDialogFragment.DIALOG_SEND_ERROR;
import static jp.co.nec.personinfoqrGenelator.CommonDialogFragment.DIALOG_SEND_NOW;
import static jp.co.nec.personinfoqrGenelator.CommonDialogFragment.FIELD_LABEL_NEGATIVE;
import static jp.co.nec.personinfoqrGenelator.CommonDialogFragment.FIELD_MESSAGE;
import static jp.co.nec.personinfoqrGenelator.CommonDialogFragment.FIELD_TITLE;
import static jp.co.nec.personinfoqrGenelator.MainActivity.RESULT_GENERATE_ERROR;
import static jp.co.nec.personinfoqrGenelator.MainActivity.SHARED_PREF_FILENAME;
import static jp.co.nec.personinfoqrGenelator.MainActivity.SHARED_PREF_SEND_IP;
import static jp.co.nec.personinfoqrGenelator.MainActivity.SHARED_PREF_SEND_PORT;
import static jp.co.nec.personinfoqrGenelator.SocketThread.MSG_COMMUNICATION_COMPLETE;
import static jp.co.nec.personinfoqrGenelator.SocketThread.MSG_COMMUNICATION_CONNECT;
import static jp.co.nec.personinfoqrGenelator.SocketThread.MSG_COMMUNICATION_ERROR;

public class GenelateActivity extends AppCompatActivity implements CommonDialogFragment.CommonDialogInterface{
    public static final int BARCODE_SIZE = 500;

    private String sendDataStr = "";
    private ActivityHandler mActivityHandler;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_genelate);

        mActivityHandler = new ActivityHandler();   // ハンドラ生成

        // テキストボックスに取得文字列を反映
        Intent intent = getIntent();
        String input_text = intent.getStringExtra( "data" );
        String input_date = intent.getStringExtra("date");

        if(input_text.isEmpty()) {
            // データがなければ戻る
            Log.e(this.getClass().getName(),"onCreate data empty");
            Toast.makeText(GenelateActivity.this, "generateQR data empty", Toast.LENGTH_SHORT).show();
            Intent result = new Intent();
            setResult( RESULT_GENERATE_ERROR, result );
            finish();
            return;
        }
        sendDataStr = input_text;

        ParseParam param = new ParseParam();
        param.setEncodeParamWithFlag(input_text);
        if(!param.decode()){
            // デコード失敗
            Log.e(this.getClass().getName(),"onCreate data format error");
            Intent result = new Intent();
            setResult( RESULT_GENERATE_ERROR, result );
            finish();
            return;
        }
        // QRコード表示
        genelateQR(input_text, R.id.view_qr);

        // 入力情報表示
        TextView textView;
        textView = (TextView)findViewById(R.id.text_view_date);
        textView.setText(input_date);
        textView = (TextView)findViewById(R.id.gen_txt_id);
        textView.setText(param.getPersonalId());
        textView = (TextView)findViewById(R.id.gen_txt_name);
        textView.setText(param.getPersonalName());
        textView = (TextView)findViewById(R.id.gen_txt_birthday);
        // 生年月日表示時はハイフンで区切る
        String strBirthday = param.getPersonalBirthday().substring(0, 4) + "-" + param.getPersonalBirthday().substring(4, 6) +
                "-" + param.getPersonalBirthday().substring(6, 8);
        textView.setText(strBirthday);
        textView = (TextView)findViewById(R.id.gen_txt_sex);
        textView.setText(param.getPersonalSex());
        textView = (TextView)findViewById(R.id.gen_txt_enter);
        textView.setText(param.getPersonalEnter());
        textView = (TextView)findViewById(R.id.gen_txt_publish);
        textView.setText(param.getPersonalPublish());
        textView = (TextView)findViewById(R.id.gen_txt_address);
        textView.setText(param.getPersonalAddress());
        textView = (TextView)findViewById(R.id.gen_txt_injury);
        textView.setText(param.getPersonalInjury());
        textView = (TextView)findViewById(R.id.gen_txt_care);
        textView.setText(param.getPersonalCare());
        textView = (TextView)findViewById(R.id.gen_txt_disability);
        textView.setText(param.getPersonalDisability());
        textView = (TextView)findViewById(R.id.gen_txt_expectant);
        textView.setText(param.getPersonalExpectant());
    }

    @Override
    protected void onResume() {
        super.onResume();
        // 画面表示サイズ指定
        Util util = new Util();

        if( util.isTablet(this) ) {
            util.setBarVisible(this, R.id.layout);
            util.setActivitySizeFullScreen(this, R.id.content);
        } else {
            util.setActivitySize(this, R.id.content);
        }
        mActivityHandler.resume();
    }

    @Override
    protected void onPause() {
        super.onPause();
        mActivityHandler.pause();
    }

    // QRコード発行処理
    public void genelateQR(String code, int id){
        ImageView imageView = (ImageView) findViewById(id);
        Intent intent = getIntent();
        try {
            BarcodeEncoder barcodeEncoder = new BarcodeEncoder();
            Map<EncodeHintType, Object> hints = new EnumMap<>(EncodeHintType.class);
            String strQRCode = code;
            hints.put(EncodeHintType.CHARACTER_SET, "Shift-JIS");
            Bitmap bitmap = barcodeEncoder.encodeBitmap(strQRCode, BarcodeFormat.QR_CODE, BARCODE_SIZE, BARCODE_SIZE, hints);
            imageView.setImageBitmap(bitmap);
        } catch (WriterException e) {
            throw new AndroidRuntimeException("Barcode Error.", e);
        } catch (Exception e){
            e.printStackTrace();
        }
    }

    // ボタンクリックイベント定義
    public void onButtonClick(View view){
        switch (view.getId()) {
            case R.id.btn_back:
                callBackTrans(view);
                break;
            case R.id.btn_send:
                callSendDialog();
                break;
            default:
                break;
        }
    }

    // 前画面に戻る
    private void callBackTrans(View v) {
        Intent result = new Intent();
        setResult( Activity.RESULT_OK, result );
        finish();
    }

    /**
     * 送信ダイアログ呼び出し
     */
    private void callSendDialog() {


        // 前回入力接続情報を取得
        SharedPreferences connectInfo = getSharedPreferences(SHARED_PREF_FILENAME, Context.MODE_PRIVATE);

//        // 送信ダイアログは表示せず、安否登録情報送信ダイアログを表示する
//        Bundle args = new Bundle();
//        args.putInt(FIELD_TITLE,R.string.send_dialog_title);
//        args.putInt(CommonDialogFragment.FIELD_LABEL_POSITIVE,R.string.send_dialog_positive_btn);
//        args.putInt(CommonDialogFragment.FIELD_LABEL_NEGATIVE,R.string.send_dialog_negative_btn);
//        args.putInt(CommonDialogFragment.FIELD_LAYOUT,R.layout.send_info_dialog);
//
//        args.putString(CommonDialogFragment.FIELD_TEXT_SEND_IP, connectInfo.getString(SHARED_PREF_SEND_IP, ""));
//        args.putString(CommonDialogFragment.FIELD_TEXT_SEND_PORT, connectInfo.getString(SHARED_PREF_SEND_PORT, ""));
//        CommonDialogFragment dialogFragment = new CommonDialogFragment();
//        dialogFragment.setArguments(args);
//        dialogFragment.show(getSupportFragmentManager(), DIALOG_SEND);

        String ip = connectInfo.getString(SHARED_PREF_SEND_IP, "");
        String port = connectInfo.getString(SHARED_PREF_SEND_PORT, "");
        // IPとPortの文字判定
        int iPort = -1;
        try{
            iPort = Integer.parseInt(port);
        }catch (NumberFormatException e){
            // 次の分岐でエラーダイアログが表示されるため何もしない
        }

        if (ip.isEmpty() || port.isEmpty() || iPort < 0 || iPort > 65535) {
            // ダイアログを表示
            Bundle errorDialogArgs = new Bundle();
            errorDialogArgs.putInt(FIELD_TITLE, R.string.send_error_dialog_title);
            errorDialogArgs.putInt(FIELD_MESSAGE, R.string.send_error_ip_port);
            errorDialogArgs.putInt(FIELD_LABEL_NEGATIVE, R.string.send_error_dialog_negative_btn);
            CommonDialogFragment errorDialog = new CommonDialogFragment();
            errorDialog.setArguments(errorDialogArgs);
            errorDialog.show(getSupportFragmentManager(), DIALOG_SEND_ERROR );
        }
        else {
            Bundle sendNowDialogArgs = new Bundle();
            sendNowDialogArgs.putInt(FIELD_TITLE, R.string.send_now_dialog_title);
            CommonDialogFragment sendNowDialog = new CommonDialogFragment();
            sendNowDialog.setArguments(sendNowDialogArgs);
            sendNowDialog.show(getSupportFragmentManager(), DIALOG_SEND_NOW);
            startSocketThread(ip, port);  // 通信スレッド開始
        }
    }

    /**
     * ソケット通信スレッドを開始
     * @param ipAddress ipアドレス
     * @param port ポート番号
     * @return SocketThread 通信用スレッド
     */
    public void startSocketThread(String ipAddress, String port) {
        SocketThread socketThread = null;
        int iPort = 0;
        try{
            iPort = Integer.parseInt(port);
        }catch (NumberFormatException e){
            e.printStackTrace();
        }
        socketThread = new SocketThread(mActivityHandler, ipAddress, iPort, sendDataStr); // ソケット通信スレッドの生成
        new Thread(socketThread).start();   // ソケット通信スレッド開始
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
     * アクティビティ用ハンドラ
     */
    class ActivityHandler extends PauseHandler {
        @Override
        protected void processMessage(Message message) {
            switch (message.what) {
                case MSG_COMMUNICATION_CONNECT:

                    break;
                case MSG_COMMUNICATION_ERROR :
                    // 送信エラー
                    if (getSupportFragmentManager().findFragmentByTag(DIALOG_SEND_NOW) != null) {
                        ((CommonDialogFragment) getSupportFragmentManager().findFragmentByTag(DIALOG_SEND_NOW)).dismiss();
                    }
                    Bundle errorDialogArgs = new Bundle();
                    errorDialogArgs.putInt(FIELD_TITLE, R.string.send_error_dialog_title);
                    errorDialogArgs.putInt(FIELD_LABEL_NEGATIVE, R.string.send_error_dialog_negative_btn);
                    errorDialogArgs.putString(FIELD_MESSAGE, (String) message.obj);
                    CommonDialogFragment errorDialogFragment = new CommonDialogFragment();
                    errorDialogFragment.setArguments(errorDialogArgs);
                    errorDialogFragment.show(getSupportFragmentManager(), DIALOG_SEND_ERROR);
                    break;
                case MSG_COMMUNICATION_COMPLETE:
                    // 送信完了
                    if (getSupportFragmentManager().findFragmentByTag(DIALOG_SEND_NOW) != null) {
                        ((CommonDialogFragment) getSupportFragmentManager().findFragmentByTag(DIALOG_SEND_NOW)).dismiss();
                    }
                    Bundle completeDialogArgs = new Bundle();
                    completeDialogArgs.putInt(FIELD_TITLE, R.string.send_complete_dialog_title);
                    completeDialogArgs.putInt(FIELD_LABEL_NEGATIVE, R.string.send_complete_dialog_negative_btn);
                    completeDialogArgs.putString(FIELD_MESSAGE, getString(R.string.send_complete_dialog_message));
                    CommonDialogFragment completeDialogFragment = new CommonDialogFragment();
                    completeDialogFragment.setArguments(completeDialogArgs);
                    completeDialogFragment.show(getSupportFragmentManager(), DIALOG_SEND_COMPLETE);
                    break;
                default:
                    break;
            }
        }
    }
}
