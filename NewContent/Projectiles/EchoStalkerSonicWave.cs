﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Projectiles
{
    internal class EchoStalkerSonicWave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.hostile = true;
            Projectile.friendly = false;
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

            scalingRate = Main.expertMode ? 0.85f /*: Main.masterMode ? 1.2f*/ : Main.getGoodWorld ? 1f : 0.6f;
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
}
