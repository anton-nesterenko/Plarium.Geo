namespace Plarium.Geo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    using Helpers;

    public class MemoryGeoSource : IGeoSource
    {
        private readonly SortedList<uint, byte> _listIPv4;
        private readonly SortedList<ulong, byte> _listIPv6;

        public MemoryGeoSource(string dbPath)
        {
            _listIPv4 = new SortedList<uint, byte>(3000000);
            _listIPv6 = new SortedList<ulong, byte>(300);
            LoadToMemory(dbPath);
        }

        public string GetCountry(IPAddress address)
        {
            var countryByte = address.IsIPv4() ? FindIPv4(address.GetIPv4()) : FindIPv6(address.GetIPv6());

            return CountryHelper.Default.GetCountryCode(countryByte);
        }
        
        public string GetTimezone(IPAddress address)
        {
            var countryByte = address.IsIPv4() ? FindIPv4(address.GetIPv4()) : FindIPv6(address.GetIPv6());

            var country = CountryHelper.Default.GetCountryCode(countryByte);
            return TimezoneHelper.Default.GetUtcOffsetByCountry(country);
        }

        private void LoadToMemory(string dbPath)
        {
            var ipBytes = new byte[4];
            using(var stream = File.Open(dbPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new BinaryReader(stream))
            {
                int pos = 0;
                int length = (int)reader.BaseStream.Length;

                while (pos < length)
                {
                    ipBytes = reader.ReadBytes(ipBytes.Length);

                    _listIPv4.Add(BitConverter.ToUInt32(ipBytes, 0), reader.ReadByte());

                    pos += ipBytes.Length + 1;
                }

                stream.Close();
                reader.Close();
            }
        }

        private byte FindIPv6(ulong value)
        {
            //TODO: non implemented
            return byte.MinValue;
        }

        private byte FindIPv4(uint value)
        {
            int lower = 0;
            int length = _listIPv4.Count;
            int upper = length - 1;
            int index = (lower + upper) / 2;

            while (lower <= upper)
            {
                var result = value.CompareTo(_listIPv4.Keys[index]);
                if (result == 0)
                {
                    return _listIPv4.Values[index];
                }

                if (result < 0)
                {
                    upper = index - 1;
                }
                else
                {
                    lower = index + 1;
                }
                index = (lower + upper) / 2;
            }

            if (index >= length - 1)
            {
                return value > _listIPv4.Keys[length - 1] ? _listIPv4.Values[0] : _listIPv4.Values[length - 1];
            }

            if (index <= 0)
            {
                return _listIPv4.Values[0];
            }

            /*if (_list.Keys[index + 1] - value < value - _list.Keys[index])
            {
                index++;
            }*/

            return _listIPv4.Values[index];
        }

        
    }
}
