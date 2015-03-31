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
using RideSharingWPApp.UserControls; //Provides the Geocoordinate class.

namespace RideSharingWPApp
{
    public partial class PostItinerary : PhoneApplicationPage
    {
        MapOverlay myCurentLocationOverlay = new MapOverlay();
        Geocoordinate myGeocoordinate = null;
        List<GeoCoordinate> wayPoints = new List<GeoCoordinate>();

        public PostItinerary()
        {
            InitializeComponent();

            ShowMyCurrentLocationOnTheMap();

            //RevCode for current Location and show on Start TextBox
        }

        private async void ShowMyCurrentLocationOnTheMap()
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
            this.mapPostItinerary.Center = myGeoCoordinate;
            this.mapPostItinerary.ZoomLevel = 16;

            
            // Create a MapOverlay to contain the circle.
            myCurentLocationOverlay = DrawMapMarker(myGeoCoordinate);

            // Create a MapLayer to contain the MapOverlay.
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(myCurentLocationOverlay);

            // Add the MapLayer to the Map.
            mapPostItinerary.Layers.Add(myLocationLayer);
        }

        public void RevCoding(GeoCoordinate point)
        {

        }

        private MapOverlay DrawMapMarker(GeoCoordinate point)
        {
            //MapVieMode.Layers.Clear();
            //MapLayer mapLayer = new MapLayer();
            // Draw marker for current position       

            // Draw markers for location(s) / destination(s)
            

            //DrawMapMarker(MyCoordinates[i], Colors.Red, mapLayer, parklist.parking_details[i].DestinationName);
            UCCustomPushPin _tooltip = new UCCustomPushPin();
            _tooltip.Description = "";
            _tooltip.DataContext = "";
            //_tooltip.Menuitem.Click += Menuitem_Click;
            //_tooltip.imgmarker.Tap += _tooltip_Tapimg;
            MapOverlay overlay = new MapOverlay();
            overlay.Content = _tooltip;
            overlay.GeoCoordinate = point;
            overlay.PositionOrigin = new Point(0.0, 1.0);

            return overlay;
            //mapLayer.Add(overlay);
            
            //MapVieMode.Layers.Add(mapLayer);
        }
    }
}