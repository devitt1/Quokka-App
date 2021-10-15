using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MvvmCross.Base;
using MvvmCross.Forms.Views;
using MvvmCross.ViewModels;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;
using TheQDeviceConnect.Core.ViewModels.Connection.Hotspot;
using Xamarin.Forms;

namespace TheQDeviceConnect.UI.Pages
{
    public partial class HotspotConnectionPage : MvxContentPage<HotspotConnectionViewModel>
    {
        IDeviceConnectionService _deviceConnectionService;
        public HotspotConnectionPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _deviceConnectionService = DependencyService.Get<IDeviceConnectionService>();
            _deviceConnectionService.Initialize(); 
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            this.ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "NetworkChanged")
            {
                DebugHelper.Info(this, "called");
                _deviceConnectionService.GetCurrentNetwork();
            }
        }
    }
}
