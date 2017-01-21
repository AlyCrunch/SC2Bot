using QuoteOfTheDay.Datas;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace QuoteOfTheDay
{
    public class SC2Quotes
    {
        private static List<Quote> Quotes;

        public SC2Quotes()
        {
            if (Quotes == null)
                Deserialize();
        }

        private void Deserialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Quote>));
            XDocument myxml = XDocument.Load(@"Datas\Quotes.xml");

            using (var reader = myxml.Root.CreateReader())
            {
                Quotes = (List<Quote>)serializer.Deserialize(reader);
            }


        }

        public Quote RandomQuote() => Quotes[Helpers.Rand.Next(Quotes.Count)];

        public Quote RandomQuoteAuthor(string player)
        {
            var quotes = (Quotes.Where(x => x.Author.ToLower() == player.ToLower()));

            if (quotes == null || quotes.Count() == 0) return null;
            var nb = Quotes.Count(x => x.Author.ToLower() == player.ToLower());
            var i = Helpers.Rand.Next(nb);

            return quotes.ToArray()[i];
        }

        public int CountQuotesAuthor(string Author) => Quotes.Count(x => Author.ToLower() == x.Author.ToLower());

        public List<string> AllAuthor() => Quotes.Select(x => x.Author).Distinct().ToList();

        public string FormatQuote(Quote q, string Add = "")
        {
            var rtn = $"{Add}```\n{q.Text.Replace("\\n","\n")}```\n*{q.Reference}*\n**{q.Author}** - {q.Date.ToShortDateString()}";
            return rtn;
        }

        public string FormatNbQuote(string Auteur, int nbQuotes)
        {
            if (nbQuotes == 0)
                return $"Il n'y pas de quote pour {Auteur}";
            else
                return $"Il y a {nbQuotes} quote{((nbQuotes > 0) ? "s" : "")} pour {Auteur}";
        }

        public string FormatAuthors(List<string> Author)
        {
            Author.Sort();
            return $"Liste des auteurs des quotes :\n{ string.Join(", ", Author) }";
        }
    }
}
