namespace Plarium.Geo.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;

    internal static class IPAddressTools
    {
        public static uint ToUInt32(string ip)
        {
            return (uint) IPAddress.NetworkToHostOrder(BitConverter.ToInt32(IPAddress.Parse(ip).GetAddressBytes(), 0));
        }

        public static ulong ToUInt64(string ip)
        {
            ulong ipnum = ulong.MinValue;
            IPAddress address;
            if (IPAddress.TryParse(ip, out address))
            {
                byte[] addrBytes = address.GetAddressBytes();
                
                if (BitConverter.IsLittleEndian)
                {
                    var byteList = new List<byte>(addrBytes);
                    byteList.Reverse();
                    addrBytes = byteList.ToArray();
                }

                if (addrBytes.Length > 8)
                {
                    //IPv6
                    ipnum = BitConverter.ToUInt64(addrBytes, 8);
                    ipnum <<= 64;
                    ipnum += BitConverter.ToUInt64(addrBytes, 0);
                }
                else
                {
                    //IPv4
                    ipnum = BitConverter.ToUInt32(addrBytes, 0);
                }
            }

            return ipnum;
        }

        public static uint GetIPv4(this IPAddress ip)
        {
            return (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ip.GetAddressBytes(), 0));
        }

        public static ulong GetIPv6(this IPAddress ip)
        {
            return (ulong)IPAddress.NetworkToHostOrder(BitConverter.ToInt64(ip.GetAddressBytes(), 0));
        }

        public static bool IsIPv4(this IPAddress address)
        {
            return address.AddressFamily == AddressFamily.InterNetwork;
        }

        public static bool TryParse(string ipString, out IPAddress address)
        {
            if (!IPAddress.TryParse(ipString, out address))
            {
                address = IPAddress.None;
                return false;
            }

            if (address.AddressFamily != AddressFamily.InterNetwork
                && address.AddressFamily != AddressFamily.InterNetworkV6)
            {
                address = IPAddress.None;
                return false;
            }

            //TODO: is loopback, link-local, reserved etc.
            return true;
        }

        public static string FromUInt32(uint ip)
        {
            return IPAddress.Parse(ip.ToString()).ToString();
        }

        public static uint FromSubnetMask(string mask)
        {
            var net = mask.Split('/');
            var calculator = IPNetwork.Default;
            calculator.Calculate(net[0], net[1]);
            return ToUInt32(calculator.FirstUsable);
        }
    }
}
