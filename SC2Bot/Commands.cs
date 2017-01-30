using AligulacSC2;
using QuoteOfTheDay;
using SC2Bot.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace SC2Bot
{
    public static class Commands
    {
        public static async Task<string> SelectCommands(string query, Discord.Server s = null, Discord.User u = null, bool isPM = false)
        {
            if (IsRaceAssignement(query) && isPM)
                return await AssignRace(query.ToLower(), s, u);

                if (query[0] != '!') return null;

            var parser = new Parser(query);

            if (parser.Method == null) return null;

            switch (parser.Method.ToLower())
            {
                case "quote": return await Quote(parser, s, u);
                case "top": return await Top(parser, s, u);
                case "player": return await Player(parser);
                case "predict": return await Predict(parser);
                case "balance": return await Balance(parser);
                case "help": return Help();
            }

            return null;
        }

        public static string Help()
        {
            return Properties.Resources.HelpCommand;
        }

        public static async Task<string> Player(Parser parser, Discord.Server s = null, Discord.User u = null)
        {
            if (parser.Parameters == null)
                return Properties.Resources.PlayerCommandMissing;

            if (parser.Parameters[0] == "-help")
                return Properties.Resources.PlayerCommandHelp;

            return Aligulac.ShowPlayerObject(await Aligulac.Player(parser.Parameters[0]));
        }

        public static async Task<string> Balance(Parser parser, Discord.Server s = null, Discord.User u = null)
        {
            if (parser.Parameters != null && parser.Parameters.Length > 0 && parser.Parameters[0] == "-help")
                return "";

            return Aligulac.ShowPeriodObject(await Aligulac.Balance());
        }

        public static async Task<string> Predict(Parser parser, Discord.Server s = null, Discord.User u = null)
        {

            if (parser.Parameters == null)
                return Properties.Resources.PredictCommandMissing;


            if (parser.Parameters[0] == "-help")
                return Properties.Resources.PredictCommandHelp;

            if (parser.Parameters.Length < 2)
                return Properties.Resources.PredictCommandMissing;

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

        public static async Task<string> Top(Parser parser, Discord.Server s = null, Discord.User u = null)
        {
            if (parser.Parameters != null
                && parser.Parameters.Length == 1
                && parser.Parameters[0] == "-help")
                return Properties.Resources.TopCommandHelp;

            int topNb = 10;
            if (parser.Parameters != null && Helpers.Discord.IsAdmin(u))
                int.TryParse(parser.Parameters[0], out topNb);
            return Aligulac.ShowTopObject(await Aligulac.Top(topNb));
        }

        public static async Task<string> Quote(Parser parser, Discord.Server s = null, Discord.User u = null)
        {
            var allQuotes = new SC2Quotes();
            var quote = new QuoteOfTheDay.Datas.Quote();

            if (parser.Parameters != null && parser.Parameters[0] == "-help")
                return Properties.Resources.QuoteCommandHelp;

            if (parser.Parameters != null && parser.Parameters[0] == "-all")
            {
                await u.SendMessage(allQuotes.FormatAuthors(allQuotes.AllAuthor()));
                return Properties.Resources.QuoteCommandSendMP;
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
                    return allQuotes.FormatQuote(quote, string.Format(Properties.Resources.QuoteCommandNotFound, parser.Parameters[0]));
                }
            }

            quote = allQuotes.RandomQuote();
            return allQuotes.FormatQuote(quote);
        }

        public static async Task<string> AssignRace(string race, Discord.Server s = null, Discord.User u = null)
        {
            var zRole = s.FindRoles("Zerg", true).First();
            var tRole = s.FindRoles("Terran", true).First();
            var pRole = s.FindRoles("Protoss", true).First();
            var rRole = s.FindRoles("Random", true).First();

            switch (race)
            {
                case "zerg":
                    await Helpers.Discord.ClearAndAddRole(s, u, zRole);
                    return Properties.Resources.ZergQuote;
                case "protoss":
                    await Helpers.Discord.ClearAndAddRole(s, u, pRole);
                    return Properties.Resources.ProtossQuote;
                case "terran":
                    await Helpers.Discord.ClearAndAddRole(s, u, tRole);
                    return Properties.Resources.TerranQuote;
                case "random":
                    await Helpers.Discord.ClearAndAddRole(s, u, rRole);
                    return Properties.Resources.RandomQuote;
            }

            return null;
        }

        private static bool IsRaceAssignement(string q)
        {
            if (q == "zerg" ||
                q == "protoss" ||
                q == "terran" ||
                q == "random")
                return true;
            return false;
        }
    }
}
