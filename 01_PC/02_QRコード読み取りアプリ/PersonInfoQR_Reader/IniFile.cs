using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace EncryptedQRCodeReader {
    public class IniFile {
        // iniファイル名
        private string m_IniFileName = "./EncryptedQRCodeReader.ini";

        // メッセージボックス出力用文字列
        private string m_InitErrorTitle = "初期化エラー";
        private string m_IniSection = "セクション名：[";
        private string m_IniKey = "キー名：";

        private string m_IniNotFound = "設定ファイルが見つかりません  ";
        private string m_IniErrMsg1 = "設定ファイル(";
        private string m_IniErrMsg2 = ")の情報が異常です。\n\n";


        public IniFile() {
        }

        // 初期化
        public bool Initilize()
        {
            // ファイルが存在しないときはエラーとする。
            if (System.IO.File.Exists(m_IniFileName) == false)
            {
                MessageBox.Show(m_IniNotFound + m_IniFileName, m_InitErrorTitle);
                return false;
            }
            return true;
        }

        // iniファイル内容異常時のメッセージボックス出力
        public void MsgBox(string sSec, string sKey, string sAdditionalInfo = "")
        {
            string sContents = "";
            sContents += m_IniErrMsg1 + m_IniFileName + m_IniErrMsg2;
            sContents += m_IniSection + sSec + "]\n";
            sContents += m_IniKey + sKey;
            if (sAdditionalInfo.Length != 0)
            {
                sContents += "\n" + sAdditionalInfo;
            }

            MessageBox.Show(sContents, m_InitErrorTitle);
        }

        public string GetString(string secName, string keyName)
        {
            uint length;
            string value;
            System.Text.StringBuilder strSb;

            strSb = new System.Text.StringBuilder(1024);
            length = GetPrivateProfileString(secName, keyName, "", strSb, (uint)strSb.Capacity, this.m_IniFileName);
            value = strSb.ToString();
            return value;
        }

        public int GetInteger(string secName, string keyName)
        {
            string value = "";
            int num = 0;
            value = this.GetString(secName, keyName);

            //空文字以外なら変換
            if (0 != value.CompareTo(""))
            {
                try
                {
                    num = int.Parse(value);
                }
                catch (Exception e)
                {
                    // 変換に失敗したら0を返す
                    string errMsg = e.Message;
                    num = 0;
                }
            }

            return num;
        }

        // ini ファイルの読み込み用の関数(GetPrivateProfileString)の宣言部分
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern uint GetPrivateProfileString(
                                       string lpApplicationName,
                                       string lpEntryName,
                                       string lpDefault,
                                       System.Text.StringBuilder lpReturnedString,
                                       uint nSize, string lpFileName);
    }
}
