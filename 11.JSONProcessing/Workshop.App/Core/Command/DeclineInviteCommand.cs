using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workshop.App.Utilities;
using Workshop.Data;
using Workshop.Models;

namespace Workshop.App.Core.Command
{

    public class DeclineInviteCommand : ICommand
    {
        private readonly AuthenticationManager authenticationManager;
        private readonly WorkShopDbContext context;

        public DeclineInviteCommand(AuthenticationManager authenticationManager)
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
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            if (!CommandHelper.IsInviteExisting(teamName, currentUser))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InviteNotFound, teamName));
            }

            Team team = this.context
                .Teams
                .FirstOrDefault(t => t.Name == teamName);

            this.context.Invitations
                .FirstOrDefault(i => i.TeamId == team.Id && i.InvitedUserId == currentUser.Id)
                .IsActive = false;

            this.context.SaveChanges();

            return $"Invite from {teamName} declined.";
        }
    }
}
