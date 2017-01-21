using Newtonsoft.Json;

namespace AligulacSC2.Objects
{
    public class Team
    { 
        public bool Active { get; set; }
        public string[] Aliases { get; set; }
        [JsonProperty(PropertyName = "current_nonplayers")]
        public Player[] CurrentNonPlayers { get; set; }
        [JsonProperty(PropertyName = "current_players")]
        public Player[] CurrentPlayers { get; set; }
        public string Disbanded { get; set; }
        public string Founded { get; set; }
        public string Homepage { get; set; }
        public int ID { get; set; }
        [JsonProperty(PropertyName = "lp_name")]
        public string NameLiquipedia { get; set; }
        public float MeanRating { get; set; }
        public string Name { get; set; }
        [JsonProperty(PropertyName = "past_players")]
        public Player[] PastPlayers { get; set; }
        [JsonProperty(PropertyName = "resource_uri")]
        public string ResourceUri { get; set; }
        [JsonProperty(PropertyName = "scoreak")]
        public float ScoreAllKill { get; set; }
        [JsonProperty(PropertyName = "scorepl")]
        public float ScoreProleague { get; set; }
        public string ShortName { get; set; }
    }
}
