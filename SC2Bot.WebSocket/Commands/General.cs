using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SC2Bot.WebSocket.Commands
{
    public class General : ModuleBase
    {
        private Color GeneralColor = new Color(0, 204, 102);
        private CommandService _service;

        public General(CommandService service)
        {
            _service = service;
        }

        [Command("help")]
        public async Task HelpAsync()
        {
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = "Tu peux utiliser :"
            };

            foreach (var module in _service.Modules)
            {
                string description = null;
                foreach (var cmd in module.Commands)
                {
                    var result = await cmd.CheckPreconditionsAsync(Context);
                    if (result.IsSuccess)
                        description += $"!{cmd.Aliases.First()}\n";
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    builder.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }
            await ReplyAsync("", false, builder.Build());
        }

        [Command("help")]
        public async Task HelpAsync(string command)
        {
            var result = _service.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($"Sorry, I couldn't find a command like **{command}**.");
                return;
            }

            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = $"Here are some commands like **{command}**"
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
                              $"Summary: {cmd.Summary}";
                    x.IsInline = false;
                });
            }

            await ReplyAsync("", false, builder.Build());
        }

        [Command("commands"), Summary("Get list commands")]
        public async Task ListCommands()
        {
            await ReplyAsync("");
        }

        [Command("yesno"), Summary("Randomly say no or yes")]
        public async Task YesNo()
        {
            var imgr = new Helpers.ImgurHelper();
            var yn = Helpers.GeneralHelper.GetRandom(2);

            string img, str;
            if (yn == 0)
            {
                str = Properties.Resources.yesno_Non;
                img = await Helpers.GeneralHelper.DownloadImage(await imgr.GetARandomImageAlbum(Properties.Resources.AlbumNo));
            }
            else
            {
                str = Properties.Resources.yesno_Oui;
                img = await Helpers.GeneralHelper.DownloadImage(await imgr.GetARandomImageAlbum(Properties.Resources.AlbumYes));
            }

            await Context.Channel.SendFileAsync(img, str);

            Helpers.GeneralHelper.RemoveImageTemp(img);
        }

        [Command("iam"), Summary("Choose your weapon, noob."), Alias("jesuis")]
        public async Task Assimilate([Summary("Race name.")] string race, [Summary("Second main race name.")] string seconde = "nope")
        {
            var config = GetConfigUserRoles(Context.Message.Author.Id);
            await ReplyAsync(await Helpers.GeneralHelper.Roles(config.Result.Item1, config.Result.Item2, new List<string>() { race, seconde }));
        }

        [Command("assign"), Summary("Choose your weapon, noob."), RequireUserPermission(GuildPermission.Administrator)]
        public async Task AssimilateSomeone([Summary("User")] IUser user, [Summary("Main race name.")] string race, [Summary("Second main race name.")] string seconde = "nope")
        {
            var config = GetConfigUserRoles(user.Id);
            await ReplyAsync(await Helpers.GeneralHelper.Roles(config.Result.Item1, config.Result.Item2, new List<string>() { race, seconde }));
        }
        
        [Command("whosplaying"), Summary("Get players by a specified game"), Alias("wspl")]
        public async Task WhosPlayingByAGame([Summary("Game (optional)")] string game = null)
        {
            var allUsers = await Context.Guild.GetUsersAsync();
            IEnumerable<Tuple<string, IEnumerable<IUser>>> players = null;
            var strRetNull = "Personne ne joue.";

            if (game != null)
            {
                strRetNull = "Personne ne joue à " + game;
                players = new Helpers.WhosPlaying().GetPlayersByGame(game, allUsers);
            }
            else
                players = new Helpers.WhosPlaying().GetPlayers(allUsers);

            if (players == null) await ReplyAsync(strRetNull);
            else await ReplyAsync("", false, CreateEmbedWhosPlaying(players));
        }

        [Command("our"), Summary("Ouranos is our")]
        public async Task OuranosIsOur([Summary("Game")] string adjectif)
        {
            await ReplyAsync("Ouranos est notre " + adjectif);
        }

        private Embed CreateEmbedWhosPlaying(IEnumerable<Tuple<string, IEnumerable<IUser>>> GamesPlayer)
        {
            EmbedBuilder eb = new EmbedBuilder()
            {
                Color = GeneralColor,
                Author = new EmbedAuthorBuilder().WithName("Who's playing")
            };

            foreach (var game in GamesPlayer)
            {
                List<IUser> u = game.Item2.ToList();
                eb.AddField(x =>
                {
                    x.IsInline = false;
                    x.Name = game.Item1;
                    x.Value = string.Join(", ", u.Select(m => m.Mention));
                });
            }

            return eb;
        }

        private async Task<Tuple<IGuildUser, List<IRole>>> GetConfigUserRoles(ulong UserID)
        {
            var roles = new List<IRole>(Context.Guild.Roles.Where(x => x.Position < 8 && x.Position > 3));
            var guild = await Context.Guild.GetUserAsync(UserID);

            return new Tuple<IGuildUser, List<IRole>>(guild, roles);
        }
    }
}
