using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
//using PhoneApp2.Resources;
using System.Device.Location; // Provides the GeoCoordinate class.
using Windows.Devices.Geolocation;
using System.Windows.Shapes;
using System.Windows.Media;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Windows.Input;
using System.Diagnostics;
using RideSharingWPApp.Map;
using RideSharingWPApp.UserControls;
using Windows.Web.Http;
using RideSharingWPApp.Request; //Provides the Geocoordinate class.

namespace RideSharingWPApp
{
    public partial class PostItinerary : PhoneApplicationPage
    {
        MapOverlay myCurentLocationOverlay = new MapOverlay(); //start point
        MapOverlay endPointOverlay = new MapOverlay();
        Geocoordinate myGeocoordinate = null;
        GeoCoordinate myGeoCoordinate = null;
        List<GeoCoordinate> wayPoints = new List<GeoCoordinate>();
        ReverseGeocodeQuery geoQ = null;
        MapLayer myLocationLayer = null;
        string nameOfTxtbox = "Start";

        bool isSetEndPoint = false;

        public PostItinerary()
        {
            InitializeComponent();
            mapPostItinerary.IsEnabled = false;
            //this.Cursor = Cursors.Wait;
            //RevCode for current Location and show on Start TextBox
            geoQ = new ReverseGeocodeQuery();
            geoQ.QueryCompleted += geoQ_QueryCompleted;
            if (geoQ.IsBusy == true)
            {
                geoQ.CancelAsync();
            }
            // Set the geo coordinate for the query


            InitCurrentLocationInfo();
            
        }

        public async void InitCurrentLocationInfo()
        {
            Task<GeoCoordinate> x = ShowMyCurrentLocationOnTheMap();
            geoQ.GeoCoordinate = await x;

            geoQ.QueryAsync();

            //txtboxStart.Text = showString;
        }

        private async Task<GeoCoordinate> ShowMyCurrentLocationOnTheMap()
        {
            // Get my current location.
            Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            myGeocoordinate = myGeoposition.Coordinate;

            wayPoints.Add(new GeoCoordinate(myGeocoordinate.Latitude, myGeocoordinate.Longitude));
            //GeoCoordinate myxGeocoordinate = new GeoCoordinate(47.6785619, -122.1311156);

            myGeoCoordinate =
                CoordinateConverter.ConvertGeocoordinate(myGeocoordinate);

            // Make my current location the center of the Map.
            this.mapPostItinerary.Center = myGeoCoordinate;
            this.mapPostItinerary.ZoomLevel = 16;


            // Create a MapOverlay to contain the circle.
            myCurentLocationOverlay = MarkerDraw.DrawMapMarker(myGeoCoordinate);

            // Create a MapLayer to contain the MapOverlay.
            myLocationLayer = new MapLayer();
            myLocationLayer.Add(myCurentLocationOverlay);

            // Add the MapLayer to the Map.
            mapPostItinerary.Layers.Add(myLocationLayer);

            return myGeoCoordinate;
        }

        public void RevCoding(GeoCoordinate point)
        {

        }

        

        void geoQ_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            //Debug.WriteLine("Geo query, error: " + e.Error);
            //Debug.WriteLine("Geo query, cancelled: " + e.Cancelled);
            //Debug.WriteLine("Geo query, cancelled: " + e.UserState.ToString());
            //Debug.WriteLine("Geo query, Result.Count(): " + e.Result.Count());


            if (e.Result.Count() > 0)
            {
                string showString = e.Result[0].Information.Name;
                showString = showString + "";
                showString = showString + "" + e.Result[0].Information.Address.HouseNumber + " " + e.Result[0].Information.Address.Street;
                showString = showString + "" + e.Result[0].Information.Address.PostalCode + " " + e.Result[0].Information.Address.City;
                showString = showString + "" + e.Result[0].Information.Address.Country + " " + e.Result[0].Information.Address.CountryCode;
                //showString = showString + "\nDescription: ";
                //showString = showString + "\n" + e.Result[0].Information.Description.ToString();

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
                //txtboxStart.Text = showString;
                //return showString;
                
            }
            //this.Cursor = Cursors.None;
            //return "null";
            mapPostItinerary.IsEnabled = true;
        }

