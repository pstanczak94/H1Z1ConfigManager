using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Z1_Config_Manager
{
    public class Tools
    {
        public static short StrToShort(string text, short defValue = 0)
        {
            short value;

            if (short.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
                return value;

            return defValue;
        }

        public static int StrToInt(string text, int defValue = 0)
        {
            int value;

            if (int.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
                return value;

            return defValue;
        }

        public static double StrToDouble(string text, double defValue = 0.0)
        {
            text = text.Replace(',', '.');

            double value;

            if (double.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
                return value;
            
            return defValue;
        }

        public static string DoubleToStr(double value)
        {
            return value.ToString("0.0##", CultureInfo.InvariantCulture);
        }
    }
}
