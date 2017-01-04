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

        public MemoryGeoSource(IGeoResourceReader reader)
        {
            _listIPv4 = new SortedList<uint, byte>(250000);
            _listIPv6 = new SortedList<ulong, byte>(21000);

            LoadToMemory(reader);
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

        private void LoadToMemory(IGeoResourceReader resourceReader)
        {
            var ipv4Bytes = new byte[4];
            var ipv6Bytes = new byte[8];
            using (var stream = resourceReader.GetStream())
            using (var reader = new BinaryReader(stream))
            {
                //check header for ipv6
                bool ipv6Exists = reader.ReadByte() == byte.MaxValue;
                int ipv6Address = 0;
                if (ipv6Exists)
                {
                    //read header
                    ipv6Address = reader.ReadInt32();
                }

                //read ipv4
                int pos = 0;
                int length = (int)reader.BaseStream.Length;
                int ipv4MaxLength = ipv6Exists ? ipv6Address : length;
                while (pos < ipv4MaxLength)
                {
                    ipv4Bytes = reader.ReadBytes(ipv4Bytes.Length);

                    _listIPv4.Add(BitConverter.ToUInt32(ipv4Bytes, 0), reader.ReadByte());

                    pos += ipv4Bytes.Length + 1;
                }

                //read ipv6
                if (ipv6Exists)
                {
                    pos = ipv4MaxLength;
                    reader.BaseStream.Position = ipv4MaxLength;
                    while (pos < length)
                    {
                        ipv6Bytes = reader.ReadBytes(ipv6Bytes.Length);

                        _listIPv6.Add(BitConverter.ToUInt64(ipv6Bytes, 0), reader.ReadByte());

                        pos += ipv6Bytes.Length + 1;
                    }
                }

                stream.Close();
                reader.Close();
            }
        }

        private byte FindIPv6(ulong value)
        {
            int lower = 0;
            int length = _listIPv6.Count;
            int upper = length - 1;
            int index = (lower + upper) / 2;

            while (lower <= upper)
            {
                var result = value.CompareTo(_listIPv6.Keys[index]);
                if (result == 0)
                {
                    return _listIPv6.Values[index];
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
                return value > _listIPv6.Keys[length - 1] ? _listIPv6.Values[0] : _listIPv6.Values[length - 1];
            }

            if (index <= 0)
            {
                return _listIPv6.Values[0];
            }

            return _listIPv6.Values[index];
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
