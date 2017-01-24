using Newtonsoft.Json;
using System.Collections.Generic;

namespace AligulacSC2.Objects
{
    public class GenericResult<T>
    {
        [JsonProperty(PropertyName = "meta")]
        public Meta Metadata { get; set; }
        [JsonProperty(PropertyName = "objects")]
        public T[] Results { get; set; }
    }        
}
