namespace Plarium.Geo.Helpers
{
    using System;
    using System.Net;
    
    public class IPAddressHelper
    {
        public static uint ToUInt32(string ip)
        {
            return (uint) IPAddress.NetworkToHostOrder(BitConverter.ToInt32(IPAddress.Parse(ip).GetAddressBytes(), 0));
        }

        public static string FromUInt32(uint ip)
        {
            return IPAddress.Parse(ip.ToString()).ToString();
        }

        public static uint FromSubnetMask(string mask)
        {
            var net = mask.Split('/');
            var calculator = SubnetHelper.Default;
            calculator.Calculate(net[0], net[1]);
            return ToUInt32(calculator.FirstUsable);
        }
    }
}
