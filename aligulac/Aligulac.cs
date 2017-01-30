using AligulacSC2.Objects;
using RestConnect;
using System;
using System.Resources;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AligulacSC2
{
    public class Aligulac
    {
        static private string _KEY_ = string.Empty;

        public Aligulac(string APIKey)
        {
            _KEY_ = APIKey;
        }

        async public Task<SearchResult> Search(string searchStr)
        {
            return await RestClient.Search(searchStr);
        }

        static async public Task<Player> Player(string name)
        {
            var playerSR = await RestClient.Search(name);
            var id = playerSR.Players;

            if (id != null && id.Length > 0)
            {
                return await RestClient.GetPlayerByID(id[0].ID.ToString(), _KEY_);
            }
            return new Player();
        }

        static async public Task<Prediction> Predict(string plAName, string plBName, string BO = "3")
        {
            var plASR = await RestClient.Search(plAName);
            var plBSR = await RestClient.Search(plBName);
            var plA = plASR.Players;
            var plB = plBSR.Players;

            if (plA != null && plB != null && plA.Length > 0 && plB.Length > 0)
            {
                return await RestClient.GetPrediction(plA[0].ID.ToString(), plB[0].ID.ToString(), BO, _KEY_);
            }

            return new Prediction() { Error = $"Désolé, je ne peux pas faire de prédiction avec des nonames {Properties.Resources.Kappa_Emoji}" };
        }

        static async public Task<GenericResult<Player>> Top(int nb = 10)
        {
            return await RestClient.GetTopPlayers(nb.ToString(), _KEY_);
        }

        static async public Task<GenericResult<Period>> Balance()
        {
            return await RestClient.GetPeriods(_KEY_);
        }

        //** Format **//
        /*
        static public List<string> ShowSearchResult(SearchResult s)
        {
            return $"Events : {s.Events.Length }\tPlayers: {s.Players.Length}\tTeams: {s.Teams.Length}";
        }
        */
        static public List<string> ShowPlayerObject(Player pl)
        {
            if (pl.ID != null)
                return new List<string>() { $"{PlayerToString(pl)} \n {PlayerLiquipediaToString(pl)}" };
            else
                return new List<string>() { $"Désolé, je ne connais pas ce noname {Properties.Resources.Kappa_Emoji}" };
        }

        static public List<string> ShowPredictionObject(Prediction p)
        {
            List<string> result = new List<string>();
            var str = string.Empty;
            var temp = string.Empty;
            foreach (var pro in p.Outcomes.OrderByDescending(o => o.Prob))
            {
                temp = string.Empty;
                if (pro.ScoreA > pro.ScoreB)
                    temp = $"**{(pro.Prob * 100).ToString("F2")}%** \t{PlayerToString(p.PlayerA, true)} \t{pro.ScoreA} - {pro.ScoreB} \t{PlayerToString(p.PlayerB)} \n";
                else
                    temp = $"**{(pro.Prob * 100).ToString("F2")}%** \t{PlayerToString(p.PlayerB, true)} \t{pro.ScoreB} - {pro.ScoreA} \t{PlayerToString(p.PlayerA)} \n";
                if (str.Length + temp.Length >= 2000)
                {
                    result.Add(str);
                    str = string.Empty;
                }
                str += temp;
            }
            if (!string.IsNullOrEmpty(str))
            {
                str += $":globe_with_meridians: {p.URL}";
                result.Add(str);
            }
            return result;
        }

        static public List<string> ShowTopObject(GenericResult<Player> ps)
        {
            var i = 0;
            List<string> rtnArrStr = new List<string>();
            var rtnStr = string.Empty;
            var tempStr = string.Empty;
            foreach (Player p in ps.Results)
            {
                i++;
                tempStr = $"{Position(i, ps.Results.Count())}\t{PlayerToString(p, false, false)}\n";

                if ((tempStr.Length + rtnStr.Length) >= 2000)
                {
                    rtnArrStr.Add(rtnStr);
                    rtnStr = string.Empty;
                }

                rtnStr += tempStr;
            }
            if (!string.IsNullOrEmpty(rtnStr)) rtnArrStr.Add(rtnStr);

            return rtnArrStr;
        }

        static public List<string> ShowPeriodObject(GenericResult<Period> ps)
        {
            List<string> rtnArrStr = new List<string>();
            string rtnStr = string.Empty;
            foreach (Period p in ps.Results)
            {
                var tempStr = $"Du **{p.StartDate.ToShortDateString()}** au **{p.EndDate.ToShortDateString()}** :\n" +
                    $"{RacePeriod(p.Leading, true, false)}  {RacePeriod(p.MidRace, false, false)}  {RacePeriod(p.Lagging, false, true)}\n\n";

                if ((tempStr.Length + rtnStr.Length) >= 2000)
                {
                    rtnArrStr.Add(rtnStr);
                    rtnStr = string.Empty;
                }

                rtnStr += tempStr;
            }
            if (!string.IsNullOrEmpty(rtnStr)) rtnArrStr.Add(rtnStr);

            return rtnArrStr;
        }

        //** Micro-Format **//

        static private string PlayerToString(Player pl, bool BoldName = false, bool ShowTeam = true)
        {
            var name = pl.Tag;

            if (BoldName)
                name = $"**{pl.Tag}**";

            return $":flag_{pl.Country.ToLower()}: {Race(pl.Race)} {PlayerCurrentTeamToString(pl, ShowTeam)}{name}";
        }

        static private string PlayerCurrentTeamToString(Player pl, bool showTeam)
        {
            if (showTeam && pl.CurrentTeams != null && pl.CurrentTeams.Length > 0)
            {
                Team t = pl.CurrentTeams[0].Team;
                return $"[{t.Name}]";
            }

            return "";
        }

        static private string PlayerLiquipediaToString(Player pl)
        {
            if (pl.ID_TLPD != null)
            {
                return $":globe_with_meridians: http://wiki.teamliquid.net/starcraft2/{pl.Tag}";
            }

            return "";
        }

        static private string Race(string r)
        {
            switch (r)
            {
                case "Z": return Properties.Resources.Zerg_Emoji;
                case "P": return Properties.Resources.Protoss_Emoji;
                case "T": return Properties.Resources.Terran_Emoji;
                case "R": return Properties.Resources.Random_Emoji;
                default: return $"({r})";
            }
        }

        static private string Position(int p, int nbMax = 10)
        {
            if (nbMax > 10) return $"({p})";

            switch (p)
            {
                case 1: return ":one:";
                case 2: return ":two:";
                case 3: return ":three:";
                case 4: return ":four:";
                case 5: return ":five:";
                case 6: return ":six:";
                case 7: return ":seven:";
                case 8: return ":eight:";
                case 9: return ":nine:";
                case 10: return ":keycap_ten:";
                default: return ":zero:";
            }
        }

        static private string RacePeriod(LeadingRace r, bool isLeading, bool isLagging)
        {
            string icon;

            if (isLeading)
                icon = ":small_red_triangle:";
            else if (isLagging)
                icon = ":small_red_triangle_down:";
            else icon = ":small_blue_diamond:";

            if (r.isWeak || r.isOP)
                icon += ":bangbang:";

            return $"{icon}{Race(r.Race)} {r.DifferencePourcent}%";
        }

        //** Easter Egg **//

        static public async Task<List<string>> CrunchyRules(string name, int BO)
        {
            var p = await Player(name);
            var nbGames = Math.Ceiling(decimal.Divide(BO, 2));
            return new List<string>() { $"**100%** \t:flag_fr: (Z) **Crunchy** \t **{nbGames}** - 0 \t{PlayerToString(p)} \n" };
        }

    }
}
