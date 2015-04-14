using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingWPApp.DateTimes
{
    class DatetimeConvert
    {
        public static DateTime convertDateTimeFromString(string datetimeString)
        {
            try
            {
                //DateTime datetime = new DateTime(0,0,0,0,0,0);
                int year = Convert.ToInt16(datetimeString.Substring(0, 4));
                int month = Convert.ToInt16(datetimeString.Substring(5, 2));
                int day = Convert.ToInt16(datetimeString.Substring(8, 2));
                int hour = Convert.ToInt16(datetimeString.Substring(11, 2));
                int minute = Convert.ToInt16(datetimeString.Substring(14, 2));

                DateTime datetime = new DateTime(year, month, day, hour, minute, 00);

                return datetime;
            }
            catch (Exception e)
            {
                return new DateTime(1, 1, 1, 1, 1, 00);
            }
            
        }
    }
}
