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
using Microsoft.Phone.Maps.Controls;
using RideSharingWPApp.UserControls;
using System.Device.Location;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using RideSharingWPApp.Map;
using System.Windows.Media.Imaging;
using RideSharingWPApp.Global;

namespace RideSharingWPApp
{
    public partial class MainMap : PhoneApplicationPage
    {
        MapLayer mainMapLayer = new MapLayer();
        //List<MapOverlay> listMainMapOvelay = new List<MapOverlay>();
        RootObject root = null;
        List<Itinerary> itineraries = new List<Itinerary>();
        

        Geocoordinate myGeocoordinate = null;
        GeoCoordinate myGeoCoordinate = null;
        public MainMap()
        {
            InitializeComponent();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //set customer status tai trang Login va isDisplayMessageBox = false tai global data

            // check neu customer status  < 3  va isDisplayMessageBox = false  ==> hien thi messagebox
            // neu ko thi ko hien thi
            if (GlobalData.customer_status < GlobalData.USER_ACCEPT_UPDATED_PROFILE && GlobalData.isDisplayMessageBox == false)
            {
                MessageBoxResult result =
                MessageBox.Show("You need to update your information to use this app!",
               "Update Account", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    GlobalData.isDisplayMessageBox = true;
                    NavigationService.Navigate(new Uri("/AccountInfo.xaml", UriKind.Relative));
                }
                else
                {
                    GlobalData.isDisplayMessageBox = true;
                }
            }

            InitCurrentLocationInfo();
            getItinerary();

           
        }

        public void InitCurrentLocationInfo()
        {
            Task<GeoCoordinate> x = ShowMyCurrentLocationOnTheMap();
        }

        private async Task<GeoCoordinate> ShowMyCurrentLocationOnTheMap()
        {
            // Get my current location.
            Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            myGeocoordinate = myGeoposition.Coordinate;

            //wayPoints.Add(new GeoCoordinate(myGeocoordinate.Latitude, myGeocoordinate.Longitude));

            myGeoCoordinate = CoordinateConverter.ConvertGeocoordinate(myGeocoordinate);

            // Make my current location the center of the Map.
            this.mapMain.Center = myGeoCoordinate;
            this.mapMain.ZoomLevel = 16;

            // Create a MapOverlay to contain the circle.
            MapOverlay myCurentLocationOverlay = MarkerDraw.DrawCurrentMapMarker(myGeoCoordinate);

            // Create a MapLayer to contain the MapOverlay.
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myCurentLocationOverlay);

            // Add the MapLayer to the Map.
            mapMain.Layers.Add(myLocationLayer);

            return myGeoCoordinate;
        }

        public async void getItinerary()
        {
            mainMapLayer = new MapLayer();
            var result = await Request.RequestToServer.sendGetRequest("itineraries");

            JObject jsonObject = JObject.Parse(result);

            string error = jsonObject.Value<string>("error").Trim();
            
            //var xlong = jsonObject.SelectToken("itineraries");
            JArray jsonVal = (JArray)jsonObject.SelectToken("itineraries");
            //Convert json to object
            root = JsonConvert.DeserializeObject<RootObject>(result);

            foreach (Itinerary i in root.itineraries)
            {
                Global.GlobalData.itinearyList.Add(new Itinerary2
                {
                    itinerary_id = i.itinerary_id,
                    driver_id = i.driver_id,
                    customer_id = Convert.ToInt32(i.customer_id),
                    start_address = i.start_address,
                    start_address_lat = i.start_address_lat,
                    start_address_long = i.start_address_long,
                    end_address = i.end_address,
                    end_address_lat = i.end_address_lat,
                    end_address_long = i.end_address_long,
                    pick_up_address = i.pick_up_address,
                    pick_up_address_lat = i.pick_up_address_lat,
                    pick_up_address_long = i.pick_up_address_long,
                    drop_address = i.drop_address,
                    drop_address_lat = i.drop_address_lat,
                    drop_address_long = i.drop_address_long,
                    cost = i.cost,
                    distance = i.distance,
                    description = i.description,
                    duration = i.duration,
                    status = i.status,
                    created_at = i.created_at,
                    leave_date = i.leave_date,
                    driver_license = i.driver_license,
                    driver_license_img = i.driver_license_img,
                    user_id = i.user_id,
                    email = i.email,
                    fullname = i.fullname,
                    phone = i.phone,
                    personalID = i.personalID,
                    //convert base64 to image
                    link_avatar = ImageConvert.ImageConvert.convertBase64ToImage(i.link_avatar),
                    average_rating = i.average_rating
                });
                MapOverlay overlay = new MapOverlay();
                overlay = MarkerDraw.DrawItineraryMarker(new GeoCoordinate(Convert.ToDouble(i.start_address_lat),
                    Convert.ToDouble(i.start_address_long)), Global.GlobalData.itinearyList.Last());
                //chua su dung
                //listMainMapOvelay.Add(overlay);

                mainMapLayer.Add(overlay);
            }
            mapMain.Layers.Add(mainMapLayer);
            longlistItineraries.ItemsSource = Global.GlobalData.itinearyList;     
        }

