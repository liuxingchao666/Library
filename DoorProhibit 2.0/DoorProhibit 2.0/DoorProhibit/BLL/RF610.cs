using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoorProhibit.BLL
{
    public class RF610
    {
        SerialPort serialPort = new SerialPort();
        public bool connState=true;
        public bool conn()
        {
            List<string> list= SerialPort.GetPortNames().ToList();
            foreach (string portName in list)
            {
                serialPort.PortName = portName;
                serialPort.BaudRate = 9600;
                serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(sp_DataReceived);
                serialPort.Open();
                if (serialPort.IsOpen)
                {
                    byte[] send = new byte[6];
                    send[0] = 0x02;
                    send[1] = 0x00;
                    send[2] = 0x02;
                    send[3] = 0x31;
                    send[4] = 0x30;
                    send[5] = 0x03;
                    serialPort.Write(send,0,6);
                    Thread.Sleep(300);
                }
                if (!connState)
                    return true;
                else
                {
                    serialPort.Close();
                    serialPort.Dispose();
                }
            }
            return false;
        }
        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int ireadbit = serialPort.BytesToRead;
            byte[]  RecvBuf = new byte[ireadbit];
            serialPort.Read(RecvBuf,0,ireadbit);
            if (RecvBuf[3].ToString().Contains("50")&& RecvBuf[2].ToString().Contains("13"))
            {
                connState = false;
            }
        }
    }
}
