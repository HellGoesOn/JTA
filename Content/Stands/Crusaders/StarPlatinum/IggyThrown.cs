using JTA.Common.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JTA.Content.Stands.Crusaders.StarPlatinum
{
    public class IggyThrown : ModProjectile
    {
        SpriteAnimation anim;
        public override void SetDefaults()
        {
            anim = new SpriteAnimation("JTA/Content/Stands/Crusaders/StarPlatinum/IggyThrown");
            anim.FillFrames(4, 40, 36, 5);
            Projectile.width = Projectile.height = 32;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
        }

        public override void AI()
        {
            anim.Update();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            Main.EntitySpriteDraw(anim.texture.Value, Projectile.Center - Main.screenPosition, anim.frames[anim.currentFrame].AsRect(), lightColor, Projectile.velocity.ToRotation(), new Vector2(20, 18), 1f, SpriteEffects.None);
        }
    }
}
