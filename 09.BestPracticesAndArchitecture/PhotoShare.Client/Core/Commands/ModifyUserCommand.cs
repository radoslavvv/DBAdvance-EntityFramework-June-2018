namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Client.Core.Validation;
    using PhotoShare.Models;
    using PhotoShare.Services.Contracts;

    [CredentialsAttribute(true)]
    public class ModifyUserCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly ITownService townService;

        public ModifyUserCommand(IUserService userService, ITownService townService)
        {
            this.userService = userService;
            this.townService = townService;
        }

        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username
        public string Execute(string[] data)
        {
            string username = data[0];
            string propertyName = data[1];
            string value = data[2];

            bool userExists = this.userService.Exists(username);
            if (!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            int userId = this.userService.ByUsername<UserDto>(username).Id;

            switch (propertyName)
            {
                case "BornTown":
                case "CurrentTown":
                    SetTown(userId, propertyName, value);
                    break;
                case "Password":
                    SetPassword(userId, value);
                    break;
                default:
                    throw new ArgumentException($"Property {propertyName} not supported!");
            }

            return $"User {username} {propertyName} is {value}.";
        }

        private void SetTown(int userId, string townType, string townName)
        {
            bool townExists = this.townService.Exists(townName);
            if (!townExists)
            {
                throw new ArgumentException($"Value {townName} not valid.Town {townName} not found");
            }

            int townId = this.townService.ByName<Town>(townName).Id;

            if (townType == "BornTown")
            {
                this.userService.SetBornTown(userId, townId);
            }
            else if (townType == "CurrentTown")
            {
                this.userService.SetCurrentTown(userId, townId);
            }
        }

        private void SetPassword(int userId, string password)
        {
            bool passwordContainsLowerChars = password.Any(c => char.IsLower(c));
            bool passwordContainsDigits = password.Any(c => char.IsDigit(c));

            if (!passwordContainsLowerChars || !passwordContainsDigits)
            {
                throw new ArgumentException($"Value {password} not valid. \r\nInvalid Password!");
            }

            this.userService.ChangePassword(userId, password);
        }
    }
}
