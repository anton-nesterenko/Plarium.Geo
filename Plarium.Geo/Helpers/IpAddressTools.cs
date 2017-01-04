using System.Linq;

namespace Plarium.Geo.Helpers
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    public static class IPAddressTools
    {
         static ulong ToUInt64(byte[] addrBytes)
        {
            ulong ipnum = ulong.MinValue;
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(addrBytes, 0, addrBytes.Length);
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
            return ipnum;
        }

         static ulong ToUInt64(string ip)
        {
            ulong ipnum = ulong.MinValue;
            IPAddress address;
            if (IPAddress.TryParse(ip, out address))
            {
                byte[] addrBytes = address.GetAddressBytes();

                ipnum = ToUInt64(addrBytes);
            }

            return ipnum;
        }

        public static uint GetIPv4(this IPAddress ip)
        {
            //return (uint)IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ip.GetAddressBytes(), 0));
            return (uint)ToUInt64(ip.GetAddressBytes());
        }

        public static ulong GetIPv6(this IPAddress ip)
        {
            //return (ulong)IPAddress.NetworkToHostOrder(BitConverter.ToInt64(ip.GetAddressBytes(), 0));
            //return ToUInt64(ip.GetAddressBytes());
            return BitConverter.ToUInt64(ip.GetAddressBytes().Take(8).Reverse().ToArray(), 0);
        }

        public static bool IsIPv4(this IPAddress address)
        {
            if (address.AddressFamily == AddressFamily.InterNetworkV6)
            {
                if (address.IsIPv4MappedToIPv6)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
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
            
            // checking martian packets
            if (address.IsIPv4())
            {
                //address.
            }
            else
            {
                
            }
            
            return true;
        }
        
        public static System.Net.IPNetwork FromSubnetMask(string mask)
        {
            var net = mask.Split('/');
            return System.Net.IPNetwork.Parse(net[0], Convert.ToByte(net[1]));
            //return ToUInt64(calculator.FirstUsable.GetAddressBytes());
        }
    }
}
