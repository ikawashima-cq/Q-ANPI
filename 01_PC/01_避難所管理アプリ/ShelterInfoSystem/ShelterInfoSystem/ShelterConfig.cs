using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ShelterInfoSystem
{
    class ShelterConfig
    {
        /* // 現在未使用
        public string TermId;       // 端末ID（避難所ID)
        public string GID;          // GID
        public string ShelterName;  // 避難所名
        public string Lat;          // 緯度
        public string Lon;          // 経度
        */
        public string IPAddress;    // TCP通信用　IPアドレス
        public string PortNo;       // TCP通信用　ポート番号
        public int TimeSyncCycle;   // 時刻同期通信送信間隔(秒)
        public int UARTAckTimeout;  // PC-ドングル間Ack応答タイムアウト時間(ms)
        private int DEFAULT_SYNC_TIME = 60;                 // デフォルト時刻同期間隔時間(分)
        private int DEFAULT_UART_ACK_TIMEOUT = 4000;        // デフォルトPC-ドングル間Ack応答タイムアウト時間(ms)

        public void Load()
        {
            XmlDocument lineXmlDoc = new XmlDocument();

            // 各値デフォルト設定
            IPAddress = "0.0.0.0";
            PortNo = "50000";
            TimeSyncCycle = DEFAULT_SYNC_TIME * 60;
            UARTAckTimeout = DEFAULT_UART_ACK_TIMEOUT;

            try
            {
                // XMLファイル存在チェック
                if (!System.IO.File.Exists("./shelter.xml"))
                {
                    Program.m_thLog.PutErrorLog("QzssConfig", "Not Found : shelter.Xml", "");
                    return;
                }

                // XMLファイル読込
                lineXmlDoc.Load("./shelter.xml");

                XmlNode rootNode = lineXmlDoc.DocumentElement;

                // ルートノードが存在する
                if (rootNode.HasChildNodes)
                {
                    // ShelterInfo解析
                    for (int nRootIdx = 0; nRootIdx < rootNode.ChildNodes.Count; nRootIdx++)
                    {
                        // ShelterInfoノードの子ノードを取得
                        XmlNode itemNode = rootNode.ChildNodes[nRootIdx];

                        // ノード名により設定を読込
                        switch (itemNode.LocalName)
                        {
                            /* // 現在未使用
                            case "TermId":
                                TermId = itemNode.InnerText;
                                break;
                            case "GID":
                                GID = itemNode.InnerText;
                                break;
                            case "ShelterName":
                                ShelterName = itemNode.InnerText;
                                break;
                            case "Lat":
                                Lat = itemNode.InnerText;
                                break;
                            case "Lon":
                                Lon = itemNode.InnerText;
                                break;
                             * */
                            case "IPAddress":
                                IPAddress = itemNode.InnerText;
                                break;
                            case "PortNo":
                                PortNo = itemNode.InnerText;
                                break;
                            case "TimeSync":
                                TimeSyncCycle = int.Parse(itemNode.InnerText) * 60;
                                break;
                            case "UARTAckTimeout":
                                UARTAckTimeout = int.Parse(itemNode.InnerText);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    Program.m_thLog.PutErrorLog("QzssConfig", "Not Found : shelter.Xml", "");
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog("QzssConfig", "Faild : Load shelter.Xml File.", ex.ToString());
            }
        }
    }
}
