using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JTA.Content
{
    public abstract class StandAbility
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public StandAbility(string name, string description) 
        {
            Name = name;
            Description = description;
        }

        public void Use(Player player, bool silent = false)
        {
            UseAbility(player);

            if(Main.netMode == NetmodeID.MultiplayerClient && !silent) {
                SendAbilityPacket(player);
            }
        }

        public static void SendAbilityPacket(Player player)
        {
            ModPacket packet = ModContent.GetInstance<JTA>().GetPacket();
            packet.Write((byte)PacketType.UsedAbility);
            packet.Write((byte)player.whoAmI);
            packet.Send(-1, player.whoAmI);
        }

        protected virtual void UseAbility(Player player) 
        { 
        }

        public virtual void Update(Player player) 
        { 
        }

    }
}
