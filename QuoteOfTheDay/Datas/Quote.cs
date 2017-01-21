using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoteOfTheDay.Datas
{
    public class Quote
    {
        [System.Xml.Serialization.XmlElement("Date")]
        public int NbDaysDate { get; set; }
        public DateTime Date
        {
            get
            {
                DateTime Date = new DateTime(1900,1,1);
                Date = Date.AddDays(NbDaysDate);
                return Date;
            }
        }
        public string Author { get; set; }
        public string Reference { get; set; }
        public string Text { get; set; }
    }
}
