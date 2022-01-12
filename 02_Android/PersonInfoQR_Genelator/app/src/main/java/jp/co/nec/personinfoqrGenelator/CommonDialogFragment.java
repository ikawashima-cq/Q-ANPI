package jp.co.nec.personinfoqrGenelator;

import android.app.AlertDialog;
import android.app.Dialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.graphics.Color;
import android.os.Bundle;
import android.support.v4.app.DialogFragment;
import android.util.Log;
import android.view.Gravity;
import android.view.KeyEvent;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;

import static jp.co.nec.personinfoqrGenelator.MainActivity.SHARED_PREF_FILENAME;
import static jp.co.nec.personinfoqrGenelator.MainActivity.SHARED_PREF_SEND_IP;
import static jp.co.nec.personinfoqrGenelator.MainActivity.SHARED_PREF_SEND_PORT;

/**
 * Created by omiya on 2018/07/18.
 * 全てのアクティビティで共通のダイアログ処理については当クラスに記載する
 */

public class CommonDialogFragment extends DialogFragment {

    public interface CommonDialogInterface{
        void onDialogButtonClick(String tag, Dialog dialog, int whitch);

        void onDialogShow(String tag, Dialog dialog);

        void onDialogDismiss(String tag, Dialog dialog);
    }

    // ダイアログ表示部品
    public static final String FIELD_LAYOUT = "layout";
    public static final String FIELD_TITLE = "title";
    public static final String FIELD_MESSAGE = "message";   // ダイアログに表示するメッセージ
    public static final String FIELD_LABEL_POSITIVE = "label_positive";
    public static final String FIELD_LABEL_NEGATIVE = "label_negative";
    public static final String FIELD_TEXT_SEND_IP = "text_send_ip"; // 送信ダイアログのIP
    public static final String FIELD_TEXT_SEND_PORT = "text_send_port"; // 送信ダイアログのPort

    // ダイアログタグ
    public static final String DIALOG_SEND = "send";    // 送信ダイアログ
    public static final String DIALOG_SEND_NOW = "send_now";    // 送信中ダイアログ
    public static final String DIALOG_SEND_ERROR = "send_error";    // 送信エラーダイアログ
    public static final String DIALOG_INPUT_ERROR = "input_error";  // 入力エラーダイアログ
    public static final String DIALOG_SEND_COMPLETE = "send_complete";    // 送信完了ダイアログ


    private CommonDialogInterface mListener;
    private static AlertDialog mAlertDialog;

    public static boolean mIsShowDialog = false;    // ダイアログ表示有無

