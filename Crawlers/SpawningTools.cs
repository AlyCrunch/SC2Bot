using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using Crawlers.Objects.SpawningTools;

namespace Crawlers
{
    public class SpawningTools
    {
        static private string BaseURL = "http://lotv.spawningtool.com/build/";
        static private string Params = "?name={0}&contributor={1}&sort_by={2}&build_type={3}&patch={4}";

        public async Task GetListBO(string MatchUp = "", string build = "", string name = "", string sortby = "t", int patch = (int)Utils.SpawningTools.Patch.Last, string contributor = "")
        {
            await Request(BaseURL + MatchUp + string.Format(Params, HttpUtility.UrlEncode(name), contributor, sortby, build, patch.ToString()));
        }

        public static async Task Request(string URI)
        {
            var req = (HttpWebRequest)WebRequest.Create(URI);
            req.Headers.Add("X-Requested-With", "XMLHttpRequest");
            req.ContentType = "application/json";
            req.Accept = "application/json";
            req.Method = WebRequestMethods.Http.Get;
            var r = await req.GetResponseAsync().ConfigureAwait(false);

            var responseReader = new StreamReader(r.GetResponseStream());
            var responseData = await responseReader.ReadToEndAsync();

            var rt = JsonConvert.DeserializeObject<List<Result>>(responseData);
        }
    }
}
