package jp.co.nec.personinfoqrGenelator;

import android.os.Handler;
import android.os.Message;

/**
 * Created by omiya on 2018/08/01.
 */

public abstract class PauseHandler extends Handler {
    Message msgBuffer = null;   // Message バッファ
    boolean paused; // アクティビティ状態管理フラグ

    @Override
    public void handleMessage(Message msg) {
        if (paused) {
            msgBuffer = new Message();
            msgBuffer.copyFrom(msg);
        } else {
            processMessage(msg);
        }
    }

    /**
     * 再開時実行
     */
    public void resume() {
        paused = false;

        if(msgBuffer != null) {
            final Message msg = msgBuffer;
            msgBuffer = null;
            sendMessage(msg);
        }
    }

    /**
     * 一時停止時実行
     */
    public void pause() {
        paused = true;
    }

    /**
     * メッセージ実行
     * @param message
     */
    protected abstract void processMessage(Message message);
}
