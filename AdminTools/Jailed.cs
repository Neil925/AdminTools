using AdminTools.Enums;
using PlayerRoles;
using System.Collections.Generic;
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
