﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Constants;
using TheQDeviceConnect.Core.Services.Interfaces;
using Xamarin.Forms;
using MvvmCross;
using TheQDeviceConnect.Core.DataModels;
using Xamarin.Essentials;

namespace TheQDeviceConnect.Core.ViewModels.Connection.Hotspot
{
    public class HotspotConnectionViewModel : BaseNavigationViewModel
    {
        IDeviceConnectionService _deviceConnectionService;
        IDeviceConnectionService _coreDeviceConnectionService;
        public MvxAsyncCommand GoToWifiConnectionViewModelCommand { get; private set; }
        public IMvxAsyncCommand OpenAppWifiSettingsCommand { get; private set; }
        public MvxCommand ShowHotspotInstructionPageCommand { get; private set; }

        public HotspotConnectionViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            GoToWifiConnectionViewModelCommand = new MvxAsyncCommand(GoToWifiConnectionVMAsync, allowConcurrentExecutions: true);
            OpenAppWifiSettingsCommand = new MvxAsyncCommand(OpenWifiSettings);
            ShowHotspotInstructionPageCommand = new MvxCommand(ShowHotspotInstructionPage);
            _deviceConnectionService = DependencyService.Get<IDeviceConnectionService>();
            _deviceConnectionService.Initialize();
            _deviceConnectionService.OnWifiNetworkChanged += handleWifiNetworkChanged;
            _deviceConnectionService.OnConnectionTimerElapsed += (async (sender, eventArgs) =>
            {
                _deviceConnectionService.StopConnectionTimer();
                preloadNearbyWifiNetworks();
            });
            //_deviceConnectionService.OnAndroidNsdResolved += handleAndroidNsdResolved;
            _coreDeviceConnectionService = Mvx.IoCProvider.Resolve<IDeviceConnectionService>();
        }

        private void handleAndroidNsdResolved(object sender, EventArgs e)
        {
            DebugHelper.Info(this, e);
        }

        private async void preloadNearbyWifiNetworks()
        {
            DebugHelper.Info(this, "Preload....");
            WifiConnectionState = WifiNetworkConnectionState.HOTSPOT_CONNECTING;

            //var deviceName = await _coreDeviceConnectionService.GetDeviceName();
            //await SecureStorage.SetAsync("ConnectedDeviceName", deviceName);
            await _coreDeviceConnectionService.GetNearbyWifiNetworksAsync();


            WifiConnectionState = WifiNetworkConnectionState.HOTSPOT_CONNECTED;

        }
        private async Task OpenWifiSettings()
        {
            await _deviceConnectionService.OpenWifiSettings();
            await _deviceConnectionService.ConnectToTheQNetwork();
            _deviceConnectionService.StartConnectionTimer();
            WifiConnectionState = WifiNetworkConnectionState.HOTSPOT_CONNECTING;
        }

        private void ShowHotspotInstructionPage()
        {
            WifiConnectionState = WifiNetworkConnectionState.DEFAULT;
        }

        public override Task Initialize()
        {
            WifiConnectionState = WifiNetworkConnectionState.DEFAULT;
           

            return base.Initialize();
        }


       

        public override void ViewAppearing()
        {
            base.ViewAppearing();
        
            Task.Run(async () =>
            {
                await _deviceConnectionService.ForcePermissionAsync("localhost", 80);

            });

            if (_deviceConnectionService.IsConnectedToHotspot())
            {
                DebugHelper.Info(this, "Connected to The Q Hotspot!");
                preloadNearbyWifiNetworks();
            }
            else
            {
                WifiConnectionState = WifiNetworkConnectionState.DEFAULT;
            }
        }

        public override void ViewDisappeared()
        {
            base.ViewDisappeared();
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);
        }

        private async Task GoToWifiConnectionVMAsync()
        {
            await NavigationService.Navigate<WifiConnectionViewModel>();
        }

        void handleWifiNetworkChanged(object sender, EventArgs eventArgs)
        {
            DebugHelper.Info(this, "Connection changed!");
            if (_deviceConnectionService.IsConnectedToHotspot())
            {
                DebugHelper.Info(this, "Connected to The Q Hotspot!");
                WifiConnectionState = WifiNetworkConnectionState.HOTSPOT_CONNECTED;
            }
        }

        private WifiNetworkConnectionState _wifiConnectionState;
        public WifiNetworkConnectionState WifiConnectionState
        {
            get
            {
                return _wifiConnectionState;
            }
            set
            {
                _wifiConnectionState = value;
                RaisePropertyChanged(() => WifiConnectionState);
                RenderStateConditionally(_wifiConnectionState);
            }
        }

        public void RenderStateConditionally(WifiNetworkConnectionState state)
        {
            switch (state)
            {
                case WifiNetworkConnectionState.DEFAULT:
                    ShouldDisplayInstructionPage = true;
                    ShouldDisplayConnectingPage = false;
                    ShouldDisplayConfirmConnectedPage = false;
                    break;
                case WifiNetworkConnectionState.HOTSPOT_CONNECTING:
                    ShouldDisplayInstructionPage = false;
                    ShouldDisplayConnectingPage = true;
                    ShouldDisplayConfirmConnectedPage = false;
                    break;
                case WifiNetworkConnectionState.HOTSPOT_CONNECTED:
                    ShouldDisplayInstructionPage = false;
                    ShouldDisplayConnectingPage = false;
                    ShouldDisplayConfirmConnectedPage = true;
                    break;
            }
        }

        //Code-snippet generated template for public fields
        private bool _shouldDisplayInstructionPage;
        public bool ShouldDisplayInstructionPage
        {
            get
            {
                return _shouldDisplayInstructionPage;
            }
            set
            {
                (_shouldDisplayInstructionPage) = value;
                RaisePropertyChanged(() => ShouldDisplayInstructionPage);
            }
        }

        private bool _shouldDisplayConnectingPage;
        public bool ShouldDisplayConnectingPage
        {
            get
            {
                return _shouldDisplayConnectingPage;
            }
            set
            {
                _shouldDisplayConnectingPage = value;
                RaisePropertyChanged(() => ShouldDisplayConnectingPage);
            }
        }

        private bool _shouldDisplayConfirmConnectedPage;
        public bool ShouldDisplayConfirmConnectedPage
        {
            get
            {
                return _shouldDisplayConfirmConnectedPage;
            }
            set
            {
                _shouldDisplayConfirmConnectedPage = value;
                RaisePropertyChanged(() => ShouldDisplayConfirmConnectedPage);
            }
        }


        private MvxObservableCollection<IWifiNetworkViewModel> _wifiNetworkVMs = new MvxObservableCollection<IWifiNetworkViewModel>();
        public MvxObservableCollection<IWifiNetworkViewModel> WifiNetworkVMs
        {
            get => _wifiNetworkVMs;
            set
            {
                _wifiNetworkVMs = value;
                RaisePropertyChanged(() => WifiNetworkVMs);
            }
        }
    }
}
