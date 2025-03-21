using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace JTA.Content.Projectiles
{
    public class ShockwaveProjectile : ModProjectile
    {
        private const int
               RIPPLE_COUNT = 1,
               RIPPLE_SIZE = 1;

        private const float
            RIPPLE_SPEED = 2.75f,
            DISTORT_STRENGTH = 75000f;

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.light = 0.9f;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            //Projectile.aiStyle = 24;
        }

        public override void AI()
        {
            if (Main.dedServ)
                return;

            Projectile.Center = Owner.Center;

            if (!Filters.Scene["JTA: Shockwave"].IsActive()) {
                Filters.Scene.Activate("JTA: Shockwave", Projectile.Center).GetShader().UseColor(RIPPLE_COUNT, RIPPLE_SIZE, RIPPLE_SPEED).UseTargetPosition(Projectile.Center);
            }
            float progress = (240f - Projectile.timeLeft) / 60f;

            Filters.Scene["JTA: Shockwave"].GetShader().UseTargetPosition(Projectile.Center);
            Filters.Scene["JTA: Shockwave"].GetShader().UseProgress(progress).UseOpacity(DISTORT_STRENGTH * (progress / 3f));
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.dedServ)
                return;

            Filters.Scene["JTA: Shockwave"].Deactivate();
        }

        public Player Owner => Main.player[Projectile.owner];

        public bool IsNativelyImmuneToTimeStop() => true;

        public override string Texture => "JTA/Assets/Textures/Kurwa";
    }
}
