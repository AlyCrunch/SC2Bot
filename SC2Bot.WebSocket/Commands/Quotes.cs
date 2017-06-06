using Discord.Commands;
using System.Linq;
using System.Threading.Tasks;
using QuoteOfTheDay;
using Discord;

namespace SC2Bot.WebSocket.Commands
{
    [Group("quote")]
    public class Quotes : ModuleBase
    {
        [Command(""), Summary("Get a random quote of a player.")]
        public async Task GetQuoteByPlayer([Summary("Player name.")] string player)
        {
            var allQuotes = new SC2Quotes();
            if (player == "all")
            {
                string reply = $"Liste des auteurs de quotes\n```{ string.Join(", ", allQuotes.AllAuthor()) }```";
                var prout = await Context.User.CreateDMChannelAsync();
                await prout.SendMessageAsync(reply);

                await ReplyAsync("Liste des joueurs envoyée en message privée.");
                return;
            }

            if (!allQuotes.AllAuthor().Any(x => x.ToLower() == player.ToLower()))
            {
                await ReplyAsync("Je n'ai pas de quote pour ce joueur.");
                await ReplyAsync("", false, CreateEmbedQuote(allQuotes.RandomQuote()));
                return;
            }

            var quote = allQuotes.RandomQuoteAuthor(player);
            await ReplyAsync("", false, CreateEmbedQuote(quote));
        }

        [Command(""), Summary("Get a random quote.")]
        public async Task GetRandomQuote()
        {
            var allQuotes = new SC2Quotes();
            var quote = allQuotes.RandomQuote();

            await ReplyAsync("", false, CreateEmbedQuote(quote));
        }

        private Embed CreateEmbedQuote(QuoteOfTheDay.Datas.Quote q)
        {
            EmbedBuilder eb = new EmbedBuilder()
            {
                Color = new Color(191, 0, 255),
                ThumbnailUrl = (q.Image != string.Empty) ? q.Image : null,
                Footer = new EmbedFooterBuilder().WithText($"{q.Date.ToShortDateString()} - {q.Reference.Replace("\\n", "\n")}")
                                                 .WithIconUrl(@"http://i.imgur.com/yxbWEK0.png")
            };

            eb.AddField(x =>
            {
                x.IsInline = true;
                x.Name = q.Author;
                x.Value = q.Text.Replace("\\n", "\n");
            });

            return eb;
        }

    }
}
