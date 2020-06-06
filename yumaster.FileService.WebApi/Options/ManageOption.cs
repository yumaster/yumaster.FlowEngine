using System.Collections.Generic;
using System.Net;

namespace yumaster.FileService.WebApi.Options
{
    public class ManageOption
    {
        private static Dictionary<string, object> _localIpAddresses;
        private static readonly object LocalIpAddressesLock = new object();

        public string[] IpWhitelist { get; set; }

        /// <summary>
        /// 判断指定IP是否是本机IP
        /// </summary>
        public bool IsLocalIp(string ip)
        {
            Dictionary<string, object> localIps = null;
            lock (LocalIpAddressesLock)
            {
                if (_localIpAddresses == null)
                {
                    _localIpAddresses = new Dictionary<string, object>
                    {
                        [IPAddress.Loopback.ToString()] = null,
                        [IPAddress.IPv6Loopback.ToString()] = null
                    };
                }
                localIps = _localIpAddresses;
            }

            return localIps.ContainsKey(ip);
        }
    }
}
