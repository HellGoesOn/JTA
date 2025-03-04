using JTA.Common.Graphics;
using JTA.Common.Players;
using JTA.Common.Stands;
using JTA.Common.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace JTA.Content.Stands.Crusaders
{
    public class StarPlatinumProjectile : StandProjectile
    {
        int punchCounter;
        public int startDirection;

        float range;
        int direction;

        public override void SafeDefaults()
        {
            range = 32 * 5;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
            var path = "JTA/Content/Stands/Crusaders/StarPlatinum/";
            animations.Add("Idle", new SpriteAnimation(path + "SPIdle").FillFrames(8, 100, 100, 5));
            animations.Add("Punch1", new SpriteAnimation(path + "SPPunch1").FillFrames(5, 140, 100, 5));
            animations.Add("Punch2", new SpriteAnimation(path + "SPPunch2").FillFrames(7, 132, 100, 5));
            animations.Add("Punch3", new SpriteAnimation(path + "SPPunch3").FillFrames(8, 118, 100, 5));
            animations.Add("Finger", new SpriteAnimation(path + "SPStarFinger").FillFrames(16, 292, 100, 5).SetSpeed(10, 3));

            CurrentAnimation = "Idle";

            Projectile.damage = 0;
            Projectile.width = Projectile.height = 100;
        }

        public override bool PreAI()
        {
            return base.PreAI();
        }

        public override void SafeAI()
        {
            base.SafeAI();

            Projectile.TryGetOwner(out var owner);

            //var frame = anim.frames[anim.currentFrame];
            //Projectile.width = frame.width;
            //Projectile.height = frame.height;

            switch (CurrentAnimation) {
                case "Spawn":
                    CurrentAnimation = "Idle";
                    break;
                case "Idle":
                    if (Main.myPlayer == Projectile.owner && KeybindSystem.SummonStand.JustPressed && Projectile.timeLeft <= 10)
                        CurrentAnimation = "Despawn";

                        direction = owner.direction;
                    Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center + new Vector2(-50 * direction, -24), 0.12f);
                    punchCounter = 0;

                    if (LeftClick) {
                        if (++punchCounter > 3)
                            punchCounter = 1;
                        Projectile.netUpdate = true;
                        CurrentAnimation = $"Punch{punchCounter}";
                    }
                    break;
                case "Punch1":
                case "Punch2":
                case "Punch3":
                    var mousePosition = Main.MouseWorld;
                    var dir = (mousePosition - owner.Center).SafeNormalize(-Vector2.UnitX) * (Math.Clamp(Vector2.Distance(owner.Center, mousePosition), 0, range));
                    Main.NewText(dir);

                    direction = mousePosition.X < owner.Center.X ? -1 : 1;

                    Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center + dir, 0.4f);

                    if (!nextAnimation.Contains("Punch") && LeftClick) {
                        if (++punchCounter > 3)
                            punchCounter = 1;
                        Projectile.netUpdate = true;
                        nextAnimation = $"Punch{punchCounter}";
                    }

                    if (!nextAnimation.Contains("Punch") && animations[CurrentAnimation].finished)
                        nextAnimation = "Idle";

                    break;
                case "Finger":
                    direction = startDirection;
                    Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center + new Vector2(140 * direction, -24), 0.4f);
                    nextAnimation = "Idle";
                    break;
                case "Despawn":
                    Projectile.Kill();
                    StandPlayer.Get(owner).activeStandProjectile = StandPlayer.UNSUMMONED;
                    break;
            }
        }

        public override void PostDraw(Color lightColor)
        {
            if (animations.TryGetValue(CurrentAnimation, out SpriteAnimation anim)) {

                Projectile.TryGetOwner(out var plr);
                var scale = new Vector2(1);
                var sfx = direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                anim.Draw(Main.spriteBatch, anim.texture, (Projectile.Center - Main.screenPosition).Floor(), Color.White, 0, scale: scale, sfx: sfx);
                var frame = anim.frames[anim.currentFrame];
                Effect fx = ShaderSystem.FadeOutShader;

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);

                fx.Parameters["uImageSize0"].SetValue(new Vector2(anim.texture.Width, anim.texture.Height));
                fx.Parameters["uSourceRect"].SetValue(new Vector4(frame.x, frame.y, frame.width, frame.height));
                fx.Parameters["beginPoint"].SetValue(0.795f);
                fx.Parameters["speed"].SetValue(6);
                fx.CurrentTechnique.Passes[0].Apply();

                anim.Draw(Main.spriteBatch, anim.texture, (Projectile.Center - Main.screenPosition).Floor(), Color.White, 0, scale: scale, frameOffset: new Vector2(frame.width, 0), sfx: sfx);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            }
        }
    }
}
