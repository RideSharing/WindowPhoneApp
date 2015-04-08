using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingWPApp.Global
{
    class GlobalData
    {
        public static Itinerary2 selectedItinerary = new Itinerary2();

        public static bool isDriver = true;

        public static string APIkey = null;

        public static int customer_status = 0;

        public static int driver_status = 0;

        //public static bool isDriver = true;

        //public static string APIkey = "ce1fb637b7eee845c73b207d931bbc10";

        //public static int customer_status = 4;

        //public static int driver_status = 2;
    }
}
