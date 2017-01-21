using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC2Bot.Helpers
{
    public class Parser
    {
        private string query;
        private string method;
        private string[] parameters;

        public string Method
        {
            get
            {
                return method;
            }
            private set
            {
                method = value;
            }
        }
        public string[] Parameters
        {
            get
            {
                return parameters;
            }
            set
            {
                parameters = value;
            }
        }

        public Parser() { }

        //public Parser(string query) { Parse(query); }

        public Parser(string query)
        {
            this.query = query;
            char[] separator = { ' ' };

            List<string> splitedWords = (query.Substring(1)).Split( separator, StringSplitOptions.RemoveEmptyEntries ).ToList();
                        if (splitedWords.Count() == 0) return;
            Method = splitedWords[0];

            splitedWords.RemoveAt(0);
                        if (splitedWords.Count() == 0) return;
            Parameters = splitedWords.ToArray();

            return;
        }

        public string GetQuery() { return query; }
    }
}
