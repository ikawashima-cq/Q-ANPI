using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using QAnpiCtrlLib.log;
using QAnpiCtrlLib.consts;

namespace ShelterInfoSystem.control
{
    class RFParamters
    {

        private static RFParamters mySelf = null;
        
        /// <summary>
        /// RFのパラメータを格納するハッシュテーブル
        /// </summary>
        private Hashtable rfParam = new Hashtable();


        
        /// <summary>
        /// RFのパラメータを格納するファイル
        /// </summary>
        private const String fileName = @"Q-ANPI_rfparam.cvs";

        /// <summary>
        /// 番号の位置
        /// </summary>
        private const int POS_INDEX_NUMBER = 0;
        /// <summary>
        /// パラメータ名の位置
        /// </summary>
        private const int POS_INDEX_PARANAMEJ = 1;
        /// <summary>
        /// 最大値の位置
        /// </summary>
        private const int POS_INDEX_MAXVAL = 3;
        /// <summary>
        /// 最小値の位置
        /// </summary>
        private const int POS_INDEX_MINVAL = 2;
        /// <summary>
        /// 単位位置
        /// </summary>
        private const int POS_INDEX_UNIT = 4;
        /// <summary>
        /// プロトタイプ端末でのデフォルト値の位置
        /// </summary>
        private const int POS_INDEX_DEFTERM = 5;
        /// <summary>
        /// 疑似地上局でのデフォルト値の位置
        /// </summary>
        private const int POS_INDEX_DEFSTA = 6;
        /// <summary>
        /// 変数名の位置
        /// </summary>
        private const int POS_INDEX_PARANAME = 7;
        /// <summary>
        /// 備考の位置
        /// </summary>
        private const int POS_INDEX_NOTE1 = 8;
        /// <summary>
        /// 欄外データの位置
        /// </summary>
        private const int POS_INDEX_NOTE2 = 9;

        /// <summary>
        /// RFのパラメータを格納するクラス
        /// </summary>
        class RFParam
        {
            /// <summary>
            /// 番号
            /// </summary>
            public int number;
            /// <summary>
            /// パラメータ名
            /// </summary>
            public String paraNameJ = "";
            /// <summary>
            /// 最大値
            /// </summary>
            public long maxVal;
            /// <summary>
            /// 最小値
            /// </summary>
            public long minVal;
            /// <summary>
            /// 単位
            /// </summary>
            public String unit = "";
            /// <summary>
            /// プロトタイプ端末でのデフォルト値
            /// </summary>
            public long defTerm;
            /// <summary>
            /// 疑似地上局でのデフォルト値
            /// </summary>
            public long defSta ;
            /// <summary>
            /// 変数名
            /// </summary>
            public String paraName = "";

        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        RFParamters()
        {
            System.IO.StreamReader sr = null;

            try
            {
                sr = new System.IO.StreamReader( fileName, new System.Text.UTF8Encoding(false));
            }
            catch(System.IO.FileNotFoundException e)
            {
                LogMng.AplLogInfo(e.Message);
                LogMng.AplLogInfo("RFParameter file無し、作成する");
                saveDefaultData();
                sr = new System.IO.StreamReader(fileName, new System.Text.UTF8Encoding(false));
            }
           
            //ファイルから読み込み、ハッシュ化する
            setDataFromFile(sr);
            sr.Close();
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
        }

        /// <summary>
        /// インスタンスの取得
        /// </summary>
        /// <returns></returns>
        public static RFParamters getInstance()
        {
            if (mySelf == null)
            {
                mySelf = new RFParamters();
            }
            return mySelf;
        }


        /// <summary>
        /// デフォルト値が入ったファイルを作成する
        /// </summary>
        private void saveDefaultData()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");


            //書き込むファイルを開く（UTF-8 BOM無し）
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                fileName, false, new System.Text.UTF8Encoding(false));

            sw.Write(DefaultRFParam.defaltData);
            sw.Flush();
            //ファイルを閉じる
            sw.Close();

            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

        }

        /// <summary>
        /// ファイルから読み出した値をpfParamに入れる
        /// </summary>
        /// <param name="sr">ファイルのStream</param>
        private void setDataFromFile(System.IO.StreamReader sr )
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            if(sr == null)
            {
                LogMng.AplLogError("引数がNULL");
                throw (new System.ArgumentNullException("sr"));
            }

