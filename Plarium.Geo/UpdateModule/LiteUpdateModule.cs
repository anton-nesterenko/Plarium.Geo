using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using Plarium.Geo.Helpers;

namespace Plarium.Geo.UpdateModule
{
    internal class LiteUpdateModule : BaseUpdateModule
    {
        private const string TEMP_FILE_BLOCKSV4 = "GeoLite2-Country-Blocks-IPv4.csv";
        private const string TEMP_FILE_BLOCKSV6 = "GeoLite2-Country-Blocks-IPv6.csv";
        private const string TEMP_FILE_LOCATIONS = "GeoLite2-Country-Locations-en.csv";
        private const string URL = "http://geolite.maxmind.com/download/geoip/database/GeoLite2-Country-CSV.zip";

        protected override void Execute()
        {
            if (!HasUpdates(URL, TEMP_FILE_BLOCKSV4, TEMP_FILE_BLOCKSV6, TEMP_FILE_LOCATIONS))
            {
                return;
            }

            UpdateIPv6();

            var countries = new Dictionary<string, byte>(256);
            using (var reader = File.OpenText(TEMP_FILE_LOCATIONS))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var values = line.Split(',');
                        if (!countries.ContainsKey(values[0]))
                        {
                            countries.Add(values[0], CountryHelper.Default.CountryToByte(values[4]));
                        }
                    }
                }

                reader.Close();
            }

            var blocksIPv4 = new Dictionary<uint, string>(250000);
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
                        ip = (uint) IPAddressTools.ToUInt64(net.FirstUsable.GetAddressBytes());
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
            }

            var blocksIPv6 = new Dictionary<ulong, byte>(40000);
            using (var reader = File.OpenText(TEMP_FILE_BLOCKSV6))
            {
                ulong ip;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var values = line.Replace("\"", "").Split(',');

                        var addressBytes = IPAddress.Parse(values[0]).GetAddressBytes().Take(8).Reverse().ToArray();

                        ip = BitConverter.ToUInt64(addressBytes, 0);

                        if (!blocksIPv6.ContainsKey(ip))
                        {
                            var id = values[4].Trim();
                            if (!string.IsNullOrEmpty(id))
                            {
                                blocksIPv6.Add(ip, CountryHelper.Default.CountryToByte(id));
                            }
                        }
                    }
                }

                reader.Close();
            }


            //clear dublicates
            byte sourcePrev = byte.MinValue;
            foreach (var source in blocksIPv6.OrderBy(x => x.Key))
            {
                if (source.Value != sourcePrev)
                {
                    sourcePrev = source.Value;
                }
                else
                {
                    blocksIPv6.Remove(source.Key);
                }
            }

            var filenameIPv4 = "_" + dbFileName;

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
                b.Write(new[] {byte.MaxValue});
                var bufferHeader = new byte[4];
                var ipv6Address = itemsIPv4.Count*bufferIPv4.Length + bufferHeader.Length + 1;
                Buffer.BlockCopy(BitConverter.GetBytes(ipv6Address), 0, bufferHeader, 0, bufferHeader.Length);
                b.Write(bufferHeader);

                //write ipv4
                foreach (var item in itemsIPv4)
                {
                    Buffer.BlockCopy(BitConverter.GetBytes(item), 0, bufferIPv4, 0, IPv4size);
                    bufferIPv4[IPv4size] = countries[blocksIPv4[item]];
                    b.Write(bufferIPv4);
                }

                //write ipv6
                foreach (var item in itemsIPv6)
                {
                    Buffer.BlockCopy(BitConverter.GetBytes(item), 0, bufferIPv6, 0, IPv6size);
                    bufferIPv6[IPv6size] = blocksIPv6[item];
                    b.Write(bufferIPv6);
                }

                b.Flush();
                b.Close();
            }


            if (File.Exists(dbFileName))
            {
                File.Delete(dbFileName);
            }

            File.Move(filenameIPv4, dbFileName);

            File.Delete(TEMP_FILE_BLOCKSV4);
            File.Delete(TEMP_FILE_BLOCKSV6);
            File.Delete(TEMP_FILE_LOCATIONS);
        }

        private void UpdateIPv6()
        {
            var url = "http://geolite.maxmind.com/download/geoip/database/GeoIPv6.csv.gz";
            var tempFile = "temp.gz";

            WebHelper.Download(url, tempFile);

            using (FileStream fInStream = new FileStream(tempFile, FileMode.Open, FileAccess.Read))
            {
                using (GZipStream zipStream = new GZipStream(fInStream, CompressionMode.Decompress))
                {
                    using (
                        FileStream fOutStream = new FileStream(TEMP_FILE_BLOCKSV6, FileMode.OpenOrCreate,
                            FileAccess.Write))
                    {
                        byte[] tempBytes = new byte[4096];
                        int i;
                        while ((i = zipStream.Read(tempBytes, 0, tempBytes.Length)) != 0)
                        {
                            fOutStream.Write(tempBytes, 0, i);
                        }
                    }
                }
            }

            File.Delete(tempFile);
        }
    }
}