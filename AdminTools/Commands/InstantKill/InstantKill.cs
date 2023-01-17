using CommandSystem;
using NorthwoodLib.Pools;
using System;
using System.Linq;
using System.Text;

namespace AdminTools.Commands.InstantKill
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public sealed class InstantKill : ParentCommand
    {
        public InstantKill() => LoadGeneratedCommands();

        public override string Command => "instakill";

        public override string[] Aliases { get; } = { "ik" };

        public override string Description => "Manage instant kill properties for users";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission(PlayerPermissions.ForceclassWithoutRestrictions))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Usage:\ninstakill ((player id / name) or (all / *))" +
                    "\ninstakill clear" +
                    "\ninstakill list" +
                    "\ninstakill remove (player id / name)";
                return false;
            }

            switch (arguments.At(0))
            {
                case "clear":
                    if (arguments.Count < 1)
                    {
                        response = "Usage: instakill clear";
                        return false;
                    }

                    foreach (AtPlayer ply in Extensions.Players)
                        ply.InstantKillEnabled = false;

                    response = "Instant killing has been removed from everyone";
                    return true;
                case "list":
                {
                    if (arguments.Count < 1)
                    {
                        response = "Usage: instakill clear";
                        return false;
                    }

                    AtPlayer[] ikPlayers = Extensions.Players.Where(p => p.InstantKillEnabled).ToArray();
                    StringBuilder playerLister = StringBuilderPool.Shared.Rent(ikPlayers.Length != 0 ? "Players with instant killing on:\n" : "No players currently online have instant killing on");
                    if (ikPlayers.Length == 0)
                    {
                        response = playerLister.ToString();
                        return true;
                    }

                    foreach (AtPlayer ply in ikPlayers)
                    {
                        playerLister.Append(ply.Nickname);
                        playerLister.Append(", ");
                    }

                    string msg = playerLister.ToString().Substring(0, playerLister.ToString().Length - 2);
                    StringBuilderPool.Shared.Return(playerLister);
                    response = msg;
                    return true;
                }
                case "remove":
                {
                    if (arguments.Count < 2)
                    {
                        response = "Usage: instakill remove (player id / name)";
                        return false;
                    }

                    var pl = Extensions.GetPlayer(arguments.At(1));
                    if (pl == null)
                    {
                        response = $"Player not found: {arguments.At(1)}";
                        return false;
                    }

                    if (pl.InstantKillEnabled)
                    {
                        pl.InstantKillEnabled = false;
                        response = $"Instant killing turned off for {pl.Nickname}";
                    }
                    else
                        response = $"Player {pl.Nickname} does not have the ability to instantly kill others";
                    return true;
                }
                case "*":
                case "all":
                    if (arguments.Count < 1)
                    {
                        response = "Usage: instakill all / *";
                        return false;
                    }

                    foreach (AtPlayer ply in Extensions.Players.Where(ply => !ply.InstantKillEnabled))
                        ply.InstantKillEnabled = true;

                    response = "Everyone on the server can instantly kill other users now";
                    return true;
                default:
                    if (arguments.Count < 1)
                    {
                        response = "Usage: instakill (player id / name)";
                        return false;
                    }

                    AtPlayer p = Extensions.GetPlayer(arguments.At(0));
                    if (p == null)
                    {
                        response = $"Player not found: {arguments.At(0)}";
                        return false;
                    }

                    p.InstantKillEnabled = !p.InstantKillEnabled;
                    response = $"Instant killing is now {(p.InstantKillEnabled ? "on" : "off")} for {p.Nickname}";
                    return true;
            }
        }

    }
}
