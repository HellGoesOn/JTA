using JTA.Common.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace JTA.Common.GlobalNPCs
{
    public class TimeStopNPC : GlobalNPC
    {
        public override bool InstancePerEntity => false;

        static readonly List<EntityState> states = [];

        public override void Load()
        {
            for (int i = 0; i < Main.maxNPCs; i++) {
                states.Add(new());
            }
        }

        public override bool PreAI(NPC npc)
        {
            var timeStopSystem = TimeStopSystem.Get();
            if (timeStopSystem.IsTimeStopped && !timeStopSystem.DidIStopTime(npc)) {

                var state = states[npc.whoAmI];

                if (!state.active && npc.active) {
                    state.active = true;
                    state.position = npc.position;
                    state.velocity = npc.velocity;
                    for (int i = 0; i < npc.ai.Length; i++) {
                        state.ai[i] = npc.ai[i];
                    }
                }
                else if(npc.active) {
                    npc.position = state.position;
                    state.velocity = npc.velocity;
                    for (int i = 0; i < npc.ai.Length; i++) {
                        npc.ai[i] = state.ai[i];
                    }

                    npc.frameCounter = 0;
                }
                return false;
            }
            else {
                states[npc.whoAmI].active = false;
            }

            return base.PreAI(npc);
        }
    }
}
