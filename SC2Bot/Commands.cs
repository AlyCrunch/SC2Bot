using AligulacSC2;
using QuoteOfTheDay;
using SC2Bot.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SC2Bot
{
    public static class Commands
    {
        private static System.Resources.ResourceManager _rm = Properties.Resources.ResourceManager;

        public static async Task<List<string>> SelectCommands(string query, Discord.Server s = null, Discord.User u = null, bool isPM = false)
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

        public static List<string> Help()
        {
            return ConvertSingleReturnToList(Res("HelpCommand"));
        }

        public static async Task<List<string>> Player(Parser parser, Discord.Server s = null, Discord.User u = null)
        {
            if (parser.Parameters == null)
                return ConvertSingleReturnToList(Res("PlayerCommandMissing"));

            if (parser.Parameters[0] == "-help")
                return ConvertSingleReturnToList(Res("PlayerCommandHelp"));

            return Aligulac.ShowPlayerObject(await Aligulac.Player(parser.Parameters[0]));
        }

        public static async Task<List<string>> Balance(Parser parser, Discord.Server s = null, Discord.User u = null)
        {
            if (parser.Parameters != null && parser.Parameters.Length > 0 && parser.Parameters[0] == "-help")
                return ConvertSingleReturnToList(Res("BalanceCommandHelp"));

            return Aligulac.ShowPeriodObject(await Aligulac.Balance());
        }

        public static async Task<List<string>> Predict(Parser parser, Discord.Server s = null, Discord.User u = null)
        {
            if (parser.Parameters == null)
                return ConvertSingleReturnToList(Res("PredictCommandMissing"));

            if (parser.Parameters[0] == "-help")
                return ConvertSingleReturnToList(Res("PredictCommandHelp"));

            if (parser.Parameters.Length < 2)
                return ConvertSingleReturnToList(Res("PredictCommandMissing"));

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
                return ConvertSingleReturnToList(retPred.Error);
        }

        public static async Task<List<string>> Top(Parser parser, Discord.Server s = null, Discord.User u = null)
        {
            if (parser.Parameters != null
                && parser.Parameters.Length == 1
                && parser.Parameters[0] == "-help")
                return ConvertSingleReturnToList(Res("TopCommandHelp"));

            int topNb = 10;
            if (parser.Parameters != null && Helpers.Discord.IsAdmin(u))
                int.TryParse(parser.Parameters[0], out topNb);
            return Aligulac.ShowTopObject(await Aligulac.Top(topNb));
        }

        public static async Task<List<string>> Quote(Parser parser, Discord.Server s = null, Discord.User u = null)
        {
            var allQuotes = new SC2Quotes();
            var quote = new QuoteOfTheDay.Datas.Quote();

            if (parser.Parameters != null && parser.Parameters[0] == "-help")
                return ConvertSingleReturnToList(Res("QuoteCommandHelp"));

            if (parser.Parameters != null && parser.Parameters[0] == "-all")
            {
                var messages = allQuotes.FormatAuthors(allQuotes.AllAuthor());
                foreach (var msg in messages)
                    await u.SendMessage(msg);

                return ConvertSingleReturnToList(Res("QuoteCommandSendMP"));
            }

            if (parser.Parameters != null && parser.Parameters.Count() > 1 && parser.Parameters[1] == "-count")
            {
                var nb = allQuotes.CountQuotesAuthor(parser.Parameters[0]);
                return ConvertSingleReturnToList(
                    allQuotes.FormatNbQuote(parser.Parameters[0], nb));
            }

            if (parser.Parameters != null && parser.Parameters[0] != "-help")
            {
                var nb = allQuotes.CountQuotesAuthor(parser.Parameters[0]);
                if (nb > 0)
                    return ConvertSingleReturnToList(
                        allQuotes.FormatQuote(
                            allQuotes.RandomQuoteAuthor(parser.Parameters[0])));
                else
                {
                    quote = allQuotes.RandomQuote();
                    return ConvertSingleReturnToList(
                        allQuotes.FormatQuote(quote, string.Format(Res("QuoteCommandNotFound"), parser.Parameters[0])));
                }
            }

            quote = allQuotes.RandomQuote();
            return ConvertSingleReturnToList(allQuotes.FormatQuote(quote));
        }

        public static async Task<List<string>> AssignRace(string race, Discord.Server s = null, Discord.User u = null)
        {
            var zRole = s.FindRoles("Zerg", true).First();
            var tRole = s.FindRoles("Terran", true).First();
            var pRole = s.FindRoles("Protoss", true).First();
            var rRole = s.FindRoles("Random", true).First();

            switch (race)
            {
                case "zerg":
                    await Helpers.Discord.ClearAndAddRole(s, u, zRole);
                    return ConvertSingleReturnToList(Res("ZergQuote"));
                case "protoss":
                    await Helpers.Discord.ClearAndAddRole(s, u, pRole);
                    return ConvertSingleReturnToList(Res("ProtossQuote"));
                case "terran":
                    await Helpers.Discord.ClearAndAddRole(s, u, tRole);
                    return ConvertSingleReturnToList(Res("TerranQuote"));
                case "random":
                    await Helpers.Discord.ClearAndAddRole(s, u, rRole);
                    return ConvertSingleReturnToList(Res("RandomQuote"));
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

        private static List<string> ConvertSingleReturnToList(string msg)
        {
            var messToRtn = msg;

            try { messToRtn = msg.Substring(0, 1999); }
            catch { };

            return new List<string>() { messToRtn };
        }

        private static string Res(string key)
        {
            string rtn = _rm.GetString(key);
            return rtn.Replace("\\n", "\n");
        }
    }
}
