using System.Collections.Generic;
using Shop.Data;

namespace Shop.App.Core.Commands
{
    public interface ICommand
    {
        string Execute(ShopDbContext context, List<string> arguments);
    }
}