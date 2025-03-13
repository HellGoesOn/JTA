using JTA.Common.Players;
using Terraria;

namespace JTA.Content.Stands.Crusaders.StarPlatinum
{
    public class StarSuckAbility : StandAbility
    {
        public StarSuckAbility() : base("Breath of Fresh Air", "Sucks in enemies, bringing them closer to you")
        {
        }

        protected override void UseAbility(Player player)
        {
            var check = StandPlayer.Get(player).GetStandProjectile();

            if (check != null && check.ModProjectile is StarPlatinumProjectile realStand) {
                realStand.startDirection = player.direction;
                realStand.CurrentAnimation = "Suck";
                check.netUpdate = true;
            }
        }
    }
}
