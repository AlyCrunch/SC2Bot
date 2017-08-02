using Discord;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

        public static async Task<string> Roles(IGuildUser u, List<IRole> roles, List<string> races)
        {
            var uRoles = roles.Where(r => u.RoleIds.Any(ru => ru == r.Id));

            List<IRole> newRoles = new List<IRole>();            
            string strRtn = string.Empty;

            foreach (string race in races)
            {
                switch (race.ToLower())
                {
                    case "zerg": newRoles.Add(roles.Find(r => r.Name.ToLower() == "zerg")); strRtn += Properties.Resources.ZergQuote; break;
                    case "terran": newRoles.Add(roles.Find(r => r.Name.ToLower() == "terran")); strRtn += Properties.Resources.TerranQuote; break;
                    case "protoss": newRoles.Add(roles.Find(r => r.Name.ToLower() == "protoss")); strRtn += Properties.Resources.ProtossQuote; break;
                    case "random": newRoles.Add(roles.Find(r => r.Name.ToLower() == "random")); strRtn += Properties.Resources.RandomQuote; break;
                }
                strRtn += "\n";
            }

            if (newRoles != null)
            {
                await u.RemoveRolesAsync(uRoles);
                await u.AddRolesAsync(newRoles);
                return strRtn;
            }
            else
            {
                return @"¯\_(ツ)_/¯";
            }
        }

        public static T First<T>(IEnumerable<T> items)
        {
            using (IEnumerator<T> iter = items.GetEnumerator())
            {
                iter.MoveNext();
                return iter.Current;
            }
        }
    }
}
