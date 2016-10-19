namespace Plarium.Geo.Services
{
    public class GeoServiceBuilder
    {
        private IGeoResourceReader _reader;

        public T RegisterResource<T>() where T : IGeoResourceReader, new()
        {
            var instance = new T();
            _reader = instance;
            return instance;
        }

        public IGeoResourceReader GetReader()
        {
            if (_reader == null)
            {
                RegisterResource<FileResourceReader>();
            }

            return _reader;
        }
    }
}