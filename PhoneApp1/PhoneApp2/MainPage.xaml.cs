using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneApp2.Resources;
using System.Device.Location; // Provides the GeoCoordinate class.
using Windows.Devices.Geolocation;
using System.Windows.Shapes;
using System.Windows.Media;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json.Linq; //Provides the Geocoordinate class.

namespace PhoneApp2
{
    public partial class MainPage : PhoneApplicationPage
    {
        private List<GeoCoordinate> MyCoordinates = new List<GeoCoordinate>();
        //BingGeocoder.BingGeocoderClient b = new BingGeocoder.BingGeocoderClient("Ajze-B_0BaOUYxiJ0Hizj6wnyAnyRDPI5jfvDa1J7zkrCQZz2GNZkIigjLhi__nM");
        List<GeoCoordinate> wayPoints = new List<GeoCoordinate>();
        RouteQuery routeQuery = null;
        private bool _isRouteSearch = false;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            ShowMyLocationOnTheMap();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        RouteQuery MyQuery = null;
        GeocodeQuery Mygeocodequery = null;
        Geocoordinate myGeocoordinate = null;

        private async void ShowMyLocationOnTheMap()
        {
            // Get my current location.
            Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            myGeocoordinate = myGeoposition.Coordinate;

            wayPoints.Add(new GeoCoordinate(myGeocoordinate.Latitude, myGeocoordinate.Longitude));
            //GeoCoordinate myxGeocoordinate = new GeoCoordinate(47.6785619, -122.1311156);

            GeoCoordinate myGeoCoordinate =
            CoordinateConverter.ConvertGeocoordinate(myGeocoordinate);

            // Make my current location the center of the Map.
            this.mapWithMyLocation.Center = myGeoCoordinate;
            this.mapWithMyLocation.ZoomLevel = 16;

            //draw a marker
            Ellipse myCircle = new Ellipse();
            myCircle.Fill = new SolidColorBrush(Colors.Blue);
            myCircle.Height = 20;
            myCircle.Width = 20;
            myCircle.Opacity = 50;

            //draw trong suot marker
            double metersPerPixels = (Math.Cos(myGeoCoordinate.Latitude * Math.PI / 180) * 2 * Math.PI * 6378137) / (256 * Math.Pow(2, mapWithMyLocation.ZoomLevel));
            double radius = 40 / metersPerPixels;

            Ellipse ellipse = new Ellipse();
            ellipse.Width = radius * 2;
            ellipse.Height = radius * 2;
            ellipse.Fill = new SolidColorBrush(Color.FromArgb(75, 200, 0, 0));

            // Create a MapOverlay to contain the circle.
            MapOverlay myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = myCircle;
            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay.GeoCoordinate = myGeoCoordinate;

            // Create a MapLayer to contain the MapOverlay.
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myLocationOverlay);

            // Add the MapLayer to the Map.
            mapWithMyLocation.Layers.Add(myLocationLayer);
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            

            Mygeocodequery = new GeocodeQuery();
            Mygeocodequery.SearchTerm = txtbSearch.Text.Trim();
            Mygeocodequery.GeoCoordinate = new GeoCoordinate(0, 0);
            //Mygeocodequery.QueryCompleted += Mygeocodequery_QueryCompleted;
            //Mygeocodequery.QueryAsync();

            
            var result = await RequestToServer.sendGetRequest("12 le duan da nang");

            //handle json return to get lat & long
            JObject jsonObject = JObject.Parse(result);

            string xlong = jsonObject.SelectToken("resourceSets[0].resources[0].point.coordinates[1]").ToString();
            string xlat = jsonObject.SelectToken("resourceSets[0].resources[0].point.coordinates[0]").ToString();
            

            //draw trong suot marker
            double metersPerPixels = (Math.Cos(Convert.ToDouble(xlat) * Math.PI / 180) * 2 * Math.PI * 6378137) / (256 * Math.Pow(2, mapWithMyLocation.ZoomLevel));
            double radius = 40 / metersPerPixels;

            Ellipse ellipse = new Ellipse();
            ellipse.Width = radius * 2;
            ellipse.Height = radius * 2;
            ellipse.Fill = new SolidColorBrush(Color.FromArgb(75, 200, 0, 0));

