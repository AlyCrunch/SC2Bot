using Newtonsoft.Json;

namespace AligulacSC2.Objects
{
    public class Prediction
    {
        public Outcome[] Outcomes { get; set; }
        [JsonProperty(PropertyName = "pla")]
        public Player PlayerA { get; set; }
        [JsonProperty(PropertyName = "plb")]
        public Player PlayerB { get; set; }
        public float ProbA { get; set; }
        public float ProbB { get; set; }
        [JsonProperty(PropertyName = "resource_uri")]
        public string ResourceURI { get; set; }
        [JsonProperty(PropertyName = "rta")]
        public float RatingA { get; set; }
        [JsonProperty(PropertyName = "rtb")]
        public float RatingB { get; set; }
        [JsonProperty(PropertyName = "sca")]
        public int FinalScoreA { get; set; }
        [JsonProperty(PropertyName = "scb")]
        public int FinalScoreB { get; set; }
        public int BO { get; set; }
        public string Error { get; set; }
        public string URL
        {
            get
            {
                return $"http://aligulac.com/inference/match/?bo={BO}&ps={PlayerA.ID}%2C{PlayerB.ID}";
            }
        }
    }
        
    public class Outcome
    {
        public float Prob { get; set; }
        [JsonProperty(PropertyName = "sca")]
        public int ScoreA { get; set; }
        [JsonProperty(PropertyName = "scb")]
        public int ScoreB { get; set; }
    }

}
