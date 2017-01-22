using System;
using AligulacSC2;
using Discord;
using System.Linq;

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

                    if (e.Channel.IsPrivate)
                    {
                        var zRole = _client.Servers.First().FindRoles("Zerg", true).First();
                        var tRole = _client.Servers.First().FindRoles("Terran", true).First();
                        var pRole = _client.Servers.First().FindRoles("Protoss", true).First();
                        var rRole = _client.Servers.First().FindRoles("Random", true).First();

                        switch (msg.Text.ToLower())
                        {
                            case "zerg":
                                await Helpers.Discord.ClearAndAddRole(server, usr, zRole);
                                await e.Channel.SendMessage("For the Swarm !");
                                break;
                            case "protoss":
                                await Helpers.Discord.ClearAndAddRole(server, usr, pRole);
                                await e.Channel.SendMessage("My life for Aiur !");
                                break;
                            case "terran":
                                await Helpers.Discord.ClearAndAddRole(server, usr, tRole);
                                await e.Channel.SendMessage("Kaboom Baby !");
                                break;
                            case "random":
                                await Helpers.Discord.ClearAndAddRole(server, usr, rRole);
                                await e.Channel.SendMessage("Pour les hommes !");
                                break;
                            case "stop":
                                if (Helpers.Discord.IsAdmin(usr, true, _SuperUserId))
                                {
                                    _client.Log.Info("MANUALLY DISCONNECTED", $"Authorized by : {usr.Name}");
                                    await _client.Disconnect();
                                }
                                break;
                            default:
                                var message = await Commands.SelectCommands(msg.Text, server, usr);
                                if (message == null) break;
                                await e.Channel.SendMessage(message);
                                break;
                        }
                    }
                    else
                    {
                        var message = await Commands.SelectCommands(msg.Text, server, usr);
                        if (message == null) return;
                        await e.Channel.SendMessage(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] {ex.Source}: {ex.Message}");
                }
            };

            _client.UserJoined += async (s, e) =>
            {
                await e.User.SendMessage("Bienvenue !\nQuelle race joues-tu ?\nécris : **zerg**, **terran**, **protoss** ou **random**");
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
    }
}
