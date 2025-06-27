using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Structures.Echosphere.Generation;
using TRAEProject.NewContent.Structures.Echosphere.ScreenEffect;

namespace TRAEProject.NewContent.Structures.Echosphere
{
    public class EchosphereSparkleSystem: ModSystem
    {
        public const int MaxEchosphereEdgeSparkles = 350;
        public static EchosphereEdgeSparkle[] echosphereEdgeSparkles = new EchosphereEdgeSparkle[MaxEchosphereEdgeSparkles + 1];

        public static Vector2 particleCenter;
        public override void PreUpdatePlayers()
        {
            float extraPadding = 16 * 50;
            EchosphereSystem.GetPaddedCorners(out Vector2 topLeft, out Vector2 bottomRight);
            Vector2 echosphereCenter = (topLeft + bottomRight) * .5f;
            Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
            if (screenCenter.X - extraPadding > bottomRight.X || screenCenter.X + extraPadding < topLeft.X || screenCenter.Y - extraPadding > bottomRight.Y || screenCenter.Y + extraPadding < topLeft.Y)
            {
                return;
            }

            float stepSize = 32f;
            float width = (bottomRight.X - topLeft.X);
            float height = (bottomRight.Y - topLeft.Y);
            screenCenter += (Main.LocalPlayer.position - Main.LocalPlayer.oldPosition) * 30;
            Vector2 closestPointOnEdge = screenCenter;
            float halfPosVariance = 16 * 45;
            //halfPosVariance = 100;//debug test value
            float ySideSign = MathF.Sign(screenCenter.Y - echosphereCenter.Y);
            float xSideSign = MathF.Sign(screenCenter.X - echosphereCenter.X);
            Vector2 pos;
            ProjectToNearestEchosphereEdge(ref closestPointOnEdge.X, ref closestPointOnEdge.Y, out float xOverflow, out float yOverflow);
            float startX = closestPointOnEdge.X - halfPosVariance;
            float endX = closestPointOnEdge.X + halfPosVariance;
            float startY = closestPointOnEdge.Y - halfPosVariance;
            float endY = closestPointOnEdge.Y + halfPosVariance;

            float distToLeft = MathF.Abs(screenCenter.X - topLeft.X);
            float distToRight = MathF.Abs(screenCenter.X - bottomRight.X);
            float distToTop = MathF.Abs(screenCenter.Y - topLeft.Y);
            float distToBottom = MathF.Abs(screenCenter.Y - bottomRight.Y);

            // Find closest edge
            bool closerToHorizontalEdge = MathF.Min(distToTop, distToBottom) < MathF.Min(distToLeft, distToRight);

            if (closerToHorizontalEdge)
            {
                // Vertical edges span X, Y is locked
                pos = new Vector2(Main.rand.NextFloat(startX, endX),
                                  distToTop < distToBottom ? topLeft.Y : bottomRight.Y);

                if (pos.X > bottomRight.X)
                {
                    pos.Y -= MathF.CopySign(pos.X - bottomRight.X, ySideSign);
                    pos.X = bottomRight.X;
                }
                else if (pos.X < topLeft.X)
                {
                    pos.Y -= MathF.CopySign(pos.X - topLeft.X, ySideSign);
                    pos.X = topLeft.X;
                }
            }
            else
            {
                // Horizontal edges span Y, X is locked
                pos = new Vector2(distToLeft < distToRight ? topLeft.X : bottomRight.X,
                                  Main.rand.NextFloat(startY, endY));

                if (pos.Y > bottomRight.Y)
                {
                    pos.X -= MathF.CopySign(pos.Y - bottomRight.Y, xSideSign);
                    pos.Y = bottomRight.Y;
                }
                else if (pos.Y < topLeft.Y)
                {
                    pos.X -= MathF.CopySign(pos.Y - topLeft.Y, xSideSign);
                    pos.Y = topLeft.Y;
                }
            }


            Vector2 vel = RandCircularEven(Main.rand.NextFloat(.5f, 2f));
            pos -= vel * 20;
            NewEchosphereEdgeSparkle(pos, Vector2.One, vel);

            if (Main.timeForVisualEffects % 20 == 0)
            {
                float innerSparkleBoxHalfSize = 16 * 50;//a sort of "reverse padding"
                Vector2 innerSparkleBoxCenter = screenCenter;
                innerSparkleBoxCenter.X = MathHelper.Clamp(innerSparkleBoxCenter.X, topLeft.X + innerSparkleBoxHalfSize, bottomRight.X - innerSparkleBoxHalfSize);
                innerSparkleBoxCenter.Y = MathHelper.Clamp(innerSparkleBoxCenter.Y, topLeft.Y + innerSparkleBoxHalfSize, bottomRight.Y - innerSparkleBoxHalfSize);
                pos = innerSparkleBoxCenter;
                pos.X += Main.rand.NextFloat(-innerSparkleBoxHalfSize, innerSparkleBoxHalfSize);
                pos.Y += Main.rand.NextFloat(-innerSparkleBoxHalfSize, innerSparkleBoxHalfSize);
                Sparkle.NewSparkle(pos, Color.White, new Vector2(1,1.5f), vel, 150);
            }
        }

