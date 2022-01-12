/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2017. All rights reserved.
 */
/**
 * @file    TcpConfig.cs
 * @brief   Tcp通信設定ファイル定義
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ShelterInfoSystem
{
    /**
     * @class: TcpConfig
     * @brief: Tcp通信設定(IPアドレス)定義ファイル
     */
    public class TcpConfig
    {
        /**
         * @brief: IPアドレス定義
         */
        public string IP;

        private const string m_fileName = @"./tcp.xml";

        /**
         * @brief: XMLファイル読み込み処理
         */
        public void Load()
        {
            XmlDocument lineXmlDoc = new XmlDocument();

            try
            {
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
                            case "IP":
                                IP = itemNode.InnerText;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    Program.m_thLog.PutErrorLog("TcpConfig", "Not Found : tcp.Xml", "");
                }
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog("TcpConfig", "Faild : Load tcp.Xml File.", ex.ToString());
            }
        }

        /**
         * @brief: XMLファイル保存処理
         */
        public bool Save()
        {
            if (IP == null || (IP != null && IP.Length == 0)) return false;
            try
            {               
                System.IO.StreamWriter sw = new System.IO.StreamWriter(m_fileName, false, new System.Text.UTF8Encoding(false));
                (new System.Xml.Serialization.XmlSerializer(typeof(TcpConfig))).Serialize(sw, this);
                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutErrorLog("TcpConfig", "Faild : Save tcp.Xml File.", ex.ToString());
                return false;
            }
        }

    }
}
