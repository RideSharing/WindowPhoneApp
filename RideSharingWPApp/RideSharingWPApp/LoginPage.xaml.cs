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
using RideSharingWPApp.Global;

namespace RideSharingWPApp
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            InitializeComponent();
            txtbEmail.Text = "cody@enclave.vn";
            txtbPassword.Text = "123123";
            /*Loaded += (s, e) =>
            {
                // Some login-password check condition
                if (IsolatedStorageSettings.ApplicationSettings.Contains("isLogin"))
                {
                    if (IsolatedStorageSettings.ApplicationSettings["isLogin"].Equals("True"))
                    {
                        Global.GlobalData.isDriver = (bool)IsolatedStorageSettings.ApplicationSettings["isDriver"];

                        Global.GlobalData.APIkey = (string)IsolatedStorageSettings.ApplicationSettings["APIkey"];

                        Global.GlobalData.customer_status = (int)IsolatedStorageSettings.ApplicationSettings["customer_status"];
                        Global.GlobalData.driver_status = (int)IsolatedStorageSettings.ApplicationSettings["driver_status"];

                        //Navigate to MainPage
                        if (GlobalData.isDriver)
                        {
                            NavigationService.Navigate(new Uri("/Driver/ItineraryManagement.xaml", UriKind.Relative));
                        }
                        else
                        {
                            NavigationService.Navigate(new Uri("/Customer/MainMap.xaml", UriKind.RelativeOrAbsolute));
                        }  
                     }
                }
            };*/
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("email", txtbEmail.Text.Trim());
            postData.Add("password", txtbPassword.Text);
            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);

            var result = await RequestToServer.sendPostRequest("user/login", content);

            JObject jsonObject = JObject.Parse(result);

            if (jsonObject.Value<string>("error").Equals("False"))
            {
                //get API key
                Global.GlobalData.APIkey = jsonObject.Value<string>("apiKey").Trim();

                //Global.GlobalData.isDriver = jsonObject.Value<bool>("driver");

                Global.GlobalData.customer_status = jsonObject.Value<int>("customer_status");
                Global.GlobalData.driver_status = jsonObject.Value<int>("driver_staus");

                //storage for the next login
                IsolatedStorageSettings.ApplicationSettings["isLogin"] = "True";
                IsolatedStorageSettings.ApplicationSettings["APIkey"] = Global.GlobalData.APIkey;
                IsolatedStorageSettings.ApplicationSettings["isDriver"] = Global.GlobalData.isDriver;
                IsolatedStorageSettings.ApplicationSettings["customer_status"] = Global.GlobalData.customer_status;
                IsolatedStorageSettings.ApplicationSettings["driver_status"] = Global.GlobalData.driver_status;
                IsolatedStorageSettings.ApplicationSettings.Save();
                //Navigate to MainPage
                if (GlobalData.isDriver)
                {
                    NavigationService.Navigate(new Uri("/Driver/ItineraryManagement.xaml", UriKind.Relative));
                }
                else
                {
                    NavigationService.Navigate(new Uri("/Customer/MainMap.xaml", UriKind.RelativeOrAbsolute));
                }               
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
            postData.Add("password", txtbPassword.Text);
            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);
            var result = await RequestToServer.sendPostRequest("user", content);

            JObject jsonObject = JObject.Parse(result);
            MessageBox.Show(jsonObject.Value<string>("message"));

        }

        private void linkForgotPass_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ForgotPass.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}