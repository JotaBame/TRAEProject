using System;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.GameContent;

namespace TRAEProject.NewContent.Projectiles
{
    internal class EchoStalkerSonicWave : ModProjectile
    {
        public override string Texture => "TRAEProject/NewContent/NPCs/EchoStalker/EchoStalker";
        static Rectangle smallRing = new Rectangle(228, 6, 12, 19);
        static Rectangle mediumRing = new Rectangle(242, 4, 14, 24);
        static Rectangle bigRing = new Rectangle(258, 0, 18, 32);
        static Texture2D texture;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.Size = new(30);
            Projectile.alpha = 255;
        }
        Color GetCircleDotColor(float i, params Color[] color)
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

            scalingRate = Main.expertMode ? 1f : Main.masterMode ? 1.2f : Main.getGoodWorld ? 2 : 0.6f;
            scalingRate *= Projectile.ai[0];
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.localAI[0] += scalingRate;
            Projectile.Opacity = Projectile.localAI[0];
            if (opacityMult < float.Epsilon && Projectile.localAI[0] > 10)
                Projectile.Kill();
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {

            for (float i = 0; i < 1; i += 1f / (int)(Projectile.localAI[0] * 0.8f + 1))
            {
                float rotation = i * MathF.Tau;
                Vector2 posOffset = rotation.ToRotationVector2() * Projectile.localAI[0] * 2;
                Vector2 posOffset2 = rotation.ToRotationVector2() * (Projectile.localAI[0] * 2-6) ;
                posOffset.X *= 0.5f;
                posOffset2.X *= 0.5f;
                posOffset = posOffset.RotatedBy(Projectile.rotation);
                posOffset2 = posOffset2.RotatedBy(Projectile.rotation);
                if (CircleHitbox(5, Projectile.Center + posOffset, targetHitbox))
                    return true;
                if (CircleHitbox(5, Projectile.Center + posOffset2, targetHitbox))
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
            if(texture == null)
            texture = TextureAssets.Projectile[ProjectileID.CultistBossLightningOrbArc].Value;//small dot texture
            float lifetime = 100000 * scalingRate;
            float fadeOutTime = 20 * scalingRate;
            opacityMult = Utils.GetLerpValue(lifetime + fadeOutTime, lifetime, Projectile.localAI[0], true);
            for (float i = 0; i < 1; i+= 1f / (int)(Projectile.localAI[0] * 3 + 1))
            {
                float rotation = i * MathF.Tau + Main.GlobalTimeWrappedHourly * -5;
                Vector2 posOffset = rotation.ToRotationVector2() * Projectile.localAI[0] * 2;
                posOffset.X *= 0.5f;
                posOffset = posOffset.RotatedBy(Projectile.rotation);
                Color color = GetCircleDotColor(i, Color.Magenta * 1.2f, Color.Purple * 1.3f, Color.White);//Color.Lerp(Color.Magenta, Color.Purple, MathF.Sin(i * MathF.Tau + Main.GlobalTimeWrappedHourly * 3) * 0.5f + 0.5f);
                color *= 0.7f;
                color *= opacityMult;
                color *= Projectile.Opacity;
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + posOffset, null, color * Projectile.Opacity, Projectile.rotation + rotation, texture.Size() / 2,1f/texture.Width * 10 /* new Vector2(0.4f, 0.6f)*/, SpriteEffects.None);     
            }
            return false;// base.PreDraw(ref lightColor);
        }
    }
}
