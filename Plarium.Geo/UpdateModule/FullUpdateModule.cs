using System;
using System.Collections.Generic;
using System.IO;
using Plarium.Geo.Helpers;

namespace Plarium.Geo.UpdateModule
{
    internal class FullUpdateModule : BaseUpdateModule
    {
        private const string TEMP_FILE_BLOCKSV4 = "GeoLite2-Country-Blocks-IPv4.csv";
        private const string TEMP_FILE_BLOCKSV6 = "GeoLite2-Country-Blocks-IPv6.csv";
        private const string TEMP_FILE_LOCATIONS = "GeoLite2-Country-Locations-en.csv";
        private const string URL = "http://geolite.maxmind.com/download/geoip/database/GeoLite2-City-CSV.zip";

        protected override void Execute()
        {
        
            var cities = new Dictionary<string, byte>(100000);
            using (var reader = File.OpenText(TEMP_FILE_LOCATIONS))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var values = line.Split(',');
                        if (!cities.ContainsKey(values[0]))
                        {
                            //var zone = TimeZoneHelper.Default.GetUtcOffsetAnyway(values[4], values[12]);
                            //cities.Add(values[0], values[4] + "," + zone);
                            cities.Add(values[0], CountryHelper.Default.CountryToByte(values[4]));
                        }
                    }
                }

                reader.Close();
            };

            var blocksIPv4 = new Dictionary<uint, string>(4000000);
            using (var reader = File.OpenText(TEMP_FILE_BLOCKSV4))
            {
                uint ip;
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var values = line.Split(',');
                        var net = IPAddressTools.FromSubnetMask(values[0]);
                        ip = (uint)IPAddressTools.ToUInt64(net.FirstUsable.GetAddressBytes());
                        if (!blocksIPv4.ContainsKey(ip))
                        {
                            var id = string.IsNullOrEmpty(values[1]) ? values[2] : values[1];
                            if (!string.IsNullOrEmpty(id))
                            {
                                blocksIPv4.Add(ip, id);
                            }
                        }
                    }
                }

                reader.Close();
            };

            //var list = new List<string>(1500000);
            var blocksIPv6 = new Dictionary<ulong, string>(1500000);
            using (var reader = File.OpenText(TEMP_FILE_BLOCKSV6))
            {
                ulong ip;
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var values = line.Split(',');
                        var net = IPAddressTools.FromSubnetMask(values[0]);
                        ip = IPAddressTools.ToUInt64(net.FirstUsable.GetAddressBytes());
                        if (!blocksIPv6.ContainsKey(ip))
                        {
                            var id = string.IsNullOrEmpty(values[1]) ? values[2] : values[1];
                            if (!string.IsNullOrEmpty(id))
                            {
                                blocksIPv6.Add(ip, id);
                                /*
                                var iplast = IPAddressTools.ToUInt64(net.LastUsable.GetAddressBytes());
                                var loc = CountryHelper.Default.GetCountryCode(cities[id]);
                                var usable = iplast - ip;
                                list.Add($"{ip},{iplast},{usable},{loc}");*/
                            }
                        }
                    }
                }

                reader.Close();
            };

            //File.WriteAllLines("_ipv6debug.bin", list);
            //list.Clear();

            var filenameIPv4 = dbFileName + "_";

            var itemsIPv4 = new List<uint>(blocksIPv4.Count);
            foreach (var block in blocksIPv4)
            {
                itemsIPv4.Add(block.Key);
            }
            itemsIPv4.Sort();

            var itemsIPv6 = new List<ulong>(blocksIPv6.Count);
            foreach (var block in blocksIPv6)
            {
                itemsIPv6.Add(block.Key);
            }
            itemsIPv6.Sort();

            var bufferIPv6 = new byte[9];
            const int IPv6size = sizeof(ulong);

            var bufferIPv4 = new byte[5];
            const int IPv4size = sizeof(uint);

            using (var b = new BinaryWriter(File.Open(filenameIPv4, FileMode.Create, FileAccess.Write, FileShare.None)))
            {
                //write header with IPv6 block address
                b.Write(new[] { byte.MaxValue });
                var bufferHeader = new byte[4];
                var ipv6Address = itemsIPv4.Count * bufferIPv4.Length + bufferHeader.Length + 1;
                Buffer.BlockCopy(BitConverter.GetBytes(ipv6Address), 0, bufferHeader, 0, bufferHeader.Length);
                b.Write(bufferHeader);

                //write ipv4
                foreach (var item in itemsIPv4)
                {
                    Buffer.BlockCopy(BitConverter.GetBytes(item), 0, bufferIPv4, 0, IPv4size);
                    bufferIPv4[IPv4size] = cities[blocksIPv4[item]];
                    //buffer[IPsize + 1] = cities[blocks[item]];
                    b.Write(bufferIPv4);
                }

                //write ipv6
                foreach (var item in itemsIPv6)
                {
                    Buffer.BlockCopy(BitConverter.GetBytes(item), 0, bufferIPv6, 0, IPv6size);
                    bufferIPv6[IPv6size] = cities[blocksIPv6[item]];
                    //buffer[IPsize + 1] = cities[blocks[item]];
                    b.Write(bufferIPv6);
                }

                b.Close();
            }

            if (File.Exists(dbFileName))
            {
                File.Delete(dbFileName);
            }

            File.Move(filenameIPv4, dbFileName);

            //File.Delete(TEMP_FILE_BLOCKSV4);
            //File.Delete(TEMP_FILE_BLOCKSV6);
            //File.Delete(TEMP_FILE_CITY);

            
        }
    }
}