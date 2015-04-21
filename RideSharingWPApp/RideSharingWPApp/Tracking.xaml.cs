using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using RideSharingWPApp.Map;
using RideSharingWPApp.UserControls;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using Microsoft.AspNet.SignalR.Client;
using System.Net.Http;

namespace RideSharingWPApp
{
    public partial class Tracking : PhoneApplicationPage
    {
        Geocoordinate myGeocoordinate = null;
        GeoCoordinate myGeoCoordinate = null;

        Geolocator myLocator = null;
        private IHubProxy HubProxy { get; set; }
        const string ServerURI = "http://52.11.206.209:8080/signalr";
        private HubConnection con { get; set; }

        MapLayer mainMapLayer = new MapLayer();
        MapOverlay overlay = new MapOverlay();

        public Tracking()
        {
            InitializeComponent();

            InitCurrentLocationInfo();

            ConnectAsync();
        }

        private async void ConnectAsync()
        {
            con = new HubConnection(ServerURI);
            con.Closed += Connection_Closed;
            con.Error += Connection_Error;
            HubProxy = con.CreateHubProxy("ChatHub");
            //Handle incoming event from server: use Invoke to write to console from SignalR's thread
            HubProxy.On<string>("getPos", (message) =>
                Dispatcher.BeginInvoke(() => test(message))
            );
            try
            {
                await con.Start();
            }
            catch (HttpRequestException)
            {
                //No connection: Don't enable Send button or show chat UI
                txtFireBase.Text = "eror";
            }
            catch (HttpClientException)
            {
                txtFireBase.Text = "eror";
            }

            Dispatcher.BeginInvoke(() =>
            {
                HubProxy.Invoke("Connect", "15");
            });
        }

        private void test(string message)
        {
            string[] latlng = message.Split(",".ToCharArray());
            double lat = Double.Parse(latlng[0]);
            double lng = Double.Parse(latlng[1]);
            addMarkertoMap(new GeoCoordinate(lat, lng));
            txtFireBase.Text = message;
        }

        private void Connection_Error(Exception obj)
        {
            txtFireBase.Text = "error";
        }

        /// <summary>
        /// If the server is stopped, the connection will time out after 30 seconds (default), and the 
        /// Closed event will fire.
        /// </summary>
        private void Connection_Closed()
        {
            //Deactivate chat UI; show login UI. 
        }



        public async void InitCurrentLocationInfo()
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

        public void addMarkertoMap(GeoCoordinate point)
        {
            mapMain.Layers.Remove(mainMapLayer);
            mainMapLayer = new MapLayer();
            overlay = MarkerDraw.DrawItineraryMarker(point, new Itinerary2());
            mainMapLayer.Add(overlay);
            
            mapMain.Layers.Add(mainMapLayer);
        }

        public void removeMarkertoMap()
        {
            //mainMapLayer.Remove(overlay);
        }

        private void btntrack_Click(object sender, RoutedEventArgs e)
        {
            myLocator = new Geolocator();
            myLocator.DesiredAccuracy = PositionAccuracy.High;
            myLocator.MovementThreshold = 5;
            myLocator.ReportInterval = 500;
            myLocator.PositionChanged += myGeoLocator_PositionChanged;
        }

        private void myGeoLocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args1)
        {
            Dispatcher.BeginInvoke(() =>
            {
                HubProxy.Invoke("SendPos", "14", args1.Position.Coordinate.Latitude.ToString() + "," + args1.Position.Coordinate.Longitude.ToString());
            });
        }
    }
}