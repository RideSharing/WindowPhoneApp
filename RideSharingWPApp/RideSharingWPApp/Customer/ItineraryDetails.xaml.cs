using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Device.Location;
using RideSharingWPApp.Global;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using Windows.Web.Http;
using Newtonsoft.Json.Linq;

namespace RideSharingWPApp
{
    public partial class ItineraryDetails : PhoneApplicationPage
    {
        MapLayer layer = new MapLayer();
        RouteQuery routeQuery = null;
        List<GeoCoordinate> wayPoints = new List<GeoCoordinate>();
        public ItineraryDetails()
        {
            InitializeComponent();
            //check status cua customer
            if (GlobalData.customer_status.Equals(2))
            {
                //check tinh trang hanh trinh
                //hanh trinh chua ai dang ki
                if (GlobalData.selectedItinerary.status.Equals(1))
                {
                    //create button register itinerary
                    Button btnAcceptItinerary = new Button();
                    btnAcceptItinerary.Content = "Accept Itinerary";
                    btnAcceptItinerary.Click += btnAcceptItinerary_Click;
                    gridMap.Children.Add(btnAcceptItinerary);
                    Grid.SetRow(btnAcceptItinerary, 1);
                }
                //dang doi driver accept
                else if (GlobalData.selectedItinerary.status.Equals(2))
                {
                    //tao trang thai dang doi accept, tao nut huy hanh trinh
                    Button btnCancelItinerary = new Button();
                    btnCancelItinerary.Content = "Cancel Itinerary";
                    btnCancelItinerary.Click += btnCancelItinerary_Click;
                    gridMap.Children.Add(btnCancelItinerary);
                    Grid.SetRow(btnCancelItinerary, 1);
                }
                //driver da accepted
                else if (GlobalData.selectedItinerary.status.Equals(3))
                {
                    //tao trang thai da duoc accept, tao nut 
                }
                //hanh trinh da ket thuc
                else if (GlobalData.selectedItinerary.status.Equals(4))
                {

                }
            }
            else if (GlobalData.customer_status.Equals(1))
            {

            }
            else
            {

            }

            MapOverlay overlay = new MapOverlay();
            //set point on map
            //draw start poit
            overlay = UserControls.MarkerDraw.DrawCurrentMapMarker(new GeoCoordinate(GlobalData.selectedItinerary.start_address_lat, GlobalData.selectedItinerary.start_address_long));
            
            wayPoints.Add(new GeoCoordinate(GlobalData.selectedItinerary.start_address_lat, GlobalData.selectedItinerary.start_address_long));

            mapItineraryDetails.Center = overlay.GeoCoordinate;
            layer.Add(overlay);
            //draw end point
            overlay = UserControls.MarkerDraw.DrawCurrentMapMarker(new GeoCoordinate(GlobalData.selectedItinerary.end_address_lat, GlobalData.selectedItinerary.end_address_long));
            wayPoints.Add(new GeoCoordinate(GlobalData.selectedItinerary.end_address_lat, GlobalData.selectedItinerary.end_address_long));
            layer.Add(overlay);
            mapItineraryDetails.Layers.Add(layer);

            mapItineraryDetails.ZoomLevel = 14;
            

            //driection
            routeQuery = new RouteQuery();
            //GeocodeQuery Mygeocodequery = null;
            routeQuery.QueryCompleted += routeQuery_QueryCompleted;
            routeQuery.TravelMode = TravelMode.Driving;
            routeQuery.RouteOptimization = RouteOptimization.MinimizeDistance;
            routeQuery.Waypoints = wayPoints;
            routeQuery.QueryAsync();
            //set itinerary info
            txtDescription.Text = GlobalData.selectedItinerary.description;
            txtStartAddress.Text = GlobalData.selectedItinerary.start_address;
            txtEndAddress.Text = GlobalData.selectedItinerary.end_address;
            txtCost.Text = GlobalData.selectedItinerary.cost;
            txtPhone.Text = GlobalData.selectedItinerary.phone;
            txtLeaveDay.Text = GlobalData.selectedItinerary.leave_date;
            txtDistance.Text = GlobalData.selectedItinerary.distance.ToString();
            txtDuration.Text = GlobalData.selectedItinerary.duration.ToString();
        }

        private void btnAcceptItinerary_Click(object sender, RoutedEventArgs e)
        {
            acceptItinerary();
        }

        private void btnCancelItinerary_Click(object sender, RoutedEventArgs e)
        {
            cancelItinerary();
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

        private void btnAccept2_Click(object sender, RoutedEventArgs e)
        {
            acceptItinerary();
        }

        private void btnAccept1_Click(object sender, RoutedEventArgs e)
        {
            acceptItinerary();
        }

        public async void acceptItinerary()
        {
            //accept itinerary
            Dictionary<string, string> postData = new Dictionary<string, string>();
            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);
            var result = await Request.RequestToServer.sendPutRequest("customer_accept_itinerary/" + GlobalData.selectedItinerary.itinerary_id, content);

            JObject jsonObject = JObject.Parse(result);
            MessageBox.Show(jsonObject.Value<string>("message"));
        }

        public async void cancelItinerary()
        {
            //rejec itinerary
            Dictionary<string, string> postData = new Dictionary<string, string>();
            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);
            var result = await Request.RequestToServer.sendPutRequest("customer_reject_itinerary/" + GlobalData.selectedItinerary.itinerary_id, content);
            JObject jsonObject = JObject.Parse(result);
            MessageBox.Show(jsonObject.Value<string>("message"));
        }

        private void menuSearch_Click(object sender, EventArgs e)
        {

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
    }
}