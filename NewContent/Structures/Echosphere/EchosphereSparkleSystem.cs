using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Structures.Echosphere.Generation;

namespace TRAEProject.NewContent.Structures.Echosphere
{
    public class EchosphereSparkleSystem : ModSystem
    {
        public const int MaxEchosphereEdgeSparkles = 400;
        public static EchosphereEdgeSparkle[] echosphereEdgeSparkles = new EchosphereEdgeSparkle[MaxEchosphereEdgeSparkles + 1];

        public static Vector2 particleCenter;
        public override void PreUpdatePlayers()
        {
           // if (!Main.mouseRight)
            {
                return;
            }
            Vector2 topLeft = EchosphereGeneratorSystem.echosphereTopLeft;
            Vector2 bottomRight = EchosphereGeneratorSystem.echosphereBottomRight;
            float stepSize = 32f;
            float width = (bottomRight.X - topLeft.X);
            float height = (bottomRight.Y - topLeft.Y);
            Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
            float startJ = screenCenter.X - 16 * 40;
            float endJ = screenCenter.X + 16 * 40;
            for (int i = 0; i < 2; i++)
            {
                for (float j = startJ; j < endJ; j += stepSize)
                {
                    Vector2 pos = Vector2.Zero;
                    pos.Y += height * i + Main.rand.NextFloat(-stepSize * .5f, stepSize * .5f);
                    pos.X += j + Main.rand.NextFloat(-stepSize * .5f, stepSize * .5f);
                    Vector2 vel = RandCircularEven(Main.rand.NextFloat(.5f,2f));
                    pos -= vel * 20;
                    NewEchosphereEdgeSparkle(pos,Vector2.One, vel);
                }
            }
        }
        private static void InsideSparkles_Old(Vector2 center, float innerSparkleRate, Vector2 scale, int sparkleDuration, float outerRadiusReal)
        {
            sparkleDuration *= 2;
            for (int i = 0; i < innerSparkleRate; i++)
            {
                if (Main.rand.NextBool(sparkleDuration))
                {
                    Vector2 spawnOffset = RandCircularEven(outerRadiusReal);
                    Vector2 vel = RandCircularEven(outerRadiusReal - 16 * 4f);
                    vel = Vector2.Normalize(vel - spawnOffset) * Main.rand.NextFloat(1, 2f);
                    Sparkle.NewSparkle(center + spawnOffset, Color.White, scale, vel, sparkleDuration);
                }
            }
        }

        private static void GetParticleParams_Old(out Vector2 center, out float innerRadius, out float innerSparkleRate, out Vector2 scale, out int sparkleDuration, out float toPlayer, out float particleSpread, out int count, out float outerRadiusReal)
        {
            center = particleCenter;
            innerRadius = 550;
            float radiusThickness = 32;
            float outerRadius = innerRadius + radiusThickness;
            innerSparkleRate = outerRadius * outerRadius * MathF.PI * 0.000005f;
            scale = new(1f, 2f);
            sparkleDuration = 100;
            Vector2 playerCenter = Main.LocalPlayer.Center;
            Vector2 playerVel = Main.LocalPlayer.position - Main.LocalPlayer.oldPosition;
            Vector2 toPlayerDeltaPos = (playerCenter + playerVel * 10) - center;
            float distToPlayer = toPlayerDeltaPos.Length();
            toPlayer = toPlayerDeltaPos.ToRotation();
            Color purple = Color.Purple;
            purple.A = 0;//making additive
            float playerDIstToOuterRadius = MathF.Abs(distToPlayer - outerRadius);
            particleSpread = Utils.Remap(playerDIstToOuterRadius, 16f * 3f, 16f * 16f, 0.001f, 2f);
            count = 1;
            if (playerDIstToOuterRadius < 16 * 7f)
            {
                count = 3;
            }
            outerRadiusReal = outerRadius;
            toPlayerDeltaPos = (playerCenter) - center;
            distToPlayer = toPlayerDeltaPos.Length();
            outerRadius = innerRadius + radiusThickness * .5f;//using the variable as dist to middle of radius
            playerDIstToOuterRadius = MathF.Abs(distToPlayer - outerRadius);
            if (playerDIstToOuterRadius < 16 * 2f + radiusThickness)
            {
                count = 0;
            }
        }

        private static void EdgeParticles_Old(Vector2 center, float innerRadius, Vector2 scale, int sparkleDuration, float toPlayer, float particleSpread, int count, float outerRadiusReal)
        {
            for (int i = 0; i < count; i++)
            {
                if (Main.rand.NextBool())
                {
                    Vector2 vel = RandInRing(0f, .4f);
                    Vector2 spawnPos = center + RandInRing2(innerRadius, outerRadiusReal, toPlayer, particleSpread);
                    NewEchosphereEdgeSparkle(spawnPos, scale, vel, sparkleDuration);
                }
            }
        }

