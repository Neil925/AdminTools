using AdminTools.Components;
using CommandSystem;
using NorthwoodLib.Pools;
using PluginAPI.Core;
using System;
using System.Linq;
using System.Text;
using Object = UnityEngine.Object;

namespace AdminTools.Commands.Regeneration
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Regeneration : ParentCommand
    {
        public Regeneration() => LoadGeneratedCommands();

        public override string Command => "reg";

        public override string[] Aliases { get; } = { };

        public override string Description => "Manages regeneration properties for users";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission(PlayerPermissions.PlayersManagement))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Usage:\nreg ((player id / name) or (all / *)) ((doors) or (all))" +
                    "\nreg clear" +
                    "\nreg list" +
                    "\nreg health (value)" +
                    "\nreg time (value)";
                return false;
            }

            switch (arguments.At(0))
            {
                case "clear":
                    if (arguments.Count != 1)
                    {
                        response = "Usage: reg clear";
                        return false;
                    }

                    foreach (Player ply in Plugin.RgnHubs.Keys)
                        if (ply.ReferenceHub.TryGetComponent(out RegenerationComponent rgCom))
                            Object.Destroy(rgCom);

                    response = "Regeneration has been removed from everyone";
                    return true;
                case "list":
                    if (arguments.Count != 1)
                    {
                        response = "Usage: regen list";
                        return false;
                    }

                    StringBuilder playerLister = StringBuilderPool.Shared.Rent(Plugin.RgnHubs.Count != 0 ? "Players with regeneration on:\n" : "No players currently online have regeneration on");
                    if (Plugin.RgnHubs.Count == 0)
                    {
                        response = playerLister.ToString();
                        return true;
                    }

                    foreach (Player ply in Plugin.RgnHubs.Keys)
                    {
                        playerLister.Append(ply.Nickname);
                        playerLister.Append(", ");
                    }

                    string msg = playerLister.ToString().Substring(0, playerLister.ToString().Length - 2);
                    StringBuilderPool.Shared.Return(playerLister);
                    response = msg;
                    return true;
                case "heal":
                    if (arguments.Count != 2)
                    {
                        response = "Usage: reg heal (value)";
                        return false;
                    }

                    if (!float.TryParse(arguments.At(1), out float healvalue) || healvalue < 0.05)
                    {
                        response = $"Invalid value for healing: {arguments.At(1)}";
                        return false;
                    }

                    Plugin.HealthGain = healvalue;
                    response = $"Players with regeneration will heal {healvalue} HP per interval";
                    return true;
                case "time":
                    if (arguments.Count != 2)
                    {
                        response = "Usage: reg time (value)";
                        return false;
                    }

                    if (!float.TryParse(arguments.At(1), out float healtime) || healtime < 0.05)
                    {
                        response = $"Invalid value for healing time interval: {arguments.At(1)}";
                        return false;
                    }

                    Plugin.HealthInterval = healtime;
                    response = $"Players with regeneration will heal every {healtime} seconds";
                    return true;
                case "*":
                case "all":
                    if (arguments.Count != 1)
                    {
                        response = "Usage: reg (all / *)";
                        return false;
                    }

                    foreach (Player ply in Player.GetPlayers().Where(ply => !ply.ReferenceHub.TryGetComponent(out RegenerationComponent _)))
                        ply.ReferenceHub.gameObject.AddComponent<RegenerationComponent>();

                    response = "Everyone on the server can regenerate health now";
                    return true;
                default:
                    if (arguments.Count != 1)
                    {
                        response = "Usage: reg (player id / name)";
                        return false;
                    }

                    Player pl = int.TryParse(arguments.At(0), out int id) ? Player.GetPlayers().FirstOrDefault(x => x.PlayerId == id) : Player.GetByName(arguments.At(0));

                    if (pl == null)
                    {
                        response = $"Player not found: {arguments.At(0)}";
                        return false;
                    }

                    if (!pl.ReferenceHub.TryGetComponent(out RegenerationComponent rgnComponent))
                    {
                        pl.GameObject.AddComponent<RegenerationComponent>();
                        response = $"Regeneration is on for {pl.Nickname}";
                    }
                    else
                    {
                        Object.Destroy(rgnComponent);
                        response = $"Regeneration is off for {pl.Nickname}";
                    }
                    return true;
            }
        }
    }
}
