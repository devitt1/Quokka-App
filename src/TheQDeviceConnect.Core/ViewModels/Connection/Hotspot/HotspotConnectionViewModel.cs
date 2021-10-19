using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;

namespace TheQDeviceConnect.Core.ViewModels.Connection.Hotspot
{
    public class HotspotConnectionViewModel : BaseNavigationViewModel
    {
        public enum WifiNetworkConnectionState
        {
            DEFAULT, // Initial connection, no configuration made
            CONNECTING,// User made configuration, waiting for connection
            HOTSPOT_CONNECTED,
        }

        public MvxAsyncCommand GoToWifiConnectionViewModelCommand { get; private set; }

        public HotspotConnectionViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            addMockedData();
            GoToWifiConnectionViewModelCommand = new MvxAsyncCommand(GoToWifiConnectionVMAsync);
        }
        
        private async Task GoToWifiConnectionVMAsync()
        {
            await NavigationService.Navigate<WifiConnectionViewModel>();
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
                ShouldDisplayConnectingPage = false;
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

        private void addMockedData()
        {
            WifiNetworkVMs.Add(new WifiNetworkViewModel()
            {
                ssid = "TheQHotSpot"
            });
            WifiNetworkVMs.Add(new WifiNetworkViewModel()
            {
                ssid = "Kogan"
            });
        }

        public override Task Initialize()
        {
            WifiConnectionState = WifiNetworkConnectionState.DEFAULT;
            return base.Initialize();
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            DebugHelper.Info(this, "view appeared!");
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);
        }
    }
}
