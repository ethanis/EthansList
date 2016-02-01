using System;
using System.Net;
using SystemConfiguration;
using CoreFoundation;

namespace Reachability
{
    public enum NetworkStatus
    {
        NotReachable,
        ReachableViaCarrierDataNetwork,
        ReachableViaWiFiNetwork
    }

    public static class Reachability
    {
        private static NetworkReachability _defaultRouteReachability;

        public static event EventHandler ReachabilityChanged;

        public static bool IsNetworkAvailable()
        {
            if (_defaultRouteReachability == null)
            {
                _defaultRouteReachability = new NetworkReachability(new IPAddress(0));
                _defaultRouteReachability.SetNotification(OnChange);
                _defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }

            NetworkReachabilityFlags flags;

            return _defaultRouteReachability.TryGetFlags(out flags) && 
                IsReachableWithoutRequiringConnection(flags);
        }

        private static bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
        {
            // Is it reachable with the current network configuration?
            bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

            // Do we need a connection to reach it?
            bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;

            // Since the network stack will automatically try to get the WAN up,
            // probe that
            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                noConnectionRequired = true;

            return isReachable && noConnectionRequired;
        }

        private static void OnChange(NetworkReachabilityFlags flags)
        {
            var h = ReachabilityChanged;
            if (h != null)
                h(null, EventArgs.Empty);
        }
    }
}