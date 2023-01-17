using CommandSystem;
using MEC;
using PluginAPI.Core;
using System;
using System.Linq;

namespace AdminTools.Commands.Jail
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public sealed class Jail : ParentCommand
    {
        public Jail() => LoadGeneratedCommands();

        public override string Command => "jail";

        public override string[] Aliases { get; } = { };

        public override string Description => "Jails or unjails a user";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission(PlayerPermissions.KickingAndShortTermBanning))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Usage: jail (player id / name)";
                return false;
            }

            Player ply = int.TryParse(arguments.At(0), out int id) ? Player.GetPlayers().FirstOrDefault(x => x.PlayerId == id) : Player.GetByName(arguments.At(0));

            if (ply == null)
            {
                response = $"Player not found: {arguments.At(0)}";
                return false;
            }

            if (Plugin.JailedPlayers.Any(j => j.UserId == ply.UserId))
            {
                try
                {
                    Timing.RunCoroutine(EventHandlers.DoUnJail(ply));
                    response = $"Player {ply.Nickname} has been unjailed now";
                }
                catch (Exception e)
                {
                    Log.Error($"{e}");
                    response = "Command failed. Check server log.";
                    return false;
                }
            }
            else
            {
                Timing.RunCoroutine(EventHandlers.DoJail(ply));
                response = $"Player {ply.Nickname} has been jailed now";
            }
            return true;
        }
    }
}
