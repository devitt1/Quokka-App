using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using TheQDeviceConnect.Core.ViewModels.Connection;
using Xamarin.Forms;

namespace TheQDeviceConnect.UI.Pages
{
    public partial class WifiConnectionPage : MvxContentPage<WifiConnectionViewModel>
    {
        public WifiConnectionPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

    }
}
