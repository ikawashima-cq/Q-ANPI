package jp.co.nec.personinfoqrGenelator;

import android.app.Activity;
import android.content.Context;
import android.graphics.Point;
import android.graphics.Rect;
import android.os.Build;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.Display;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewTreeObserver;
import android.view.WindowManager;
import android.widget.LinearLayout;

import java.io.UnsupportedEncodingException;
import java.lang.reflect.Method;
import java.text.DateFormat;
import java.util.Date;

/**
 * Created by 113105A008PEZ on 2017/10/24.
 */

public class Util {
    // 画面サイズフルスクリーン設定
    public void setActivitySizeFullScreen (Activity activity, int screenId) {
        Display d = activity.getWindowManager().getDefaultDisplay();
        Point size = new Point(0, 0);
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN_MR1)
        {
            d.getRealSize(size);
        }
        else if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB_MR2)
        {
            try {
                Method getRawWidth = Display.class.getMethod("getRawWidth");
                Method getRawHeight = Display.class.getMethod("getRawHeight");
                size.set((Integer) getRawWidth.invoke(d), (Integer) getRawHeight.invoke(d));
            } catch (Exception e) {
                Log.e(this.getClass().getName(), "onCreate:fullscreen error");
            }
        }

        LinearLayout layout = (LinearLayout) activity.findViewById(screenId);
        ViewGroup.LayoutParams params = layout.getLayoutParams();
        params.width = size.x;
        params.height = size.y;
        layout.setLayoutParams(params);
    }

    // 画面サイズスクリーン設定(ナビ表示あり)
    public void setActivitySize (final Activity activity, final int screenId) {
        activity.getWindow().getDecorView().getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    @Override
                    public void onGlobalLayout() {
                        Display d = activity.getWindowManager().getDefaultDisplay();
                        Point size = new Point(0, 0);
                        d.getSize(size);
                        Rect rect = new Rect();
                        activity.getWindow().getDecorView().getWindowVisibleDisplayFrame(rect);
                        LinearLayout layout = (LinearLayout) activity.findViewById(screenId);
                        ViewGroup.LayoutParams params = layout.getLayoutParams();
                        params.width = size.x;
                        params.height = size.y - rect.top;
                        layout.setLayoutParams(params);
                    }
                }
        );
    }

    // ナビゲーション表示削除
    public void setBarVisible(Activity activity, int screenId) {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.KITKAT) {
            activity.findViewById(screenId).setSystemUiVisibility(View.SYSTEM_UI_FLAG_FULLSCREEN | View.SYSTEM_UI_FLAG_HIDE_NAVIGATION | View.SYSTEM_UI_FLAG_IMMERSIVE_STICKY);
        } else if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN) {
            activity.findViewById(screenId).setSystemUiVisibility(View.SYSTEM_UI_FLAG_FULLSCREEN | View.SYSTEM_UI_FLAG_HIDE_NAVIGATION);
        } else {
            activity.findViewById(screenId).setSystemUiVisibility(View.SYSTEM_UI_FLAG_HIDE_NAVIGATION);
        }
    }

    // タブレットか確認する
    public boolean isTablet(Context context) {
        WindowManager windowManager = (WindowManager)context.getSystemService(Context.WINDOW_SERVICE);
        Display display = windowManager.getDefaultDisplay();

        DisplayMetrics metrics = new DisplayMetrics();
        display.getMetrics(metrics);

        double inchX = metrics.widthPixels / metrics.xdpi;
        double inchY = metrics.heightPixels / metrics.ydpi;
        double inch = Math.sqrt((inchX * inchX) + (inchY * inchY));

        return inch > 6;
    }

    /**
     * 現在日時を取得する
     * @param df データフォーマット
     * @return 日付文字列
     */
    public static String getNowDate(DateFormat df){
        final Date date = new Date(System.currentTimeMillis());
        return df.format(date);
    }


    /**
     * 文字がSJISに変換できるか判定する
     * @param src
     * @return true：変換可能　false：Evaluating…変換不可
     */
    public static boolean isSJIS(String src){
        try {
            byte[] srcBytes = src.getBytes("Shift_JIS");
            String tmp = new String(srcBytes, "Shift_JIS");
            return src.equals(tmp);
        }
        catch(UnsupportedEncodingException e) {
            return false;
        }
    }
}
