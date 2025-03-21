using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace JTA.Common.Systems
{
    [Autoload(Side = ModSide.Client)]
    public class ShaderSystem : ModSystem
    {
        public static Effect FadeOutShader { get; private set; }

        public override void OnModLoad()
        {
            if (Main.netMode != NetmodeID.Server) {
                FadeOutShader = ModContent.Request<Effect>("JTA/Assets/Effects/FadeOutShader", AssetRequestMode.ImmediateLoad).Value;

                Asset<Effect> shockWaveShader = ModContent.Request<Effect>("JTA/Assets/Effects/ShockwaveEffect");

                Filters.Scene["JTA: Shockwave"] = new Filter(new ScreenShaderData(shockWaveShader, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["JTA: FreezeSky"] = new Filter(new ScreenShaderData("FilterMiniTower").UseColor(.7f, .7f, .7f), EffectPriority.VeryHigh);
            }
        }

        public override void OnModUnload()
        {
            FadeOutShader = null;
        }
    }
}
