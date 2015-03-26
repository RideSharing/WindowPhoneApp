using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.Phone.Tasks;
using System.Device.Location;
using Microsoft.Phone.Net.NetworkInformation;

namespace CustomPushpin
{
    public partial class MapViewDetailsPage : PhoneApplicationPage
    {
        LocationDetail selectedlocdata = new LocationDetail();
        IsolatedStorageFile Settings = IsolatedStorageFile.GetUserStoreForApplication();
        public MapViewDetailsPage()
        {
            InitializeComponent();
            this.Loaded += MapViewDetailsPage_Loaded;
        }

        private void MapViewDetailsPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Settings.FileExists("LocationDetailItem"))
                {
                    using (IsolatedStorageFileStream fileStream = Settings.OpenFile("LocationDetailItem", FileMode.Open))
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(LocationDetail));
                        selectedlocdata = (LocationDetail)serializer.ReadObject(fileStream);

                    }
                }
                BodyContentPanel.DataContext = selectedlocdata;

            }
            catch
            {
            }
        }

        private void BackBtn_tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void FindRout_tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    //var locationX = new GeoCoordinate(cl.latitude, cl.langitude);
                    var locationY = new GeoCoordinate(double.Parse("" + selectedlocdata.Lat), double.Parse("" + selectedlocdata.Long));
                    BingMapsDirectionsTask directionTask = new BingMapsDirectionsTask();
                    directionTask.End = new LabeledMapLocation("" + selectedlocdata.D_name, locationY);
                    //  directionTask.Start = new LabeledMapLocation("My Location", locationX);
                    directionTask.Show();
                }
                else
                {
                    MessageBox.Show("Network is not available.");


                }
            }
            catch
            {

            }
        }
    }
}