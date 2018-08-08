using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workshop.App.Utilities;
using Workshop.Data;
using Workshop.Models;

namespace Workshop.App.Core.Command
{
    public class ShowEventCommand : ICommand
    {
        private readonly AuthenticationManager authenticationManager;
        private readonly WorkShopDbContext context;

        public ShowEventCommand(AuthenticationManager authenticationManager)
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

            string eventName = arguments[0];

            if (!CommandHelper.IsEventExisting(eventName))
            {
                throw new ArgumentException(String.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }

            Event @event = this.context.Events.OrderByDescending(e => e.StartDate).First(e => e.Name == eventName);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{@event.Name} {@event.StartDate.ToString(Constants.DateTimeFormat)} {@event.EndDate.ToString(Constants.DateTimeFormat)}");
            sb.AppendLine($"{@event.Description}");
            sb.AppendLine($"Teams: ");
            foreach (var team in @event.ParticipatingEventTeams)
            {
                sb.AppendLine($"-{team.Team.Name}");
            }

            return sb.ToString().Trim();
        }
    }
}
