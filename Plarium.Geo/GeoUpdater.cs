using Plarium.Geo.UpdateModule;

namespace Plarium.Geo
{
    public class GeoUpdater
    {
        public enum UpdateMode
        {
            Lite,
            Full
        }

        public static string DbFile = "geoip.bin";

        public static void Update(UpdateMode mode = UpdateMode.Lite, bool force = false)
        {
            BaseUpdateModule module;
            module = new LiteUpdateModule();
            /*if (mode == UpdateMode.Lite)
            {
                module = new LiteUpdateModule();
            }
            else
            {
                module = new FullUpdateModule();
            }*/

            module.Update(DbFile, force);
        }
    }
}
