using System.IO;
using Plarium.Geo.Embedded;

namespace Plarium.Geo.Console
{
    using Services;

    class Program
    {
        static void Main(string[] args)
        {

            ExampleUpdate();
            ChecklistV4Test();
            ChecklistV6Test();
            //ExampleServiceUsage();

            //ExampleServiceEmbeddedUsage();
            System.Console.ReadKey();
        }

        static void ExampleUpdate()
        {
            GeoUpdater.Update(mode: GeoUpdater.UpdateMode.Lite, force: true);
        }

        static void ExampleServiceUsage()
        {
            var builder = new GeoServiceBuilder();
            builder.RegisterResource<FileResourceReader>();
            var service = new GeoService(builder);

            //var ipAddress = "2607:f0d0:1002:51::4";
            var ipAddress = "2a02:120b:2c20:9910:8:2e54:ca19:cbea";

            var code = service.ResolveCountry(ipAddress);
            System.Console.WriteLine(code);
            System.Console.WriteLine(service.ResolveCountryName(code));
            //System.Console.WriteLine(service.ResolveDialCodeByIP(ipAddress));
            //System.Console.WriteLine(service.ResolveTimezone(ipAddress));
        }

        static void ChecklistV6Test()
        {
            var builder = new GeoServiceBuilder();
            builder.RegisterResource<FileResourceReader>();
            var service = new GeoService(builder);

            var lines = File.ReadAllLines("geo-checklist.csv");
            foreach (var line in lines)
            {
                var vars = line.Split(',');
                var ip = vars[0];
                var wrongCode = vars[1];
                var rightCode = vars[2];

                var code = service.ResolveCountry(ip);

                if (code.Equals(rightCode))
                {
                    System.Console.WriteLine("OK6. "+ code);
                }
                else
                {
                    System.Console.WriteLine("FAIL6. " + code + "!="+ rightCode  +" for "+ ip);
                }
            }
        }

        static void ChecklistV4Test()
        {
            var builder = new GeoServiceBuilder();
            builder.RegisterResource<FileResourceReader>();
            var service = new GeoService(builder);

            var lines = File.ReadAllLines("geo-checklistv4.csv");
            foreach (var line in lines)
            {
                var vars = line.Split(',');
                var ip = vars[0];
                var rightCode = vars[1];

                var code = service.ResolveCountry(ip);

                if (code.Equals(rightCode))
                {
                    System.Console.WriteLine("OK4. "+ code);
                }
                else
                {
                    System.Console.WriteLine("FAIL4. " + code + "!=" + rightCode + " for " + ip);
                }
            }
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
