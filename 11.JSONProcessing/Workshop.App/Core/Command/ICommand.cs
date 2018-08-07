using System;
using System.Collections.Generic;
using System.Text;

namespace Workshop.App.Core.Command
{
    public interface ICommand
    {
        string Execute(string[] arguments);
    }
}