        private void longlistItineraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Itinerary2 selectedItem = (Itinerary2)longlistItineraries.SelectedItem;
            MessageBox.Show("ss: " + selectedItem.itinerary_id);
            //luu tru tam thoi
            //Global.GlobalData.selectedItinerary = selectedItem;
            //navigate sang details
            NavigationService.Navigate(new Uri("/Customer/ItineraryDetails.xaml", UriKind.Relative));
        }
        
        private void menuSearch_Click(object sender, EventArgs e)
        {
            //display search
            StackPanel panel = new StackPanel();

            TextBox txtbStart = new TextBox();
            TextBlock b1 = new TextBlock(); b1.Text = "Password: ";

            TextBox txtbEnd = new TextBox();
            TextBlock b2 = new TextBlock(); b1.Text = "Password: ";

            Button btnAdvanceSearch = new Button(); btnAdvanceSearch.Content = "Advance Search";
            btnAdvanceSearch.Click += btnAdvanceSearch_Click;

            panel.Children.Add(b1);
            panel.Children.Add(txtbStart);
            panel.Children.Add(b2);
            panel.Children.Add(txtbEnd);
            panel.Children.Add(btnAdvanceSearch);

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                //set the properties
                Caption = "Search",
                Message = "",
                
                LeftButtonContent = "Find",
                RightButtonContent = "Cancel"
            };

            messageBox.Content = panel;
            //messageBox.Content = b2;

            //Add the dismissed event handler
            messageBox.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        //add the task you wish to perform when user clicks on yes button here
                        //goi ham search
                        //
                        var result = Request.RequestToServer.sendGetRequest("");


                        break;
                    case CustomMessageBoxResult.RightButton:
                        //add the task you wish to perform when user clicks on no button here

                        break;
                    case CustomMessageBoxResult.None:
                        // Do something.
                        break;
                    default:
                        break;
                }
            };

            //add the show method
            messageBox.Show();
        }

        void btnAdvanceSearch_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AdvanceSearch.xamll", UriKind.RelativeOrAbsolute));
        }
        private void menuHome_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Customer/MainMap.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menuPostItinerary_Click(object sender, EventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Customer/....xaml", UriKind.RelativeOrAbsolute));
        }

        private void menuItineraryManagement_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Customer/CustomerItineraryManagement.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menuAccountInfo_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AccountInfo.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menuAboutUs_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutUs.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menuLogOut_Click(object sender, EventArgs e)
        {

        }

        private void menuLogOut_Click_1(object sender, EventArgs e)
        {
            //delete UserInfo Before Logout
            Global.GlobalData.deleteUserInfoBeforeLogout();

            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (mapMain.ZoomLevel > 1)
            {
                mapMain.ZoomLevel = mapMain.ZoomLevel - 1;
            }
            
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mapMain.ZoomLevel = mapMain.ZoomLevel + 1;
            }
            catch (Exception)
            {

            }
            
        }

        private void menuSwitchRole_Click(object sender, EventArgs e)
        {

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

    public class Itinerary2
    {
        public int itinerary_id { get; set; }
        public int driver_id { get; set; }
        public int customer_id { get; set; }
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
        public BitmapImage link_avatar { get; set; }
        public int average_rating { get; set; }
    }

    public class RootObject
    {
        public bool error { get; set; }
        public List<Itinerary> itineraries { get; set; }
    }

    public class ItineraryList : List<Itinerary2>
    {

    }

}