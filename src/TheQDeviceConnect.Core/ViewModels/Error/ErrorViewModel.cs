using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using TheQDeviceConnect.Core.ViewModels.Connection.Hotspot;
using ErrorModel = TheQDeviceConnect.Core.DataModels.Error;
namespace TheQDeviceConnect.Core.ViewModels.Error
{
    public class ErrorViewModel : BaseNavigationViewModel<ErrorModel>
    {
        //Code-snippet generated template for public fields
        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                (_message) = value;
                RaisePropertyChanged(() => Message);
            }
        }

        public ErrorViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            TryAgainCommand = new MvxAsyncCommand(TryAgainAsync, allowConcurrentExecutions: true);


        }
        public MvxAsyncCommand TryAgainCommand { get; private set; }

        private async Task TryAgainAsync()
        {
            await NavigationService.Navigate<HotspotConnectionViewModel>();
        }

        public override void Prepare(ErrorModel error)
        {
            base.Prepare();
        }
    }
}
