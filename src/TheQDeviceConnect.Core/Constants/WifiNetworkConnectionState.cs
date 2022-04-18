using System;
namespace TheQDeviceConnect.Core.Constants
{
    public enum WifiNetworkConnectionState
    {
        DEFAULT, // Initial connection, no configuration made
        HOTSPOT_CONNECTING,// User made configuration, waiting for connection
        HOTSPOT_CONNECTED,
        WIFI_SELECTING,
        WIFI_PASSWORD_ENTRY,
        WIFI_CONNECTING,
        WIFI_CONNECTED_CONFIRM,
        WIFI_CONNECTED

    }
    
}
