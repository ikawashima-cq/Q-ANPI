/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2014-2015. All rights reserved.
 */
using System;
using System.Collections.Generic;
using QAnpiCtrlLib.log;

namespace QAnpiCtrlLib.utils
{
    /// <summary>
    /// CSV管理クラス
    /// </summary>
    public class CsvMng
    {
        /// <summary>
        /// CSV読出しデータ
        /// </summary>
        public List<List<String>> LoadData = null;

        /// <summary>
        /// CSVファイルパス
        /// </summary>
        public string filePath { get; set; }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CsvMng()
        {
            this.LoadData = new List<List<String>>();
        }


        /// <summary>
        /// CSV読出し
        /// </summary>
        /// <param name="path">CSVファイルパス</param>
        /// <returns>
        /// 処理結果
        /// true：成功、false：失敗
        /// </returns>
        public Boolean Load(String path)
        {
            Boolean ret = false;
            this.filePath = path;

            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.IO.StreamReader streamCsv;

                    // CSVファイル読出し
                    using (streamCsv = new System.IO.StreamReader(this.filePath))
                    {
                        // 最終行まで読出し
                        while (streamCsv.Peek() >= 0)
                        {
                            List<String> lineList = new List<String>();

                            // 一行読み出し
                            string lineStr = streamCsv.ReadLine();
                            // 分割
                            String[] columns = lineStr.Split(',');

                            // 一行Listに追加
                            for (int i = 0; i < columns.Length; i++)
                            {
                                lineList.Add(columns[i]);
                            }

                            // 全行Listに追加
                            this.LoadData.Add(lineList);
                        }

                        ret = true;
                    }
                }
                catch (Exception e)
                {
                    LogMng.AplLogDebug(e.Message);
                    LogMng.AplLogDebug(e.StackTrace);
                }
            }

            return ret;
        }

        /// <summary>
        /// すべての項目が数字変換可能かチェックする
        /// </summary>
        /// <returns>
        /// 処理結果
        /// RET_OK：全ての項目が数字変換可能
        /// RET_NG：いずれかの項目が数字変換不可能
        /// </returns>
        public int CheckAllInteger()
        {
            int result = consts.CommonConst.RET_OK;

            int lineCount = 0;
            while (lineCount < LoadData.Count)
            {
                List<string> line = LoadData[lineCount];

                int colCount = 0;
                while (colCount < line.Count)
                {
                    int colInt;
                    bool rslt = int.TryParse(line[colCount], out colInt);
                    if (rslt)
                    {
                        colCount++;
                    }
                    else
                    {
                        result = consts.CommonConst.RET_NG;
                        LogMng.AplLogDebug("数値変換NG " + lineCount + "行目の" + colCount + "列目：" + line[colCount]);
                        break;
                    }
                }

                lineCount++;
            }

            return result;
        }

        /// <summary>
        /// 読み出したデータの周波数/PNの数値範囲チェック
        /// ※周波数/PNのCSVデータ専用
        /// </summary>
        /// <returns>
        /// 処理結果
        /// RET_OK：数値範囲チェックOK
        /// RET_NG：数値範囲チェックNG
        /// </returns>
        public int CheckFrqPn()
        {
            int result = consts.CommonConst.RET_OK;

            // 数値チェック
            if (CheckAllInteger() != consts.CommonConst.RET_OK)
            {
                return consts.CommonConst.RET_NG;
            }

            int lineCount = 0;
            foreach (List<string> line in LoadData)
            {
                lineCount++;
                int frq = int.Parse(line[0]);
                if (frq < consts.CommonConst.FREQ_MIN || consts.CommonConst.FREQ_MAX < frq)
                {
                    result = consts.CommonConst.RET_NG;
                    LogMng.AplLogDebug(lineCount + "行目NG：周波数=" + frq);
                    break;
                }

                int pn = int.Parse(line[1]);
                if (pn < consts.CommonConst.PN_MIN || consts.CommonConst.PN_MAX < pn)
                {
                    result = consts.CommonConst.RET_NG;
                    LogMng.AplLogDebug(lineCount + "行目NG：PN=" + frq);
                    break;
                }
            }

            return result;
        }


        // end of class
    }
}
