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
            //check tinh trang hanh trinh
            if (GlobalData.selectedItinerary.status.Equals(null))
            {

            }
            else
            {

            }
            MapOverlay overlay = new MapOverlay();
            //set point on map
            //draw start poit
            overlay = UserControls.MarkerDraw.DrawMapMarker(new GeoCoordinate(GlobalData.selectedItinerary.start_address_lat, GlobalData.selectedItinerary.start_address_long));
            wayPoints.Add(new GeoCoordinate(GlobalData.selectedItinerary.start_address_lat, GlobalData.selectedItinerary.start_address_long));
            layer.Add(overlay);
            //draw end point
            overlay = UserControls.MarkerDraw.DrawMapMarker(new GeoCoordinate(GlobalData.selectedItinerary.end_address_lat, GlobalData.selectedItinerary.end_address_long));
            wayPoints.Add(new GeoCoordinate(GlobalData.selectedItinerary.end_address_lat, GlobalData.selectedItinerary.end_address_long));
            layer.Add(overlay);

            mapItineraryDetails.Layers.Add(layer);
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

        } 
    }
}