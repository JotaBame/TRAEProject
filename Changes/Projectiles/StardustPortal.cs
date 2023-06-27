using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using System;
using TRAEProject.Common;
using TRAEProject.NewContent.SummonReforges;

namespace TRAEProject.Changes.Projectiles
{
    public class StardustPortalProj : ModProjectile
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
                    if (distanceSquaredToTarget < maxRangeSquared)
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 24;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
        }
        static int baseTimeLeft = 4000;
        TestForSummonReforgesMinionChanges reforgeStats;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Summon;
            try//maybe bad idea
            {
                reforgeStats = Projectile.GetGlobalProjectile<TestForSummonReforgesMinionChanges>();//ok so basically the mod throws an error on loading.
            }//so I used try catch to circumvent it
            catch (Exception)
            {
            }
            Projectile.penetrate = 3;
            Projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.15f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.extraUpdates = 2;
            Projectile.Size = new Vector2(20, 20);
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = baseTimeLeft;
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
            float curveTime = baseTimeLeft - 180;
            float homingTime = 20f;
            float aggroRange = 1000 * reforgeStats.GetAggroRangeBoost(Projectile.owner);
            float decelerationAmount = 0.98f;
            float minSmoothStepAmount = 0.075f * reforgeStats.GetMoveAcceleration(Projectile.owner);
            float maxSmoothStepAmount = 0.125f * reforgeStats.GetMoveAcceleration(Projectile.owner);
            float maxVel = 26f * reforgeStats.GetMoveSpeed(Projectile.owner);
            if (Projectile.timeLeft == baseTimeLeft - 3)
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
            if ((int)Projectile.ai[0] == -1 || shouldCurve)
                Projectile.velocity *= decelerationAmount;
            if (shouldCurve)
            {
                float amountToCurve = (float)Math.Cos(Projectile.whoAmI % 6f / 6f + Projectile.position.X / 320f + Projectile.position.Y / 160f);
                amountToCurve *= Utils.GetLerpValue(curveTime, curveTime + 120, Projectile.timeLeft, true);
                if(Projectile.velocity.LengthSquared() > 0.1f)
                    Projectile.velocity = Projectile.velocity.RotatedBy(amountToCurve * MathF.PI * 0.5f * 1f / 30f);
            }

            int targetIndex = (int)Projectile.ai[0];
            if (Main.npc.IndexInRange(targetIndex) && !ValidHomingTarget(targetIndex))
            {
                targetIndex = -1;
                Projectile.ai[0] = -1f;
                Projectile.netUpdate = true;
            }
            if (targetIndex == -1 && Projectile.timeLeft % 10 == 0)//only search for a target every 10th update(projectile has 2 extra updates) to improve performance
            {
                int newTargetIndex = FindTargetWithLineOfSight(aggroRange);//TODO: TEST THIS
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
                float amount = MathHelper.Lerp(minSmoothStepAmount, maxSmoothStepAmount, Utils.GetLerpValue(curveTime, 30f, Projectile.timeLeft, true));
                Projectile.velocity = Vector2.SmoothStep(Projectile.velocity, targetVelocityOrSomething, amount);
                Projectile.velocity *= MathHelper.Lerp(0.85f, 1f, Utils.GetLerpValue(0, 90f, Projectile.timeLeft, true));
            }
            Projectile.Opacity = Utils.GetLerpValue(-3, 30, Projectile.timeLeft, clamped: true);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color afterImgColor = Main.hslToRgb(Projectile.ai[1], 1, 0.5f);
            float opacityForSparkles = Utils.GetLerpValue(0, 40, Projectile.timeLeft, true);
            afterImgColor.A = (byte)(Main.dayTime ? 0 : 0);
            afterImgColor *= Projectile.Opacity;
            Main.instance.LoadProjectile(ProjectileID.FairyQueenMagicItemShot);
            Texture2D texture = TextureAssets.Projectile[ProjectileID.FairyQueenMagicItemShot].Value;
            for (int i = Projectile.oldPos.Length - 1; i >= 0; i--)
            {
                float opacity = 1 - (float)i / Projectile.oldPos.Length;
                int oldPosIndex = (int)MathHelper.Clamp(i - 1, 0, 100000);
                Vector2 oldScale = Vector2.Clamp(new Vector2(1, MathF.Abs((Projectile.oldPos[i] - Projectile.oldPos[oldPosIndex]).Length() * 0.3f)), Vector2.One, Vector2.One * 4000f);
                Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition, null, afterImgColor * opacity, Projectile.oldRot[i], texture.Size() / 2, oldScale, SpriteEffects.None);
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

    public class StardustPortal : GlobalProjectile
    {
        TestForSummonReforgesMinionChanges reforgeStats;
        public override void SetDefaults(Projectile entity)
        {
            if(entity.type == ProjectileID.MoonlordTurret)
            {
                reforgeStats = entity.GetGlobalProjectile<TestForSummonReforgesMinionChanges>();
            }
        }
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
                const float waveAmplitude = 100;
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
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.MoonlordTurret;
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
                }
                int fireRate = (int)(35 * reforgeStats.GetAttackRateAsTimerThresholdMultiplier(projectile.owner));
                if ((int)projectile.ai[0] % fireRate == 0 && projectile.ai[0] > 20)
                {
                    float color = Main.rand.Next(0, 2) * 0.5f + 0.09f + Main.rand.NextFloat() / 20;
                    Projectile.NewProjectileDirect(projectile.GetSource_FromThis(), projectile.Center, new Vector2(Main.rand.NextFloat() + 3).RotatedByRandom(MathF.Tau) * 1.4f, ModContent.ProjectileType<StardustPortalProj>(), projectile.damage, 3, Main.myPlayer,-1, color).netUpdate = true;
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
