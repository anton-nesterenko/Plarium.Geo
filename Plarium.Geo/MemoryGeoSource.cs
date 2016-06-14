namespace Plarium.Geo
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Helpers;

    public class MemoryGeoSource : IGeoSource
    {
        private readonly string _dbPath;
        
        private readonly SortedList<uint, byte> _list;

        public MemoryGeoSource(string dbPath)
        {
            this._dbPath = dbPath;
            _list = new SortedList<uint, byte>(3000000);
            LoadToMemory();
        }

        public string GetCountry(uint ip)
        {
            var countryByte = FindNearest(ip);
            return CountryHelper.Default.GetCountryCode(countryByte);
        }

        public string GetCountry(string ip)
        {
            return GetCountry(IPAddressHelper.ToUInt32(ip));
        }

        public string GetTimezone(uint ip)
        {
            var countryByte = FindNearest(ip);
            var country = CountryHelper.Default.GetCountryCode(countryByte);
            return TimeZoneHelper.Default.GetUtcOffsetByCountry(country);
        }

        public string GetTimezone(string ip)
        {
            return GetTimezone(IPAddressHelper.ToUInt32(ip));
        }

        private void LoadToMemory()
        {
            var ipBytes = new byte[4];
            using(var stream = File.Open(_dbPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new BinaryReader(stream))
            {
                int pos = 0;
                int length = (int)reader.BaseStream.Length;

                while (pos < length)
                {
                    ipBytes = reader.ReadBytes(ipBytes.Length);

                    _list.Add(BitConverter.ToUInt32(ipBytes, 0), reader.ReadByte());

                    pos += ipBytes.Length + 1;
                }

                stream.Close();
                reader.Close();
            }
        }

        private byte FindNearest(uint value)
        {
            int lower = 0;
            int length = _list.Count;
            int upper = length - 1;
            int index = (lower + upper) / 2;
            int result;

            while (lower <= upper)
            {
                result = value.CompareTo(_list.Keys[index]);
                if (result == 0)
                {
                    return _list.Values[index];
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
                return value > _list.Keys[length - 1] ? _list.Values[0] : _list.Values[length - 1];
            }

            if (index <= 0)
            {
                return _list.Values[0];
            }

            /*if (_list.Keys[index + 1] - value < value - _list.Keys[index])
            {
                index++;
            }*/

            return _list.Values[index];
        }
    }
}
