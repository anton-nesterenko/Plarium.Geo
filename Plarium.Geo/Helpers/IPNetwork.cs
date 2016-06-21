namespace Plarium.Geo.Helpers
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    /*
     * https://github.com/lduchosal/ipnetwork/blob/master/solution/System.Net.IPNetwork/IPNetwork.cs
     * http://www.codeproject.com/Articles/1082101/IP-Geolocation-and-CIDR-Range-Parsing-in-Csharp
     * https://en.wikipedia.org/wiki/Martian_packet
     * https://en.wikipedia.org/wiki/Reserved_IP_addresses
     * https://en.wikipedia.org/wiki/Localhost
     * https://en.wikipedia.org/wiki/Link-local_address
     * https://en.wikipedia.org/wiki/Private_network
     */

    internal class IPNetwork
    {
        private byte[] _ipBytes;
        private byte[] _broadcastBytes;
        private byte[] _netmaskBytes;
        private byte[] _networkBytes;
        private byte[] _firstBytes;
        private byte[] _lastBytes;

        private AddressFamily _family;

        public AddressFamily AddressFamily
        {
            get
            {
                return _family;
            }
        }

        public string IP
        {
            get
            {
                return string.Join(".", _ipBytes);
            }
        }

        public string Netmask
        {
            get
            {
                return string.Join(".", _netmaskBytes);
            }
        }

        public string NetworkAddress
        {
            get
            {
                return string.Join(".", _networkBytes);
            }
        }

        public string FirstUsable
        {
            get
            {
                return string.Join(".", _firstBytes);
            }
        }

        public string LastUsable
        {
            get
            {
                return string.Join(".", _lastBytes);
            }
        }

        public string Broadcast
        {
            get
            {
                return string.Join(".", _broadcastBytes);
            }
        }


        public string[] _ipv4Martians = new string[]
                                                   {
                                                     "0.0.0.0/8",
                                                     "10.0.0.0/8",
                                                     "100.64.0.0/10",
                                                     "127.0.0.0/8",
                                                     "127.0.53.53",
                                                     /*"169.254.0.0/16",
                                                     "172.16.0.0/12",
                                                     "192.0.0.0/24",
                                                     "192.0.2.0/24",
                                                     "192.168.0.0/16",
                                                     "198.18.0.0/15",
                                                     "198.51.100.0/24",
                                                     "203.0.113.0/24",
                                                     "224.0.0.0/4",
                                                     "240.0.0.0/4",
                                                     "255.255.255.255/32"*/
                                                   };
        public string[] _ipv6Martians = new string[]
                                                   {
                                                     "::/128",
                                                     "::1/128",
                                                     //"::ffff:0:0/96",
                                                     //"::/96",
                                                     "100::/64",
                                                     "2001:10::/28",
                                                     "2001:db8::/32",
                                                     "fc00::/7",
                                                     "fe80::/10",
                                                     "fec0::/10",
                                                     "ff00::/8"
                                                   };

        private IPNetwork()
        {


        }

        public static IPNetwork Parse(string ip, string cidr = null)
        {
            if (cidr == null)
            {
                var net = ip.Split('/');
                if (net.Length == 2)
                {
                    ip = net[0];
                    cidr = net[1];
                }
                else
                {
                    cidr = "255";
                }
            }

            return new IPNetwork().InternalParse(ip, cidr);
        }

        private IPNetwork InternalParse(string ip, string cidr)
        {
            _ipBytes = IPAddress.Parse(ip).GetAddressBytes();

            CalculateNetmask(cidr);
            CalculateNetworkAddress();
            CalculateBroadcastAddress();
            CalculateFirstUsableAddress();
            CalculateLastUsableAddress();

            return this;
        }

        private void CalculateLastUsableAddress()
        {
            _lastBytes = _broadcastBytes;
            _lastBytes[3]--;
        }

        private void CalculateFirstUsableAddress()
        {
            _firstBytes = _networkBytes;
            _firstBytes[3]++;
        }

        private void CalculateBroadcastAddress()
        {
            _broadcastBytes = new byte[_ipBytes.Length];

            for (int i = 0; i < _broadcastBytes.Length; i++)
            {
                _broadcastBytes[i] = (byte)(_ipBytes[i] | (_netmaskBytes[i] ^ 255));
            }
        }

        private void CalculateNetworkAddress()
        {
            _networkBytes = new byte[_ipBytes.Length];

            for (int i = 0; i < _networkBytes.Length; i++)
            {
                _networkBytes[i] = (byte)(_ipBytes[i] & (_netmaskBytes[i]));
            }
        }

        private void CalculateNetmask(string cidr)
        {
            uint mask = ~(0xFFFFFFFF >> int.Parse(cidr));
            byte[] bytes = BitConverter.GetBytes(mask);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes, 0, bytes.Length);
            }

            _netmaskBytes = bytes;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("IP:" + IP);
            builder.AppendLine("Netmask:" + Netmask);
            builder.AppendLine("NetworkAddress:" + NetworkAddress);
            builder.AppendLine("FirstUsable:" + FirstUsable);
            builder.AppendLine("LastUsable:" + LastUsable);
            builder.AppendLine("Broadcast:" + Broadcast);

            return builder.ToString();
        }
    }
}
