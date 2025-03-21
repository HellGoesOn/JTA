using JTA.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace JTA.Common.GlobalProjectiles
{
    public class TimeStopProjectile : GlobalProjectile
    {
        public static List<EntityState> states = [];
        
        public override void Load()
        {
            for (int i = 0; i < Main.maxProjectiles; i++) {
                states.Add(new());
            }
        }

        public override bool PreAI(Projectile projectile)
        {
            var timeStopSystem = TimeStopSystem.Get();
            projectile.TryGetOwner(out var plr);
            if (timeStopSystem.IsTimeStopped && !(plr != null && projectile.friendly) && !timeStopSystem.DidIStopTime(projectile)) {

                var state = states[projectile.whoAmI];

                if (!state.active && projectile.active) {
                    state.active = true;
                    state.position = projectile.position;
                    state.velocity = projectile.velocity;
                    for (int i = 0; i < projectile.ai.Length; i++) {
                        state.ai[i] = projectile.ai[i];
                    }
                }
                else if (projectile.active) {
                    projectile.position = state.position;
                    state.velocity = projectile.velocity;
                    for (int i = 0; i < projectile.ai.Length; i++) {
                        projectile.ai[i] = state.ai[i];
                    }

                    projectile.frameCounter = 0;
                }
                projectile.timeLeft++;
                return false;
            }
            else {
                states[projectile.whoAmI].active = false;
            }

            return base.PreAI(projectile);
        }
    }
}
