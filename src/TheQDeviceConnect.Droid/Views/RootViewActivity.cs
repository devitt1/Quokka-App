using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

        }
    }
}
