namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;

    public class ExitCommand : ICommand
    {
        public string Execute(string[] data)
        {
            return "Good Bye!";
        }
    }
}
