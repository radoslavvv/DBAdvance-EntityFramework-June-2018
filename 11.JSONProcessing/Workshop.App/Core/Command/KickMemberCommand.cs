using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workshop.App.Utilities;
using Workshop.Data;
using Workshop.Models;

namespace Workshop.App.Core.Command
{
    public class KickMemberCommand : ICommand
    {
        private readonly AuthenticationManager authenticationManager;
        private readonly WorkShopDbContext context;
        public KickMemberCommand(AuthenticationManager authenticationManager)
        {
            this.context = new WorkShopDbContext();
            this.authenticationManager = authenticationManager;
        }

        public string Execute(string[] arguments)
        {
            int argsCount = arguments.Length;
            if (argsCount != 2)
            {
                throw new FormatException(Constants.ErrorMessages.InvalidArgumentsCount);
            }

            this.authenticationManager.Authorize();
            User currentUser = this.authenticationManager.GetCurrentUser();

            string teamName = arguments[0];
            string username = arguments[1];

            if (!CommandHelper.IsUserCreatorOfTeam(teamName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(String.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            if (!CommandHelper.IsUserExisting(username))
            {
                throw new ArgumentException(String.Format(Constants.ErrorMessages.UserNotFound, username));
            }

            if (!CommandHelper.IsMemberOfTeam(teamName, username))
            {
                throw new ArgumentException(String.Format(Constants.ErrorMessages.NotPartOfTeam, username, teamName));
            }

            if (currentUser.Username == username)
            {
                throw new InvalidOperationException("Command not allowed. Use DisbandTeam instead.");
            }

            User kickUser = this.context.Users.FirstOrDefault(u => u.Username == username);
            Team team = this.context.Teams.FirstOrDefault(t => t.Name == teamName);
            UserTeam userTeam = this.context.UserTeams.FirstOrDefault(ut => ut.Team == team && ut.User == kickUser);

            this.context.UserTeams.Remove(userTeam);
            this.context.SaveChanges();

            return $"User {username} was kicked from {teamName}!";
        }
    }
}
