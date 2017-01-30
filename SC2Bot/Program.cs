using System;
using AligulacSC2;
using Discord;
using System.Threading.Tasks;

namespace SC2Bot
{
    class Program
    {
        private static string _tokenBot;
        private static ulong _SuperUserId = 0;
        private static DiscordClient _client;
        private static Aligulac _aligu;

        static void Main(string[] args)
        {
            Init();

            _client = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
            });

            _client.Log.Message += (s, e) => Console.WriteLine($"[{e.Severity}] {e.Source}: {e.Message}");

            _client.MessageReceived += async (s, e) =>
            {
                try
                {
                    var server = e.Server;
                    var msg = e.Message;
                    var usr = msg.User;

                    if (msg.Text == "" || msg.IsAuthor) return;

                    if (msg.Text.ToLower() == "stop") await DisconnectClient(usr);
                    else await SendMessage(msg.Text, server, usr, e.Channel);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] {ex.Source}: {ex.Message}");
                }
            };

            _client.UserJoined += async (s, e) =>
            {
                await e.User.SendMessage(Properties.Resources.WelcomeStr);
            };

            _client.ExecuteAndWait(async () =>
            {
                await _client.Connect(_tokenBot, TokenType.Bot);
            });
        }

        static void Init()
        {
            _tokenBot = Environment.GetEnvironmentVariable("APIBotSC2");
            _aligu = new Aligulac(Environment.GetEnvironmentVariable("APIBotAligulacSC2"));
            ulong.TryParse(Environment.GetEnvironmentVariable("SuperUserIdDiscord"), out _SuperUserId);
        }

        static async Task SendMessage(string msg, Server s, User u, Channel c)
        {
            var messages = await Commands.SelectCommands(msg, s, u, c.IsPrivate);
            if (messages != null)
            {
                foreach (var message in messages)
                    await c.SendMessage(message);
            }
        }

        static async Task DisconnectClient(User usr)
        {
            if (Helpers.Discord.IsAdmin(usr, true, _SuperUserId))
            {
                _client.Log.Info("DISCONNECTED", $"Authorized by : {usr.Name}");
                await _client.Disconnect();
            }
        }
    }
}
