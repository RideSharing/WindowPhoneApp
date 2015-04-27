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
    public partial class Refresh : PhoneApplicationPage
    {
        public Refresh()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var j = this.NavigationService.RemoveBackEntry();
            this.Dispatcher.BeginInvoke(() => App.RootFrame.Navigate(j.Source));
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.NavigationService.RemoveBackEntry();
            base.OnNavigatedTo(e);
        }
    }
}