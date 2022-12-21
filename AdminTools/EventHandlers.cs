using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AdminTools.Enums;
using Interactables.Interobjects.DoorUtils;
using MEC;
using Mirror;
using NorthwoodLib.Pools;
using PlayerRoles;
using PlayerRoles.Ragdolls;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using RemoteAdmin;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AdminTools
{
	using InventorySystem.Items.Firearms.Attachments;
	using PlayerStatsSystem;

	public class EventHandlers
	{
		public static Plugin Plugin;
		public EventHandlers(Plugin plugin) => Plugin = plugin;
		public static List<Player> BreakDoorsList { get; } = new();

        [PluginEvent(ServerEventType.PlayerInteractDoor)]
		public void OnDoorOpen(Player player, DoorVariant door, bool canOpen)
		{
			if (Plugin.PryGateHubs.Contains(player))
				door.TryPryOpen();
		}

		public static string FormatArguments(ArraySegment<string> sentence, int index)
		{
			StringBuilder sb = StringBuilderPool.Shared.Rent();
			foreach (string word in sentence.Segment(index))
			{
				sb.Append(word);
				sb.Append(" ");
			}
			string msg = sb.ToString();
			StringBuilderPool.Shared.Return(sb);
			return msg;
		}

		public static void SpawnDummyModel(Player ply, Vector3 position, Quaternion rotation, RoleTypeId role, float x, float y, float z, out int dummyIndex)
		{
            dummyIndex = 0;
			GameObject obj = Object.Instantiate(NetworkManager.singleton.playerPrefab);
			CharacterClassManager ccm = obj.GetComponent<CharacterClassManager>();
			if (ccm == null)
				Log.Error("CCM is null, this can cause problems!");
			ccm._hub.roleManager.ServerSetRole(role, RoleChangeReason.RemoteAdmin);
			ccm.GodMode = true;
			obj.GetComponent<NicknameSync>().Network_myNickSync = "Dummy";
			obj.GetComponent<QueryProcessor>()._hub.Network_playerId = new RecyclablePlayerId(9999);
			obj.transform.localScale = new Vector3(x, y, z);
			obj.transform.position = position;
			obj.transform.rotation = rotation;
			NetworkServer.Spawn(obj);
			if (Plugin.DumHubs.TryGetValue(ply, out List<GameObject> objs))
			{
				objs.Add(obj);
			}
			else
			{
				Plugin.DumHubs.Add(ply, new List<GameObject>());
				Plugin.DumHubs[ply].Add(obj);
				dummyIndex = Plugin.DumHubs[ply].Count();
			}
			if (dummyIndex != 1)
				dummyIndex = objs.Count();
		}

		public static IEnumerator<float> SpawnBodies(Player player, RoleTypeId role, int count)
		{
			for (int i = 0; i < count; i++)
            {
                if (!PlayerRoleLoader.AllRoles.TryGetValue(role, out var roleBase) || roleBase is not IRagdollRole currentRole) 
                    yield break;
                
                GameObject gameObject = Object.Instantiate(currentRole.Ragdoll.gameObject);
                if (gameObject.TryGetComponent(out BasicRagdoll component))
                {
                    Transform transform = currentRole.Ragdoll.transform;
                    UniversalDamageHandler handler = new UniversalDamageHandler(0.0f, DeathTranslations.Unknown, DamageHandlerBase.CassieAnnouncement.Default);
                    component.NetworkInfo = new RagdollData(ReferenceHub._hostHub, handler, transform.localPosition, transform.localRotation);
                }

                NetworkServer.Spawn(gameObject);

                RagdollManager.ServerSpawnRagdoll(ReferenceHub._hostHub,
                    new UniversalDamageHandler(0.0f, DeathTranslations.Unknown, DamageHandlerBase.CassieAnnouncement.Default));
				yield return Timing.WaitForSeconds(0.15f);
			}
		}

        [PluginEvent(ServerEventType.PlayerLeft)]
        public void OnPlayerDestroyed(Player player, Player attacker, DamageHandlerBase damageHandler)
        {
            if (!Plugin.RoundStartMutes.Contains(player)) return;
            player.Unmute(true);
            Plugin.RoundStartMutes.Remove(player);
        }

        public static void SpawnWorkbench(Player ply, Vector3 position, Vector3 rotation, Vector3 size, out int benchIndex)
		{
			try
			{
				Log.Debug($"Spawning workbench");
				benchIndex = 0;
				GameObject bench =
					Object.Instantiate(
						NetworkManager.singleton.spawnPrefabs.Find(p => p.gameObject.name == "Work Station"));
				rotation.x += 180;
				rotation.z += 180;
				Offset offset = new()
                {
                    position = position,
                    rotation = rotation,
                    scale = Vector3.one
                };
                bench.gameObject.transform.localScale = size;
				NetworkServer.Spawn(bench);
				if (Plugin.BchHubs.TryGetValue(ply, out List<GameObject> objs))
				{
					objs.Add(bench);
				}
				else
				{
					Plugin.BchHubs.Add(ply, new List<GameObject>());
					Plugin.BchHubs[ply].Add(bench);
					benchIndex = Plugin.BchHubs[ply].Count();
				}

				if (benchIndex != 1)
					benchIndex = objs.Count();
				bench.transform.localPosition = offset.position;
				bench.transform.localRotation = Quaternion.Euler(offset.rotation);
				bench.AddComponent<WorkstationController>();
			}
			catch (Exception e)
			{
				Log.Error($"{nameof(SpawnWorkbench)}: {e}");
				benchIndex = -1;
			}
		}

        public static void SetPlayerScale(GameObject target, float x, float y, float z)
		{
			try
			{
				NetworkIdentity identity = target.GetComponent<NetworkIdentity>();
				target.transform.localScale = new Vector3(1 * x, 1 * y, 1 * z);

				ObjectDestroyMessage destroyMessage = new()
                {
                    netId = identity.netId
                };

                foreach (GameObject player in Player.GetPlayers().Select(player => player.GameObject))
				{
					NetworkConnection playerCon = player.GetComponent<NetworkIdentity>().connectionToClient;
					if (player != target)
						playerCon.Send(destroyMessage, 0);

					object[] parameters = { identity, playerCon };
					typeof(NetworkServer).InvokeStaticMethod("SendSpawnMessage", parameters);
				}
			}
			catch (Exception e)
			{
				Log.Info($"Set Scale error: {e}");
			}
		}

		public static void SetPlayerScale(GameObject target, float scale)
		{
			try
			{
				NetworkIdentity identity = target.GetComponent<NetworkIdentity>();
				target.transform.localScale = Vector3.one * scale;

				ObjectDestroyMessage destroyMessage = new()
                {
                    netId = identity.netId
                };

                foreach (GameObject player in Player.GetPlayers().Select(player => player.GameObject))
				{
					if (player == target)
						continue;

					NetworkConnection playerCon = player.GetComponent<NetworkIdentity>().connectionToClient;
					playerCon.Send(destroyMessage, 0);

					object[] parameters = { identity, playerCon };
					typeof(NetworkServer).InvokeStaticMethod("SendSpawnMessage", parameters);
				}
			}
			catch (Exception e)
			{
				Log.Info($"Set Scale error: {e}");
			}
		}

		public static IEnumerator<float> DoRocket(Player player, float speed)
		{
			const int maxAmnt = 50;
			int amnt = 0;
			while (player.Role != RoleTypeId.Spectator)
			{
				player.Position += Vector3.up * speed;
				amnt++;
				if (amnt >= maxAmnt)
				{
					player.IsGodModeEnabled = false;
                    Handlers.CreateThrowable(ItemType.GrenadeHE).SpawnActive(player.Position, .5f, player);
					player.Kill("Went on a trip in their favorite rocket ship.");
				}

				yield return Timing.WaitForOneFrame;
			}
		}

		public static IEnumerator<float> DoJail(Player player, bool skipadd = false)
		{
            Dictionary<AmmoType, ushort> ammo = player.Ammo().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            List<ItemType> items = player.ReferenceHub.inventory.UserInventory.Items.Select(x => x.Value.ItemTypeId).ToList();
            if (!skipadd)
			{
                Plugin.JailedPlayers.Add(new Jailed
				{
					Health = player.Health,
					Position = player.Position,
					Items = items,
					Name = player.Nickname,
					Role = player.Role,
					Userid = player.UserId,
					CurrentRound = true,
					Ammo = ammo
				});
            }

			if (player.IsOverwatchEnabled)
				player.IsOverwatchEnabled = false;
            yield return Timing.WaitForSeconds(1f);

            player.SetRole(RoleTypeId.Tutorial);
			player.Position = new Vector3(38f, 1020f, -32f);
		}

		public static IEnumerator<float> DoUnJail(Player player)
		{
			Jailed jail = Plugin.JailedPlayers.Find(j => j.Userid == player.UserId);
			if (jail.CurrentRound)
			{
				player.SetRole(jail.Role);
				yield return Timing.WaitForSeconds(0.5f);
				try
				{
					player.ResetInventory(jail.Items);
					player.Health = jail.Health;
					player.Position = jail.Position;
					foreach (KeyValuePair<AmmoType, ushort> kvp in jail.Ammo)
                        player.AddAmmo(kvp.Key.GetItemType(), kvp.Value);
				}
				catch (Exception e)
				{
					Log.Error($"{nameof(DoUnJail)}: {e}");
				}
			}
			else
			{
				player.SetRole(RoleTypeId.Spectator);
			}
			Plugin.JailedPlayers.Remove(jail);
		}

        [PluginEvent(ServerEventType.PlayerJoined)]
		public void OnPlayerVerified(Player player)
		{
			try
			{
				if (Plugin.JailedPlayers.Any(j => j.Userid == player.UserId))
					Timing.RunCoroutine(DoJail(player, true));

				if (File.ReadAllText(Plugin.OverwatchFilePath).Contains(player.UserId))
				{
					Log.Debug($"Putting {player.UserId} into overwatch.");
					Timing.CallDelayed(1, () => player.IsOverwatchEnabled = true);
				}

				if (File.ReadAllText(Plugin.HiddenTagsFilePath).Contains(player.UserId))
				{
					Log.Debug($"Hiding {player.UserId}'s tag.");
					Timing.CallDelayed(1, () => player.SetBadgeHidden(true));
				}

                if (Plugin.RoundStartMutes.Count == 0 || player.ReferenceHub.serverRoles.RemoteAdmin ||
                    Plugin.RoundStartMutes.Contains(player)) return;
                
                Log.Debug($"Muting {player.UserId} (no RA).");
                player.Mute();
                Plugin.RoundStartMutes.Add(player);
            }
			catch (Exception e)
			{
				Log.Error($"Player Join: {e}");
			}
		}

        [PluginEvent(ServerEventType.RoundStart)]
		public void OnRoundStart()
        {
            foreach (var ply in Plugin.RoundStartMutes.Where(ply => ply != null)) 
                ply.Unmute(true);

            Plugin.RoundStartMutes.Clear();
        }

        [PluginEvent(ServerEventType.RoundEnd)]
		public void OnRoundEnd(RoundSummary.LeadingTeam leadingTeam)
		{
			try
			{
				List<string> overwatchRead = File.ReadAllLines(Plugin.OverwatchFilePath).ToList();
				List<string> tagsRead = File.ReadAllLines(Plugin.HiddenTagsFilePath).ToList();

				foreach (Player player in Player.GetPlayers())
				{
					string userId = player.UserId;

					if (player.IsOverwatchEnabled && !overwatchRead.Contains(userId))
						overwatchRead.Add(userId);
					else if (!player.IsOverwatchEnabled && overwatchRead.Contains(userId))
						overwatchRead.Remove(userId);

					if (player.IsBadgeHidden() && !tagsRead.Contains(userId))
						tagsRead.Add(userId);
					else if (!player.IsBadgeHidden() && tagsRead.Contains(userId))
						tagsRead.Remove(userId);
				}

				foreach (string s in overwatchRead)
					Log.Debug($"{s} is in overwatch.");
				foreach (string s in tagsRead)
					Log.Debug($"{s} has their tag hidden.");
				File.WriteAllLines(Plugin.OverwatchFilePath, overwatchRead);
				File.WriteAllLines(Plugin.HiddenTagsFilePath, tagsRead);

				// Update all the jails that it is no longer the current round, so when they are unjailed they don't teleport into the void.
				foreach (var jail in Plugin.JailedPlayers.Where(jail => jail.CurrentRound)) 
                    jail.CurrentRound = false;
            }
			catch (Exception e)
			{
				Log.Error($"Round End: {e}");
			}

			if (Plugin.RestartOnEnd)
			{
				Log.Info("Restarting server....");
				Round.Restart(false, true, ServerStatic.NextRoundAction.Restart);
			}
		}

        [PluginEvent(ServerEventType.WaitingForPlayers)]
		public void OnWaitingForPlayers()
		{
			Plugin.IkHubs.Clear();
			BreakDoorsList.Clear();
		}

        [PluginEvent(ServerEventType.PlayerInteractDoor)]
		public void OnPlayerInteractingDoor(Player player, DoorVariant door, bool canOpen)
		{
			if (BreakDoorsList.Contains(player))
				door.BreakDoor();
		}
	}
}
