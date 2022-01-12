using System;

namespace ShelterInfoSystem
{
    public class HttpThreadBase
    {
        // スレッド
        private System.Threading.Thread m_Thread = null;
        private bool m_CommFlag = false;
        private int m_nRetryCount = 0;
        private int m_nHttpTimeOut = 15000;
        private string m_sUrl = "";

        //--------------------
        // 送信状態
        //--------------------
        public void SetHttpStatust(bool val)
        {
            m_CommFlag = val;
        }

        public bool GetHttpStatus()
        {
            return m_CommFlag;
        }

        //--------------------
        // リトライ回数
        //--------------------
        public void SetRetryCount(int val)
        {
            m_nRetryCount = val;
        }
        public int GetRetryCount()
        {
            return m_nRetryCount;
        }

        //--------------------
        // HTTP通信タイムアウト
        //--------------------
        public void SetHttpTimeOut(int val)
        {
            m_nHttpTimeOut = val;
        }
        public int GeHttpTimeOut()
        {
            return m_nHttpTimeOut;
        }

        //--------------------
        // URL
        //--------------------
        public void SetHttpUrl(string val)
        {
            m_sUrl = val;
        }
        public string GetHttpUrl()
        {
            return m_sUrl;
        }
        //--------------------
        // HTTP 
        //--------------------
        public void HttpProc(string val)
        {
            // スレッド作成
            m_Thread = new System.Threading.Thread(httpfunc);

            // スレッドを開始
            m_Thread.Start(val);

        }

        private void httpfunc(object val)
        {
            SetHttpStatust(true);

            string inVal = (string)val;

            int nCnt = 0;
            int nCode;
            string sErr;
            string sDmy;
            while (true)
            {
                Boolean nRet = HttpReqRes(inVal, out nCode, out sErr, out sDmy);

                if (nRet == false)
                {
                    //                if (!(nCode > 300 && nCode <= 502) )
                    if (nCode == 503 || nCode < 100)
                    {
                        //繰り返し
                        nCnt++;
                        if (nCnt < m_nRetryCount)
                        {
                            continue;
                        }
                    }
                }

                OnEvent(nCode, sErr, sDmy);

                break;
            }

            SetHttpStatust(false);
            m_Thread = null;
        }

        public virtual Boolean HttpReqRes(string inData, out int code, out string errMsg, out string outVal)
        {
            code = 0;
            errMsg = "";
            outVal = "";
            return true;
        }


        // 呼び出し元用デリゲート
        public delegate void EventMessageDelegate(object sender, int code, string msg, string outVal);

        // イベント定義
        public event EventMessageDelegate MessageEvent;

        // イベント呼び出し
        public virtual void OnEvent(int code, string msg, string outVal)
        {
            if (MessageEvent != null)
            {
                // 実行
                MessageEvent(this, code, msg, outVal);
            }
        }

    }
}

