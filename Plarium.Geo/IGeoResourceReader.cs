using System.IO;

namespace Plarium.Geo
{
    public interface IGeoResourceReader
    {
        Stream GetStream();
    }
}