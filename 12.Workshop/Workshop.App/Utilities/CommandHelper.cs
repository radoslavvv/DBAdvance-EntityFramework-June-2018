using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workshop.Data;
using Workshop.Models;

namespace Workshop.App.Utilities
{
    public static class CommandHelper
    {

        public static bool IsTeamExisting(string teamName)
        {
            using (WorkShopDbContext context = new WorkShopDbContext())
            {
                return context.Teams.SingleOrDefault(x => x.Name == teamName) == null ? false : true;
            }
        }

        public static bool IsUserExisting(string username)
        {
            using (WorkShopDbContext context = new WorkShopDbContext())
            {
                return context.Users.SingleOrDefault(u => u.Username == username) == null ? false : true;
            }
        }

        public static bool IsInviteExisting(string teamName, User user)
        {
            return user.ReceivedInvitations.SingleOrDefault(x => x.Team.Name == teamName) == null ? false : true;
        }

        public static bool IsUserCreatorOfTeam(string teamName, User user)
        {
            return user.CreatedUserTeams.SingleOrDefault(x => x.Team.Name == teamName) == null ? false : true;
        }

        public static bool IsUserCreatorOfEvent(string eventName, User user)
        {
            return user.CreatedEvents.SingleOrDefault(x => x.Name == eventName) == null ? false : true;
        }

        public static bool IsMemberOfTeam(string teamName, string username)
        {
            using (WorkShopDbContext context = new WorkShopDbContext())
            {
                return context.Teams.SingleOrDefault(t => t.Name == teamName).Members.Any(m => m.User.Username == username);
            }
        }

        public static bool IsEventExisting(string eventName)
        {
            using (WorkShopDbContext context = new WorkShopDbContext())
            {
                return context.Events.Any(e => e.Name == eventName);
            }
        }
    }
}
