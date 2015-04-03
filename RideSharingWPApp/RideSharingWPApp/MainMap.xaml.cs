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

namespace RideSharingWPApp
{
    public partial class MainMap : PhoneApplicationPage
    {
        MapLayer mainMapLayer = new MapLayer();
        List<MapOverlay> listMainMapOvelay = new List<MapOverlay>();
        RootObject root = null;
        List<Itinerary> itineraries = null;
        public MainMap()
        {
            InitializeComponent();

            //set current location

            //get itinerary
            getItinerary();
                //send get request
            

                //xu ly json


                //hien thi tren map


                //hien thi tren list
            //
        }

        public async void getItinerary()
        {
            var result = await Request.RequestToServer.sendGetRequest("itineraries");

            JObject jsonObject = JObject.Parse(result);


            string APIkey = jsonObject.Value<string>("error").Trim();
            
            //var xlong = jsonObject.SelectToken("itineraries");
            JArray jsonVal = (JArray)jsonObject.SelectToken("itineraries");
            var kk = jsonVal;

            root =
            JsonConvert.DeserializeObject<RootObject>(result);
            
            
            string z = root.error.ToString();
            itineraries = root.itineraries;
            foreach (Itinerary i in root.itineraries)
            {
                MapOverlay overlay = new MapOverlay();
                overlay = DrawItineraryMarker(new GeoCoordinate(Convert.ToDouble(i.start_address_lat), 
                    Convert.ToDouble(i.start_address_long)),i);
                //chua su dung
                listMainMapOvelay.Add(overlay);

                mainMapLayer.Add(overlay);
            }

            mapRecent.Layers.Add(mainMapLayer);

            longlistItineraries.ItemsSource = itineraries;
            
        }


        public MapOverlay DrawItineraryMarker(GeoCoordinate point, Itinerary i)
        {
            //MapVieMode.Layers.Clear();
            //MapLayer mapLayer = new MapLayer();
            // Draw marker for current position       

            // Draw markers for location(s) / destination(s)


            //DrawMapMarker(MyCoordinates[i], Colors.Red, mapLayer, parklist.parking_details[i].DestinationName);
            UCCustomPushPin _tooltip = new UCCustomPushPin();
            _tooltip.Description = "";
            _tooltip.DataContext = itineraries;
            _tooltip.Menuitem.Click += Menuitem_Click;
            _tooltip.imgmarker.Tap += _tooltip_Tapimg;
            MapOverlay overlay = new MapOverlay();
            overlay.Content = _tooltip;
            overlay.GeoCoordinate = point;
            overlay.PositionOrigin = new Point(0.0, 1.0);

            return overlay;
            //mapLayer.Add(overlay);

            //MapVieMode.Layers.Add(mapLayer);
        }

        private void Menuitem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem item = (MenuItem)sender;
                string selecteditem = item.Tag.ToString().Trim();
                var selectedparkdata = itineraries.Where(s => s.itinerary_id.ToString().Equals(selecteditem)).ToList();
                if (selectedparkdata.Count > 0)
                {
                    foreach (var items in selectedparkdata)
                    {
                        //storage data
                        /*if (Settings.FileExists("LocationDetailItem"))
                        {
                            Settings.DeleteFile("LocationDetailItem");
                        }
                        using (IsolatedStorageFileStream fileStream = Settings.OpenFile("LocationDetailItem", FileMode.Create))
                        {
                            DataContractSerializer serializer = new DataContractSerializer(typeof(LocationDetail));
                            serializer.WriteObject(fileStream, items);

                        }*/

                        //navigate to Detail page
                        NavigationService.Navigate(new Uri("/ItineraryDetails.xaml", UriKind.Relative));
                        break;
                    }
                }
            }
            catch
            {
            }
        }

        private void _tooltip_Tapimg(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                Image item = (Image)sender;
                string selecteditem = item.Tag.ToString();
                var selectedparkdata = itineraries.Where(s => s.itinerary_id.ToString().Equals(selecteditem)).ToList();


                if (selectedparkdata.Count > 0)
                {
                    foreach (var items in selectedparkdata)
                    {
                        ContextMenu contextMenu =
                    ContextMenuService.GetContextMenu(item);
                        contextMenu.DataContext = items;
                        if (contextMenu.Parent == null)
                        {
                            contextMenu.IsOpen = true;

                        }
                        break;
                    }
                }
            }
            catch
            {
            }
        }

        private void longlistItineraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

    public class RootObject
    {
        public bool error { get; set; }
        public List<Itinerary> itineraries { get; set; }
    }

}