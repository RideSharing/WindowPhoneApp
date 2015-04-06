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
        public DriverItineraryDetails()
        {
            InitializeComponent();

            //check status hanh trinh
            if (GlobalData.selectedItinerary.status.Equals(null))
            {

            }
            else
            {

            }

            //draw 2 points on map
            startPointOverlay = UserControls.MarkerDraw.DrawMapMarker(new GeoCoordinate(GlobalData.selectedItinerary.start_address_lat, GlobalData.selectedItinerary.start_address_long));
            mapLayer.Add(startPointOverlay);

            endPointOverlay = UserControls.MarkerDraw.DrawMapMarker(new GeoCoordinate(GlobalData.selectedItinerary.end_address_lat, GlobalData.selectedItinerary.end_address_long));
            mapLayer.Add(endPointOverlay);

            mapItineraryDetails.Layers.Add(mapLayer);

            //draw route

            //set text 2 points
            txtboxStart.Text = GlobalData.selectedItinerary.start_address;
            txtboxEnd.Text = GlobalData.selectedItinerary.end_address;

            //set text itinerary info



        }

        public void txtboxEnd_KeyDown(object sender, KeyEventArgs e)
        {

        }

        public void txtboxStart_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}