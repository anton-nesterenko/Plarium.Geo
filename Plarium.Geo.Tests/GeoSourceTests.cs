using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plarium.Geo.Embedded;
using Plarium.Geo.Helpers;

namespace Plarium.Geo.Tests
{
    [TestClass]
    public class GeoSourceTests
    {
        IGeoSource source;

        [TestInitialize]
        public void Init()
        {
            IGeoResourceReader resourceReader;

            GeoUpdater.Update();

            Stopwatch stopwatch = Stopwatch.StartNew();
            var m1 = GC.GetTotalMemory(false);
            {
                //resourceReader = new FileResourceReader();
                resourceReader = new EmbeddedResourceReader();
                source = new MemoryGeoSource(resourceReader);
            }
            var m2 = GC.GetTotalMemory(false);
            stopwatch.Stop();
            
            Console.WriteLine("Loading time FileResourceReader: " + stopwatch.ElapsedMilliseconds + " ms");
            Console.WriteLine("Loading memory FileResourceReader: " + (m2 - m1) / 1024 / 1024 + " Mb");
        }

        [TestMethod]
        public void PerformanceTest()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            var tests = new[]
                            {
                                "63.135.255.174", "177.89.5.100", "63.135.255.216", "86.24.151.35", "64.237.226.193", "193.239.72.246"
                            };

            var results = new List<string>(tests.Length);

            stopwatch.Start();

            var m3 = GC.GetTotalMemory(false);
            {
                for (int i = 0; i < tests.Length; i++)
                {
                    results.Add(source.GetCountry(IPAddress.Parse(tests[i])));
                }
            }
            var m4 = GC.GetTotalMemory(false);

            stopwatch.Stop();

            Console.WriteLine("Process time: " + stopwatch.ElapsedMilliseconds + " ms");
            Console.WriteLine("Process memory: " + (m4 - m3) / 1024 + " kb");
            Console.WriteLine("Results:");

            for (int i = 0; i < tests.Length; i++)
            {
                Console.WriteLine("{0}:{1}:{2}",
                    tests[i],
                    CountryHelper.Default.GetCountryName(results[i]),
                    TimezoneHelper.Default.GetUtcOffsetByCountry(results[i])
                    );
            }
        }

        [TestMethod]
        public void BenchmarkTest()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            var count = 10000000;
            var ips = new List<IPAddress>(count);
            var rand = new Random();

            for (var i = 0; i < count; i++)
            {
                var ip = new IPAddress(rand.Next(int.MaxValue));
                ips.Add(ip);
            }

            stopwatch.Start();
            var m3 = GC.GetTotalMemory(true);
            {
                for (int i = 0; i < count; i++)
                {
                    source.GetCountry(ips[i]);
                }
            }
            var m4 = GC.GetTotalMemory(true);
            stopwatch.Stop();

            Console.WriteLine("Total Process time: " + stopwatch.Elapsed.TotalSeconds.ToString("N") + " seconds");
            Console.WriteLine("Total Processed IPs: " + count.ToString("N"));
            Console.WriteLine("Avg. Requests per second: " + Math.Round(count / stopwatch.Elapsed.TotalSeconds).ToString("N"));
            Console.WriteLine("Process memory: " + (m4 - m3) / 1024 + " kb");
        }
    }
}
