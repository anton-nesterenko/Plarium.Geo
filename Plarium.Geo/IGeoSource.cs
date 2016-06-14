namespace Plarium.Geo
{
    using System.Net;

    public interface IGeoSource
    {
        string GetCountry(IPAddress address);
        
        string GetTimezone(IPAddress address);
        
    }
}
