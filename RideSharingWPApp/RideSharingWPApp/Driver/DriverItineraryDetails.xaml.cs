using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RideSharingWPApp.Global;
using Microsoft.Phone.Maps.Controls;
using Windows.Devices.Geolocation;
using System.Device.Location;
using Microsoft.Phone.Maps.Services;
using System.Windows.Input;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RideSharingWPApp.UserControls;
using Windows.Web.Http;

namespace RideSharingWPApp.Driver
{
    public partial class DriverItineraryDetails : PhoneApplicationPage
    {
        MapOverlay startPointOverlay = new MapOverlay();
        MapOverlay endPointOverlay = new MapOverlay();
        MapLayer mapLayer = new MapLayer();
        Geocoordinate myGeocoordinate = null;
        GeoCoordinate myGeoCoordinate = null;
        ReverseGeocodeQuery geoQ = null;

        RouteQuery routeQuery = null;
        List<GeoCoordinate> wayPoints = new List<GeoCoordinate>();
        string nameOfTxtbox = "Start";

        public DriverItineraryDetails()
        {
            InitializeComponent();

            //hanh trinh chua ai dang ki
            if (GlobalData.selectedItinerary.status.Equals(Global.GlobalData.ITINERARY_STATUS_CREATED))
            {
                //create button update hanh trinh
                Button btnUpdate = new Button();
                btnUpdate.Content = "Cập Nhật";
                btnUpdate.Click += btnUpdate_Click;
                gridInfo.Children.Add(btnUpdate);
                Grid.SetRow(btnUpdate, 5);

                //create button huy hanh trinh
                Button btnDelete = new Button();
                btnDelete.Content = "Xóa";
                btnDelete.Click += btnDelete_Click;
                gridInfo.Children.Add(btnDelete);
                Grid.SetRow(btnDelete, 6);
            }
            //dang doi driver accept
            else if (GlobalData.selectedItinerary.status.Equals(Global.GlobalData.ITINERARY_STATUS_CUSTOMER_ACCEPTED))
            {
                //tao button accept va button huy customer accept
                //create button accept hanh trinh
                Button btnAccept = new Button();
                btnAccept.Content = "Chấp Nhận";
                btnAccept.Click += btnAccept_Click;
                gridInfo.Children.Add(btnAccept);
                Grid.SetRow(btnAccept, 5);

                //create button reject hanh trinh
                Button btnReject = new Button();
                btnReject.Content = "Từ Chối";
                btnReject.Click += btnReject_Click;
                gridInfo.Children.Add(btnReject);
                Grid.SetRow(btnReject, 6);
            }
            //driver da accepted
            else if (GlobalData.selectedItinerary.status.Equals(Global.GlobalData.ITINERARY_STATUS_DRIVER_ACCEPTED))
            {
                // 
            }
            //hanh trinh da ket thuc
            else if (GlobalData.selectedItinerary.status.Equals(Global.GlobalData.ITINERARY_STATUS_FINISHED))
            {

            }

            //draw 2 points on map
            startPointOverlay = UserControls.MarkerDraw.DrawCurrentMapMarker(new GeoCoordinate(GlobalData.selectedItinerary.start_address_lat, GlobalData.selectedItinerary.start_address_long));
            wayPoints.Add(new GeoCoordinate(GlobalData.selectedItinerary.start_address_lat, GlobalData.selectedItinerary.start_address_long));
            mapLayer.Add(startPointOverlay);
            
            endPointOverlay = UserControls.MarkerDraw.DrawCurrentMapMarker(new GeoCoordinate(GlobalData.selectedItinerary.end_address_lat, GlobalData.selectedItinerary.end_address_long));
            wayPoints.Add(new GeoCoordinate(GlobalData.selectedItinerary.end_address_lat, GlobalData.selectedItinerary.end_address_long));
            mapLayer.Add(endPointOverlay);
            
            //set zoom and center point
            mapItineraryDetails.ZoomLevel = 14;
            mapItineraryDetails.Center = startPointOverlay.GeoCoordinate;

            mapItineraryDetails.Layers.Add(mapLayer);

            //draw route
            routeQuery = new RouteQuery();
            //GeocodeQuery Mygeocodequery = null;
            routeQuery.QueryCompleted += routeQuery_QueryCompleted;
            routeQuery.TravelMode = TravelMode.Driving;
            routeQuery.RouteOptimization = RouteOptimization.MinimizeDistance;
            routeQuery.Waypoints = wayPoints;
            routeQuery.QueryAsync();

            //set text 2 points
            txtboxStart.Text = GlobalData.selectedItinerary.start_address;
            txtboxEnd.Text = GlobalData.selectedItinerary.end_address;

            //set text itinerary info

            txtbDistance.Text = GlobalData.selectedItinerary.distance.ToString();
            txtbDescription.Text = GlobalData.selectedItinerary.description;
            txtbCost.Text = GlobalData.selectedItinerary.cost;

            //xu ly ngay thang
            string datetimeString = GlobalData.selectedItinerary.leave_date.Trim();

            DateTime datetime = DateTimes.DatetimeConvert.convertDateTimeFromString(datetimeString);

            datePicker.Value = datetime;
            timePicker.Value = datetime;

            //datePicker.Value = GlobalData.selectedItinerary.da

        }

