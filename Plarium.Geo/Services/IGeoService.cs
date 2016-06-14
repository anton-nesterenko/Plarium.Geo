namespace Plarium.Geo.Services
{
    public interface IGeoService
    {
        string ResolveCountry(string ip);
        string ResolveTimezone(string ip, string format = "GMT{0}");
        string ResolveCountryName(string countryCode);
        string ResolveDialCodeByIP(string ip);
    }
}
