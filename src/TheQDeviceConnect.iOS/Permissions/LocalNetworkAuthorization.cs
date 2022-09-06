using System;
using System.Diagnostics;
using Foundation;
using Network;

namespace TheQDeviceConnect.iOS.Permissions
{
    public class LocalNetworkAuthorization : NSObject
    {
        private NWBrowser _NWBrowser;
        private NSNetService _netService;
        private bool _completion;

        public LocalNetworkAuthorization()
        {
        }

        public void RequestAuthorization(bool completion)
        {
            _completion = completion;

            NWParameters parameters = new NWParameters();
            parameters.IncludePeerToPeer = true;

            NWBrowserDescriptor descriptor = NWBrowserDescriptor.CreateBonjourService("_bonjour._tcp", null);
            NWBrowser browser = new NWBrowser(descriptor, parameters: parameters);

            _NWBrowser = browser;

            _NWBrowser.SetStateChangesHandler((newState, error) =>
            {
                switch (newState) {
                    case NWBrowserState.Invalid:
                        Debug.WriteLine("State Invalid");

                        break;
                    case NWBrowserState.Ready:
                        Debug.WriteLine("State Ready");

                        break;
                    case NWBrowserState.Failed:
                        Debug.WriteLine("State Failed");

                        break;
                    case NWBrowserState.Cancelled:
                        Debug.WriteLine("State Canceled");
                        break;
                    default:
                        throw new Exception("Unexpected state changed exception");
                }
            });

            _netService = new NSNetService("local.", "_lnp._tcp", "LocalNetworkPrivacy", 1110);

            _netService.Delegate = new Authorization();



            _netService.Publish();
            _NWBrowser.Start();
        }

        private void reset()
        {

        }
    } 

    public class Authorization : NSNetServiceDelegate
    {
        [Export("netServiceDidPublish:")]
        public virtual void Publish(NSNetService sender) {
            Debug.WriteLine("Did published?");

        }
    }
}
