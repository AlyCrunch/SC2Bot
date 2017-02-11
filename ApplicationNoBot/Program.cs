using System;
using Crawlers;
using System.Threading.Tasks;

namespace ApplicationNoBot
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        static async Task MainAsync(string[] args)
        {
            try
            {
                var l = new Liquipedia();
                var lt = await l.GetTransfert();
                foreach (var t in lt)
                {
                    Console.WriteLine($"{t.Date.ToShortDateString()} {t.Players[0].Name} {t.OldTeam.Name} => {t.NewTeam.Name}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Source}: {ex.Message}\n{ex.InnerException}");
            }
            Console.ReadLine();
        }

    }
}
