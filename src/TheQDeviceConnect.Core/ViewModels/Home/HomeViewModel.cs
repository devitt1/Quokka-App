using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;
using TheQDeviceConnect.Core.Utils;
using TheQDeviceConnect.Core.ViewModels.Connection;
using TheQDeviceConnect.Core.ViewModels.Connection.Hotspot;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace TheQDeviceConnect.Core.ViewModels.Home
{

    public class HomeViewModel : BaseNavigationViewModel {

        public HomeViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            GoToHotspotConnectionViewModelCommand = new MvxAsyncCommand(GoToHotspotConnectionVMAsync, allowConcurrentExecutions: true);
            _deviceConnectionService = DependencyService.Get<IDeviceConnectionService>();
            _coreDeviceConnectionService = Mvx.IoCProvider.Resolve<IDeviceConnectionService>();
            _deviceConnectionService.OnAndroidNsdResolved += handleAndroidNsdResolved;

        }

        public MvxAsyncCommand GoToHotspotConnectionViewModelCommand { get; private set; }
        IDeviceConnectionService _coreDeviceConnectionService; //Service from core
        IDeviceConnectionService _deviceConnectionService; // Native

        private async Task GoToHotspotConnectionVMAsync()
        {
            await Permissions.RequestAsync<Permissions.StorageWrite>();
            await Permissions.RequestAsync<Permissions.StorageRead>();
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            
            if (SecureStorage.GetAsync("DeviceResolvedLocalAddress").Result == null)
            {
                await NavigationService.Navigate<HotspotConnectionViewModel>();
            }
            else
            {
                await NavigationService.Navigate<WifiConnectionViewModel>();
            }
        }

        public override Task Initialize()
        {
            try
            {
                _deviceConnectionService.InitializeAndroidNsd();
            }
            catch (Exception e)
            {
                DebugHelper.Error(this, e);

            }
            return base.Initialize();
        }

        private void handleAndroidNsdResolved(object sender, EventArgs eventArgs)
        {
            SimpleServiceResolvedEventArgs e = eventArgs as SimpleServiceResolvedEventArgs;
            DebugHelper.Info(this, "handleAndroidNsdResolved, Host = " + e.Host);
            SecureStorage.SetAsync("DeviceResolvedLocalAddress", e.Host);
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            _deviceConnectionService.DiscoverNeabymDNSServices();
            //try
            //{
            //    if (SecureStorage.GetAsync("DeviceResolvedLocalAddress").Result == null)
            //    {
            //        _deviceConnectionService.DiscoverNeabymDNSServices();
            //    }
            //    else
            //    {
            //        NavigationService.Navigate<WifiConnectionViewModel>();
            //    }
            //}
            //catch (Exception e)
            //{
            //    DebugHelper.Error(this, e);

            //}
        }

        public override void ViewDisappeared()
        {
            base.ViewDisappeared();
            _deviceConnectionService.StopDiscoverNearbymDNSServices();
        }
    }
}
