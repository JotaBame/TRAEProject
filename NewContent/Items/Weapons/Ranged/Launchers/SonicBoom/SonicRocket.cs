using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Weapon.Ranged.Rockets;
using TRAEProject.Common;

namespace TRAEProject.NewContent.Items.Weapons.Ranged.Launchers.SonicBoom
{
    public class SonicRocket : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
            Projectile.Size = new(30);
            Projectile.alpha = 255;
        }
        static Color GetCircleDotColor(float i, params Color[] color)
        {
            i *= color.Length;
            for (int j = 0; j < color.Length + 1; j++)
            {
                if (i < j)
                    return Color.Lerp(color[j % color.Length], color[(j + 1) % color.Length], i % 1);
            }
            return Color.Black;
        }
        float scalingRate;
        float opacityMult;
        public override void AI()
        {
            scalingRate = Projectile.ai[1];
            scalingRate *= Projectile.ai[0];
            Projectile.localAI[2]++;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.localAI[0] += scalingRate;
            int minSize = 8;
            if (Projectile.localAI[0] < minSize)
                Projectile.localAI[0] = minSize;
            Projectile.Opacity = Projectile.localAI[0];
            opacityMult = Utils.GetLerpValue(110, 80, Projectile.localAI[2], true) * Utils.GetLerpValue(0, 5, Projectile.localAI[2], true);
            if (opacityMult < float.Epsilon && Projectile.localAI[0] > 10)
                Projectile.Kill();
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 rectSize = new Vector2(4);
            float increment = 1f / (int)(Projectile.localAI[0] * 0.8f + 1);
            for (float i = 0; i < 1; i += increment)
            {
                float rotation = i * MathF.Tau;
                Vector2 posOffset = rotation.ToRotationVector2() * Projectile.localAI[0] * 2;
                Vector2 posOffset2 = rotation.ToRotationVector2() * (Projectile.localAI[0] * 2 - 6);
                posOffset.X *= 0.5f;
                posOffset2.X *= 0.5f;
                posOffset = posOffset.RotatedBy(Projectile.rotation);
                posOffset2 = posOffset2.RotatedBy(Projectile.rotation);
                Rectangle hitbox = Utils.CenteredRectangle(Projectile.Center + posOffset, rectSize);
                if (hitbox.Intersects(targetHitbox))
                    return true;
                hitbox = Utils.CenteredRectangle(Projectile.Center + posOffset2, rectSize);
                if (hitbox.Intersects(targetHitbox))
                {
                    return true;
                }
            }
            return false;
        }
        static bool CircleHitbox(float radius, Vector2 circleOrigin, Rectangle targetHitbox)
        {
            return circleOrigin.DistanceSQ(targetHitbox.ClosestPointInRect(circleOrigin)) <= radius * radius;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Asset<Texture2D> texture = TextureAssets.Projectile[Type];
            float increment = 1f / (int)(Projectile.localAI[0] * 6 + 2);

            for (float i = 0; i < 1; i += increment)
            {
                float rotation = i * MathF.Tau + MathF.PI / 2;
                Vector2 posOffset = rotation.ToRotationVector2() * Projectile.localAI[0] * 2;
                posOffset.X *= 0.5f;
                posOffset = posOffset.RotatedBy(Projectile.rotation);
                Color color = Color.Lerp(Color.Purple, Color.Black, .5f);
                color *= opacityMult;
                color *= Projectile.Opacity;
                Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition + posOffset, null, color * Projectile.Opacity, Main.rand.NextFloat(MathF.Tau), texture.Size() / 2, 1f / texture.Width() * 8 /* new Vector2(0.4f, 0.6f)*/, SpriteEffects.None);
            }

            for (float i = 0; i < 1; i += increment)
            {
                float rotation = i * MathF.Tau + MathF.PI / 2;
                Vector2 posOffset = rotation.ToRotationVector2() * Projectile.localAI[0] * 2;
                posOffset.X *= 0.5f;
                posOffset = posOffset.RotatedBy(Projectile.rotation);
                Color color = GetCircleDotColor(i, Color.Magenta * 1.2f, Color.Purple * 1.3f, Color.White);//Color.Lerp(Color.Magenta, Color.Purple, MathF.Sin(i * MathF.Tau + Main.GlobalTimeWrappedHourly * 3) * 0.5f + 0.5f);
                color *= opacityMult;
                color *= Projectile.Opacity;
                Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition + posOffset, null, color * Projectile.Opacity, Main.rand.NextFloat(MathF.Tau), texture.Size() / 2, 1f / texture.Width() * 5 /* new Vector2(0.4f, 0.6f)*/, SpriteEffects.None);
            }
            return false;// base.PreDraw(ref lightColor);
        }
    }
    //public class DestructiveSonic : ModProjectile
    //{
    //    public override void SetDefaults()
    //    {
    //        Projectile.height = 20;
    //        Projectile.width = 20;
    //        Projectile.timeLeft = 300;
    //        Projectile.GetGlobalProjectile<NewRockets>().DestructiveRocketStats(Projectile);
    //        Projectile.GetGlobalProjectile<SonicRockets>().IceRocket = true;
    //    }
    //    public override void AI()
    //    {
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicRocketAI(Projectile);
    //    }
    //    public override void OnKill(int timeLeft)
    //    {
    //        Projectile.GetGlobalProjectile<SonicRockets>().FrostExplosion(Projectile);
    //        Projectile.GetGlobalProjectile<NewRockets>().DestroyTiles(Projectile, 3);
    //    }
    //}
    //public class SuperSonic : ModProjectile
    //{
    //    public override void SetDefaults()
    //    {
    //        Projectile.height = 20;
    //        Projectile.width = 20;
    //        Projectile.timeLeft = 300;
    //        Projectile.GetGlobalProjectile<NewRockets>().SuperRocketStats(Projectile, false);
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicExplosion = true;
    //        Projectile.GetGlobalProjectile<SonicRockets>().IceRocket = true;
    //    }
    //    public override void AI()
    //    {
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicRocketAI(Projectile);
    //    }
    //}
    //public class DirectSonic : ModProjectile
    //{
    //    public override void SetDefaults()
    //    {
    //        Projectile.height = 20;
    //        Projectile.width = 20;
    //        Projectile.timeLeft = 300;
    //        Projectile.GetGlobalProjectile<NewRockets>().DirectRocketStats(Projectile, false);
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicExplosion = true;
    //        Projectile.GetGlobalProjectile<SonicRockets>().IceRocket = true;
    //    }
    //    public override void AI()
    //    {
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicRocketAI(Projectile);
    //    }
    //}
    //public class MiniNukeSonic : ModProjectile
    //{
    //    public override void SetDefaults()
    //    {
    //        Projectile.height = 20;
    //        Projectile.width = 20;
    //        Projectile.timeLeft = 300;
    //        Projectile.GetGlobalProjectile<NewRockets>().MiniNukeStats(Projectile, false);
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicExplosion = true;
    //        Projectile.GetGlobalProjectile<SonicRockets>().IceRocket = true;
    //    }
    //    public override void AI()
    //    {
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicRocketAI(Projectile);
    //    }
    //}
    //public class DestructiveMiniNukeSonic : ModProjectile
    //{
    //    public override void SetDefaults()
    //    {
    //        Projectile.height = 20;
    //        Projectile.width = 20;
    //        Projectile.timeLeft = 300;
    //        Projectile.GetGlobalProjectile<NewRockets>().MiniNukeStats(Projectile, false);
    //        Projectile.GetGlobalProjectile<SonicRockets>().IceRocket = true;
    //    }
    //    public override void AI()
    //    {
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicRocketAI(Projectile);
      
    //    }
    //    public override void OnKill(int timeLeft)
    //    {
    //        Projectile.GetGlobalProjectile<SonicRockets>().FrostExplosion(Projectile);
    //        Projectile.GetGlobalProjectile<NewRockets>().DestroyTiles(Projectile, 7);
    //    }
    //}
    //public class ClusterSonic : ModProjectile
    //{
    //    public override void SetDefaults()
    //    {
    //        Projectile.height = 20;
    //        Projectile.width = 20;
    //        Projectile.timeLeft = 300;
    //        Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile,false);
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicExplosion = true;
    //        Projectile.GetGlobalProjectile<SonicRockets>().IceRocket = true;
    //    }
    //    public override void AI()
    //    {
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicRocketAI(Projectile);
    //    }
    //    public override void OnKill(int timeLeft)
    //    {
    //        if (Projectile.owner == Main.myPlayer)
    //        {
    //            int Cluster = 862; // snowman cannon's projectile, doesn't damage the player
    //            float num852 = (MathF.PI * 2f);
    //            for (float c = 0f; c < 1f; c += 355f / (678f * MathF.PI))
    //            {
    //                float f2 = num852 + c * (MathF.PI * 2f);
    //                Vector2 velocity = f2.ToRotationVector2() * (4f + Main.rand.NextFloat() * 2f);
    //                velocity += Vector2.UnitY * -1f;
    //                int num854 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, Cluster, Projectile.damage / 4, 0f, Projectile.owner);
    //                Projectile pRojectile = Main.projectile[num854];
    //                Projectile projectile2 = pRojectile;
    //                projectile2.timeLeft = 30;
    //            }
    //        }
    //    }
    //}
    //public class HeavySonic: ModProjectile
    //{
    //    public override void SetDefaults()
    //    {
    //        Projectile.height = 20;
    //        Projectile.width = 20;
    //        Projectile.timeLeft = 300;
    //        Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicExplosion = true;
    //        Projectile.GetGlobalProjectile<SonicRockets>().IceRocket = true;
    //        Projectile.GetGlobalProjectile<NewRockets>().HeavyRocket = true;
 
    //    }
    //    public override void AI()
    //    {
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicRocketAI(Projectile);
    //    }
    //}
    //public class DrySonic : ModProjectile
    //{
    //    public override void SetDefaults()
    //    {
    //        Projectile.height = 20;
    //        Projectile.width = 20;
    //        Projectile.timeLeft = 300;
    //        Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicExplosion = true;
    //        Projectile.GetGlobalProjectile<SonicRockets>().IceRocket = true;
    //        Projectile.GetGlobalProjectile<NewRockets>().DryRocket = true;
    //    }
    //    public override void AI()
    //    {
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicRocketAI(Projectile);
    //    }
    //}
    //public class WetSonic : ModProjectile
    //{
    //    public override void SetDefaults()
    //    {
    //        Projectile.height = 20;
    //        Projectile.width = 20;
    //        Projectile.timeLeft = 300;
    //        Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicExplosion = true;
    //        Projectile.GetGlobalProjectile<SonicRockets>().IceRocket = true;
    //        Projectile.GetGlobalProjectile<NewRockets>().WetRocket = true;

    //    }
    //    public override void AI()
    //    {
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicRocketAI(Projectile);
    //    }
    //}
    //public class LavaSonic : ModProjectile
    //{
    //    public override void SetDefaults()
    //    {
    //        Projectile.height = 20;
    //        Projectile.width = 20;
    //        Projectile.timeLeft = 300;
    //        Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicExplosion = true;
    //        Projectile.GetGlobalProjectile<SonicRockets>().IceRocket = true;
    //        Projectile.GetGlobalProjectile<NewRockets>().LavaRocket = true;

    //    }
    //    public override void AI()
    //    {
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicRocketAI(Projectile);
    //    }
    //}
    //public class HoneySonic : ModProjectile
    //{
    //    public override void SetDefaults()
    //    {
    //        Projectile.height = 20;
    //        Projectile.width = 20;
    //        Projectile.timeLeft = 300;
    //        Projectile.GetGlobalProjectile<NewRockets>().RocketStats(Projectile, false);
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicExplosion = true;
    //        Projectile.GetGlobalProjectile<SonicRockets>().IceRocket = true;
    //        Projectile.GetGlobalProjectile<NewRockets>().HoneyRocket = true;
    //    }
    //    public override void AI()
    //    {
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicRocketAI(Projectile);
    //    }
    //}
    //public class LuminiteSonic : ModProjectile
    //{
    //    public override void SetDefaults()
    //    {
    //        Projectile.height = 20;
    //        Projectile.width = 20;
    //        Projectile.timeLeft = 300;
    //        Projectile.GetGlobalProjectile<SonicRockets>().IceRocket = true;
    //        Projectile.GetGlobalProjectile<NewRockets>().LuminiteStats(Projectile);
    //    }
    //    public override void AI()
    //    {
    //        Projectile.GetGlobalProjectile<SonicRockets>().SonicRocketAI(Projectile);
    //    }
    //}
}