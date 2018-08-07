using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workshop.App.Utilities;
using Workshop.Data;
using Workshop.Models;

namespace Workshop.App.Core.Command
{
    public class AcceptInviteCommand : ICommand
    {
        private readonly AuthenticationManager authenticationManager;
        private readonly WorkShopDbContext context;

        public AcceptInviteCommand(AuthenticationManager authenticationManager)
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

            if (CommandHelper.IsInviteExisting(teamName, currentUser))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InviteNotFound, teamName));
            }

            if (CommandHelper.IsMemberOfTeam(teamName, currentUser.Username))
            {
                Invitation invite = this.context
                    .Invitations
                    .FirstOrDefault(i => i.Team.Name == teamName && i.InvitedUser == currentUser && i.IsActive);

                invite.IsActive = false;
                this.context.SaveChanges();

                throw new InvalidOperationException($"You are a member of team {teamName}!");
            }

            Team currentTeam = this.context.Teams.FirstOrDefault(t => t.Name == teamName);

            Invitation currentInvite = this.context.Invitations.FirstOrDefault(i => i.TeamId == currentTeam.Id && i.InvitedUserId == currentUser.Id);

            this.context.UserTeams.Add(new UserTeam
            {
                UserId = currentUser.Id,
                TeamId = currentTeam.Id
            });

            this.context
                .Invitations
                .FirstOrDefault(i => i.TeamId == currentTeam.Id && i.InvitedUserId == currentUser.Id)
                .IsActive = false;

            this.context.SaveChanges();

            return $"User {currentUser.Username} joined team {teamName}!";
        }
    }
}
