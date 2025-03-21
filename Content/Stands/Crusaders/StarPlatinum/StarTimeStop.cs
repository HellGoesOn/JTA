using JTA.Common.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace JTA.Content.Stands.Crusaders.StarPlatinum
{
    public class StarTimeStop : StandAbility
    {
        public StarTimeStop() : base("Time Stop", "Stops Time")
        {
        }

        protected override void UseAbility(Player player)
        {
            var check = StandPlayer.Get(player).GetStandProjectile();

            if (check != null && check.ModProjectile is StarPlatinumProjectile realStand && realStand.CurrentAnimation == "Idle") {
                realStand.startDirection = player.direction;
                realStand.CurrentAnimation = "TimeStop";
                check.netUpdate = true;
            }
        }
    }
}
