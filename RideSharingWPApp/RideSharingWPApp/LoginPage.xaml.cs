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

            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);

            Loaded += (s, e) =>
            {

                // Some login-password check condition
                if (IsolatedStorageSettings.ApplicationSettings.Contains("isLogin"))
                {
                    if (IsolatedStorageSettings.ApplicationSettings["isLogin"].Equals("True"))
                    {
                        NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
                    }
                }
            };

        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            BingGeocoder.BingGeocoderClient b = new BingGeocoder.BingGeocoderClient("Ajze-B_0BaOUYxiJ0Hizj6wnyAnyRDPI5jfvDa1J7zkrCQZz2GNZkIigjLhi__nM");

            b.Geocode("sdfsd");

            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("email", txtbEmail.Text.Trim());
            postData.Add("password", txtbPassword.Password);
            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);

            //HttpClient client = new HttpClient();
            //HttpResponseMessage response = await client.PostAsync(uri, formContent);

            //Windows.Web.Http.IHttpContent content = (Windows.Web.Http.IHttpContent)postData;
            //Windows.Web.Http.IHttpContent content = new Windows.Web.Http.IHttpContent.
            
             //var result = await RequestToServer.sendGetRequest("itinerary/2", content);
            var result = await RequestToServer.sendPostRequest("user/login", content);
             //MessageBox.Show(result);
             //MessageBox.Show(x);

            JObject jsonObject = JObject.Parse(result);
            
            if (jsonObject.Value<string>("error").Equals("False"))
            {
                //get API key
                string APIkey = jsonObject.Value<string>("apiKey").Trim();

                //storage for the next login
                IsolatedStorageSettings.ApplicationSettings["isLogin"] = "True";
                IsolatedStorageSettings.ApplicationSettings["APIkey"] = APIkey;
                IsolatedStorageSettings.ApplicationSettings.Save();

                //Navigate to MainPage
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show(jsonObject.Value<string>("message"));
            }

            //string name = jsonObject.Value<string>("Name");
            //var y = result.T
             //await dialog.ShowAsync();
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            /*Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("email", txtbEmail.Text.Trim());
            postData.Add("password", txtbPassword.Password);
            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);
            var result = await RequestToServer.sendPostRequest("user", content);

            JObject jsonObject = JObject.Parse(result);
            MessageBox.Show(jsonObject.Value<string>("message"));*/

            photoChooserTask.Show();

        }

        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                

                //Code to display the photo on the page in an image control named myImage.
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);

                //MessageBox.Show(bmp.ToString());
                Image myImgage = new Image();
                myImgage.Source = bmp;
                string str = ImageConvert.ImageConvert.convertImageToBase64(myImgage);

                myImg.Source = ImageConvert.ImageConvert.convertBase64ToImage(str);
                //MessageBox.Show(str);
            }
        }
    }
}