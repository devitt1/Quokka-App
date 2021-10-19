using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MvvmCross.Base;
using MvvmCross.Forms.Views;
using MvvmCross.ViewModels;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;
using TheQDeviceConnect.Core.ViewModels.Connection.Hotspot;
using Xamarin.Forms;

namespace TheQDeviceConnect.UI.Pages
{
    public partial class HotspotConnectionPage : MvxContentPage<HotspotConnectionViewModel>
    {
        IDeviceConnectionService _deviceConnectionService;
        public HotspotConnectionPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _deviceConnectionService = DependencyService.Get<IDeviceConnectionService>();
            _deviceConnectionService.Initialize();
            _deviceConnectionService.OnWifiNetworkChanged += handleWifiNetworkChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
        }

       
        void handleWifiNetworkChanged(object sender, EventArgs eventArgs)
        {
            DebugHelper.Info(this, "Connection changed!");
            if (_deviceConnectionService.IsConnectedToHotspot())
            {
                DebugHelper.Info(this, "Connected to The Q Hotspot!");
                ViewModel.WifiConnectionState = HotspotConnectionViewModel.WifiNetworkConnectionState.HOTSPOT_CONNECTED;
                ViewModel.ShouldDisplayConfirmConnectedPage = true;
            }
            else if (ViewModel.WifiConnectionState == HotspotConnectionViewModel.WifiNetworkConnectionState.CONNECTING) 
            {
                ViewModel.ShouldDisplayConnectingPage = true;
            }
            else if (ViewModel.WifiConnectionState == HotspotConnectionViewModel.WifiNetworkConnectionState.DEFAULT)
            {
                ViewModel.ShouldDisplayConnectingPage = false;
            }
        }
    }
}
