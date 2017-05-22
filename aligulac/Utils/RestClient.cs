using AligulacSC2.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RestConnect
{
    static public class RestClient
    {
        static HttpClient client = new HttpClient();
        static string endPoint = "http://aligulac.com/";
        static string playerRoot = "api/v1/player/{0}/";
        static string playersRoot = "api/v1/player/";
        static string searchRoot = "search/json/?q={0}";
        static string predictMatchRoot = "api/v1/predictmatch/{0},{1}/";
        static string teamRoot = "api/v1/team/{0}/";
        static string periodRoot = "api/v1/period/";

        static RestClient()
        {
            client.BaseAddress = new Uri(endPoint);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        static public async Task<SearchResult> Search(string search)
        {
            SearchResult searchObj = null;
            try
            {

                var response = await client
                    .GetAsync(endPoint + string.Format(searchRoot, search));
                if (response.IsSuccessStatusCode)
                {
                    searchObj = await response.Content.ReadAsAsync<SearchResult>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return searchObj;
        }

        static public async Task<Player> GetPlayerByID(string id, string key)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("apikey", key);
            string parameters = BuildParameters(dic);

            Player playerObj = null;

            try
            {
                HttpResponseMessage response = await client.GetAsync(endPoint + string.Format(playerRoot, id) + parameters);
                if (response.IsSuccessStatusCode)
                {
                    playerObj = await response.Content.ReadAsAsync<Player>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return playerObj;
        }

        static public async Task<GenericResult<Player>> GetTopPlayers(string limit, string key)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("current_rating__isnull", "false");
            dic.Add("current_rating__decay__lt", "4");
            dic.Add("order_by", "-current_rating__rating");
            dic.Add("limit", limit);
            dic.Add("apikey", key);
            string parameters = BuildParameters(dic);

            GenericResult<Player> playersObj = null;

            try
            {
                HttpResponseMessage response = await client.GetAsync(endPoint + playersRoot + parameters);
                if (response.IsSuccessStatusCode)
                {
                    playersObj = await response.Content.ReadAsAsync<GenericResult<Player>>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return playersObj;
        }

        static public async Task<Team> GetTeamByID(string id, string key)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("apikey", key);
            string parameters = BuildParameters(dic);

            Team teamObj = null;

            try
            {
                HttpResponseMessage response = await client.GetAsync(endPoint + string.Format(teamRoot, id) + parameters);
                if (response.IsSuccessStatusCode)
                {
                    teamObj = await response.Content.ReadAsAsync<Team>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return teamObj;
        }

        static public async Task<Prediction> GetPrediction(string idPlayerA, string idPlayerB, string BO, string key)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("apikey", key);
            dic.Add("bo", BO);
            string parameters = BuildParameters(dic);

            Prediction predObj = null;

            try
            {
                HttpResponseMessage response = await client.GetAsync(endPoint + string.Format(predictMatchRoot, idPlayerA, idPlayerB) + parameters);
                if (response.IsSuccessStatusCode)
                {
                    predObj = await response.Content.ReadAsAsync<Prediction>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            predObj.BO = int.Parse(BO);
            return predObj;
        }

        static public async Task<GenericResult<Period>> GetPeriods(string key, int limit = 0)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("apikey", key);
            dic.Add("order_by", "-end");
            dic.Add("limit", limit.ToString());
            string parameters = BuildParameters(dic);

            GenericResult<Period> periodObj = null;

            try
            {
                HttpResponseMessage response = await client.GetAsync(endPoint + periodRoot + parameters);
                if (response.IsSuccessStatusCode)
                {
                    periodObj = await response.Content.ReadAsAsync<GenericResult<Period>>();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return periodObj;
        }

        static private string BuildParameters(Dictionary<string, string> parameters)
        {
            return "?" + string.Join("&", parameters.Select(x => x.Key + "=" + x.Value).ToArray());
        }
    }
}