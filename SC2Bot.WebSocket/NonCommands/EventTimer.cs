using System;
using Discord;
using System.Timers;
using Discord.WebSocket;

namespace SC2Bot.WebSocket.NonCommands
{
    public class EventTimer
    {
        SocketTextChannel _channel;

        public void InitialiseAutoEvent(SocketTextChannel channel)
        {
            Console.WriteLine("### First Day Events ###");
            _channel = channel;
            ScheduleEvents(new object(), new EventArgs() as ElapsedEventArgs);
        }

        void ScheduleDayEvents()
        {
            Console.WriteLine("### creating Day+1 event ###");

            DateTime nowTime = DateTime.Now;
            DateTime scheduledTime = nowTime.AddDays(1).Date;
            
            double tickTime = (double)(scheduledTime - DateTime.Now).TotalMilliseconds;
            var t = new Timer(tickTime);
            t.Elapsed += new ElapsedEventHandler(ScheduleEvents);
            t.Start();
        }

        async void ScheduleEvents(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("### Get all events of the day ### \n");
            var events = await Commands.Liquipedia.GetEventOfTheDay();

            foreach(var ev in events)
            {
                if (ev.Date < DateTime.Now) continue;
                
                DateTime nowTime = DateTime.Now;
                DateTime scheduledTime = ev.Date;
                double tickTime = (double)(scheduledTime - DateTime.Now).TotalMilliseconds;
                
                Console.WriteLine($"--- Events created : {ev.Title} in {tickTime}ms ({ev.Date.ToShortTimeString()}) --- \n");

                var t = new Timer(tickTime);
                t.Elapsed += new ElapsedEventHandler(ScheduleEvent);
                t.Start();
            }

            Console.WriteLine("### Day Events Finished ### \n\n");
            ScheduleDayEvents();
        }

        async void ScheduleEvent(object sender, ElapsedEventArgs e)
        {
            foreach(var l in Commands.Liquipedia.GetLiveEventsExt())
            {
                await _channel.SendMessageAsync("", false, l);
            }
        }
    }
}
