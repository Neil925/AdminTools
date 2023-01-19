using CommandSystem;
using PlayerRoles;
using PluginAPI.Core;
using System;
using System.Linq;
using UnityEngine;

namespace AdminTools.Commands.Size
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Size : ParentCommand
    {
        public Size() => LoadGeneratedCommands();

        public override string Command => "size";

        public override string[] Aliases { get; } =
            { };

        public override string Description => "Sets the size of all users or a user";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission(PlayerPermissions.Effects))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count >= 1)
                return arguments.At(0) switch
                {
                    "reset" => Reset(out response),
                    "*" or "all" => All(arguments, out response),
                    _ => HandleDefault(arguments, out response)
                };
            response = "Usage:\nsize (player id / name) or (all / *)) (x value) (y value) (z value)" +
                "\nsize reset";
            return false;

        }
        private static bool HandleDefault(ArraySegment<string> arguments, out string response)
        {
            if (arguments.Count < 4)
            {
                response = "Usage: size (player id / name) (x) (y) (z)";
                return false;
            }

            Player p = Extensions.GetPlayer(arguments.At(0));
            if (p == null)
            {
                response = $"Player not found: {arguments.At(0)}";
                return false;
            }

            if (!float.TryParse(arguments.At(1), out float x))
            {
                response = $"Invalid value for x size: {arguments.At(1)}";
                return false;
            }

            if (!float.TryParse(arguments.At(2), out float y))
            {
                response = $"Invalid value for y size: {arguments.At(2)}";
                return false;
            }

            if (!float.TryParse(arguments.At(3), out float z))
            {
                response = $"Invalid value for z size: {arguments.At(3)}";
                return false;
            }

            EventHandlers.SetPlayerScale(p, new Vector3(x, y, z));
            response = $"Player {p.Nickname}'s scale has been set to {x} {y} {z}";
            return true;
        }
        private static bool All(ArraySegment<string> arguments, out string response)
        {
            if (arguments.Count < 4)
            {
                response = "Usage: size (all / *) (x) (y) (z)";
                return false;
            }

            if (!float.TryParse(arguments.At(1), out float x))
            {
                response = $"Invalid value for x size: {arguments.At(1)}";
                return false;
            }

            if (!float.TryParse(arguments.At(2), out float y))
            {
                response = $"Invalid value for y size: {arguments.At(2)}";
                return false;
            }

            if (!float.TryParse(arguments.At(3), out float z))
            {
                response = $"Invalid value for z size: {arguments.At(3)}";
                return false;
            }

            foreach (Player p in Player.GetPlayers().Where(ply => ply.Role is not (RoleTypeId.Spectator or RoleTypeId.None)))
            {
                EventHandlers.SetPlayerScale(p, new Vector3(x, y, z));
            }

            response = $"Everyone's scale has been set to {x} {y} {z}";
            return true;
        }
        private static bool Reset(out string response)
        {
            foreach (Player p in Player.GetPlayers().Where(ply => ply.Role is not (RoleTypeId.Spectator or RoleTypeId.None)))
            {
                EventHandlers.SetPlayerScale(p, new Vector3(1, 1, 1));
            }

            response = "Everyone's size has been reset";
            return true;
        }
    }
}
