/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2017. All rights reserved.
 */
/**
 * @file    L1sFormat.cs
 * @brief   災害危機通報表示メッセージフォーマットクラス
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ShelterInfoSystem
{
    /**
     * @class L1sFormat
     * @brief 災害危機通報表示メッセージフォーマットクラス
     */
    public class L1sFormat
    {
        private Dictionary<string, string> m_conMap = new Dictionary<string,string>();

        /**
         * @brief フォーマットファイル読み込み
         */
        public void load()
        {
            //"./format"以下の".csv"ファイルをすべて取得する
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@"./format");
            System.IO.FileInfo[] files =
                di.GetFiles("*.csv", System.IO.SearchOption.AllDirectories);

            //ListBox1に結果を表示する
            foreach (System.IO.FileInfo f in files)
            {
                string filename = f.FullName;
                string csvText = "";
                try
                {
                    StreamReader sr = new StreamReader(
                        filename, Encoding.GetEncoding("Shift_JIS"));

                    csvText = sr.ReadToEnd();

                    sr.Close();
                }
                catch (Exception ex)
                {
                    // err
                    Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "L1sConvert", "load " + ex.Message);
                    return;
                }

                string fname = f.Name;
                fname = fname.Replace(".csv", "");

                csvText = csvText.Replace("\n", "");
                string[] readData = csvText.Split('\r');

                for (int i = 0; i < readData.Length; i++)
                {
                    string dat = readData[i];
                    dat.Replace('"', '\0');
                    int idx = dat.IndexOf(",");
                    if (idx > 0)
                    {
                        string key = dat.Substring(0, idx);
                        string val = dat.Substring(idx + 1);
                        try
                        {
                            if (fname == "mt00")
                            {
                                string test = fname + key;
                            }
                            m_conMap.Add(fname + key, val);
                        }
                        catch(Exception ex)
                        {
                            // 重複
                            Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "L1sConvert", "m_conMap.Add " + ex.Message);
                        }
                    }
                }
            }
        }

        /**
         * @brief 表示する文字列取得
         * @param 項目ID名
         * @param バイナリ内にセットされた数
         * @return 数か変換された名称
         */
        public string getName(string key, string code)
        {
            string codeorname = code;
            double lsb = 0.0;
            try
            {
                // %%key%% が Ev か Ev.t43_1 か
                if (key.IndexOf('.') <= 0)
                {
                    // Evのみ　数字そのまま
                    return codeorname;
                }
                else {
                    string[] par = key.Split('.');
                    key = par[1];
                }

                if (m_conMap.ContainsKey(key + code))
                {
                    
                    //  文字列変換
                    codeorname = m_conMap[key + code];

                    if (codeorname.IndexOf("%d") >= 0)
                    {
                        codeorname = codeorname.Replace("%d", code);
                    }
                }
                else
                {
                    // フォーマット
                    // LSB
                    if (m_conMap.ContainsKey(key  + "lsb"))
                    {
                        lsb = double.Parse(m_conMap[key+"lsb"]);
                        double para = double.Parse(code);
                        codeorname = (para * lsb).ToString();
                    }

                    // 単位
                    if (m_conMap.ContainsKey(key + "unit"))
                    {
                        codeorname += m_conMap[key + "unit"];
                    }

                    // %d
                    if (m_conMap.ContainsKey(key + "print"))
                    {
                        string moto = m_conMap[key + "print"];
                        if (moto.IndexOf("%d") >= 0)
                        {
                            codeorname = moto.Replace("%d", codeorname);
                        }
                    }

                    // 範囲
                    if (m_conMap.ContainsKey(key + "range"))
                    {
                        bool inRange = false;

                        string moto = m_conMap[key + "range"];
                        string[] range = moto.Split(',');
                        int i;
                        for (i = 0; i < range.Length; i++) 
                        {
                            if (range[i].IndexOf('-') >= 0)
                            {
                                // 範囲チェック
                                string[] field = range[i].Split('-');
                                if( int.Parse(code) >= int.Parse(field[0]) 
                                    && int.Parse(code) <= int.Parse(field[1]) )
                                {
                                    inRange = true;
                                    break;
                                }
                            }
                            else
                            {
                                // 数値単体チェック
                                if (code.Equals(range[i]))
                                {
                                    inRange = true;
                                    break;
                                }
                            }
                        }
                        if (!inRange)
                        {
                            // 範囲外であればerror(codeorname)を表示する
                            codeorname = "error(" + codeorname + ")";
                        }
                    }
                }
  
            }
            catch(Exception ex){
                // キーが存在しない
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "L1sConvert", "m_conMap.Add " + ex.Message);
            }
            return codeorname;
        }
    }
}
