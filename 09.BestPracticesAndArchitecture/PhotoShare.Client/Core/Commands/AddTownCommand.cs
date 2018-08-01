namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Dtos;
    using Contracts;
    using Services.Contracts;
    using PhotoShare.Models;
    using PhotoShare.Client.Core.Validation;

    [CredentialsAttribute(true)]
    public class AddTownCommand : ICommand
    {
        private readonly ITownService townService;

        public AddTownCommand(ITownService townService)
        {
            this.townService = townService;
        }

        // AddTown <townName> <countryName>
        public string Execute(string[] data)
        {
            string townName = data[0];
            string country = data[1];

            bool townExists = this.townService.Exists(townName);
            if (townExists)
            {
                throw new ArgumentException($"Town {townName} was already added!");
            }

            Town town = this.townService.Add(townName, country);

            return $"Town {townName} was added successfully!";
        }
    }
}
