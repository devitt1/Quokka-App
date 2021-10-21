using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.ViewModels.Connection;
using Xamarin.Forms;

namespace TheQDeviceConnect.UI.Pages
{
    public partial class WifiConnectionPage : MvxContentPage<WifiConnectionViewModel>
    {
        public WifiConnectionPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        void WifiNetworksListView_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {

        }



        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            ViewModel.PropertyChanged -= WifiNetworksRepeaterView_ItemSelected;
            ViewModel.PropertyChanged += WifiNetworksRepeaterView_ItemSelected;

        }

        private void WifiNetworksRepeaterView_ItemSelected(object sender, EventArgs e)
        {
            DebugHelper.Info(this, "WifiNetworksRepeaterView_ItemSelected");
        }

    }
}
