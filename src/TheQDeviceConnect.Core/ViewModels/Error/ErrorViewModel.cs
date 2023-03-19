using System;
using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;
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

        }

        public override void Prepare(ErrorModel error)
        {
            base.Prepare();
        }
    }
}
