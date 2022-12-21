using System;
using System.Linq;
using CommandSystem;
using PluginAPI.Core;

namespace AdminTools.Commands.Tags
{
    public class Show : ICommand
    {
        public string Command { get; } = "show";

        public string[] Aliases { get; } = new string[] { };

        public string Description { get; } = "Shows staff tags on the server";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission(PlayerPermissions.SetGroup))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: tags show";
                return false;
            }

            foreach (var player in Player.GetPlayers().Where(player => player.ReferenceHub.serverRoles.RemoteAdmin && !player.ReferenceHub.serverRoles.RaEverywhere && player.IsBadgeHidden()))
                player.SetBadgeHidden(false);

            response = "All staff tags are now visible";
            return true;
        }
    }
}
