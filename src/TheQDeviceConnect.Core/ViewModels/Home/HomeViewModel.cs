using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using TheQDeviceConnect.Core.ViewModels.Connection.Hotspot;


namespace TheQDeviceConnect.Core.ViewModels.Home
{

    public class HomeViewModel : BaseNavigationViewModel {

        public HomeViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            GoToHotspotConnectionViewModelCommand = new MvxAsyncCommand(GoToHotspotConnectionVMAsync);

        }

        public MvxAsyncCommand GoToHotspotConnectionViewModelCommand { get; private set; }

        private async Task GoToHotspotConnectionVMAsync()
        {
            await NavigationService.Navigate<HotspotConnectionViewModel>();
        }
        public override void ViewAppeared()
        {
            base.ViewAppeared();

        }
    }
}
