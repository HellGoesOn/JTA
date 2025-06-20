﻿using JTA.Common.Graphics;
using JTA.Common.Players;
using JTA.Content;
using JTA.Content.Stands;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace JTA.Common.Stands
{
    public abstract class StandProjectile : ModProjectile
    {
        private string currentAnimation;

        public string nextAnimation = "";

        public bool mouseLeftState, mouseRightState;

        public float searchRange = 1280; // ~40 tiles

        public Dictionary<string, SpriteAnimation> animations = [];

        public override string Texture => "JTA/Assets/Textures/Kurwa";

        public StandAI standAI;

        public sealed override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = 12;
            Projectile.tileCollide = false;
            Projectile.friendly = true;

            CurrentAnimation = "";

            SafeDefaults();
        }

        public virtual void SafeDefaults()
        {

        }

        public sealed override void AI()
        {
            base.AI();

            Projectile.TryGetOwner(out var owner);

            if (StandPlayer.Get(owner).standAutoMode)
            {
                standAI.Update(owner, Projectile);
            }

            SafeAI();

            if(animations.TryGetValue(CurrentAnimation, out SpriteAnimation value)) {
                value.Update();

                if(value.finished && nextAnimation != "") {
                    value.Reset();
                    currentAnimation = nextAnimation;
                    nextAnimation = "";
                    Projectile.netUpdate = true;
                }
            }

            mouseLeftState = owner.controlUseItem;
            mouseRightState = owner.controlUseTile;
            Projectile.timeLeft = 10;
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

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);

            writer.Write(CurrentAnimation);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);

            CurrentAnimation = reader.ReadString();
        }

        public bool LeftClick => Main.player[Projectile.owner].whoAmI == Main.myPlayer && Main.player[Projectile.owner].controlUseItem && !mouseLeftState && NoAuto;
        public bool RightClick => Main.player[Projectile.owner].whoAmI == Main.myPlayer && Main.player[Projectile.owner].controlUseTile && !mouseRightState && NoAuto;

        public string CurrentAnimation 
            { 
            get => currentAnimation;
            set {
                if(!string.IsNullOrWhiteSpace(currentAnimation) && animations.TryGetValue(currentAnimation, out SpriteAnimation anim))
                    anim.Reset();
                currentAnimation = value;
            }
        }

        public Vector2 MousePosition(Player player)
        {
            if (Projectile.owner == Main.myPlayer)
                return Main.MouseWorld;

            return StandPlayer.Get(player).MousePosition;
        }
        public Vector2 GetTargetPosition(Player player)
        {
            var plr = StandPlayer.Get(player);

            if (plr.standAutoMode)
            {
                NPC npc = Projectile.FindTargetWithinRange(searchRange);

                if (npc != null && npc.active)
                {
                    return npc.Center;
                }
            }

            return MousePosition(player);

        }

        public void Add(string animationName, SpriteAnimation animation)
        {
            animations.Add(animationName, animation);
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.TryGetOwner(out var plr))
                StandPlayer.Get(plr).activeStandProjectile = StandPlayer.INACTIVE_STAND_ID;
            
        }

        public bool FirstTick => Projectile.timeLeft <= 10;

        public void Damage(int value, Vector2 position, Vector2 size, bool follow = true, int timeLeft = 2, float knockback = 0)
        {
            Player plr = Main.player[Projectile.owner];
            int hitboxId = Projectile.NewProjectile(plr.GetSource_FromThis("JTA: Damage Hitbox"), position, Vector2.Zero, ModContent.ProjectileType<DamageHitbox>(), value,knockback, Projectile.owner, follow ? Projectile.whoAmI : -1);

            Projectile proj = Main.projectile[hitboxId];
            proj.width = (int)size.X;
            proj.height = (int)size.Y;
            //proj.Center -= size * 0.5f;
            proj.timeLeft = timeLeft;
            proj.DamageType = Projectile.DamageType;
            proj.direction = Projectile.direction;
        }

        public bool NoAuto => !StandPlayer.Get(Main.player[Projectile.owner]).standAutoMode;
    }
}
