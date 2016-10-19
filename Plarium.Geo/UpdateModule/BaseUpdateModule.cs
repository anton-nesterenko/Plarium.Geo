using System.IO;
using System.IO.Compression;
using Plarium.Geo.Helpers;

namespace Plarium.Geo.UpdateModule
{
    internal abstract class BaseUpdateModule
    {
        private const string TEMP_FILE = "temp.zip";
        private const string VERSION_FILE = "version.txt";

        private bool forceUpdate = false;
        protected string dbFileName;

        public void Update(string dbFile, bool force)
        {
            forceUpdate = force;
            dbFileName = dbFile;

            Execute();
        }

        protected abstract void Execute();

        protected bool HasUpdates(string url, string tempFileBlocksv4, string tempFileBlocksv6, string tempFileLocations)
        {
            if (!RequiresUpdates(url))
            {
                return false;
            }

            WebHelper.Download(url, TEMP_FILE);

            // unpack
            using (var zip = ZipFile.OpenRead(TEMP_FILE))
            {
                foreach (var entry in zip.Entries)
                {
                    if (entry.Name.Equals(tempFileBlocksv4) || entry.Name.Equals(tempFileBlocksv6) || entry.Name.Equals(tempFileLocations))
                    {
                        entry.ExtractToFile(entry.Name, true);
                    }
                }
            }

            File.Delete(TEMP_FILE);

            return true;
        }

        private bool RequiresUpdates(string url)
        {
            if (forceUpdate)
            {
                SetVersion(0);
            }

            var date = WebHelper.GetModificationDate(url);
            var modified = long.Parse(date);
            if (modified <= GetVersion()) return false;
            SetVersion(modified);
            return true;
        }

        private long GetVersion()
        {
            long version = 0;
            if (File.Exists(VERSION_FILE))
            {
                version = long.Parse(File.ReadAllText(VERSION_FILE));
            }
            else
            {
                SetVersion(version);
            }

            return version;
        }

        private void SetVersion(long data)
        {
            File.WriteAllText(VERSION_FILE, data.ToString());
        }
    }
}