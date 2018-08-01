namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Dtos;
    using Contracts;
    using Services.Contracts;
    using PhotoShare.Models;
    using PhotoShare.Client.Core.Validation;

    [CredentialsAttribute(true)]
    public class DeleteUserCommand : ICommand
    {
        private readonly IUserService userService;

        public DeleteUserCommand(IUserService userService)
        {
            this.userService = userService;
        }

        // DeleteUser <username>
        public string Execute(string[] data)
        {
            string username = data[0];

            bool userExists = this.userService.Exists(username);
            if (!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            this.userService.Delete(username);

            return $"User {username} was deleted from the database!";
        }
    }
}
