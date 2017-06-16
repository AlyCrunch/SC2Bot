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
        private SocketGuild guild;
        private SocketTextChannel channel;

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
                Console.WriteLine("Yolo swagghetti");

                guild = client.GetGuild(139482122548281345);
                channel = guild.GetTextChannel(269901596266201089);

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
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
        
        private Task Log(LogMessage msg)
        {
            Console.WriteLine($"[{msg.Severity}] {msg.Source}: {msg.Message}");
            return Task.CompletedTask;
        }

    }
}
