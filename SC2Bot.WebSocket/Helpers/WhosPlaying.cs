using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC2Bot.WebSocket.Helpers
{
    public class WhosPlaying
    {
        public IEnumerable<IUser> GetExactPlayers(string game, IReadOnlyCollection<IUser> users)
        {
            return users.Where(x => x.Game?.Name?.ToUpperInvariant() == game.ToUpperInvariant());
        }

        public IEnumerable<Tuple<string, IEnumerable<IUser>>> GetPlayers(IReadOnlyCollection<IUser> users)
        {
            var result = users.Where(x => !x.IsBot && x.Game.HasValue)
                            .GroupBy(x => x.Game?.Name);

            return ConvertIGroupingToTuple(result);
        }

        public IEnumerable<Tuple<string, IEnumerable<IUser>>> GetPlayersByGame(string game, IReadOnlyCollection<IUser> users)
        {
            string g = game.ToUpperInvariant();
            var result = users.Where(x => !x.IsBot && x.Game.HasValue)
                            .GroupBy(x => x.Game?.Name)
                            .Where(x => x.Key.ToUpperInvariant().Contains(g));

            return ConvertIGroupingToTuple(result);
        }

        private IEnumerable<Tuple<string, IEnumerable<IUser>>> ConvertIGroupingToTuple(IEnumerable<IGrouping<string, IUser>> result)
        {
            if (result.Count() == 0) return null;

            List<Tuple<string, IEnumerable<IUser>>> outResult = new List<Tuple<string, IEnumerable<IUser>>>();

            foreach(var r in result)
                outResult.Add(new Tuple<string, IEnumerable<IUser>>(r.Key,r.ToList()));

            return outResult;
        }
    }
}
