using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JTA.Common.Graphics;
using JTA.Common.Stands;
using JTA.Common.Systems;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace JTA.Content.Stands.Crusaders
{
    public class StarPlatinumProjectile : StandProjectile
    {
        public override void SafeDefaults()
        {
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
            var path = "JTA/Content/Stands/Crusaders/StarPlatinum/";
            animations.Add("Idle", new SpriteAnimation(path + "SPIdle").FillFrames(8, 50, 50, 5));
            currentAnimation = "Idle";
            Projectile.width = Projectile.height = 100;
        }

        public override void SafeAI()
        {
            base.SafeAI();
        }

        public override void PostDraw(Color lightColor)
        {
            if(animations.TryGetValue(currentAnimation, out SpriteAnimation anim)) {

                Projectile.TryGetOwner(out var plr);
                var scale = new Vector2(2);
                var sfx = plr.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                anim.Draw(Main.spriteBatch, anim.texture, Projectile.Center - Main.screenPosition, Color.White, 0, scale: scale, sfx: sfx);
                var frame = anim.frames[anim.currentFrame];
                Effect fx = ShaderSystem.fadeOutShader;

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);

                fx.Parameters["uImageSize0"].SetValue(new Vector2(anim.texture.Width, anim.texture.Height));
                fx.Parameters["uSourceRect"].SetValue(new Vector4(frame.x, frame.y, frame.width, frame.height));
                fx.Parameters["beginPoint"].SetValue(0.795f);
                fx.Parameters["speed"].SetValue(6);
                fx.CurrentTechnique.Passes[0].Apply();

                anim.Draw(Main.spriteBatch, anim.texture, Projectile.Center - Main.screenPosition, Color.White, 0, scale: scale, frameOffset: new Vector2(50, 0), sfx: sfx);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
    }
}
