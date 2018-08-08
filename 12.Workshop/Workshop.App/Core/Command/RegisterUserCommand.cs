using System;
using System.Collections.Generic;
using System.Text;
using Workshop.App.Utilities;
using Workshop.Data;
using Workshop.Models;
using Workshop.Models.Enums;

namespace Workshop.App.Core.Command
{
    public class RegisterUserCommand : ICommand
    {
        private readonly WorkShopDbContext context;
        private readonly AuthenticationManager authenticationManager;

        public RegisterUserCommand(AuthenticationManager authenticationManager)
        {
            this.context = new WorkShopDbContext();
            this.authenticationManager = authenticationManager;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLenght(7, arguments);

            string username = arguments[0];
            string password = arguments[1];
            string passwordConfirm = arguments[2];
            string firstName = arguments[3];
            string lastName = arguments[4];
            int age;
            string genderString = arguments[6];
            Gender gender;

            if (username.Length < Constants.MinUsernameLength || username.Length > Constants.MaxUsernameLength)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UsernameNotValid, username));
            }

            if (password.Length < Constants.MinPasswordLength || password.Length > Constants.MaxPasswordLength)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UserOrPasswordIsInvalid, password));
            }

            if (!int.TryParse(arguments[5], out age))
            {
                throw new ArgumentException(Constants.ErrorMessages.AgeNotValid);
            }

            if (!Enum.TryParse<Gender>(genderString, out gender))
            {
                throw new ArgumentException(Constants.ErrorMessages.GenderNotValid);
            }

            if (password != passwordConfirm)
            {
                throw new ArgumentException(Constants.ErrorMessages.PasswordDoesNotMatch);
            }

            if (CommandHelper.IsUserExisting(username))
            {
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.UsernameIsTaken, username));
            }

            if (this.authenticationManager.IsAutheticated())
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LogoutFirst);
            }

            User user = new User()
            {
                Username = username,
                Password = password,
                Age = age,
                FirstName = firstName,
                LastName = lastName
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();

            return $"User {username} was registered succesffully!";
        }
    }
}
