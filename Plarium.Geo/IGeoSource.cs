namespace Plarium.Geo
{
    public interface IGeoSource
    {
        string GetCountry(uint ip);

        string GetCountry(string ip);

        string GetTimezone(uint ip);

        string GetTimezone(string ip);
    }
}
