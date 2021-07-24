using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace mossrecru.Views.Popups
{
    public partial class Filter : Rg.Plugins.Popup.Pages.PopupPage
    {
        public Filter()
        {
            InitializeComponent();

            LoadData();
        }

        private void LoadData()
        {
            if (Models.DataStore.Technologies == null || !Models.DataStore.Technologies.Any())
                return;

            foreach (var tech in Models.DataStore.Technologies)
            {
                dateBag.Children.Add(new UserControls.FilterItem
                {
                    Title = tech.Name
                });
            }
        }

        async void BtnOK_OnTapped(System.Object sender, System.EventArgs e)
        {
            //await Navigation.PopAsync();
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAllAsync();

        }
    }
}
