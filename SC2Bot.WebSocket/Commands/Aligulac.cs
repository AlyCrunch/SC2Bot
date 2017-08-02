using Discord.Commands;
using a = AligulacSC2;
using o = AligulacSC2.Objects;
using hs = SC2Bot.WebSocket.Helpers.HelpStrings.Aligulac;
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace SC2Bot.WebSocket.Commands
{
    [Remarks("")]
    public class Aligulac : ModuleBase
    {
        private a.Aligulac al = Helpers.Infos.Aligulac;
        private Color AligulacColor = new Color(255, 255, 255);

        [Command("predict"), Summary(hs.predictSummary), Remarks(hs.predictRemarks)]
        public async Task Predict([Summary("Joueur A")] string player1, [Summary("Joueur B")] string player2, [Summary("Best Of (Optionnel = 3)")] int BO = 3)
        {
            var retPred = await al.Predict(player1, player2, BO.ToString());

            if (string.IsNullOrEmpty(retPred.Error))
                await ReplyAsync("", false, CreateEmbedPrediction(retPred));
            else
                await ReplyAsync(retPred.Error);
        }

        [Command("top"), Summary(hs.topSummary), Remarks(hs.topRemarks)]
        public async Task Top([Summary("Nombre du top, max 60 (Optionnel, 10)")] int NbTop = 10)
        {
            if (NbTop > 50) NbTop = 50;
            var top = await al.Top(NbTop);
            await ReplyAsync("", false, CreateEmbedTop(top, NbTop));
        }

        [Command("op"), Summary(hs.opSummary), Remarks(hs.opRemarks)]
        public async Task Balance(int MaxResult = 20)
        {
            var bs = await al.Balance(new DateTime(), new DateTime(), false, false, false, MaxResult);
            await ReplyAsync("", false, CreateEmbedBalance(bs));
        }

        [Command("earn"), Summary(hs.earnSummary), Remarks(hs.earnRemarks)]
        public async Task Earnings([Summary("Année(s) de gain (Optionnel, toutes les années)")] string year = "", [Summary("Pays (Norme ISO) (Optionnel, tous les pays)")] string country = "")
        {
            await ReplyAsync(Properties.Resources.ReleaseSoon);
        }

        private Embed CreateEmbedBalance(o.GenericResult<o.Period> ps)
        {
            EmbedBuilder eb = new EmbedBuilder()
            {
                Color = AligulacColor,
                Author = new EmbedAuthorBuilder().WithName("Aligulac")
                                                .WithIconUrl("http://i.imgur.com/HcSfSR2.png")
                                                .WithUrl("http://aligulac.com/periods/"),
                Description = "Au dessus de 10% une race est considérée OP (OverPowered) et en dessous de 10% UP (UnderPowered), ces chiffres sont soulignés dans la liste."
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

    /*
     * Shitty results
     * 
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
    */

}