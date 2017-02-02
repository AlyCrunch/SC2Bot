using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquipedia
{
    public class Liquipedia
    {
        public void test()
        {
            string url = "http://wiki.teamliquid.net/starcraft2/Main_Page";
            var parser = new Crawlers.Utils.Parser();
            parser.GetDocument(url);
        }
    }
}
