using System;
using System.Collections.Generic;

namespace Crawlers.Objects.Liquipedia
{
    public class Event
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public DateTime Date { get; set; }
        public StatusEnum Status { get; set; }
        public List<Match> Matches { get; set; }
        public List<Stream> Streams { get; set; }
        public string Wikipedia { get; set; }
        public string Thread { get; set; }
    }

    public class Match
    {
        public string PlayerA { get; set; }
        public string PlayerB { get; set; }
        public StatusEnum Status { get; set; }
    }

    public class Stream
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Lang { get; set; }
    }
    
    public enum StatusEnum
    {
        Pending,
        Live,
        Done
    }
}
