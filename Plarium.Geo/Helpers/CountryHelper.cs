namespace Plarium.Geo.Helpers
{
    using Services;

    public class CountryHelper
    {
        public const string UnknownCountryCode = "--";
        public const string UnknownDialCode = "0";

        private static ICountryService _default;

        private CountryHelper() { }

        public static ICountryService Default
        {
            get
            {
                return _default ?? (_default = new CountryService());
            }
        }
    }
}
