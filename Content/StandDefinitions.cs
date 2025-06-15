using JTA.Common.Players;
using JTA.Content.Stands;
using JTA.Content.Stands.Crusaders;
using JTA.Content.Stands.Crusaders.StarPlatinum;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace JTA.Content
{
    public class StandDefinitions : ModSystem
    {
        public static Dictionary<string, Stand> Stands { get; private set; } = [];

        public override void OnModLoad()
        {
            var starPlatinum = new Stand(ModContent.ProjectileType<StarPlatinumProjectile>(), "Star Platinum");
            starPlatinum.Abilities.Add(new StarFingerAbility());
            starPlatinum.Abilities.Add(new StarSuckAbility());
            starPlatinum.Abilities.Add(new StarGroundPound());
            starPlatinum.Abilities.Add(new StarThrowAbility());
            starPlatinum.Abilities.Add(new StarTimeStop());

            RegisterStand(starPlatinum);
        }

        public void RegisterStand(Stand stand)
        {
            Stands.Add(stand.Name, stand);
        }
    }
}
