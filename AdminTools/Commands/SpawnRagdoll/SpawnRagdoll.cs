using CommandSystem;
using MEC;
using System;
using System.Linq;
using PlayerRoles;
using PluginAPI.Core;

namespace AdminTools.Commands.SpawnRagdoll
{
    using System.Collections.Generic;
    using Mirror;
    using PlayerStatsSystem;
    using UnityEngine;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class SpawnRagdoll : ParentCommand
    {
        public SpawnRagdoll() => LoadGeneratedCommands();

        public override string Command { get; } = "spawnragdoll";

        public override string[] Aliases { get; } = new string[] { "ragdoll", "rd", "rag", "doll" };

        public override string Description { get; } = "Spawns a specified number of ragdolls on a user";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission(PlayerPermissions.WarheadEvents))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count != 3)
            {
                response = "Usage: spawnragdoll ((player id / name) or (all / *)) (RoleTypeId) (amount)";
                return false;
            }

            if (!Enum.TryParse(arguments.At(1), true, out RoleTypeId type))
            {
                response = $"Invalid RoleTypeId for ragdoll: {arguments.At(1)}";
                return false;
            }

            if (!int.TryParse(arguments.At(2), out int amount))
            {
                response = $"Invalid amount of ragdolls to spawn: {arguments.At(2)}";
                return false;
            }

            switch (arguments.At(0))
            {
                case "*":
                case "all":
                    foreach (var player in Player.GetPlayers().Where(player => player.Role != RoleTypeId.Spectator))
                    {
                        Timing.RunCoroutine(EventHandlers.SpawnBodies(player, type, amount));
                    }

                    break;
                default:
                    Player ply = int.TryParse(arguments.At(0), out int id) ? Player.GetPlayers().FirstOrDefault(x => x.PlayerId == id) : Player.GetByName(arguments.At(0));
                    if (ply is null)
                    {
                        response = $"Player {arguments.At(0)} not found.";
                        return false;
                    }

                    Timing.RunCoroutine(EventHandlers.SpawnBodies(ply, type, amount));

                    break;
            }

            response = $"{amount} {type} ragdoll(s) have been spawned on {arguments.At(0)}.";
            return true;
        }
    }
}