        void routeQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            if (null == e.Error)
            {
                Route MyRoute = e.Result;
                MapRoute MyMapRoute = new MapRoute(MyRoute);
                mapItineraryDetails.AddRoute(MyMapRoute);
                //length of route
                //time = route / v trung binh(hang so)
                //MessageBox.Show("Distance: " + MyMapRoute.Route.LengthInMeters.ToString());
                routeQuery.Dispose();
            }
        }

        void geoQ_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            if (e.Result.Count() > 0)
            {
                string showString = e.Result[0].Information.Name;
                showString = showString + "";
                showString = showString + "" + e.Result[0].Information.Address.HouseNumber + " " + e.Result[0].Information.Address.Street;
                showString = showString + "" + e.Result[0].Information.Address.PostalCode + " " + e.Result[0].Information.Address.City;
                showString = showString + "" + e.Result[0].Information.Address.Country + " " + e.Result[0].Information.Address.CountryCode;
                //MessageBox.Show(showString);
                if (nameOfTxtbox.Equals("Start"))
                {
                    txtboxStart.Text = showString;
                    nameOfTxtbox = "End";
                }
                else
                {
                    txtboxEnd.Text = showString;
                }

            }
            mapItineraryDetails.IsEnabled = true;
        }

        public async void txtboxEnd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                Task<string> returnString = Request.RequestToBingMap.sendGeoCodingRequest(txtboxEnd.Text.Trim());

                //handle json return to get lat & long
                JObject jsonObject = JObject.Parse(await returnString);

                string xlong = jsonObject.SelectToken("resourceSets[0].resources[0].point.coordinates[1]").ToString().Trim();
                string xlat = jsonObject.SelectToken("resourceSets[0].resources[0].point.coordinates[0]").ToString().Trim();

                //set marker again

                if (endPointOverlay != null)
                {
                    mapLayer.Remove(endPointOverlay);
                }
                //dat pushpin
                endPointOverlay = MarkerDraw.DrawCurrentMapMarker(new GeoCoordinate(Convert.ToDouble(xlat), Convert.ToDouble(xlong)));
                // Create a MapLayer to contain the MapOverlay.
                mapLayer.Add(endPointOverlay);

                // Add the MapLayer to the Map.
                mapItineraryDetails.Layers.Remove(mapLayer);
                mapItineraryDetails.Layers.Add(mapLayer);

                this.Focus();
            }
        }

        public async void txtboxStart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                Task<string> returnString = Request.RequestToBingMap.sendGeoCodingRequest(txtboxStart.Text.Trim());

                //handle json return to get lat & long
                JObject jsonObject = JObject.Parse(await returnString);

                string xlong = jsonObject.SelectToken("resourceSets[0].resources[0].point.coordinates[1]").ToString().Trim();
                string xlat = jsonObject.SelectToken("resourceSets[0].resources[0].point.coordinates[0]").ToString().Trim();

                //set marker again

                if (startPointOverlay != null)
                {
                    mapLayer.Remove(startPointOverlay);
                }
                //dat pushpin
                startPointOverlay = MarkerDraw.DrawCurrentMapMarker(new GeoCoordinate(Convert.ToDouble(xlat), Convert.ToDouble(xlong)));
                // Create a MapLayer to contain the MapOverlay.
                mapLayer.Add(startPointOverlay);

                // Add the MapLayer to the Map.
                mapItineraryDetails.Layers.Remove(mapLayer);
                mapItineraryDetails.Layers.Add(mapLayer);

                this.Focus();
            }
        }

        private async void btnReject_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);
            var result = await Request.RequestToServer.sendPutRequest("driver_reject_itinerary/" + GlobalData.selectedItinerary.itinerary_id, content);
            JObject jsonObject = JObject.Parse(result);
            MessageBox.Show(jsonObject.Value<string>("message"));
            //do something


        }

        private async void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);
            var result = await Request.RequestToServer.sendPutRequest("driver_accept_itinerary/" +GlobalData.selectedItinerary.itinerary_id, content);
            JObject jsonObject = JObject.Parse(result);
            MessageBox.Show(jsonObject.Value<string>("message"));
            //do something

        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //delete itinerary
            var result = await Request.RequestToServer.sendDeleteRequest("itinerary/"+ GlobalData.selectedItinerary.itinerary_id);
            JObject jsonObject = JObject.Parse(result);
            MessageBox.Show(jsonObject.Value<string>("message"));
            NavigationService.RemoveBackEntry();
            NavigationService.Navigate(new Uri("/Driver/ItineraryManagement.xaml", UriKind.RelativeOrAbsolute));
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //update itinerary
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("start_address", txtboxStart.Text.Trim());
            postData.Add("end_address", txtboxEnd.Text.Trim());
            postData.Add("description", txtbDescription.Text.Trim());
            postData.Add("cost", txtbCost.Text.Trim());
            postData.Add("distance", txtbDistance.Text.Trim());
            postData.Add("start_address_lat", startPointOverlay.GeoCoordinate.Latitude.ToString().Trim());
            postData.Add("start_address_long", startPointOverlay.GeoCoordinate.Longitude.ToString().Trim());
            postData.Add("end_address_lat", endPointOverlay.GeoCoordinate.Latitude.ToString().Trim());
            postData.Add("end_address_long", endPointOverlay.GeoCoordinate.Longitude.ToString().Trim());

            //string date = datePicker.Value.Value.ToString().Substring(0,10).Trim();
            //string time = timePicker.Value.Value.ToUniversalTime().ToString();


            string date2 = datePicker.Value.Value.Year + "-" + datePicker.Value.Value.Month + "-" + datePicker.Value.Value.Day;
            string time2 = timePicker.Value.Value.Hour + ":" + timePicker.Value.Value.Minute +":00";

            postData.Add("leave_date", date2.Trim() + " " + time2.Trim());
            //datePicker.Value.Value.
            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);
            var result = await Request.RequestToServer.sendPutRequest("itinerary/" + GlobalData.selectedItinerary.itinerary_id, content);
            JObject jsonObject = JObject.Parse(result);
            MessageBox.Show(jsonObject.Value<string>("message"));

        }

        private void mapItineraryDetails_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (endPointOverlay != null)
            {
                mapLayer.Remove(endPointOverlay);
            }
            GeoCoordinate asd = this.mapItineraryDetails.ConvertViewportPointToGeoCoordinate(e.GetPosition(this.mapItineraryDetails));
            //MessageBox.Show("lat: " + asd.Latitude + "; long: " + asd.Longitude);

            //dat pushpin
            endPointOverlay = MarkerDraw.DrawCurrentMapMarker(asd);
            // Create a MapLayer to contain the MapOverlay.
            mapLayer.Add(endPointOverlay);

            // Add the MapLayer to the Map.
            mapItineraryDetails.Layers.Remove(mapLayer);
            mapItineraryDetails.Layers.Add(mapLayer);

            //mapPostItinerary.Layers.Remove()
            //hien thi thong tin diem den tren textbox
            geoQ.GeoCoordinate = asd;

            geoQ.QueryAsync();
        }

        private void menuSearch_Click(object sender, EventArgs e)
        {

        }
        private void menuHome_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Driver/DriverMainMap.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menuPostItinerary_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Driver/PostItinerary.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menuItineraryManagement_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Driver/ItineraryManagement.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menuAccountInfo_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AccountInfo.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnZoomIn_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnZoomOut_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void menuAboutUs_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutUs.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menuLogOut_Click(object sender, EventArgs e)
        {
            //delete UserInfo Before Logout
            Global.GlobalData.deleteUserInfoBeforeLogout();

            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (mapItineraryDetails.ZoomLevel > 1)
            {
                mapItineraryDetails.ZoomLevel = mapItineraryDetails.ZoomLevel - 1;
            }
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            mapItineraryDetails.ZoomLevel = mapItineraryDetails.ZoomLevel + 1;
        }
    }   
}