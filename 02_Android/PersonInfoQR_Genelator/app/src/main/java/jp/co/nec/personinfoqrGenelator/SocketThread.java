package jp.co.nec.personinfoqrGenelator;

import android.os.Handler;
import android.os.Message;
import android.util.Log;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.net.SocketTimeoutException;
import java.net.UnknownHostException;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;

/**
 * Created by omiya on 2018/07/24.
 */

public class SocketThread implements Runnable{
    public static final int INPUT_STREAM_BUFFER_SIZE = 6;   // 受信データ格納用バッファーサイズ
    // メッセージ
    public static final short MSG_COMMUNICATION_COMPLETE = 0x0001; // 完了
    public static final short MSG_COMMUNICATION_ERROR = 0x0002; // エラー
    public static final short MSG_COMMUNICATION_CONNECT = 0x0003; // コネクト成功

    private final int RESP_BYTE_DEATLED_CODE = 5;   // 受信コード5バイト目（LittleEndian 詳細コード）
    // 詳細コード
    private final byte RESP_DEAILED_CODE_OK = 0x00;
    private final byte RESP_DEAILED_CODE_QUEUE_FULL = 0x01;
    private final byte RESP_DEAILED_CODE_SERVER_FAULT = 0x02;
    private final byte RESP_DEAILED_CODE_FORMAT_ERROR = 0x03;
    private final byte RESP_DEAILED_CODE_DATA_CORRUPTION = 0x04;
    private final byte RESP_DEAILED_CODE_DECODING_FAILURE = 0x05;

    private final int RECV_TIME_OUT_SEC = 10;   // 応答タイムアウト(秒)

    private String ipAddress = "";
    private int port;
    private Socket socket;
    private Handler handler;
    private String sendData;

    InputStream inputStream = null;
    OutputStream outputStream = null;

    public SocketThread (Handler handler, String ipAddress, int port, String sendData){
        this.handler = handler;
        this.ipAddress = ipAddress;
        this.port = port;
        this.sendData = sendData;
    }

    @Override
    public void run() {
        Message toMainThreadMsg = new Message();
        try {
//            socket = new Socket(ipAddress, port);   // サーバへ接続
            InetSocketAddress endpoint = new InetSocketAddress(ipAddress, port);
            socket = new Socket();
            socket.connect(endpoint, RECV_TIME_OUT_SEC * 1000);

            // コネクト成功通知
            Message connectMsg = new Message();
            connectMsg.what = MSG_COMMUNICATION_CONNECT;
            handler.sendMessage(connectMsg);

            inputStream = socket.getInputStream();
            outputStream = socket.getOutputStream();

            // 通信処理
            byte[] data = createTransmissionData(sendData); // 送信データ生成
            if (data == null){
                toMainThreadMsg.what = MSG_COMMUNICATION_ERROR;
                toMainThreadMsg.obj = "送信に失敗しました。\n";
            } else {
                outputStream.write(data);   // データ送信
                // 応答受付
                byte[] inputBuff = new byte[INPUT_STREAM_BUFFER_SIZE];  // 受信データ
                socket.setSoTimeout(RECV_TIME_OUT_SEC * 1000);  // タイムアウトセット
                try {
                    while (inputStream.read(inputBuff) != -1) {
                        // 詳細コードチェック
                        toMainThreadMsg = peceptionProcessing(inputBuff[RESP_BYTE_DEATLED_CODE - 1]);
                        break;
                    }
                } catch (SocketTimeoutException e) {
                    toMainThreadMsg.what = MSG_COMMUNICATION_ERROR;
                    toMainThreadMsg.obj = "応答受信タイムアウトしました。送信に失敗しました。\n";
                }
            }
        } catch (SocketTimeoutException e) {
            toMainThreadMsg.what = MSG_COMMUNICATION_ERROR;
            toMainThreadMsg.obj = "接続に失敗しました。\n";
            Log.e(this.getClass().getName(), "Exception:" + e.toString());
        } catch (UnknownHostException e) {
            toMainThreadMsg.what = MSG_COMMUNICATION_ERROR;
            toMainThreadMsg.obj = "接続に失敗しました。\n";
            Log.e(this.getClass().getName(), "Exception:" + e.toString());
        } catch (IOException e) {
            toMainThreadMsg.what = MSG_COMMUNICATION_ERROR;
            toMainThreadMsg.obj = "接続に失敗しました。\n";
            Log.e(this.getClass().getName(), "Exception:" + e.toString());
        } finally {
            if (socket != null) {
                try {
                    socket.close();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }
        handler.sendMessage(toMainThreadMsg);
    }


    /**
     * 送信データを作成
     * @param personInfo 個人安否情報
     * @return 送信データ
     */
    private byte[] createTransmissionData(String personInfo) {
        final int DATASIZE_PART_SIZE = 2;   // データサイズ部のサイズ（byte）
        try {
            byte[] bPersonInfo = personInfo.getBytes("Shift_JIS");
            // データサイズ部のバイトオーダー変換
            short nLen = (short)(bPersonInfo.length + DATASIZE_PART_SIZE);
            byte[] size = new byte[2];
            ByteBuffer bybuff = ByteBuffer.wrap(size);
            bybuff.order(ByteOrder.LITTLE_ENDIAN);
            bybuff.putShort(nLen);

            byte[] rSendData = new byte[size.length + bPersonInfo.length];
            System.arraycopy(size, 0, rSendData, 0, size.length);
            System.arraycopy(bPersonInfo, 0, rSendData, size.length, bPersonInfo.length);
            return rSendData;
        } catch (Exception e) {
            Log.e(this.getClass().getName(), "Exception:" + e.toString());
            return null;
        }
    }

    /**
     * 受信時処理
     * objにエラー内容文字列を格納
     * @param detaileCode 詳細コード
     * @return Message
     */
    private Message peceptionProcessing (byte detaileCode) {
        Message retMsg = new Message();
        switch(detaileCode) {
            case RESP_DEAILED_CODE_OK:
                retMsg.what = MSG_COMMUNICATION_COMPLETE;
                break;
            case RESP_DEAILED_CODE_QUEUE_FULL:
                retMsg.what = MSG_COMMUNICATION_ERROR;
                retMsg.obj = "しばらく時間をおいて再度送信してください。\n";
                break;
            case RESP_DEAILED_CODE_SERVER_FAULT:
                retMsg.what = MSG_COMMUNICATION_ERROR;
                retMsg.obj = "サーバ側で障害が発生しました。送信に失敗しました。\n";
                break;
            case RESP_DEAILED_CODE_FORMAT_ERROR:
                retMsg.what = MSG_COMMUNICATION_ERROR;
                retMsg.obj = "不正なフォーマットです。送信に失敗しました。";
                break;
            case RESP_DEAILED_CODE_DATA_CORRUPTION:
                retMsg.what = MSG_COMMUNICATION_ERROR;
                retMsg.obj = "不正なデータです。送信に失敗しました。";
                break;
            case RESP_DEAILED_CODE_DECODING_FAILURE:
                retMsg.what = MSG_COMMUNICATION_ERROR;
                retMsg.obj = "情報の復号に失敗したため避難所安否登録に失敗しました。";
                break;
            default:
                // Reserved
                break;
        }
        return retMsg;
    }

}
