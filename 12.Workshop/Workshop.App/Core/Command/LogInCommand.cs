using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workshop.App.Utilities;
using Workshop.Data;
using Workshop.Models;

namespace Workshop.App.Core.Command
{
    public class LogInCommand : ICommand
    {
        private readonly AuthenticationManager authenticationManager;
        private readonly WorkShopDbContext context;

        public LogInCommand(AuthenticationManager authenticationManager)
        {
            this.context = new WorkShopDbContext();
            this.authenticationManager = authenticationManager;
        }

        public string Execute(string[] arguments)
        {
            Check.CheckLenght(2, arguments);

            if (this.authenticationManager.IsAutheticated())
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LogoutFirst);
            }
            string username = arguments[0];
            string password = arguments[1];

            if (!this.context.Users.Any(x => x.Username == username))
            {
                throw new ArgumentException(Constants.ErrorMessages.UserOrPasswordIsInvalid);
            }

            User user = this.context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                throw new ArgumentException(Constants.ErrorMessages.UserOrPasswordIsInvalid);
            }

            this.authenticationManager.LogIn(user);

            return $"User {username} successfully logged in!";
        }
    }
}
