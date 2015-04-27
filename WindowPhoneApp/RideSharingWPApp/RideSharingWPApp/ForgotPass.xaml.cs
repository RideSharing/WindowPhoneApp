using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace RideSharingWPApp
{
    public partial class ForgotPass : PhoneApplicationPage
    {
        public ForgotPass()
        {
            InitializeComponent();
        }

        private void btnForgotPass_Click(object sender, RoutedEventArgs e)
        {
            //send request to server
        }

        private void hplFogot_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}