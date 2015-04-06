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
        string nameOfTxtbox = "Start";
        public DriverItineraryDetails()
        {
            InitializeComponent();

            //hanh trinh chua ai dang ki
            if (GlobalData.selectedItinerary.status.Equals(1))
            {
                //create button huy hanh trinh
            }
            //dang doi driver accept
            else if (GlobalData.selectedItinerary.status.Equals(2))
            {
                //tao button accept va button huy customer accept
            }
            //driver da accepted
            else if (GlobalData.selectedItinerary.status.Equals(3))
            {
                // 
            }
            //hanh trinh da ket thuc
            else if (GlobalData.selectedItinerary.status.Equals(4))
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

            txtbDistance.Text = GlobalData.selectedItinerary.distance.ToString();
            txtbDescription.Text = GlobalData.selectedItinerary.description;
            txtbCost.Text = GlobalData.selectedItinerary.cost;

            //xu ly ngay thang
            //datePicker.Value = GlobalData.selectedItinerary.da

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
                endPointOverlay = MarkerDraw.DrawMapMarker(new GeoCoordinate(Convert.ToDouble(xlat), Convert.ToDouble(xlong)));
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
                startPointOverlay = MarkerDraw.DrawMapMarker(new GeoCoordinate(Convert.ToDouble(xlat), Convert.ToDouble(xlong)));
                // Create a MapLayer to contain the MapOverlay.
                mapLayer.Add(startPointOverlay);

                // Add the MapLayer to the Map.
                mapItineraryDetails.Layers.Remove(mapLayer);
                mapItineraryDetails.Layers.Add(mapLayer);

                this.Focus();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //delete itinerary
            var result = Request.RequestToServer.sendDeleteRequest("itinerary/"+ GlobalData.selectedItinerary.itinerary_id);
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
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
            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);
            Request.RequestToServer.sendPutRequest("itinerary/" + GlobalData.selectedItinerary.itinerary_id, content);
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
            endPointOverlay = MarkerDraw.DrawMapMarker(asd);
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
    }
}