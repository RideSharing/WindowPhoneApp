using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RideSharingWPApp.Global
{
    class GlobalData
    {
        public static ItineraryList itinearyList = new ItineraryList();
        public static Itinerary2 selectedItinerary = new Itinerary2();

        public static bool isDriver = false;

        public static string APIkey = null;

        public static int customer_status = 0;

        public static int driver_status = 0;

        //public static bool isDriver = true;

        //public static string APIkey = "ce1fb637b7eee845c73b207d931bbc10";

        //public static int customer_status = 4;

        //public static int driver_status = 2;

        public const int ITINERARY_STATUS_CREATED = 1;
        public const int ITINERARY_STATUS_CUSTOMER_ACCEPTED = 2;
        public const int ITINERARY_STATUS_DRIVER_ACCEPTED = 3;
        public const int ITINERARY_STATUS_FINISHED = 4;

        public static void deleteUserInfoBeforeLogout()
        {

        }
    }
}
