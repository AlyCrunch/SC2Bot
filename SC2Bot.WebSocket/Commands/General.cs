using Discord;
using Discord.Commands;
using hs = SC2Bot.WebSocket.Helpers.HelpStrings.General;
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

        [Command("commands"), Alias("cmd"), Summary(hs.cmdSummary), Remarks(hs.cmdRemarks)]
        public async Task Commands()
        {
            var builder = new EmbedBuilder()
            {
                Color = GeneralColor
            };

            foreach (var module in _service.Modules)
            {
                string cmdDescription = null;
                foreach (var cmd in module.Commands)
                {
                    var result = await cmd.CheckPreconditionsAsync(Context);
                    if (result.IsSuccess)
                        cmdDescription += $"!{cmd.Aliases.First()}  *{cmd.Summary}*\n";
                }

                if (!string.IsNullOrWhiteSpace(cmdDescription))
                {
                    builder.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = cmdDescription;
                        x.IsInline = false;
                    });
                }
            }
            await ReplyAsync("", false, builder.Build());
        }

        [Command("help"), Alias("h"), Summary(hs.helpSummary), Remarks(hs.helpRemarks)]
        public async Task HelpCommand(string command)
        {
            var result = _service.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($"Désolée, il n'y a pas de commande **{command}** (encore ?).");
                return;
            }

            var builder = new EmbedBuilder()
            {
                Color = GeneralColor,
            };

            foreach (var match in result.Commands)
            {            
                var cmd = match.Command;
                string howTo = (string.IsNullOrEmpty(cmd.Remarks)) ? "" : $"\n\n{cmd.Remarks}";

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = $"*{cmd.Summary}*" + howTo;
                    x.IsInline = false;
                });
            }

            await ReplyAsync("", false, builder.Build());
        }
        
        [Command("yesno"), Summary(hs.yesnoSummary), Remarks(hs.yesnoRemarks)]
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

        [Command("iam"), Alias("jesuis"), Summary(hs.iamSummary), Remarks(hs.iamRemarks)]
        public async Task Assimilate([Summary("Race name.")] string race, [Summary("Second main race name.")] string seconde = "nope")
        {
            var config = GetConfigUserRoles(Context.Message.Author.Id);
            await ReplyAsync(await Helpers.GeneralHelper.Roles(config.Result.Item1, config.Result.Item2, new List<string>() { race, seconde }));
        }
        /*
        [Command("assign"), Summary("Choose your weapon, noob."), RequireUserPermission(GuildPermission.Administrator)]
        public async Task AssimilateSomeone([Summary("User")] IUser user, [Summary("Main race name.")] string race, [Summary("Second main race name.")] string seconde = "nope")
        {
            var config = GetConfigUserRoles(user.Id);
            await ReplyAsync(await Helpers.GeneralHelper.Roles(config.Result.Item1, config.Result.Item2, new List<string>() { race, seconde }));
        }
        */

        [Command("whosplaying"), Alias("wspl"), Summary(hs.wsplSummary), Remarks(hs.wsplRemarks)]
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

        [Command("our"), Summary(hs.ourSummary), Remarks(hs.ourRemarks)]
        public async Task OuranosIsOur([Summary("Nom")] string noun)
        {
            await ReplyAsync("<@!155732781966688256> est notre " + noun);
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
