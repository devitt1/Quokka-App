using System;
namespace TheQDeviceConnect.Core.DataModels
{
    public class WifiNetworkCredential
    {
        public WifiNetworkCredential()
        {
        }

        public string ssid;
        public string password;
        public string auth_mgnt;
        public string peap_username;
        public string peap_password;
    }
}
