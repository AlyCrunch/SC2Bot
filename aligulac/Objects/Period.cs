using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AligulacSC2.Objects
{
    public class Period
    {
        public int id { get; set; }
        public float dom_p { get; set; }
        public float dom_t { get; set; }
        public float dom_z { get; set; }
        public string end { get; set; }
        public string start { get; set; }
        public bool needs_recompute { get; set; }
        public int num_games { get; set; }
        public int num_newplayers { get; set; }
        public int num_retplayers { get; set; }
        public string resource_uri { get; set; }
    }
}
