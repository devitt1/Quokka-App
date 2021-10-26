using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using TheQDeviceConnect.Core.Constants;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;
using Xamarin.Forms;

namespace TheQDeviceConnect.Core.ViewModels.Connection
{
    public class WifiConnectionViewModel : BaseNavigationViewModel
    {

        public WifiConnectionViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            _deviceConnectionService = DependencyService.Get<IDeviceConnectionService>();
            _deviceConnectionService.OnConnectionTimerElapsed += handleConnectionTimerElapsed;

            addMockedData();
            CloseCommand = new MvxAsyncCommand(CloseAsync, allowConcurrentExecutions: true);
            ShowWifiNetworkPasswordInsertPageCommand = new MvxCommand(ShowWifiNetworkPasswordInsertPage);
            ShowWifiNetworkSelectionPageCommand = new MvxCommand(ShowWifiNetworkSelectionPage);
            ShowWifiNetworkConnectingPageCommand = new MvxCommand(ShowWifiNetworkConnectingPage);
            ShowWifiNetworkConnectedPageCommand = new MvxCommand(ShowWifiWifiNetworkConnectedPage);
        }

        IDeviceConnectionService _deviceConnectionService;
        public IMvxAsyncCommand CloseCommand { get; private set; }
        public IMvxCommand ShowWifiNetworkSelectionPageCommand { get; private set; }
        public IMvxCommand ShowWifiNetworkPasswordInsertPageCommand { get; private set; }
        public IMvxCommand ShowWifiNetworkConnectingPageCommand { get; private set; }
        public IMvxCommand ShowWifiNetworkConnectedPageCommand { get; private set; }

        private async Task CloseAsync()
        {
            await NavigationService.Close(this);
        }


        public override Task Initialize()
        {
            WifiConnectionState = WifiNetworkConnectionState.WIFI_SELECTING;
            SelectedWifiNetworkSSID = "";
            return base.Initialize();
        }

        private void handleConnectionTimerElapsed(object sender, EventArgs eventArgs)
        {
            ShowWifiNetworkConnectedPageCommand.Execute();
            _deviceConnectionService.StopConnectionTimer();
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

        public void RenderStateCondtionally(WifiNetworkConnectionState state)
        {
            switch (state)
            {
                case WifiNetworkConnectionState.WIFI_SELECTING:
                    ShouldDisplayWifiSelectionPage = true;
                    ShouldDisplayWifiNetworkPasswordInsertPage = false;
                    ShouldDisplayWifiConnectingPage = false;
                    ShouldDisplayWifiConnectedPage = false;
                    break;
                case WifiNetworkConnectionState.WIFI_PASSWORD_ENTRY:
                    ShouldDisplayWifiSelectionPage = false;
                    ShouldDisplayWifiNetworkPasswordInsertPage = true;
                    ShouldDisplayWifiConnectingPage = false;
                    ShouldDisplayWifiConnectedPage = false;
                    break;
                case WifiNetworkConnectionState.WIFI_CONNECTING:
                    ShouldDisplayWifiSelectionPage = false;
                    ShouldDisplayWifiNetworkPasswordInsertPage = false;
                    ShouldDisplayWifiConnectingPage = true;
                    ShouldDisplayWifiConnectedPage = false;
                    break;
                case WifiNetworkConnectionState.WIFI_CONNECTED:
                    ShouldDisplayWifiSelectionPage = false;
                    ShouldDisplayWifiNetworkPasswordInsertPage = false;
                    ShouldDisplayWifiConnectingPage = false;
                    ShouldDisplayWifiConnectedPage = true;
                    break;
            }
        }

        private void ShowWifiNetworkSelectionPage()
        {
            WifiConnectionState = WifiNetworkConnectionState.WIFI_SELECTING;
        }

        private void ShowWifiNetworkPasswordInsertPage()
        {
            WifiConnectionState = WifiNetworkConnectionState.WIFI_PASSWORD_ENTRY;

        }

        private void ShowWifiNetworkConnectingPage()
        {

            WifiConnectionState = WifiNetworkConnectionState.WIFI_CONNECTING;

            _deviceConnectionService.StartConnectionTimer();
        }

        private void ShowWifiWifiNetworkConnectedPage()
        {
            WifiConnectionState = WifiNetworkConnectionState.WIFI_CONNECTED;
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
                RenderStateCondtionally(_wifiConnectionState);
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

        private string _selectedWifiNetworkSSID;
        public string SelectedWifiNetworkSSID {
            get {
                return _selectedWifiNetworkSSID;
            }
            set {
                _selectedWifiNetworkSSID = value;
                RaisePropertyChanged(() => SelectedWifiNetworkSSID);
            }
        }

        private WifiNetworkViewModel _selectedWifiNetworkVM;
        public WifiNetworkViewModel SelectedWifiNetworkVM
        {
            get => _selectedWifiNetworkVM;
            set
            {
                _selectedWifiNetworkVM = value;
                SelectedWifiNetworkSSID = SelectedWifiNetworkVM.ssid;
                RaisePropertyChanged(() => SelectedWifiNetworkVM);
            }
        }

        //Code-snippet generated template for public fields
        private bool _shouldDisplayWifiConnectingPage;
        public bool ShouldDisplayWifiConnectingPage
        {
            get
            {
                return _shouldDisplayWifiConnectingPage;
            }
            set
            {
                (_shouldDisplayWifiConnectingPage) = value;
                RaisePropertyChanged(() => ShouldDisplayWifiConnectingPage);
            }
        }

        //Code-snippet generated template for public fields
        private bool _shouldDisplayWifiSelectionPage;
        public bool ShouldDisplayWifiSelectionPage
        {
            get
            {
                return _shouldDisplayWifiSelectionPage;
            }
            set
            {
                (_shouldDisplayWifiSelectionPage) = value;
                RaisePropertyChanged(() => ShouldDisplayWifiSelectionPage);
            }
        }

        //Code-snippet generated template for public fields
        private bool _shouldDisplayWifiConnectedPage;
        public bool ShouldDisplayWifiConnectedPage
        {
            get
            {
                return _shouldDisplayWifiConnectedPage;
            }
            set
            {
                (_shouldDisplayWifiConnectedPage) = value;
                RaisePropertyChanged(() => ShouldDisplayWifiConnectedPage);
            }
        }

        //Code-snippet generated template for public fields
        private bool _shouldDisplayWifiNetworkPasswordInsertPage;
        public bool ShouldDisplayWifiNetworkPasswordInsertPage
        {
            get
            {
                return _shouldDisplayWifiNetworkPasswordInsertPage;
            }
            set
            {
                (_shouldDisplayWifiNetworkPasswordInsertPage) = value;
                RaisePropertyChanged(() => ShouldDisplayWifiNetworkPasswordInsertPage);
            }
        }
    }
}
