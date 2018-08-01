using PhotoShare.Client.Core.Contracts;
using PhotoShare.Client.Core.Validation;
using PhotoShare.Models;
using PhotoShare.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoShare.Client.Core.Commands
{
    [CredentialsAttribute(false)]
    public class LoginCommand : ICommand
    {
        private readonly IUserService userService;
        public LoginCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] args)
        {
            string username = args[0];
            string password = args[1];

            bool userExists = this.userService.Exists(username);
            if (!userExists)
            {
                throw new ArgumentException($"Invalid username!");
            }

            User user = this.userService.ByUsername<User>(username);
            if(user.Password != password)
            {
                throw new ArgumentException("Invalid password!");
            }

            if (Session.User == user)
            {
                throw new ArgumentException("You should logout first!");
            }

            Session.User = user;

            return $"User {username} successfully logged in!";
        }
    }
}
