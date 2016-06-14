namespace Plarium.Geo.Helpers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Text;

    public class SubnetHelper
    {
        private static SubnetHelper _default;

        public string IP { get; private set; }

        public string Netmask { get; private set; }

        //public string NetmaskBinary { get; private set; }
        public string NetworkAddress { get; private set; }

        public string FirstUsable { get; private set; }

        public string LastUsable { get; private set; }

        public string Broadcast { get; private set; }

        //public string NumberOfSubnets { get; private set; }
        //public string NumberOfHosts { get; private set; }

        private SubnetHelper()
        {
        }

        public static SubnetHelper Default
        {
            get
            {
                return _default ?? (_default = new SubnetHelper());
            }
        }

        public void Calculate(string ip, string cidr)
        {
            this.IP = ip;
            this.CalculateNetmask(cidr);
            this.CalculateNetworkAddress();
            this.CalculateBroadcastAddress();
            this.CalculateFirstUsableAddress();
            this.CalculateLastUsableAddress();
            //CalculateHosts(cidr);
            //CalculateSubnets(cidr);
            //CalculateBinaryNetmask();
        }

        /*
        private void CalculateSubnets(string cidr)
        {
            int cidrAsInt = int.Parse(cidr);
            int borrowedBits = 0;
            if (cidrAsInt >= 24)
                borrowedBits = cidrAsInt - 24;
            else if (cidrAsInt >= 16 && cidrAsInt < 24)
                borrowedBits = cidrAsInt - 16;
            else if (cidrAsInt >= 8 && cidrAsInt < 16)
                borrowedBits = cidrAsInt - 8;
            else if (cidrAsInt >= 0 && cidrAsInt < 8)
                borrowedBits = cidrAsInt;

            this.NumberOfSubnets = Math.Pow(2, borrowedBits).ToString();
        }*/
        /*
        private void CalculateHosts(string cidr)
        {
            int cidrAsInt = int.Parse(cidr);
            this.NumberOfHosts = (Math.Pow(2, (32 - cidrAsInt)) - 2).ToString();
        }*/

        private void CalculateLastUsableAddress()
        {
            byte[] broadcastBytes = IPAddress.Parse(this.Broadcast).GetAddressBytes();
            broadcastBytes[3]--;
            this.LastUsable = new IPAddress(broadcastBytes).ToString();
        }

        private void CalculateFirstUsableAddress()
        {
            byte[] networkBytes = IPAddress.Parse(this.NetworkAddress).GetAddressBytes();
            networkBytes[3]++;
            this.FirstUsable = new IPAddress(networkBytes).ToString();
        }

        private void CalculateBroadcastAddress()
        {
            byte[] ipBytes = IPAddress.Parse(this.IP).GetAddressBytes();
            byte[] netmaskBytes = IPAddress.Parse(this.Netmask).GetAddressBytes();
            byte[] broadcastBytes = new byte[ipBytes.Length];

            for (int i = 0; i < broadcastBytes.Length; i++) broadcastBytes[i] = (byte)(ipBytes[i] | (netmaskBytes[i] ^ 255));

            this.Broadcast = new IPAddress(broadcastBytes).ToString();
        }

        private void CalculateNetworkAddress()
        {
            byte[] ipBytes = IPAddress.Parse(this.IP).GetAddressBytes();
            byte[] netmaskBytes = IPAddress.Parse(this.Netmask).GetAddressBytes();
            byte[] networkBytes = new byte[ipBytes.Length];

            for (int i = 0; i < networkBytes.Length; i++) networkBytes[i] = (byte)(ipBytes[i] & (netmaskBytes[i]));

            this.NetworkAddress = new IPAddress(networkBytes).ToString();
        }

        private void CalculateNetmask(string cidr)
        {
            uint mask = ~(0xFFFFFFFF >> int.Parse(cidr));
            byte[] bytes = BitConverter.GetBytes(mask);
            this.Netmask = string.Join(".", bytes.Reverse());
        }

        /*
        private void CalculateBinaryNetmask()
        {
            byte[] tempMask = IPAddress.Parse(this.Netmask).GetAddressBytes();
            StringBuilder netmask = new StringBuilder();

            for (int i = 0; i < tempMask.Length; i++)
            {
                netmask.Append(Convert.ToString(tempMask[i], 2).PadLeft(8, '0'));
                netmask.Append(".");
            }
            netmask.Remove(netmask.Length - 1, 1);
            this.NetmaskBinary = netmask.ToString();
        }*/

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("IP:" + this.IP);
            builder.AppendLine("Netmask:" + this.Netmask);
            builder.AppendLine("NetworkAddress:" + this.NetworkAddress);
            builder.AppendLine("FirstUsable:" + this.FirstUsable);
            builder.AppendLine("LastUsable:" + this.LastUsable);
            builder.AppendLine("Broadcast:" + this.Broadcast);
            //builder.AppendLine("NumberOfSubnets:" + NumberOfSubnets);
            //builder.AppendLine("NumberOfHosts:" + NumberOfHosts);
            return builder.ToString();
        }
    }
}
