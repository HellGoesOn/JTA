using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace JTA.Common.Systems
{
    [Autoload(Side = ModSide.Client)]
    public class ShaderSystem : ModSystem
    {
        public static Effect FadeOutShader { get; private set; }

        public override void OnModLoad()
        {
            FadeOutShader = ModContent.Request<Effect>("JTA/Assets/Effects/FadeOutShader", AssetRequestMode.ImmediateLoad).Value;

            //Ref<Effect> screenRef = new Ref<Effect>();
        }

        public override void OnModUnload()
        {
            FadeOutShader = null;
        }
    }
}
