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

namespace RideSharingWPApp
{
    public partial class ItineraryManagement : PhoneApplicationPage
    {
        public ItineraryManagement()
        {
            InitializeComponent();

        }

        public async void getItinerariesOfDriver()
        {
            //send get request
            var result = Request.RequestToServer.sendGetRequest("");

            var root = JsonConvert.DeserializeObject<dynamic>(result);

            //xu ly json

            //binding vao list
        }

        private void longlistItineraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}