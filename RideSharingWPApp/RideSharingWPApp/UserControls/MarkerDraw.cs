using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace RideSharingWPApp.UserControls
{
    class MarkerDraw
    {
        public static MapOverlay DrawCurrentMapMarker(GeoCoordinate point)
        {
            //MapVieMode.Layers.Clear();
            //MapLayer mapLayer = new MapLayer();
            // Draw marker for current position       
            UCCurrentPushPin _tooltip = new UCCurrentPushPin();
            // Draw markers for location(s) / destination(s)

            //DrawMapMarker(MyCoordinates[i], Colors.Red, mapLayer, parklist.parking_details[i].DestinationName);
            
            _tooltip.Description = "";
            _tooltip.DataContext = "";
            //_tooltip.Menuitem.Click += Menuitem_Click;
            //_tooltip.imgmarker.Tap += _tooltip_Tapimg;
            MapOverlay overlay = new MapOverlay();
            overlay.Content = _tooltip;
            overlay.GeoCoordinate = point;
            overlay.PositionOrigin = new Point(0.0, 1.0);

            return overlay;
        }

        public static MapOverlay DrawItineraryMarker(GeoCoordinate point, Itinerary2 i)
        {
            //MapVieMode.Layers.Clear();
            //MapLayer mapLayer = new MapLayer();
            // Draw marker for current position       

            // Draw markers for location(s) / destination(s)

            //DrawMapMarker(MyCoordinates[i], Colors.Red, mapLayer, parklist.parking_details[i].DestinationName);
            UCCustomPushPin _tooltip = new UCCustomPushPin();
            _tooltip.Description = "";
            _tooltip.DataContext = i;
            _tooltip.Menuitem.Click += Menuitem_Click;
            _tooltip.imgmarker.Tap += _tooltip_Tapimg;
            MapOverlay overlay = new MapOverlay();
            overlay.Content = _tooltip;
            overlay.GeoCoordinate = point;
            overlay.PositionOrigin = new Point(0.0, 1.0);

            return overlay;
        }

        private static void Menuitem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem menuItem = (MenuItem)sender;
                string selecteditem = menuItem.Tag.ToString().Trim();
                var selectedparkdata = Global.GlobalData.itinearyList.Where(s => s.itinerary_id.ToString().Equals(selecteditem)).ToList();
                if (selectedparkdata.Count > 0)
                {
                    foreach (var item in selectedparkdata)
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
                        //luu tru tam thoi
                        Global.GlobalData.selectedItinerary = item;
                        //navigate to Details page
                        (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/Customer/ItineraryDetails.xaml", UriKind.RelativeOrAbsolute));
                        //NavigationService.Navigate(new Uri("/Customer/ItineraryDetails.xaml", UriKind.RelativeOrAbsolute));
                        break;
                    }
                }
            }
            catch
            {
            }
        }

        private static void _tooltip_Tapimg(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                Image item = (Image)sender;
                string selecteditem = item.Tag.ToString();
                var selectedparkdata = Global.GlobalData.itinearyList.Where(s => s.itinerary_id.ToString().Equals(selecteditem)).ToList();


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

    }
}
