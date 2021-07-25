using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace mossrecru.Views.Popups
{
    public partial class Filter : Rg.Plugins.Popup.Pages.PopupPage
    {
        public Filter()
        {
            InitializeComponent();

            BindingContext = vm = new ViewModels.CandidateVM();
            LoadData();
        }

        ViewModels.CandidateVM vm;
        public Filter(ViewModels.CandidateVM _vm)
        {
            InitializeComponent();

            BindingContext = vm = _vm;
            LoadData();
        }

        private void LoadData()
        {
            if (Models.DataStore.Technologies == null || !Models.DataStore.Technologies.Any())
                return;

            foreach (var tech in Models.DataStore.Technologies)
            {
                var item = new UserControls.FilterItem
                {
                    Title = tech.Name
                };
                item.Tapped += ByTech_Tapped;

                dateBag.Children.Add(item);
            }
        }

        private async Task GoBack()
        {
            //await Navigation.PopAsync();
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAllAsync();
        }


        UserControls.FilterItem byStatus;
        async void ByStatus_Tapped(System.Object sender, System.EventArgs e)
        {
            if (sender == byStatus)
                return;

            //deselect former
            if (byStatus != null)
                byStatus.DeSelect();

            //deselect tech
            if (byTech != null)
                byTech.DeSelect();

            txtYearsExp.Text = "";

            byStatus = sender as UserControls.FilterItem;

            string title = byStatus.Title;
            vm.FilterByStatus(title);

            await GoBack();
        }

        UserControls.FilterItem byTech;
        async void ByTech_Tapped(System.Object sender, System.EventArgs e)
        {
            if (sender == byTech)
                return;

            //deselect former
            if (byTech != null)
                byTech.DeSelect();

            //deselect byStatus
            if (byStatus != null)
                byStatus.DeSelect();

            byTech = sender as UserControls.FilterItem;

            //string tech = byTech.Title;
            //vm.FilterByTechAndExperience(tech, txtYearsExp.Text);
        }

        async void BtnApply_OnTapped(System.Object sender, System.EventArgs e)
        {
            //deselect byStatus
            if (byStatus != null)
                byStatus.DeSelect();

            string tech;

            if (byTech != null)
                tech = byTech.Title;

            vm.FilterByTechAndExperience(byTech.Title, txtYearsExp.Text);
            await GoBack();
        }

    }
}
