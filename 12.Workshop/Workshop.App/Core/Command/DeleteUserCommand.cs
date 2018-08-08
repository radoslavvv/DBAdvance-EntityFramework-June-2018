using System;
using System.Collections.Generic;
using System.Text;
using Workshop.App.Utilities;
using Workshop.Data;
using Workshop.Models;

namespace Workshop.App.Core.Command
{
    public class DeleteUserCommand : ICommand
    {
        private readonly WorkShopDbContext context;
        private readonly AuthenticationManager authenticationManager;
        public DeleteUserCommand(AuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
            this.context = new WorkShopDbContext();
        }

        public string Execute(string[] arguments)
        {
            User user = this.authenticationManager.GetCurrentUser();

            if(user == null)
            {
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
            }

            user.IsDeleted = true;
            this.context.Users.Update(user);
            this.context.SaveChanges();

            return $"User {user.Username} was deleted successfully!";
        }
    }
}
