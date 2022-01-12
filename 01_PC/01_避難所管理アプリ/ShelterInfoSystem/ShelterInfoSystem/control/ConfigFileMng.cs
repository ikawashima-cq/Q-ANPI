using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QAnpiCtrlLib.log;
using QAnpiCtrlLib.consts;

namespace ShelterInfoSystem.control
{
    /// <summary>
    /// 制御ツールコンフィグ情報取得更新クラス
    /// </summary>
    public class ConfigFileMng
    {
        /// <summary>
        /// 個別データ格納クラス
        /// </summary>
        public class Attribute
        {
            /// <summary>
            /// config fileの属性名
            /// </summary>
            public string Name;
            
            /// <summary>
            /// config fileの属性値
            /// </summary>
            public string Value;
        }

        /// <summary>
        /// データリストの格納クラス
        /// </summary>
        public class CtrlToolConfiguration
        {
            /// <summary>
            /// config fileの属性リスト
            /// </summary>
            public List<Attribute> AttributeList = new List<Attribute>();
        }

        /// <summary>
        /// データリストの実態
        /// </summary>
        static CtrlToolConfiguration cfgFileData = null; 

        /// <summary>
        /// config fileからデータを読み出す
        /// </summary>
        public static void load()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            
            if(cfgFileData != null)
            {
                LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
                return;
            }

            //読み出し元ののファイル名
            string fileName = @"Q-ANPI_config.xml";

            //XmlSerializerオブジェクトを作成
            //オブジェクトの型を指定する
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CtrlToolConfiguration));
            //読み込むファイルを開く
            System.IO.StreamReader sr = null;

            try
            {
                sr = new System.IO.StreamReader( fileName, new System.Text.UTF8Encoding(false));
            }
            catch(System.IO.FileNotFoundException e)
            {
                LogMng.AplLogInfo(e.Message);
                LogMng.AplLogInfo("config file無し、作成する");
                cfgFileData = new CtrlToolConfiguration();
                save();
                sr = new System.IO.StreamReader(fileName, new System.Text.UTF8Encoding(false));
            }

           
            //XMLファイルから読み込み、逆シリアル化する
            cfgFileData = (CtrlToolConfiguration)serializer.Deserialize(sr);
            //ファイルを閉じる
            sr.Close();
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

        }

        /// <summary>
        /// config fileにデータを書き込む
        /// </summary>
        public static void save()
        {

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            //保存先のファイル名
            string fileName = @"Q-ANPI_config.xml";

            //XmlSerializerオブジェクトを作成
            //オブジェクトの型を指定する
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CtrlToolConfiguration));
            //書き込むファイルを開く（UTF-8 BOM無し）
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                fileName, false, new System.Text.UTF8Encoding(false));
            //シリアル化し、XMLファイルに保存する
            serializer.Serialize(sw, cfgFileData);
            //ファイルを閉じる
            sw.Close();

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
        }


        /// <summary>
        /// config fileから与えられたkeyStringに対応する値を読み出す
        /// </summary>
        /// <param name="keyString">config fileに記載されてる属性名</param>
        /// <returns>属性値</returns>
        public static String getValue(String keyString)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            foreach (Attribute d in cfgFileData.AttributeList)
            {
                if(d.Name.Equals(keyString))
                {
                    LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
                    return d.Value;
                }
            }
            throw (new System.ArgumentException("keyStringに対応するName属性がありません"));
        }

        /// <summary>
        /// config fileに与えられた属性名、属性値の組を書き込む
        /// </summary>
        /// <param name="keyString">config fileに記載する属性名</param>
        /// <param name="Value">config fileに記載する属性値</param>
        public static void setValue(String keyString, String Value)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            Boolean found = false;  
            //keyStringがconfigにあるか確認
            //あれば更新
            foreach (Attribute d in cfgFileData.AttributeList)
            {
                if (d.Name.Equals(keyString))
                {
                    found = true;
                    d.Value = Value;
                }
            }

            //keyStringが無かったので追加
            if(found == false)
            {
                Attribute newdata = new Attribute();
                newdata.Name = keyString;
                newdata.Value = Value;
                cfgFileData.AttributeList.Add(newdata);
            }

            //コンフィグファイルを更新
            save();

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            
        }
    }
}
