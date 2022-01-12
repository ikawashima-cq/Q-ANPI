using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ShelterInfoSystem
{
    class QzssConfig
    {
        /* // 現在未使用
        public string PostURL;      // QZSSサーバデータ送信URL
        public string GetURL;       // QZSSサーバデータ取得URL
        public int RetryCount;      // QZSSサーバ通信エラー時のリトライ回数
        public int TimeOut;         // QZSSサーバ通信応答無し時のタイムアウト時間(ms)   
        public int CycleFeature;    // QZSSサーバからのフィーチャーフォン個人情報収集間隔
        public string LocalURL;     // ローカルWebの個人安否登録のURL

        public void Load()
        {
            XmlDocument lineXmlDoc = new XmlDocument();

            try
            {
                // XMLファイル読込
                lineXmlDoc.Load("./qzss.xml");

                XmlNode rootNode = lineXmlDoc.DocumentElement;

                // ルートノードが存在する
                if (rootNode.HasChildNodes)
                {
                    // SettingInfoノードを解析
                    for (int nRootIdx = 0; nRootIdx < rootNode.ChildNodes.Count; nRootIdx++)
                    {
                        // SettingInfoノードの子ノードを取得
                        XmlNode itemNode = rootNode.ChildNodes[nRootIdx];

                        // ノード名により設定を読込
                        switch (itemNode.LocalName)
                        {
                            case "PostURL":
                                PostURL = itemNode.InnerText;
                                break;
                            case "GetURL":
                                GetURL = itemNode.InnerText;
                                break;
                            case "RetryCount":
                                RetryCount = int.Parse(itemNode.InnerText);
                                break;
                            case "TimeOut":
                                TimeOut = int.Parse(itemNode.InnerText);
                                break;
                            case "CycleFeature":
                                CycleFeature = int.Parse(itemNode.InnerText);
                                break;
                            case "LocalURL":
                                LocalURL = itemNode.InnerText;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    Program.m_thLog.PutErrorLog("QzssConfig", "Not Found : qzss.Xml", "");
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog("QzssConfig", "Faild : Load qzss.Xml File.", ex.ToString());
            }
        }
         * */
    }
}
