using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workshop.App.Utilities;
using Workshop.Data;
using Workshop.Models;

namespace Workshop.App.Core.Command
{
    public class AddTeamToCommand : ICommand
    {
        private readonly AuthenticationManager authenticationManager;
        private readonly WorkShopDbContext context;

        public AddTeamToCommand(AuthenticationManager authenticationManager)
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

            string eventName = arguments[0];
            string teamName = arguments[1];

            if (!CommandHelper.IsUserCreatorOfEvent(eventName, currentUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            if (!CommandHelper.IsEventExisting(eventName))
            {
                throw new ArgumentException(String.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(String.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            Event @event = this.context
                .Events
                .Where(e => e.Name == eventName)
                .OrderBy(e => e.StartDate)
                .Last();

            Team team = this.context
                .Teams
                .FirstOrDefault(t => t.Name == teamName);

            if (this.context.TeamEvents.Any(et => et.Team == team && et.Event == @event))
            {
                throw new InvalidOperationException("Cannot add same team twice!");
            }

            this.context.TeamEvents.Add(new TeamEvent
            {
                Event = @event,
                Team = team
            });

            this.context.SaveChanges();

            return $"Team {teamName} added for {eventName}!";
        }
    }
}
