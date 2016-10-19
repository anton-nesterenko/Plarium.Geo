using Plarium.Geo.Embedded;

namespace Plarium.Geo.Console
{
    using Services;

    class Program
    {
        static void Main(string[] args)
        {

            ExampleUpdate();

            ExampleServiceUsage();

            ExampleServiceEmbeddedUsage();
        }

        static void ExampleUpdate()
        {
            GeoUpdater.Update();
        }

        static void ExampleServiceUsage()
        {
            var builder = new GeoServiceBuilder();
            builder.RegisterResource<FileResourceReader>();
            var service = new GeoService(builder);

            var ipAddress = "2607:f0d0:1002:51::4";

            service.ResolveCountry(ipAddress);
            service.ResolveDialCodeByIP(ipAddress);
            service.ResolveTimezone(ipAddress);
        }

        static void ExampleServiceEmbeddedUsage()
        {
            var builder = new GeoServiceBuilder();
            builder.RegisterResource<EmbeddedResourceReader>();
            var service = new GeoService(builder);

            var ipAddress = "2607:f0d0:1002:51::4";

            service.ResolveCountry(ipAddress);
            service.ResolveDialCodeByIP(ipAddress);
            service.ResolveTimezone(ipAddress);
        }
    }
}
