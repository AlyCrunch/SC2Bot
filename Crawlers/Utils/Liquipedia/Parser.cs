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

        public List<Event> ParsingEventsDay(HtmlDocument page, DateTime date)
        {
            return GetEvents(page.DocumentNode, date);
        }

        public List<Event> ParsingEventsWeek(HtmlDocument page, DateTime date)
        {
            var weekEvents = new List<Event>();

            for (int i = 0; i < 7; i++)
            {
                weekEvents.AddRange(GetEvents(page.DocumentNode, date.AddDays(i)));
            }
            return weekEvents;
        }

        public List<Event> ParsingLiveEvents(HtmlDocument page)
        {
            var listEventsN = page.DocumentNode.Descendants().First(x => IsEventsTabLive(x))
                                              .Descendants().Where(x => IsEventLive(x) && IsStarcraftEvent(x));
            var events = new List<Event>();

            foreach (var e in listEventsN)
            {
                var nodeIDTitle = e.Descendants().First(x => IsEventIDTitle(x));
                var nodeSubtitle = e.Descendants().First(x => IsEventSubtitle(x));
                var dateStr = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + e.Descendants().First(x => IsEventHour(x)).InnerText + "Z");

                var ev = new Event()
                {
                    ID = nodeIDTitle.Attributes["data-event-id"].Value,
                    Title = nodeIDTitle.InnerText,
                    Subtitle = nodeSubtitle.InnerText,
                    Date = dateStr,
                    Matches = new List<Match>(),
                    Streams = new List<Stream>()
                };

                var nodeMatches = e.Descendants().First(x => IsMatchesList(x))
                                   .ChildNodes.Where(x => x.Name == "div");

                foreach (var m in nodeMatches)
                {
                    ev.Matches.Add(GetMatch(m));
                }

                var nodeStreams = e.Descendants().First(x => IsStreamList(x))
                                   .ChildNodes.Where(x => x.Name == "div");

                foreach (var s in nodeStreams)
                {
                    ev.Streams.Add(GetStream(s));
                }

                events.Add(ev);

            }

            Match GetMatch(HtmlNode n)
            {
                var nms = n.Descendants().Where(x => IsMatchPlayer(x));
                var t = (n.Descendants().First(x => IsEventHour(x)).InnerText != string.Empty) ? StatusEnum.Live : StatusEnum.Pending;

                Match m = new Match()
                {
                    PlayerA = nms.ToArray()[0].InnerText,
                    PlayerB = nms.ToArray()[1].InnerText,
                    Status = t
                };
                return m;
            }
            
            Stream GetStream(HtmlNode n)
            {
                var flag = (n.Descendants().Any(x => IsLang(x))) ? ExtractFlag(n.Descendants().First(x => IsLang(x)).Attributes["src"].Value) : "";
                var sn = n.Descendants().First(x => x.Name == "a");

                Stream s = new Stream()
                {
                    Name = sn.InnerText,
                    Link = "http://www.teamliquid.net" + sn.Attributes["href"].Value,
                    Lang = flag
                };
                return s;
            }
            string ExtractFlag(string url)
            {
                int pFrom = url.IndexOf("/images/flags2/") + "/images/flags2/".Length;
                int pTo = url.LastIndexOf(".png");

                return url.Substring(pFrom, pTo - pFrom);
            }

            return events;
        }

        private List<Event> GetEvents(HtmlNode page, DateTime date)
        {
            var eventsNode = page.Descendants().First(x => IsEventsDay(x, date.Day))
                                             .Descendants().Where(n => IsEventLine(n));

            List<Event> listEvent = new List<Event>();

            foreach (var e in eventsNode)
            {
                string ID, title, subtitle, time;

                var nodeIDTitle = e.Descendants().First(x => IsEventIDTitle(x));
                ID = nodeIDTitle.Attributes["data-event-id"].Value;
                title = nodeIDTitle.InnerText;
                subtitle = e.Descendants().First(x => IsEventSubtitle(x)).InnerText;
                time = e.Descendants().First(x => IsEventHour(x)).InnerText;

                var dateE = date;
                dateE = DateTime.Parse(date.ToShortDateString() + " " + time + "Z");

                listEvent.Add(new Event()
                {
                    ID = ID,
                    Title = title,
                    Subtitle = subtitle,
                    Date = dateE
                });
            }

            return listEvent;
        }

        #region Filters

        #region Transfer
        private bool IsTransfertLine(HtmlNode t) => (t.Name == "div" && t.Attributes["class"] != null && t.Attributes["class"].Value.Contains("divRow"));
        private bool IsTransfertTable(HtmlNode t) => (t.Name == "div" && t.Attributes["class"] != null && t.Attributes["class"].Value.Contains("divTable mainpage-transfer Ref"));
        private bool IsDate(HtmlNode t) => (t.Attributes["class"] != null && t.Attributes["class"].Value.Equals("divCell Date"));
        private bool IsPlayer(HtmlNode t) => (t.Attributes["class"] != null && t.Attributes["class"].Value.Equals("divCell Name"));
        private bool IsTeam(HtmlNode t) => (t.Attributes["class"] != null && t.Attributes["class"].Value.Equals("divCell Team"));
        #endregion

        #region Events
        private bool IsEventsDay(HtmlNode n, int day) => (n.Name == "div" && n.Attributes["data-day"] != null && n.Attributes["data-day"].Value.Contains(day.ToString()));
        private bool IsEventLine(HtmlNode n) => (n.Name == "div" && n.Attributes["class"] != null && n.Attributes["class"].Value.Contains("ev-block"));
        private bool IsEventsTabLive(HtmlNode n) => (n.Name == "div" && n.Attributes["id"] != null && n.Attributes["id"].Value.Contains("live_events_block"));
        private bool IsEventLive(HtmlNode n) => (n.Name == "div" && n.Attributes["class"] != null && n.Attributes["class"].Value.Contains("ev-block ev-live"));
        private bool IsStarcraftEvent(HtmlNode n) => (n.Descendants().Any(x => x.Attributes["style"] != null && x.Attributes["style"].Value.Equals("background: url(/images/games/1.png) transparent no-repeat")));
        private bool IsEventIDTitle(HtmlNode n) => (n.Name == "span" && n.Attributes["data-event-id"] != null);
        private bool IsEventSubtitle(HtmlNode n) => (n.Name == "div" && n.Attributes["class"] != null && n.Attributes["class"].Value.Contains("ev-stage"));
        private bool IsEventHour(HtmlNode n) => (n.Name == "span" && n.Attributes["class"] != null && n.Attributes["class"].Value.Contains("ev-timer"));

        private bool IsMatchesList(HtmlNode n) => (n.Name == "div" && n.Attributes["class"] != null && n.Attributes["class"].Value.Contains("ev-match"));
        private bool IsMatchPlayer(HtmlNode n) => (n.Name == "span" && n.Attributes["class"] != null && n.Attributes["class"].Value.Contains("ev-player"));

        private bool IsStreamList(HtmlNode n) => (n.Name == "div" && n.Attributes["class"] != null && n.Attributes["class"].Value.Contains("ev-stream"));

        private bool IsWiki(HtmlNode n) => (n.Name == "a" && n.Attributes["class"] != null && n.Attributes["class"].Value.Contains("ev-btn wikibtn"));
        private bool IsThread(HtmlNode n) => (n.Name == "a" && n.Attributes["class"] != null && n.Attributes["class"].Value.Contains("ev-btn lrbtn"));
        private bool IsLang(HtmlNode n) => (n.Name == "img");
        #endregion

        #endregion
    }
}
