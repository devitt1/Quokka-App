using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using TheQDeviceConnect.Core.Constants;
using TheQDeviceConnect.Core.DataModels;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;
using TheQDeviceConnect.Core.Utils;
using TheQDeviceConnect.Core.ViewModels.Converters;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TheQDeviceConnect.Core.ViewModels.Connection
{
    public class WifiConnectionViewModel: BaseNavigationViewModel
    {
        public WifiConnectionViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            _deviceConnectionService = DependencyService.Get<IDeviceConnectionService>();
            _deviceConnectionService.OnConnectionTimerElapsed += handleConnectionTimerElapsed;
            _deviceConnectionService.OnWifiNetworkChanged += handleWifiNetworkChanged;
            _deviceConnectionService.OnAndroidNsdResolved += handleAndroidNsdResolved;

            _coreDeviceConnectionService = Mvx.IoCProvider.Resolve<IDeviceConnectionService>();

            CloseCommand = new MvxAsyncCommand(CloseAsync, allowConcurrentExecutions: true);
            ShowWifiNetworkPasswordInsertPageCommand = new MvxCommand(ShowWifiNetworkPasswordInsertPage);
            ShowWifiNetworkSelectionPageCommand = new MvxCommand(ShowWifiNetworkSelectionPage);
            ShowWifiNetworkConnectingPageCommand = new MvxCommand(ShowWifiNetworkConnectingPage);
            ShowWifiNetworkConnectedConfirmPageCommand = new MvxCommand(ShowWifiNetworkConnectedConfirmPage);
            ShowWifiNetworkConnectedPageCommand = new MvxCommand(ShowWifiWifiNetworkConnectedPage);
        }

        IDeviceConnectionService _coreDeviceConnectionService; //Service from core
        IDeviceConnectionService _deviceConnectionService; // Native 
        public IMvxAsyncCommand CloseCommand { get; private set; }
        public IMvxCommand ShowWifiNetworkSelectionPageCommand { get; private set; }
        public IMvxCommand ShowWifiNetworkPasswordInsertPageCommand { get; private set; }
        public IMvxCommand ShowWifiNetworkConnectingPageCommand { get; private set; }
        public IMvxCommand ShowWifiNetworkConnectedConfirmPageCommand { get; private set; }
        public IMvxCommand ShowWifiNetworkConnectedPageCommand { get; private set; }


        private MvxNotifyTask _taskNotifier;
        public MvxNotifyTask TaskNotifier
        {
            get => _taskNotifier;
            private set => SetProperty(ref _taskNotifier, value);
        }


        private async Task GetNearbyWifiNetworksAsync()
        {
            MvxObservableCollection<WifiNetwork> result = await _coreDeviceConnectionService.GetNearbyWifiNetworksAsync();

            if (result.Count > 0)
            {
                WifiNetworkVMs = WifiNetworkConverter.Convert(result);
                registerPropertyChangedEventHandler(WifiNetworkVMs);
            }
        }

        private async Task CheckInternetReachabilityAsync()
        {
            if (await _coreDeviceConnectionService.IsInternetReachable())
            {
                _deviceConnectionService.StartConnectionTimer();

                //Only needed for Android applications
                DebugHelper.Info(this, "Connected to the internet, waiting for mDNS service to be online!\n");
                Thread.Sleep(2000);
                startAndroidNsd();
                DebugHelper.Info(this, "Wait time over, beginning discovery process...\n");

            }
        }

        private void startAndroidNsd()
        {
            _deviceConnectionService.InitializeAndroidNsd();
            _deviceConnectionService.DiscoverNeabymDNSServices();
        }
        private async Task CloseAsync()
        {
            await NavigationService.Close(this);
        }

        public override void Prepare()
        {
            base.Prepare();
        }

        public override Task Initialize()
        {
            WifiConnectionState = WifiNetworkConnectionState.WIFI_SELECTING;
            SelectedWifiNetworkSSID = "";
            SelectedWifiNetworkPassword = "";
            SelectedWifiNetworkAuthMgnt = "psk";
            return base.Initialize();
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();
            TaskNotifier = MvxNotifyTask.Create(GetNearbyWifiNetworksAsync);
        }
        public override void ViewAppeared()
        {
            if (SecureStorage.GetAsync("DeviceResolvedLocalAddress").Result != null)
            {
                ShowWifiNetworkConnectedPageCommand.Execute();
            }
            _deviceConnectionService.DiscoverNeabymDNSServices();
            base.ViewAppeared();
        }

        public override void ViewDisappeared()
        {
            _deviceConnectionService.StopDiscoverNearbymDNSServices();
            base.ViewDisappeared();
        }

        private void handleConnectionTimerElapsed(object sender, EventArgs eventArgs)
        {
            ShowWifiNetworkConnectedConfirmPageCommand.Execute();
            _deviceConnectionService.StopConnectionTimer();
        }

        
        private void handleWifiNetworkChanged(object sender, EventArgs e)
        {
            DebugHelper.Info(this, "Connection changed!");
            if (_deviceConnectionService.CurrentConnectedNetworkSSID == $"\"{SelectedWifiNetworkSSID}\""
                || _deviceConnectionService.CurrentConnectedNetworkSSID == $"{ SelectedWifiNetworkSSID}")
            {
                DebugHelper.Info(this, $"Connected to the selected Wifi Network: {SelectedWifiNetworkSSID}");
                MvxNotifyTask task = MvxNotifyTask.Create(CheckInternetReachabilityAsync);
            }
        }

        private void handleAndroidNsdResolved(object sender, EventArgs eventArgs)
        {
            DebugHelper.Info(this, eventArgs);
            _deviceConnectionService.StopConnectionTimer();
            SimpleServiceResolvedEventArgs e = eventArgs as SimpleServiceResolvedEventArgs;
            Application.Current.Properties["DeviceResolvedLocalAddress"] = e.Host;
            _deviceConnectionService.DeviceResolvedLocalAddress = e.Host;
            ShowWifiNetworkConnectedConfirmPageCommand.Execute();
        }


        private void registerPropertyChangedEventHandler(MvxObservableCollection<WifiNetworkViewModel> wifiNetworkViewModels)
        {
            foreach (IWifiNetworkViewModel wifiNetworkVM in wifiNetworkViewModels)
            {
                wifiNetworkVM.PropertyChanged += handleWifiNetworkVMPropertyChanged;
            }
        }

        private void registerPropertyChangedEventHandler(WifiNetworkViewModel wifiNetworkViewModel)
        {
            wifiNetworkViewModel.PropertyChanged += handleWifiNetworkVMPropertyChanged;

        }

        private void handleWifiNetworkVMPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
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
                    ShouldDisplayWifiConnectedConfirmPage = false;
                    ShouldDisplayWifiConnectedPage = false;

                    break;
                case WifiNetworkConnectionState.WIFI_PASSWORD_ENTRY:
                    ShouldDisplayWifiSelectionPage = false;
                    ShouldDisplayWifiNetworkPasswordInsertPage = true;
                    ShouldDisplayWifiConnectingPage = false;
                    ShouldDisplayWifiConnectedConfirmPage = false;
                    ShouldDisplayWifiConnectedPage = false;
                    break;
                case WifiNetworkConnectionState.WIFI_CONNECTING:
                    ShouldDisplayWifiSelectionPage = false;
                    ShouldDisplayWifiNetworkPasswordInsertPage = false;
                    ShouldDisplayWifiConnectingPage = true;
                    ShouldDisplayWifiConnectedConfirmPage = false;
                    ShouldDisplayWifiConnectedPage = false;
                    break;
                case WifiNetworkConnectionState.WIFI_CONNECTED_CONFIRM:
                    ShouldDisplayWifiSelectionPage = false;
                    ShouldDisplayWifiNetworkPasswordInsertPage = false;
                    ShouldDisplayWifiConnectingPage = false;
                    ShouldDisplayWifiConnectedConfirmPage = true;
                    ShouldDisplayWifiConnectedPage = false;
                    break;
                case WifiNetworkConnectionState.WIFI_CONNECTED:
                    ShouldDisplayWifiSelectionPage = false;
                    ShouldDisplayWifiNetworkPasswordInsertPage = false;
                    ShouldDisplayWifiConnectingPage = false;
                    ShouldDisplayWifiConnectedConfirmPage = false;
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
            _coreDeviceConnectionService.UpdateDeviceWifiNetworkCredential(SelectedWifiNetworkSSID,
                SelectedWifiNetworkPassword, SelectedWifiNetworkAuthMgnt,
                SelectedWifiNetworkEAPUsername, SelectedWifiNetworkEAPPassword);
            _deviceConnectionService.ConnectToWifiNetwork(SelectedWifiNetworkSSID, SelectedWifiNetworkPassword);

        }

        private void ShowWifiNetworkConnectedConfirmPage()
        {
            WifiConnectionState = WifiNetworkConnectionState.WIFI_CONNECTED_CONFIRM;
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

        private MvxObservableCollection<WifiNetworkViewModel> _wifiNetworkVMs = new MvxObservableCollection<WifiNetworkViewModel>();
        public MvxObservableCollection<WifiNetworkViewModel> WifiNetworkVMs
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

        //Code-snippet generated template for public fields
        private string _selectedWifiNetworkPassword;
        public string SelectedWifiNetworkPassword
        {
            get
            {
                return _selectedWifiNetworkPassword;
            }
            set
            {
                (_selectedWifiNetworkPassword) = value;
                RaisePropertyChanged(() => SelectedWifiNetworkPassword);
            }
        }

        //Code-snippet generated template for public fields
        private string _selectedWifiNetworkAuthMgnt;
        public string SelectedWifiNetworkAuthMgnt
        {
            get
            {
                return _selectedWifiNetworkAuthMgnt;
            }
            set
            {
                (_selectedWifiNetworkAuthMgnt) = value;
                RaisePropertyChanged(() => SelectedWifiNetworkAuthMgnt);
            }
        }

        //Code-snippet generated template for public fields
        private string _selectedWifiNetworkEAPUsername;
        public string SelectedWifiNetworkEAPUsername
        {
            get
            {
                return _selectedWifiNetworkEAPUsername;
            }
            set
            {
                (_selectedWifiNetworkEAPUsername) = value;
                RaisePropertyChanged(() => SelectedWifiNetworkEAPUsername);
            }
        }

        //Code-snippet generated template for public fields
        private string _selectedWifiNetworkEAPPassword;
        public string SelectedWifiNetworkEAPPassword
        {
            get
            {
                return _selectedWifiNetworkEAPPassword;
            }
            set
            {
                (_selectedWifiNetworkEAPPassword) = value;
                RaisePropertyChanged(() => SelectedWifiNetworkEAPPassword);
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
        private bool _shoudDisplayWifiConnectedConfirmPage;
        public bool ShouldDisplayWifiConnectedConfirmPage
        {
            get
            {
                return _shoudDisplayWifiConnectedConfirmPage;
            }
            set
            {
                (_shoudDisplayWifiConnectedConfirmPage) = value;
                RaisePropertyChanged(() => ShouldDisplayWifiConnectedConfirmPage);
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
