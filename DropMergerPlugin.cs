using System.Collections.Generic;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace DropMerger
{
    /// <summary>
    /// Drops configured resource items at the nearby player instead of letting them scatter.
    /// Hooks <see cref="ItemManager.onServerSpawningItemDrop"/> and, for matching ids, redirects
    /// the spawn position to the nearest player's feet (so a harvester's logs/ore pile up on them).
    /// Runs entirely on the Unity main thread (the event fires there).
    /// </summary>
    public sealed class DropMergerPlugin : RocketPlugin<DropMergerConfiguration>
    {
        public static DropMergerPlugin Instance { get; private set; }

        private HashSet<ushort> _ids;

        protected override void Load()
        {
            Instance = this;
            RebuildIdSet();
            ItemManager.onServerSpawningItemDrop += OnServerSpawningItemDrop;
            Logger.Log("DropMerger loaded. Resources=" + _ids.Count +
                       ", MaxPlayerDistance=" + Configuration.Instance.MaxPlayerDistance + "m." +
                       (_ids.Count == 0 ? " (configure ResourceItemIds — use /itemid)" : ""));
        }

        protected override void Unload()
        {
            ItemManager.onServerSpawningItemDrop -= OnServerSpawningItemDrop;
            _ids = null;
            Instance = null;
            Logger.Log("DropMerger unloaded.");
        }

        /// <summary>Rebuild the fast id lookup from config (called on load and after reload).</summary>
        public void RebuildIdSet()
        {
            _ids = new HashSet<ushort>();
            ushort[] arr = Configuration.Instance.ResourceItemIds;
            if (arr != null)
                foreach (ushort id in arr)
                    _ids.Add(id);
        }

        // Player capsule radius is ~0.4m; spawn drops just outside it so the
        // pickup hitbox isn't swallowed by the player's own collider.
        private const float CapsuleClearance = 0.6f;
        // Spawn slightly above the ground so the item settles cleanly instead
        // of clipping into terrain when the player stands on a slope.
        private const float SpawnHeightOffset = 0.5f;

        private void OnServerSpawningItemDrop(Item item, ref Vector3 location, ref bool shouldAllow)
        {
            if (!shouldAllow || item == null || _ids == null || _ids.Count == 0)
                return;
            if (!_ids.Contains(item.id))
                return;

            DropMergerConfiguration cfg = Configuration.Instance;
            if (cfg.MaxPlayerDistance <= 0f)
                return;

            Player player = FindNearestPlayer(location, cfg.MaxPlayerDistance);
            if (player == null)
                return; // no one close enough — leave the drop where it is

            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = CapsuleClearance + Random.Range(0f, Mathf.Max(0f, cfg.PileJitter));
            Vector3 feet = player.transform.position;
            location = new Vector3(
                feet.x + Mathf.Cos(angle) * radius,
                feet.y + SpawnHeightOffset,
                feet.z + Mathf.Sin(angle) * radius);
        }

        private static Player FindNearestPlayer(Vector3 pos, float maxDistance)
        {
            float best = maxDistance * maxDistance;
            Player result = null;
            List<SteamPlayer> clients = Provider.clients;
            for (int i = 0; i < clients.Count; i++)
            {
                Player p = clients[i]?.player;
                if (p == null)
                    continue;
                float d = (p.transform.position - pos).sqrMagnitude;
                if (d <= best)
                {
                    best = d;
                    result = p;
                }
            }
            return result;
        }
    }
}
