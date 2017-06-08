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
                await Liquipedia();
                //await SpawningTools();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Source}: {ex.Message}\n{ex.InnerException}");
            }
            Console.ReadLine();
        }

        static async Task Liquipedia()
        {
            var lt = await new Liquipedia().GetCalendarEvents(DateTime.Now, Crawlers.Objects.Liquipedia.Period.Day);

            //var lt = await new Liquipedia().GetLiveEvents();

            Console.WriteLine("\n---- Events ----");
            foreach (var t in lt)
            {
                Console.WriteLine($"{t.Date.ToString()} {t.Title} {t.Subtitle}");
            }
        }

        static async Task SpawningTools()
        {
            await new SpawningTools().GetListBO("zvt/", "2", "", "r");
        }
    }
}
