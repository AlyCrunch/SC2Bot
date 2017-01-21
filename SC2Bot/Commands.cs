using AligulacSC2;
using QuoteOfTheDay;
using SC2Bot.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace SC2Bot
{
    public static class Commands
    {
        public static async Task<string> SelectCommands(string query, Discord.User u = null)
        {
            if (query[0] != '!') return null;

            var parser = new Parser(query);

            if (parser.Method == null) return null;

            switch (parser.Method.ToLower())
            {
                case "quote": return await Quote(parser, u);
                case "top": return await Top(parser);
                case "player": return await Player(parser);
                case "predict": return await Predict(parser);
                case "help": return Help();
            }

            return null;
        }

        public static string Help()
        {
            return "Il y a plusieurs commandes disponibles : `!player` `!top` `!predict` `!quote`\nIl suffit d'utiliser le mot clé \"**-help**\" pour plus d'information, example :\n`!predict -help`";
        }

        public static async Task<string> Player(Parser parser)
        {
            if (parser.Parameters == null)
                return "Il manque des paramètres.\nLa syntaxe correcte est : \n `!search mot_clé)`";

            if (parser.Parameters[0] == "-help")
                return "Recherche un joueur sur Aligulac, si les informations sont disponibles, la team ainsi que la page liquipedia est retournée.\nLa syntaxe à utiliser est : \n `!player pseudo)`";

            return Aligulac.ShowPlayerObject(await Aligulac.Player(parser.Parameters[0]));
        }

        public static async Task<string> Predict(Parser parser)
        {

            if (parser.Parameters == null)
                return "Il manque des paramètres.\nLa syntaxe correcte est : \n `!predict Player_A Player_B (Optionnel BO_nb)`\n\nExample :\n `!predict ByuN Dark 3`";

            if (parser.Parameters[0] == "-help")
                return "Utilise le système de prédiction d'Aligulac (http://aligulac.com/inference/).\nLa syntaxe à utiliser est : \n`!predict Player_A Player_B (Optionnel BO_nb)`\n\nExample :\n`!predict ByuN Dark 3`";

            if (parser.Parameters.Length < 2)
                return "Il manque des paramètres.\nLa syntaxe correcte est : \n `!predict Player_A Player_B (Optionnel BO_nb)`\n\nExample :\n `!predict ByuN Dark 3`";

            #region EasterEggCrunchyPredict
            if (parser.Parameters[0].ToLower() == "crunchy" || parser.Parameters[1].ToLower() == "crunchy")
            {
                if (parser.Parameters.Where(x => x == "-real").Count() > 0)
                {
                    int BOnb = 3;
                    if (parser.Parameters.Length > 2)
                        int.TryParse(parser.Parameters[2], out BOnb);

                    if (parser.Parameters[0].ToLower() == "crunchy")
                        return await Aligulac.CrunchyRules(parser.Parameters[1], BOnb);
                    else
                        return await Aligulac.CrunchyRules(parser.Parameters[0], BOnb);
                }
            }
            #endregion

            int nbBO = 3;

            if (parser.Parameters.Length == 3)
                int.TryParse(parser.Parameters[2], out nbBO);

            var retPred = await Aligulac.Predict(parser.Parameters[0], parser.Parameters[1], nbBO.ToString());

            if (string.IsNullOrEmpty(retPred.Error))
                return Aligulac.ShowPredictionObject(retPred);
            else
                return retPred.Error;
        }

        public static async Task<string> Top(Parser parser)
        {
            if (parser.Parameters != null
                && parser.Parameters.Length == 1
                && parser.Parameters[0] == "-help")
                return "Utilise le système de classement d'Aligulac.\nLa syntaxe à utiliser est : \n`!top (Optionnel nombre_de_joueur)`\n\nExample :\n`!top 20`";

            int topNb = 10;
            if (parser.Parameters != null)
                int.TryParse(parser.Parameters[0], out topNb);
            return Aligulac.ShowTopObject(await Aligulac.Top(topNb));
        }

        public static async Task<string> Quote(Parser parser, Discord.User u)
        {
            var allQuotes = new SC2Quotes();
            var quote = new QuoteOfTheDay.Datas.Quote();

            if (parser.Parameters != null && parser.Parameters[0] == "-help")
                return "Retourne une quote SC2 via le site sc2quoteoftheday.com (et plus)\n"
                    + "La syntaxe à utiliser est : `!quote (optionnel auteur) (optionnel -count)`\n"
                    + "Exemple :\n```\n"
                    + "# Récupère les quotes de l'auteur cité\n!quote Day9\n\n"
                    + "# Retourne le nombre de quote pour l'auteur cité\n!quote Day9 -count\n\n"
                    + "# Retourne tous les auteurs de quotes (en message privé)\n!quote -all```";


            if (parser.Parameters != null && parser.Parameters[0] == "-all")
            {
                await u.SendMessage(allQuotes.FormatAuthors(allQuotes.AllAuthor()));
                return "Liste envoyée en MP.";
            }

            if (parser.Parameters != null && parser.Parameters.Count() > 1 && parser.Parameters[1] == "-count")
            {
                var nb = allQuotes.CountQuotesAuthor(parser.Parameters[0]);
                return allQuotes.FormatNbQuote(parser.Parameters[0], nb);
            }

            if (parser.Parameters != null && parser.Parameters[0] != "-help")
            {
                var nb = allQuotes.CountQuotesAuthor(parser.Parameters[0]);
                if (nb > 0)
                    return allQuotes.FormatQuote(allQuotes.RandomQuoteAuthor(parser.Parameters[0]));
                else
                {
                    quote = allQuotes.RandomQuote();
                    return allQuotes.FormatQuote(quote, $"Je n'ai pas de quote pour {parser.Parameters[0]}\nMais celle là est pas mal quand même :\n");
                }
            }
                        
            quote = allQuotes.RandomQuote();
            return allQuotes.FormatQuote(quote);
        }
    }
}
