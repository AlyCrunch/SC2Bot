using Discord.Commands;
using a = AligulacSC2;
using o = AligulacSC2.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace SC2Bot.WebSocket.Commands
{
    public class Aligulac : ModuleBase
    {
        private a.Aligulac al = Helpers.Infos.Aligulac;
        private Color AligulacColor = new Color(255, 255, 255);

        [Command("predict"), Summary("Get prediction about matches.")]
        public async Task Predict([Summary("First player")] string player1, [Summary("Second player")] string player2, [Summary("Optionnal, number of best of")] int BO = 3)
        {
            var retPred = await al.Predict(player1, player2, BO.ToString());

            if (string.IsNullOrEmpty(retPred.Error))
                await ReplyAsync("", false, CreateEmbedPrediction(retPred));
            else
                await ReplyAsync(retPred.Error);
        }

        [Command("top"), Summary("Get top player.")]
        public async Task Top([Summary("Optional, TOP number max")] int NbTop = 10)
        {
            var top = await al.Top(NbTop);
            await ReplyAsync("", false, CreateEmbedTop(top, NbTop));
        }

        private Embed CreateEmbedPrediction(o.Prediction p)
        {
            EmbedBuilder eb = new EmbedBuilder()
            {
                Color = AligulacColor,
                Author = new EmbedAuthorBuilder().WithName("Aligulac")
                                                .WithIconUrl("http://i.imgur.com/HcSfSR2.png")
                                                .WithUrl(p.URL)
            };
            p.Outcomes = p.Outcomes.OrderByDescending(x => x.Prob).ToArray();

            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = p.PlayerA.Tag + $" ({FloatToPercentString(p.ProbA)})";
                x.Value = string.Join("\n", p.Outcomes.Select(s => s.ScoreA));
            });

            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = (p.ProbA > p.ProbB) ? "<-----" : "----->";
                x.Value = string.Join("\n", p.Outcomes.Select(s => FloatToPercentString(s.Prob)));
            });

            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = p.PlayerB.Tag + $" ({FloatToPercentString(p.ProbB)})";
                x.Value = string.Join("\n", p.Outcomes.Select(s => s.ScoreB));
            });

            string FloatToPercentString(float f)
            {
                return Math.Round((Decimal)(f * 100), 2, MidpointRounding.AwayFromZero) + " %";
            }

            return eb;
        }

        private Embed CreateEmbedTop(o.GenericResult<o.Player> ps, int nb)
        {
            EmbedBuilder eb = new EmbedBuilder()
            {
                Color = AligulacColor,
                Author = new EmbedAuthorBuilder().WithName("Aligulac")
                                                .WithIconUrl("http://i.imgur.com/HcSfSR2.png")
                                                .WithUrl("http://aligulac.com/periods/latest/")
            };

            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = "TOP " + nb;
                x.Value = string.Join("\n", ps.Results.Select(p => FormatPlayer(p)));
            });

            string FormatPlayer(o.Player p)
            {
                return $":flag_{p.Country.ToLower()}: {RaceFormat(p.Race)} {p.Tag}";
            }


            return eb;
        }

        private string RaceFormat(string r)
        {
            switch (r)
            {
                case "Z": return Properties.Resources.Emoji_Zerg;
                case "P": return Properties.Resources.Emoji_Protoss;
                case "T": return Properties.Resources.Emoji_Terran;
                case "R": return Properties.Resources.Emoji_Random;
                default: return $"({r})";
            }
        }
    }

    [Group("balance")]
    public class AligulacBalance : ModuleBase
    {
        private a.Aligulac al = Helpers.Infos.Aligulac;
        private Color AligulacColor = new Color(255, 255, 255);

        [Command(""), Summary("Get stats about global winrate races.")]
        public async Task Balance(int MaxResult = 20)
        {
            var bs = await al.Balance(new DateTime(), new DateTime(), false, false, false, MaxResult);
            await ReplyAsync("", false, CreateEmbedBalanceSimplified(bs));
        }

        [Command("avg"), Summary("Get stats about global winrate races.")]
        public async Task BalanceAverage()
        {
            var bs = await al.Balance(new DateTime(), new DateTime(), true, false, false);
            await ReplyAsync("", false, CreateEmbedBalance(bs));
        }

        [Command("op"), Summary("Get stats about global winrate races.")]
        public async Task BalanceOP()
        {
            var bs = await al.Balance(new DateTime(), new DateTime(), false, true, false);
            await ReplyAsync("", false, CreateEmbedBalance(bs));
        }

        [Command("up"), Summary("Get stats about global winrate races.")]
        public async Task BalanceUP()
        {
            var bs = await al.Balance(new DateTime(), new DateTime(), false, false, true);
            await ReplyAsync("", false, CreateEmbedBalance(bs));
        }

        private Embed CreateEmbedBalance(o.GenericResult<o.Period> ps)
        {
            EmbedBuilder eb = new EmbedBuilder()
            {
                Color = AligulacColor,
                Author = new EmbedAuthorBuilder().WithName("Aligulac")
                                                .WithIconUrl("http://i.imgur.com/HcSfSR2.png")
                                                .WithUrl("http://aligulac.com/misc/balance/")
            };

            foreach (var p in ps.Results)
                eb.AddField(x =>
                {
                    x.IsInline = false;
                    x.Name = FormatPeriodDate(p);
                    x.Value = FormatPeriodResult(p);
                });

            string FormatPeriodDate(o.Period p)
            {
                return $"{p.StartDate.ToShortDateString()} -------> {p.EndDate.ToShortDateString()}";
            }
            string FormatPeriodResult(o.Period p)
            {
                return $"{RacePeriod(p.Leading, true, false)}    | {RacePeriod(p.MidRace, false, false)}    | {RacePeriod(p.Lagging, false, true)}";
            }

            string RacePeriod(o.LeadingRace r, bool isLeading, bool isLagging)
            {
                string percent = $"{r.DifferencePourcent}%";

                if (r.isOP || r.isWeak) percent = $"__**{percent}**__";

                return $"{RaceFormat(r.Race)} {percent}";
            }

            return eb;
        }

        private Embed CreateEmbedBalanceSimplified(o.GenericResult<o.Period> ps)
        {
            EmbedBuilder eb = new EmbedBuilder()
            {
                Color = AligulacColor,
                Author = new EmbedAuthorBuilder().WithName("Aligulac")
                                                .WithIconUrl("http://i.imgur.com/HcSfSR2.png")
                                                .WithUrl("http://aligulac.com/periods/"),
                Description = "Représente la domination de race dans le top player. Au dessus de 10% une race est considérée OP (OverPowered) et en dessous de 10% UP (UnderPowered), ces chiffres sont soulignés dans la liste."
            };
            var psa = ps.Results;

            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = "Date (games)";
                x.Value = string.Join("\n", psa.Select(p => $":black_small_square:**{p.EndDate.ToShortDateString()}** ({p.NumGames})"));
            });

            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = "Dominant";
                x.Value = string.Join("\n", psa.Select(p => RacePeriod(p.Leading, true, false)));
            });

            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = "Dominé";
                x.Value = string.Join("\n", psa.Select(p => RacePeriod(p.Lagging, false, true)));
            });
            
            string RacePeriod(o.LeadingRace r, bool isLeading, bool isLagging)
            {
                string percent = $"{r.DifferencePourcent}%";

                if (r.isOP || r.isWeak) percent = $"__**{percent}**__";

                return $"{RaceFormat(r.Race)} {percent}";
            }

            return eb;
        }

        private string RaceFormat(string r)
        {
            switch (r)
            {
                case "Z": return Properties.Resources.Emoji_Zerg;
                case "P": return Properties.Resources.Emoji_Protoss;
                case "T": return Properties.Resources.Emoji_Terran;
                case "R": return Properties.Resources.Emoji_Random;
                default: return $"({r})";
            }
        }
    }
}