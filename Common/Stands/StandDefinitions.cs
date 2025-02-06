using System.Collections.Generic;
using JTA.Content.Stands.Crusaders;
using Terraria.ModLoader;

namespace JTA.Common.Stands
{
    public class StandDefinitions : ModSystem
    {
        internal static readonly Dictionary<string, Stand> stands = [];

        public override void Load()
        {
            base.Load();
        }

        public override void OnModLoad()
        {
            base.OnModLoad();

            var starPlatinum = new Stand();
            starPlatinum.name = "Star Platinum";
            starPlatinum.standProjectile = ModContent.ProjectileType<StarPlatinumProjectile>();

            RegisterStand("StarPlatinum", starPlatinum);
        }

        public override void Unload()
        {
            stands.Clear();
        }

        public static void RegisterStand(string name, Stand stand)
        {
            stands.Add(name, stand);
        }

        public static Stand Get(string name) => stands[name].GetCopy();
    }
}
