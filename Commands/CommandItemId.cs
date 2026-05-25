using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace DropMerger
{
    /// <summary>
    /// <c>/itemid</c> - prints the id (and name) of the item currently held/equipped, so you can
    /// fill <c>ResourceItemIds</c>. Permission node: <c>dropmerger.itemid</c>.
    /// </summary>
    public sealed class CommandItemId : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "itemid";
        public string Help => "Show the id of the item you are holding.";
        public string Syntax => "";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "dropmerger.itemid" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer up = caller as UnturnedPlayer;
            if (up?.Player == null)
                return;

            ushort id = up.Player.equipment.itemID;
            if (id == 0)
            {
                UnturnedChat.Say(caller, "ถือ/ถืออาวุธไอเทมก่อน แล้วพิมพ์ /itemid | Hold the item first, then /itemid", Color.yellow);
                return;
            }

            string name = up.Player.equipment.asset != null ? up.Player.equipment.asset.itemName : "?";
            UnturnedChat.Say(caller, "Item: " + name + "  |  ID = " + id, Color.green);
        }
    }
}
