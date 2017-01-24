using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AligulacSC2.Objects
{
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
