using Discord.Commands;
using hs = SC2Bot.WebSocket.Helpers.HelpStrings.SpawningTools;
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;

namespace SC2Bot.WebSocket.Commands
{
    public class SpawningTool : ModuleBase
    {

        [Command("BO"), Summary(hs.BOSummary), Remarks(hs.BORemarks)]
        public async Task BO()
        {
            await ReplyAsync(Properties.Resources.ReleaseSoon);
        }
    }
}