            // Create a MapOverlay to contain the circle.
            MapOverlay myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = ellipse;
            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay.GeoCoordinate = new GeoCoordinate(Convert.ToDouble(xlat), Convert.ToDouble(xlong));

            wayPoints.Add(myLocationOverlay.GeoCoordinate);
            // Create a MapLayer to contain the MapOverlay.
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myLocationOverlay);

            // Add the MapLayer to the Map.
            //mapWithMyLocation.Layers.Remove();
            mapWithMyLocation.Layers.Add(myLocationLayer);

            this.mapWithMyLocation.Center = new GeoCoordinate(Convert.ToDouble(xlat), Convert.ToDouble(xlong));
            this.mapWithMyLocation.ZoomLevel = 10;




            routeQuery = new RouteQuery();
            //GeocodeQuery Mygeocodequery = null;

            routeQuery.QueryCompleted += routeQuery_QueryCompleted;
            routeQuery.TravelMode = TravelMode.Walking;
            routeQuery.Waypoints = wayPoints;
            routeQuery.QueryAsync();

            
        }

        void routeQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            if (null == e.Error)
            {
                Route MyRoute = e.Result;
                MapRoute MyMapRoute = new MapRoute(MyRoute);
                mapWithMyLocation.AddRoute(MyMapRoute);

                //length of route
                MessageBox.Show(MyMapRoute.Route.LengthInMeters.ToString());
                routeQuery.Dispose();
            }
        }

        void Mygeocodequery_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            if (e.Error == null)
            {
                 if (e.Result.Count > 0)
                 {
                     if (_isRouteSearch) // Query is made to locate the destination of a route
                     {
                         
                     }
                     else // Query is made to search the map for a keyword
                     {
                         // Add all results to MyCoordinates for drawing the map markers
                         MyCoordinates = new List<GeoCoordinate>();
                         for (int i = 0; i < e.Result.Count; i++)
                         {
                             MyCoordinates.Add(e.Result[i].GeoCoordinate);




                             
                         }

                         //draw trong suot marker
                         double metersPerPixels = (Math.Cos(MyCoordinates[0].Latitude * Math.PI / 180) * 2 * Math.PI * 6378137) / (256 * Math.Pow(2, mapWithMyLocation.ZoomLevel));
                         double radius = 40 / metersPerPixels;

                         Ellipse ellipse = new Ellipse();
                         ellipse.Width = radius * 2;
                         ellipse.Height = radius * 2;
                         ellipse.Fill = new SolidColorBrush(Color.FromArgb(75, 200, 0, 0));

                         // Create a MapOverlay to contain the circle.
                         MapOverlay myLocationOverlay = new MapOverlay();
                         myLocationOverlay.Content = ellipse;
                         myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
                         myLocationOverlay.GeoCoordinate = MyCoordinates[0];

                         // Create a MapLayer to contain the MapOverlay.
                         MapLayer myLocationLayer = new MapLayer();
                         myLocationLayer.Add(myLocationOverlay);

                         // Add the MapLayer to the Map.
                         //mapWithMyLocation.Layers.Remove();
                         mapWithMyLocation.Layers.Add(myLocationLayer);

                         this.mapWithMyLocation.Center = MyCoordinates[0];
                         this.mapWithMyLocation.ZoomLevel = 10;
                         
                     }
                 }
                 else
                 {
                     MessageBox.Show("No match found. Narrow your search e.g. Seattle WA.");
                 }
            }
        }

        void MyQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            if (e.Error == null)
            {
                Route MyRoute = e.Result;
                MapRoute MyMapRoute = new MapRoute(MyRoute);
                //MyMap.AddRoute(MyMapRoute);
                MyQuery.Dispose();
            }
        }

        private void mapWithMyLocation_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            GeoCoordinate asd = this.mapWithMyLocation.ConvertViewportPointToGeoCoordinate(e.GetPosition(this.mapWithMyLocation));
            MessageBox.Show("lat: " + asd.Latitude + "; long: " + asd.Longitude);
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}