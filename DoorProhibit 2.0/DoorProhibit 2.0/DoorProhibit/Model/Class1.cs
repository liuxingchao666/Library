using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorProhibit.Model
{
    public class MessageMan
    {
        private byte btPacketType;     //数据包头，默认为0xA0
        private byte btDataLen;        //数据包长度，数据包从‘长度’字节后面开始的字节数，不包含‘长度’字节本身
        private byte btReadId;         //读写器地址
        private byte btCmd;            //数据包命令代码
        private byte[] btAryData;      //数据包命令参数，部分命令无参数
        private byte btCheck;          //校验和，除校验和本身外所有字节的校验和
        private byte[] btAryTranData;  //完整数据包

        public byte BtPacketType { get => btPacketType; set => btPacketType = value; }
        public byte BtDataLen { get => btDataLen; set => btDataLen = value; }
        public byte BtReadId { get => btReadId; set => btReadId = value; }
        public byte BtCmd { get => btCmd; set => btCmd = value; }
        public byte[] BtAryData { get => btAryData; set => btAryData = value; }
        public byte BtCheck { get => btCheck; set => btCheck = value; }
        public byte[] BtAryTranData { get => btAryTranData; set => btAryTranData = value; }



        public MessageMan(byte[] btAryTranData)
        {
            //长度
            int nLen = btAryTranData.Length;

            this.btAryTranData = new byte[nLen];
            btAryTranData.CopyTo(this.btAryTranData, 0);


            byte btCK = CheckSum(this.btAryTranData, 0, this.btAryTranData.Length - 1);
            if (btCK != btAryTranData[nLen - 1])
            {
                return;
            }
            //包头
            this.btPacketType = btAryTranData[0];
            //数据长度
            this.btDataLen = btAryTranData[1];
            //地址
            this.btReadId = btAryTranData[2];
            //命令
            this.btCmd = btAryTranData[3];
            //CS校验和
            this.btCheck = btAryTranData[nLen - 1];

            if (nLen > 5)
            {//命令参数
                this.btAryData = new byte[nLen - 5];
                for (int nloop = 0; nloop < nLen - 5; nloop++)
                {
                    this.btAryData[nloop] = btAryTranData[4 + nloop];
                }
            }
        }
        public byte CheckSum(byte[] btAryBuffer, int nStartPos, int nLen)
        {
            byte btSum = 0x00;

            for (int nloop = nStartPos; nloop < nStartPos + nLen; nloop++)
            {
                btSum += btAryBuffer[nloop];
            }

            return Convert.ToByte(((~btSum) + 1) & 0xFF);
        }
    }
}
