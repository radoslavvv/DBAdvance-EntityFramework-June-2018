using AutoMapper.QueryableExtensions;
using PhotoShare.Data;
using PhotoShare.Models;
using PhotoShare.Models.Enums;
using PhotoShare.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PhotoShare.Services
{
    public class AlbumService : IAlbumService
    {
        private IEnumerable<TModel> By<TModel>(Func<Album, bool> predicate)
            => this.context
                .Albums
                .Where(predicate)
                .AsQueryable()
                .ProjectTo<TModel>();

        private PhotoShareContext context;
        public AlbumService(PhotoShareContext context)
        {
            this.context = context;
        }

        public Album Create(int userId, string albumTitle, string BgColor, string[] tags)
        {
            Album album = new Album()
            {
                Name = albumTitle,
                BackgroundColor = Enum.Parse<Color>(BgColor, true)
            };

            this.context.Albums.Add(album);
            this.context.SaveChanges();

            AlbumRole albumRole = new AlbumRole()
            {
                UserId = userId,
                Album = album
            };

            this.context.AlbumRoles.Add(albumRole);
            this.context.SaveChanges();

            List<AlbumTag> albumTagsToAdd = new List<AlbumTag>();
            foreach (string tag in tags)
            {
                int currentTagId = this.context
                    .Tags
                    .FirstOrDefault(t => t.Name == tag)
                    .Id;

                AlbumTag albumTag = new AlbumTag()
                {
                    Album = album,
                    TagId = currentTagId
                };

                albumTagsToAdd.Add(albumTag);
            }

            this.context.AlbumTags.AddRange(albumTagsToAdd);
            this.context.SaveChanges();

            return album;
        }

        public Album AddTag(string albumnName, string tagName)
        {
            Album album = ByName<Album>(albumnName);
            Tag tag = this.context.Tags.FirstOrDefault(t => t.Name == tagName);

            AlbumTag albumTag = new AlbumTag
            {
                Album = album,
                Tag = tag
            };

            album.AlbumTags.Add(albumTag);
            this.context.SaveChanges();

            return album;
        }

        public Album AddRole(int albumId, string username, Role role)
        {
            Album album = ById<Album>(albumId);
            User user = this.context.Users.FirstOrDefault(t => t.Username == username);

            AlbumRole albumRole = new AlbumRole
            {
                Album = album,
                User = user,
                Role = role
            };

            album.AlbumRoles.Add(albumRole);
            this.context.SaveChanges();

            return album;
        }
        public TModel ById<TModel>(int id) => By<TModel>(i => i.Id == id).SingleOrDefault();

        public TModel ByName<TModel>(string name) => By<TModel>(i => i.Name == name).SingleOrDefault();

        public bool Exists(int id) => ById<Album>(id) != null;

        public bool Exists(string name) => ByName<Album>(name) != null;
    }
}
