using CommandSystem;
using InventorySystem.Items.ThrowableProjectiles;
using PlayerRoles;
using PluginAPI.Core;
using System;
using System.Linq;

namespace AdminTools.Commands.Explode
{

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public sealed class Explode : ParentCommand
    {
        public Explode() => LoadGeneratedCommands();

        public override string Command => "expl";

        public override string[] Aliases { get; } = { "boom" };

        public override string Description => "Explodes a specified user or everyone instantly";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission(PlayerPermissions.ForceclassToSpectator))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Usage: expl ((player id / name) or (all / *))";
                return false;
            }

            switch (arguments.At(0))
            {
                case "*":
                case "all":
                    if (arguments.Count < 1)
                    {
                        response = "Usage: expl (all / *)";
                        return false;
                    }

                    foreach (Player ply in Player.GetPlayers().Where(ply => ply.Role != RoleTypeId.Spectator && ply.Role != RoleTypeId.None))
                    {
                        ply.Kill("Exploded by admin.");
                        ThrowableItem grenade = Handlers.CreateThrowable(ItemType.GrenadeHE);
                        grenade.SpawnActive(ply.Position, .5f, ply);
                    }
                    response = "Everyone exploded, Hubert cannot believe you have done this";
                    return true;
                default:
                    if (arguments.Count < 1)
                    {
                        response = "Usage: expl (player id / name)";
                        return false;
                    }

                    Player pl = int.TryParse(arguments.At(0), out int id) ? Player.GetPlayers().FirstOrDefault(x => x.PlayerId == id) : Player.GetByName(arguments.At(0));
                    if (pl == null)
                    {
                        response = $"Invalid target to explode: {arguments.At(0)}";
                        return false;
                    }

                    if (pl.Role is RoleTypeId.Spectator or RoleTypeId.None)
                    {
                        response = $"Player \"{pl.Nickname}\" is not a valid class to explode";
                        return false;
                    }

                    pl.Kill("Exploded by admin.");
                    ThrowableItem gr = Handlers.CreateThrowable(ItemType.GrenadeHE);
                    gr.SpawnActive(pl.Position, .5f, pl);
                    response = $"Player \"{pl.Nickname}\" game ended (exploded)";
                    return true;
            }
        }
    }
}
