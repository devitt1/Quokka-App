using System;
using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using static TheQDeviceConnect.Core.ViewModels.Connection.Hotspot.HotspotConnectionViewModel;

namespace TheQDeviceConnect.Core.ViewModels.Connection
{
    public class WifiConnectionViewModel : BaseNavigationViewModel
    {
        public WifiConnectionViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
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
