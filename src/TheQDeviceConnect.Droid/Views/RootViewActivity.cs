using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Forms.Platforms.Android.Views;

namespace TheQDeviceConnect.Droid.Views
{
    [Activity(Label = "RootViewActivity",
          Theme = "@style/AppTheme",
          ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
          LaunchMode = LaunchMode.SingleTask)]
    public class RootViewActivity : MvxFormsAppCompatActivity
    {
        public RootViewActivity()
        {
            
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState); 
        }
    }
}
