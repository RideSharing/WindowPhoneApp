using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;

namespace RideSharingWPApp
{
    public partial class ItineraryManagement : PhoneApplicationPage
    {
        ItineraryList itinearyList = new ItineraryList();

        public ItineraryManagement()
        {
            InitializeComponent();

            getItinerariesOfDriver();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //(this.DataContext as ItineraryManagement).LoadData();
            //NavigationService.RemoveBackEntry();
            itinearyList = new ItineraryList();
            longlistItineraries.ItemsSource = itinearyList;
            getItinerariesOfDriver();

        }

        public async void getItinerariesOfDriver()
        {
            //send get request
            var result = await Request.RequestToServer.sendGetRequest("itineraries/driver/itinerary_status");

            RootObject root = JsonConvert.DeserializeObject<RootObject>(result);
            itinearyList = new ItineraryList();
            //xu ly json
            foreach (Itinerary i in root.itineraries)
            {
                itinearyList.Add(new Itinerary2
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
                });

                
            }
            //binding vao list
            longlistItineraries.ItemsSource = null;
            longlistItineraries.ItemsSource = itinearyList;
        }

        private void longlistItineraries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Itinerary2 selectedItem = (Itinerary2)longlistItineraries.SelectedItem;
            MessageBox.Show("ss: " + selectedItem.itinerary_id);
            //luu tru tam thoi
            Global.GlobalData.selectedItinerary = selectedItem;
            //navigate sang details
            NavigationService.Navigate(new Uri("/Driver/DriverItineraryDetails.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            //navigate sang details
            NavigationService.Navigate(new Uri("/Driver/PostItinerary.xaml", UriKind.RelativeOrAbsolute));
        }

        private void menuCustomer_Click(object sender, EventArgs e)
        {
            //navigate sang details
            NavigationService.Navigate(new Uri("/Customer/MainMap.xaml", UriKind.RelativeOrAbsolute));
        }
    }

    
}