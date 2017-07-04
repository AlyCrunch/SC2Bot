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
        public async Task Assimilate([Summary("Race name.")] string race)
        {
            await ReplyAsync(await Roles(Context.Message.Author, race));
        }

        [Command("assign"), Summary("Choose your weapon, noob."), RequireUserPermission(GuildPermission.Administrator)]
        public async Task AssimilateSomeone([Summary("User")] IUser user, [Summary("Race name.")] string race)
        {
            await ReplyAsync(await Roles(user, race));
        }

        [Command("whosplaying"), Summary("Get all games and players")]
        public async Task WhosPlayingAll()
        {
            var allUsers = await Context.Guild.GetUsersAsync();
            var players = new Helpers.WhosPlaying().GetPlayers(allUsers);

            if (players == null) await ReplyAsync("Personne ne joue.");
            else await ReplyAsync("", false, CreateEmbedWhosPlaying(players));
        }

        [Command("whosplaying"), Summary("Get players by a specified game")]
        public async Task WhosPlayingByAGame([Summary("Game")] string game)
        {
            var allUsers = await Context.Guild.GetUsersAsync();
            var players = new Helpers.WhosPlaying().GetPlayersByGame(game, allUsers);

            if (players == null) await ReplyAsync("Personne ne joue à " + game);
            else await ReplyAsync("", false, CreateEmbedWhosPlaying(players));
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

        private async Task<string> Roles(IUser u, string race)
        {
            var roles = new List<IRole>(Context.Guild.Roles);
            var guild = await Context.Guild.GetUserAsync(u.Id);

            IRole role = null;
            string strRtn = string.Empty;

            switch (race.ToLower())
            {
                case "zerg": role = roles.Find(r => r.Name.ToLower() == "zerg"); strRtn = Properties.Resources.ZergQuote; break;
                case "terran": role = roles.Find(r => r.Name.ToLower() == "terran"); strRtn = Properties.Resources.TerranQuote; break;
                case "protoss": role = roles.Find(r => r.Name.ToLower() == "protoss"); strRtn = Properties.Resources.ProtossQuote; break;
                case "random": role = roles.Find(r => r.Name.ToLower() == "random"); strRtn = Properties.Resources.RandomQuote; break;
            }

            if (role != null)
            {
                await guild.AddRoleAsync(role);
                return strRtn;
            }
            else
            {
                return @"¯\_(ツ)_/¯";
            }
        }
    }
}
