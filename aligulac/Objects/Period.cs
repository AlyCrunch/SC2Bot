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
        public int DominationPPer => (int)(DominationP * 100);
        [JsonProperty(PropertyName = "dom_t")]
        public float DominationT { get; set; }
        public int DominationTPer => (int)(DominationT * 100);
        [JsonProperty(PropertyName = "dom_z")]
        public float DominationZ { get; set; }
        public int DominationZPer => (int)(DominationZ * 100);

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
    }
}
