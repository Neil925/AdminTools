using System.Collections.Generic;
using AdminTools.Enums;
using InventorySystem.Items;
using PlayerRoles;
using PluginAPI.Core.Items;
using UnityEngine;

namespace AdminTools
{

	public class Jailed
	{
		public string Userid;
		public string Name;
		public List<ItemType> Items;
		public RoleTypeId Role;
		public Vector3 Position;
		public float Health;
		public Dictionary<AmmoType, ushort> Ammo;
		public bool CurrentRound;
	}
}
