using System;
using System.Windows;

namespace WpfApp2.Model
{
    public class ArchivesInfo
    {
        public int num
        {
            get;
            set;
        }

        public string color
        {
            get;
            set;
        }

        public string EPC
        {
            get;
            set;
        }

        public string Id
        {
            get;
            set;
        }

        public string PIC
        {
            get;
            set;
        }

        public string ArchivesName
        {
            get;
            set;
        }

        public string barcode
        {
            get;
            set;
        }

        public string serchNumber
        {
            get;
            set;
        }

        public string WZ
        {
            get;
            set;
        }

        public int RackState
        {
            get;
            set;
        }

        public Visibility IsOpen
        {
            get;
            set;
        }

        public Visibility IsClose
        {
            get;
            set;
        }

        public string Author
        {
            get;
            set;
        }

        public string Press
        {
            get;
            set;
        }

        public string PressDate
        {
            get;
            set;
        }

        public string DetailedMessage
        {
            get;
            set;
        }

        public string ICCardNum
        {
            get;
            set;
        }

        public string BSTime
        {
            get;
            set;
        }

        public string EDTime
        {
            get;
            set;
        }

        public int RenewableTimes
        {
            get;
            set;
        }

        public int SurplusRenewableTimes
        {
            get;
            set;
        }

        public string AppendRenewableTime
        {
            get;
            set;
        }

        public string PageNum
        {
            get;
            set;
        }

        public string source
        {
            get;
            set;
        }

        public bool isCheck
        {
            get;
            set;
        }

        public Visibility XJState
        {
            get;
            set;
        }

        public Visibility FState
        {
            get;
            set;
        }

        public Visibility SState
        {
            get;
            set;
        }

        public Visibility IsSend
        {
            get;
            set;
        }

        public string errorMsg
        {
            get;
            set;
        }

        public string ISBN
        {
            get;
            set;
        }

        public string GCSL
        {
            get;
            set;
        }

        public string code
        {
            get;
            set;
        }

        public string openBook
        {
            get;
            set;
        }

        public string LayOut
        {
            get;
            set;
        }

        public ArchivesInfo()
        {
          color = "Gray";
            XJState = Visibility.Visible;
            FState = Visibility.Hidden;
            SState = Visibility.Hidden;
            IsSend = Visibility.Hidden;
        }
    }
}
