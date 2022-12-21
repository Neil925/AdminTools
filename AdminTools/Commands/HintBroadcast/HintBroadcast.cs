﻿using CommandSystem;
using NorthwoodLib.Pools;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayerRoles;
using PluginAPI.Core;

namespace AdminTools.Commands.HintBroadcast
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class HintBroadcast : ParentCommand
    {
        public HintBroadcast() => LoadGeneratedCommands();

        public override string Command { get; } = "hbc";

        public override string[] Aliases { get; } = { "broadcasthint" };

        public override string Description { get; } = "Broadcasts a message to either a user, a group, a role, all staff, or everyone";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "This command is not currently available.";
            return false;

            // if (!CommandProcessor.CheckPermissions(((CommandSender)sender), "hints", PlayerPermissions.Broadcasting, "AdminTools", false))
            // {
            //     response = "You do not have permission to use this command";
            //     return false;
            // }
            //
            // if (arguments.Count < 1)
            // {
            //     response = "Usage:\nhint (time) (message)" +
            //         "\nhbc user (player id / name) (time) (message)" +
            //         "\nhbc users (player id / name group (i.e.: 1,2,3 or hello,there,hehe)) (time) (message)" +
            //         "\nhbc group (group name) (time) (message)" +
            //         "\nhbc groups (list of groups (i.e.: owner,admin,moderator)) (time) (message)" +
            //         "\nhbc role (RoleTypeId) (time) (message)" +
            //         "\nhbc roles (RoleTypeId group (i.e.: ClassD,Scientist,NtfCadet)) (time) (message)" +
            //         "\nhbc (random / someone) (time) (message)" +
            //         "\nhbc (staff / admin) (time) (message)" +
            //         "\nhbc clearall";
            //     return false;
            // }
            //
            // switch (arguments.At(0))
            // {
            //     case "user":
            //         if (arguments.Count < 4)
            //         {
            //             response = "Usage: hbc user (player id / name) (time) (message)";
            //             return false;
            //         }
            //
            //         int id;
            //         
            //         Player ply = int.TryParse(arguments.At(1), out id) ? Player.GetPlayers().FirstOrDefault(x => x.PlayerId == id) : Player.GetByName(arguments.At(1));
            //         if (ply == null)
            //         {
            //             response = $"Player not found: {arguments.At(1)}";
            //             return false;
            //         }
            //
            //         if (!ushort.TryParse(arguments.At(2), out ushort time) && time <= 0)
            //         {
            //             response = $"Invalid value for duration: {arguments.At(2)}";
            //             return false;
            //         }
            //
            //         ply.ReceiveHint(EventHandlers.FormatArguments(arguments, 3), time);
            //         response = $"Hint sent to {ply.Nickname}";
            //         return true;
            //     case "users":
            //         if (arguments.Count < 4)
            //         {
            //             response = "Usage: hbc users (player id / name group (i.e.: 1,2,3 or hello,there,hehe)) (time) (message)";
            //             return false;
            //         }
            //
            //         string[] users = arguments.At(1).Split(',');
            //         List<Player> plyList = new();
            //         foreach (string s in users)
            //         {
            //             if (int.TryParse(s, out id) && Player.Get(id) != null)
            //                 plyList.Add(Player.Get(id));
            //             else if (Player.Get(s) != null)
            //                 plyList.Add(Player.Get(s));
            //         }
            //
            //         if (!ushort.TryParse(arguments.At(2), out ushort tme) && tme <= 0)
            //         {
            //             response = $"Invalid value for duration: {arguments.At(2)}";
            //             return false;
            //         }
            //
            //         foreach (Player p in plyList)
            //             p.ReceiveHint(EventHandlers.FormatArguments(arguments, 3), tme);
            //
            //
            //         StringBuilder builder = StringBuilderPool.Shared.Rent("Hint sent to players: ");
            //         foreach (Player p in plyList)
            //         {
            //             builder.Append("\"");
            //             builder.Append(p.Nickname);
            //             builder.Append("\"");
            //             builder.Append(" ");
            //         }
            //         string message = builder.ToString();
            //         StringBuilderPool.Shared.Return(builder);
            //         response = message;
            //         return true;
            //     case "group":
            //         if (arguments.Count < 4)
            //         {
            //             response = "Usage: hbc group (group) (time) (message)";
            //             return false;
            //         }
            //
            //         UserGroup broadcastGroup = ServerStatic.PermissionsHandler.GetGroup(arguments.At(1));
            //         if (broadcastGroup == null)
            //         {
            //             response = $"Invalid group: {arguments.At(1)}";
            //             return false;
            //         }
            //
            //         if (!ushort.TryParse(arguments.At(2), out ushort tim) && tim <= 0)
            //         {
            //             response = $"Invalid value for duration: {arguments.At(2)}";
            //             return false;
            //         }
            //
            //         foreach (var player in Player.GetPlayers().Where(player => player.GetGroup().BadgeText.Equals(broadcastGroup.BadgeText)))
            //             player.ReceiveHint(EventHandlers.FormatArguments(arguments, 3), tim);
            //
            //         response = $"Hint sent to all members of \"{arguments.At(1)}\"";
            //         return true;
            //     case "groups":
            //         if (arguments.Count < 4)
            //         {
            //             response = "Usage: hbc groups (list of groups (i.e.: owner,admin,moderator)) (time) (message)";
            //             return false;
            //         }
            //
            //         string[] groups = arguments.At(1).Split(',');
            //         List<string> groupList = new();
            //         foreach (string s in groups)
            //         {
            //             UserGroup broadGroup = ServerStatic.PermissionsHandler.GetGroup(s);
            //             if (broadGroup != null)
            //                 groupList.Add(broadGroup.BadgeText);
            //
            //         }
            //
            //         if (!ushort.TryParse(arguments.At(2), out ushort e) && e <= 0)
            //         {
            //             response = $"Invalid value for duration: {arguments.At(2)}";
            //             return false;
            //         }
            //
            //         foreach (var p in Player.GetPlayers().Where(p => groupList.Contains(p.GetGroup().BadgeText)))
            //             p.ReceiveHint(EventHandlers.FormatArguments(arguments, 3), e);
            //
            //
            //         StringBuilder bdr = StringBuilderPool.Shared.Rent("Hint sent to groups with badge text: ");
            //         foreach (string p in groupList)
            //         {
            //             bdr.Append("\"");
            //             bdr.Append(p);
            //             bdr.Append("\"");
            //             bdr.Append(" ");
            //         }
            //         string ms = bdr.ToString();
            //         StringBuilderPool.Shared.Return(bdr);
            //         response = ms;
            //         return true;
            //     case "role":
            //         if (arguments.Count < 4)
            //         {
            //             response = "Usage: hbc role (RoleTypeId) (time) (message)";
            //             return false;
            //         }
            //
            //         if (!Enum.TryParse(arguments.At(1), true, out RoleTypeId role))
            //         {
            //             response = $"Invalid value for RoleTypeId: {arguments.At(1)}";
            //             return false;
            //         }
            //
            //         if (!ushort.TryParse(arguments.At(2), out ushort te) && te <= 0)
            //         {
            //             response = $"Invalid value for duration: {arguments.At(2)}";
            //             return false;
            //         }
            //
            //         foreach (var player in Player.GetPlayers().Where(player => player.Role == role))
            //         {
            //             player.ReceiveHint(EventHandlers.FormatArguments(arguments, 3), te);
            //         }
            //
            //         response = $"Hint sent to all members of \"{arguments.At(1)}\"";
            //         return true;
            //     case "roles":
            //         if (arguments.Count < 4)
            //         {
            //             response = "Usage: hbc roles (RoleTypeId group (i.e.: ClassD, Scientist, NtfCadet)) (time) (message)";
            //             return false;
            //         }
            //
            //         string[] roles = arguments.At(1).Split(',');
            //         List<RoleTypeId> roleList = new();
            //         foreach (string s in roles)
            //         {
            //             if (Enum.TryParse(s, true, out RoleTypeId r))
            //                 roleList.Add(r);
            //         }
            //
            //         if (!ushort.TryParse(arguments.At(2), out ushort ti) && ti <= 0)
            //         {
            //             response = $"Invalid value for duration: {arguments.At(2)}";
            //             return false;
            //         }
            //
            //         foreach (var p in Player.GetPlayers().Where(p => roleList.Contains(p.Role)))
            //             p.ReceiveHint(EventHandlers.FormatArguments(arguments, 3), ti);
            //
            //         StringBuilder build = StringBuilderPool.Shared.Rent("Hint sent to roles: ");
            //         foreach (RoleTypeId ro in roleList)
            //         {
            //             build.Append("\"");
            //             build.Append(ro.ToString());
            //             build.Append("\"");
            //             build.Append(" ");
            //         }
            //         string msg = build.ToString();
            //         StringBuilderPool.Shared.Return(build);
            //         response = msg;
            //         return true;
            //     case "random":
            //     case "someone":
            //         if (arguments.Count < 3)
            //         {
            //             response = "Usage: hbc (random / someone) (time) (message)";
            //             return false;
            //         }
            //
            //         if (!ushort.TryParse(arguments.At(1), out ushort me) && me <= 0)
            //         {
            //             response = $"Invalid value for duration: {arguments.At(1)}";
            //             return false;
            //         }
            //
            //         Player plyr = Player.GetPlayers().ToList()[Plugin.NumGen.Next(0, Player.GetPlayers().Count())];
            //         plyr.ReceiveHint(EventHandlers.FormatArguments(arguments, 2), me);
            //         response = $"Hint sent to {plyr.Nickname}";
            //         return true;
            //     case "staff":
            //     case "admin":
            //         if (arguments.Count < 3)
            //         {
            //             response = "Usage: hbc (staff / admin) (time) (message)";
            //             return false;
            //         }
            //
            //         if (!ushort.TryParse(arguments.At(1), out ushort t))
            //         {
            //             response = $"Invalid value for hint broadcast time: {arguments.At(1)}";
            //             return false;
            //         }
            //
            //         foreach (var pl in Player.GetPlayers().Where(pl => pl.ReferenceHub.serverRoles.RemoteAdmin))
            //             pl.ReceiveHint(
            //                 $"<color=orange>[Admin Hint]</color> <color=green>{EventHandlers.FormatArguments(arguments, 2)} - {((CommandSender)sender).Nickname}</color>",
            //                 t);
            //
            //         response = $"Hint sent to all currently online staff";
            //         return true;
            //     case "clearall":
            //         if (arguments.Count != 1)
            //         {
            //             response = "Usage: hbc clearall";
            //             return false;
            //         }
            //
            //         foreach (Player py in Player.GetPlayers())
            //             py.ReceiveHint(" ");
            //         response = "All hints have been cleared";
            //         return true;
            //     default:
            //         if (arguments.Count < 2)
            //         {
            //             response = "Usage: hbc (time) (message)";
            //             return false;
            //         }
            //
            //         if (!ushort.TryParse(arguments.At(0), out ushort tm))
            //         {
            //             response = $"Invalid value for hint broadcast time: {arguments.At(0)}";
            //             return false;
            //         }
            //
            //         foreach (var py in Player.GetPlayers().Where(py => py.ReferenceHub.queryProcessor._ipAddress != "127.0.0.1"))
            //             py.ReceiveHint(EventHandlers.FormatArguments(arguments, 2), tm);
            //         break;
            // }
            // response = "";
            // return false;
        }
    }
}