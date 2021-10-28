using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;
using TheQDeviceConnect.Core.ViewModels.Connection;
using TheQDeviceConnect.UI.CustomUI;
using Xamarin.Forms;

namespace TheQDeviceConnect.UI.Pages
{
    public partial class WifiConnectionPage : MvxContentPage<WifiConnectionViewModel>
    {
        IDeviceConnectionService _deviceConnectionService;

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

       
        private void WifiNetworksRepeaterView_ItemSelected(object sender, EventArgs e)
        {
            DebugHelper.Info(this, "WifiNetworksRepeaterView_ItemSelected");
        }

    }
}
