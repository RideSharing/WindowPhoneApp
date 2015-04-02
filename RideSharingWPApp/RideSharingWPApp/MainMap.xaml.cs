using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RideSharingWPApp
{
    public partial class MainMap : PhoneApplicationPage
    {

        public MainMap()
        {
            InitializeComponent();

            //set current location

            //get itinerary
            getItinerary();
                //send get request
            

                //xu ly json


                //hien thi tren map


                //hien thi tren list
            //
        }

        public async void getItinerary()
        {
            var result = await Request.RequestToServer.sendGetRequest("itineraries");

            JObject jsonObject = JObject.Parse(result);


            string APIkey = jsonObject.Value<string>("error").Trim();
            
            //var xlong = jsonObject.SelectToken("itineraries");
            JArray jsonVal = (JArray)jsonObject.SelectToken("itineraries");
            var kk = jsonVal;

            var root =
            JsonConvert.DeserializeObject<RootObject>(result);

            string z = root.error.ToString();
            foreach (var k in kk)
            {
                //k.
                string yy = "";
                
                
            }
        }
    }

    public class Itinerary
    {
        public int itinerary_id { get; set; }
        public int driver_id { get; set; }
        public int? customer_id { get; set; }
        public string start_address { get; set; }
        public double start_address_lat { get; set; }
        public double start_address_long { get; set; }
        public object pick_up_address { get; set; }
        public object pick_up_address_lat { get; set; }
        public object pick_up_address_long { get; set; }
        public object drop_address { get; set; }
        public object drop_address_lat { get; set; }
        public object drop_address_long { get; set; }
        public string end_address { get; set; }
        public double end_address_lat { get; set; }
        public double end_address_long { get; set; }
        public string leave_date { get; set; }
        public int duration { get; set; }
        public double distance { get; set; }
        public string cost { get; set; }
        public string description { get; set; }
        public int status { get; set; }
        public string created_at { get; set; }
        public string driver_license { get; set; }
        public string driver_license_img { get; set; }
        public int user_id { get; set; }
        public string email { get; set; }
        public string fullname { get; set; }
        public string phone { get; set; }
        public string personalID { get; set; }
        public string link_avatar { get; set; }
        public int average_rating { get; set; }
    }

    public class RootObject
    {
        public bool error { get; set; }
        public List<Itinerary> itineraries { get; set; }
    }

}