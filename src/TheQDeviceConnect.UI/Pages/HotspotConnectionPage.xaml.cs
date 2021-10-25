using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MvvmCross.Base;
using MvvmCross.Forms.Views;
using MvvmCross.ViewModels;
using TheQDeviceConnect.Core.Constants;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Services.Interfaces;
using TheQDeviceConnect.Core.ViewModels.Connection.Hotspot;
using Xamarin.Forms;

namespace TheQDeviceConnect.UI.Pages
{
    public partial class HotspotConnectionPage : MvxContentPage<HotspotConnectionViewModel>
    {
        public HotspotConnectionPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
        }

    }
}
