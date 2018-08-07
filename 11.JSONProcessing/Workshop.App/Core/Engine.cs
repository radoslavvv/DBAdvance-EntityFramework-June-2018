using System;
using System.Collections.Generic;
using System.Text;

namespace Workshop.App.Core
{
    public class Engine
    {
        private readonly CommandDispatcher _dispatcher;
        public Engine(CommandDispatcher dispatcher)
        {
            this._dispatcher = dispatcher;
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    string input = Console.ReadLine();
                    Console.WriteLine(this._dispatcher.Dispatch(input));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().Message);
                }
            }
        }
    }
}
