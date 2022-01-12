using System;
using System.Net;
using System.IO;
using System.Diagnostics;       // Debug.WriteLine用



namespace ShelterInfoSystem
{
    public class HttpPostThread : HttpThreadBase
    {

        public override Boolean HttpReqRes(string inData, out int code, out string errMsg, out string outVal)
        {

            code = 0;
            errMsg = "";
            outVal = "";

            Boolean bRet = true;
            byte[] postBytes = System.Text.Encoding.ASCII.GetBytes(inData);

            string strUrl = GetHttpUrl();

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strUrl);

            int tmout = GeHttpTimeOut();
            if (tmout <= 0)
            {
                tmout = 10;
            }
            req.Timeout = tmout * 1000;

            req.Method = "POST";
            req.ContentLength = postBytes.Length;

            System.Net.HttpWebResponse res = null;
            System.IO.Stream reqStream = null;

            try
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "HttpPostThread", "Send Data : " + inData);

                //データをPOST送信するためのStreamを取得
                reqStream = req.GetRequestStream();
                //送信するデータを書き込む
                reqStream.Write(postBytes, 0, postBytes.Length);
                reqStream.Close();

                res = (HttpWebResponse)req.GetResponse();
                Stream s = res.GetResponseStream();
                StreamReader sr = new StreamReader(s);

                outVal = sr.ReadToEnd();
                Debug.WriteLine(outVal);

                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "HttpPostThread", "Recv Data : " + outVal);

            }
            catch (System.Net.WebException ex)
            {
                if (reqStream != null)
                {
                    reqStream.Close();
                }

                bRet = false;
                Debug.WriteLine(ex.Message);

                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    // タイムアウト
                    // エラーコード
                    code = 10;
                    //errMsg = "通信タイムアウト";
                    errMsg = ex.Message;

                }
                else
                {

                    System.Net.HttpWebResponse errres =
                               (System.Net.HttpWebResponse)ex.Response;

                    if (errres != null)
                    {
                        // エラーコード
                        code = (int)errres.StatusCode;
                        //errMsg = errres.StatusDescription;
                        errMsg = ex.Message;
                    }
                }
                string sLog = String.Format("ERR CODE {0}, Err {1}, Data {2} ", code, errMsg, inData);
                Program.m_thLog.PutErrorLog("HttpPostThread", "Post Faild.", sLog);
            }
            finally
            {
                req.Abort();            // インターネットへの要求をキャンセル（これをしないと、エラー発生後すべてタイムアウトとなってしまう）
                if (res != null)
                    res.Close();
            }

            return bRet;
        }

    }
}

