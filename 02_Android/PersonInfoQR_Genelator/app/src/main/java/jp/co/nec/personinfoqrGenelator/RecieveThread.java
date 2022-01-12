package jp.co.nec.personinfoqrGenelator;

import android.content.SharedPreferences;
import android.net.wifi.WifiManager;

import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.util.Arrays;

import static jp.co.nec.personinfoqrGenelator.MainActivity.SHARED_PREF_SEND_IP;
import static jp.co.nec.personinfoqrGenelator.MainActivity.SHARED_PREF_SEND_PORT;

public class RecieveThread extends Thread{

    // ブロードキャストのポート番号(PCアプリ版と同じ)
    private static final int BROADCAST_PORT_NO = 56412;
    // マルチキャストロックのタグ
    private static final String LOCK_TAG = "lock" ;
    // 受信データの文字コード
    private static final String CHARACTER_SET = "UTF-8";
    // ソケットのタイムアウト
    private static final int TIMEOUT = 10000;
    // 終了フラグ
    private boolean finishFlag  = false;

    private SharedPreferences data;
    private WifiManager wifi;

    public RecieveThread (SharedPreferences data, WifiManager wifi){
        this.data = data;
        this.wifi = wifi;
    }

    @Override
    public void run(){

        DatagramSocket recvUdpSocket = null;
        DatagramPacket packet = null;

        // マルチキャストロックを取得
        WifiManager.MulticastLock lock = wifi.createMulticastLock(LOCK_TAG);
        lock.acquire();

        while(!finishFlag){
            try {
                // データを受信
                recvUdpSocket = new DatagramSocket(BROADCAST_PORT_NO);
                byte[] buffer = new byte[10];
                packet = new DatagramPacket(buffer, buffer.length);
                recvUdpSocket.setSoTimeout(TIMEOUT);

                recvUdpSocket.receive(packet);

                // IPアドレスの設定
                String sendIpAdress = null;
                InetAddress inetAddress = packet.getAddress();
                if (inetAddress != null) {
                    // 一文字目の「/」を削除して文字列化
                    sendIpAdress = inetAddress.toString().substring(1);
                } else {
                    // IPアドレス未収得時は破棄
                    continue;
                }

                // ポート番号の設定
                // 受信したデータをデコード
                String sendPort = null;
                String result = new String(resizeByteData(buffer), CHARACTER_SET);
                if (result != null) {
                    //　先頭文字列が「ANPI_」ではない場合は破棄
                    if (result.matches("^ANPI_.*")) {
                        sendPort = result.substring(5);

                        // 数値チェック
                        // 数値ではない場合は破棄
                        try {
                            Integer.parseInt(sendPort);
                        } catch (NumberFormatException e) {
                            continue;
                        }
                    } else {
                        continue;
                    }
                } else {
                    continue;
                }

                SharedPreferences.Editor editor = data.edit();
                editor.putString(SHARED_PREF_SEND_IP, sendIpAdress);
                editor.putString(SHARED_PREF_SEND_PORT, sendPort);
                editor.commit();

            }
            catch (Exception e) {

                continue;
            }
            finally{
                if(recvUdpSocket != null){
                    recvUdpSocket.close();
                }
            }
        }
        // マルチキャストのロックを解除
        lock.release();
    }
    /**
     * 受信データのバイト配列をリサイズする
     * @param buffer バイト配列
     * @return リサイズしたバイト配列
     */
    private byte[] resizeByteData(byte[] buffer){
        int size = 0;
        for(byte b : buffer){
            if(b == 0x00){
                return Arrays.copyOf(buffer, size);
            }
            else{
                size++;
            }
        }
        return buffer;
    }

    /**
     * 　データ受信スレッドを終了する
     */
    public void finish(){
        this.finishFlag = true;
    }
}
