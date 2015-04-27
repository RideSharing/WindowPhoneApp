using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RideSharingWPApp.ListItemClass;

namespace RideSharingWPApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            string txt = "LTV";
            //txtbUserName
            List<MainMenuItem> source = new List<MainMenuItem>();
            source.Add(new MainMenuItem("Search", "/Assets/MainMenuImg/ic_search.png"));
            source.Add(new MainMenuItem("Home", "/Assets/MainMenuImg/ic_home.png"));
            source.Add(new MainMenuItem("Login", "/Assets/MainMenuImg/ic_login.png"));
            source.Add(new MainMenuItem("Itinerary", "/Assets/MainMenuImg/ic_communities.png"));
            source.Add(new MainMenuItem("User", "/Assets/MainMenuImg/ic_user.png"));
            source.Add(new MainMenuItem("Profile", "/Assets/MainMenuImg/ic_profile.png"));
            source.Add(new MainMenuItem("Register", "/Assets/MainMenuImg/ic_register.png"));

            longlistMainMenu.ItemsSource = source;
            txtUserName.Text = txt;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }
    }
}