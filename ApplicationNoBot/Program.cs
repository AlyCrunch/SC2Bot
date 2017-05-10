using System;
using Crawlers;
using System.Threading.Tasks;
using System.Collections.Generic;

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

                /*
                List<Crawlers.Objects.Event> le = await l.GetEvents(DateTime.Now, Crawlers.Objects.Period.Day);

                Console.WriteLine("------ Events ------");
                foreach (var e in le)
                {
                    Console.WriteLine($"{e.Date.ToShortDateString()} - {e.Title}");
                }
                */
                List<Crawlers.Objects.Transfert> lt = await l.GetTransfert();

                Console.WriteLine("\n---- Transferts ----");
                foreach (var t in lt)
                {
                    Console.WriteLine($"{t.Date.ToShortDateString()} {t.Players[0].Name} {t.OldTeam.Name} => {t.NewTeam.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Source}: {ex.Message}\n{ex.InnerException}");
            }
            Console.ReadLine();
        }

    }
}
