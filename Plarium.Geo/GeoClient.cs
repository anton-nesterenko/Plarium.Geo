namespace Plarium.Geo
{
    using Helpers;

    public class GeoClient : IGeoClient
    {
        private readonly IGeoSource _source;

        public GeoClient(IGeoSource source)
        {
            _source = source;
        }

        public string ResolveCountry(string ip)
        {
            return _source.GetCountry(ip);
        }

        public string ResolveTimezone(string ip, string format = "GMT{0}:00")
        {
            var address = IPAddressHelper.ToUInt32(ip);
            var timezone = _source.GetTimezone(address);
            string utc;

            if (string.IsNullOrEmpty(timezone))
            {
                var countryCode = _source.GetCountry(address);
                utc = TimeZoneHelper.Default.GetUtcOffsetByCountry(countryCode);
            }
            else
            {
                utc = TimeZoneHelper.Default.GetUtcOffsetByTimezone(timezone);
            }

            return string.Format(format, utc);
        }

        public string ResolveCountryName(string countryCode)
        {
            return CountryHelper.Default.GetCountryName(countryCode);
        }

        public string ResolveDialCodeByIP(string ip)
        {
            var countryCode = _source.GetCountry(ip);
            return CountryHelper.Default.GetDialCode(countryCode);
        }

    }
}
