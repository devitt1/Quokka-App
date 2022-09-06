using System.Threading.Tasks;
using Xamarin.Essentials;
using static Xamarin.Essentials.Permissions;

namespace TheQDeviceConnect.iOS.Permissions
{
    public class LocalNetworkAccess : BasePlatformPermission 
    {
        LocalNetworkAuthorization _localNetworkAuthorization;
        public LocalNetworkAccess()
        {
            _localNetworkAuthorization = new LocalNetworkAuthorization();
        }

        public override Task<PermissionStatus> CheckStatusAsync()
        {
            throw new System.NotImplementedException();
        }

        public override void EnsureDeclared()
        {
            throw new System.NotImplementedException();
        }

        public override Task<PermissionStatus> RequestAsync()
        {
            _localNetworkAuthorization.RequestAuthorization(true);
            return Task.FromResult<PermissionStatus>(0);
        }

        public override bool ShouldShowRationale()
        {
            throw new System.NotImplementedException();
        }
    }
}
