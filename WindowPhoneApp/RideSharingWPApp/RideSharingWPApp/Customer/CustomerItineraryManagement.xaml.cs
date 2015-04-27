﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;

namespace RideSharingWPApp.Customer
{
    public partial class CustomerItineraryManagement : PhoneApplicationPage
    {
        //ItineraryList itinearyList = new ItineraryList();
        ItineraryList itinearyCustomerAcceptedList = new ItineraryList();
        ItineraryList itinearyDriverAcceptedList = new ItineraryList();
        ItineraryList itinearyFinishedList = new ItineraryList();

        public CustomerItineraryManagement()
        {
            InitializeComponent();
            getItinerariesOfCustomer();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            getItinerariesOfCustomer();
        }

        public async void getItinerariesOfCustomer()
        {
            //reset Itinerary
            Global.GlobalData.itinearyList = new ItineraryList();
            //send get request
            var result = await Request.RequestToServer.sendGetRequest("itineraries/customer/itinerary_status");

            RootObject root = JsonConvert.DeserializeObject<RootObject>(result);

            //xu ly json
            foreach (Itinerary i in root.itineraries)
            {
                Itinerary2 i2 = new Itinerary2
                {
                    itinerary_id = i.itinerary_id,
                    driver_id = i.driver_id,
                    customer_id = Convert.ToInt32(i.customer_id),
                    start_address = i.start_address,
                    start_address_lat = i.start_address_lat,
                    start_address_long = i.start_address_long,
                    end_address = i.end_address,
                    end_address_lat = i.end_address_lat,
                    end_address_long = i.end_address_long,
                    pick_up_address = i.pick_up_address,
                    pick_up_address_lat = i.pick_up_address_lat,
                    pick_up_address_long = i.pick_up_address_long,
                    drop_address = i.drop_address,
                    drop_address_lat = i.drop_address_lat,
                    drop_address_long = i.drop_address_long,
                    cost = i.cost,
                    distance = i.distance,
                    description = i.description,
                    duration = i.duration,
                    status = i.status,
                    created_at = i.created_at,
                    leave_date = i.leave_date,
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

                if (i2.status == 1)
                {
                    //itinearyCreatedList.Add(i2);
                }
                else if (i2.status == 2)
                {
                    itinearyCustomerAcceptedList.Add(i2);
                }
                else if (i2.status == 3)
                {
                    itinearyDriverAcceptedList.Add(i2);
                }
                else if (i2.status == 4)
                {
                    itinearyFinishedList.Add(i2);
                }
                else
                {
                    //null
                }
            }
            //binding vao list
            longlistItinerariesCustomerAccepted.ItemsSource = itinearyCustomerAcceptedList;
            longlistItinerariesDriverAccepted.ItemsSource = itinearyDriverAcceptedList;
            longlistItinerariesFinished.ItemsSource = itinearyFinishedList;
        }

        /*private void longlistItineraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Itinerary2 selectedItem = (Itinerary2)longlistItineraries.SelectedItem;
            MessageBox.Show("ss: " + selectedItem.itinerary_id);
            //luu tru tam thoi
            Global.GlobalData.selectedItinerary = selectedItem;
            //navigate sang details
            NavigationService.Navigate(new Uri("/Customer/DriverItineraryDetails.xaml", UriKind.RelativeOrAbsolute));
        }*/

        private void longlistItinerariesCustomerAccepted_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Itinerary2 selectedItem = (Itinerary2)longlistItinerariesCustomerAccepted.SelectedItem;
            MessageBox.Show("ss: " + selectedItem.itinerary_id);
            //luu tru tam thoi
            Global.GlobalData.selectedItinerary = selectedItem;
            //navigate sang details
            NavigationService.Navigate(new Uri("/Customer/ItineraryDetails.xaml", UriKind.RelativeOrAbsolute));
        }

        private void longlistItinerariesDriverAccepted_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Itinerary2 selectedItem = (Itinerary2)longlistItinerariesDriverAccepted.SelectedItem;
            MessageBox.Show("ss: " + selectedItem.itinerary_id);
            //luu tru tam thoi
            Global.GlobalData.selectedItinerary = selectedItem;
            //navigate sang details
            NavigationService.Navigate(new Uri("/Customer/ItineraryDetails.xaml", UriKind.RelativeOrAbsolute));
        }

        private void longlistItinerariesFinished_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Itinerary2 selectedItem = (Itinerary2)longlistItinerariesFinished.SelectedItem;
            MessageBox.Show("ss: " + selectedItem.itinerary_id);
            //luu tru tam thoi
            Global.GlobalData.selectedItinerary = selectedItem;
            //navigate sang details
            NavigationService.Navigate(new Uri("/Customer/ItineraryDetails.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menuHome_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Customer/MainMap.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menuPostItinerary_Click(object sender, EventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Customer/....xaml", UriKind.RelativeOrAbsolute));
        }

        private void menuItineraryManagement_Click(object sender, EventArgs e)
        {
           // NavigationService.Navigate(new Uri("/Customer/CustomerItineraryManagement.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menuSearch_Click(object sender, EventArgs e)
        {

        }

        private void menuAccountInfo_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AccountInfo.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menuAboutUs_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutUs.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menuLogOut_Click(object sender, EventArgs e)
        {
            //delete UserInfo Before Logout
            Global.GlobalData.deleteUserInfoBeforeLogout();

            NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.RelativeOrAbsolute));
        }

    }
}