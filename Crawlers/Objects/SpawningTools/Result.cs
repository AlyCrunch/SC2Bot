using Newtonsoft.Json;

namespace Crawlers.Objects.SpawningTools
{
    public class Result
    {
        [JsonProperty(PropertyName = "thumbsRatio")]
        public int Ratio { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "race")]
        public string Race { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "buildOrder")]
        public string BuildOrder { get; set; }
        [JsonProperty(PropertyName = "buildType")]
        public string BuildType { get; set; }
        [JsonProperty(PropertyName = "Analysis")]
        public string analysis { get; set; }
        [JsonProperty(PropertyName = "Creator")]
        public string creator { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }
        [JsonProperty(PropertyName = "views")]
        public int Views { get; set; }
        [JsonProperty(PropertyName = "matchup")]
        public string MatchUp { get; set; }
        [JsonProperty(PropertyName = "isComplete")]
        public bool IsComplete { get; set; }
    }

}
