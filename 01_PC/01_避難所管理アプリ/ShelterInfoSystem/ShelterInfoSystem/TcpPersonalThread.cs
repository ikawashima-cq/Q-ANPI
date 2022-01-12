/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2018. All rights reserved.
 */
/**
 * @file    TcpPersonalThread.cs
 * @brief   TcpIP通信個人安否情報受信スレッドクラス
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ShelterInfoSystem
{
    class TcpPersonalThread
    {
        // スマホIPアドレス通知ブロードキャスト用ポート番号
        // PC側とスマホ側で同じ番号を設定する
        // 他のプログラムと被らなさそうなものを設定(後々このポート番号で問題になるようなら調整する)
        private const int BROADCAST_PORT_NO = 56412;

        /// <summary>
        /// エンディアン形式
        /// </summary>
        enum Endian
        {
            Little, Big
        }
        
        // 32bit暗号の複合化用定数
        const String KEY = "zRcRMrWYdU2i3J4z";
        const String IV = "test_iva";

        /// <summary>
        ///  データ読み書きタイムアウト(10秒)
        /// </summary>
        const int TIMEOUT = 10000;

        /// <summary>
        /// 同時接続可能最大数(10台)
        /// </summary>
        const int CONNECT_MAX = 10;


        // 送信履歴用
        private struct SendData
        {
            public bool bEdit;
            public bool bDelete;
            public bool bSendWait;
            public DbAccess.PersonInfo info;

            public void init()
            {
                bEdit = false;
                bDelete = false;
                bSendWait = true;
                info = new DbAccess.PersonInfo();
            }
        }


        /// <summary>
        /// 個人安否情報TCP/IP通信スレッド開始
        /// </summary>
        public void startTcpPersonalThread(FormShelterInfo mainForm, bool isFirst)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    // IPアドレス（定義）
                    string ipString = "0.0.0.0";

                    // 自マシンのホスト名を取得する
                    string hostname = Dns.GetHostName();

                    // 初回のみ自マシンのIPアドレスを取得する
                    if (isFirst)
                    {
                        // ホスト名からIPアドレスを取得する
                        IPAddress[] adrList = Dns.GetHostAddresses(hostname);
                        foreach (IPAddress address in adrList)
                        {
                            // IPアドレスが複数存在した場合、一番最初に見つかったものを採用する
                            if ((!address.IsIPv6LinkLocal) && (!address.IsIPv6Multicast) && (!address.IsIPv6SiteLocal) && (!address.IsIPv6Teredo))
                            {
                                Console.WriteLine(address.ToString());
                                ipString = address.ToString();
                                Program.m_objShelterAppConfig.IPAddress = ipString;
                                break;
                            }
                        }
                    }

                    // 同時通信数(最大10)
                    int nowConnect = 0;

                    // ブロードキャスト送信間隔(5秒毎)
                    int broadCount = 5;

                    //接続要求監視ループ
                    while (true)
                    {
                        // スマホアプリに避難所管理PCのIPアドレスとポート番号を通知(5秒毎に通知)
                        if (broadCount > 5)
                        {
#if DEBUG
                            // デバッグ中はブロードキャストを送信しない
#else
                            SendBroadcastIP();
#endif
                            broadCount = 0;
                        }
                        broadCount++;

                        // ---ListenするIPアドレス,ポート番号の設定---
                        ipString = Program.m_objShelterAppConfig.IPAddress;
                        int port = int.Parse(Program.m_objShelterAppConfig.PortNo);

                        System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse(ipString);

                        //TcpListenerオブジェクトを作成する
                        System.Net.Sockets.TcpListener listener =
                            new System.Net.Sockets.TcpListener(ipAdd, port);

                        //TCP/IP通信開始(Listenを開始する)
                        listener.Start();

                        try
                        {
                            // スレッド停止フラグがONの場合、TCP/IP通信スレッド終了
                            if (mainForm.m_bTcpIpStop)
                            {
                                //リスナを閉じる
                                listener.Stop();
                                break;
                            }

                            // 接続要求監視中断用スレッド(接続要求監視を1秒ごとにリフレッシュする)
                            bool waitOn = true;
                            Task.Factory.StartNew(() =>
                            {
                                // 1秒待機し、接続要求がない場合リフレッシュ
                                System.Threading.Thread.Sleep(1000);
                                if (waitOn)
                                {
                                    listener.Stop();
                                }
                            });

                            //接続要求があったら受け入れる(要求が来るまで待機)
                            System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
                            client = listener.AcceptTcpClient();
                            waitOn = false; // 中断無効化

                            Task.Factory.StartNew(() =>
                            {
                                nowConnect++;

                                // 1ソケット分の処理を実施
                                execSocketFunc(mainForm, client, nowConnect);

                                nowConnect--;

                            });
                        }
                        catch (Exception)
                        {
                        }

                        //リスナを閉じる
                        listener.Stop();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(mainForm, "指定されたIPアドレス/ポート番号への接続に失敗したため個人安否情報受信を停止します。" + Environment.NewLine +
                                    "個人安否情報受信を再開するためには、安否情報受信設定ダイアログから" +
                                    "接続先IPアドレス/ポート番号を再設定して下さい。" + Environment.NewLine
                                    , "避難所システム", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "FormShelterInfo", "person", "設定されたIPアドレス/ポート番号への接続失敗");
                }
            });
        }

        /// <summary>
        /// 1ソケット毎の処理
        /// </summary>
        private void execSocketFunc(FormShelterInfo mainForm, System.Net.Sockets.TcpClient client,int nowConnect)
        {
            int errCode = 0;

            //NetworkStreamを取得
            System.Net.Sockets.NetworkStream ns = client.GetStream();

            //読み取り、書き込みのタイムアウトを設定する
            ns.ReadTimeout = TIMEOUT;
            ns.WriteTimeout = TIMEOUT;

            // 同時通信数が10以下の場合、データ取得(超えた場合はエラー)
            if (nowConnect <= CONNECT_MAX)
            {
                //クライアントから送られたデータを受信する
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                int resSize = 0;
                try
                {
                    do
                    {
                        errCode = 0;
                        //byte配列からデータを取得
                        //先頭から、2byte:データ部サイズ
                        //          1byte:暗号化フラグ(0x00=非暗号化,0x01=暗号化)
                        //          可変長：個人安否情報(非暗号化の場合はカンマ「,」区切りのテキスト)

                        // データ部サイズ2byte分をを取得
                        //(データがない場合はデータが来るまで待機　タイムアウトしたらループを抜ける)
                        byte[] dataSizeBites = new byte[2];
                        resSize = ns.Read(dataSizeBites, 0, dataSizeBites.Length);
                        if (resSize == 0)
                        {
                            // データが読み込めない場合、切断されたとみなしデータ読み込みを終了
                            break;
                        }

                        //暗号化フラグ(1byte)と個人安否情報(可変長)を取得
                        int dataSizeInt = BitConverter.ToInt16(dataSizeBites, 0);
                        byte[] otherBites = new byte[dataSizeInt - 2];
                        resSize = ns.Read(otherBites, 0, otherBites.Length);
                        if (resSize == 0)
                        {
                            // データが読み込めない場合、切断されたとみなしデータ読み込みを終了
                            // さらにデータ部サイズ以降のデータの読み込みができない状態なのでエラーとして処理
                            errCode = -4;
                            Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "execSocketFunc", "個人安否情報受信(TCP/IP通信)", "データ不正");

                            //応答フォーマットを返す
                            returnFormat(errCode, ns);
                            continue;
                        }

                        //受信したデータをbyte配列から文字列に変換(文字コードはSJIS)
                        string isEncording = System.Text.Encoding.Default.GetString(otherBites, 0, 1);
                        string personalData = System.Text.Encoding.Default.GetString(otherBites, 1, (dataSizeInt - 3));
                        personalData = personalData.TrimEnd('\n');   //末尾の\nを削除
                        personalData = personalData.TrimEnd('\0');   //末尾の\0を削除
                        Console.WriteLine(personalData);

                        // 32bit暗号を複合化
                        string decodeStr = "";
                        bool decodeOK = true;
                        if (isEncording == "1")
                        {
                            // 複合化
                            Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "execSocketFunc", "個人安否情報受信(TCP/IP通信)", "複合化を実施： 複合前文字列=[" + personalData + "]");
                            decodeOK = decrypt(personalData, ref decodeStr);
                            if (!decodeOK)
                            {
                                errCode = -5;
                                Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "execSocketFunc", "個人安否情報受信(TCP/IP通信)", "暗号化文字列の複合化失敗, 受信データ=[" + personalData + "]");
                                //応答フォーマットを返す
                                returnFormat(errCode, ns);
                                continue;
                            }
                        }
                        else
                        {
                            // 暗号化フラグがOFFの場合、そのまま読み取り
                            decodeStr = personalData;
                        }

                        // 個人安否情報のフォーマットチェック
                        if (!CheckFormatPersonalData(decodeStr))
                        {
                            errCode = -3;
                            //応答フォーマットを返す
                            returnFormat(errCode, ns);
                            continue;
                        }

                        // 取得した個人安否情報をDBに登録
                        // (応答メッセージはすぐに返さなければならないので、時間のかかる登録処理は別スレッドで実施)
                        Task.Factory.StartNew(() =>
                        {
                            // 重複処理の排他制御
                            // (同時にDBに書き込みを行うとエラーとなるため、DB書き込み処理は1処理ずつ実施する)
                            lock (mainForm)
                            {
                                if (!resisterParsonalData(mainForm, decodeStr))
                                {
                                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "execSocketFunc", "個人安否情報受信(TCP/IP通信)", "データ重複の為、個人安否情報登録失敗, 受信データ=[" + decodeStr + "]");
                                }
                                else
                                {
                                    Program.m_thLog.PutLog(LogThread.P_LOG_OPE, "execSocketFunc", "個人安否情報受信(TCP/IP通信)", "データ登録OK, 受信データ=[" + decodeStr + "]");
                                }
                             }
                        });

                        //タイムアウト以外は応答フォーマットを返す
                        returnFormat(errCode, ns);

                    } while (ns.DataAvailable);

                }
                catch
                {
                    // サーバ処理異常
                    errCode = -2;
                    returnFormat(errCode, ns);
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "execSocketFunc", "個人安否情報受信(TCP/IP通信)", "サーバ処理異常発生");
                }

                // データ読み込みをクローズ    
                ms.Close();
            }
            else
            {
                // 同時通信数オーバー(queue full)
                errCode = -1;
                returnFormat(errCode, ns);
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "execSocketFunc", "個人安否情報受信(TCP/IP通信)", "同時通信数オーバー");
            }

            //クライアントとの通信(ソケット)を閉じる
            ns.Close();
            client.Close();
        }

        /// <summary>
        /// 応答フォーマットをクライアントへ返信する
        /// </summary>
        /// <param name="errCode"></param>
        /// <param name="ns"></param>
        /// <returns></returns>
        private bool returnFormat(int errCode, System.Net.Sockets.NetworkStream ns)
        {
            try
            {
                //クライアントに送信するデータ列を作成
                byte[] returnSendBytes = null;
                int redCode = 0, errDetail = 0;
                CreateReturnFormat(errCode, ref returnSendBytes, ref redCode, ref errDetail);

                //データを送信する
                ns.Write(returnSendBytes, 0, returnSendBytes.Length);
                Program.m_thLog.PutLog(LogThread.P_LOG_TRANS, "execSocketFunc", "個人安否情報受信(TCP/IP通信)", "応答コード=[" + redCode + "]" + "詳細コード=[" + errDetail + "]");

                int timeoutCount = 0;     
                while (!ns.DataAvailable)
                {
                    // 次のデータが送信されるまで少し待つ
                    System.Threading.Thread.Sleep(100);

                    timeoutCount++;
                    //タイムアウトは10秒
                    if (timeoutCount > TIMEOUT/100)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                // クライアントがタイムアウトしていた場合はエラーになる
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "execSocketFunc", "個人安否情報受信(TCP/IP通信)", "クライアントタイムアウト");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 1人分の個人安否情報をDBに登録する
        /// </summary>
        /// <returns></returns>
        private bool resisterParsonalData(FormShelterInfo mainForm, string personalData)
        {
            string[] splitData = personalData.Split(',');

            for (int ic = 0; ic < splitData.Length - 1; ic++)
            {
                splitData[ic] = splitData[ic].Replace("\n", "");
                splitData[ic] = splitData[ic].Replace("\r", "");
            }

            DbAccess.PersonInfo info = new DbAccess.PersonInfo();
            info.init();

            // 電話番号
            info.id = splitData[0];

            // 名前
            info.name = splitData[1];
            info.name = info.name.Replace(' ', '　');

            // 住所
            info.txt01 = splitData[6];

            //sel02	入/退所  0:入所 1:退所 2:在宅 
            info.sel02 = splitData[4];

            //生年月日
            info.txt02 = splitData[2].Substring(0, 4) + "-" + splitData[2].Substring(4, 2) + "-" + splitData[2].Substring(6, 2);

            //sel01	性別       0:男性 1:女性
            info.sel01 = splitData[3];

            //sel03	公表       0:しない 1:する
            info.sel03 = splitData[5];

            //sel04	怪我       0:無 1:有 2:未使用 3:未選択
            if (splitData[7] == "")
            {
                info.sel04 = "3";
            }
            else
            {
                info.sel04 = splitData[7];
            }

            //sel05	介護       2:未選択 0:否  1:要
            if (splitData[8] == "")
            {
                info.sel05 = "2";
            }
            else
            {
                info.sel05 = splitData[8];
            }

            //sel06	障がい    2:未選択 0:無  1:有
            if (splitData[9] == "")
            {
                info.sel06 = "2";
            }
            else
            {
                info.sel06 = splitData[9];
            }

            //sel07	妊産婦    2:未選択 0:いいえ  1:はい
            if (splitData[10] == "")
            {
                info.sel07 = "2";
            }
            else
            {
                info.sel07 = splitData[10];
            }

            // アプリ側改修までの一時対応
            if (splitData.Length > 11)
            {
                //sel08	避難所内外    0:内  1:外
                if (splitData[11] == "")
                {
                    info.sel08 = "0";
                }
                else
                {
                    info.sel08 = splitData[11];
                }
            }
            else
            {
                //sel08	避難所内外は入所/退所に依存（入所：内、退所/在宅：外）
                if (info.sel02 == "0")
                {
                    info.sel08 = "0";
                }
                else
                {
                    info.sel08 = "1";
                }
            }

            // 更新日時
            info.update_datetime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            // sidは現在アクティブに設定されている避難所IDに設定する
            // 現在アクティブな避難所が「開設」でない場合、仮IDを設定する
            // 避難所が1件も登録されていない場合は仮の避難所IDを設定する
            if (String.IsNullOrEmpty(mainForm.GetActiveTerminalInfo().sid)||
                mainForm.GetActiveTerminalInfo().open_flag != FormShelterInfo.SHELTER_STATUS.OPEN)
            {
                info.sid = Program.TEMP_SHELTER_ID;
            }
            else
            {
                info.sid = mainForm.GetActiveTerminalInfo().sid;
            }
            // 個人安否情報をメイン画面に登録
            if (!mainForm.resisterPersonalData(info))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 32bit暗号複合化処理
        /// </summary>
        /// <param name="inputStr">暗号化された文字列</param>
        /// <returns>複合化した文字列</returns>
        private bool decrypt(String inputStr, ref String outputStr)
        {
            outputStr = "";

            try
            {
                Encoding encording = Encoding.GetEncoding("Shift_JIS");
                // 暗号化キー
                byte[] cipherKey = encording.GetBytes(KEY);
                // BlowFishインスタンス生成
                BlowFish bf = new BlowFish(cipherKey);
                bf.IV = encording.GetBytes(IV);
                byte[] bEnc = Convert.FromBase64String(inputStr);
                String strEnc = ByteToHex(bEnc);
                outputStr = bf.Decrypt_ECB_SJIS(strEnc);
            }
            catch (FormatException)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "32bit暗号化", "複合化失敗");
                return false;
            }
            catch (Exception)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_DEBUG, "32bit暗号化", "複合化失敗");
                return false;
            }
            return true;
        }
        private String ByteToHex(byte[] bytes)
        {
            StringBuilder s = new StringBuilder();

            foreach (byte b in bytes)
            {
                s.Append(b.ToString("x2"));
            }

            return s.ToString();
        }

        /// <summary>
        /// エラーコードから返信フォーマットを生成する
        /// </summary>
        /// <param name="errCode"></param>
        /// <param name="returnSendBytes"></param>
        private void CreateReturnFormat(int errCode, ref byte[] returnSendBytes,ref int retCode, ref int retDetailCode)
        {
            // 応答フォーマットデータサイズ(固定)
            byte[] dataSize = BitConverter.GetBytes((short)6);

            // 応答コード
            byte[] returnCode = BitConverter.GetBytes((short)0);

            // 詳細コード
            byte[] retErrCode = BitConverter.GetBytes((short)0);
            switch (errCode)
            {
                case 0:
                    returnCode = BitConverter.GetBytes((short)0);
                    retErrCode = BitConverter.GetBytes((short)0);
                    break;
                case -1:
                    returnCode = BitConverter.GetBytes((short)1);
                    retErrCode = BitConverter.GetBytes((short)1);
                    break;

                case -2:
                    returnCode = BitConverter.GetBytes((short)1);
                    retErrCode = BitConverter.GetBytes((short)2);
                    break;

                case -3:
                    returnCode = BitConverter.GetBytes((short)2);
                    retErrCode = BitConverter.GetBytes((short)3);
                    break;

                case -4:
                    returnCode = BitConverter.GetBytes((short)2);
                    retErrCode = BitConverter.GetBytes((short)4);
                    break;

                case -5:
                    returnCode = BitConverter.GetBytes((short)2);
                    retErrCode = BitConverter.GetBytes((short)5);
                    break;

                default:
                    returnCode = BitConverter.GetBytes((short)1);
                    retErrCode = BitConverter.GetBytes((short)1);
                    break;
            }

            returnSendBytes = new byte[dataSize.Length + returnCode.Length + retErrCode.Length];
            dataSize.CopyTo(returnSendBytes, 0);
            returnCode.CopyTo(returnSendBytes, dataSize.Length);
            retErrCode.CopyTo(returnSendBytes, dataSize.Length + returnCode.Length);

            retCode =BitConverter.ToInt16(returnCode,0);
            retDetailCode =BitConverter.ToInt16(retErrCode,0);
            return;
        }

        /// <summary>
        /// 個人安否情報が正しいフォーマットかどうかチェックする
        /// </summary>
        /// <param name="personalData"></param>
        /// <returns></returns>
        private bool CheckFormatPersonalData(string personalData)
        {
            string[] splitData = personalData.Split(',');

            // カラム数チェック(11カラム必要)
            if (
                (splitData.Length != 11)&&      // アプリ側改修までの一時対応(スマホアプリ側で避難所内外を指定する)
                (splitData.Length != 12)
                )
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "CheckFormatPersonalData", "個人安否情報受信 受信データフォーマット不正", "カラム数が11ではない, 受信データ=[" + personalData + "]");
                return false;
            }

            // 以下各カラムのフォーマットチェック
            // 電話番号(最大12桁)
            if ((splitData[0].Length < 1) || (splitData[0].Length > 12))
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "CheckFormatPersonalData", "個人安否情報受信 受信データフォーマット不正", "(カラム1)電話番号エラー(最大12桁), 受信データ=[" + personalData + "]");
                return false;
            }

            // 氏名(最大25文字)
            if ((splitData[1].Length < 1) || (splitData[1].Length > 25))
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "CheckFormatPersonalData", "個人安否情報受信 受信データフォーマット不正", "(カラム2)氏名エラー(最大25文字), 受信データ=[" + personalData + "]");
                return false;
            }

            // 生年月日(yyyymmdd)
            bool isDateStrOk = true;
            bool isYearOk = true;
            bool isDateOk = true;
            //生年月日のフォーマットチェック(yyyymmdd)
            if (!System.Text.RegularExpressions.Regex.IsMatch(
                splitData[2],
                @"\d\d\d\d\d\d\d\d"))
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "CheckFormatPersonalData", "個人安否情報受信 受信データフォーマット不正", "(カラム3)生年月日エラー(yyyymmdd), 受信データ=[" + personalData + "]");
                isDateStrOk = false;
            }
            else
            {
                DateTime dt = DateTime.Now;
                // 年は(120年前)～(現在)までの範囲内か 
                if (((dt.Year - (int.Parse(splitData[2].Substring(0, 4)))) < 0) || ((dt.Year - (int.Parse(splitData[2].Substring(0, 4)))) > 120))
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "CheckFormatPersonalData", "個人安否情報受信 受信データフォーマット不正", "(カラム3)生年月日エラー(誕生日は120年前まで登録可), 受信データ=[" + personalData + "]");
                    isYearOk = false;
                }
                // 日付として成立しているか
                try
                {
                    DateTime.Parse(splitData[2].Substring(0, 4) + "/" + splitData[2].Substring(4, 2) + "/" + splitData[2].Substring(6, 2));
                }
                catch (Exception)
                {
                    Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "CheckFormatPersonalData", "個人安否情報受信 受信データフォーマット不正", "(カラム3)生年月日エラー(存在しない日付), 受信データ=[" + personalData + "]");
                    isDateOk = false;
                }
            }
            // フォーマット、年の範囲、正しい年月日かのチェックいずれかがNGの場合フォーマットエラーとする
            if (!isDateStrOk || !isYearOk || !isDateOk)
            {
                return false;
            }


            // 性別(0:男、1:女)
            if ((splitData[3] != "0") && (splitData[3] != "1"))
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "CheckFormatPersonalData", "個人安否情報受信 受信データフォーマット不正", "(カラム4)性別エラー(0:男、1:女), 受信データ=[" + personalData + "]");
                return false;
            }

            // 入所状況(0:入所、1:退所、2:在宅)
            if ((splitData[4] != "0") && (splitData[4] != "1") && (splitData[4] != "2"))
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "CheckFormatPersonalData", "個人安否情報受信 受信データフォーマット不正", "(カラム5)入所状況エラー(0:入所、1:退所、2:在宅), 受信データ=[" + personalData + "]");
                return false;
            }
            // 公表可否(0:許可、1:拒否)
            if ((splitData[5] != "0") && (splitData[5] != "1"))
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "CheckFormatPersonalData", "個人安否情報受信 受信データフォーマット不正", "(カラム6)公表可否エラー(0:許可、1:拒否), 受信データ=[" + personalData + "]");
                return false;
            }
            // 住所(最大64文字)
            if (splitData[6].Length > 64)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "CheckFormatPersonalData", "個人安否情報受信 受信データフォーマット不正", "(カラム7)住所エラー(最大64文字), 受信データ=[" + personalData + "]");
                return false;
            }
            // ケガの有無(0:無し、1:有り、NULL:※DB登録しない)
            if ((splitData[7] != "0") && (splitData[7] != "1") && (splitData[7] != ""))
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "CheckFormatPersonalData", "個人安否情報受信 受信データフォーマット不正", "(カラム8)ケガの有無エラー(0:無し、1:有り、NULL:※DB登録しない), 受信データ=[" + personalData + "]");
            }

            // 要介護(0:不要、1:必要、NULL:※DB登録しない)
            if ((splitData[8] != "0") && (splitData[8] != "1") && (splitData[8] != ""))
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "CheckFormatPersonalData", "個人安否情報受信 受信データフォーマット不正", "(カラム9)要介護エラー(0:不要、1:必要、NULL:※DB登録しない), 受信データ=[" + personalData + "]");
                return false;
            }

            // 障がいの有無(0:無し、1:有り、NULL:※DB登録しない)
            if ((splitData[9] != "0") && (splitData[9] != "1") && (splitData[9] != ""))
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "CheckFormatPersonalData", "個人安否情報受信 受信データフォーマット不正", "(カラム10)障がいの有無エラー(0:無し、1:有り、NULL:※DB登録しない), 受信データ=[" + personalData + "]");
                return false;
            }

            // 妊産婦(0:いいえ、1:はい、NULL:※DB登録しない)
            if ((splitData[10] != "0") && (splitData[10] != "1") && (splitData[10] != ""))
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "CheckFormatPersonalData", "個人安否情報受信 受信データフォーマット不正", "(カラム11)妊産婦エラー(0:いいえ、1:はい、NULL:※DB登録しない), 受信データ=[" + personalData + "]");
                return false;
            }

            return true;
        }

        /// <summary>
        /// スマホに待ち受けポート番号をブロードキャストで送信する
        /// </summary>
        private void SendBroadcastIP()
        {
            // PC側待ち受けポート番号
            string sendMessage = "ANPI_" + Program.m_objShelterAppConfig.PortNo;

            // 送受信に利用するポート番号
            var port = BROADCAST_PORT_NO;

            // 送信データ
            var buffer = Encoding.UTF8.GetBytes(sendMessage);
            // var reverseBuff = Reverse(buffer, Endian.Big);

            try
            {
                // ブロードキャスト送信
                var client = new UdpClient(port);
                client.EnableBroadcast = true;
                //System.Net.IPAddress ipAdd = System.Net.IPAddress.Parse("127.0.0.1");     // ローカルテスト用
                //client.Connect(new IPEndPoint(ipAdd, port));                              // ローカルテスト用
                string hostIP = "255.255.255.255";
                client.Send(buffer, buffer.Length, hostIP, port);
                client.Close();
            }
            catch (Exception ex)
            {
                Program.m_thLog.PutLog(LogThread.P_LOG_ERR, "AutoSendThread", "SendBroadcastIP", "ブロードキャスト失敗" + ex.ToString());
            }
        }

        /// <summary>
        /// byteのエンディアン調整用
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="endian"></param>
        /// <returns></returns>
        static byte[] Reverse(byte[] bytes, Endian endian)
        {
            if (BitConverter.IsLittleEndian ^ endian == Endian.Little)
                return bytes.Reverse().ToArray();
            else
                return bytes;
        }
    }
}
