using Newtonsoft.Json;

namespace AligulacSC2.Objects
{
    public class Players
    {
        [JsonProperty(PropertyName = "meta")]
        public Meta Metadata { get; set; }
        [JsonProperty(PropertyName = "objects")]
        public Player[] PlayersResult { get; set; }
    }

    public class Meta
    {
        [JsonProperty(PropertyName = "limit")]
        public int Limit { get; set; }
        [JsonProperty(PropertyName = "next")]
        public string NextURI { get; set; }
        [JsonProperty(PropertyName = "offset")]
        public int Offset { get; set; }
        [JsonProperty(PropertyName = "previous")]
        public object PreviousURI { get; set; }
        [JsonProperty(PropertyName = "total_count")]
        public int Total { get; set; }
    }
    
}
