using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.Items.RuneSetEffects.SpecificWeapons
{
    public static class RuneSetCrystalVileShard
    {
        public static void CrystalAI(Projectile proj)
        {
            proj.rotation = (float)Math.Atan2(proj.velocity.Y, proj.velocity.X) + MathF.PI * .5f;
            float velLength = proj.velocity.Length();
            if (RuneSetHelper.IsHoming(proj.ai[2]))
            {
                if (proj.alpha > 200)
                {

                    RuneSetGlobalProj gProj = proj.GetGlobalProjectile<RuneSetGlobalProj>();
                    int target = RuneSetHelper.FindHomingTarget(proj, velLength, ref gProj.amethystHomingTarget, ref proj.localAI[2]);

                    if (target != -1)
                    {
                        NPC targetNPC = Main.npc[target];
                        proj.velocity = Vector2.Lerp(proj.velocity, proj.DirectionTo(targetNPC.velocity * 10 + targetNPC.Center) * velLength, .2f);
                        proj.velocity.Normalize();
                        proj.velocity *= velLength;
                    }
                }

            }
            if (RuneSetHelper.IsSwirlTwins(proj.ai[2], out int twinSign))
            {
                if (proj.localAI[2] == 0)
                {
                    float rotAmount = 0.3f;
                    proj.rotation += twinSign * MathF.Cos(proj.ai[1]) * rotAmount;

                    proj.velocity = (proj.rotation - MathF.PI / 2).ToRotationVector2() * velLength;
                }
                proj.localAI[2]++;
            }


            if (Main.netMode != NetmodeID.Server && proj.ai[1] == 0f && proj.localAI[0] == 0f)
            {
                proj.localAI[0] = 1f;

                SoundStyle legacySoundStyle = SoundID.Item101;

                SoundEngine.PlaySound(legacySoundStyle, proj.Center);
            }
            if (proj.ai[0] == 0f)
            {
                proj.alpha -= 100;
                if (proj.alpha > 0)
                {
                    return;
                }
                proj.alpha = 0;
                proj.ai[0] = 1f;
                if (proj.ai[1] == 0f)
                {
                    proj.ai[1] += 1f;
                    proj.position += proj.velocity * 1f;
                }
                if (Main.myPlayer == proj.owner && proj.type != ProjectileID.CrystalVileShardHead)
                {
                    int projIDToSpawn = proj.type;
                    int maxSegments = 8;
                    if (RuneSetHelper.BiggerHitbox(proj.ai[2]))
                    {
                        maxSegments *= 2;
                    }

                    if (proj.ai[1] >= maxSegments)
                    {
                        projIDToSpawn--;
                    }
                    int nextDamage = proj.damage;
                    float nextKB = proj.knockBack;
                    if (projIDToSpawn == ProjectileID.CrystalVileShardHead)
                    {
                        nextDamage = (int)(proj.damage * 1.25);
                        nextKB = proj.knockBack * 1.25f;
                    }
                    int number = Projectile.NewProjectile(proj.GetSource_FromThis(), proj.position.X + proj.velocity.X + proj.width / 2, proj.position.Y + proj.velocity.Y + proj.height / 2, proj.velocity.X, proj.velocity.Y, projIDToSpawn, nextDamage, nextKB, proj.owner, 0f, proj.ai[1] + 1f, proj.ai[2]);
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, number);
                }
                return;
            }
            if (proj.alpha < 170 && proj.alpha + 5 >= 170)
            {

                for (int num39 = 0; num39 < 8; num39++)
                {
                    int dustIndex = Dust.NewDust(proj.position, proj.width, proj.height, Main.rand.Next(68, 71), proj.velocity.X * 0.025f, proj.velocity.Y * 0.025f, 200, default(Color), 1.3f);
                    Main.dust[dustIndex].noGravity = true;
                    Dust dust = Main.dust[dustIndex];
                    dust.velocity *= 0.5f;
                }

            }


            proj.alpha += 4;


            if (proj.alpha >= 255)
            {
                proj.Kill();
            }
        }

        public static void Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float ai2 = 0;
            RuneSetHelper.SetUpAI2(player, item, ref ai2, out bool doubleFire, out float secondAi2, out float damageMult, out float secondProjVelMult, out int extraFlatDamage, out float globalvelocityMult);
            damage = (int)(damage * damageMult);
            damage += extraFlatDamage;
            float numberProjectiles = 3; // 3, 4, or 5 shots
            float rotation = MathHelper.ToRadians(Main.rand.Next(30, 40));
            position += Vector2.Normalize(velocity) * 45f;
            for (int i = 0; i < numberProjectiles; i++)
            {

                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
                Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI, 0, 0, ai2);
                if (doubleFire)
                {
                    Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, Main.myPlayer, 0, 0, secondAi2);
                }

            }
        }


    }
    public class CrystalVileShardExplosionFrag : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.CrystalStorm}";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = ProjectileID.Sets.TrailCacheLength[ProjectileID.CrystalStorm];
            ProjectileID.Sets.TrailingMode[Type] = ProjectileID.Sets.TrailingMode[ProjectileID.CrystalStorm];
        }
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 300;
        }
        public override void AI()
        {
            Projectile.light = Projectile.scale * 0.5f;
            Projectile.rotation += Projectile.velocity.X * 0.2f;
            Projectile.ai[1] += 1f;

            if (Main.rand.NextBool(4))
            {
                int num199 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.PurpleCrystalShard);
                Main.dust[num199].noGravity = true;
                Dust dust2 = Main.dust[num199];
                dust2.velocity *= 0.5f;
                dust2 = Main.dust[num199];
                dust2.scale *= 0.9f;
            }
            Projectile.velocity *= 0.985f;
            if (Projectile.ai[1] > 130f)
            {
                Projectile.scale -= 0.05f;
                if (Projectile.scale <= 0.2f)
                {
                    Projectile.scale = 0.2f;
                    Projectile.Kill();
                }
            }
            return;

        }
    }
}

