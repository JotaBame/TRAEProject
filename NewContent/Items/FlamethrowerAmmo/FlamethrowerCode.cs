using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
using static Terraria.ModLoader.PlayerDrawLayer;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameContent;
using TRAEProject.Changes.Weapons.Ranged;

namespace TRAEProject.NewContent.Items.FlamethrowerAmmo
{
    public abstract class FlamethrowerProjectile : ModProjectile
    {
        // these are the defaults for all gels
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.alpha = 255;
            Projectile.extraUpdates = 2;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.penetrate = 3;

            Projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.15f;

            FlamethrowerDefaults();
        }
        public virtual void FlamethrowerDefaults()
        {
            
        }
        protected Color ColorMiddle = new Color(255, 80, 20, 200);
        protected Color ColorBack = new Color(255, 255, 20, 70);
        protected Color ColorLerp = Color.Lerp(new Color(80, 255, 20, 100), new Color(255, 255, 20, 70), 0.25f);
        protected Color ColorSmoke = new Color(80, 80, 80, 100);
        protected short dustID = DustID.Torch;
        protected float dustScale = 1;
        protected bool dieInWater = false;
        protected float scalemodifier = 1f;

        public override bool PreDraw(ref Color lightColor)
        {
            Color[] palette = new Color[] { ColorMiddle, ColorBack, ColorLerp, ColorSmoke};
            DrawFlamethrower(ColorMiddle, ColorBack, ColorLerp, ColorSmoke);
            return false;
        }
        public void DrawFlamethrower(Color color1, Color color2, Color color3, Color color4)
        {
            bool elfMelterProjectile = Projectile.type == ProjectileType<FrostFlameP>();
            Main.instance.LoadProjectile(ProjectileID.Flames);
            float num = 60f;
            float num2 = 12f;
            float fromMax = num + num2;
            Texture2D value = TextureAssets.Projectile[ProjectileID.Flames].Value;
            Color transparent = Color.Transparent;
            float num3 = 0.35f;
            float num4 = 0.7f;
            float num5 = 0.85f;
            float num6 = ((Projectile.localAI[0] > num - 10f) ? 0.175f : 0.2f);
            if (elfMelterProjectile) 
            {
                color1 = new Color(95, 120, 255, 200);
                color2 = new Color(50, 180, 255, 70);
                color3 = new Color(95, 160, 255, 100);
                color4 = new Color(33, 125, 202, 100);
            }
            int num7 = 3;
            int num8 = 2;
            int num9 = 7;
            int num10 = num9 * num8 * num7;
            float num11 = Utils.Remap(Projectile.localAI[0], num, fromMax, 1f, 0f);
            float num12 = Math.Min(Projectile.localAI[0], 20f);
            float num13 = Utils.Remap(Projectile.localAI[0], 0f, fromMax, 0f, 1f);
            float scale = Utils.Remap(num13, 0.2f, 0.5f, 0.25f, 1f) * scalemodifier;
            Rectangle rectangle = value.Frame(1, num9, 0, 3);
            if (!(num13 < 1f))
            {
                return;
            }
            for (int i = 0; i < 2; i++)
            {
                for (float num15 = 1f; num15 >= 0f; num15 -= num6)
                {
                    transparent = ((num13 < 0.1f) ? Color.Lerp(Color.Transparent, color1, Utils.GetLerpValue(0f, 0.1f, num13, clamped: true)) : ((num13 < 0.2f) ? Color.Lerp(color1, color2, Utils.GetLerpValue(0.1f, 0.2f, num13, clamped: true)) : ((num13 < num3) ? color2 : ((num13 < num4) ? Color.Lerp(color2, color3, Utils.GetLerpValue(num3, num4, num13, clamped: true)) : ((num13 < num5) ? Color.Lerp(color3, color4, Utils.GetLerpValue(num4, num5, num13, clamped: true)) : ((!(num13 < 1f)) ? Color.Transparent : Color.Lerp(color4, Color.Transparent, Utils.GetLerpValue(num5, 1f, num13, clamped: true))))))));
                    float num16 = (1f - num15) * Utils.Remap(num13, 0f, 0.2f, 0f, 1f);
                    Vector2 vector = Projectile.Center - Main.screenPosition + Projectile.velocity * (0f - num12) * num15;
                    Color color5 = transparent * num16;
                    Color color6 = color5;
                    if (!elfMelterProjectile) 
                    {
                        color6.G /= 2;
                        color6.B /= 2;
                        color6.A = (byte)Math.Min((float)(int)color5.A + 80f * num16, 255f);
                        float num17 = Utils.Remap(Projectile.localAI[0], 20f, fromMax, 0f, 1f);
                        num17 *= num17;
                    }
                    float num18 = 1f / num6 * (num15 + 1f);
                    float num19 = Projectile.rotation + num15 * ((float)Math.PI / 2f) + Main.GlobalTimeWrappedHourly * num18 * 2f;
                    float num20 = Projectile.rotation - num15 * ((float)Math.PI / 2f) - Main.GlobalTimeWrappedHourly * num18 * 2f;
                    switch (i)
                    {
                        case 0:
                            Main.EntitySpriteDraw(value, 
                                vector + Projectile.velocity * (0f - num12) * num6 * 0.5f, 
                                rectangle, 
                                color6 * num11 * 0.25f,
                                num19 + (float)Math.PI / 4f,
                                rectangle.Size() / 2f,
                                scale, 
                                SpriteEffects.None);
                            Main.EntitySpriteDraw(value, vector, rectangle, color6 * num11, num20, rectangle.Size() / 2f, scale, SpriteEffects.None);
                            break;
                        case 1:
                            if (!elfMelterProjectile)
                            {
                                Main.EntitySpriteDraw(value, vector + Projectile.velocity * (0f - num12) * num6 * 0.2f, rectangle, color5 * num11 * 0.25f, num19 + (float)Math.PI / 2f, rectangle.Size() / 2f, scale * 0.75f, SpriteEffects.None);
                            Main.EntitySpriteDraw(value, vector, rectangle, color5 * num11, num20 + (float)Math.PI / 2f, rectangle.Size() / 2f, scale * 0.75f, SpriteEffects.None);
                            }
                            break;
                    }
                }
            }
            return;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (!projHitbox.Intersects(targetHitbox))
            {
                return false;
            }
            return Collision.CanHit(Projectile.Center, 0, 0, targetHitbox.Center.ToVector2(), 0, 0);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            Projectile.velocity = oldVelocity * 0.95f;
            Projectile.position -= Projectile.velocity;
            return false;
        }
        public override void AI()
        {
            if (dieInWater && Projectile.wet)
                Projectile.Kill();
            Projectile.localAI[0] += 1f;
            int timeItFadesOut = 12;
            int timeBeforeItSlowsDown = 60;
            int maxTime = timeBeforeItSlowsDown + timeItFadesOut;
            if (Projectile.localAI[0] >= maxTime)
            {
                Projectile.Kill();
            }
            if (Projectile.localAI[0] >= (float)timeBeforeItSlowsDown)
            {
                Projectile.velocity *= 0.95f;
            }
            bool IsItElfMelter = Projectile.ai[0] == 1f;
            int dustTimer = 60;
            int timerExclusiveToElfMelter = dustTimer;
            if (IsItElfMelter)
            {
                dustTimer = 0;
                timerExclusiveToElfMelter = timeBeforeItSlowsDown;
            }
            if (Projectile.localAI[0] < (float)timerExclusiveToElfMelter && Main.rand.NextFloat() < 0.25f)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(60f, 60f) * Utils.Remap(Projectile.localAI[0], 0f, 72f, 0.5f, 1f), 4, 4, dustID, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, Scale: dustScale);
                if (Main.rand.NextBool(4))
                {
                    dust.noGravity = true;
                    dust.scale *= 3f;
                    dust.velocity.X *= 2f;
                    dust.velocity.Y *= 2f;
                }
                else
                {
                    dust.scale *= 1.5f;
                }
                dust.scale *= 1.5f;
                dust.velocity *= 1.2f;
                dust.velocity += Projectile.velocity * 1f * Utils.Remap(Projectile.localAI[0], 0f, (float)timeBeforeItSlowsDown * 0.75f, 1f, 0.1f) * Utils.Remap(Projectile.localAI[0], 0f, (float)timeBeforeItSlowsDown * 0.1f, 0.1f, 1f);
                dust.customData = 1;
            }
            if (dustTimer > 0 && Projectile.localAI[0] >= (float)dustTimer && Main.rand.NextFloat() < 0.5f)
            {
                Vector2 center = Main.player[Projectile.owner].Center;
                Vector2 vector = (Projectile.Center - center).SafeNormalize(Vector2.Zero).RotatedByRandom(0.19634954631328583) * 7f;
                short num7 = 31;
                Dust dust2 = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(50f, 50f) - vector * 2f, 4, 4, num7, 0f, 0f, 150, new Color(80, 80, 80));
                dust2.noGravity = true;
                dust2.velocity = vector;
                dust2.scale *= 1.1f + Main.rand.NextFloat() * 0.2f;
                dust2.customData = -0.3f - 0.15f * Main.rand.NextFloat();
            }
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int num = (int)(Utils.Remap(Projectile.localAI[0], 0f, 72f, 10f, 40f) * scalemodifier);
            hitbox.Inflate(num, num);
        }

    }


}