using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;
using Xamarin.Essentials;

namespace TheQDeviceConnect.Core.ViewModels.Connection.Hotspot
{
    public class HotspotConnectionViewModel : BaseNavigationViewModel
    {
        public bool IsTheQHotspotConnected;
        private bool _networkChanged;
        public bool NetworkChanged
        {
            get
            {
                return _networkChanged;
            }
            set
            {
                _networkChanged = value;
                RaisePropertyChanged(() => NetworkChanged);
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

        public HotspotConnectionViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            addMockedData();
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
            Connectivity.ConnectivityChanged += HandleConnectivityChanged;
            return base.Initialize();
        }

        void HandleConnectivityChanged(object sender, ConnectivityChangedEventArgs eventArgs)
        {
            NetworkChanged = true;
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            DebugHelper.Info(this, "view appeared!");
        }
    }
}
