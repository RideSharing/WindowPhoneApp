using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace RideSharingWPApp
{
    public partial class ViewOtherProfile : PhoneApplicationPage
    {
        OtherUserList viewOtherUserList = new OtherUserList();
        RootObject root = null;
        List<OtherUser> ListOtherUser = new List<OtherUser>();

        //  ViewUserList otherUserList = new ViewUserList();
        public ViewOtherProfile()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            InitializeComponent();
            GetUserList();
        }



        public async void GetUserList()
        {
            //Global.GlobalData.otherUserList = new OtherUserList();
            //send get request
            //string result = null;
            var result = await Request.RequestToServer.sendGetRequest("itineraries/driver");
            RootObject root = JsonConvert.DeserializeObject<RootObject>(result);
            //xu ly json
            foreach (OtherUser i in root.otheruser)
            {
                OtherUser2 i2 = new OtherUser2
                {

                    driver_license = i.driver_license,
                    driver_license_img = i.driver_license_img,
                    user_id = i.user_id,
                    email = i.email,
                    fullname = i.fullname,
                    phone = i.phone,
                    personalID = i.personalID,
                    //convert base64 to image
                    link_avatar = ImageConvert.ImageConvert.convertBase64ToImage(i.link_avatar),
                    average_rating = i.average_rating
                };
                //Binding into other user list
                longlistViewOtherUser.ItemsSource = viewOtherUserList;
            }
        }



        private void longlistViewOtherUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OtherUser2 selectedItem = (OtherUser2)longlistViewOtherUser.SelectedItem;
            MessageBox.Show("ss: " + selectedItem.user_id);
            //luu tru tam thoi
            //Global.GlobalData.viewOtherUserList = selectedItem;
            //navigate sang details
            NavigationService.Navigate(new Uri("/OtherUserDetails.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnSubmitComment_Click(object sender, RoutedEventArgs e)
        {

        }


        private void menuAccountInfo_Click(object sender, EventArgs e)
        {

        }

        private void menuAboutUs_Click(object sender, EventArgs e)
        {

        }

        private void menuLogOut_Click(object sender, EventArgs e)
        {

        }

        private void menuHome_Click(object sender, EventArgs e)
        {

        }

        private void menuSearch_Click(object sender, EventArgs e)
        {

        }

        private void menuPostItinerary_Click(object sender, EventArgs e)
        {

        }

        private void menuItineraryManagement_Click(object sender, EventArgs e)
        {

        }

        private void txtbPhone_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        public class OtherUser
        {
            public string driver_license { get; set; }
            public string driver_license_img { get; set; }
            public int user_id { get; set; }
            public string email { get; set; }
            public string fullname { get; set; }
            public string phone { get; set; }
            public string personalID { get; set; }
            public string link_avatar { get; set; }
            public int average_rating { get; set; }
        }

        public class OtherUser2
        {
            public string driver_license { get; set; }
            public string driver_license_img { get; set; }
            public int user_id { get; set; }
            public string email { get; set; }
            public string fullname { get; set; }
            public string phone { get; set; }
            public string personalID { get; set; }
            public BitmapImage link_avatar { get; set; }
            public int average_rating { get; set; }
        }

        public class RootObject
        {
            public bool error { get; set; }
            public List<OtherUser> otheruser { get; set; }
        }

        public class OtherUserList : List<OtherUser2>
        {

        }

    }
}