using Crawlers.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crawlers
{
    public class Liquipedia
    {
        static private string _WEEK = "week";
        static private string _MONTH = "month";

        public async Task<List<Transfert>> GetTransfert()
        {
            string url = "http://wiki.teamliquid.net/starcraft2/Main_Page";
            var parser = new Utils.Parser();

            return parser.ParsingTransfers(await parser.GetDocumentHTML(url));
        }

        public async Task<List<Event>> GetEvents(DateTime dt, Period p)
        {
            string period = "";
            if (p == Period.Month)
                period = _MONTH;
            else
                period = _WEEK;

            string url = $"http://www.teamliquid.net/calendar/?view={ period }&year={ dt.Year }&month={ dt.Month }&day={ dt.Day }&game=1";

            var parser = new Utils.Parser();

            return parser.ParsingEvents(await parser.GetDocumentHTML(url),p);
        }
    }
}
