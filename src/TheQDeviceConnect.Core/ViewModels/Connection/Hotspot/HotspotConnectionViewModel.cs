using System;
using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;

namespace TheQDeviceConnect.Core.ViewModels.Connection.Hotspot
{
    public class HotspotConnectionViewModel : BaseNavigationViewModel
    {
        public bool IsConnected;
        //Service declaration
        private IDeviceConnectionService _deviceConnectionService;



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

        public HotspotConnectionViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService, IDeviceConnectionService deviceConnectionService) : base(logProvider, navigationService)
        {
            _deviceConnectionService = deviceConnectionService;
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

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            DebugHelper.Info(this, "view appeared!");
        }
    }
}