        private void mapPostItinerary_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            if (endPointOverlay != null)
            {
                myLocationLayer.Remove(endPointOverlay);
            }
            GeoCoordinate asd = this.mapPostItinerary.ConvertViewportPointToGeoCoordinate(e.GetPosition(this.mapPostItinerary));
            //MessageBox.Show("lat: " + asd.Latitude + "; long: " + asd.Longitude);

            //dat pushpin
            endPointOverlay = MarkerDraw.DrawMapMarker(asd);
            // Create a MapLayer to contain the MapOverlay.
            myLocationLayer.Add(endPointOverlay);

            // Add the MapLayer to the Map.
            mapPostItinerary.Layers.Remove(myLocationLayer);
            mapPostItinerary.Layers.Add(myLocationLayer);

            //mapPostItinerary.Layers.Remove()
            //hien thi thong tin diem den tren textbox
            geoQ.GeoCoordinate = asd;

            geoQ.QueryAsync();

        }

        private async void txtboxStart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                string returnString = await Request.RequestToBingMap.sendGeoCodingRequest(txtboxStart.Text.Trim());
            }
        }

        private async void txtboxEnd_KeyDown(object sender, KeyEventArgs e)
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
                    myLocationLayer.Remove(endPointOverlay);
                }
                //dat pushpin
                endPointOverlay = MarkerDraw.DrawMapMarker(new GeoCoordinate(Convert.ToDouble(xlat), Convert.ToDouble(xlong)));
                // Create a MapLayer to contain the MapOverlay.
                myLocationLayer.Add(endPointOverlay);

                // Add the MapLayer to the Map.
                mapPostItinerary.Layers.Remove(myLocationLayer);
                mapPostItinerary.Layers.Add(myLocationLayer);

                this.Focus();
            }
        }

        private void btnXn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AdvancePostItinerary.xaml?start=" + txtboxStart.Text 
                + "&end=" + txtboxEnd.Text + "&s_lat=" + myCurentLocationOverlay.GeoCoordinate.Latitude
                + "&s_long=" + myCurentLocationOverlay.GeoCoordinate.Longitude
                + "&e_lat=" + endPointOverlay.GeoCoordinate.Latitude
                + "&e_long=" + endPointOverlay.GeoCoordinate.Longitude, UriKind.Relative));
        }


    }
}

/*private async void ShowMyCurrentLocationOnTheMap()
        {
            // Get my current location.
            Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync();
            myGeocoordinate = myGeoposition.Coordinate;

            wayPoints.Add(new GeoCoordinate(myGeocoordinate.Latitude, myGeocoordinate.Longitude));
            //GeoCoordinate myxGeocoordinate = new GeoCoordinate(47.6785619, -122.1311156);

            myGeoCoordinate =
                CoordinateConverter.ConvertGeocoordinate(myGeocoordinate);

            // Make my current location the center of the Map.
            this.mapPostItinerary.Center = myGeoCoordinate;
            this.mapPostItinerary.ZoomLevel = 16;

            
            // Create a MapOverlay to contain the circle.
            myCurentLocationOverlay = DrawMapMarker(myGeoCoordinate);

            // Create a MapLayer to contain the MapOverlay.
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myCurentLocationOverlay);

            // Add the MapLayer to the Map.
            mapPostItinerary.Layers.Add(myLocationLayer);

            //RevCode for current Location and show on Start TextBox
            geoQ = new ReverseGeocodeQuery();
            geoQ.QueryCompleted += geoQ_QueryCompleted;
            if (geoQ.IsBusy == true)
            {
                geoQ.CancelAsync();
            }
            // Set the geo coordinate for the query
            geoQ.GeoCoordinate = myGeoCoordinate;

            geoQ.QueryAsync();
        }*/