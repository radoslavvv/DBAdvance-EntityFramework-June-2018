using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workshop.App.Utilities;
using Workshop.Data;
using Workshop.Models;

namespace Workshop.App.Core.Command
{
    public class DisbandCommand : ICommand
    {
        private readonly AuthenticationManager authenticationManager;
        private readonly WorkShopDbContext context;
        public DisbandCommand(AuthenticationManager authenticationManager)
        {
            this.context = new WorkShopDbContext();
            this.authenticationManager = authenticationManager;
        }

        public string Execute(string[] arguments)
        {
            int argsCount = arguments.Length;
            if (argsCount != 1)
            {
                throw new FormatException(Constants.ErrorMessages.InvalidArgumentsCount);
            }

            this.authenticationManager.Authorize();
            User currentUser = this.authenticationManager.GetCurrentUser();

            string teamName = arguments[0];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(String.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            if (!CommandHelper.IsUserCreatorOfTeam(teamName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            Team team = this.context
                .Teams
                .FirstOrDefault(t => t.Name == teamName);

            IEnumerable<TeamEvent> teamEvents = this.context
                .TeamEvents
                .Where(et => et.Team == team);

            IEnumerable<UserTeam> userTeams = context
                .UserTeams
                .Where(ut => ut.Team == team);

            IEnumerable<Invitation> invitations = context
                .Invitations
                .Where(i => i.Team == team);

            this.context.TeamEvents.RemoveRange(teamEvents);
            this.context.UserTeams.RemoveRange(userTeams);
            this.context.Invitations.RemoveRange(invitations);

            this.context.Teams.Remove(team);

            this.context.SaveChanges();

            return $"{teamName} has disbanded!";
        }
    }
}

