using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using JTA.Common.Graphics;
using JTA.Common.Players;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace JTA.Common.Stands
{
    public abstract class StandProjectile : ModProjectile
    {
        public string currentAnimation;

        public Dictionary<string, SpriteAnimation> animations = [];

        public override string Texture => "JTA/Assets/Textures/Kurwa";

        public sealed override void SetDefaults()
        {
            base.SetDefaults();

            currentAnimation = "";

            SafeDefaults();
        }

        public virtual void SafeDefaults()
        {

        }

        public sealed override void AI()
        {
            base.AI();
            Projectile.timeLeft = 10;
            SafeAI();

            if(animations.TryGetValue(currentAnimation, out SpriteAnimation value)) {
                value.Update();
            }
        }

        public virtual void SafeAI()
        {

        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!StandPlayer.Get().IsStandUser)
                return false;

            return base.PreDraw(ref lightColor);
        }
    }
}
