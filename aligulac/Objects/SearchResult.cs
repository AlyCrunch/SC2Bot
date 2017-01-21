using Newtonsoft.Json;

namespace AligulacSC2.Objects
{
    public class SearchResult
    {
        [JsonProperty(PropertyName = "players")]
        public Player[] Players { get; set; }
        [JsonProperty(PropertyName = "teams")]
        public Team[] Teams { get; set; }
        [JsonProperty(PropertyName = "events")]
        public EventSearch[] Events { get; set; }
    }

    public class EventSearch
    {
        [JsonProperty(PropertyName = "fullname")]
        public string Fullname { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }
    }

}
