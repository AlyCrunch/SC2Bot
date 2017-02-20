using Crawlers.Objects;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Crawlers.Utils
{
    public class Parser
    {
        public async Task<List<Transfert>> ParsingTransfers(string url)
        {
            var http = new HttpClient();
            var response = await http.GetByteArrayAsync(url);

            String source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            HtmlDocument resultat = new HtmlDocument();
            resultat.LoadHtml(source);


            HtmlNode transfertNode = resultat.DocumentNode.Descendants().
                First(x => (x.Name == "div"
                            && x.Attributes["class"] != null
                            && x.Attributes["class"].Value.Equals("divTable mainpage-transfer Ref")));

            var cleanTN = transfertNode.Descendants().Where(x => (x.Name == "div"
                            && x.Attributes["class"] != null
                            && x.Attributes["class"].Value.Contains("divRow")));

            var listTransfert = new List<Transfert>();

            foreach (var child in cleanTN)
            {
                var t = new Transfert();
                var pl = new List<Player>();
                var d = new DateTime();
                t.Players = new List<Player>();

                var dateNode = child.Descendants().First(x => x.Attributes["class"] != null
                                                              && x.Attributes["class"].Value.Equals("divCell Date"));
                var playersNode = child.Descendants().First(x => x.Attributes["class"] != null
                                                              && x.Attributes["class"].Value.Equals("divCell Name"));
                var teamsNode = child.Descendants().Where(x => x.Attributes["class"] != null
                                                              && x.Attributes["class"].Value.Equals("divCell Team")).ToArray();


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
                    var name = TeamNode.Descendants("img").First().GetAttributeValue("alt", "Aucune");
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
                return new Player()
                {
                    Country = Player[0].GetAttributeValue("title", string.Empty),
                    Name = Player[2].GetAttributeValue("title", string.Empty),
                    RaceLong = Player[1].GetAttributeValue("title", string.Empty)
            };
            }

            return listTransfert;
        }
    }
}
