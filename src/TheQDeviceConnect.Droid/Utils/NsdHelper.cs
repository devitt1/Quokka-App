using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Net.Nsd;
using Android.Runtime;
using Java.Net;
using TheQDeviceConnect.Core.Helpers;
using TheQDeviceConnect.Core.Utils;
using Zeroconf;
using static Android.Net.Nsd.NsdManager;

namespace TheQDeviceConnect.Droid.Utils
{
    public class NsdHelper
    {
        public static readonly string SERVICE_NAME = "theqbox1";
        public static readonly string SERVICE_TYPE = "_theq._tcp.";

        public RegistrationListener RegistrationListener { get; set; }
        public DiscoveryListener DiscoveryListener { get; set; }
        public ResolveListener ResolveListener { get; set; }

        public NsdManager NsdManager;
        public NsdServiceInfo NsdServiceInfo { get; private set; }

        public event EventHandler NsdResolved;

        public NsdHelper(Context context)
        {
            NsdManager = (NsdManager)context.GetSystemService(Context.NsdService);

        }

        public void Initialize()
        {
            NsdServiceInfo = new NsdServiceInfo
            {
                ServiceName = SERVICE_NAME,
                ServiceType = SERVICE_TYPE,
                Port = 5555,
            };


            RegistrationListener = new RegistrationListener(this);
            ResolveListener = new ResolveListener(this);
            DiscoveryListener = new DiscoveryListener(this);

            DiscoveryListener.ServiceType = SERVICE_TYPE;
            //TearDown();

        }

        public void OnDiscoveryTimerElapsed()
        {
            DebugHelper.Info(this, "Discovery Timer Elapsed!");
            DiscoveryListener.DiscoveryTimer.Stop();
        }

        public void OnResolveTimerElapsed()
        {
            DebugHelper.Info(this, "Resolve Timer Elapsed!");
            ResolveListener.ResolveTimer.Stop();
        }

