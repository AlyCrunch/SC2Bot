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
        private string _KEY_ = string.Empty;

        public Aligulac(string APIKey)
        {
            _KEY_ = APIKey;
        }

        async public Task<SearchResult> Search(string searchStr)
        {
            return await RestClient.Search(searchStr);
        }

        async public Task<Player> Player(string name)
        {
            var playerSR = await RestClient.Search(name);
            var id = playerSR.Players;

            if (id != null && id.Length > 0)
            {
                return await RestClient.GetPlayerByID(id[0].ID.ToString(), _KEY_);
            }
            return new Player();
        }

        async public Task<Prediction> Predict(string plAName, string plBName, string BO = "3")
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

        async public Task<GenericResult<Player>> Top(int nb = 10)
        {
            return await RestClient.GetTopPlayers(nb.ToString(), _KEY_);
        }

        async public Task<GenericResult<Period>> Balance(DateTime From, DateTime To, bool avg, bool op, bool wk, int limit = 0)
        {
            var listP = await RestClient.GetPeriods(_KEY_, limit);


            if (From != new DateTime())
                if (To != new DateTime())
                    listP.Results = FilterFromToPeriod(listP, From, To);
                else
                    listP.Results = FilterFromToPeriod(listP, From, DateTime.Now);

            if (op)
                listP.Results = OpPeriod(listP);

            if (wk)
                listP.Results = WeakPeriod(listP);

            if (avg)
                listP.Results = AveragePeriod(listP.Results);

            return listP;
        }

        async public Task<GenericResult<Period>> BalanceByMonth(DateTime From, DateTime To, int limit = 0)
        {
            var listP = await RestClient.GetPeriods(_KEY_, limit);

            if (From != new DateTime())
                if (To != new DateTime())
                    listP.Results = FilterFromToPeriod(listP, From, To);
                else
                    listP.Results = FilterFromToPeriod(listP, From, DateTime.Now);

            var groupesElements = listP.Results.GroupBy(p => p.StartDate.Month);


            return listP;
        }
        
        //** Balance **//

        private Period[] FilterFromToPeriod(GenericResult<Period> p, DateTime f, DateTime t)
        {
            var tempList = p.Results.Where(x => x.StartDate >= f && x.EndDate <= t).ToArray();
            return tempList;
        }

        private Period[] AveragePeriod(Period[] ps)
        {
            Period p = new Period();
            int l = ps.Length;
            string from = ps[0].Start;
            string to = ps[l - 1].End;
            float dP = 0;
            float dT = 0;
            float dZ = 0;

            foreach (Period tp in ps)
            {
                dP += tp.DominationP;
                dT += tp.DominationT;
                dZ += tp.DominationZ;
            }

            p.Start = from;
            p.End = to;
            p.DominationP = dP / l;
            p.DominationT = dT / l;
            p.DominationZ = dZ / l;

            return new Period[] { p };
        }

        private Period[] OpPeriod(GenericResult<Period> pt)
        {
            var ps = pt.Results;
            return ps.Where(x => x.Leading.isOP).ToArray();
        }

        private Period[] WeakPeriod(GenericResult<Period> pt)
        {
            var ps = pt.Results;
            return ps.Where(x => x.Lagging.isWeak).ToArray();
        }
        
    }
}
