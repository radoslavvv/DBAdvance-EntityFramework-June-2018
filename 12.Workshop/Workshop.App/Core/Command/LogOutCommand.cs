using System;
using System.Collections.Generic;
using System.Text;
using Workshop.App.Utilities;

namespace Workshop.App.Core.Command
{
    public class LogOutCommand : ICommand
    {
        private readonly AuthenticationManager authenticationManager;
        public LogOutCommand(AuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }

        public string Execute(string[] arguments)
        {
            if (!this.authenticationManager.IsAutheticated())
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
            }
            string username = this.authenticationManager.GetCurrentUser().Username;

            this.authenticationManager.LogOut();

            return $"User {username} successfully logged out!";
        }
    }
}
