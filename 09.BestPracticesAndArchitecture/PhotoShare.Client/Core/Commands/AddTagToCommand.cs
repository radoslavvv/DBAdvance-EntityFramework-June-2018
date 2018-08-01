namespace PhotoShare.Client.Core.Commands
{
    using System;
    using Contracts;
    using PhotoShare.Client.Core.Validation;
    using PhotoShare.Models;
    using PhotoShare.Services.Contracts;

    [CredentialsAttribute(true)]
    public class AddTagToCommand : ICommand
    {
        private readonly ITagService tagService;
        private readonly IAlbumService albumService;
        private readonly IAlbumTagService albumTagService;

        public AddTagToCommand(ITagService tagService, IAlbumService albumService, IAlbumTagService albumTagService)
        {
            this.tagService = tagService;
            this.albumService = albumService;
            this.albumTagService = albumTagService;
        }

        // AddTagTo <albumName> <tag>
        public string Execute(string[] args)
        {
            string albumName = args[0];
            string tagName = args[1];

            bool albumExists = this.albumService.Exists(albumName);
            bool tagExists = this.tagService.Exists(tagName);

            if (!albumExists || !tagExists)
            {
                throw new ArgumentException($"Either tag or album do not exist!");
            }

            Album album = this.albumService.ByName<Album>(albumName);
            Tag tag = this.tagService.ByName<Tag>(tagName);

            this.albumTagService.AddTagTo(album.Id, tag.Id);

            return $"Tag {tagName} added to {albumName}!";
        }
    }
}
