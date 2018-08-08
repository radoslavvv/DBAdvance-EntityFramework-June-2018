using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workshop.App.Core.Command;
using Workshop.Data;

namespace Workshop.App.Core
{
    public class CommandDispatcher
    {
        private readonly AuthenticationManager authenticationManager;
        public CommandDispatcher(AuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }

        public string Dispatch(string input)
        {
            string[] arguments = input.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            string commandName = arguments.Length > 0 ? arguments[0] : string.Empty;

            arguments = arguments.Skip(1).ToArray();

            switch (commandName)
            {
                case "RegisterUser":
                    RegisterUserCommand register = new RegisterUserCommand(authenticationManager);
                    return register.Execute(arguments);
                case "Login":
                    LogInCommand login = new LogInCommand(authenticationManager);
                    return login.Execute(arguments);
                case "Logout":
                    LogOutCommand logout = new LogOutCommand(authenticationManager);
                    return logout.Execute(arguments);
                case "DeleteUser":
                    DeleteUserCommand delete = new DeleteUserCommand(authenticationManager);
                    return delete.Execute(arguments);
                case "CreateEvent":
                    CreateEventCommand createEvent = new CreateEventCommand(authenticationManager);
                    return createEvent.Execute(arguments);
                case "CreateTeam":
                    CreateTeamCommand createTeam = new CreateTeamCommand(authenticationManager);
                    return createTeam.Execute(arguments);
                case "InviteToTeam":
                    InviteToTeamCommand inviteToTeam = new InviteToTeamCommand(authenticationManager);
                    return inviteToTeam.Execute(arguments);
                case "AcceptInvite":
                    AcceptInviteCommand acceptInvite = new AcceptInviteCommand(authenticationManager);
                    return acceptInvite.Execute(arguments);
                case "DeclineInvite":
                    DeclineInviteCommand declineInvite = new DeclineInviteCommand(authenticationManager);
                    return declineInvite.Execute(arguments);
                case "KickMember":
                    KickMemberCommand kickMember = new KickMemberCommand(authenticationManager);
                    return kickMember.Execute(arguments);
                case "Disband":
                    DisbandCommand disband = new DisbandCommand(authenticationManager);
                    return disband.Execute(arguments);
                case "AddTeamTo":
                    AddTeamToCommand addTeamToCommand = new AddTeamToCommand(authenticationManager);
                    return addTeamToCommand.Execute(arguments);
                case "ShowEvent":
                    ShowEventCommand showEvent = new ShowEventCommand(authenticationManager);
                    return showEvent.Execute(arguments);
                case "ShowTeam":
                    ShowTeamCommand showTeam = new ShowTeamCommand(authenticationManager);
                    return showTeam.Execute(arguments);
                case "Exit":
                    ExitCommand exitCommand = new ExitCommand();
                    return exitCommand.Execute(arguments);
                default:
                    throw new NotSupportedException($"Command {commandName} is not supported!");
            }
        }
    }
}
