using Newtonsoft.Json;

namespace AligulacSC2.Objects
{
    public class Rating
    {
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "resource_uri")]
        public string ResourceURI { get; set; }

        [JsonProperty(PropertyName = "player")]
        public Player Player { get; set; }

        [JsonProperty(PropertyName = "decay")]
        public int Decay { get; set; }

        [JsonProperty(PropertyName = "period")]
        public string Period { get; set; }

        [JsonProperty(PropertyName = "domination")]
        public float Domination { get; set; }

        [JsonProperty(PropertyName = "bf_dev")]
        public float SmoothRD { get; set; }
        [JsonProperty(PropertyName = "bf_dev_vp")]
        public float SmoothRD_vP { get; set; }
        [JsonProperty(PropertyName = "bf_dev_vt")]
        public float SmoothRD_vT { get; set; }
        [JsonProperty(PropertyName = "bf_dev_vz")]
        public float SmoothRD_vZ { get; set; }

        [JsonProperty(PropertyName = "bf_rating")]
        public float SmoothRating { get; set; }
        [JsonProperty(PropertyName = "bf_rating_vp")]
        public float SmoothRating_vP { get; set; }
        [JsonProperty(PropertyName = "bf_rating_vt")]
        public float SmoothRating_vT { get; set; }
        [JsonProperty(PropertyName = "bf_rating_vz")]
        public float SmoothRating_vZ { get; set; }

        [JsonProperty(PropertyName = "comp_rat")]
        public float RatingPeriod { get; set; }
        [JsonProperty(PropertyName = "comp_rat_vp")]
        public float RatingPeriod_vP { get; set; }
        [JsonProperty(PropertyName = "comp_rat_vt")]
        public float RatingPeriod_vT { get; set; }
        [JsonProperty(PropertyName = "comp_rat_vz")]
        public float RatingPeriod_vZ { get; set; }

        [JsonProperty(PropertyName = "dev")]
        public float RD { get; set; }
        [JsonProperty(PropertyName = "dev_vp")]
        public float RD_vP { get; set; }
        [JsonProperty(PropertyName = "dev_vt")]
        public float RD_vT { get; set; }
        [JsonProperty(PropertyName = "dev_vz")]
        public float RD_vZ { get; set; }

        [JsonProperty(PropertyName = "position")]
        public int Position { get; set; }
        [JsonProperty(PropertyName = "position_vp")]
        public int Position_vP { get; set; }
        [JsonProperty(PropertyName = "position_vt")]
        public int Position_vT { get; set; }
        [JsonProperty(PropertyName = "position_vz")]
        public int Position_vZ { get; set; }

        [JsonProperty(PropertyName = "prev")]
        public string PreviousRating { get; set; }
        [JsonProperty(PropertyName = "rating")]
        public float CurrentRating { get; set; }
        [JsonProperty(PropertyName = "rating_vp")]
        public float Rating_vP { get; set; }
        [JsonProperty(PropertyName = "rating_vt")]
        public float Rating_vT { get; set; }
        [JsonProperty(PropertyName = "rating_vz")]
        public float Rating_vZ { get; set; }

        [JsonProperty(PropertyName = "tot_vp")]
        public float Total_vP { get; set; }
        [JsonProperty(PropertyName = "tot_vt")]
        public float Total_vT { get; set; }
        [JsonProperty(PropertyName = "tot_vz")]
        public float Total_vZ { get; set; }
    }
}
