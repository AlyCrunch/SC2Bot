using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC2Bot.Helpers
{
    public static class Discord
    {
        public static async Task ClearAndAddRole(Server server, User user, Role role)
        {
            var u = server.Users.Where(x => x.Id == user.Id).First();
            var roles = u.Roles;
            foreach (var r in roles)
                if (r.Name == "Protoss"
                    || r.Name == "Terran"
                    || r.Name == "Zerg"
                    || r.Name == "Random")
                    await u.RemoveRoles(r);

            await u.AddRoles(role);
        }
    }
}
