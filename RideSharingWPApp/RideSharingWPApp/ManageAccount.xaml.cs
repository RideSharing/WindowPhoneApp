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
using Newtonsoft.Json.Linq;
using Windows.Web.Http;

namespace RideSharingWPApp
{
    public partial class ManageAccount : PhoneApplicationPage
    {
        public ManageAccount()
        {
            InitializeComponent();

            //get user infomation from server
            getUserInformation();
            //


        }

        public async void getUserInformation()
        {

            string methodName = "user";
            var result = await RequestToServer.sendGetRequest(methodName);
            JObject jsonObject = JObject.Parse(result);
            //set information to view


        }

        public async void getDriverInformation()
        {
            string methodName = "driver";
            var result = await RequestToServer.sendGetRequest(methodName);
            JObject jsonObject = JObject.Parse(result);
            //set information to view
        }

        public async void updateUserInfomation()
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("fullname", "");
            postData.Add("phone", "");
            postData.Add("personalID", "");
            postData.Add("personalID_img", "");
            postData.Add("link_avatar", "");
            postData.Add("locked", "");
            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);
            var result = await RequestToServer.sendPutRequest("user", content);
            
            JObject jsonObject = JObject.Parse(result);
            MessageBox.Show(jsonObject.Value<string>("message"));
        }

        public async void updatePassword()
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("value", "");
            
            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);
            var result = await RequestToServer.sendPutRequest("user/password", content);

            JObject jsonObject = JObject.Parse(result);
            MessageBox.Show(jsonObject.Value<string>("message"));
        }

        public async void upgradeDriver()
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("driver_license", "");
            postData.Add("driver_license_img", "");
            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);
            var result = await RequestToServer.sendPostRequest("driver", content);

            JObject jsonObject = JObject.Parse(result);
            MessageBox.Show(jsonObject.Value<string>("message"));
        }
    }
}