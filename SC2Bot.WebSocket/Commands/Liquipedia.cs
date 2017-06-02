using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using c = Crawlers;

namespace SC2Bot.WebSocket.Commands
{
    public class Liquipedia : ModuleBase
    {
        [Command("transfer"), Summary("Get a random quote of a player.")]
        public async Task GetRandomQuote()
        {
            var TL = await new c.Liquipedia().GetTransfert();
            await ReplyAsync("");
        }

        private Embed CreateEmbedQuote(QuoteOfTheDay.Datas.Quote q)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.Color = new Color(102, 0, 102);

            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = q.Author;
                x.Value = q.Text.Replace("\\n", "\n");
            });

            eb.Footer = new EmbedFooterBuilder().WithText($"{q.Date.ToShortDateString()} - {q.Reference.Replace("\\n", "\n")}")
                                                .WithIconUrl(@"http://i.imgur.com/yxbWEK0.png");

            return eb;
        }
    }
}
