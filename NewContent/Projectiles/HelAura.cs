using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using Microsoft.CodeAnalysis;
using TRAEProject.Common;


namespace TRAEProject.NewContent.Projectiles
{
    public class HelAura : ModProjectile
    {

        // these are the defaults for all gels
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 2;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.Daybreak;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffDuration = 180;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return AABBvCircleCollision(targetHitbox, Projectile.Center, 105);
        }
        static bool AABBvCircleCollision(Rectangle AABB, Vector2 circleCenter, float circleRadius)
        {
            float closestX = circleCenter.X;
            if(closestX > AABB.X + AABB.Width)
            {
                closestX = AABB.X + AABB.Width;
            }
            if(closestX < AABB.X)
            {
                closestX = AABB.X;
            }
            float closestY = circleCenter.Y;
            if(closestY > AABB.Y + AABB.Height)
            {
                closestY = AABB.Y + AABB.Height;
            }
            if(closestY < AABB.Y)
            {
                closestY = AABB.Y;
            }
            return (circleCenter - new Vector2(closestX, closestY)).Length() < circleRadius;
        }
        float scaleFactor = 2f;
        public override void AI()
        {
            Projectile parent = Main.projectile[(int)Projectile.ai[0]];
            if(!parent.active || parent.type != ProjectileID.HelFire)
            {
                Projectile.Kill();
                return;
            }
            else
            {
                Projectile.Center = parent.Center;
            }
            Projectile.timeLeft = 2;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            
            float projScale = 1f;
            float num2 = 0.1f;
            float num3 = 0.9f;
            if (!Main.gamePaused && Main.instance.IsActive)
            {
                Projectile.scale += 0.004f;
            }
            if (Projectile.scale < 1f)
            {
                projScale = Projectile.scale;
            }
            else
            {
                Projectile.scale = 0.8f;
                projScale = Projectile.scale;
            }
            if (!Main.gamePaused && Main.instance.IsActive)
            {
                Projectile.rotation += 0.05f;
            }
            if (Projectile.rotation > (float)Math.PI * 2f)
            {
                Projectile.rotation -= (float)Math.PI * 2f;
            }
            if (Projectile.rotation < (float)Math.PI * -2f)
            {
                Projectile.rotation += (float)Math.PI * 2f;
            }
            for (int j = 0; j < 3; j++)
            {
                float num4 = projScale + num2 * (float)j;
                if (num4 > 1f)
                {
                    num4 -= num2 * 2f;
                }
                float num5 = MathHelper.Lerp(0.8f, 0f, Math.Abs(num4 - num3) * 10f);
                Main.EntitySpriteDraw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, 
                new Rectangle(0, 400 * j, 400, 400), new Color(num5, num5, num5, num5 / 2f), 
                Projectile.rotation + (float)Math.PI / 3f * (float)j, new Vector2(200f, 200f), 
                num4 * 0.625f, SpriteEffects.None);
            }
            return false;
        }
    }
}