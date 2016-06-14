namespace Plarium.Geo
{
    public interface IGeoClient
    {
        string ResolveCountry(string ip);
        string ResolveTimezone(string ip, string format);
        string ResolveCountryName(string countryCode);
        string ResolveDialCodeByIP(string ip);
    }
}