        private static Vector2 GetParticlePos_Old(Vector2 topLeft, Vector2 bottomRight, Vector2 screenCenter, Vector2 closestPointOnEdge, float ySideSign, float xSideSign, float startX, float endX, float startY, float endY)
        {
            Vector2 pos;
            if (MathF.Min(MathF.Abs(screenCenter.X - bottomRight.X), MathF.Abs(screenCenter.X - topLeft.X)) > MathF.Min(MathF.Abs(screenCenter.Y - bottomRight.Y), MathF.Abs(screenCenter.Y - topLeft.Y)))
            {
                pos = new Vector2(Main.rand.NextFloat(startX, endX), closestPointOnEdge.Y);
                if (pos.X > bottomRight.X)
                {
                    pos.Y -= MathF.CopySign(pos.X - bottomRight.X, ySideSign);
                    pos.X = bottomRight.X;
                }
                else if (pos.X < topLeft.X)
                {
                    pos.Y -= MathF.CopySign(pos.X - topLeft.X, ySideSign);
                    pos.X = topLeft.X;
                }
            }
            else
            {
                pos = new Vector2(closestPointOnEdge.X, Main.rand.NextFloat(startY, endY));
                if (pos.Y > bottomRight.Y)
                {
                    pos.X -= MathF.CopySign(pos.Y - bottomRight.Y, xSideSign);
                    pos.Y = bottomRight.Y;
                }
                else if (pos.Y < topLeft.Y)
                {
                    pos.X -= MathF.CopySign(pos.Y - topLeft.Y, xSideSign);
                    pos.Y = topLeft.Y;
                }
            }

            return pos;
        }

        static void ProjectToNearestEchosphereEdge(ref float x, ref float y, out float xOverflow, out float yOverflow)
        {
            EchosphereSystem.GetPaddedCorners(out Vector2 topLeft, out Vector2 bottomRight);

            float clampedX = Math.Clamp(x, topLeft.X, bottomRight.X);
            float clampedY = Math.Clamp(y, topLeft.Y, bottomRight.Y);

            xOverflow = x - clampedX;
            yOverflow = y - clampedY;

            x = clampedX;
            y = clampedY;
            if (xOverflow == 0 && yOverflow == 0)
            {
                float distToLeft = Math.Abs(x - topLeft.X);
                float distToRight = Math.Abs(bottomRight.X - x);
                float distToTop = Math.Abs(y - topLeft.Y);
                float distToBottom = Math.Abs(bottomRight.Y - y);

                float minDist = Math.Min(Math.Min(distToLeft, distToRight), Math.Min(distToTop, distToBottom));

                if (minDist == distToLeft)
                    x = topLeft.X;
                else if (minDist == distToRight)
                    x = bottomRight.X;
                else if (minDist == distToTop)
                    y = topLeft.Y;
                else
                    y = bottomRight.Y;
            }
        }



        static void ClampToEchosphereBounds(ref float x, ref float y, out float xOverflow, out float yOverflow)
        {
            Vector2 topLeft = EchosphereGeneratorSystem.echosphereTopLeft;
            Vector2 bottomRight = EchosphereGeneratorSystem.echosphereBottomRight;
            xOverflow = 0;
            yOverflow = 0;
            if(x < topLeft.X)
            {
                xOverflow = x - topLeft.X;
                x = topLeft.X;
            }
            if (x > bottomRight.X)
            {
                xOverflow = x - bottomRight.X;
                x = bottomRight.X;
            }
            if (y < topLeft.Y)
            {
                yOverflow = y - topLeft.Y;
                y = topLeft.Y;
            }
            if (y > bottomRight.Y)
            {
                yOverflow = y - bottomRight.Y;
                y = bottomRight.Y;
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
            EchosphereBorderEffect.Update();
        }
        public override void Load()
        {
            EchosphereBorderEffect.CallOnLoad();
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
