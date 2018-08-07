
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workshop.App.Utilities;
using Workshop.Data;
using Workshop.Models;

namespace Workshop.App.Core.Command
{
    public class ShowTeamCommand : ICommand
    {
        private readonly AuthenticationManager authenticationManager;
        private readonly WorkShopDbContext context;

        public ShowTeamCommand(AuthenticationManager authenticationManager)
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

            string teamName = arguments[0];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            Team team = this.context.Teams.FirstOrDefault(t => t.Name == teamName);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{teamName} {team.Acronym}");
            sb.AppendLine("Members:");
            foreach (var member in team.Members)
            {
                sb.AppendLine($"--{member.User.Username}");
            }

            return sb.ToString().Trim();
        }
    }
}
