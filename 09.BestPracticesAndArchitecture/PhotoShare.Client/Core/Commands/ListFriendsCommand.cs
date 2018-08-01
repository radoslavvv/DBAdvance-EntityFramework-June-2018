using PhotoShare.Client.Core.Contracts;
using PhotoShare.Client.Core.Dtos;
using PhotoShare.Models;
using PhotoShare.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoShare.Client.Core.Commands
{
    public class ListFriendsCommand : ICommand
    {
        private readonly IUserService userService;

        public ListFriendsCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] args)
        {
            string username = args[0];

            bool userExists = this.userService.Exists(username);
            if (!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            List<User> userFriends = this.userService
                .ByUsername<User>(username)
                .FriendsAdded
                .Select(fr => fr.Friend)
                .OrderBy(f => f.Username)
                .ToList();

            StringBuilder sb = new StringBuilder();
            if(userFriends.Count == 0)
            {
                sb.AppendLine("No friends for this user. :(");
            }
            else
            {
                sb.AppendLine("Friends:");
                foreach (User friend in userFriends)
                {
                    sb.AppendLine(friend.Username);
                }
            }

            return sb.ToString().Trim();
        }
    }
}
