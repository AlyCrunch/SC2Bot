using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SC2Bot.WebSocket.Commands
{
    public class General : ModuleBase
    {
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

        [Command("jesuis"), Summary("Choose your weapon, noob."), Alias("iam")]
        public async Task Assimilate([Summary("Race name.")] string race)
        {
            await ReplyAsync(await Roles(Context.Message.Author, race));
        }

        [Command("tues"), Summary("Choose your weapon, noob."), RequireUserPermission(GuildPermission.Administrator), Alias("youare")]
        public async Task AssimilateSomeone([Summary("User")] IUser user, [Summary("Race name.")] string race)
        {
            await ReplyAsync(await Roles(user, race));
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
