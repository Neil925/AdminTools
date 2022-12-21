using CommandSystem;
using System;
using System.Linq;
using PlayerRoles;
using PluginAPI.Core;

namespace AdminTools.Commands.Ball
{
    using System.Collections.Generic;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Ball : ParentCommand
    {
        public Ball() => LoadGeneratedCommands();

        public override string Command { get; } = "ball";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Spawns a bouncy ball (SCP-018) on a user or all users";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "This command is not currently available.";
            return false;

            // if (!((CommandSender)sender).CheckPermission(PlayerPermissions.WarheadEvents))
            // {
            //     response = "You do not have permission to use this command";
            //     return false;
            // }
            //
            // if (arguments.Count != 1)
            // {
            //     response = "Usage: ball ((player id/ name) or (all / *))";
            //     return false;
            // }
            //
            // List<Player> players = new();
            // switch (arguments.At(0)) 
            // {
            //     case "*":
            //     case "all":
            //         players.AddRange(Player.GetPlayers().Where(pl => pl.Role is not (RoleTypeId.Spectator or RoleTypeId.None)));
            //
            //         break;
            //     default:
            //         Player ply = int.TryParse(arguments.At(0), out int id) ? Player.GetPlayers().FirstOrDefault(x => x.PlayerId == id) : Player.GetByName(arguments.At(0));
            //         if (ply == null)
            //         {
            //             response = $"Player not found: {arguments.At(0)}";
            //             return false;
            //         }
            //
            //         if (ply.Role == RoleTypeId.Spectator || ply.Role == RoleTypeId.None)
            //         {
            //             response = $"You cannot spawn a ball on that player right now";
            //             return false;
            //         }
            //
            //         players.Add(ply);
            //         break;
            // }
            //
            // response = players.Count == 1
            //     ? $"{players[0].Nickname} has received a bouncing ball!"
            //     : $"The balls are bouncing for {players.Count} players!";
            // if (players.Count > 1)
            //     Cassie.Message("pitch_1.5 xmas_bouncyballs", true, false);
            //
            // foreach (Player p in players)
            //     Handlers.CreateThrowable(ItemType.SCP018).SpawnActive(p.Position, owner: p);
            // return true;
        }
    }
}
