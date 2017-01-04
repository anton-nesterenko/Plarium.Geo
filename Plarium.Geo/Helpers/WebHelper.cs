namespace Plarium.Geo.Helpers
{
    using System.Net;

    internal class WebHelper
    {
        public static string GetModificationDate(string url, string dateFormat = "yyyyMMddHHmmss")
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return response.LastModified.ToString(dateFormat);
                }
            }

            return null;
        }

        public static void Download(string url, string path)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(url, path);
            }
        }
    }
}
