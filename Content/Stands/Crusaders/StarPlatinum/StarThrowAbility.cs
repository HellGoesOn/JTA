using JTA.Common.Players;
using Terraria;

namespace JTA.Content.Stands.Crusaders.StarPlatinum
{
    public class StarThrowAbility : StandAbility
    {
        public StarThrowAbility() : base("Perfect Pitch", "Ranged Attack")
        {
        }

        protected override void UseAbility(Player player)
        {
            var check = StandPlayer.Get(player).GetStandProjectile();

            if (check != null && check.ModProjectile is StarPlatinumProjectile realStand && realStand.CurrentAnimation == "Idle") {
                realStand.startDirection = player.direction;
                realStand.CurrentAnimation = "Throw";
                check.netUpdate = true;
            }
        }
    }
}
