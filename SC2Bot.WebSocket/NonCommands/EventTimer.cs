using System;
using System.Timers;
using Discord.WebSocket;
using System.Collections.Generic;

namespace SC2Bot.WebSocket.NonCommands
{
    public class EventTimer
    {
        SocketTextChannel _channel;
        Timer _t = null;
        Dictionary<string, Timer> _eT;

        public void InitialiseAutoEvent(SocketTextChannel channel)
        {
            _channel = channel;
            ScheduleEvents(new object(), new EventArgs() as ElapsedEventArgs);
        }

        void ScheduleDayEvents()
        {
            DateTime nowTime = DateTime.Now;
            DateTime scheduledTime = nowTime.AddDays(1).Date;
            double tickTime = (double)(scheduledTime - DateTime.Now).TotalMilliseconds;
            
            _t = new Timer(tickTime);
            _t.Elapsed += new ElapsedEventHandler(ScheduleEvents);
            _t.Start();
        }

        async void ScheduleEvents(object sender, ElapsedEventArgs e)
        {
            var events = await Commands.Liquipedia.GetEventOfTheDay();

            if (_t != null)
                _t.Stop();

            _eT = new Dictionary<string, Timer>();

            foreach (var ev in events)
            {
                if (ev.Date < DateTime.Now) continue;
                if (_eT.ContainsKey(ev.Date.ToShortTimeString())) continue;

                DateTime nowTime = DateTime.Now;
                DateTime scheduledTime = ev.Date;
                double tickTime = (scheduledTime - DateTime.Now).TotalMilliseconds;
                
                string key = ev.Date.ToShortTimeString();
                
                _eT.Add(key, new Timer(tickTime));
                _eT[key].Elapsed += new ElapsedEventHandler(ScheduleEvent);
                _eT[key].Start();
                Console.WriteLine($"Event créé : {ev.Title} - {ev.Date.ToShortDateString()}-{ev.Date.ToShortTimeString()} ({tickTime / 1000}s)");
            }

            ScheduleDayEvents();
        }

        async void ScheduleEvent(object sender, ElapsedEventArgs e)
        {
            string key = e.SignalTime.ToShortTimeString();
            _eT[key].Stop();
            
            foreach (var l in Commands.Liquipedia.GetLiveEventsExt())
            {
                await _channel.SendMessageAsync("", false, l);
            }
            _eT.Remove(key);
        }
    }
}
