using PhotoShare.Client.Core.Contracts;
using PhotoShare.Client.Core.Validation;
using PhotoShare.Client.Utilities;
using PhotoShare.Models;
using PhotoShare.Services.Contracts;
using System;

namespace PhotoShare.Client.Core.Commands
{
    [CredentialsAttribute(true)]
    public class AddTagCommand : ICommand
    {
        private readonly ITagService tagService;

        public AddTagCommand(ITagService tagService)
        {
            this.tagService = tagService;
        }

        public string Execute(string[] args)
        {
            string tagName = args[0];

            bool tagExists = this.tagService.Exists(tagName);
            if (tagExists)
            {
                throw new ArgumentException($"Tag {tagName} exists!");
            }

            tagName = tagName.ValidateOrTransform();

            Tag tag = this.tagService.AddTag(tagName);

            return $"Tag {tagName} was added successfully!";
        }
    }
}
