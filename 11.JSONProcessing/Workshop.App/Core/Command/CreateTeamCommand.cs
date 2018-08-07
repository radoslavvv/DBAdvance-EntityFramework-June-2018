using System;
using System.Collections.Generic;
using System.Text;
using Workshop.App.Utilities;
using Workshop.Data;
using Workshop.Models;

namespace Workshop.App.Core.Command
{
    public class CreateTeamCommand : ICommand
    {
        private readonly AuthenticationManager authenticationManager;
        public CreateTeamCommand(AuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }

        public string Execute(string[] arguments)
        {
            int argsCount = arguments.Length;
            if (argsCount < 2 || argsCount > 3)
            {
                throw new FormatException(Constants.ErrorMessages.InvalidArgumentsCount);
            }

            this.authenticationManager.Authorize();
            User user = this.authenticationManager.GetCurrentUser();

            string teamName = arguments[0];
            if (teamName.Length > 25)
            {
                throw new ArgumentException("Team name should be up to 25 characters long!");
            }

            if (CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamExists, teamName));
            }

            string acronym = arguments[1];
            if (acronym.Length != 3)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InvalidAcronym, acronym));
            }

            string description = argsCount == 3 ? arguments[2] : null;
            if (description != null && description.Length > 32)
            {
                throw new ArgumentException("Team description should be up to 32 characters long!");
            }

            using (WorkShopDbContext context = new WorkShopDbContext())
            {
                Team currentTeam = new Team()
                {
                    Name = teamName,
                    Acronym = acronym,
                    Description = description,
                    CreatorId = user.Id
                };

                context.Teams.Add(currentTeam);
                context.SaveChanges();
            }

            return $"Team {teamName} successfully created!";
        }
    }
}
