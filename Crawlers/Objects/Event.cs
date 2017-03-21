using System;
using System.Collections.Generic;

namespace Crawlers.Objects
{
    public class Event
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public DateTime Date { get; set; }
        public StatusEnum Status { get; set; }
        public List<Match> Matches { get; set; }
        public Dictionary<string,string> Links { get; set; }
    }

    public class Match
    {
        public string PlayerA { get; set; }
        public string PlayerB { get; set; }
        public StatusEnum Status { get; set; }
    }
    
    public enum StatusEnum
    {
        Pending,
        Live,
        Done
    }
}
