using CommandSystem;
using PlayerRoles;
using PluginAPI.Core;
using System;
using System.Linq;

namespace AdminTools.Commands.TeleportX
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public sealed class TeleportX : ParentCommand
    {
        public TeleportX() => LoadGeneratedCommands();

        public override string Command => "teleportx";

        public override string[] Aliases { get; } = { "tpx" };

        public override string Description => "Teleports all users or a user to another user";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission(PlayerPermissions.PlayersManagement))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "Usage: teleportx (People teleported: (player id / name) or (all / *)) (Teleported to: (player id / name) or (all / *))";
                return false;
            }

            int id;

            switch (arguments.At(0))
            {
                case "*":
                case "all":
                    Player ply = int.TryParse(arguments.At(1), out id) ? Player.GetPlayers().FirstOrDefault(x => x.PlayerId == id) : Player.GetByName(arguments.At(1));
                    if (ply == null)
                    {
                        response = $"Player not found: {arguments.At(1)}";
                        return false;
                    }


                    foreach (Player plyr in Player.GetPlayers().Where(plyr => plyr.Role != RoleTypeId.Spectator && ply.Role != RoleTypeId.None))
                        plyr.Position = ply.Position;

                    response = $"Everyone has been teleported to Player {ply.Nickname}";
                    return true;
                default:
                    Player pl = int.TryParse(arguments.At(0), out id) ? Player.GetPlayers().FirstOrDefault(x => x.PlayerId == id) : Player.GetByName(arguments.At(0));
                    if (pl == null)
                    {
                        response = $"Player not found: {arguments.At(0)}";
                        return false;
                    }

                    Player plr = int.TryParse(arguments.At(1), out id) ? Player.GetPlayers().FirstOrDefault(x => x.PlayerId == id) : Player.GetByName(arguments.At(1));
                    if (plr == null)
                    {
                        response = $"Player not found: {arguments.At(1)}";
                        return false;
                    }

                    pl.Position = plr.Position;
                    response = $"Player {pl.Nickname} has been teleported to Player {plr.Nickname}";
                    return true;
            }
        }
    }
}
