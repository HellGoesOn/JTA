using JTA.Common.Players;
using Terraria;

namespace JTA.Content.Stands.Crusaders.StarPlatinum
{
    public class StarGroundPound : StandAbility
    {
        public StarGroundPound() : base("Ground Pound", "Leaps into the air & slams the ground.")
        {
        }

        protected override void UseAbility(Player player)
        {
            var check = StandPlayer.Get(player).GetStandProjectile();

            if (check != null && check.ModProjectile is StarPlatinumProjectile realStand && realStand.CurrentAnimation == "Idle") {
                realStand.startDirection = player.direction;
                if(player.velocity.Y == 0)
                realStand.CurrentAnimation = "GPJump";
                else
                    realStand.CurrentAnimation = "GPFall";
                check.netUpdate = true;
            }
        }
    }
}
