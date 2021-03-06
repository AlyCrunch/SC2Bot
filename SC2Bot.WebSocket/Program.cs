﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SC2Bot.WebSocket
{
    public class Program
    {
        private CommandService commands;
        private DiscordSocketClient client;
        private NonCommands.EventTimer events = new NonCommands.EventTimer();

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();

            client.Log += Log;

            await InstallCommands();

            await client.LoginAsync(TokenType.Bot, Helpers.Infos.BotAPI);
            await client.StartAsync();
            
            client.Ready += () =>
            {
                var guild = Helpers.GeneralHelper.First(client.Guilds);
                var channel = guild.DefaultChannel;

                Console.WriteLine($"Connecté sur {guild.Name}, le channel par défaut est {channel.Name}");
                client.SetGameAsync("écrivez !cmd");

                events.InitialiseAutoEvent(channel);

                return Task.CompletedTask;
            };

            await Task.Delay(-1);            
        }

        public async Task InstallCommands()
        {
            client.MessageReceived += HandleCommand;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommand(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            if (message.Author.IsBot) return;

            int argPos = 0;
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos)))
                return;

            var context = new CommandContext(client, message);
            var result = await commands.ExecuteAsync(context, argPos);

            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(ErrorMessageManagement(result));
        }
        
        private Task Log(LogMessage msg)
        {
            Console.WriteLine($"[{msg.Severity}] {msg.Source}: {msg.Message}");
            return Task.CompletedTask;
        }

        private string ErrorMessageManagement(IResult r)
        {
            if (r.Error == CommandError.BadArgCount) return Properties.Resources.BadArgCount;
            if (r.Error == CommandError.UnknownCommand) return Properties.Resources.UnknownCommand;

            return r.ErrorReason;
        }
        
    }
}
