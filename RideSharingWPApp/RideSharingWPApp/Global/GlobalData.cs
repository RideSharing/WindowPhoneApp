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

        public static string role = "customer";

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


        public const int USER_NOT_REGISTER = 0;
        public const int USER_NOT_ACTIVATE = 1;//moi vua dk
        public const int USER_ACTIVATED = 2; //da kich hoat tai khoan thong qua email
        public const int USER_UPDATED_PROFILE = 3; //cap nhat thong tin tai khoan
        public const int USER_ACCEPT_UPDATED_PROFILE = 4; //admin accepted thong tin tai khoan ==> duoc phep dk hanh trinh
        public const int USER_LOCKED = 5;

        public static bool isDisplayMessageBox = false;

        public static void deleteUserInfoBeforeLogout()
        {

        }
    }
}
