﻿using JTA.Common.Graphics;
using JTA.Common.Players;
using JTA.Common.Stands;
using JTA.Common.Systems;
using JTA.Content.Projectiles;
using JTA.Content.Stands.Crusaders.StarPlatinum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace JTA.Content.Stands.Crusaders
{
    /// <summary>
    /// Also check StandDefinitions.cs for additional context.
    /// This handles just the stand you see on screen.
    /// Abilities & making the stand obtainable is handle there.
    /// </summary>
    public class StarPlatinumProjectile : StandProjectile
    {
        int recoveryTime;
        int punchCounter;
        public int startDirection;

        float range;
        int direction;

        bool mouseMiddleOld;
        bool mouseMiddleReleaseOld;

        int lmbHoldTime;

        List<StarFist> fists;

        public override void SafeDefaults()
        {
            fists = [];
            for(int i = 0; i < 30; i++) {
                fists.Add(new StarFist());
            }
            range = 32 * 7;
            Projectile.timeLeft = 10;
            Projectile.tileCollide = false;
            var path = "JTA/Content/Stands/Crusaders/StarPlatinum/";
            Add("Idle", new SpriteAnimation(path + "SPIdle").FillFrames(8, 100, 100, 5));
            Add("Punch2", new SpriteAnimation(path + "SPPunch1").FillFrames(5, 140, 100, 3));
            Add("Punch1", new SpriteAnimation(path + "SPPunch2").FillFrames(7, 132, 100, 3));
            Add("Punch3", new SpriteAnimation(path + "SPPunch3").FillFrames(11, 118, 100, 3));
            Add("Barrage", new SpriteAnimation(path + "SPBarrage").FillFrames(6, 92, 100, 2));
            Add("Finger", new SpriteAnimation(path + "SPStarFinger").FillFrames(16, 292, 100, 5).SetSpeed(10, 3));
            Add("Suck", new SpriteAnimation(path + "SPZuck").FillFrames(23, 300, 202, 5));
            Add("GPJump", new SpriteAnimation(path + "SPGroundPound").FillFrames(8, 148, 206, 5).SetLoop(false));
            Add("GPFall", new SpriteAnimation(path + "SPGroundPound").FillFrames(12, 148, 206, 5).SetLoop(false));
            Add("GPHit", new SpriteAnimation(path + "SPGroundPound").FillFrames(16, 148, 206, 5).SetLoop(false));
            Add("Throw", new SpriteAnimation(path + "SPThrow").FillFrames(11, 166, 128, 5).SetLoop(false));

            Add("Block", new SpriteAnimation(path + "SPBlock").FillFrames(4, 100, 100, 5).SetLoop(false));

            Add("ChargePrep", new SpriteAnimation(path + "SPChargePrep").FillFrames(22, 178, 146, 2));
            Add("ChargeHold", new SpriteAnimation(path + "SPChargeIdle").FillFrames(4, 178, 146, 6));
            Add("ChargeAttack", new SpriteAnimation(path + "SPChargeAttack").FillFrames(25, 178, 146, 4));

            Add("Pose", new SpriteAnimation(path + "SPPose").FillFrames(6, 136, 100, 5).SetLoop(false));
            Add("PoseIdle", new SpriteAnimation(path + "SPPoseIdle").FillFrames(9, 136, 100, 5));

            Add("TimeStop", new SpriteAnimation(path + "SPTimeStop").FillFrames(24, 200, 160, 5));

            CurrentAnimation = "Spawn";

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

            animations.TryGetValue(CurrentAnimation, out var anim);

            if (owner == null)
            {
                return;
            }

            if (owner.controlUseItem) {
                lmbHoldTime++;
            }
            else {
                lmbHoldTime = 0;
            }

            if (recoveryTime > 0)
                recoveryTime--;

            var mousePosition = Main.MouseWorld;
            if (Projectile.owner != Main.myPlayer)
                mousePosition = MousePosition(owner);
            //Main.NewText($"PLR: {mousePosition} MOUSE: {Main.MouseWorld}");
            var dir = (mousePosition - owner.Center).SafeNormalize(-Vector2.UnitX) * (Math.Clamp(Vector2.Distance(owner.Center, mousePosition), 0, range));

            switch (CurrentAnimation) {
                case "Spawn":
                    CurrentAnimation = "Idle";
                    break;
                case "TimeStop":
                    if(anim.time == 0 && anim.currentFrame == 18) {
                        TimeStopSystem.StopTimeFor(owner, 360);
                        Projectile.NewProjectile(owner.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShockwaveProjectile>(), 0, 0, Projectile.owner);
                    }
                    nextAnimation = "Idle";
                    break;
                case "Block":
                    direction = owner.direction;
                    owner.heldProj = Projectile.whoAmI;
                    Projectile.Center = owner.Center - new Vector2(0, 25);
                    StandPlayer.Get(owner).damageReduction = 0.75f;
                    if (!mouseRightState)
                        nextAnimation = "Idle";
                    break;
                case "Throw":
                    dir = (mousePosition - owner.Center).SafeNormalize(-Vector2.UnitX) * 24;
                    if (anim.currentFrame == 8 && anim.time == 0)
                        Projectile.NewProjectile(owner.GetSource_FromThis(), Projectile.Center, dir, ModContent.ProjectileType<IggyThrown>(), 60, 4, Projectile.owner);

                    nextAnimation = "Idle";
                    break;
                case "Barrage":
                    StarFist fist = fists[Main.rand.Next(fists.Count)];
                    if (fist.opacity <= 0) {
                        fist.distance = 0;
                        fist.startOffset = new Vector2(-25, Main.rand.Next(-30, 40));
                        fist.opacity = 1.25f;
                        fist.variation = Main.rand.Next(4);
                        fist.speed = Main.rand.Next(850, 1250) * 0.01f;
                    }
                    owner.heldProj = Projectile.whoAmI;
                    //mousePosition = Main.MouseWorld;
                    dir = (mousePosition - owner.Center).SafeNormalize(-Vector2.UnitX) * (Math.Clamp(Vector2.Distance(owner.Center, mousePosition), 0, range));

                    direction = mousePosition.X < owner.Center.X ? -1 : 1;
                    Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center + dir, 0.4f);

                    if (anim.currentFrame % 2 == 0 && anim.time == 0)
                        Damage(10, Projectile.Center, new Vector2(160, 100));

                    if (!mouseLeftState)
                        nextAnimation = "Idle";
                    break;
                case "Idle":

                    if (mouseRightState)
                        CurrentAnimation = "Block";

                    if (lmbHoldTime >= 30)
                        CurrentAnimation = "Barrage";

                    if (Main.myPlayer == Projectile.owner) {
                        if (KeybindSystem.SummonStand.JustPressed && Projectile.timeLeft <= 10)
                            CurrentAnimation = "Despawn";

                        if (KeybindSystem.StandPose.JustPressed) {
                            StandPlayer.Get(owner).parryTime = 30;
                            CurrentAnimation = "Pose";
                        }
                        if (Main.mouseMiddle && !mouseMiddleReleaseOld) {
                            CurrentAnimation = "ChargePrep";
                        }
                    }
                    direction = owner.direction;
                    if (recoveryTime <= 0) {
                        punchCounter = 0;
                        Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center + new Vector2(-50 * direction, -24), 0.12f);
                    }
                    else {
                        direction = mousePosition.X < owner.Center.X ? -1 : 1;
                        Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center + dir, 0.4f);
                    }

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
                    recoveryTime = 15;
                    owner.heldProj = Projectile.whoAmI;

                    direction = mousePosition.X < owner.Center.X ? -1 : 1;

                    // reason we check both frame & frame time is so we only spawn 1 hitbox per punch
                    if (anim.time == 0 && anim.currentFrame == 1 + punchCounter) {
                        Damage(60, Projectile.Center, new Vector2(140, 100), true, 2, 6);
                    }

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
                    owner.heldProj = Projectile.whoAmI;
                    direction = startDirection;
                    Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center + new Vector2(140 * direction, -24), 0.4f);
                    nextAnimation = "Idle";

                    anim = animations[CurrentAnimation];

                    if (anim.time == 0 && anim.currentFrame == 10) {
                        Damage(60, Projectile.Center, new Vector2(300, 60), true, 10, 10);
                    }

                    break;
                case "Suck":
                    owner.heldProj = Projectile.whoAmI;
                    direction = startDirection;
                    Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center + new Vector2(140 * direction, -24), 0.4f);
                    anim = animations[CurrentAnimation];

                    var rect = new Rectangle((int)Projectile.Center.X - 50 * direction - (direction == -1 ? 500 : 0), (int)Projectile.Center.Y - 50, 500, 302);
                    if (anim.currentFrame >= 4) {
                        for (int i = 0; i < Main.maxNPCs; i++) {
                            NPC npc = Main.npc[i];

                            if (!npc.active || npc.type == NPCID.TargetDummy)
                                continue;

                            if (rect.Contains(npc.Center.ToPoint())) {
                                npc.Center += (Projectile.Center - npc.Center).SafeNormalize(-Vector2.UnitY) * 6f;
                            }
                        }
                    }
                    nextAnimation = "Idle";
                    break;
                case "ChargePrep":
                    owner.heldProj = Projectile.whoAmI;
                    //mousePosition = Main.MouseWorld;
                    dir = (mousePosition - owner.Center).SafeNormalize(-Vector2.UnitX) * (Math.Clamp(Vector2.Distance(owner.Center, mousePosition), 0, range));

                    direction = mousePosition.X < owner.Center.X ? -1 : 1;

                    Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center + dir, 0.4f);
                    nextAnimation = "ChargeHold";
                    break;
                case "ChargeHold":
                    owner.heldProj = Projectile.whoAmI;
                   // mousePosition = Main.MouseWorld;
                    dir = (mousePosition - owner.Center).SafeNormalize(-Vector2.UnitX) * (Math.Clamp(Vector2.Distance(owner.Center, mousePosition), 0, range));

                    direction = mousePosition.X < owner.Center.X ? -1 : 1;

                    Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center + dir, 0.4f);
                    if (Main.myPlayer == Projectile.owner && !Main.mouseMiddle)
                        nextAnimation = "ChargeAttack";
                    break;
                case "ChargeAttack":
                    owner.heldProj = Projectile.whoAmI;
                    anim = animations[CurrentAnimation];

                    //mousePosition = Main.MouseWorld;
                    dir = (mousePosition - owner.Center).SafeNormalize(-Vector2.UnitX) * (Math.Clamp(Vector2.Distance(owner.Center, mousePosition), 0, range));

                    direction = mousePosition.X < owner.Center.X ? -1 : 1;

                    Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center + dir, 0.4f);

                    if (anim.currentFrame == 4 && anim.time <= 0) {
                        Damage(600, Projectile.Center, new Vector2(200, 200));
                    }

                    nextAnimation = "Idle";
                    break;
                case "GPJump":
                    direction = owner.direction;
                    owner.heldProj = Projectile.whoAmI;
                    Projectile.Center = owner.Center + new Vector2(20 * direction, -30);
                    anim = animations[CurrentAnimation];

                    if (anim.currentFrame == 7 && anim.time <= 0) {
                        owner.velocity.Y = -16f;
                    }
                    nextAnimation = "GPFall";
                    break;
                case "GPFall":
                    direction = owner.direction;
                    owner.heldProj = Projectile.whoAmI;
                    Projectile.Center = owner.Center;
                    if (anim.currentFrame == 0 && anim.time <= 0) {
                        anim.currentFrame = 8;
                    }

                    for(int i = 0; i < Main.maxNPCs; i++) {
                        var npc = Main.npc[i];
                        bool? canHit = CanHitNPC(npc);
                        if (!npc.active || canHit.GetValueOrDefault())
                            continue;

                        if (Projectile.Hitbox.Intersects(npc.Hitbox))
                            nextAnimation = "GPHit";
                    }

                    if (owner.TouchedTiles.Count > 0)
                        nextAnimation = "GPHit";
                    break;
                case "GPHit":
                    direction = owner.direction;
                    owner.heldProj = Projectile.whoAmI;
                    Projectile.Center = owner.Center - new Vector2(0, 70);
                    if (anim.currentFrame == 0 && anim.time <= 0) {
                        anim.currentFrame = 13;
                        Damage(60, Projectile.Center + new Vector2(0, 70), new Vector2(300, 100), false, 10, 6);
                    }

                    nextAnimation = "Idle";
                    break;
                case "Pose":
                    if ((Main.myPlayer == Projectile.whoAmI && !KeybindSystem.StandPose.Current))
                        nextAnimation = "Idle";
                    else
                        nextAnimation = "PoseIdle";
                    Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center + new Vector2(10 * direction, -24), 0.4f);
                    owner.heldProj = Projectile.whoAmI;
                    direction = owner.direction;
                    break;
                case "PoseIdle":
                    if ((Main.myPlayer == Projectile.whoAmI && !KeybindSystem.StandPose.Current))
                        nextAnimation = "Idle";

                    Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center + new Vector2(40 * direction, -24), 0.4f);
                    owner.heldProj = Projectile.whoAmI;
                    direction = owner.direction;
                    break;
                case "Despawn":
                    Projectile.Kill();
                    StandPlayer.Get(owner).activeStandProjectile = StandPlayer.UNSUMMONED;
                    break;
            }
            if (Main.myPlayer == Projectile.owner) {
                mouseMiddleOld = Main.mouseMiddle;
                mouseMiddleReleaseOld = Main.mouseMiddleRelease;
            }

            for (int i = 0; i < fists.Count; i++) {
                var fist = fists[i];
                if (fist.opacity > 0)
                    fist.opacity -= 0.24f;
                fist.distance += fist.speed;
            }
        }

        public override void PostDraw(Color lightColor)
        {
            if (animations.TryGetValue(CurrentAnimation, out SpriteAnimation anim)) {

                Asset<Texture2D> fistTexture = ModContent.Request<Texture2D>("JTA/Content/Stands/Crusaders/StarPlatinum/StarFists");
                //Projectile.TryGetOwner(out var plr);
                var scale = new Vector2(1);
                var sfx = direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                var texture = anim.texture.Value;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);
                if(CurrentAnimation == "GPJump")
                    anim.Draw(Main.spriteBatch, texture, (Projectile.Center - Main.screenPosition).Floor(), Color.White * 0.75f, 0, scale: scale * 0.5f, sfx: sfx);
                else
                    anim.Draw(Main.spriteBatch, texture, (Projectile.Center - Main.screenPosition).Floor(), Color.White, 0, scale: scale, sfx: sfx);
                var frame = anim.frames[anim.currentFrame];
                Effect fx = ShaderSystem.FadeOutShader;

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.ZoomMatrix);

                fx.Parameters["uImageSize0"].SetValue(new Vector2(texture.Width, texture.Height));
                fx.Parameters["uSourceRect"].SetValue(new Vector4(frame.x, frame.y, frame.width, frame.height));
                fx.Parameters["beginPoint"].SetValue(0.795f);
                fx.Parameters["speed"].SetValue(6);
                fx.CurrentTechnique.Passes[0].Apply();

                anim.Draw(Main.spriteBatch, texture, (Projectile.Center - Main.screenPosition).Floor(), Color.White, 0, scale: scale, frameOffset: new Vector2(frame.width, 0), sfx: sfx);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

                if (CurrentAnimation == "Barrage")
                    for (int i = 0; i < fists.Count; i++) {
                        var fist = fists[i];
                        var fistFrame = new Rectangle(0, 18 * fist.variation, 44, 18);
                        var finalOffset = new Vector2(fist.startOffset.X * -direction + fist.distance * direction, fist.startOffset.Y);
                        Main.EntitySpriteDraw(fistTexture.Value, Projectile.Center + finalOffset - Main.screenPosition, fistFrame, Color.White * fist.opacity, 0f, new Vector2(22, 9), 1f, sfx);
                    }
            }
        }
    }

    class StarFist
    {
        public int variation;
        public float opacity;
        public float distance;
        public float speed;
        public Vector2 startOffset;
    }
}
