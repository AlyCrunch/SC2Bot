using Newtonsoft.Json;

namespace AligulacSC2.Objects
{
    public class Player
    {
        [JsonProperty(PropertyName = "aliases")]
        public string[] Aliases { get; set; }
        [JsonProperty(PropertyName = "birthday")]
        public string Birthday { get; set; }
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; } //code pays ISO-3166-1 alpha-2
        [JsonProperty(PropertyName = "current_rating")]
        public Rating CurrentRating { get; set; }
        [JsonProperty(PropertyName = "current_teams")]
        public TeamPlayer[] CurrentTeams { get; set; }
        [JsonProperty(PropertyName = "dom_end")]
        public string DominationEndURI { get; set; }
        [JsonProperty(PropertyName = "dom_start")]
        public string DominationStartURI { get; set; }
        [JsonProperty(PropertyName = "dom_val")]
        public float DominationValue { get; set; }
        [JsonProperty(PropertyName = "form")]
        public Form Form { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int? ID { get; set; }
        [JsonProperty(PropertyName = "lp_name")]
        public string NameLiquipedia { get; set; }
        [JsonProperty(PropertyName = "mcnum")]
        public int? MCNum { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "past_teams")]
        public TeamPlayer[] PastTeams { get; set; }
        [JsonProperty(PropertyName = "race")]
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
        [JsonProperty(PropertyName = "resource_uri")]
        public string ResourceURI { get; set; }
        [JsonProperty(PropertyName = "romanized_name")]
        public string RomanizedName { get; set; }
        [JsonProperty(PropertyName = "sc2e_id")]
        public int? IDSC2Earning { get; set; } // external ID sc2earnings.com 
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }
        [JsonProperty(PropertyName = "tlpd_db")]        
        public int? DB_TLPD { get; set; } // bit-flag value denoting which TLPD databases this player is in (1 = KR WoL, 2 = IN WoL, 4 = HotS, 8 = HotS beta, 16 = WoL beta)
        [JsonProperty(PropertyName = "tlpd_id")]
        public int? ID_TLPD { get; set; } // external TLPD ID
        [JsonProperty(PropertyName = "total_earnings")]
        public int? TotalEarnings { get; set; }
    }

    public class Form
    {
        public int[] P { get; set; }
        public int[] T { get; set; }
        public int[] Z { get; set; }
        public int[] total { get; set; }
    }

    public class TeamPlayer
    {
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }
        [JsonProperty(PropertyName = "playing")]
        public bool Playing { get; set; }
        [JsonProperty(PropertyName = "resource_uri")]
        public string ResourceURI { get; set; }
        [JsonProperty(PropertyName = "start")]
        public string Start { get; set; }
        [JsonProperty(PropertyName = "end")]
        public string End { get; set; }
        [JsonProperty(PropertyName = "team")]
        public Team Team { get; set; }
    }
}
