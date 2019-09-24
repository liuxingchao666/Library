using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rfid系统
{
   public static  class Objextension
    {
        public static int ToInt(this object obj)
        {
            if (obj == null)
                return 0;
            if (string.IsNullOrEmpty(obj.ToString()))
                return 0;
            if (int.TryParse(obj.ToString(), out int result))
                return result;
            return 0;
        }
        public static double ToDouble(this object obj)
        {
            if (obj == null)
                return 0;
            if (string.IsNullOrEmpty(obj.ToString()))
                return 0;
            if (double.TryParse(obj.ToString(), out double result))
                return result;
            return 0;
        }
        public static string ToDate(this object obj)
        {
            if (obj == null)
                return "";
            if (string.IsNullOrEmpty(obj.ToString()))
                return "";
            if (DateTime.TryParse(obj.ToString(), out DateTime result))
                return result.ToString("yyyy-MM-dd");
            return "";
        }
    }
}
