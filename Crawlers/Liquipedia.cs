using Crawlers.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crawlers
{
    public class Liquipedia
    {
        public async Task<List<Transfert>> GetTransfert()
        {
            string url = "http://wiki.teamliquid.net/starcraft2/Main_Page";
            var parser = new Utils.Parser();

            return parser.ParsingTransfers(await parser.GetDocumentHTML(url));            
        }
    }
}
