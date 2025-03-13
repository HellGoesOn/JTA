using Terraria;
using Terraria.ModLoader;

namespace JTA.Content.Stands
{
    public class DamageHitbox : ModProjectile
    {
        int[] hitTimers;
        bool[] didHit;
        public int hitSpeed;
        public bool multiHit;

        public override string Texture => "JTA/Assets/Textures/Kurwa";

        public override void SetDefaults()
        {
            hitTimers = new int[Main.maxNPCs];
            didHit = new bool[Main.maxNPCs];
            Projectile.timeLeft = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.damage = 0;
            Projectile.tileCollide = false;
            hitSpeed = 20;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return hitTimers[target.whoAmI] <= 0 && (multiHit || !didHit[target.whoAmI]) && !target.friendly;
        }

        public override bool PreAI()
        {
            if (Projectile.ai[0] != -1 && Main.projectile[(int)Projectile.ai[0]].active) {
                Projectile.Center = Main.projectile[(int)Projectile.ai[0]].Center;
            }

            return base.PreAI();
        }

        public override void AI()
        {
            base.AI();
            for (int i = 0; i < hitTimers.Length; i++) {
                hitTimers[i]--;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hitTimers[target.whoAmI] = hitSpeed;
            didHit[target.whoAmI] = true;
        }
    }
}
