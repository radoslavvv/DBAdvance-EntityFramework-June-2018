using Shop.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.App.Core.Commands
{
    public abstract class Command : ICommand
    {
        public abstract string Execute(ShopDbContext context, List<string> arguments);
    }
}
