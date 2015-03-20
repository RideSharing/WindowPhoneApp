using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RideSharingWPApp.Request;

namespace RideSharingWPApp
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("email", "dauxanhrauma@gmail.com"));
            postData.Add(new KeyValuePair<string, string>("password", "ditconmemay"));

            //Windows.Web.Http.IHttpContent content = (Windows.Web.Http.IHttpContent)postData;
            //Windows.Web.Http.IHttpContent content = new Windows.Web.Http.IHttpContent.
            Object a = new Object;
             var result = await RequestToServer.sendGetRequest("itinerary/2", content);
             //var result = await RequestToServer.sendGetRequest("user");
             MessageBox.Show(result);
             //await dialog.ShowAsync();
        }
    }
}