        public override void PostUpdatePlayers()
        {
            for (int i = 0; i < MaxEchosphereEdgeSparkles; i++)
            {
                if (echosphereEdgeSparkles[i].Active)
                {
                    echosphereEdgeSparkles[i].Update();
                }
            }
        }
        public static Vector2 RandInRing2(float minDist, float maxDist, float angle, float maxAngleOffset)
        {

            float minDistNormalized = minDist / maxDist;
            minDistNormalized *= minDistNormalized;//compensate for the square rooting.
            float dist = Main.rand.NextFloat() * (1 - minDistNormalized) + minDistNormalized;
            dist = MathF.Sqrt(dist);//even out distribution of points
            dist *= maxDist;
            maxAngleOffset *= 0.5f;
            angle += Main.rand.NextFloat(-maxAngleOffset, maxAngleOffset);
            return new Vector2(MathF.Cos(angle) * dist, MathF.Sin(angle) * dist);
        }
        public static Vector2 RandInRing(float minDist, float maxDist)
        {
            float minDistNormalized = minDist / maxDist;
            minDistNormalized *= minDistNormalized;//compensate for the square rooting.
            float dist = Main.rand.NextFloat() * (1 - minDistNormalized) + minDistNormalized;
            dist = MathF.Sqrt(dist);//even out distribution of points
            dist *= maxDist;
            float angle = Main.rand.NextFloat() * MathF.PI * 2;
            return new Vector2(MathF.Cos(angle) * dist, MathF.Sin(angle) * dist);
        }
        public static Vector2 RandCircularEven(float radius)
        {
            float dist = Main.rand.NextFloat();
            dist = MathF.Sqrt(dist);//even out distribution of points
            dist *= radius;
            float angle = Main.rand.NextFloat() * MathF.PI * 2;
            return new Vector2(MathF.Cos(angle) * dist, MathF.Sin(angle) * dist);
        }

        public static EchosphereEdgeSparkle NewEchosphereEdgeSparkle(Vector2 position, Vector2? scale = null, Vector2? velocity = null, int duration = 100)
        {
            if (ParticleSystem.OutsideScreen(position))
            {
                return echosphereEdgeSparkles[MaxEchosphereEdgeSparkles];
            }
            scale ??= Vector2.One;
            velocity ??= Vector2.Zero;
            int finalIndex = MaxEchosphereEdgeSparkles;
            for (int i = 0; i < MaxEchosphereEdgeSparkles; i++)
            {
                if (echosphereEdgeSparkles[i].Active)
                    continue;
                finalIndex = i;
                echosphereEdgeSparkles[i].Position = position;
                echosphereEdgeSparkles[i].Scale = scale.Value;
                echosphereEdgeSparkles[i].Velocity = velocity.Value;
                echosphereEdgeSparkles[i].TimeLeft = duration;
                echosphereEdgeSparkles[i].Opacity = 0.2f;//they will fade in over the course of 5 frames
                break;
            }
            return echosphereEdgeSparkles[finalIndex];
        }
        public override void PostDrawTiles()
        {
            //not sure if should use deferred or texture sort mode.
            //todo: test performance of both
            //REMEMBER THAT TEXTURE SORT MODE IS EFFECTIVELY FRONT TO BACK!

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            for (int i = 0; i < MaxEchosphereEdgeSparkles; i++)
            {
                if (i < MaxEchosphereEdgeSparkles && echosphereEdgeSparkles[i].Active)
                    echosphereEdgeSparkles[i].Draw();
            }
            for (int i = 0; i < MaxEchosphereEdgeSparkles; i++)
            {
                if (i < MaxEchosphereEdgeSparkles && echosphereEdgeSparkles[i].Active)
                    echosphereEdgeSparkles[i].DrawWhitePart();
            }
            Main.spriteBatch.End();
        }
        public override void Load()
        {
            for (int i = 0; i < echosphereEdgeSparkles.Length; i++)
                echosphereEdgeSparkles[i] = new EchosphereEdgeSparkle(i);
        }
        public static void Debug_DisplayPaddedEchosphereBounds()
        {
            if (Main.timeForVisualEffects % 5 == 0)
            {
                EchosphereSystem.GetPaddedCorners(out Vector2 topLeft, out Vector2 bottomRight, out Vector2 topRight, out Vector2 bottomLeft);
                EchosphereGenTestItem.ShootMarkerTowards(topLeft, bottomLeft);
                EchosphereGenTestItem.ShootMarkerTowards(bottomLeft, topLeft);

                EchosphereGenTestItem.ShootMarkerTowards(topRight, topLeft);
                EchosphereGenTestItem.ShootMarkerTowards(topLeft, topRight);

                EchosphereGenTestItem.ShootMarkerTowards(bottomLeft, bottomRight);
                EchosphereGenTestItem.ShootMarkerTowards(bottomRight, bottomLeft);

                EchosphereGenTestItem.ShootMarkerTowards(topRight, bottomRight);
                EchosphereGenTestItem.ShootMarkerTowards(bottomRight, topRight);
            }
        }

        public static void Debug_DisplayEchosphereBounds()
        {

            if (Main.timeForVisualEffects % 5 == 0)
            {
                EchosphereGeneratorSystem.GetCorners(out Vector2 topLeft, out Vector2 topRight, out Vector2 bottomLeft, out Vector2 bottomRight);
                EchosphereGenTestItem.ShootMarkerTowards(topLeft, bottomLeft);
                EchosphereGenTestItem.ShootMarkerTowards(bottomLeft, topLeft);

                EchosphereGenTestItem.ShootMarkerTowards(topRight, topLeft);
                EchosphereGenTestItem.ShootMarkerTowards(topLeft, topRight);

                EchosphereGenTestItem.ShootMarkerTowards(bottomLeft, bottomRight);
                EchosphereGenTestItem.ShootMarkerTowards(bottomRight, bottomLeft);

                EchosphereGenTestItem.ShootMarkerTowards(topRight, bottomRight);
                EchosphereGenTestItem.ShootMarkerTowards(bottomRight, topRight);
            }

        }
    }

    public class EchosphereSparkleTestItem : ModItem
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.HallowBossRainbowStreak;
        public override void SetDefaults()
        {
            Item.useStyle = Item.useAnimation = 3;
            Item.shoot = ProjectileID.PurificationPowder;//dummy value, needed for Shoot to execute
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            EchosphereSparkleSystem.particleCenter = Main.MouseWorld;
            return false;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Item.color = Main.hslToRgb(Main.rand.NextFloat(), 1, .5f);
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
    }
}
