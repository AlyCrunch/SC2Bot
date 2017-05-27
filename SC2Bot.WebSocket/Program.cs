using Discord;
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
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message is a command, based on if it starts with '!' or a mention prefix
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos)))
                return;
            // Create a Command Context
            var context = new CommandContext(client, message);
            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed succesfully)
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
