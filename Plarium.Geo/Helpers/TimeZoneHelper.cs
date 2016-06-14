namespace Plarium.Geo.Helpers
{
    using Services;

    public class TimezoneHelper
    {
        private static ITimezoneService _default;
        
        private TimezoneHelper() { }

        public static ITimezoneService Default
        {
            get
            {
                return _default ?? (_default = new TimezoneService());
            }
        }
    }
}
