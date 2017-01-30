using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AligulacSC2.Objects
{
    public class Period
    {
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "dom_p")]
        public float DominationP { get; set; }
        public int DominationPPercent => (int)((DominationP - 1) * 100);
        [JsonProperty(PropertyName = "dom_t")]
        public float DominationT { get; set; }
        public int DominationTPercent => (int)((DominationT - 1) * 100);
        [JsonProperty(PropertyName = "dom_z")]
        public float DominationZ { get; set; }
        public int DominationZPercent => (int)((DominationZ - 1) * 100);

        private LeadingRace _leading = null;
        public LeadingRace Leading
        {
            get
            {
                if (_leading == null)
                {
                    string race = GetLead();
                    switch (race)
                    {
                        case "Z": _leading = new LeadingRace(race, DominationZ); break;
                        case "T": _leading = new LeadingRace(race, DominationT); break;
                        case "P": _leading = new LeadingRace(race, DominationP); break;
                    }
                }
                return _leading;
            }
        }

        private LeadingRace _lagging = null;
        public LeadingRace Lagging
        {
            get
            {
                if (_lagging == null)
                {
                    string race = GetLag();
                    switch (race)
                    {
                        case "Z": _lagging = new LeadingRace(race, DominationZ); break;
                        case "T": _lagging = new LeadingRace(race, DominationT); break;
                        case "P": _lagging = new LeadingRace(race, DominationP); break;
                    }
                }
                return _lagging;
            }
        }

        private LeadingRace _midRace = null;
        public LeadingRace MidRace
        {
            get
            {
                if (_midRace == null)
                {
                    string race = GetMid();
                    switch (race)
                    {
                        case "Z": _midRace = new LeadingRace(race, DominationZ); break;
                        case "T": _midRace = new LeadingRace(race, DominationT); break;
                        case "P": _midRace = new LeadingRace(race, DominationP); break;
                    }
                }
                return _midRace;
            }
        }

        [JsonProperty(PropertyName = "start")]
        public string Start { get; set; }
        public DateTime StartDate => DateTime.ParseExact(Start, "yyyy-MM-dd", null);
        [JsonProperty(PropertyName = "end")]
        public string End { get; set; }
        public DateTime EndDate => DateTime.ParseExact(End, "yyyy-MM-dd", null);

        [JsonProperty(PropertyName = "num_games")]
        public int NumGames { get; set; }
        [JsonProperty(PropertyName = "num_newplayers")]
        public int NumNewPlayers { get; set; }
        [JsonProperty(PropertyName = "num_retplayers")]
        public int NumReturningPlayers { get; set; }

        [JsonProperty(PropertyName = "needs_recompute")]
        public bool NeedsRecompute { get; set; }
        [JsonProperty(PropertyName = "resource_uri")]
        public string ResourceURI { get; set; }

        private string GetLead()
        {
            if (DominationP > DominationT && DominationP > DominationZ)
                return "P";
            if (DominationZ > DominationT && DominationZ > DominationP)
                return "Z";
            return "T";
        }
        private string GetLag()
        {
            if (DominationP < DominationT && DominationP < DominationZ)
                return "P";
            if (DominationZ < DominationT && DominationZ < DominationP)
                return "Z";
            return "T";
        }
        private string GetMid()
        {
            string Le = GetLead();
            string La = GetLag();

            if (!(Le + La).Contains("Z"))
                return "Z";
            if (!(Le + La).Contains("P"))
                return "P";

            return "T";
        }
    }

    public class LeadingRace
    {
        public string Race { get; set; }
        public string RaceLong
        {
            get
            {
                switch (Race.ToUpper())
                {
                    case "Z": return "Zerg";
                    case "T": return "Terran";
                    case "P": return "Protoss";
                    default: return "Random";
                }
            }
        }
        public float Domination { get; set; }
        public int DominationPourcent => (int)(Domination * 100);
        public float Difference => Domination - 1;
        public int DifferencePourcent => (int)(Difference * 100);
        public bool isOP => Domination > 1.1;
        public bool isWeak => Domination < 0.9;

        public LeadingRace(string R, float dom)
        {
            Race = R;
            Domination = dom;
        }
    }
}