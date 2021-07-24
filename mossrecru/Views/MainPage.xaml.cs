using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace mossrecru.Views
{
    public partial class MainPage : ContentPage
    {
        ViewModels.CandidateVM vm;
        public MainPage()
        {
            InitializeComponent();

            BindingContext = vm = new ViewModels.CandidateVM();
            LoadCandidates();
        }

        private async void LoadCandidates()
        {
            //ensure no network issues
            await CheckNetwork();

            var result = await vm.LoadCandidates();

            if (!result.isSuccessful && string.IsNullOrWhiteSpace(result.message))
            {
                //if api call fails, check network
                await CheckNetwork();
                return;
            }

            if (!result.isSuccessful)
            {
                await DisplayAlert("Try Again", result.message, "ok");
            }
        }

        private async Task CheckNetwork()
        {
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                await DisplayAlert("No network", "Please check your network connection", "ok");
                return;
            }
        }

        void cvDevelopers_SelectionChanged(System.Object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            //var item = e.CurrentSelection?.FirstOrDefault() as Models.Response.Notification.NotificationResponse.NotificationObject.Notification;

            //if (item == null)
            //{
            //    cvDevelopers.SelectedItem = null;
            //    return;
            //}
        }

        void SwipeGestureRecognizer_Swiped(System.Object sender, Xamarin.Forms.SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    GetSwipedElement((sender as Frame).BindingContext, isSelected: true);
                    break;
                case SwipeDirection.Right:
                    GetSwipedElement((sender as Frame).BindingContext, isSelected: false);
                    break;
            }
        }


        private void GetSwipedElement(object bindingContext, bool isSelected)
        {
            vm.ChangeCandidateStatus(bindingContext, isSelected);
        }

        void OnDeleteSwipeItemInvoked(System.Object sender, System.EventArgs e)
        {
            var context = (sender as SwipeItem).BindingContext;
            GetSwipedElement(context, isSelected: false);
        }

        void OnChooseSwipeItemInvoked(System.Object sender, System.EventArgs e)
        {
            var context = (sender as SwipeItem).BindingContext;
            GetSwipedElement(context, isSelected: true);
        }

        Popups.Filter filterPopup;
        async void History_Tapped(System.Object sender, System.EventArgs e)
        {
            // filterPopup = filterPopup ?? new Popups.Filter();
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new Popups.Filter());
            //await Navigation.PushAsync(new Popups.Filter());
        }
    }
}
