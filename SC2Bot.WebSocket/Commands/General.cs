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

        [Command("whosplaying"), Summary("Get all players grouped by games"), Alias("wspl")]
        public async Task WhosPlayingAll()
        {
            var allUsers = await Context.Guild.GetUsersAsync();
            var players = new Helpers.WhosPlaying().GetPlayers(allUsers);

            if (players == null) await ReplyAsync("Personne ne joue.");
            else await ReplyAsync("", false, CreateEmbedWhosPlaying(players));
        }

        [Command("whosplaying"), Summary("Get players by a specified game"), Alias("wspl")]
        public async Task WhosPlayingByAGame([Summary("Game")] string game)
        {
            var allUsers = await Context.Guild.GetUsersAsync();
            var players = new Helpers.WhosPlaying().GetPlayersByGame(game, allUsers);

            if (players == null) await ReplyAsync("Personne ne joue à " + game);
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