    @Override
    public Dialog onCreateDialog(Bundle saveInstanceState){
        final Bundle args = getArguments();

        AlertDialog.Builder builder = new AlertDialog.Builder(getActivity(), android.R.style.Theme_DeviceDefault_Light_Dialog_NoActionBar);

        // 呼び出し元FragmentかActivityを取得
        if (getTargetFragment() != null){
            setListener(getTargetFragment());
        } else if(getActivity() != null){
            setListener(getActivity());
        }

        // Title
        if (args.containsKey(FIELD_TITLE)) {
            if( getTag() == DIALOG_SEND_NOW){
                TextView title = new TextView(getContext());
                title.setPadding(30, 30, 30, 30);
                title.setGravity(Gravity.CENTER);
                title.setText(args.getInt(FIELD_TITLE));
                title.setTextColor(Color.BLACK);
                title.setTextSize(20);
                builder.setCustomTitle(title);
            } else {
                builder.setTitle(args.getInt(FIELD_TITLE));
            }
        }

        // Message
        if (args.containsKey(FIELD_MESSAGE)) {
            builder.setMessage(args.getString(FIELD_MESSAGE));
        }

        // Customize View
        if (args.containsKey(FIELD_LAYOUT)) {
            LayoutInflater inflater = getActivity().getLayoutInflater();
            View view = inflater.inflate(args.getInt(FIELD_LAYOUT), null);
            if (getTag().equals(DIALOG_SEND)) {
                // 送信ダイアログ
                if (args.containsKey(FIELD_TEXT_SEND_IP)) {
                    EditText etIp = (EditText) view.findViewById(R.id.ip_address);
                    etIp.setText(args.getString(FIELD_TEXT_SEND_IP));
                }
                if (args.containsKey(FIELD_TEXT_SEND_PORT)) {
                    EditText etPort = (EditText) view.findViewById(R.id.port);
                    etPort.setText(args.getString(FIELD_TEXT_SEND_PORT));
                }
            }
            builder.setView(view);
        }

        // Positive button click listener
        if (args.containsKey(FIELD_LABEL_POSITIVE)) {
            builder.setPositiveButton(args.getInt(FIELD_LABEL_POSITIVE), new DialogInterface.OnClickListener() {
                @Override
                public void onClick(DialogInterface dialog, int which) {
                    mIsShowDialog = false;
                    String tag = getTag();
                    switch (tag){
                        case DIALOG_SEND:
                            String ip = ((EditText) mAlertDialog.findViewById(R.id.ip_address)).getText().toString();
                            String port = ((EditText) mAlertDialog.findViewById(R.id.port)).getText().toString();
                            // IPとPortの文字判定
                            int iPort = 0;
                            try{
                                iPort = Integer.parseInt(port);
                            }catch (NumberFormatException e){
                                e.printStackTrace();
                            }
                            if (ip.isEmpty() || port.isEmpty() || iPort < 0 || iPort > 65535) {
                                // ダイアログを表示
                                Bundle errorDialogArgs = new Bundle();
                                errorDialogArgs.putInt(FIELD_TITLE, R.string.send_error_dialog_title);
                                errorDialogArgs.putInt(FIELD_MESSAGE, R.string.send_error_ip_port);
                                errorDialogArgs.putInt(FIELD_LABEL_NEGATIVE, R.string.send_error_dialog_negative_btn);
                                CommonDialogFragment errorDialog = new CommonDialogFragment();
                                errorDialog.setArguments(errorDialogArgs);
                                errorDialog.show(getFragmentManager(), DIALOG_SEND_ERROR );
                            } else {
                                // SharedPreferences にIPとPortを保存
                                SharedPreferences data = getActivity().getSharedPreferences(SHARED_PREF_FILENAME, Context.MODE_PRIVATE);
                                SharedPreferences.Editor editor = data.edit();
                                editor.putString(SHARED_PREF_SEND_IP, ip);
                                editor.putString(SHARED_PREF_SEND_PORT, port);
                                editor.commit();
                                // 送信ダイアログを表示
                                Bundle sendNowDialogArgs = new Bundle();
                                sendNowDialogArgs.putInt(FIELD_TITLE, R.string.send_now_dialog_title);
                                CommonDialogFragment sendNowDialog = new CommonDialogFragment();
                                sendNowDialog.setArguments(sendNowDialogArgs);
                                sendNowDialog.show(getFragmentManager(), DIALOG_SEND_NOW);
                                ((GenelateActivity)getActivity()).startSocketThread(ip, port);  // 通信スレッド開始
                            }
                            break;
                        default:
                            if (mListener != null) {
                                mListener.onDialogButtonClick(getTag(), mAlertDialog, which);
                            }
                            break;
                    }
                }
            });
        }

        // Negative button click listener
        if (args.containsKey(FIELD_LABEL_NEGATIVE)) {
            builder.setNegativeButton(args.getInt(FIELD_LABEL_NEGATIVE), new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialog, int which) {
                    mIsShowDialog = false;
                    if (mListener != null) {
                        mListener.onDialogButtonClick(getTag(), mAlertDialog, which);
                    }
                }
            });
        }

        // ハードボタン
        builder.setOnKeyListener(new DialogInterface.OnKeyListener() {
            @Override
            public boolean onKey(DialogInterface dialog, int keyCode, KeyEvent event) {
                switch (keyCode) {
                    case KeyEvent.KEYCODE_BACK:
                    case KeyEvent.KEYCODE_SEARCH:
                        // 戻るボタン、検索ボタンを無効
                        return true;
                    default:
                        return false;
                }
            }
        });

        mAlertDialog = builder.create();

        // Show dialog
        if (mListener != null) {
            mAlertDialog.setOnShowListener(new DialogInterface.OnShowListener() {
                @Override
                public void onShow(DialogInterface dialog) {
                    String tag = getTag();
                    switch (tag) {
                        default:
                            mListener.onDialogShow(getTag(), mAlertDialog);
                            break;
                    }
                }
            });
        }


        mAlertDialog.setCanceledOnTouchOutside(false);

        return mAlertDialog;
    }

    @Override
    public void show(android.support.v4.app.FragmentManager manager, String tag) {
        if (mIsShowDialog) {
            return;
        }
        mIsShowDialog = true;
        try {
            super.show(manager, tag);
        } catch (Exception e) {
            mIsShowDialog = false;
            Log.e(this.getClass().getName(), "Exception:" + e.toString());
        }
    }

    @Override
    public void onResume() {
        super.onResume();
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
        mIsShowDialog = false;
    }

    @Override
    public void dismiss() {
        super.dismiss();
        mIsShowDialog =false;
    }

    /**
     * Listener登録
     * @param listener
     */
    private void setListener(Object listener) {
        if (listener instanceof CommonDialogInterface) {
            mListener = (CommonDialogInterface) listener;
        }
    }

}
