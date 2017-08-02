using AligulacSC2.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AligulacSC2
{
    static public class Format
    {
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

        static public List<string> ShowPeriodObject(GenericResult<Period> ps, bool isLL, bool isOp, bool isAvg)
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


            if (!string.IsNullOrEmpty(rtnStr))
            {
                var bresume = string.Empty;
                if (isLL)
                    bresume = BalanceResume(ps.Results, isOp);

                if (isLL && isAvg)
                {
                    rtnArrStr.Clear();
                    rtnStr = bresume;
                    rtnArrStr.Add(rtnStr);
                }
                else
                {
                    if (bresume.Length + rtnStr.Length > 2000)
                    {
                        rtnArrStr.Add(rtnStr);
                        rtnStr = bresume;
                    }
                    else
                        rtnStr += bresume;

                    rtnArrStr.Add(rtnStr);
                }

            }

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

            if (isLeading) icon = ":small_red_triangle:";
            else if (isLagging) icon = ":small_red_triangle_down:";
            else icon = ":small_blue_diamond:";

            if (r.isOP) icon = ":bangbang:";
            if (r.isWeak) icon = ":interrobang:";

            return $"{icon}{Race(r.Race)} {r.DifferencePourcent}%";
        }

        static private string BalanceResume(Period[] ps, bool isOp)
        {
            var Start = ps[0].StartDate;
            var End = ps[ps.Length - 1].EndDate;

            var nbP = 0;
            var nbT = 0;
            var nbZ = 0;
            var prefix = (isOp) ? "Ont été **OP**" : "Ont été **weak**";
            nbP = (isOp) ? ps.Count(x => x.Leading.isOP && x.Leading.Race == "P") :
                ps.Count(x => x.Lagging.isWeak && x.Lagging.Race == "P");
            nbT = (isOp) ? ps.Count(x => x.Leading.isOP && x.Leading.Race == "T") :
                ps.Count(x => x.Lagging.isWeak && x.Lagging.Race == "T");
            nbZ = (isOp) ? ps.Count(x => x.Leading.isOP && x.Leading.Race == "Z") :
                ps.Count(x => x.Lagging.isWeak && x.Lagging.Race == "Z");

            return $"{prefix} du **{Start.ToShortDateString()}** au **{End.ToShortDateString()}** :\n{Race("P")} {nbP} fois, {Race("T")} {nbT} fois, {Race("Z")} {nbZ} fois";
        }
    }
}
