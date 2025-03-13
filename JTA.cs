using JTA.Common.Players;
using JTA.Common.Stands;
using JTA.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JTA
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public partial class JTA : Mod
	{
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            PacketType type = (PacketType)reader.ReadByte();

            switch (type) {
                case PacketType.SyncPlayer:

                    int plrID= reader.ReadByte();

                    StandPlayer splr = StandPlayer.Get(Main.player[plrID]);

                    splr.ReceiveSyncPlayer(reader);

                    if (Main.netMode == NetmodeID.Server)
                        splr.SyncPlayer(-1, whoAmI, false);

                    break;
                case PacketType.UsedAbility:
                    plrID = reader.ReadByte();
                    Player player = Main.player[plrID];

                    splr = StandPlayer.Get(player);
                    var ability = StandDefinitions.Stands[splr.stand].Abilities[splr.selectedAbilityIndex];
                    Main.NewText($"Tried using ability: {ability.Name} by player[{plrID}]");
                    ability.Use(player, true);

                    if (Main.netMode == NetmodeID.Server) {
                        StandAbility.SendAbilityPacket(player);
                    }
                    break;
                case PacketType.SyncPerks:
                    break;
                default:
                    break;
            }
        }
    }
}