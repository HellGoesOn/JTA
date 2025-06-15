using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTA.Common.Players;
using JTA.Common.Stands;
using Terraria;

namespace JTA.Content.Stands
{
    // use strictly for StandAI purposes;
    public class AIActionAbility : StandAbility
    {
        public Action<Player> action;
        public AIActionAbility() : base("Dummy", "You are not supposed to see this!")
        {
        }
        protected override void UseAbility(Player player)
        {
            base.UseAbility(player);

            var plr = StandPlayer.Get(player);

            if(plr.activeStandProjectile != StandPlayer.INACTIVE_STAND_ID)
            {
                action?.Invoke(player);
            }
        }
    }
}
