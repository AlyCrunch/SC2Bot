using System;
using System.Collections.Generic;

namespace Crawlers.Objects
{
    public class Transfert
    {
        public DateTime Date { get; set; }
        public List<Player> Players { get; set; }
        public Team OldTeam { get; set; }
        public Team NewTeam { get; set; }
    }

    public class Player
    {
        public string RaceLong { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
    }

    public class Team
    {
        public string Name { get; set; }
        public string ImageURL { get; set; }
    }
}
