using CommandSystem;
using RemoteAdmin;
using System;
using System.Linq;
using PluginAPI.Core;
using UnityEngine;

namespace AdminTools.Commands.RandomTeleport
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class RandomTeleport : ParentCommand
    {
        public RandomTeleport() => LoadGeneratedCommands();

        public override string Command { get; } = "randomtp";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Randomly teleports a user or all users to a random room in the facility";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "This command is not available at this time.";
            return false;

            // if (!CommandProcessor.CheckPermissions(((CommandSender)sender), "randomtp", PlayerPermissions.PlayersManagement, "AdminTools", false))
            // {
            //     response = "You do not have permission to use this command";
            //     return false;
            // }
            //
            // if (arguments.Count != 1)
            // {
            //     response = "Usage: randomtp ((player id / name) or (all / *))";
            //     return false;
            // }
            //
            // switch (arguments.At(0))
            // {
            //     case "*":
            //     case "all":
            //         foreach (Player ply in Player.GetPlayers())
            //         {
            //             Room randRoom = Room.List.ElementAt(Plugin.NumGen.Next(0, Room.List.Count()));
            //             ply.Position = randRoom.Position + Vector3.up;
            //         }
            //
            //         response = $"Everyone was teleported to a random room in the facility";
            //         return true;
            //     default:
            //         Player pl = int.TryParse(arguments.At(0), out int id) ? Player.GetPlayers().FirstOrDefault(x => x.PlayerId == id) : Player.GetByName(arguments.At(0));
            //         if (pl == null)
            //         {
            //             response = $"Player not found: {arguments.At(0)}";
            //             return false;
            //         }
            //
            //         Room rand = Room.List.ElementAt(Plugin.NumGen.Next(0, Room.List.Count()));
            //         pl.Position = rand.Position + Vector3.up;
            //
            //         response = $"Player {pl.Nickname} was teleported to {rand.Name}";
            //         return true;
            // }
        }
    }
}
