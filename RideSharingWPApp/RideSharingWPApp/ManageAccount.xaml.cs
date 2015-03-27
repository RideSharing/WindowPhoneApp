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
using Microsoft.Phone.Tasks;
using RideSharingWPApp.ImageConvert;

namespace RideSharingWPApp
{
    public partial class ManageAccount : PhoneApplicationPage
    {
        PhotoChooserTask photoChooserTask;
        CameraCaptureTask cameraCaptureTask;

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
            txtboxEmail.Text = jsonObject.Value<string>("email");
            txtboxPhone.Text = jsonObject.Value<string>("phone");
            txtboxPersionalId.Text = jsonObject.Value<string>("personalID");
            txtblockFullName.Text = jsonObject.Value<string>("fullname");

            //set Image
            imgAvatar.Source = ImageConvert.ImageConvert.convertBase64ToImage(jsonObject.Value<string>("link_avatar").Trim());
            imgPersonalID.Source = ImageConvert.ImageConvert.convertBase64ToImage(jsonObject.Value<string>("personalID_img").Trim());

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

        public async void updatePassword(string newPassword)
        {
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("value", newPassword);

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

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            StackPanel panel = new StackPanel();

            TextBox txtbNewPassword = new TextBox();
            TextBox txtbConfirmNewPassword = new TextBox();

            TextBlock b1 = new TextBlock(); b1.Text = "Password: ";
            TextBlock b2 = new TextBlock(); b2.Text = "Confirm Password: ";
            panel.Children.Add(b1);
            panel.Children.Add(txtbNewPassword);
            panel.Children.Add(b2);
            panel.Children.Add(txtbConfirmNewPassword);
            CustomMessageBox messageBox = new CustomMessageBox()
            {
                //set the properties
                Caption = "Update your password",
                Message = "",
                LeftButtonContent = "Change",
                RightButtonContent = "No"
            };

            messageBox.Content = panel;
            //messageBox.Content = b2;

            //Add the dismissed event handler
            messageBox.Dismissed += (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:
                        //add the task you wish to perform when user clicks on yes button here
                        if (txtbNewPassword.Text.Trim().Equals(txtbConfirmNewPassword.Text.Trim()))
                        {
                            updatePassword(txtbNewPassword.Text.Trim());
                        }
                        else
                        {
                            MessageBox.Show("Password not match");
                        }
                        
                        break;
                    case CustomMessageBoxResult.RightButton:
                        //add the task you wish to perform when user clicks on no button here

                        break;
                    case CustomMessageBoxResult.None:
                        // Do something.
                        break;
                    default:
                        break;
                }
            };

            //add the show method
            messageBox.Show();
        }

        private void btnSelectAvaImg_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoAvatarChooserTask_Completed);
            photoChooserTask.Show();

            //imgAvatar.Source = ImageConvert.ImageConvert.convertBase64ToImage(PhotosPicker.photoPick);
        }

        void photoAvatarChooserTask_Completed(object sender, PhotoResult e)
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

                imgAvatar.Source = ImageConvert.ImageConvert.convertBase64ToImage(str);
                //MessageBox.Show(str);
            }
        }

        void photoPerIDChooserTask_Completed(object sender, PhotoResult e)
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

                imgPersonalID.Source = ImageConvert.ImageConvert.convertBase64ToImage(str);
                //MessageBox.Show(str);
            }
        }

        private void btnSelectPerIDImg_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoPerIDChooserTask_Completed);
            photoChooserTask.Show();
        }

        private void btnCaptureAvaImg_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(cameraCaptureAvaTask_Completed);
            cameraCaptureTask.Show();
        }

        private void btnCapturePerIDImg_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(cameraCapturePerIDTask_Completed);
            cameraCaptureTask.Show();
        }

        void cameraCaptureAvaTask_Completed(object sender, PhotoResult e)
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

                imgAvatar.Source = ImageConvert.ImageConvert.convertBase64ToImage(str);
            }
        }

        void cameraCapturePerIDTask_Completed(object sender, PhotoResult e)
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

                imgPersonalID.Source = ImageConvert.ImageConvert.convertBase64ToImage(str);
            }
        }


    }
}