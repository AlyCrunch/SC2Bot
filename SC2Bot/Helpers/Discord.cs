using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SC2Bot.Helpers
{
    public static class Discord
    {
        public static bool IsAdminServer(List<Server> servers, string server, User u)
        {
            var s = servers.First(x => x.Name == server);
            if (s == null) return false;

            var us = s.Users.First(x => x.Id == u.Id);
            if (us == null) return false;

            if (us.Roles.Any(r => r.Permissions.Administrator)) return true;
            return false;
        }

        public static bool IsAdminGetFirstServer(User u, DiscordClient c)
        {
            var s = c.Servers.First();
            return IsAdminServer(new List<Server>() { s }, s.Name, u);
        }

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

        public static List<User> GetUserNoRole(Server s) => s.Users.Where(u => u.Roles.Count() == 1 && u.Roles.First().IsEveryone).ToList();

        public static bool IsAdmin(User u, bool SuperAdmin = false, ulong SuperAdminID = 0)
        {
            if (SuperAdmin)
                return u.Id == SuperAdminID;

            return u.Roles.Count(
                x => x.Permissions.Administrator) > 0;
        }

        public static List<User> NoRoleUsers(Server serv)
        {
            return serv.Users.Where(x => x.Roles.Count() == 1 && x.Roles.First().IsEveryone).ToList();
        }
    }
}
