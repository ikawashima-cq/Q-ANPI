using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ShelterInfoSystem
{
    public class SubGHzConfig
    {
        public string COMPort;   
        public string Src_ID;     
        public string Channel;
        public string Dst_ID;
        public string Power;
        public string RF_Baud;
        public string CS_Mode;
        public string Retry_Count;
        public string Baudrate;
        public string Enable;
        public string RtsEnable;

        private const string m_fileName = @"./subghz.xml";

        // 読み込み
        public void Load()
        {
            XmlDocument lineXmlDoc = new XmlDocument();

            try
            {
                // ファイル存在チェック
                if (!System.IO.File.Exists(m_fileName))
                {
                    Program.m_thLog.PutErrorLog("SubGHzConfig", "Not Found : subghz.xml", "");
                    return;
                }

                // XMLファイル読込
                lineXmlDoc.Load(m_fileName);
                XmlNode rootNode = lineXmlDoc.DocumentElement;

                if (rootNode.HasChildNodes)
                {
                    for (int nRootIdx = 0; nRootIdx < rootNode.ChildNodes.Count; nRootIdx++)
                    {
                        XmlNode itemNode = rootNode.ChildNodes[nRootIdx];
                        switch (itemNode.LocalName)
                        {
                            case "COMPort":
                                COMPort = itemNode.InnerText;
                                break; 
                            case "Src_ID":
                                Src_ID = itemNode.InnerText;
                                break;
                            case "Channel":
                                Channel = itemNode.InnerText;
                                break;
                            case "Dst_ID":
                                Dst_ID = itemNode.InnerText;
                                break;
                            case "Power":
                                Power = itemNode.InnerText;
                                break;
                            case "RF_Baud":
                                RF_Baud = itemNode.InnerText;
                                break;
                            case "CS_Mode":
                                CS_Mode = itemNode.InnerText;
                                break;
                            case "Retry_Count":
                                Retry_Count = itemNode.InnerText;
                                break;
                            case "Baudrate":
                                Baudrate = itemNode.InnerText;
                                break;
                            case "Enable":
                                Enable = itemNode.InnerText;
                                break;
                            case "RtsEnable":
                                RtsEnable = itemNode.InnerText;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    Program.m_thLog.PutErrorLog("SubGHzConfig", "Not Found : subghz.xml", "");
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog("SubGHzConfig", "Faild : Load subghz.xml File.", ex.ToString());
            }
        }

        // 保存
        public void Save()
        {
            if (COMPort == null || (COMPort != null && COMPort.Length == 0)) return;
            try
            {               
                System.IO.StreamWriter sw = new System.IO.StreamWriter(m_fileName, false, new System.Text.UTF8Encoding(false));
                (new System.Xml.Serialization.XmlSerializer(typeof(SubGHzConfig))).Serialize(sw, this);
                sw.Close();
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog("SubGHzConfig", "Faild : Save subghz.xml File.", ex.ToString());
            }
        }
    }
}
