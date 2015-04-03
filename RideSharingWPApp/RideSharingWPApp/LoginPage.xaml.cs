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
using Windows.Web.Http;
using Newtonsoft.Json.Linq;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Tasks;
using System.IO;
using System.Windows.Media.Imaging;

namespace RideSharingWPApp
{
    public partial class LoginPage : PhoneApplicationPage
    {
        PhotoChooserTask photoChooserTask;

        public LoginPage()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {

                // Some login-password check condition
                if (IsolatedStorageSettings.ApplicationSettings.Contains("isLogin"))
                {
                    if (IsolatedStorageSettings.ApplicationSettings["isLogin"].Equals("True"))
                    {
                        Global.GlobalData.isDriver = (bool)IsolatedStorageSettings.ApplicationSettings["isDriver"];

                        Global.GlobalData.APIkey = (string)IsolatedStorageSettings.ApplicationSettings["isDriver"];

                        NavigationService.Navigate(new Uri("/MainMap.xaml", UriKind.RelativeOrAbsolute));
                    }
                }
            };
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("email", txtbEmail.Text.Trim());
            postData.Add("password", txtbPassword.Password);
            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);

            var result = await RequestToServer.sendPostRequest("user/login", content);

            JObject jsonObject = JObject.Parse(result);

            if (jsonObject.Value<string>("error").Equals("False"))
            {
                //get API key
                Global.GlobalData.APIkey = jsonObject.Value<string>("apiKey").Trim();

                Global.GlobalData.isDriver = jsonObject.Value<bool>("driver");
                //storage for the next login
                IsolatedStorageSettings.ApplicationSettings["isLogin"] = "True";
                IsolatedStorageSettings.ApplicationSettings["APIkey"] = Global.GlobalData.APIkey;
                IsolatedStorageSettings.ApplicationSettings["isDriver"] = Global.GlobalData.isDriver;
                IsolatedStorageSettings.ApplicationSettings.Save();
                //Navigate to MainPage
                NavigationService.Navigate(new Uri("/MainMap.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show(jsonObject.Value<string>("message"));
            }
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("email", txtbEmail.Text.Trim());
            postData.Add("password", txtbPassword.Password);
            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);
            var result = await RequestToServer.sendPostRequest("user", content);

            JObject jsonObject = JObject.Parse(result);
            MessageBox.Show(jsonObject.Value<string>("message"));

        }
    }
}