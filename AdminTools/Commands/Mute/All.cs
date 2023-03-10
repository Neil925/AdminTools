using CommandSystem;
using System;
using System.Linq;
using PluginAPI.Core;

namespace AdminTools.Commands.Mute
{
    public class All : ICommand
    {
        public string Command { get; } = "all";

        public string[] Aliases { get; } = new string[] { "*" };

        public string Description { get; } = "Mutes everyone from speaking at all in the server";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission(PlayerPermissions.PlayersManagement))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: pmute all";
                return false;
            }

            foreach (var player in Player.GetPlayers().Where(player => !player.ReferenceHub.serverRoles.RemoteAdmin && player.IsMuted))
                player.Unmute(true);

            response = "Everyone from the server who is not a staff has been muted completely";
            return true;
        }
    }
}
