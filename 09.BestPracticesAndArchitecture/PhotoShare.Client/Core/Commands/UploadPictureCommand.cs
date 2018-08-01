namespace PhotoShare.Client.Core.Commands
{
    using System;
    using Dtos;
    using Contracts;
    using Services.Contracts;
    using PhotoShare.Models;
    using PhotoShare.Client.Core.Validation;

    [CredentialsAttribute(true)]
    public class UploadPictureCommand : ICommand
    {
        private readonly IPictureService pictureService;
        private readonly IAlbumService albumService;

        public UploadPictureCommand(IPictureService pictureService, IAlbumService albumService)
        {
            this.pictureService = pictureService;
            this.albumService = albumService;
        }

        // UploadPicture <albumName> <pictureTitle> <pictureFilePath>
        public string Execute(string[] data)
        {
            string albumName = data[0];
            string pictureTitle = data[1];
            string path = data[2];

            bool albumExists = this.albumService.Exists(albumName);
            if (!albumExists)
            {
                throw new ArgumentException($"Album {albumName} not found!");
            }

            int albumId = this.albumService.ByName<AlbumDto>(albumName).Id;

            Picture picture = this.pictureService.Create(albumId, pictureTitle, path);

            return $"Picture {pictureTitle} added to {albumName}!";
        }
    }
}
