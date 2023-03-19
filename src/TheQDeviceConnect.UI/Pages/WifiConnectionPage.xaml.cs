using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;
using TheQDeviceConnect.Core.ViewModels.Connection;
using TheQDeviceConnect.UI.CustomUI;
using Xamarin.Essentials;
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


        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            ViewModel.PropertyChanged -= WifiNetworksRepeaterView_ItemSelected;
            ViewModel.PropertyChanged += WifiNetworksRepeaterView_ItemSelected;

        }

        private async void OpenTheQ(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://theq-webapp.fordevpurposeonly.com/");
        }
       
        private void WifiNetworksRepeaterView_ItemSelected(object sender, EventArgs e)
        {
            DebugHelper.Info(this, "WifiNetworksRepeaterView_ItemSelected");
        }

    }
}
