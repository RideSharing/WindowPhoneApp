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
using Microsoft.Phone.Tasks;
using Windows.Web.Http;

namespace RideSharingWPApp
{
    public partial class Upgrade : PhoneApplicationPage
    {
        CameraCaptureTask cameraCaptureTask;
        public Upgrade()
        {
            InitializeComponent();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (Global.GlobalData.isDriver)
            {
                Button btnUpdate = new Button();
                btnUpdate.Content = "Update";
                btnUpdate.Click += btnUpdate_Click;
                LayoutRoot.Children.Add(btnUpdate);
                Grid.SetRow(btnUpdate, 4);


                getDriverInfo();
            }
            else
            {
                //create button update hanh trinh
                Button btnUpgrade = new Button();
                btnUpgrade.Content = "Upgrade";
                btnUpgrade.Click += btnUpgrade_Click;
                LayoutRoot.Children.Add(btnUpgrade);
                Grid.SetRow(btnUpgrade, 4);


            }
        }

        private async void btnUpgrade_Click(object sender, RoutedEventArgs e)
        {
            //validate all fields

            //upgrade
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("driver_license", txtbDriverLicense.Text.Trim());

            postData.Add("driver_license_img", ImageConvert.ImageConvert.convertImageToBase64(imgDriverLicense));

            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);
            var result = await RequestToServer.sendPostRequest("driver", content);

            JObject jsonObject = JObject.Parse(result);
            if (jsonObject.Value<bool>("error"))
            {
                MessageBox.Show(jsonObject.Value<string>("message"));
            }
            else
            {
                Global.GlobalData.isDriver = true;
                MessageBox.Show(jsonObject.Value<string>("message"));
                // navigate ve acc info
                NavigationService.Navigate(new Uri("/AccountInfo.xaml", UriKind.RelativeOrAbsolute));
            }
            
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //validate all fields

            //update
            Dictionary<string, string> postData = new Dictionary<string, string>();
            postData.Add("driver_license", txtbDriverLicense.Text.Trim());

            postData.Add("driver_license_img", ImageConvert.ImageConvert.convertImageToBase64(imgDriverLicense));

            HttpFormUrlEncodedContent content =
                new HttpFormUrlEncodedContent(postData);
            var result = await RequestToServer.sendPutRequest("driver", content);

            JObject jsonObject = JObject.Parse(result);
            if (jsonObject.Value<bool>("error"))
            {
                MessageBox.Show(jsonObject.Value<string>("message"));
            }
            else
            {
                //Global.GlobalData.isDriver = true;
                MessageBox.Show(jsonObject.Value<string>("message"));
                // refresh lai trang
                NavigationService.Navigate(new Uri("/Refresh.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        public async void getDriverInfo()
        {
            var result = await RequestToServer.sendGetRequest("driver");

            JObject jsonObject = JObject.Parse(result);

            txtbDriverLicense.Text = jsonObject.Value<string>("driver_license");
            
            //get picture
            imgDriverLicense.Source = ImageConvert.ImageConvert.convertBase64ToImage(jsonObject.Value<string>("driver_license_img").Trim());
        }

        private void btnUpdatePID_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            StackPanel panel = new StackPanel();

            TextBox txtbUpdateDriverLicense = new TextBox();

            TextBlock b1 = new TextBlock(); b1.Text = "Driver License: ";
            txtbUpdateDriverLicense.Text = txtbDriverLicense.Text;
            panel.Children.Add(b1);
            panel.Children.Add(txtbUpdateDriverLicense);
            CustomMessageBox messageBox = new CustomMessageBox()
            {
                //set the properties
                Caption = "Update your Driver License",
                Message = "",
                LeftButtonContent = "Change",
                RightButtonContent = "Cancel"
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
                        if (!txtbUpdateDriverLicense.Text.Trim().Equals(""))
                        {
                            txtbDriverLicense.Text = txtbUpdateDriverLicense.Text.Trim();
                        }
                        else
                        {
                            MessageBox.Show("Please fill up all the fields");
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

        private void btnCaptureLicenseImg_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(cameraCaptureLicenseTask_Completed);
            cameraCaptureTask.Show();
        }

        private void cameraCaptureLicenseTask_Completed(object sender, PhotoResult e)
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

                imgDriverLicense.Source = ImageConvert.ImageConvert.convertBase64ToImage(str);
            }
        }
    }
}