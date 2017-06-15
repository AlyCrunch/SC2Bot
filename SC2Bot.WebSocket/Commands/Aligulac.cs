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

        [Command("balance"), Summary("Get stats about global winrate races.")]
        public async Task Balance()
        {
            await ReplyAsync("");
        }

        [Command("predict"), Summary("Get stats about global winrate races.")]
        public async Task Predict([Summary("First player")] string player1, [Summary("Second player")] string player2, [Summary("Optionnal, number of best of")] int BO = 3)
        {
            var retPred = await al.Predict(player1, player2, BO.ToString());

            if (string.IsNullOrEmpty(retPred.Error))
                await ReplyAsync("", false, CreateEmbedTransfer(retPred));
            else
                await ReplyAsync(retPred.Error);
        }

        [Command("top"), Summary("Get stats about global winrate races.")]
        public async Task Top([Summary("Optional, TOP number max")] int NbTop = 10)
        {
            var top = await al.Top(NbTop);
            await ReplyAsync("", false, CreateEmbedTop(top, NbTop));
        }

        private Embed CreateEmbedTransfer(o.Prediction p)
        {
            EmbedBuilder eb = new EmbedBuilder()
            {
                Color = AligulacColor,
                Author = new EmbedAuthorBuilder().WithName("Aligulac")
                                                .WithIconUrl("http://i.imgur.com/HcSfSR2.png")
                                                .WithUrl(p.URL)
            };

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
            string RaceFormat(string r)
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

            return eb;
        }

    }
}