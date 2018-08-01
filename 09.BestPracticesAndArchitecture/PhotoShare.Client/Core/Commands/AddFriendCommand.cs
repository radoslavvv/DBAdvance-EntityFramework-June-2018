namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using PhotoShare.Client.Core.Dtos;
    using PhotoShare.Client.Core.Validation;
    using PhotoShare.Models;
    using PhotoShare.Services.Contracts;

    [CredentialsAttribute(true)]
    public class AddFriendCommand : ICommand
    {
        private readonly IUserService userService;
        public AddFriendCommand(IUserService userService)
        {
            this.userService = userService;
        }

        // AddFriend <username1> <username2>
        public string Execute(string[] data)
        {
            string username = data[0];
            string friendUsername = data[1];

            bool userExists = this.userService.Exists(username);
            bool friendExists = this.userService.Exists(friendUsername);

            if (!userExists)
            {
                throw new ArgumentException($"{username} not found!");
            }

            if (!friendExists)
            {
                throw new ArgumentException($"{friendUsername} not found!");
            }

            UserFriendsDto user = this.userService.ByUsername<UserFriendsDto>(username);
            UserFriendsDto friend = this.userService.ByUsername<UserFriendsDto>(friendUsername);

            bool isSendRequestFromUser = user.Friends.Any(u => u.Username == friend.Username);
            bool isSendRequestFromFriend = friend.Friends.Any(f => f.Username == user.Username);

            if (isSendRequestFromUser && isSendRequestFromFriend)
            {
                throw new InvalidOperationException($"{username} is already friend to {friendUsername}!");
            }
            else if (isSendRequestFromUser && !isSendRequestFromUser)
            {
                throw new InvalidOperationException("Request is already sent!");
            }

            this.userService.AddFriend(user.Id, friend.Id);

            return $"Friend {friend.Username} added to {user.Username}";
        }
    }
}
