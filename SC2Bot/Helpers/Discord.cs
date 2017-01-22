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

        public static bool IsAdmin(User u, bool SuperAdmin = false, ulong SuperAdminID = 0)
        {
            if(SuperAdmin)
                return u.Id == SuperAdminID;

            return u.Roles.Count(x => x.Name == "Jungsu Zerg" || x.Name == "Jungsu Protoss" || x.Name == "Jungsu Terran" || x.Name == "Gosu 💎") > 0;
        }
    }
}
