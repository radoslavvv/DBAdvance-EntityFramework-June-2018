namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;
    using PhotoShare.Client.Core.Validation;
    using PhotoShare.Models;
    using PhotoShare.Models.Enums;
    using PhotoShare.Services.Contracts;

    [CredentialsAttribute(true)]
    public class ShareAlbumCommand : ICommand
    {
        private readonly IAlbumService albumService;
        private readonly IUserService userService;
        private readonly IAlbumRoleService albumRoleService;
        public ShareAlbumCommand(IAlbumService albumService, IUserService userService, IAlbumRoleService albumRoleService)
        {
            this.albumService = albumService;
            this.userService = userService;
            this.albumRoleService = albumRoleService;
        }

        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer
        public string Execute(string[] data)
        {
            int albumId = int.Parse(data[0]);
            string username = data[1];
            string inputRole = data[2];
            bool roleIsValid = Enum.TryParse(inputRole, out Role role);

            if (!roleIsValid)
            {
                throw new ArgumentException($"Permission must be either “Owner” or “Viewer”!");
            }

            bool albumExists = this.albumService.Exists(albumId);
            if (!albumExists)
            {
                throw new ArgumentException($"Album {albumId} not found!");
            }

            bool userExists = this.userService.Exists(username);
            if (!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            User user = this.userService.ByUsername<User>(username);
            Album album = this.albumService.ById<Album>(albumId);

            AlbumRole albumRole = this.albumRoleService.PublishAlbumRole(albumId, user.Id, inputRole);
            
            return $"Username {username} added to album {album.Name} ({role.ToString()})";
        }
    }
}
