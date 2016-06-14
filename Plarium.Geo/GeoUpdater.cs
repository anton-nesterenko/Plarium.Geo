namespace Plarium.Geo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using Helpers;

    public class GeoUpdater
    {
        private const string TEMP_FILE = "temp.zip";
        private const string TEMP_FILE_BLOCKSV4 = "GeoLite2-City-Blocks-IPv4.csv";
        private const string TEMP_FILE_BLOCKSV6 = "GeoLite2-City-Blocks-IPv6.csv";
        private const string TEMP_FILE_CITY = "GeoLite2-City-Locations-en.csv";

        public bool Update(string file, string url)
        {
            //if (!File.Exists(TEMP_FILE_BLOCKS))
            //{
                WebHelper.Download(url, TEMP_FILE);

                // unpack
                using (var zip = ZipFile.OpenRead(TEMP_FILE))
                {
                    foreach (var entry in zip.Entries)
                    {
                        if (entry.Name.Equals(TEMP_FILE_BLOCKSV4) || entry.Name.Equals(TEMP_FILE_CITY))
                        {
                            entry.ExtractToFile(entry.Name, true);
                        }
                    }
                }

                File.Delete(TEMP_FILE);
            //}

            // parse db data
            uint ip;
            var blocks = new Dictionary<uint, string>(3000000);
            using (var reader = File.OpenText(TEMP_FILE_BLOCKSV4))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var values = line.Split(',');
                        ip = IPAddressTools.FromSubnetMask(values[0]);
                        if (!blocks.ContainsKey(ip))
                        {
                            var id = string.IsNullOrEmpty(values[1]) ? values[2] : values[1];
                            if (!string.IsNullOrEmpty(id))
                            {
                                blocks.Add(ip, id);
                            }
                        }
                    }
                }

                reader.Close();
            };

            var cities = new Dictionary<string, byte>(3000000);
            using (var reader = File.OpenText(TEMP_FILE_CITY))
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

            var filename = file + "_";

            var items = new List<uint>(blocks.Count);
            foreach (var block in blocks)
            {
                items.Add(block.Key);
            }
            items.Sort();

            var buffer = new byte[5];
            const int IPsize = sizeof(uint);
            using (var b = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                foreach (var item in items)
                {
                    Buffer.BlockCopy(BitConverter.GetBytes(item), 0, buffer, 0, IPsize);
                    buffer[IPsize] = cities[blocks[item]];
                    //buffer[IPsize + 1] = cities[blocks[item]];
                    b.Write(buffer);
                }
                b.Close();
            }
            if (File.Exists(file))
            {
                File.Delete(file);
            }
            File.Move(filename, file);
            File.Delete(TEMP_FILE_BLOCKSV4);
            File.Delete(TEMP_FILE_CITY);

            return true;
        }
    }
}
