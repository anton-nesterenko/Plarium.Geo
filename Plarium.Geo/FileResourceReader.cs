using System.IO;

namespace Plarium.Geo
{
    public class FileResourceReader : IGeoResourceReader
    {
        private string _filePath;

        public void SetFile(string filePath)
        {
            _filePath = filePath;
        }

        public Stream GetStream()
        {
            var path = _filePath ?? GeoUpdater.DbFile;
            return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }
}