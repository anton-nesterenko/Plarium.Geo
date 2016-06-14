namespace Plarium.Geo.Services
{
    public interface ICountryService
    {
        string GetCountryName(string code);

        byte CountryToByte(string code);

        string GetCountryName(byte code);

        string GetCountryCode(byte code);

        string GetDialCode(string code);
    }
}
