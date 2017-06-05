using Crawlers.Objects.Liquipedia;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crawlers
{
    public class Liquipedia
    {
        public async Task<List<Transfert>> GetTransfert()
        {
            string url = "http://wiki.teamliquid.net/starcraft2/Main_Page";
            var parser = new Utils.Liquipedia.Parser();

            return parser.ParsingTransfers(await parser.GetDocumentHTML(url));
        }

        public async Task<List<Event>> GetCalendarEvents(DateTime dt, Period p)
        {
            string url = $"http://www.teamliquid.net/calendar/?view=week&year={ dt.Year }&month={ dt.Month }&day={ dt.Day }&game=1";

            var parser = new Utils.Liquipedia.Parser();
            if (p == Period.Day)
                return parser.ParsingEventsDay(await parser.GetDocumentHTML(url), dt);
            else
                return parser.ParsingEventsWeek(await parser.GetDocumentHTML(url), dt);
        }

        public async Task<List<Event>> GetLiveEvents()
        {
            string url = $"http://www.teamliquid.net/";

            var parser = new Utils.Liquipedia.Parser();

            return parser.ParsingLiveEvents(await parser.GetDocumentHTML(url));
        }
    }
}
