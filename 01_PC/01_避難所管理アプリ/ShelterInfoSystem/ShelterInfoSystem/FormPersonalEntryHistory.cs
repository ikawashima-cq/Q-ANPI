using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShelterInfoSystem
{
    public partial class FormPersonalEntryHistory : Form
    {

        public FormPersonalEntryHistory()
        {
            InitializeComponent();
/*
            // 行データの値を設定
            string[] strItems = new string[14];

            List<DbAccess.PersonLog> logList = new List<DbAccess.PersonLog>();
            // ログリスト取得
            Program.m_objDbAccess.GetPersonLogList(ref logList);

            // 行データの追加
            int nIdx;
            int nItemCnt = logList.Count;
            for (nIdx = 0; nIdx < nItemCnt; nIdx++)
            {
                // person_infoをstring配列に変換
                strItems = ConvPersonLogToStringList(logList[nIdx]);

                listData.Items.Add(new ListViewItem(strItems));
            }
*/
            // テストデータのためコメント化
            //int nItemCnt = m_testData1.Length / strItems.Length;
            //for (nIdx = 0; nIdx < nItemCnt; nIdx++)
            //{
            //    if ((m_testData1.Length / strItems.Length) > nIdx)
            //    {
            //        int nColIdx;
            //        for (nColIdx = 0; nColIdx < strItems.Length; nColIdx++)
            //        {
            //            strItems[nColIdx] = m_testData1[nIdx, nColIdx];
            //        }
            //    }
            //    else
            //    {
            //        int nColIdx;
            //        for (nColIdx = 0; nColIdx < strItems.Length; nColIdx++)
            //        {
            //            strItems[nColIdx] = "";
            //        }
            //    }
            //    listData.Items.Add(new ListViewItem(strItems));
            //}
        }

        public void init(DbAccess.PersonInfo info)
        {

            // 行データの値を設定
            string[] strItems = new string[14];

            List<DbAccess.PersonLog> logList = new List<DbAccess.PersonLog>();
            // ログリスト取得
            Program.m_objDbAccess.GetPersonLogList(info.id, info.name, info.sid, ref logList);

            // 行データの追加
            int nIdx;
            int nItemCnt = logList.Count;
            for (nIdx = 0; nIdx < nItemCnt; nIdx++)
            {
                // person_infoをstring配列に変換
                strItems = ConvPersonLogToStringList(logList[nIdx]);

                listData.Items.Add(new ListViewItem(strItems));
            }
        }


        private string[] ConvPersonLogToStringList(DbAccess.PersonLog log)
        {
            string[] strItems = new string[14];
            int nIdx;

            // 初期化
            for (nIdx = 0; nIdx < strItems.Length; nIdx++)
            {
                strItems[nIdx] = "";
            }

            int nStrListIdx = 0;
            // No
            strItems[nStrListIdx] = "0";
            nStrListIdx++;

            // 登録日時
            strItems[nStrListIdx] = log.update_datetime;
            nStrListIdx++;

            // 氏名
            strItems[nStrListIdx] = log.name;
            nStrListIdx++;

            // 電話番号
            strItems[nStrListIdx] = log.id;
            nStrListIdx++;

            // 入/退所  0:入所 1:退所 2:在宅
            strItems[nStrListIdx] = Program.ConvSel02(log.sel02);
            nStrListIdx++;

            // 2018/07/20 
            // 年齢 -> 生年月日
            //strItems[nStrListIdx] = log.num01;
            strItems[nStrListIdx] = log.txt02;
            nStrListIdx++;

            // 性別       0:男性 1:女性
            strItems[nStrListIdx] = Program.ConvSel01(log.sel01);
            nStrListIdx++;

            // 住所
            strItems[nStrListIdx] = log.txt01;
            nStrListIdx++;

            // 怪我       0:無 1:有 2:未使用 3:未選択
            strItems[nStrListIdx] = Program.ConvSel04(log.sel04);
            nStrListIdx++;

            // 公表       0:しない 1:する
            strItems[nStrListIdx] = Program.ConvSel03(log.sel03);
            nStrListIdx++;

            // 2016/04/18 救護を削除：これ以降を前詰
            //// 救護       2:未選択 0:否  1:要
            //strItems[nStrListIdx] = Program.ConvSel05(log.sel05);
            //nStrListIdx++;

            // 介護       0:否 1:要 2:未選択
            strItems[nStrListIdx] = Program.ConvSel05(log.sel05);
            nStrListIdx++;

            // 障がい    0:無 1:有 2:未選択
            strItems[nStrListIdx] = Program.ConvSel06(log.sel06);
            nStrListIdx++;

            // 妊産婦    0:いいえ 1:はい 2:未選択
            strItems[nStrListIdx] = Program.ConvSel07(log.sel07);
            nStrListIdx++;

            // 避難所内外    0：内 1:外
            strItems[nStrListIdx] = Program.ConvSel08(log.sel08);
            nStrListIdx++;


            return strItems;
        }

        private void listData_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // 描画はOSに任せる
            e.DrawDefault = true;
        }

        private void listData_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            StringFormat stringFormat = new StringFormat();
            Rectangle objStrRect = e.Bounds;

            switch (e.ColumnIndex)
            {
                default:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    objStrRect = e.Bounds;
                    break;
                //case :
                //    stringFormat.Alignment = StringAlignment.Near;
                //    stringFormat.LineAlignment = StringAlignment.Center;
                //    objStrRect = new Rectangle(e.Bounds.X + FormMain.LIST_PADDING, e.Bounds.Y, e.Bounds.Width - FormMain.LIST_PADDING, e.Bounds.Height);
                //    break;
                //case :
                //    stringFormat.Alignment = StringAlignment.Far;
                //    stringFormat.LineAlignment = StringAlignment.Center;
                //    objStrRect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - FormMain.LIST_PADDING, e.Bounds.Height);
                //    break;
            }

            // 選択されているアイテムの描画
            //if ((e.ItemState & ListViewItemStates.Selected)
            //                == ListViewItemStates.Selected)
            if (e.Item.Selected) 
            {
                e.Graphics.FillRectangle(
                                SystemBrushes.Highlight,
                                e.Bounds);
                e.Graphics.DrawString(
                                e.SubItem.Text,
                                e.Item.Font,
                                SystemBrushes.HighlightText,
                                objStrRect,
                                stringFormat);
                return;
            }

            // 奇数行は背景色を変更して描画
            if ((e.Item.Index % 2) != 0)
            {
//                SolidBrush brushes = new SolidBrush(Color.LightBlue);
                SolidBrush brushes = new SolidBrush(Color.AliceBlue);
//                SolidBrush brushes = new SolidBrush(Color.Azure);

                e.Graphics.FillRectangle(
                                brushes,
                                e.Bounds);

                e.Graphics.DrawString(
                                e.SubItem.Text,
                                e.Item.Font,
                                SystemBrushes.WindowText,
                                e.Bounds,
                                stringFormat);
                return;
            }
            else
            {
                e.Graphics.FillRectangle(
                                SystemBrushes.Window,
                                e.Bounds);
                e.Graphics.DrawString(
                                e.SubItem.Text,
                                e.Item.Font,
                                SystemBrushes.WindowText,
                                objStrRect,
                                stringFormat);
            }
            return;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
