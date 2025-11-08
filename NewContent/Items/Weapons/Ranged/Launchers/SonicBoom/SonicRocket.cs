using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using TRAEProject.NewContent.Items.Weapons.Ranged.Ammo;

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
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.tileCollide = true;
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
        float opacityMult;

        public override void AI()
        {

            Projectile.ai[1] = 0.65f; // default value, this is the wave size
            switch (Projectile.ai[2])
            {

                case ItemID.RocketIII:
                    Projectile.ai[1] = 1f;
                    break;
                case ItemID.RocketIV:
                    Projectile.ai[1] = 0.5f;
                    Projectile.extraUpdates = 2;
                    Projectile.GetGlobalProjectile<ProjectileStats>().FirstHitDamage = 1.4f;
                    break;
                case ItemID.MiniNukeI:
                    Projectile.ai[1] = 1.3f;
                    break;
                case ItemID.ClusterRocketI:
                    Projectile.ai[1] = 0.20f;
                    break;

                case ItemID.HoneyRocket:
                    Projectile.ai[1] = 0.65f;
                    break;
            }
            if (Projectile.ai[2] == ModContent.ItemType<LuminiteRocket>()) 
                Projectile.localNPCHitCooldown = 10;

            Projectile.ai[1] *= Projectile.ai[0];


            Projectile.localAI[2]++;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.localAI[0] += Projectile.ai[1];
            int minSize = 8;
            if (Projectile.localAI[0] < minSize)
                Projectile.localAI[0] = minSize;
            Projectile.Opacity = Projectile.localAI[0];
            opacityMult = Utils.GetLerpValue(110, 80, Projectile.localAI[2], true) * Utils.GetLerpValue(0, 5, Projectile.localAI[2], true);
            if (opacityMult < float.Epsilon && Projectile.localAI[0] > 10)
                Projectile.Kill();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localAI[2] += 10; // this is to cap the pierce

            switch (Projectile.ai[2])
            {
                case ItemID.WetRocket:
                    target.AddBuff(BuffID.Wet, 300);
                    break;
                case ItemID.LavaRocket:
                    target.AddBuff(BuffID.OnFire3, 300);
                    break;
                case ItemID.RocketIV:
                    Projectile.extraUpdates = 1;
                    Projectile.localAI[2] += 5; // even less pierce
                    break;
                case ItemID.ClusterRocketII:
                    target.GetGlobalNPC<Stun>().StunMe(target, 60);

                    break;
            }
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
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = oldVelocity;
            Projectile.localAI[2] += 6f;
            return false;
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
                Color color1 = Color.Magenta;
                Color color2 = Color.Purple;

                if (Projectile.ai[2] == ItemID.WetRocket)
                {
                    color1 = Color.Blue;
                    color2 = Color.LightBlue;
                }

                if (Projectile.ai[2] == ItemID.LavaRocket)
                {
                    color1 = Color.OrangeRed;
                    color2 = Color.Orange;
                }

                if (Projectile.ai[2] == ItemID.HoneyRocket)
                {
                    color1 = Color.Yellow;
                    color2 = Color.Goldenrod;

                }
                if (Projectile.ai[2] == ItemID.DryRocket)
                {
                    color1 = Color.White;
                    color2 = Color.Gray;
                }
                if (Projectile.ai[2] == ModContent.ItemType<LuminiteRocket>())
                {
                    color1 = Color.Red;

                    color2 = Color.Teal;
                }
                if (Projectile.ai[2] == ItemID.RocketIV) // direct
                {
                    color1 = Color.Yellow;
                    color2 = Color.DarkGoldenrod;
                }
                if (Projectile.ai[2] == ItemID.ClusterRocketII) // Heavy
                {
                    color1 = Color.Gray;
                    color2 = Color.DarkGray;
                }
                Color color = GetCircleDotColor(i, color1 * 1.2f, color2 * 1.3f, Color.White);//Color.Lerp(Color.Magenta, Color.Purple, MathF.Sin(i * MathF.Tau + Main.GlobalTimeWrappedHourly * 3) * 0.5f + 0.5f);
                color *= opacityMult;
                color *= Projectile.Opacity;
                Main.EntitySpriteDraw(texture.Value, Projectile.Center - Main.screenPosition + posOffset, null, color * Projectile.Opacity, Main.rand.NextFloat(MathF.Tau), texture.Size() / 2, 1f / texture.Width() * 5 /* new Vector2(0.4f, 0.6f)*/, SpriteEffects.None);
            }
            return false;// base.PreDraw(ref lightColor);
        }


    }

}