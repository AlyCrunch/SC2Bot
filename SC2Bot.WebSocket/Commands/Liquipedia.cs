using Crawlers.Objects.Liquipedia;
using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SC2Bot.WebSocket.Commands
{
    public class Liquipedia : ModuleBase
    {
        [Command("transfer"), Summary("Get a random quote of a player.")]
        public async Task GetTransferList()
        {
            var TL = await new Crawlers.Liquipedia().GetTransfert();
            await ReplyAsync("", false, CreateEmbedQuote(TL));
        }

        private Embed CreateEmbedQuote(List<Transfert> tl)
        {
            EmbedBuilder eb = new EmbedBuilder()
            {
                Color = new Color(102, 0, 102),
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
    }
}
