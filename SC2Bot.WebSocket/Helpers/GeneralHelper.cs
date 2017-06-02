using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SC2Bot.WebSocket.Helpers
{
    public static class GeneralHelper
    {
        static Random rnd = new Random();

        public static int GetRandom(int max = -1)
        {
            if (max == -1) return rnd.Next();

            return rnd.Next(max);
        }

        public static int GetRandom(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public async static Task<string> DownloadImage(string urlPath)
        {
            var url = new Uri(urlPath);
            var local = Path.Combine(Path.GetTempPath(),Path.GetFileName(url.LocalPath));
            WebClient client = new WebClient();
            await client.DownloadFileTaskAsync(new Uri(urlPath), local.ToString());
            client.Dispose();
            return local.ToString();
        }

        public static void RemoveImageTemp(string urlPath)
        {
            File.Delete(urlPath);
        }
    }
}
