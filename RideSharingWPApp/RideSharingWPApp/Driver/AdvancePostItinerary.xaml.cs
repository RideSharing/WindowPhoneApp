using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Web.Http;
using RideSharingWPApp.Request;
using Newtonsoft.Json.Linq;

namespace RideSharingWPApp
{
    public partial class AdvancePostItinerary : PhoneApplicationPage
    {
        public AdvancePostItinerary()
        {
            InitializeComponent();
        }

        string start = "";
        string end = "";
        string start_lat, start_long, end_lat, end_long = "";

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            if (NavigationContext.QueryString.TryGetValue("start", out start))
            {
                txtbStart.Text = start;
            }

            if (NavigationContext.QueryString.TryGetValue("end", out end))
            {
                txtbEnd.Text = end;
            }

            if (NavigationContext.QueryString.TryGetValue("s_lat", out start_lat) && NavigationContext.QueryString.TryGetValue("s_long", out start_long)
                && NavigationContext.QueryString.TryGetValue("e_lat", out end_lat) && NavigationContext.QueryString.TryGetValue("e_long", out end_long))
            {
                //OK
            }
            else
            {
                //roll back
            }
                
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            //validate

            //send info to server
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("start_address", txtbStart.Text.Trim());
            postData.Add("end_address", txtbEnd.Text.Trim());
            postData.Add("description", txtbDescription.Text.Trim());
            postData.Add("cost", txtbCost.Text.Trim());
            postData.Add("distance", txtbDistance.Text.Trim());
            postData.Add("start_address_lat", start_lat.Trim());
            postData.Add("start_address_long", start_long.Trim());
            postData.Add("end_address_lat", end_lat.Trim());
            postData.Add("end_address_long", end_long.Trim());
            postData.Add("leave_date", "09/09/2015");
            postData.Add("duration", txtbCost.Text.Trim());
            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);

            //var result = await RequestToServer.sendGetRequest("itinerary/2", content);
            var result = await RequestToServer.sendPostRequest("itinerary", content);

            JObject jsonObject = JObject.Parse(result);
            MessageBox.Show(jsonObject.Value<string>("message"));
        }
    }
}