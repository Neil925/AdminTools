using CommandSystem;
using System;

namespace AdminTools.Commands.Cleanup
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Clear : ParentCommand
    {
        public Clear() => LoadGeneratedCommands();

        public override string Command { get; } = "clear";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Clears up items and ragdolls from the server";

        public override void LoadGeneratedCommands() 
        {
            RegisterCommand(new Items());
            RegisterCommand(new Ragdolls());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission(PlayerPermissions.FacilityManagement))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            response = "Invalid subcommand. Available ones: items, ragdolls";
            return false;
        }
    }
}
