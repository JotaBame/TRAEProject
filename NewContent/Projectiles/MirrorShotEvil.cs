using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using TRAEProject.Common;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace TRAEProject.NewContent.Projectiles
{
    public class MirrorShotEvil : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 42;

            Projectile.friendly = true;
            Projectile.timeLeft = 180;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.scale = 0.8f;
            Projectile.light = 0.8f;
            Projectile.localNPCHitCooldown = 10;
            Projectile.GetGlobalProjectile<ProjectileStats>().explodes = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().explodes = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().ExplosionRadius = 80;
            Projectile.GetGlobalProjectile<ProjectileStats>().UsesDefaultExplosion = false;
            Projectile.ArmorPenetration = 200;
            Projectile.penetrate = 5;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, Projectile.damage * 3);
            target.AddBuff(BuffID.WitheredArmor, Projectile.damage * 3);
            target.AddBuff(BuffID.WitheredWeapon, Projectile.damage * 3);

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item28 with { MaxInstances = 0 }, Projectile.position);
            int DustCount = 30;
            int[] DustTypes = { DustID.Shadowflame, 21, 179 };
            for (int i = 0; i < DustCount; ++i)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, Main.rand.Next(DustTypes), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 15, default, 1.5f);
                dust.noGravity = true;
            }
            for (int i = 0; i < DustCount / 3; i++)
            {
                Color sparkleColor = Color.Lerp(Color.LightGreen, Color.Lavender, Main.rand.NextFloat());
                Vector2 sparkleScale = new Vector2(2, 1);
                Vector2 sparkleVel = TRAEMethods.PolarVector(Main.rand.NextFloat() * 8 + 2, Main.rand.NextFloat() * MathF.Tau);
                Sparkle.NewSparkle(Projectile.Center - sparkleVel, sparkleColor, sparkleScale, sparkleVel, Main.rand.Next(20, 40), sparkleScale, null, 1, sparkleVel.ToRotation(), .9f);
            }
            return;

        }
        public override void AI()
        {

            int num = 32;
            Player player = Main.player[Projectile.owner];
            int num2 = Main.maxTilesY * 16;
            int num3 = 0;
            if (Projectile.ai[0] >= 0f)
            {
                num3 = (int)(Projectile.ai[1] / (float)num2);
            }
            bool flag = Projectile.ai[0] == -1f || Projectile.ai[0] == -2f;

            if (Projectile.owner == Main.myPlayer)
            {
                if (Projectile.ai[0] >= 0f)
                {


                    Projectile.netUpdate = true;
                    Projectile.ai[0] = -1f;
                    Projectile.ai[1] = -1f;
                    int num4 = Projectile.FindTargetWithLineOfSight();
                    if (num4 != -1)
                    {
                        Projectile.ai[1] = num4;
                    }
                    else if (Projectile.velocity.Length() < 2f)
                    {
                        Projectile.velocity = Projectile.DirectionFrom(player.Center) * num;
                    }
                    else
                    {
                        Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * num;
                    }

                }
                if (flag && Projectile.ai[1] == -1f)
                {
                    int num5 = Projectile.FindTargetWithLineOfSight();
                    if (num5 != -1)
                    {
                        Projectile.ai[1] = num5;
                        Projectile.netUpdate = true;
                    }
                }
            }
            Vector2? vector = null;
            float amount = 1f;
            if (Projectile.ai[0] > 0f && Projectile.ai[1] > 0f)
            {
                vector = new Vector2(Projectile.ai[0], Projectile.ai[1] % (float)num2);
            }
            if (flag && Projectile.ai[1] >= 0f)
            {
                int num6 = (int)Projectile.ai[1];
                if (Main.npc.IndexInRange(num6))
                {
                    NPC nPC = Main.npc[num6];
                    if (nPC.CanBeChasedBy(this))
                    {
                        vector = nPC.Center;
                        float t = Projectile.Distance(vector.Value);
                        float num7 = Utils.GetLerpValue(0f, 100f, t, clamped: true) * Utils.GetLerpValue(600f, 400f, t, clamped: true);
                        amount = MathHelper.Lerp(0f, 0.2f, Utils.GetLerpValue(200f, 20f, 1f - num7, clamped: true));
                    }
                    else
                    {
                        Projectile.ai[1] = -1f;
                        Projectile.netUpdate = true;
                    }
                }
            }
            bool flag2 = false;
            if (flag)
            {
                flag2 = true;
            }
            if (vector.HasValue)
            {
                Vector2 value = vector.Value;
                if (Projectile.Distance(value) >= 64f)
                {
                    flag2 = true;
                    Vector2 v = value - Projectile.Center;
                    Vector2 vector2 = v.SafeNormalize(Vector2.Zero);
                    float num8 = Math.Min(num, v.Length());
                    Vector2 value2 = vector2 * num8;
                    if (Projectile.velocity.Length() < 4f)
                    {
                        Projectile.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(0.78539818525314331).SafeNormalize(Vector2.Zero) * 4f;
                    }
                    if (Projectile.velocity.HasNaNs())
                    {
                        Projectile.Kill();
                    }
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, value2, amount);
                }
                else
                {
                    Projectile.velocity *= 0.3f;
                    Projectile.velocity += (value - Projectile.Center) * 0.3f;
                    flag2 = Projectile.velocity.Length() >= 2f;
                }

            }
            if (flag && Projectile.ai[1] < 0f)
            {
                if (Projectile.velocity.Length() != (float)num)
                {
                    Projectile.velocity = Projectile.velocity.MoveTowards(Projectile.velocity.SafeNormalize(Vector2.UnitY) * num, 4f);
                }

            }
           if (flag2 && Projectile.velocity != Vector2.Zero)
            {
                Projectile.rotation = Projectile.rotation.AngleTowards(Projectile.velocity.ToRotation(), (float)Math.PI / 4f);
            }
            else
            {
                Projectile.rotation = Projectile.rotation.AngleLerp(0f, 0.2f);
            }
            bool flag3 = Projectile.velocity.Length() > 0.1f && Vector2.Dot(Projectile.oldVelocity.SafeNormalize(Vector2.Zero), Projectile.velocity.SafeNormalize(Vector2.Zero)) < 0.2f;

            if (Projectile.soundDelay == 0 && Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) > 2f)
            {
                Projectile.soundDelay = 10;
            }
            int DustId = Main.rand.NextFromList(DustID.GreenTorch, 179);
            int num9 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustId, 0f, 0f, 100, default(Color), 2f);
            Main.dust[num9].velocity *= 0.3f;
            Main.dust[num9].position.X = Projectile.position.X + (float)(Projectile.width / 2) + 4f + (float)Main.rand.Next(-4, 5);
            Main.dust[num9].position.Y = Projectile.position.Y + (float)(Projectile.height / 2) + (float)Main.rand.Next(-4, 5);
            Main.dust[num9].noGravity = true;
            Main.dust[num9].velocity += Main.rand.NextVector2Circular(2f, 2f);

            if (flag3)
            {
                int num10 = Main.rand.Next(2, 5);
                for (int i = 0; i < num10; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 21, 0f, 0f, 100, default(Color), 1.5f);
                    dust.velocity *= 0.3f;
                    dust.position = Projectile.Center;
                    dust.noGravity = true;
                    dust.velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);
                    dust.fadeIn = 2.2f;
                }
            }

            float lerpValue = Utils.GetLerpValue(0f, 10f, Projectile.localAI[0], clamped: true);
            Color newColor = Color.Lerp(Color.Transparent, Color.Crimson, lerpValue);

        }
 
    }
}
    


