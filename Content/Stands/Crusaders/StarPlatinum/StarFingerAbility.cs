using JTA.Common.Players;
using Terraria;

namespace JTA.Content.Stands.Crusaders.StarPlatinum
{
    public class StarFingerAbility : StandAbility
    {
        public StarFingerAbility() : base("Star Finger", "Ranged Attack inflicting knockback")
        {
        }

        protected override void UseAbility(Player player)
        {
            var check = StandPlayer.Get(player).GetStandProjectile();

            if (check != null && check.ModProjectile is StarPlatinumProjectile realStand 
                && (realStand.CurrentAnimation == "Idle" || StandPlayer.Get(player).standAutoMode)) {
                realStand.startDirection = player.direction;
                realStand.nextAnimation = realStand.CurrentAnimation = "Finger";
                check.netUpdate = true;
            }
        }
    }
}
