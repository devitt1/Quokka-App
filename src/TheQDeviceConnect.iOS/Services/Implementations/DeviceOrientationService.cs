using System;
using TheQDeviceConnect.Core.Services.Interfaces;
using TheQDeviceConnect.iOS.Services.Implementations;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceOrientationService))]
namespace TheQDeviceConnect.iOS.Services.Implementations
{
    public class DeviceOrientationService : IDeviceOrientationService
    {
        public DeviceOrientationService()
        {
        }

        public DeviceOrientation GetOrientation()
        {
            UIInterfaceOrientation orientation = UIApplication.SharedApplication.StatusBarOrientation;

            bool isPortrait = orientation == UIInterfaceOrientation.Portrait ||
                orientation == UIInterfaceOrientation.PortraitUpsideDown;
            return isPortrait ? DeviceOrientation.Portrait : DeviceOrientation.Landscape;
        }
    }
}
