using PhotoShare.Client.Core.Contracts;
using PhotoShare.Client.Core.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoShare.Client.Core
{
    [CredentialsAttribute(true)]
    public class LogoutCommand : ICommand
    {
        public LogoutCommand() { }

        public string Execute(string[] args)
        {
            if(Session.User == null)
            {
                throw new ArgumentException($"You should log in first in order to logout!");
            }
            string username = Session.User.Username;
            Session.User = null;

            return $"User {username} successfully logged out!";
        }
    }
}
