using Crawlers.Objects.Liquipedia;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Crawlers.Utils.Liquipedia
{
    public class Parser
    {
        public async Task<HtmlDocument> GetDocumentHTML(string URL)
        {
            var response = await new HttpClient().GetByteArrayAsync(URL);

            String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            HtmlDocument resultat = new HtmlDocument();
            resultat.LoadHtml(source);

            return resultat;
        }

        public List<Transfert> ParsingTransfers(HtmlDocument page)
        {
            var transferNode = page.DocumentNode.Descendants().First(x => IsTransfertTable(x))
                                                .Descendants().Where(x => IsTransfertLine(x));

            List<Transfert> listTransfert = new List<Transfert>();

            foreach (var child in transferNode)
            {
                var t = new Transfert();
                var pl = new List<Player>();
                var d = new DateTime();
                t.Players = new List<Player>();

                var dateNode = child.Descendants().First(x => IsDate(x));
                var playersNode = child.Descendants().First(x => IsPlayer(x));
                var teamsNode = child.Descendants().Where(x => IsTeam(x)).ToArray();

                var playerNodes = Split(playersNode.Descendants("a").ToList());

                foreach (var p in playerNodes)
                    t.Players.Add(GetPlayer(p));

                DateTime.TryParseExact(dateNode.InnerText, "yyyy-MM-dd",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out d);

                t.Date = d;

                t.OldTeam = GetTeam(teamsNode[0]);
                t.NewTeam = GetTeam(teamsNode[1]);

                listTransfert.Add(t);
            }

            List<List<HtmlNode>> Split<HtmlNode>(List<HtmlNode> plF)
            {
                return plF
                    .Select((x, i) => new { Index = i, Value = x })
                    .GroupBy(x => x.Index / 3)
                    .Select(x => x.Select(v => v.Value).ToList())
                    .ToList();
            }

            Team GetTeam(HtmlNode TeamNode)
            {
                if (TeamNode.Descendants().Any(x => x.Name == "img"))
                {
                    var name = TeamNode.Descendants("a").First().GetAttributeValue("title", string.Empty);
                    string urlT = TeamNode.Descendants("img").First().GetAttributeValue("src", string.Empty);
                    return new Team() { Name = name, ImageURL = urlT };
                }
                else
                {
                    return new Team() { Name = TeamNode.Descendants("i").First().InnerText, ImageURL = string.Empty };
                }
            }

            Player GetPlayer(List<HtmlNode> Player)
            {
                if (Player.Count == 3)
                    return new Player()
                    {
                        Country = Player[0].GetAttributeValue("title", string.Empty),
                        Name = Player[2].GetAttributeValue("title", string.Empty),
                        RaceLong = Player[1].GetAttributeValue("title", string.Empty)
                    };
                if (Player.Count == 1)
                    return new Player()
                    {
                        Name = Player[0].InnerText,
                        Country = string.Empty,
                        RaceLong = string.Empty
                    };
                return new Player();
            }

            return listTransfert;
        }

        public List<Event> ParsingEvents(HtmlDocument page, Period p, bool All = false)
        {
            List<Event> listEvent = new List<Event>();

            return listEvent;
        }

        #region Filters
        private bool IsTransfertLine(HtmlNode t) => (t.Name == "div" && t.Attributes["class"] != null && t.Attributes["class"].Value.Contains("divRow"));
        private bool IsTransfertTable(HtmlNode t) => (t.Name == "div" && t.Attributes["class"] != null && t.Attributes["class"].Value.Contains("divTable mainpage-transfer Ref"));
        private bool IsDate(HtmlNode t) => (t.Attributes["class"] != null && t.Attributes["class"].Value.Equals("divCell Date"));
        private bool IsPlayer(HtmlNode t) => (t.Attributes["class"] != null && t.Attributes["class"].Value.Equals("divCell Name"));
        private bool IsTeam(HtmlNode t) => (t.Attributes["class"] != null && t.Attributes["class"].Value.Equals("divCell Team"));
        #endregion
    }
}
