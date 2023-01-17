using AdminTools.Components;
using CommandSystem;
using NorthwoodLib.Pools;
using PluginAPI.Core;
using System;
using System.Linq;
using System.Text;
using Object = UnityEngine.Object;

namespace AdminTools.Commands.InstantKill
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class InstantKill : ParentCommand
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

            int id;

            switch (arguments.At(0))
            {
                case "clear":
                    if (arguments.Count != 1)
                    {
                        response = "Usage: instakill clear";
                        return false;
                    }

                    foreach (Player ply in Plugin.IkHubs.Keys)
                        if (ply.ReferenceHub.TryGetComponent(out InstantKillComponent ikCom))
                            Object.Destroy(ikCom);

                    response = "Instant killing has been removed from everyone";
                    return true;
                case "list":
                    if (arguments.Count != 1)
                    {
                        response = "Usage: instakill clear";
                        return false;
                    }

                    StringBuilder playerLister = StringBuilderPool.Shared.Rent(Plugin.IkHubs.Count != 0 ? "Players with instant killing on:\n" : "No players currently online have instant killing on");
                    if (Plugin.IkHubs.Count == 0)
                    {
                        response = playerLister.ToString();
                        return true;
                    }

                    foreach (Player ply in Plugin.IkHubs.Keys)
                    {
                        playerLister.Append(ply.Nickname);
                        playerLister.Append(", ");
                    }

                    string msg = playerLister.ToString().Substring(0, playerLister.ToString().Length - 2);
                    StringBuilderPool.Shared.Return(playerLister);
                    response = msg;
                    return true;
                case "remove":
                    if (arguments.Count != 2)
                    {
                        response = "Usage: instakill remove (player id / name)";
                        return false;
                    }

                    Player pl = int.TryParse(arguments.At(1), out id) ? Player.GetPlayers().FirstOrDefault(x => x.PlayerId == id) : Player.GetByName(arguments.At(1));
                    if (pl == null)
                    {
                        response = $"Player not found: {arguments.At(1)}";
                        return false;
                    }

                    if (pl.ReferenceHub.TryGetComponent(out InstantKillComponent ikComponent))
                    {
                        Plugin.IkHubs.Remove(pl);
                        Object.Destroy(ikComponent);
                        response = $"Instant killing is off for {pl.Nickname} now";
                    }
                    else
                        response = $"Player {pl.Nickname} does not have the ability to instantly kill others";
                    return true;
                case "*":
                case "all":
                    if (arguments.Count != 1)
                    {
                        response = "Usage: instakill all / *";
                        return false;
                    }

                    foreach (Player ply in Player.GetPlayers().Where(ply => !ply.ReferenceHub.TryGetComponent(out InstantKillComponent _)))
                        ply.ReferenceHub.gameObject.AddComponent<InstantKillComponent>();

                    response = "Everyone on the server can instantly kill other users now";
                    return true;
                default:
                    if (arguments.Count != 1)
                    {
                        response = "Usage: instakill (player id / name)";
                        return false;
                    }

                    Player plyr = int.TryParse(arguments.At(0), out id) ? Player.GetPlayers().FirstOrDefault(x => x.PlayerId == id) : Player.GetByName(arguments.At(0));
                    if (plyr == null)
                    {
                        response = $"Player not found: {arguments.At(0)}";
                        return false;
                    }

                    if (!plyr.ReferenceHub.TryGetComponent(out InstantKillComponent ikComp))
                    {
                        plyr.GameObject.AddComponent<InstantKillComponent>();
                        response = $"Instant killing is on for {plyr.Nickname}";
                    }
                    else
                    {
                        Object.Destroy(ikComp);
                        response = $"Instant killing is off for {plyr.Nickname}";
                    }
                    return true;
            }
        }
    }
}
