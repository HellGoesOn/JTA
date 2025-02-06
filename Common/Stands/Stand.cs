using Microsoft.Xna.Framework;
using rail;
using System;
using System.Collections.Generic;
using Terraria;

namespace JTA.Common.Stands
{
    public class Stand
    {
        int myStand = -1;

        public int standProjectile;

        public string name;

        public Func<Player, bool> canObtain;

        public Stand()
        {
            canObtain = Unconditional;
        }

        public readonly List<StandAbility> abilities = [];
        public readonly List<LearnableAbility> abilitiesToLearn = [];
        readonly List<StandPerk> perks = [];

        public void Update(Player player)
        {
            for (int i = abilitiesToLearn.Count - 1; i >= 0; i--) {
                var entry = abilitiesToLearn[i];
                if (entry.condition != null && entry.condition.Invoke()) {
                    abilities.Add(entry.abilityToLearn.GetCopy());
                    abilitiesToLearn.Remove(entry);
                }
            }

            foreach (var perk in perks) {
                perk.UpdatePerk(this, player);
            }

            foreach (var ability in abilities) {
                ability.Update(player, this);
            }
        }

        public void Summon(Player player)
        {
            if (myStand == -1) {
                // TO-DO: summon logic
                myStand = Projectile.NewProjectile(
                    player.GetSource_FromThis("JTA: Spawn Stand"),
                    player.Center + new Vector2(25 * player.direction * -1, -25),
                    Vector2.Zero,
                    standProjectile,
                    0,
                    0,
                    Main.myPlayer
                    );
            }
            else {
                Main.projectile[myStand].Kill();
                // TO-DO: despawn logic
                myStand = -1;
            }
        }

        public Stand GetCopy()
        {
            var newStand = new Stand {
                standProjectile = standProjectile,
                name = name
            };

            foreach (var ability in abilities) {
                newStand.abilities.Add(ability.GetCopy());
            }

            foreach (var ability in abilitiesToLearn) {
                newStand.abilitiesToLearn.Add(ability);
            }

            foreach(var perk in perks) {
                newStand.perks.Add(perk.GetCopy());
            }

            return newStand;
        }

        bool Unconditional(Player player) => true;

        public void AddPerk(StandPerk perk)
        {
            perks.Add(perk);
            perk.id = perks.Count;
        }

        public void AddPerks(StandPerk[] perks)
        {
            foreach (var p in perks) {
                AddPerk(p);
            }
        }

        public List<StandPerk> GetPerks() => perks;

        public Projectile GetProjectile()
        {
            if(myStand != -1)
                return Main.projectile[myStand];

            return null;
        }
    }
}
