using System;

namespace c_sharp_dll
{
    public class Utils
    {
        public DateTime DateFromGregorian(double valuationDate)
        {
            string valuationdatestring = valuationDate.ToString();
            if (valuationdatestring.Length != 8)
                throw new ArgumentException("Invalid valuationdate value entered!");

            DateTime dt = new DateTime(Convert.ToInt32(valuationdatestring.Substring(0, 4)),
                Convert.ToInt32(valuationdatestring.Substring(4, 2)),
                Convert.ToInt32(valuationdatestring.Substring(6, 2)));
            return dt;
        }

        public double GregorianFromDate(DateTime dt)
        {
            string strday = (dt.Day.ToString().Length == 1) ? "0" + dt.Day.ToString() : dt.Day.ToString();
            string strmonth = (dt.Month.ToString().Length == 1) ? "0" + dt.Month.ToString() : dt.Month.ToString();
            return Convert.ToDouble(dt.Year.ToString() + strmonth + strday);
        }

        public double TimeFromTenorString(string tenorString)
        {
            int numberofmonths;
            double timeval = Convert.ToDouble(tenorString.Substring(0, tenorString.Length - 1));
            string timetype = tenorString.Substring(tenorString.Length - 1, 1);

            switch (timetype.ToLower())
            {
                case "y":
                    numberofmonths = 12;
                    break;
                case "m":
                    numberofmonths = 1;
                    break;
                default:
                    throw new Exception("Invalid Tenor type!");
            }

            return timeval * numberofmonths / 12;
        }


    }
}
