using JTA.Content.Stands.Crusaders;
using JTA.Content.Stands.Crusaders.StarPlatinum;
using System.Collections.Generic;
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

            RegisterStand(starPlatinum);
        }

        public void RegisterStand(Stand stand)
        {
            Stands.Add(stand.Name, stand);
        }
    }
}
