using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Workshop.App.Utilities;
using Workshop.Models;

namespace Workshop.App.Core
{
    public class AuthenticationManager
    {
        public AuthenticationManager() { }

        public User LogedInUser { get; private set; }

        public void LogIn(User user) 
        {
            if(this.LogedInUser != null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LogoutFirst);
            }

            this.LogedInUser = user;
        }

        public void LogOut()
        {
            this.LogedInUser = null;
        }

        public void Authorize()
        {
            if(!IsAutheticated())
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
            }
        }

        public bool IsAutheticated()
        {
            return this.LogedInUser != null;
        }

        public User GetCurrentUser()
        {
            Authorize();
             
            return this.LogedInUser;
        }
    }
}
