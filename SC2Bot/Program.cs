using System;
using Discord;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Collections.Generic;

namespace SC2Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            var _client = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
            });
            var _info = new Helpers.Infos();

            _client.Log.Message += (s, e) => Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");

            var messages = Observable.FromEventPattern<MessageEventArgs>(
                h => _client.MessageReceived += h,
                h => _client.MessageReceived -= h);

            var validMessages = messages.Where(e => IsValidMessage(e.EventArgs));
            
            var commandTasks = validMessages.Select(
                    (s) =>
                    {
                        var tMsg = Task.Run(() => Commands.SelectCommands(s.EventArgs, _client));
                        return new Tuple<MessageEventArgs, List<string>>(s.EventArgs, tMsg.Result);
                    }
            );

            var sendingTasks = commandTasks.Select(
                    (s) =>
                    {
                        return Task.Run(() => SendMessage(s.Item1, s.Item2)).ToObservable();
                    }
            );

            var tasks = sendingTasks;
            var tasksSub = tasks.Subscribe();

            _client.UserJoined += async (s, e) =>
            {
                await e.User.SendMessage(Properties.Resources.WelcomeStr);
            };

            _client.ExecuteAndWait(async () =>
            {
                await _client.Connect(_info.BotAPI, TokenType.Bot);
            });
        }

        static private IObservable<bool> ErrorHandler(Exception ex)
        {
            Console.WriteLine($"[Error Handler] {ex.Source}: {ex.Message}");
            return Observable.Return(false);
        }
        
        static private bool IsValidMessage(MessageEventArgs e) => !string.IsNullOrEmpty(e.Message.Text) && !e.Message.IsAuthor;

        static async Task<bool> SendMessage(MessageEventArgs e, List<string> messages)
        {
            if (messages != null || messages.Count > 0)
            {
                foreach (var message in messages)
                    await e.Channel.SendMessage(message);
                return true;
            }
            return false;
        }
    }
}
