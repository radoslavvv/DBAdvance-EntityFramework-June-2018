namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using PhotoShare.Client.Core.Validation;
    using PhotoShare.Models;
    using PhotoShare.Services.Contracts;

    [CredentialsAttribute(true)]
    public class AcceptFriendCommand : ICommand
    {
        private readonly IUserService userService;
        public AcceptFriendCommand(IUserService userService)
        {
            this.userService = userService;
        }

        // AcceptFriend <username1> <username2>
        public string Execute(string[] data)
        {
            string firstUsername = data[0];
            string secondUsername = data[1];

            bool firstUserExists = this.userService.Exists(firstUsername);
            if (!firstUserExists)
            {
                throw new ArgumentException($"{firstUsername} not found!");
            }

            bool secondUserExists = this.userService.Exists(secondUsername);
            if (!secondUserExists)
            {
                throw new ArgumentException($"{secondUsername} not found!");
            }

            User firstUser = this.userService.ByUsername<User>(firstUsername);
            User secondUser = this.userService.ByUsername<User>(secondUsername);

            if (firstUser.FriendsAdded.Any(f => f.User.Username == secondUsername))
            {
                throw new InvalidOperationException($"{secondUsername} is already a friend to {firstUsername}");
            }
            else if (!secondUser.FriendsAdded.Any(f => f.User.Username == firstUsername))
            {
                throw new InvalidOperationException($"{secondUsername} has not added {firstUsername} as a friend");
            }

            this.userService.AddFriend(firstUser.Id, secondUser.Id);

            return $"{firstUsername} accepted {secondUsername} as a friend";
        }
    }
}
