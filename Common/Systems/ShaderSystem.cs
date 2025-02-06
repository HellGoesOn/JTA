using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace JTA.Common.Systems
{
    [Autoload(Side = ModSide.Client)]
    public class ShaderSystem : ModSystem
    {
        public static Effect fadeOutShader;

        public override void OnModLoad()
        {
            fadeOutShader = ModContent.Request<Effect>("JTA/Assets/Effects/FadeOutShader", AssetRequestMode.ImmediateLoad).Value;
        }

        public override void OnModUnload()
        {
            fadeOutShader = null;
        }
    }
}
