using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plarium.Geo.Console
{
    using System.Net;
    using Plarium.Geo.Helpers;
    using Plarium.Geo.Services;
    using Console = System.Console;

    class Program
    {
        static void Main(string[] args)
        {
            //GeoUpdater.DefaultUpdate();
            //Console.WriteLine("done");
            //Console.ReadLine();
            
            var ipAddress = "193.239.72.246";
            var client = new GeoService();
            //Console.WriteLine(client.ResolveCountry(ipAddress));
            //Console.WriteLine(client.ResolveDialCodeByIP(ipAddress));
            //Console.WriteLine(client.ResolveTimezone(ipAddress));

            ipAddress = IPAddress.Parse(ipAddress).MapToIPv6().ToString();
            //ipAddress = "::ffff:c1ef:48f6";
            //ipAddress = "2002:ad2d:7632::ad2d:7632";
            //ipAddress = "0::24.24.24.24";
            Console.WriteLine(client.ResolveCountryName(client.ResolveCountry(ipAddress)));
            Console.WriteLine(client.ResolveDialCodeByIP(ipAddress));
            Console.WriteLine(client.ResolveTimezone(ipAddress));

            Console.ReadLine();
        }
    }
}