            String dummyKey = "dummyKey";
            int i = 0;
            for (string oneLine = sr.ReadLine(); oneLine != null; oneLine = sr.ReadLine())
            {
                //fileのフォーマットは
                //番号,和名,最小値,最大値,単位,プロト端末のデフォルト値、疑似地上局のデフォルト値,変数名,備考1,備考2
                //これをRFParamに載せ替え、ハッシュテーブルへ格納
                RFParam oneData = new RFParam();
                string[] par = oneLine.Split(',');

                oneData.number = (int)convertStrToLong(par[POS_INDEX_NUMBER]);
                oneData.paraNameJ = par[POS_INDEX_PARANAMEJ];
                oneData.maxVal = convertStrToLong(par[POS_INDEX_MAXVAL]);
                oneData.minVal = convertStrToLong(par[POS_INDEX_MINVAL]);
                oneData.unit = par[POS_INDEX_UNIT];
                oneData.defTerm = convertStrToLong(par[POS_INDEX_DEFTERM]);
                oneData.defSta = convertStrToLong(par[POS_INDEX_DEFSTA]);
                oneData.paraName = par[POS_INDEX_PARANAME];

                String key = oneData.paraName;
                if (key.Length == 0)
                {
                    key = dummyKey + i.ToString();
                    i++;
                }
                rfParam.Add(key, oneData); 
            }
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            
        }

        /// <summary>
        /// 文字列をlogに変換
        /// </summary>
        /// <param name="data">変換する文字列</param>
        /// <returns>変換された値　変換に失敗した場合は0</returns>
        private long convertStrToLong(String data)
        {
            //これのLogで起動のLogが埋められるのでLogを出さない
            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            long ret;

            if(data == null)
            {
                return 0;
            }

            if (data.Length == 0)
            {
                return 0;
            }

            try
            {
                if(data.StartsWith("0x") == true)
                {
                    ret = Convert.ToInt64(data, 16);
                }
                else if(data.Equals("--"))
                {
                    ret = 0;
                }
                else if(data.Equals("TBD"))
                {
                    ret = 0;
                }
                else
                {
                    ret = Int64.Parse(data);
                }
            }
            catch (System.Exception e)
            {
                LogMng.AplLogDebug(e.Message);
                LogMng.AplLogDebug(e.StackTrace);
                LogMng.AplLogDebug(data);
                ret = 0;
            }
            //LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return ret;
        }

        /// <summary>
        /// 値を取得する
        /// </summary>
        /// <param name="paraName">取得するパラメータ名</param>
        /// <returns></returns>
        public int getValue(string paraName)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            if (rfParam.ContainsKey(paraName) == false)
            {
                throw (new System.ArgumentException("paraNameがありません"));
            }

            RFParam oneData = (RFParam)rfParam[paraName];

            int ret = 0;
            if (ObjectKeeper.mode == ObjectKeeper.MODE_TERM)
            {
                ret = (int)oneData.defTerm;
            }
            else if (ObjectKeeper.mode == ObjectKeeper.MODE_STATION)
            {
                ret = (int)oneData.defSta;
            }
            else
            {
                throw (new System.InvalidOperationException("動作モード設定前に呼ばれました"));
            }
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
            return ret;
        }

        /// <summary>
        /// 値の設定
        /// </summary>
        /// <param name="paraName">設定するパラメタ名</param>
        /// <param name="val">設定する値</param>
        public void setValue(String paraName, long val)
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");

            if(rfParam.ContainsKey(paraName) == false)
            {
                throw (new System.ArgumentException("paraNameがありません"));
            }

            RFParam oneData = (RFParam)rfParam[paraName];

            if (ObjectKeeper.mode == ObjectKeeper.MODE_TERM)
            {
                oneData.defTerm = val;
            }
            else if (ObjectKeeper.mode == ObjectKeeper.MODE_STATION)
            {
                oneData.defSta = val;
            }
            else
            {
                throw (new System.InvalidOperationException("動作モード設定前に呼ばれました"));
            }
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");
        }

        /// <summary>
        /// 現在のpfParamの値をファイルに保存する
        /// </summary>
        public void saveValToFile()
        {
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "開始");
            //書き込むファイルを開く（UTF-8 BOM無し）
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                fileName, false, new System.Text.UTF8Encoding(false));

            foreach (DictionaryEntry de in rfParam)
            {
                RFParam oneData = (RFParam)de.Value;

                //fileのフォーマットは
                //番号,和名,最小値,最大値,単位,プロト端末のデフォルト値、疑似地上局のデフォルト値,変数名,備考1,備考2
                StringBuilder sb = new StringBuilder();
                sb.Append((oneData.number == 0) ? "" : oneData.number.ToString());
                sb.Append(",");
                sb.Append(oneData.paraNameJ);
                sb.Append(",");
                sb.Append("0x" + oneData.minVal.ToString("X"));
                sb.Append(",");
                sb.Append("0x" + oneData.maxVal.ToString("X"));
                sb.Append(",");
                sb.Append(oneData.unit);
                sb.Append(",");
                sb.Append("0x" + oneData.defTerm.ToString("X"));
                sb.Append(",");
                sb.Append("0x" + oneData.defSta.ToString("X"));
                sb.Append(",");
                sb.Append(oneData.paraName);

                sw.WriteLine(sb.ToString());
            }
            sw.Flush();
            sw.Close();
            LogMng.AplLogDebug(System.Reflection.MethodBase.GetCurrentMethod().Name + "終了");

        }



    }
}
