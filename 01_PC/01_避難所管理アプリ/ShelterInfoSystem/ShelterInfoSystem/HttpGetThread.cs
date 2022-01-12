using System;
using System.Net;
using System.IO;
using System.Diagnostics;        // Debug.WriteLine用


namespace ShelterInfoSystem
{
    public class HttpGetThread : HttpThreadBase
    {

        public override Boolean HttpReqRes(string inData, out int code, out string errMsg, out string outVal)
        {

            code = 0;
            errMsg = "";
            outVal = "";
            Boolean bRet = true;

            string strUrl = GetHttpUrl();

            if (inData.Length > 0)
            {
                strUrl = strUrl + "?" + inData;
            }

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strUrl);

            int tmout = GeHttpTimeOut();
            if (tmout <= 0)
            {
                tmout = 10;
            }
            req.Timeout = tmout * 1000;
            //        req.Timeout = GeHttpTimeOut()*1000; 
            req.Method = "GET";

            System.Net.HttpWebResponse res = null;

            try
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "HttpPostThread", "Send Data : " + inData);

                res = (HttpWebResponse)req.GetResponse();
                Stream s = res.GetResponseStream();
                StreamReader sr = new StreamReader(s);

                string content = sr.ReadToEnd();
                outVal = content;

                Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "HttpGetThread", "Recv Data : " + outVal);
            }
            catch (System.Net.WebException ex)
            {
                bRet = false;
                Debug.WriteLine(ex.Message);

                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    // タイムアウト
                    // エラーコード
                    code = 10;
                    errMsg = "通信タイムアウト";
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
                        errMsg = errres.StatusDescription;
                    }
                }
                string sLog = String.Format("ERR CODE {0}, Err {1}, Data {2} ", code, errMsg, inData);
                Program.m_thLog.PutErrorLog("HttpGetThread", "Get Faild.", sLog);
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


