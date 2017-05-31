using Discord.Commands;
using System.Threading.Tasks;

namespace SC2Bot.WebSocket.Commands
{
    public class Info : ModuleBase
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
        }
    }
}
