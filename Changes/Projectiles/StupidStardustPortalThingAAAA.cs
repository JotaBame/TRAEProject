﻿using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
namespace TRAEProject.Changes.Projectiles
{
    public class BezierCurveProjThing : ModProjectile
    {
        public bool ValidHomingTarget(int targetIndex)
        {
            NPC npc = Main.npc[targetIndex];
            if (npc.active && npc.chaseable && npc.lifeMax > 5 && (!npc.dontTakeDamage) && !npc.friendly && Projectile.localNPCImmunity[targetIndex] <= 0)
            {
                return !npc.immortal;
            }
            return false;
        }
        public int FindTargetWithLineOfSight(float maxRange = 800f)
        {
            float maxRangeSquared = maxRange * maxRange;
            int finalTargetIndex = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (ValidHomingTarget(i))
                {
                    float distanceSquaredToTarget = Projectile.DistanceSQ(npc.Center);
                    if (distanceSquaredToTarget < maxRangeSquared && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                    {
                        maxRangeSquared = distanceSquaredToTarget;
                        finalTargetIndex = i;
                    }
                }
            }
            return finalTargetIndex;
        }
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.FairyQueenMagicItemShot;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.extraUpdates = 2;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.Size = new Vector2(20, 20);
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 360;
            Projectile.alpha = 0;
        }
        public override void AI()
        {
            ModifiedNightglowAI();
        }
        private void ModifiedNightglowAI()
        {
            bool shouldCurve = false;
            bool chaseTargets = false;
            float curveTime = 180f;
            float homingTime = 20f;
            float decelerationAmount = 0.98f;
            float minSmoothStepAmount = 0.075f;
            float maxSmoothStepAmount = 0.125f;
            float maxVel = 26f;//931 is nightglow proj
            if (Projectile.timeLeft == 357)
            {
                int alpha = Projectile.alpha;
                Projectile.alpha = 0;
                Color fairyQueenWeaponsColor = Projectile.GetFairyQueenWeaponsColor();
                Projectile.alpha = alpha;//this is so the projectile keeps the alpha after calling GetFairyQUeenWeaponsColor
                for (int i = 0; i < 3; i++)
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, 267, Main.rand.NextVector2CircularEdge(3f, 3f) * (Main.rand.NextFloat() * 0.5f + 0.5f), 0, fairyQueenWeaponsColor);
                    dust.scale *= 1.2f;
                    dust.noGravity = true;
                }
            }

            if (Projectile.timeLeft > curveTime)
            {
                shouldCurve = true;
            }
            else if (Projectile.timeLeft > homingTime)
            {
                chaseTargets = true;
            }
            if (shouldCurve)
            {
                float amountToCurve = (float)Math.Cos(Projectile.whoAmI % 6f / 6f + Projectile.position.X / 320f + Projectile.position.Y / 160f);
                amountToCurve *= Utils.GetLerpValue(curveTime, curveTime + 120, Projectile.timeLeft, true);
                Projectile.velocity *= decelerationAmount;
                Projectile.velocity = Projectile.velocity.RotatedBy(amountToCurve * MathF.PI * 0.5f * 1f / 30f);
            }

            int targetIndex = (int)Projectile.ai[0];
            if (Main.npc.IndexInRange(targetIndex) && !ValidHomingTarget(targetIndex))
            {
                targetIndex = -1;
                Projectile.ai[0] = -1f;
                Projectile.netUpdate = true;
            }
            if (targetIndex == -1)
            {
                int newTargetIndex = FindTargetWithLineOfSight(8000);
                if (newTargetIndex != -1)
                {
                    Projectile.ai[0] = newTargetIndex;
                    Projectile.netUpdate = true;
                }
            }
            if (chaseTargets)
            {
                int targetIndexAGAIN = (int)Projectile.ai[0];
                Vector2 targetVelocityOrSomething = Projectile.velocity;
                if (Main.npc.IndexInRange(targetIndexAGAIN))
                {
                    if (Projectile.timeLeft < 30)
                    {
                        Projectile.timeLeft = 30;
                        maxVel *= 2;
                    }
                    NPC nPC = Main.npc[targetIndexAGAIN];
                    targetVelocityOrSomething = Projectile.DirectionTo(nPC.Center) * maxVel;
                }
                //else
                //{
                //    Projectile.timeLeft--;
                //}
                float amount = MathHelper.Lerp(minSmoothStepAmount, maxSmoothStepAmount, Utils.GetLerpValue(curveTime, 30f, Projectile.timeLeft, clamped: true));
                Projectile.velocity = Vector2.SmoothStep(Projectile.velocity, targetVelocityOrSomething, amount);
                Projectile.velocity *= MathHelper.Lerp(0.85f, 1f, Utils.GetLerpValue(0, 90f, Projectile.timeLeft, clamped: true));

            }
            Projectile.Opacity = Utils.GetLerpValue(-3, 30, Projectile.timeLeft, clamped: true);
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color afterImgColor = Main.hslToRgb(Projectile.ai[1], 1, 0.5f);
            float opacityForSparkles = Utils.GetLerpValue(0, 40, Projectile.timeLeft, true);
            afterImgColor.A = (byte)(Main.dayTime ? 128 : 0);
            afterImgColor *= Projectile.Opacity;
            Main.instance.LoadProjectile(ProjectileID.FairyQueenMagicItemShot);
            Texture2D texture = TextureAssets.Projectile[ProjectileID.FairyQueenMagicItemShot].Value;
            Vector2 scale = Vector2.Clamp(new Vector2(1, MathF.Abs(Projectile.oldVelocity.Y)), Vector2.One, Vector2.One * 2.5f);
            for (int i = Projectile.oldPos.Length - 1; i >= 0; i--)
            {
                float opacity = 1 - (float)i / Projectile.oldPos.Length;
                Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition, null, afterImgColor * opacity, Projectile.oldRot[i], texture.Size() / 2, scale, SpriteEffects.None);
            }
            
            Texture2D sparkleTexture = TextureAssets.Extra[98].Value;
            Vector2 sparkleOrigin = sparkleTexture.Size() / 2;
            float sparkleIntensity = MathF.Cos(Main.GlobalTimeWrappedHourly * 27) * 0.075f + 0.3f;
            afterImgColor *= MathF.Pow(opacityForSparkles, 2);
            Vector2 scaleX = new Vector2(1.2f, 5f) * sparkleIntensity * opacityForSparkles;
            Vector2 scaleY = new Vector2(1.2f, 2f) * sparkleIntensity * opacityForSparkles;
            scaleX *= 0.4f;
            scaleY *= 0.4f;
            scaleY *= 4;
            scaleX *= 4;
            Main.EntitySpriteDraw(sparkleTexture, Projectile.Center - Main.screenPosition, null, afterImgColor, MathF.PI / 2f, sparkleOrigin, scaleX, SpriteEffects.None,0);
            Main.EntitySpriteDraw(sparkleTexture, Projectile.Center - Main.screenPosition, null, afterImgColor, 0f, sparkleOrigin, scaleY, SpriteEffects.None,0);
            Main.EntitySpriteDraw(sparkleTexture, Projectile.Center - Main.screenPosition, null, afterImgColor * 0.5f, MathF.PI / 2f, sparkleOrigin, scaleX * 0.6f, SpriteEffects.None,0);
            Main.EntitySpriteDraw(sparkleTexture, Projectile.Center - Main.screenPosition, null, afterImgColor * 0.5f, 0f, sparkleOrigin, scaleY * 0.6f, SpriteEffects.None,0);
            
            
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White * Projectile.Opacity, Projectile.rotation, texture.Size() / 2, 1, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255,255,255,0) * Projectile.Opacity, Projectile.rotation, texture.Size() / 2, 1, SpriteEffects.None, 0);
            
            return false;
        }
    }

    public class StupidStardustPortalThingAAAA : GlobalProjectile
    {
        static Color[] goldPortalColors = new Color[6] { Color.White, new Color(250, 234, 192), new Color(250, 216, 124), new Color(250, 176, 0), new Color(183, 106, 3), new Color(91, 57, 29) };
        static Color[] bluePortalColors = new Color[6] { Color.White, new Color(115, 223, 255), new Color(35, 200, 255), new Color(104, 214, 255), new Color(0, 174, 238), new Color(0, 106, 185) };

        static void DrawPortal(Color[] colors, Vector2 pos, float rotation, float colorMult = 1, SpriteEffects spriteEffects = SpriteEffects.None, bool fullyOpaque = false, float scale = 1, byte opacity = 255)
        {
            for (int i = 0; i < colors.Length; i++)//INPUT ARRAY SHOULD BE 6 ELEMENTS LONG
            {
                string texturePath = "TRAEProject/Assets/SpecialTextures/PortalLayer" + (i + 1);
                if (fullyOpaque)
                    texturePath += "O";
                Texture2D texture = ModContent.Request<Texture2D>(texturePath).Value;

                Color colorToDraw = colors[i] * colorMult;
                colorToDraw.A = opacity;
                Main.EntitySpriteDraw(texture, pos, null, colorToDraw, rotation, texture.Size() / 2, scale, spriteEffects, 0);
            }
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (projectile.type == ProjectileID.MoonlordTurret)
            {
                projectile.scale = projectile.ai[1];
                float rotationColorOscillation = 0.95f + (projectile.rotation * 0.75f).ToRotationVector2().Y * 0.1f;
                Vector2 drawPos = projectile.Center - Main.screenPosition;
                               Color white = Color.White;
                float projRotation = (float)(projectile.rotation / 1.75f);
                Color lightTransparentGray = white * 0.8f;
                lightTransparentGray.A /= 2;
                float drawScale = projectile.scale * 1.2f * rotationColorOscillation;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                DrawPortal(bluePortalColors, drawPos, projRotation + 0.35f, 0.7f, SpriteEffects.FlipHorizontally, true, drawScale);
                DrawPortal(bluePortalColors, drawPos, projRotation, 1, SpriteEffects.FlipHorizontally, true, projectile.scale);
                DrawPortal(bluePortalColors, drawPos, -projRotation, 0.8f, SpriteEffects.None, true, 0.9f * projectile.scale);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < goldPortalColors.Length; j++)//INPUT ARRAY SHOULD BE 6 ELEMENTS LONG
                    {
                        Texture2D portalLayer = ModContent.Request<Texture2D>("TRAEProject/Assets/SpecialTextures/PortalLayer" + (i + 1)).Value;
                        Color colorToDraw = goldPortalColors[j] * 0.5f;
                        colorToDraw.A = 255;
                        colorToDraw.A = (byte)(255 * (1 - (float)j / (float)5));
                        colorToDraw.A *= 2;
                        Main.EntitySpriteDraw(portalLayer, drawPos, null, colorToDraw, projRotation * 0.7f, portalLayer.Size() / 2, projectile.scale, SpriteEffects.FlipHorizontally, 0);
                        Main.EntitySpriteDraw(portalLayer, drawPos, null, colorToDraw, -projRotation * 0.7f, portalLayer.Size() / 2, projectile.scale, SpriteEffects.None, 0);
                    }
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                DrawPortal(goldPortalColors, drawPos, projRotation * 0.7f, 0.6f, SpriteEffects.FlipHorizontally, true, projectile.scale);
                DrawPortal(goldPortalColors, drawPos, -projRotation * 0.7f, 0.6f, SpriteEffects.None, false, projectile.scale);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                Texture2D texture = ModContent.Request<Texture2D>("TRAEProject/Assets/SpecialTextures/GoldenShine").Value;
                Color shineColor = Color.White;
                float waveAmplitude = 100;
                float triangleWave = (float)(1 / Math.PI * Math.Asin(Math.Sin(2 * Math.PI / waveAmplitude * Main.timeForVisualEffects)) + 0.5);
                shineColor.A = (byte)(255 * triangleWave);
                Color altShineColor = shineColor;
                altShineColor.A = (byte)(255 - shineColor.A);
                Main.EntitySpriteDraw(texture, drawPos, null, shineColor, -projRotation, texture.Size() / 2, projectile.scale, SpriteEffects.None, 0);
                texture = TextureAssets.Extra[57].Value;
                Main.EntitySpriteDraw(texture, drawPos, null, altShineColor, projRotation, texture.Size() / 2, projectile.scale, SpriteEffects.None, 0);
                return false;
            }
            return true;
        }
        public override bool InstancePerEntity => true;
        public static Vector2 CubicBezier(Vector2 start, Vector2 controlPoint1, Vector2 controlPoint2, Vector2 end, float t)
        {
            float tSquared = t * t;
            float tCubed = t * t * t;
            return
                start * (-tCubed + 3 * tSquared - 3 * t - 1) +
                controlPoint1 * (3 * tCubed - 6 * tSquared + 3 * t) +
                controlPoint2 * (-3 * tCubed + 3 * tSquared) +
                end * tCubed;
        }//DELETE THIS LATER
        public override bool PreAI(Projectile projectile)
        {
            if (projectile.type == ProjectileID.MoonlordTurret)
            {
                if (projectile.scale > 0.05f)
                {
                    if (Main.rand.NextBool())
                    {
                        Vector2 randomRotationUnitVec = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi);
                        Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center - randomRotationUnitVec * Main.rand.Next(20, 31) * projectile.scale - new Vector2(4, 4), 0, 0, DustID.YellowTorch)];
                        spawnedDust.noGravity = true;
                        spawnedDust.velocity = randomRotationUnitVec * 2f * projectile.scale;
                        spawnedDust.scale = 0.5f + Main.rand.NextFloat() * projectile.scale * 0.6f;
                        spawnedDust.fadeIn = projectile.scale;
                        spawnedDust.customData = projectile.Center;
                    }
                    if (Main.rand.NextBool())
                    {
                        Vector2 randomRotationUnitVec = Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi);
                        Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center - randomRotationUnitVec * 30f * projectile.scale - new Vector2(4, 4), 0, 0, DustID.BlueCrystalShard)];
                        spawnedDust.noGravity = true;
                        spawnedDust.velocity = randomRotationUnitVec.RotatedBy(-MathHelper.PiOver2) * 3f * projectile.scale;
                        spawnedDust.scale = 0.5f + Main.rand.NextFloat() * projectile.scale * 1.6f;
                        spawnedDust.fadeIn = 0.5f * projectile.scale;
                    }
                }//FIRERATE is 17
                if (projectile.ai[0] % 17 == 0 && projectile.ai[0] > 20)
                {
                    float color = Main.rand.Next(0, 2) * 0.5f + 0.09f + Main.rand.NextFloat() / 20;
                    Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, new Vector2(Main.rand.NextFloat() + 3).RotatedByRandom(MathF.Tau), ModContent.ProjectileType<BezierCurveProjThing>(), projectile.damage, 3, Main.myPlayer, 0, color).netUpdate = true;
                }
                projectile.ai[0]++;
                projectile.ai[1] = MathHelper.Lerp(projectile.ai[1], 1, 0.1f);
                projectile.velocity = Vector2.Zero;
                projectile.rotation += 0.08f;
                return false;
            }
            return true;
        }
        public override void Unload()
        {
            bluePortalColors = null;
            goldPortalColors = null;
        }
    }
}