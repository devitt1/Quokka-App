using System.Diagnostics;
using Foundation;
using Lottie.Forms.Platforms.Ios;
using MvvmCross.Forms.Platforms.Ios.Core;
using UIKit;

namespace TheQDeviceConnect.iOS
{
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : MvxFormsApplicationDelegate<Setup, Core.App, UI.App>
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

           

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

    }
}
