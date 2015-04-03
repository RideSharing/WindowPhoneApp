using Microsoft.Phone.Maps.Controls;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RideSharingWPApp.UserControls
{
    class MarkerDraw
    {
        public static MapOverlay DrawMapMarker(GeoCoordinate point)
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
