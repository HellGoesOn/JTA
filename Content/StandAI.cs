using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace JTA.Content
{
    public class StandAI
    {
        int _oldAbilityCount;
        public int duration;
        public List<AbilityAI> abilities;

        public StandAI()
        {
            _oldAbilityCount = 0;
            abilities = [];
        }

        public void Update(Player owner, Projectile stand)
        {
            if(_oldAbilityCount != abilities.Count)
            {
                _oldAbilityCount = abilities.Count;
                abilities.Sort((a, b) =>
                {
                    if(a.priority < b.priority) return 1;
                    if(a.priority > b.priority) return -1;
                    return 0;
                });
            }

            if(duration > 0)
            { 
                duration--;
                //Main.NewText(duration);
                return;
            }

            foreach (var ability in abilities)
            {
                if (ability.internalCooldown > 0)
                    ability.internalCooldown--;

                if (ability.internalCooldown <= 0 && ability.condition != null && ability.condition.Invoke(owner, stand))
                {
                    ability.ability.Use(owner);
                    ability.internalCooldown = ability.internalCooldownMax;
                    duration = ability.duration;
                    Main.NewText(ability.ability.Name);
                    break;
                }
            }
        }
    }

    public class AbilityAI // rename later?
    {
        public int priority; // in case we have two equally applicable ability, choose one with higher priority
        public int internalCooldownMax; // prevent AI from spamming same ability too much if needed
        public int internalCooldown;
        public int duration;
        public Func<Player, Projectile, bool> condition; // condition when ability can be used. i.e. check distance to target, etc
        public StandAbility ability; // ability to use


        public AbilityAI(StandAbility ability, int priority = 0, int internalCooldownMax = 180, int duration = 0)
        {
            this.priority = priority;
            this.ability = ability;
            this.internalCooldownMax = internalCooldownMax;
            this.duration = duration;
        }
    }
}
