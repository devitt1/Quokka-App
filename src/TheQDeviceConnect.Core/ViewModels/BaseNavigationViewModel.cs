using System;
using Microsoft.Extensions.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace TheQDeviceConnect.Core.ViewModels
{
    public class BaseNavigationViewModel : MvxNavigationViewModel
    {
        
        public BaseNavigationViewModel(ILoggerFactory loggerFactory, IMvxNavigationService navigationService) : base(loggerFactory, navigationService)
        {
        }
    }
}
