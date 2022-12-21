using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using UnityEngine;

namespace AdminTools
{
    public class InstantKillComponent : MonoBehaviour
    {
        public Player Player;
        public void Awake()
        {
            Player = Player.Get(gameObject);
            EventManager.RegisterEvents(EventHandlers.Plugin, this);
            Plugin.IkHubs.Add(Player, this);
        }

        [PluginEvent(ServerEventType.PlayerLeft)]
        private void OnLeave(Player player)
        {
            if (player == Player)
                Destroy(this);
        }

        public void OnDestroy()
        {
            EventManager.UnregisterEvents(EventHandlers.Plugin, this);
            Plugin.IkHubs.Remove(Player);
        }

        [PluginEvent(ServerEventType.PlayerDamage)]
        public void RunWhenPlayerIsHurt(Player player, Player target, DamageHandlerBase damageHandler)
        {
            if (player != target && player == Player)
                damageHandler.SetAmount(float.MaxValue);
        }
    }
}
