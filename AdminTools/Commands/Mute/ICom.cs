using CommandSystem;
using System;
using System.Linq;
using PluginAPI.Core;

namespace AdminTools.Commands.Mute
{
    public class Com : ICommand
    {
        public string Command { get; } = "icom";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Intercom mutes everyone in the server";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission(PlayerPermissions.PlayersManagement))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: pmute icom";
                return false;
            }

            foreach (var player in Player.GetPlayers().Where(player => !player.ReferenceHub.serverRoles.RemoteAdmin && player.IsIntercomMuted))
                player.IntercomUnmute(true);

            response = "Everyone from the server who is not a staff has been intercom muted";
            return true;
        }
    }
}
