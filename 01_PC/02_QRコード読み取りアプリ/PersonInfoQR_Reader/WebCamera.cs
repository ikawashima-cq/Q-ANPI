using System;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace EncryptedQRCodeReader
{
    public class WebCamera : IDisposable
    {

        public enum WebCameraStatus : int
        {
            WC_OK = 0,
            WC_NOTFOUND = 1,      // カメラ未接続
            WC_INIERR = 2,        // iniファイル内容での更新失敗
            WC_OTHER = 3          // その他エラー
        }
        
        private const int WM_CAP_DRIVER_CONNECT = 0x40a;
        private const int WM_CAP_DRIVER_DISCONNECT = 0x40b;

        // カメラ画像取得
        private const int WM_CAP_EDIT_COPY = 0x41e;
        private const int WM_CAP_GRAB_FRAME = 0x43c;

        // ウィンドウ作成時
        private const int WM_CAP_SET_PREVIEW = 0x432;
        private const int WM_CAP_SET_PREVIEWRATE = 0x434;
        private const int WM_CAP_SET_SCALE = 0x435;


        private const int WM_CAP_DLG_VIDEOFORMAT = 0x429;
        private const int WM_CAP_DLG_VIDEOSOURCE = 0x42a;
        private const int WM_CAP_DLG_VIDEODISPLAY = 0x42b;
        private const int WM_CAP_GET_VIDEOFORMAT = 0x42c;
        private const int WM_CAP_SET_VIDEOFORMAT = 0x42d;
        private const int WM_CAP_DLG_VIDEOCOMPRESSION = 0x42e;

        private const int WM_CAP_DRIVER_GET_CAPS = 0x40e;

        private const int WS_CHILD = 0x40000000;
        private const int WS_VISIBLE = 0x10000000;
        private const int SWP_NOMOVE = 0x2;
        private const int SWP_NOZORDER = 0x4;
        private const int HWND_BOTTOM = 1;

        // iniファイルを読み込む際の情報
        private string m_Sec_WebCameraInfo = "WebCameraInfo";
        private string m_Key_Compression = "Compression";
        private string m_Key_ImageSize = "ImageSize";
        private string m_Key_BitCount = "BitCount";
        private string m_Key_Width = "Width";
        private string m_Key_Height = "Height";

        private string m_Sec_DebugInfo = "DebugInfo";
        private string m_Key_DebugMode = "DebugMode";

        private string m_Compression;
        private uint m_ImageSize;
        private ushort m_BitCount;
        private int m_Width;
        private int m_Height;
        private int m_DebugMode;

        // キャプチャデバイスのID(常に固定で0)
        int m_DeviceID = 0;
        // キャプチャ用ウィンドウのハンドル
        int m_hHwnd = 0;

        public struct CAPDRIVERCAPS
        {
            public uint a_DeviceIndex;
            public bool b_HasOverlay;
            public bool c_HasDlgVideoSource;
            public bool d_HasDlgVideoFormat;
            public bool e_HasDlgVideoDisplay;
            public bool f_CaptureInitialized;
            public bool g_DriverSuppliesPalettes;
            public uint h_1;
            public uint h_2;
            public uint h_3;
            public uint h_4;
        }
        
        public struct BITMAPINFOHEADER
        {
            public uint bi_a_Size;
            public int bi_b_Width;
            public int bi_c_Height;
            public ushort bi_d_Planes;
            public ushort bi_e_BitCount;
            public byte bi_f_Compression_1;
            public byte bi_f_Compression_2;
            public byte bi_f_Compression_3;
            public byte bi_f_Compression_4;
            public uint bi_g_SizeImage;
            public int bi_h_XPelsPerMeter;
            public int bi_i_YPelsPerMeter;
            public uint bi_j_ClrUsed;
            public uint bi_k_ClrImportant;

            public void Init()
            {
                bi_a_Size = (uint)Marshal.SizeOf(this);
            }
        }

        public struct RGBQUAD
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }

        public struct BITMAPINFO
        {
            public BITMAPINFOHEADER bmiaHeader;
            public RGBQUAD bmibColors;
        }
        private BITMAPINFO m_BitMap;

        //キャプチャ用ウィンドウ作成呼び出しのための宣言部分
        [DllImport("avicap32.dll")]
        protected static extern int capCreateCaptureWindowA([MarshalAs(UnmanagedType.VBByRefStr)] ref string lpszWindowName,
             int dwStyle, int x, int y, int nWidth, int nHeight, int hWndParent, int nID);

        //キャプチャ用ウィンドウサイズ変更呼び出しのための宣言部分
        [DllImport("user32")]
        protected static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        //キャプチャ用ウィンドウメッセージ通知呼び出しのための宣言部分
        [DllImport("user32", EntryPoint = "SendMessageA")]
        protected static extern int SendMessage(int hwnd, int wMsg, int wParam, [MarshalAs(UnmanagedType.AsAny)] object lParam);

        [DllImport("user32", EntryPoint = "SendMessage")]
        protected static extern int SendMessage(int hwnd, int wMsg, int wParam, out BITMAPINFO lParam);

        [DllImport("user32", EntryPoint = "SendMessage")]
        protected static extern int SendMessage(int hwnd, int wMsg, int wParam, out CAPDRIVERCAPS lParam);

        [DllImport("user32", EntryPoint = "SendMessage")]
        protected static extern int SendUpdateMessage(int hwnd, int wMsg, int wParam, ref BITMAPINFO lParam);

        //キャプチャ用ウィンドウ破棄呼び出しのための宣言部分
        [DllImport("user32")]
        protected static extern bool DestroyWindow(int hwnd);

        //表示用画像
        public PictureBox Container { get; set; }

        // キャプチャウィンドウの作成
        public WebCameraStatus OpenConnection()
        {
            string DeviceIndex = Convert.ToString(m_DeviceID);
            IntPtr oHandle = Container.Handle;

            // キャプチャ用ウィンドウ作成
            m_hHwnd = capCreateCaptureWindowA(ref DeviceIndex, WS_VISIBLE | WS_CHILD, 0, 0, 640, 480, oHandle.ToInt32(), 0);
            if (m_hHwnd == 0)
            {
                return WebCameraStatus.WC_OTHER;
            }

            // ドライバー接続のメッセージを送信
            if (SendMessage(m_hHwnd, WM_CAP_DRIVER_CONNECT, m_DeviceID, 0) != 0)
            {
                // Webカメラの設定を行う
                bool bRet = this.SetDefaut();
                if (bRet != true)
                {
                    CloseConnection();
                    return WebCameraStatus.WC_INIERR;
                }

                // デバッグ情報の出力(iniファイルで出力する設定になっていたら出力される)
                this.DebugInfo();

                SendMessage(m_hHwnd, WM_CAP_SET_SCALE, -1, 0);
                SendMessage(m_hHwnd, WM_CAP_SET_PREVIEWRATE, 100, 0);
                SendMessage(m_hHwnd, WM_CAP_SET_PREVIEW, -1, 0);
                SetWindowPos(m_hHwnd, HWND_BOTTOM, 0, 0, Container.Width, Container.Height, SWP_NOMOVE | SWP_NOZORDER);

                return WebCameraStatus.WC_OK;
            }
            else
            {
                DestroyWindow(m_hHwnd);
                return WebCameraStatus.WC_NOTFOUND;
            }
        }

        void CloseConnection()
        {
            SendMessage(m_hHwnd, WM_CAP_DRIVER_DISCONNECT, m_DeviceID, 0);
            DestroyWindow(m_hHwnd);
        }

        public Bitmap GetCurrentImage()
        {
            // フレームの取得
            int ret = SendMessage(m_hHwnd, WM_CAP_GRAB_FRAME, 0, 0);

            // クリップボードへコピー
            ret = SendMessage(m_hHwnd, WM_CAP_EDIT_COPY, 0, 0);

            // クリップボードの内容をビットマップ形式に変換
            IDataObject data = Clipboard.GetDataObject();
            if ((data != null) && (data.GetDataPresent(typeof(Bitmap))))
            {
                var oImage = (Bitmap)data.GetData(typeof(Bitmap));
                return oImage;
            }

            return null;
        }

        ~WebCamera()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public bool Init(IniFile iniFile)
        {
            // サポートする圧縮形式
            m_Compression = iniFile.GetString(m_Sec_WebCameraInfo, m_Key_Compression);
            if (m_Compression.Length <= 3)
            {
                iniFile.MsgBox(m_Sec_WebCameraInfo, m_Key_Compression);
                return false;
            }

            // サポートする圧縮形式の際の画像サイズ
            m_ImageSize = (uint)iniFile.GetInteger(m_Sec_WebCameraInfo, m_Key_ImageSize);
            if (m_ImageSize == 0)
            {
                iniFile.MsgBox(m_Sec_WebCameraInfo, m_Key_ImageSize);
                return false;
            }

            // サポートする圧縮形式の際の画像形式
            m_BitCount = (ushort)iniFile.GetInteger(m_Sec_WebCameraInfo, m_Key_BitCount);
            if (m_BitCount == 0)
            {
                iniFile.MsgBox(m_Sec_WebCameraInfo, m_Key_BitCount);
                return false;
            }

            // デフォルト解像度(横)
            m_Width = (ushort)iniFile.GetInteger(m_Sec_WebCameraInfo, m_Key_Width);
            if (m_Width == 0)
            {
                iniFile.MsgBox(m_Sec_WebCameraInfo, m_Key_Width);
                return false;
            }

            // デフォルト解像度(縦)
            m_Height = (ushort)iniFile.GetInteger(m_Sec_WebCameraInfo, m_Key_Height);
            if (m_Height == 0)
            {
                iniFile.MsgBox(m_Sec_WebCameraInfo, m_Key_Height);
                return false;
            }
            // デバッグ情報(取得できなくても問題ない)
            m_DebugMode = iniFile.GetInteger(m_Sec_DebugInfo, m_Key_DebugMode);

            m_BitMap = new BITMAPINFO();
            m_BitMap.bmiaHeader = new BITMAPINFOHEADER();
            m_BitMap.bmibColors = new RGBQUAD();
            return true;
        }

        virtual protected void Dispose(bool disposing)
        {
            CloseConnection();
        }

        // iniファイルから読み込んだデフォルト値を設定する
        private bool SetDefaut()
        {
            // 現在の設定を取り出す
            SendMessage(m_hHwnd, WM_CAP_GET_VIDEOFORMAT, Marshal.SizeOf(m_BitMap), out m_BitMap);

            // 変更する値を設定
            m_BitMap.bmiaHeader.bi_b_Width = m_Width;
            m_BitMap.bmiaHeader.bi_c_Height = m_Height;
            m_BitMap.bmiaHeader.bi_f_Compression_1 = (byte)m_Compression[0];
            m_BitMap.bmiaHeader.bi_f_Compression_2 = (byte)m_Compression[1];
            m_BitMap.bmiaHeader.bi_f_Compression_3 = (byte)m_Compression[2];
            m_BitMap.bmiaHeader.bi_f_Compression_4 = (byte)m_Compression[3];
            m_BitMap.bmiaHeader.bi_g_SizeImage = m_ImageSize;
            m_BitMap.bmiaHeader.bi_e_BitCount = m_BitCount;

            // 変更する
            int iRet = SendUpdateMessage(m_hHwnd, WM_CAP_SET_VIDEOFORMAT, Marshal.SizeOf(m_BitMap), ref m_BitMap);
            if (iRet != 1)
            {
                return false;
            }
            return true;
        }

        // 現在のWebカメラ設定を確認する
        private void DebugInfo()
        {
            if (m_DebugMode == 0)
            {
                // 何も出力/表示しない
                return;
            }

            // ビデオフォーマットの内容を出力
            int iRet = SendMessage(m_hHwnd, WM_CAP_GET_VIDEOFORMAT, Marshal.SizeOf(m_BitMap), out m_BitMap);
            string sMsgBox = "AllData" + '\n' + '\n';
            string sIntToStr;

            sIntToStr = Convert.ToString(m_BitMap.bmiaHeader.bi_b_Width);
            sMsgBox = sMsgBox + "Width:" + sIntToStr + '\n';

            sIntToStr = Convert.ToString(m_BitMap.bmiaHeader.bi_c_Height);
            sMsgBox = sMsgBox + "Height:" + sIntToStr + '\n';

            sIntToStr = Convert.ToString(m_BitMap.bmiaHeader.bi_d_Planes);
            sMsgBox = sMsgBox + "Planes:" + sIntToStr + '\n';

            sIntToStr = Convert.ToString(m_BitMap.bmiaHeader.bi_e_BitCount);
            sMsgBox = sMsgBox + "BitCount:" + sIntToStr + '\n';

            string sTmp = Encoding.ASCII.GetString(
                new byte[] {m_BitMap.bmiaHeader.bi_f_Compression_1,
                                m_BitMap.bmiaHeader.bi_f_Compression_2,
                                m_BitMap.bmiaHeader.bi_f_Compression_3,
                                m_BitMap.bmiaHeader.bi_f_Compression_4});

            sMsgBox = sMsgBox + "Compression:" + sTmp + '\n';

            sIntToStr = Convert.ToString(m_BitMap.bmiaHeader.bi_g_SizeImage);
            sMsgBox = sMsgBox + "SizeImage:" + sIntToStr + '\n';

            MessageBox.Show(sMsgBox);

            // 表示可能ダイアログの取得
            CAPDRIVERCAPS stDriverCaps = new CAPDRIVERCAPS();
            iRet = SendMessage(m_hHwnd, WM_CAP_DRIVER_GET_CAPS, Marshal.SizeOf(stDriverCaps), out stDriverCaps);
            if (iRet != 0)
            {
                if (stDriverCaps.e_HasDlgVideoDisplay == true)
                {
                    SendMessage(m_hHwnd, WM_CAP_DLG_VIDEODISPLAY, 0, 0);
                }

                if (stDriverCaps.c_HasDlgVideoSource == true)
                {
                    SendMessage(m_hHwnd, WM_CAP_DLG_VIDEOSOURCE, 0, 0);
                }

                if (stDriverCaps.d_HasDlgVideoFormat == true)
                {
                    SendMessage(m_hHwnd, WM_CAP_DLG_VIDEOFORMAT, 0, 0);
                }

            }
            SendMessage(m_hHwnd, WM_CAP_DLG_VIDEOCOMPRESSION, 0, 0);
        }
    }
}