        public void RegisterService()
        {

            // The name is subject to change based on conflicts
            // with other services advertised on the same network.
            try
            {
                NsdManager.RegisterService(
                        NsdServiceInfo, NsdProtocol.DnsSd, this.RegistrationListener);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void OnServiceResolved(SimpleServiceResolvedEventArgs e)
        {
            NsdResolved.Invoke(this, e); 
        }


        private int GenerateFreePort()
        {
            //setting the ServerSocket to 0 will generate the next free port
            var serverSocket = new ServerSocket(0);
            return serverSocket.LocalPort;
        }

        public void DiscoverServices()
        {
            try
            {

                NsdManager.DiscoverServices(SERVICE_TYPE, NsdProtocol.DnsSd, DiscoveryListener);
                //DiscoveryListener.DiscoveryTimer.Start();

            }

            catch (Exception e)
            {
                DebugHelper.Error(this, e);
            }
            

        }

        public void StopDiscovery()
        {
            NsdManager.StopServiceDiscovery(DiscoveryListener);
        }

        public void TearDown()
        {
            this.NsdManager.UnregisterService(this.RegistrationListener);
        }
    }

    public class RegistrationListener : Java.Lang.Object, IRegistrationListener
    {
        private NsdHelper _nsdHelper;
        public RegistrationListener(NsdHelper nsdHelper)
        {
            _nsdHelper = nsdHelper;
        }
        public void OnRegistrationFailed(NsdServiceInfo serviceInfo, [GeneratedEnum] NsdFailure errorCode)
        {
            DebugHelper.Error(this, serviceInfo + "\nERROR_CODE: " + errorCode);
            //throw new NotImplementedException();
        }

        public void OnServiceRegistered(NsdServiceInfo serviceInfo)
        {
            DebugHelper.Info(this, serviceInfo.ServiceName);

            //_nsdHelper.Se = serviceInfo.ServiceName;
        }

        public void OnServiceUnregistered(NsdServiceInfo serviceInfo)
        {
            throw new NotImplementedException();
        }

        public void OnUnregistrationFailed(NsdServiceInfo serviceInfo, [GeneratedEnum] NsdFailure errorCode)
        {
            throw new NotImplementedException();
        }
    }

    public class DiscoveryListener : Java.Lang.Object, IDiscoveryListener
    {
        public string ServiceName;
        public string ServiceType;
        private NsdHelper _nsdHelper;
        public Timer DiscoveryTimer;
        private int _discoveryTimeoutValue;

        public DiscoveryListener(NsdHelper nsdHelper)
        {
            _nsdHelper = nsdHelper;
            //_discoveryTimeoutValue = 5000;
            //DiscoveryTimer = new Timer(_discoveryTimeoutValue);

        }
        public void OnDiscoveryStarted(string serviceType)
        {
            DebugHelper.Info(this, $"Start Discovery service Type : {serviceType}, Time out after {_discoveryTimeoutValue}");
            //DiscoveryTimer.Elapsed += handleTimerElapsed;
        }

        private void handleTimerElapsed(object sender, ElapsedEventArgs eventArgs)
        {
            _nsdHelper.OnDiscoveryTimerElapsed();
        }

        public void OnDiscoveryStopped(string serviceType)
        {
            DebugHelper.Info(this, serviceType);
        }

        public void OnServiceFound(NsdServiceInfo serviceInfo)
        {
            DebugHelper.Info(this, serviceInfo);
            string name = serviceInfo.ServiceName;
            string type = serviceInfo.ServiceType;
            DebugHelper.Info(this, $"Name : {name} \n" +
                $"Service Type: {type}");
            if (type.Equals(this.ServiceType) && name.Contains("theqbox1"))
            {
                DebugHelper.Info(this, "Service found @ '" + name + "'");
                _nsdHelper.NsdManager.ResolveService(serviceInfo, _nsdHelper.ResolveListener);
                _nsdHelper.ResolveListener.ResolveTimer.Start();
            }
        }

        public void OnServiceLost(NsdServiceInfo serviceInfo)
        {
            DebugHelper.Info(this, serviceInfo);
        }

        public void OnStartDiscoveryFailed(string serviceType, [GeneratedEnum] NsdFailure errorCode)
        {
            DebugHelper.Error(this, serviceType);
        }

        public void OnStartDiscoveyFailed(string serviceType, [GeneratedEnum] NsdFailure errorCode)
        {
            DebugHelper.Error(this, serviceType);
        }

        public void OnStopDiscoveryFailed(string serviceType, [GeneratedEnum] NsdFailure errorCode)
        {
            DebugHelper.Error(this, serviceType);
        }
    }

    public class ResolveListener : Java.Lang.Object, IResolveListener
    {
        private NsdHelper _nsdHelper;
        public Timer ResolveTimer { get; set; }

        public ResolveListener(NsdHelper nsdHelper)
        {
            DebugHelper.Info(this, "ResolveListener Created");
            _nsdHelper = nsdHelper;
            ResolveTimer = new Timer(5000);
            //ResolveTimer.Elapsed += handleTimerElapsed;

        }

        private void handleTimerElapsed(object sender, ElapsedEventArgs eventArgs)
        {
            //_nsdHelper.OnResolveTimerElapsed();
        }

        public void OnResolveFailed(NsdServiceInfo serviceInfo, [GeneratedEnum] NsdFailure errorCode)
        {
            DebugHelper.Error(this, $"{serviceInfo} \n Error code: {errorCode}");
            ResolveTimer.Stop();
        }

        public void OnServiceResolved(NsdServiceInfo serviceInfo)
        {
            var host = serviceInfo.Host;
            var port = serviceInfo.Port;

            DebugHelper.Info(this, $"Host: {host}\nPort: {port}");
            ResolveTimer.Stop();

            SimpleServiceResolvedEventArgs e = new SimpleServiceResolvedEventArgs(host.ToString(), port);

            _nsdHelper.OnServiceResolved(e);

        }
    }
}
