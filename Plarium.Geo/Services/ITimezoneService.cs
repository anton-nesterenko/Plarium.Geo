namespace Plarium.Geo.Services
{
    public interface ITimezoneService
    {
        string GetUtcOffsetAnyway(string countryCode, string timezone);

        string GetUtcOffsetByCountry(string countryCode);

        string GetUtcOffsetByTimezone(string timezone);

        //byte TimezoneToByte(string code);
    }
}
