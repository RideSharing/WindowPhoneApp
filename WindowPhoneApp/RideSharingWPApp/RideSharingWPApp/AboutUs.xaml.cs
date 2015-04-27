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
    public partial class AboutUs : PhoneApplicationPage
    {
        public AboutUs()
        {
            InitializeComponent();
        }

        public async void sendFeedback()
        {
            
        }

        private async void btnSendFeedback_Click(object sender, RoutedEventArgs e)
        {
            StackPanel panel = new StackPanel();

            TextBox txtbUserName = new TextBox();
            TextBox txtbUserEmail = new TextBox();
            TextBox txtbUserFeedback = new TextBox();
            txtbUserFeedback.AcceptsReturn = true;
            txtbUserFeedback.TextWrapping = TextWrapping.Wrap;
            txtbUserFeedback.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            txtbUserFeedback.Height = 150;

            TextBlock b1 = new TextBlock(); b1.Text = "Name: ";
            TextBlock b2 = new TextBlock(); b2.Text = "Email: ";
            TextBlock b3 = new TextBlock(); b3.Text = "Your feedback: ";
            panel.Children.Add(b1);
            panel.Children.Add(txtbUserName);
            panel.Children.Add(b2);
            panel.Children.Add(txtbUserEmail);
            panel.Children.Add(b3);
            panel.Children.Add(txtbUserFeedback);

            CustomMessageBox messageBox = new CustomMessageBox()
            {
                //set the properties
                Caption = "Send your feedback",
                Message = "",
                LeftButtonContent = "Send",
                RightButtonContent = "Cancel"
            };

            messageBox.Content = panel;
            //messageBox.Content = b2;

            //Add the dismissed event handler
            messageBox.Dismissed += async (s1, e1) =>
            {
                switch (e1.Result)
                {
                    case CustomMessageBoxResult.LeftButton:

                        Dictionary<string, string> postData = new Dictionary<string, string>();
                        postData.Add("email", txtbUserEmail.Text.Trim());
                        postData.Add("name", txtbUserName.Text.Trim());
                        postData.Add("content", txtbUserFeedback.Text.Trim());
                        

                        HttpFormUrlEncodedContent content =
                            new HttpFormUrlEncodedContent(postData);
                        var result = await RequestToServer.sendPostRequest("feedback", content);

                        JObject jsonObject = JObject.Parse(result);
                        MessageBox.Show(jsonObject.Value<string>("message"));
                        

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
    }
}