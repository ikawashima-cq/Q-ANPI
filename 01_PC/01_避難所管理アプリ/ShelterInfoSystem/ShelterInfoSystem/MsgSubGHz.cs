/*
 * NEC Confidential
 * Copyright(c) NEC Corporation 2016-2017. All rights reserved.
 */
using System;

namespace ShelterInfoSystem
{
    /**
     * @class MsgSubGHz
     * @brief メッセージ管理。エンコード／デコードを実行する。
     */
    public class MsgSubGHz
    {
        protected const int HEADER_SIZE = 13;
        protected const int RET_NG = -1;
        protected const int RET_OK = 0;

        public byte Length;
		public byte MsgId;
        public byte MsgNo;
        public byte[] DstID;
        public byte[] SrcID;
        public byte[] Parameter;
		
		public byte[] encodedData;
        public byte Port;
        public byte Num;


        public MsgSubGHz()
        {
            encodedData = new byte[HEADER_SIZE];

            // Start
            encodedData[0] = 0x0F;
            encodedData[1] = 0x5A;
            MsgId = 0;
            MsgNo = 0;
            DstID = new byte[4];
            SrcID = new byte[4];
            Parameter = new byte[0];
            Length = (byte)(encodedData.Length);
	    }

        //　ヘッダ部のみ
        public int encode() 
        {
            encodedData = new byte[HEADER_SIZE + Parameter.Length];
            encodedData[0] = 0x0F;
            encodedData[1] = 0x5A;

            // メッセージ長 254 byte以内
            if(Parameter.Length >= 0xFF){
                // 大きすぎ
                return RET_NG;
            }
            encodedData[2] = (byte)(Parameter.Length + HEADER_SIZE);
            encodedData[3] = MsgId;
            encodedData[4] = MsgNo;
            if (SrcID == null || SrcID.Length < 4)
            {
                SrcID = new byte[4];
                SrcID[0] = 0xFF; SrcID[1] = 0xFF;
                SrcID[2] = 0xFF; SrcID[3] = 0xFF;
            }
            if (DstID == null  || Parameter == null)
            {
                return RET_NG;
            }

            for (int i = 0; i < 4; i++)
            {
                encodedData[5 + i] = DstID[i];
                encodedData[9 + i] = SrcID[i];
            }

            for (int i = 0; i < Parameter.Length; i++)
            {
                encodedData[13 + i] = Parameter[i];
            }

            return RET_OK;
	    }

        // 


        // ヘッダ部およびパラメータ部にセット
	    public int decode()
        {
            if (encodedData.Length < HEADER_SIZE)
            {
                return RET_NG;
            }

            MsgId = encodedData[3];
            MsgNo = encodedData[4];
            Length = encodedData[2];
            for (int i = 0; i < 4; i++)
            {
                DstID[i] = encodedData[5 + i];
                SrcID[i] = encodedData[9 + i];
            }


            Parameter = new byte[Length];
            for (int i = 0; i < encodedData.Length - HEADER_SIZE; i++)
            {
                Parameter[i] = encodedData[13 + i];
            }

            Port = Parameter[0];

            Num = Parameter[1];

            return RET_OK;
	    }

        // パラメータ取得
        private byte[] getParamByKey(string key, string[] keys, int[] size)
        {
            byte[] ret = { 0 };
            if (key == null || Parameter == null)
            {
                return ret;
            }
            int pos = 0;
            for (int i = 0; i < keys.Length; i++)
            {
                if (pos > Parameter.Length)
                {
                    break;
                }
                if (key.Equals(keys[i]))
                {
                    ret = new byte[size[i]];
                    for (int j = 0; j < size[i]; j++)
                    {
                        ret[j] = Parameter[pos + j];
                    }
                }
                pos += size[i];
            }

            return ret;
        }

        // デフォルト設定読み込み MsgID=0x7D のときのパラメータ取得
        public byte[] getMsg7D(string key)
        {
            string[] keys = { "Power", "Channel", "RF_Baud", "CS_Mode", "Cmd_Enable"
                            , "Rsp_Enable", "Retry_Count", "Uart_Baud", "Sleep_Time", "Rcv_Time"
                            , "Forward_ID1", "Forward_ID2", "System_ID", "Product_ID", "Device_ID"
                            , "FW_ID", "FW_Ver"};
            int[] size = { 1, 1, 1, 1, 1  , 1, 1, 1, 1, 2
                          , 4, 4, 2, 2, 4  , 2, 2};

            return getParamByKey(key, keys, size);
        }

        public string makeMsg7dDeviceID()
        {
            byte[] bDevice = getMsg7D("Device_ID");
            if (bDevice.Length < 4)
            {
                return "ERR";
            }
            string sDevice = "";
            for (int i = 0; i < 4; i++)
            {
                sDevice += Convert.ToString(bDevice[i], 16).PadLeft(2, '0');
            }

            sDevice = sDevice.ToUpper();

            return sDevice;
        }

        // デバイス検索 MsgID=0x10 のときのパラメータ取得
        public byte[] getMsg10(string key)
        {
            string[] keys = { "System_ID", "Product_ID", "Rssi1", "Rssi2"};
            int[] size = { 2,2,1,1};

            return getParamByKey(key, keys, size);
        }

        public string makeMsg10()
        {
            // バイナリを１６進数文字列に
            byte[] Rssi1 = getMsg10("Rssi1");
            byte[] Rssi2 = getMsg10("Rssi2");

            byte[] Device_ID = SrcID;
            string strDevice_ID = "";
            for (int i = 0; i < Device_ID.Length; i++)
            {
                string hsin = Convert.ToString(Device_ID[i], 16);
                hsin = hsin.ToUpper();
                hsin = hsin.PadLeft(2, '0');
                strDevice_ID += hsin;
            }

            string msg = "DeviceID:" + strDevice_ID + "\r\n"
                        + "RSSI1:-" + Rssi1[0] + "dBm RSSI2:-" + Rssi2[0] + "dBm\r\n";

            return msg;
        }
    }
}
