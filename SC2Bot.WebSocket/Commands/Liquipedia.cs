using Crawlers.Objects.Liquipedia;
using hs = SC2Bot.WebSocket.Helpers.HelpStrings.Liquipedia;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SC2Bot.WebSocket.Commands
{
    public class Liquipedia : ModuleBase
    {
        private static Color LiquipediaColor = new Color(37, 70, 115);

        [Command("transfer"), Summary(hs.transferSummary), Remarks(hs.transferRemarks)]
        public async Task GetTransferList()
        {
            var TL = await new Crawlers.Liquipedia().GetTransfert();
            await ReplyAsync("", false, CreateEmbedTransfer(TL));
        }

        [Command("live"), Summary(hs.liveSummary), Remarks(hs.liveRemarks)]
        public async Task GetLiveEvents()
        {
            var LE = await new Crawlers.Liquipedia().GetLiveEvents();

            if (LE.Count() == 0) await ReplyAsync("Pas d'événement pour le moment.");

            foreach (var e in LE)
            {
                await ReplyAsync("", false, CreateEmbedLiveEvent(e));
            }
        }
        
        [Command("event"), Summary(hs.eventSummary), Remarks(hs.eventRemarks)]
        public async Task GetEventsType([Summary("Jour ou Semaine (day|week) (optionnel, day)")] string type = "day")
        {
            await SendEvents(await new Crawlers.Liquipedia().GetCalendarEvents(DateTime.Now, IsWeek(type)));
        }
        
        private async Task SendEvents(List<Event> es)
        {
            var gd = es.GroupBy(x => x.Date.Date);
            foreach (var esd in gd)
                await ReplyAsync("", false, CreateEmbedDayEvent(esd));
        }

        private static Embed CreateEmbedTransfer(List<Transfert> tl)
        {
            EmbedBuilder eb = new EmbedBuilder()
            {
                Color = LiquipediaColor,
                Author = new EmbedAuthorBuilder().WithName("Liquipedia")
                                                .WithIconUrl("http://wiki.teamliquid.net/starcraft2/skins/LiquiFlow/images/liquipedia_logo.png")
                                                .WithUrl("http://wiki.teamliquid.net/starcraft2/Main_Page")
            };

            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = "PLAYER";
                x.Value = string.Join("\n", tl.Select(t => string.Join(", ", t.Players.Select(n => $"**{n.Name}**"))));
            });
            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = "OLD TEAM";
                x.Value = string.Join("\n", tl.Select(t => t.OldTeam.Name));
            });
            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = "NEW TEAM";
                x.Value = string.Join("\n", tl.Select(t => FormatRetiring(t.NewTeam.Name)));
            });

            string FormatRetiring(string t)
            {
                return (t == "Retirement") ? $"**{t}**" : t;
            }

            return eb;
        }

        private static Embed CreateEmbedLiveEvent(Event e)
        {
            EmbedAuthorBuilder eab = null;

            if (e.Wikipedia != "")
                eab = new EmbedAuthorBuilder().WithName("Wiki")
                                                    .WithIconUrl("http://www.teamliquid.net/images/frontpage/games/tl_16.png")
                                                    .WithUrl(e.Wikipedia);
            else
                eab = new EmbedAuthorBuilder().WithName("teamliquid")
                                                    .WithIconUrl("http://www.teamliquid.net/images/frontpage/games/tl_16.png");

            EmbedBuilder eb = new EmbedBuilder()
            {
                Color = LiquipediaColor,
                Author = eab,
                Title = e.Title,
                Description = e.Subtitle
            };

            if (e.Thread != "")
                eb.Url = e.Thread;

            if (e.Matches == null || e.Matches.Count == 0)
                return eb;

            var fm = e.Matches.First();
            var lm = new List<Match>();
            if (e.Matches.Count > 1)
            {
                for (int i = 1; i < e.Matches.Count; i++)
                {
                    lm.Add(e.Matches.ElementAt(i));
                }
            }
            else
                lm.Add(new Match()
                {
                    PlayerA = ".",
                    PlayerB = "."
                });

            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = fm.PlayerA;
                x.Value = string.Join("\n", lm.Select(t => t.PlayerA));
            });
            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = "vs";
                x.Value = (lm.First().PlayerA == ".") ? "." : string.Join("\n", lm.Select(t => "vs"));
            });
            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = fm.PlayerB;
                x.Value = string.Join("\n", lm.Select(t => t.PlayerB));
            });

            var links = GetLinks(e);
            if (links != "")
                eb.AddField(x =>
                {
                    x.IsInline = false;
                    x.Name = "Links";
                    x.Value = links;
                });

            return eb;
        }

        private static Embed CreateEmbedDayEvent(IGrouping<DateTime, Event> events)
        {
            EmbedBuilder eb = new EmbedBuilder()
            {
                Color = LiquipediaColor,
                Author = new EmbedAuthorBuilder().WithName("teamliquid")
                                                    .WithIconUrl("http://www.teamliquid.net/images/frontpage/games/tl_16.png")
            };
            var c = new System.Globalization.CultureInfo("fr-FR");
            
            
            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = c.DateTimeFormat.GetDayName(events.Key.DayOfWeek);
                x.Value = string.Join("\n", events.Select(t => $"**{t.Title}**\n{t.Subtitle}\n"));
            });


            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = "·";
                x.Value = string.Join("\n", events.Select(t => $"------------------------\n\n"));
            });

            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = events.Key.ToString("dd MMMM");
                x.Value = string.Join("\n", events.Select(t => $"{t.Date.ToShortTimeString()}\n\n"));
            });

            return eb;
        }

        private static string GetLinks(Event e)
        {
            string rtnListLinks = string.Empty;

            rtnListLinks += string.Join("\n", e.Streams.Select(s => $"[{s.Name}]({s.Link})  {Flags(s)}"));

            string Flags(Stream s)
            {
                if (string.IsNullOrEmpty(s.Lang))
                    return "";

                return $":flag_{s.Lang}: ";
            }

            return rtnListLinks;
        }

        private Period IsWeek(string s) => (s.ToLower() == "week") ? Period.Week : Period.Day;

        public static async Task<List<Event>> GetEventOfTheDay()
        {
            var es = await new Crawlers.Liquipedia().GetCalendarEvents(DateTime.Now, Period.Day);
            var gd = es.GroupBy(x => x.Date.Date);
            return gd.First().ToList();
        }

        public static IEnumerable<Embed> GetLiveEventsExt()
        {
            var LE = new Crawlers.Liquipedia().GetLiveEvents().Result;
            foreach (var e in LE)
            {
                yield return CreateEmbedLiveEvent(e);
            }
        }
    }
}
