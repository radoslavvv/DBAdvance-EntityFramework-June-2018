using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workshop.App.Utilities;
using Workshop.Data;
using Workshop.Models;

namespace Workshop.App.Core.Command
{
    public class InviteToTeamCommand : ICommand
    {
        private readonly AuthenticationManager authenticationManager;
        private readonly WorkShopDbContext context;
        public InviteToTeamCommand(AuthenticationManager authenticationManager)
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

            string teamName = arguments[0];
            string username = arguments[1];

            User currentUser = this.authenticationManager.GetCurrentUser();

            Team team = this.context.Teams.FirstOrDefault(t => t.Name == teamName);
            User invitedUser = this.context.Users.FirstOrDefault(u => u.Username == username);

            if (currentUser == invitedUser)
            {
                this.context.UserTeams.Add(new UserTeam
                {
                    UserId = team.CreatorId,
                    TeamId = team.Id
                });

                this.context.SaveChanges();

                return $"Team {teamName} invited {username}";
            }

            if (team == null || invitedUser == null)
            {
                throw new ArgumentException(Constants.ErrorMessages.TeamOrUserNotExist);
            }

            if (!CommandHelper.IsUserCreatorOfTeam(team.Name, currentUser) || CommandHelper.IsMemberOfTeam(team.Name, currentUser.Username) || CommandHelper.IsMemberOfTeam(team.Name, invitedUser.Username))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            if (CommandHelper.IsInviteExisting(team.Name, invitedUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.InviteIsAlreadySent);
            }

            this.context.Invitations.Add(new Invitation
            {
                InvitedUserId = invitedUser.Id,
                TeamId = team.Id
            });

            this.context.SaveChanges();

            return $"Team {teamName} invited {username}!";
        }
    }
}
