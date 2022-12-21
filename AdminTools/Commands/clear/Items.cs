using CommandSystem;
using System;
using InventorySystem.Items.Pickups;

namespace AdminTools.Commands.Cleanup
{
    class Items : ICommand
    {
        public string Command => "items";

        public string[] Aliases { get; } = { };

        public string Description => "Cleans up items dropped on the ground from the server";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission(PlayerPermissions.FacilityManagement))
            {
                response = "You do not have permission to use this command";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: cleanup items";
                return false;
            }

            foreach (ItemPickupBase item in Handlers.GetPickups())
                UnityEngine.Object.Destroy(item);

            response = "Items have been cleaned up now";
            return true;
        }
    }
}
