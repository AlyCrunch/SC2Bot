using AligulacSC2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC2Bot.Helpers
{
    public class Infos
    {
        public string BotAPI { get; set; }
        public Aligulac Aligulac { get; set; }
        public string ServerName { get; set; }

        public Infos(string tokenDiscord = "APIBotSC2", string tokenAligulac = "APIBotAligulacSC2", string serverName = "SC2France")
        {
            BotAPI = Environment.GetEnvironmentVariable(tokenDiscord);
            Aligulac = new Aligulac(Environment.GetEnvironmentVariable(tokenAligulac));
            ServerName = serverName;
        }
    }
}
