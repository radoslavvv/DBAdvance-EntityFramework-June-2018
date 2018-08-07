using System;
using Workshop.App.Core;

namespace Workshop.App
{
    public class StartUp
    {
        public static void Main()
        {
            AuthenticationManager authenticationManager = new AuthenticationManager();
            CommandDispatcher dispatcher = new CommandDispatcher(authenticationManager);
            Engine engine = new Engine(dispatcher);

            engine.Run();
        }
    }
}
