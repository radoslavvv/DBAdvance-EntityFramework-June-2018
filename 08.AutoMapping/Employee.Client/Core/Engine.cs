using Shop.App.Core.Commands;
using Shop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shop.App.Core
{
    public class Engine
    {
        private ShopDbContext context;

        public Engine(ShopDbContext context)
        {
            this.context = context;
        }

        public void StartCommnadInterepreting()
        {
            List<string> input = Console.ReadLine()
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            while (input[0] != "Exit")
            {
                string commandName = input[0];

                List<string> arguments = input.Skip(1).ToList();

                Type commandType = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .FirstOrDefault(t => t.Name == commandName + "Command");

                if (commandType == null)
                {
                    Console.WriteLine("Invalid command");
                    continue;
                }

                ICommand cmd = (ICommand)Activator.CreateInstance(commandType);

                string result = cmd.Execute(this.context, arguments);
                Console.WriteLine(result);
            }
        }
    }
}
