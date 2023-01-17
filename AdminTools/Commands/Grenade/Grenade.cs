using CommandSystem;
using PlayerRoles;
using PluginAPI.Core;
using System;
using System.Linq;

namespace AdminTools.Commands.Grenade
{

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Grenade : ParentCommand
    {
        public Grenade() => LoadGeneratedCommands();

        public override string Command => "grenade";

        public override string[] Aliases { get; } = { "gn" };

        public override string Description => "Spawns a frag/flash/scp018 grenade on a user or users";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission(PlayerPermissions.GivingItems))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count is < 2 or > 3)
            {
                response = "Usage: grenade ((player id / name) or (all / *)) (GrenadeType) (grenade time)";
                return false;
            }

            if (!Enum.TryParse(arguments.At(1), true, out ItemType type) || type != ItemType.SCP018 && type != ItemType.GrenadeHE && type != ItemType.GrenadeFlash)
            {
                response = $"Invalid value for grenade type: {arguments.At(1)}";
                return false;
            }

            if (!float.TryParse(arguments.At(2), out float fuseTime))
            {
                response = $"Invalid fuse time for grenade: {arguments.At(2)}";
                return false;
            }

            switch (arguments.At(0))
            {
                case "*":
                case "all":
                    if (type == ItemType.SCP018)
                        Cassie.Message("pitch_1.5 xmas_bouncyballs", true, false);

                    foreach (Player player in Player.GetPlayers().Where(player => player.Role != RoleTypeId.Spectator))
                        Handlers.CreateThrowable(type).SpawnActive(player.Position, fuseTime);

                    break;
                default:
                    Player ply = int.TryParse(arguments.At(0), out int id) ? Player.GetPlayers().FirstOrDefault(x => x.PlayerId == id) : Player.GetByName(arguments.At(0));
                    if (ply is null)
                    {
                        response = $"Player {arguments.At(0)} not found.";
                        return false;
                    }

                    Handlers.CreateThrowable(type).SpawnActive(ply.Position, fuseTime);
                    break;
            }

            response = $"Grenade has been sent to {arguments.At(0)}";
            return true;
        }
    }
}
