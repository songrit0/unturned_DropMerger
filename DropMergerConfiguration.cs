using System.Xml.Serialization;
using Rocket.API;

namespace DropMerger
{
    /// <summary>
    /// Configuration for DropMerger. RocketMod serializes the public fields below to/from
    /// <c>Plugins/DropMerger/DropMerger.configuration.xml</c>.
    /// </summary>
    public sealed class DropMergerConfiguration : IRocketPluginConfiguration
    {
        /// <summary>
        /// Item ids that get dropped at the nearby player instead of where they would scatter
        /// (resources such as logs, ores, stone, scrap). Use /itemid to find ids.
        /// </summary>
        [XmlArray("ResourceItemIds")]
        [XmlArrayItem("Id")]
        public ushort[] ResourceItemIds;

        /// <summary>Only redirect a drop if a player is within this distance (m) of it.</summary>
        public float MaxPlayerDistance;

        /// <summary>Tiny random spread (m) at the player's feet so items don't overlap exactly.</summary>
        public float PileJitter;

        public void LoadDefaults()
        {
            // Empty by default — fill with YOUR resource ids (vanilla or modded).
            // Find an id quickly by holding the item and running /itemid.
            ResourceItemIds = new ushort[0];
            MaxPlayerDistance = 16f;
            PileJitter = 0.15f;
        }
    }
}
