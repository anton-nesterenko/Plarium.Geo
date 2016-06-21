namespace Plarium.Geo.Helpers
{
    using Services;

    public static class CountryHelper
    {
        public const string UnknownCountryCode = "--";
        public const string UnknownDialCode = "0";

        private static ICountryService _default;

        public static ICountryService Default
        {
            get
            {
                return _default ?? (_default = new CountryService());
            }
        }
    }
}
