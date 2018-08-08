using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Workshop.App.Utilities;
using Workshop.Data;
using Workshop.Models;

namespace Workshop.App.Core.Command
{
    public class CreateEventCommand : ICommand
    {
        private readonly AuthenticationManager authenticationManager;
        public CreateEventCommand(AuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLenght(6, arguments);
            this.authenticationManager.Authorize();

            User user = this.authenticationManager.GetCurrentUser();

            string eventName = arguments[0];
            if (eventName.Length > Constants.MaxEventNameLength)
            {
                throw new ArgumentException("Event name should be up to 25 characters long!");

            }

            var eventDescription = arguments[1];
            if (eventDescription.Length > Constants.MaxEventDescriptionLength)
            {
                throw new ArgumentException("Event description should bet up to 250 characters long!");
            }


            string startDateString = $"{arguments[2]} {arguments[3]}";
            DateTime startDate;

            string endDateString = $"{arguments[4]} {arguments[5]}";
            DateTime endDate;

            bool endDateIsValid = DateTime.TryParseExact(endDateString, Constants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
            bool startDateIsValid = DateTime.TryParseExact(startDateString, Constants.DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);

            if (!startDateIsValid || !endDateIsValid)
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidDateFormat);
            }

            using (WorkShopDbContext context = new WorkShopDbContext())
            {
                Event currentEvent = new Event()
                {
                    Name = eventName,
                    Description = eventDescription,
                    StartDate = startDate,
                    EndDate = endDate,
                    CreatorId = user.Id
                };

                context.Events.Add(currentEvent);
                context.SaveChanges();
            }

            return $"Event {eventName} was created successfully!";
        }
    }
}
