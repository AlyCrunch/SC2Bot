using AligulacSC2;
using Crawlers;
using Discord;
using QuoteOfTheDay;
using SC2Bot.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SC2Bot
{
    public static class Commands
    {
        private static Infos _i = new Infos();
        private static System.Resources.ResourceManager _rm = Properties.Resources.ResourceManager;

        public static async Task<List<string>> SelectCommands(MessageEventArgs e, DiscordClient c)
        {
            try
            {
                var q = e.Message.Text;
                var u = e.User;

                if (IsRaceAssignement(e))
                    return await AssignRace(q.ToLower(), e.Server, e.User);

                if (IsDisconnectedCommand(e, c))
                {
                    c.Log.Info("DISCONNECTED", $"Authorized by : {u.Name}");
                    await c.Disconnect();
                    return new List<string>();
                }
                if (IsAskForRace(e, c))
                {
                    var usrs = Helpers.Discord.GetUserNoRole(c.Servers.First(s => s.Name == _i.ServerName));
                    await SendMessageToAll(Res("RappelRaceStr"), usrs);
                }

                if (q[0] != '!') return new List<string>();

                var parser = new Parser(q);

                if (parser.Method == null) return new List<string>();

                switch (parser.Method.ToLower())
                {
                    case "quote": return await Quote(parser, e.Server, e.User);
                    case "top": return await Top(parser, e.Server, e.User);
                    //case "player": return await Player(parser, e.Server, e.User);
                    case "predict": return await Predict(parser, e.Server, e.User);
                    case "balance": return await Balance(parser, e.Server, e.User);
                    case "transfer": return await Transfers(parser, e.Server, e.User);
                    case "help": return Help();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Source}: {ex.Message}\n{ex.InnerException}");
            }
            return new List<string>();
        }

        public static List<string> Help()
        {
            return ConvertSingleReturnToList(Res("HelpCommand"));
        }

        public static async Task<List<string>> Player(Parser parser, Server s = null, User u = null)
        {
            if (parser.Parameters == null)
                return ConvertSingleReturnToList(Res("PlayerCommandMissing"));

            if (parser.Parameters[0] == "-help")
                return ConvertSingleReturnToList(Res("PlayerCommandHelp"));

            return Aligulac.ShowPlayerObject(await _i.Aligulac.Player(parser.Parameters[0]));
        }

        public static async Task<List<string>> Balance(Parser parser, Server s = null, User u = null)
        {
            var err = string.Empty;
            DateTime fromDate = new DateTime(), toDate = new DateTime();
            var from = false;
            var average = false;
            var op = false;
            var weak = false;
            var limit = 10;

            if (parser.Parameters != null && parser.Parameters.Length > 0 && parser.Parameters[0] == "-help")
                return ConvertSingleReturnToList(Res("BalanceCommandHelp"));

            if (parser.Parameters.Count(x => x.ToLower() == "-from") > 0)
            {
                int i = Array.FindIndex(parser.Parameters, x => x.ToLower() == "-from");
                DateTime.TryParse(parser.Parameters[i + 1], out fromDate);
                if (fromDate == new DateTime())
                    err = "la date du champ **-from** n'est pas au bon format :\n```!balance -from 15/02/2015```\n\n";
                else
                {
                    from = true;
                    if (parser.Parameters.Count(x => x.ToLower() == "-to") > 0)
                    {
                        int j = Array.FindIndex(parser.Parameters, x => x.ToLower() == "-to");
                        DateTime.TryParse(parser.Parameters[j + 1], out toDate);
                        if (toDate == null)
                            err += "la date du champ **-to** n'est pas au bon format :\n```!balance -from 02/06/2015```\n\n";
                        else
                        {
                            if (fromDate > toDate)
                                err += "la date de fin **-to** doit se trouver après la date de début **-from** \n\n";
                            else
                                limit = 0;
                        }
                    }
                    else
                    {
                        toDate = DateTime.Now;
                        limit = 0;
                    }
                }
            }

            if (!from)
                if (Helpers.Discord.IsAdmin(u))
                    limit = 0;

            if (parser.Parameters.Count(x => x.ToLower() == "-avg") > 0)
                average = true;

            if (parser.Parameters.Count(x => x.ToLower() == "-op") > 0)
                op = true;

            if (parser.Parameters.Count(x => x.ToLower() == "-weak") > 0)
                weak = true;

            if (op && weak)
                err += "Les deux paramètres ne peuvent pas être utilisé en même temps.";

            if (string.IsNullOrEmpty(err))
            {
                if ((op || weak) && average)
                    return Aligulac.ShowPeriodObject(
                        await _i.Aligulac.Balance(fromDate, toDate, false, op, weak, limit),
                            (op || weak), op, average);

                return Aligulac.ShowPeriodObject(
                    await _i.Aligulac.Balance(fromDate, toDate, average, op, weak, limit),
                        (op || weak), op, average);

            }
            else
                return ConvertSingleReturnToList(err);
        }

        public static async Task<List<string>> Predict(Parser parser, Server s = null, User u = null)
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
                        return await _i.Aligulac.CrunchyRules(parser.Parameters[1], BOnb);
                    else
                        return await _i.Aligulac.CrunchyRules(parser.Parameters[0], BOnb);
                }
            }
            #endregion

            int nbBO = 3;

            if (parser.Parameters.Length == 3)
                int.TryParse(parser.Parameters[2], out nbBO);

            var retPred = await _i.Aligulac.Predict(parser.Parameters[0], parser.Parameters[1], nbBO.ToString());

            if (string.IsNullOrEmpty(retPred.Error))
                return Aligulac.ShowPredictionObject(retPred);
            else
                return ConvertSingleReturnToList(retPred.Error);
        }

        public static async Task<List<string>> Top(Parser parser, Server s = null, User u = null)
        {
            if (parser.Parameters != null
                && parser.Parameters.Length == 1
                && parser.Parameters[0] == "-help")
                return ConvertSingleReturnToList(Res("TopCommandHelp"));

            int topNb = 10;
            if (parser.Parameters != null && Helpers.Discord.IsAdmin(u))
                int.TryParse(parser.Parameters[0], out topNb);
            return Aligulac.ShowTopObject(await _i.Aligulac.Top(topNb));
        }

        public static async Task<List<string>> Quote(Parser parser, Server s = null, User u = null)
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

        public static async Task<List<string>> AssignRace(string race, Server s = null, User u = null)
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

        public static async Task<List<string>> Transfers(Parser parser, Server s = null, User u = null)
        {
            if (parser.Parameters != null && parser.Parameters[0] == "-help")
                return ConvertSingleReturnToList(Res("TransfersHelp"));

            var TL = await new Liquipedia().GetTransfert();
            string alltransfers = string.Empty;

            foreach (var transfer in TL)
            {
                alltransfers += $"{transfer.Date.ToShortDateString()} : **{string.Join(", ", transfer.Players.Select(x => x.Name))}** \t{transfer.OldTeam.Name} \t⇒ \t{transfer.NewTeam.Name}" + "\n";
            }

            return ConvertSingleReturnToList(alltransfers);
        }

        private static async Task SendMessageToAll(string Message, List<User> Us)
        {
            foreach (var u in Us)
                await u.SendMessage(Message);
        }

        #region Filters
        private static bool IsAskForRace(MessageEventArgs e, DiscordClient c)
        {
            if (!e.Channel.IsPrivate) return false;
            if (e.Message.Text.ToLower() != "race") return false;
            if (Helpers.Discord.IsAdminServer(c.Servers.ToList(), _i.ServerName, e.User))
                return true;

            return false;
        }

        private static bool IsRaceAssignement(MessageEventArgs e)
        {
            var q = e.Message.Text;
            var pm = e.Channel.IsPrivate;

            if (pm &&
                (q == "zerg" ||
                q == "protoss" ||
                q == "terran" ||
                q == "random"))
                return true;
            return false;
        }

        private static bool IsDisconnectedCommand(MessageEventArgs e, DiscordClient c)
        {
            if (e.Message.Text.ToLower() == "stop")
            {
                if (Helpers.Discord.IsAdminGetFirstServer(e.User, c))
                    return true;
                return false;
            }
            else
            {
                var qS = e.Message.Text.Split(':');
                if (qS.Count() != 2) return false;
                if (qS[0] != "stop") return false;
                if (!e.Channel.IsPrivate) return false;

                if (Helpers.Discord.IsAdminServer(c.Servers.ToList(), qS[1], e.User))
                    return true;
            }
            return false;
        }
        #endregion

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