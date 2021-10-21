using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using TheQDeviceConnect.Core.Helpers;
using static TheQDeviceConnect.Core.ViewModels.Connection.Hotspot.HotspotConnectionViewModel;

namespace TheQDeviceConnect.Core.ViewModels.Connection
{
    public class WifiConnectionViewModel : BaseNavigationViewModel
    {
        public WifiConnectionViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            addMockedData();
        }
        public IMvxAsyncCommand GoToWifiNetworkPasswordInsertVMCommand { get; private set; }

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
            registerPropertyChangedEventHandler(WifiNetworkVMs);
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

        private WifiNetworkViewModel _selectedWifiNetworkVM;
        public WifiNetworkViewModel SelectedWifiNetworkVM
        {
            get => _selectedWifiNetworkVM;
            set
            {
                _selectedWifiNetworkVM = value;
                RaisePropertyChanged(() => SelectedWifiNetworkVM);
            }
        }

        private void registerPropertyChangedEventHandler(MvxObservableCollection<IWifiNetworkViewModel> wifiNetworkViewModels)
        {
            foreach (IWifiNetworkViewModel wifiNetworkVM in wifiNetworkViewModels)
            {
                wifiNetworkVM.PropertyChanged += handleWifiNetworkVMPropertyChanged;
            }
        }

        private void registerPropertyChangedEventHandler(IWifiNetworkViewModel wifiNetworkViewModel)
        {
            wifiNetworkViewModel.PropertyChanged += handleWifiNetworkVMPropertyChanged;

        }

        private void handleWifiNetworkVMPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DebugHelper.Info(this, "Changed!");
            if (e.PropertyName == "IsClicked")
            {
                (sender as WifiNetworkViewModel).IsSelected = true;
                SelectedWifiNetworkVM = sender as WifiNetworkViewModel;
                unselectOtherWifiNetworks(sender as WifiNetworkViewModel);
            }
        }

        private void unselectOtherWifiNetworks(WifiNetworkViewModel selectedNetwork)
        {
            foreach (WifiNetworkViewModel wifiNetworkVM in WifiNetworkVMs)
            {
                if (wifiNetworkVM.ssid != selectedNetwork.ssid)
                {
                    wifiNetworkVM.IsSelected = false;
                }
            }
            
        }
    }
}
