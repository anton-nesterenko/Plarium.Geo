using System.IO;
using Plarium.Geo.Embedded.Properties;

namespace Plarium.Geo.Embedded
{
    public class EmbeddedResourceReader : IGeoResourceReader
    {
        public Stream GetStream()
        {
            return new MemoryStream(Resources.geoip);
        }
    }
}
