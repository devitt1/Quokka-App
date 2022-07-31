using System.Diagnostics;
using MvvmCross.Forms.Views;
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
