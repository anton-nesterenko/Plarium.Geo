namespace Plarium.Geo
{
    using System.Net;
    using Helpers;
    using Services;

    public class GeoService : IGeoService
    {
        private readonly IGeoSource _source;

        public GeoService(IGeoSource source)
        {
            _source = source;
        }

        public string ResolveCountry(string ip)
        {
            IPAddress address;
            if (IPAddressTools.TryParse(ip, out address))
            {
                return _source.GetCountry(address);
            }

            return CountryHelper.UnknownCountryCode;
        }

        public string ResolveTimezone(string ip, string format = "GMT{0}")
        {
            IPAddress address;
            if (!IPAddressTools.TryParse(ip, out address))
            {
                return string.Format(format, TimezoneService.DefaultOffset);
            }

            string utc;
            var timezone = _source.GetTimezone(address);
            
            if (string.IsNullOrEmpty(timezone))
            {
                var countryCode = _source.GetCountry(address);
                utc = TimezoneHelper.Default.GetUtcOffsetByCountry(countryCode);
            }
            else
            {
                utc = TimezoneHelper.Default.GetUtcOffsetByTimezone(timezone);
            }

            return string.Format(format, utc);
        }

        public string ResolveCountryName(string countryCode)
        {
            return CountryHelper.Default.GetCountryName(countryCode);
        }

        public string ResolveDialCodeByIP(string ip)
        {
            IPAddress address;
            if (IPAddressTools.TryParse(ip, out address))
            {
                var countryCode = _source.GetCountry(address);
                return CountryHelper.Default.GetDialCode(countryCode);
            }

            return CountryHelper.UnknownDialCode;
        }

    }
}
