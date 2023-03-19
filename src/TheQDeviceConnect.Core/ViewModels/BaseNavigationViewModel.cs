using System;
using System.ComponentModel;
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

        //Task
        private MvxNotifyTask _taskNotifier;

        public event PropertyChangedEventHandler PropertyChanged;

        public MvxNotifyTask TaskNotifier
        {
            get => _taskNotifier;
            set => SetProperty(ref _taskNotifier, value);
        }

    }

    public abstract class BaseNavigationViewModel<Parameter> :
         MvxNavigationViewModel<Parameter> where Parameter : class
    {
        public BaseNavigationViewModel(ILoggerFactory logFactory, IMvxNavigationService navigationService) : base(logFactory, navigationService)
        {
        }

        //Task
        private MvxNotifyTask _taskNotifier;
        public MvxNotifyTask TaskNotifier
        {
            get => _taskNotifier;
            set => SetProperty(ref _taskNotifier, value);
        }
        public override void Prepare(Parameter parameter)
        {

        }
    }
}
