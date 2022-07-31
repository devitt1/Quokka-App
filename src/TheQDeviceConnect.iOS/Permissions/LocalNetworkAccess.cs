using System.Threading.Tasks;
using Xamarin.Essentials;
using static Xamarin.Essentials.Permissions;

namespace TheQDeviceConnect.iOS.Permissions
{
    public class LocalNetworkAccess : BasePermission
    {
        public LocalNetworkAccess()
        {

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
            throw new System.NotImplementedException();
        }

        public override bool ShouldShowRationale()
        {
            throw new System.NotImplementedException();
        }
    }
}
