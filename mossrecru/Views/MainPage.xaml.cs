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

        private bool isLoading;
        private bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                if (value)
                {
                    actInd.IsRunning = true;
                    filterIcon.IsVisible = false;
                }
                else
                {
                    actInd.IsRunning = false;
                    filterIcon.IsVisible = true;
                }
            }
        }

        private async void LoadCandidates()
        {
            //ensure no network issues
            if (!await CheckNetwork())
                return;

            IsLoading = true;

            var result = await vm.LoadCandidates();

            IsLoading = false;

            if (!result.isSuccessful && string.IsNullOrWhiteSpace(result.message))
            {
                //if api call fails, check network
                await CheckNetwork();
                return;
            }

            if (!result.isSuccessful)
            {
                await DisplayAlert("Try Again", result.message, "Ok");
            }
        }

        private async Task<bool> CheckNetwork()
        {
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                await DisplayAlert("No network", "Please check your network connection", "ok");
                return false;
            }

            return true;
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
            if (IsLoading)
                return;

            filterPopup = filterPopup ?? new Popups.Filter(vm);
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(filterPopup);


            //await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new Popups.Filter(vm));
            //await Navigation.PushAsync(new Popups.Filter());
        }
    }
}
