﻿namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using PhotoShare.Client.Core.Validation;
    using PhotoShare.Client.Utilities;
    using PhotoShare.Models;
    using PhotoShare.Models.Enums;
    using Services.Contracts;

    [CredentialsAttribute(true)]
    public class CreateAlbumCommand : ICommand
    {
        private readonly IAlbumService albumService;
        private readonly IUserService userService;
        private readonly ITagService tagService;

        public CreateAlbumCommand(IAlbumService albumService, IUserService userService, ITagService tagService)
        {
            this.albumService = albumService;
            this.userService = userService;
            this.tagService = tagService;
        }

        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>
        public string Execute(string[] data)
        {
            string username = data[0];
            string albumTitle = data[1];
            string bgColor = data[2];
            string[] tags = data.Skip(3).ToArray();

            bool userExists = this.userService.Exists(username);
            if (!userExists)
            {
                throw new ArgumentException($"User {username} not found!");
            }

            bool albumExists = this.albumService.Exists(albumTitle);
            if (albumExists)
            {
                throw new ArgumentException($"Album {albumTitle} already created!");
            }

            bool bgColorIsvalid = Enum.TryParse(bgColor, out Color result);
            if (!bgColorIsvalid)
            {
                throw new ArgumentException($"Color {bgColor} not found!");
            }

            for (int i = 0; i < tags.Length; i++)
            {
                tags[i] = tags[i].ValidateOrTransform();

                bool currentTagExists = this.tagService.Exists(tags[i]);
                if (!currentTagExists)
                {
                    throw new ArgumentException("Invalid tags!");
                }
            }

            int userId = this.userService.ByUsername<User>(username).Id;
            this.albumService.Create(userId, albumTitle, bgColor, tags);

            return $"Album {albumTitle} successfully created!";
        }
    }
}
