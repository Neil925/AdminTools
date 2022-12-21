﻿using CommandSystem;
using System;
using PluginAPI.Core;

namespace AdminTools.Commands.Ghost
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Ghost : ParentCommand
    {
        public Ghost() => LoadGeneratedCommands();

        public override string Command { get; } = "ghost";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Sets everyone or a user to be invisible";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "This command is not available at this time.";
            return false;

            // if (!((CommandSender)sender).CheckPermission(PlayerPermissions.Noclip))
            // {
            //     response = "You do not have permission to use this command";
            //     return false;
            // }
            //
            // if (arguments.Count != 1)
            // {
            //     response = "Usage:\nghost ((player id / name) or (all / *))" +
            //         "\nghost clear";
            //     return false;
            // }
            //
            // switch (arguments.At(0))
            // {
            //     case "clear":
            //         foreach (Player pl in Player.GetPlayers())
            //             pl.IsInvisible = false;
            //
            //         response = "Everyone is no longer invisible";
            //         return true;
            //     case "*":
            //     case "all":
            //         foreach (Player pl in Player.List)
            //             pl.IsInvisible = true;
            //
            //         response = "Everyone is now invisible";
            //         return true;
            //     default:
            //         Player ply = int.TryParse(arguments.At(0), out int id) ? Player.GetPlayers().FirstOrDefault(x => x.PlayerId == id) : Player.GetByName(arguments.At(0));
            //         if (ply == null)
            //         {
            //             response = $"Player not found: {arguments.At(0)}";
            //             return false;
            //         }
            //
            //         if (!ply.IsInvisible)
            //         {
            //             ply.IsInvisible = true;
            //             response = $"Player {ply.Nickname} is now invisible";
            //         }
            //         else
            //       l
        }
    }
}