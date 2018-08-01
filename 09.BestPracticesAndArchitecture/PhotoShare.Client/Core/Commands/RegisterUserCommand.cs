﻿namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Client.Core.Validation;
    using PhotoShare.Services.Contracts;

    [CredentialsAttribute(false)]
    public class RegisterUserCommand : ICommand
    {
        private readonly IUserService userService;
        public RegisterUserCommand(IUserService userService)
        {
            this.userService = userService;
        }

        // RegisterUser <username> <password> <repeat-password> <email>
        public string Execute(string[] data)
        {
            string username = data[0];
            string password = data[1];
            string repeatPassowrd = data[2];
            string email = data[3];

            RegisterUserDto registerUserDto = new RegisterUserDto()
            {
                Username = username,
                Password = password,
                Email = email
            };

            if (!IsValid(registerUserDto))
            {
                throw new ArgumentException("Invalid data!");
            }

            bool userExists = this.userService.Exists(username);
            if (userExists)
            {
                throw new InvalidOperationException($"Username {username} is already taken!");
            }

            if(password != repeatPassowrd)
            {
                throw new ArgumentException("Passwords do not match!");
            }

            this.userService.Register(username, password, email);

            return $"User {username} was registered successfully!";
        }

        private bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}
