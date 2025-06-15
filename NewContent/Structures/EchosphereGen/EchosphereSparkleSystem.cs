using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Structures.EchosphereGen
{
    public class EchosphereSparkleSystem : ModSystem
    {
        public const int MaxEchosphereEdgeSparkles = 400;
        public static EchosphereEdgeSparkle[] echosphereEdgeSparkles = new EchosphereEdgeSparkle[MaxEchosphereEdgeSparkles + 1];

        public static Vector2 particleCenter;
        public override void PreUpdatePlayers()
        {
            return;

            Debug_DisplayEchosphereBounds();
            return;
            Vector2 center = particleCenter;
            float innerRadius = 550;
            float radiusThickness = 32;
            float outerRadius = innerRadius + radiusThickness;
            float innerSparkleRate = outerRadius * outerRadius * MathF.PI * 0.000005f;
            Vector2 scale = new(1f, 2f);
            int sparkleDuration = 100;
            Vector2 playerCenter = Main.LocalPlayer.Center;
            Vector2 playerVel = Main.LocalPlayer.position - Main.LocalPlayer.oldPosition;
            Vector2 toPlayerDeltaPos = (playerCenter + playerVel * 10) - center;
            float distToPlayer = toPlayerDeltaPos.Length();
            float toPlayer = toPlayerDeltaPos.ToRotation();
            Color purple = Color.Purple;
            purple.A = 0;//make additive
            float playerDIstToOuterRadius = MathF.Abs(distToPlayer - outerRadius);
            float particleSpread = Utils.Remap(playerDIstToOuterRadius, 16f * 3f, 16f * 16f, 0.001f, 2f);

            int count = 1;
            if (playerDIstToOuterRadius < 16 * 7f)
            {
                count = 3;
            }
            float outerRadiusReal = outerRadius;
            toPlayerDeltaPos = (playerCenter) - center;
            distToPlayer = toPlayerDeltaPos.Length();
            outerRadius = innerRadius + radiusThickness * .5f;//using the variable as dist to middle of radius
            playerDIstToOuterRadius = MathF.Abs(distToPlayer - outerRadius);
            if (playerDIstToOuterRadius < 16 * 2f + radiusThickness)
            {
                count = 0;
            }
            //SPARKLES ON THE EDGE
            for (int i = 0; i < count; i++)
            {
                if (Main.rand.NextBool())
                {
                    Vector2 vel = RandInRing(0f, .4f);
                    Vector2 spawnPos = center + RandInRing2(innerRadius, outerRadiusReal, toPlayer, particleSpread);
                    NewEchosphereEdgeSparkle(spawnPos, scale, vel, sparkleDuration);
                }
            }

            //SPARKLES INSIDE THE ECHOSPHERE AREA
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
