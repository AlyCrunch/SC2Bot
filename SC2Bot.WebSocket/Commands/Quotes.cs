using Discord.Commands;
using hs = SC2Bot.WebSocket.Helpers.HelpStrings.Quote;
using System.Linq;
using System.Threading.Tasks;
using QuoteOfTheDay;
using Discord;

namespace SC2Bot.WebSocket.Commands
{
    public class Quotes : ModuleBase
    {
        [Command("quote"), Summary(hs.quoteSummary), Remarks(hs.quoteRemarks)]
        public async Task GetQuoteByPlayer([Summary("Nom de l'auteur")] string player = null)
        {
            var allQuotes = new SC2Quotes();

            if(player == null)
            {
                await GetRandomQuote(allQuotes);
                return;
            }

            if (player == "all")
            {
                //string reply = $"Liste des auteurs de quotes\n```{ string.Join(", ", allQuotes.AllAuthor()) }```";
                string reply = string.Format(Properties.Resources.AuthorQuotesList, string.Join(", ", allQuotes.AllAuthor()));
                var DMChannel = await Context.User.CreateDMChannelAsync();
                await DMChannel.SendMessageAsync(reply);

                await ReplyAsync(Properties.Resources.QuoteCommandSendMP);
                return;
            }

            if (!allQuotes.AllAuthor().Any(x => x.ToLower() == player.ToLower()))
            {
                await ReplyAsync(Properties.Resources.NoQuoteFor);
                await ReplyAsync("", false, CreateEmbedQuote(allQuotes.RandomQuote()));
                return;
            }

            var quote = allQuotes.RandomQuoteAuthor(player);
            await ReplyAsync("", false, CreateEmbedQuote(quote));
        }
        
        public async Task GetRandomQuote(SC2Quotes q)
        {
            var quote = q.RandomQuote();

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
