using CommandSystem;
using System;
using System.Linq;
using PluginAPI.Core;

namespace AdminTools.Commands.Mute
{
    public class RoundStart : ICommand
    {
        public string Command { get; } = "roundstart";

        public string[] Aliases { get; } = new string[] { "rs" };

        public string Description { get; } = "Mutes everyone from speaking until the round starts.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission(PlayerPermissions.PlayersManagement))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: pmute roundstart";
                return false;
            }

            if (Round.IsRoundStarted)
            {
                response = "You cannot use this command after the round has started!";
                return false;
            }

            foreach (var player in Player.GetPlayers().Where(player => !player.IsMuted && !player.ReferenceHub.serverRoles.RemoteAdmin))
            {
                player.Mute();
                Plugin.RoundStartMutes.Add(player);
            }

            response = "All non-staff players have been muted until the round starts.";
            return true;
        }
    }
}
