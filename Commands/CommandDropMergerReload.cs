using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using UnityEngine;

namespace DropMerger
{
    /// <summary>
    /// <c>/dmreload</c> - reloads config and rebuilds the resource-id lookup.
    /// Permission node: <c>dropmerger.reload</c>.
    /// </summary>
    public sealed class CommandDropMergerReload : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;
        public string Name => "dropmergerreload";
        public string Help => "Reload DropMerger configuration.";
        public string Syntax => "";
        public List<string> Aliases => new List<string> { "dmreload" };
        public List<string> Permissions => new List<string> { "dropmerger.reload" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            DropMergerPlugin plugin = DropMergerPlugin.Instance;
            if (plugin == null)
                return;

            plugin.Configuration.Load();
            plugin.RebuildIdSet();
            UnturnedChat.Say(caller, "[DropMerger] Configuration reloaded.", Color.green);
        }
    }
}
