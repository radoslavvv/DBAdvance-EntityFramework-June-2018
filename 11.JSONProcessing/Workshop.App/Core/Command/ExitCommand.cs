using System;
using System.Collections.Generic;
using System.Text;

namespace Workshop.App.Core.Command
{
    public class ExitCommand : ICommand
    {
        public string Execute(string[] arguments)
        {
            Environment.Exit(0);

            return "Bye!";
        }
    }
}
