using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Summoner.Whip;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Common;
using TRAEProject.NewContent.TRAEDebuffs;

namespace TRAEProject.Changes.Weapon.Melee
{
    public class MeleeProjectile : GlobalProjectile
    {        
	public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile projectile)
        {
            // Yoyo Defaults
            // Default: -1f lifetime, 200f Range, 10f Top Speed
            // Wooden Yoyo: 3f lifetime, 130f Range, 9f Top Speed
            // Rally: 5f lifetime, 170f Range, 11f Top Speed
            // Malaise: 7f lifetime, 195f Range, 12.5f Top Speed
            // Artery: 6f lifetime, 207f Range, 12f Top Speed
            // Amazon: 8f lifetime, 215f Range, 13f Top Speed
            // Code1: 9f lifetime, 220f Range, 13f Top Speed
            // Valor: 11f lifetime, 225f Range, 14f Top Speed
            // Cascade: 13f lifetime, 235f Range, 14f Top Speed
            // Format C: 8f lifetime, 235f Range, 15f Top Speed
            // Gradient: 10f lifetime, 250f Range, 12f Top Speed
            // Chik: 16f lifetime, 275f range, 17f Top Speed
            // Amarok: 15f lifetime, 270f range, 14f Top Speed
            // Hel-fire: 12f lifetime, 275f range, 15f Top Speed
            // Code 2: -1f (infinite) lifetime, 280 range, 17f Top Speed
            // Yelets: 14f lifetime, 290f range, 16f Top Speed
            // Kraken: -1f lifetime, 340f range, 16f Top Speed
            // Red's Throw: -1f lifetime, 370f range, 16f Top Speed
            // Valkyrie Yoyo: -1f lifetime, 370f range, 16f Top Speed
            // Eye Of Cthulhu: -1f lifetime, 360f range, 16.5f Top Speed
            // Terrarian: -4f lifetime, 400f range, 17.5f top speed
            // 1f = 1 second
            // 16f = 1 tile
            ProjectileID.Sets.YoyosMaximumRange[ProjectileID.Kraken] = 175f; // 
            ProjectileID.Sets.YoyosLifeTimeMultiplier[ProjectileID.Kraken] = 6f;
            ProjectileID.Sets.YoyosTopSpeed[ProjectileID.Kraken] = 6f;
            ProjectileID.Sets.YoyosLifeTimeMultiplier[ProjectileID.Code1] = -1f;
            ProjectileID.Sets.YoyosLifeTimeMultiplier[ProjectileID.Chik] = 3f;
            ProjectileID.Sets.YoyosMaximumRange[ProjectileID.Chik] = 220f;
            ProjectileID.Sets.YoyosMaximumRange[ProjectileID.HelFire] = 330f; // up from 275f
            //
            if (projectile.aiStyle == 99)
            {
                projectile.usesIDStaticNPCImmunity = true;
                projectile.idStaticNPCHitCooldown = 10;
            }
            //
            switch (projectile.type)
            {
                case ProjectileID.CorruptYoyo:
                    projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.Poisoned;
                    projectile.GetGlobalProjectile<ProjectileStats>().AddsBuffDuration = 120;
                    break;
                case ProjectileID.BlackCounterweight:
                case ProjectileID.YellowCounterweight:
                case ProjectileID.RedCounterweight:
                case ProjectileID.BlueCounterweight:
                case ProjectileID.PurpleCounterweight:
                case ProjectileID.GreenCounterweight:
                    projectile.extraUpdates = 2; // up from 0
                    projectile.usesIDStaticNPCImmunity = true;
                    projectile.idStaticNPCHitCooldown = 10;
                    break;
                case ProjectileID.FormatC:
                    projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.4f;
                    break;
                case ProjectileID.ButchersChainsaw:
                    projectile.penetrate = -1;
                    projectile.usesIDStaticNPCImmunity = true;
                    projectile.idStaticNPCHitCooldown = 10;
                    break;
                case ProjectileID.Spark:
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    break;
                case ProjectileID.FlamingJack:
                    projectile.extraUpdates = 1;
                    projectile.scale = 1.25f;
                    break;
                case ProjectileID.SporeCloud:
                    projectile.penetrate = 4;
                    projectile.DamageType = DamageClass.Melee;
                    projectile.usesIDStaticNPCImmunity = true;
                    projectile.idStaticNPCHitCooldown = 10;
                    break;
                case ProjectileID.ChlorophyteOrb: // Revisit
                    projectile.penetrate = 6;
                    break;
                case ProjectileID.PaladinsHammerFriendly:
                    projectile.tileCollide = false;
                    break;

                case ProjectileID.TerraBeam:
                    projectile.extraUpdates = 1;
                    break;
                case ProjectileID.IceSickle:
                    projectile.timeLeft = 180;
                    projectile.penetrate = 6;
                    break;
                case ProjectileID.FrostBoltSword:
                    projectile.penetrate = 3;
                    break;
            }
        }
        int timer = 0;
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.counterweight)
            {
                projectile.damage = player.HeldItem.damage;
                return;
            }
            switch (projectile.type)
            {
                case ProjectileID.Shroomerang:
                    ++timer;
					if (timer > 20)
                    {
                        timer -= 20;
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, projectile.velocity, 131, projectile.damage / 2, 0f, projectile.owner);
                    }
                    return;
                case ProjectileID.Gradient:
                    {
                        float PosX = projectile.position.X;
                        float PosY = projectile.position.Y;
                        float Float1 = 600f;
                        bool flag4 = false;
                        if (projectile.owner == Main.myPlayer)
                        {
                            projectile.localAI[1] += 1f + (1 * player.GetAttackSpeed(DamageClass.Melee));
                            if (projectile.localAI[1] > 90f)
                            {
                                projectile.localAI[1] = 90f;
                                for (int o = 0; o < 200; ++o)
                                {
                                    if (Main.npc[o].CanBeChasedBy(this, false))
                                    {
                                        float posX = Main.npc[o].position.X + (Main.npc[o].width / 2);
                                        float posY = Main.npc[o].position.Y + (Main.npc[o].height / 2);
                                        float num228 = Math.Abs(projectile.position.X + (projectile.width / 2) - posX) + Math.Abs(projectile.position.Y + (projectile.height / 2) - posY);
                                        if (num228 < Float1 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[o].position, Main.npc[o].width, Main.npc[o].height))
                                        {
                                            Float1 = num228;
                                            PosX = posX;
                                            PosY = posY;
                                            flag4 = true;
                                        }
                                    }
                                }
                            }

                        }
                        if (flag4)
                        {
                            projectile.localAI[1] = 0f;
                            float num229 = 14f;
                            Vector2 vector19 = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
                            float velX = PosX - vector19.X;
                            float velY = PosY - vector19.Y;
                            float sqrRoot = (float)Math.Sqrt(velX * velX + velY * velY);
                            sqrRoot = num229 / sqrRoot;
                            velX *= sqrRoot;
                            velY *= sqrRoot;
                            int bone = Projectile.NewProjectile(projectile.GetSource_FromThis(), vector19.X, vector19.Y, velX, velY, ProjectileID.BoneGloveProj, projectile.damage * 2, projectile.knockBack, Main.myPlayer, 0f, 0f);
                            Main.projectile[bone].DamageType = DamageClass.Melee;
                        }
                        return;
                    }
                case ProjectileID.Kraken:
                    {
                        float PosX = projectile.position.X;
                        float PosY = projectile.position.Y;
                        float Range = 500f;
                        bool flag4 = false;
                        if (projectile.owner == Main.myPlayer)
                        {
                            projectile.localAI[1] += 1f;
                            if (projectile.localAI[1] >= 15f)
                            {
                                projectile.localAI[1] = 15f;
                                for (int o = 0; o < 200; ++o)
                                {
                                    if (Main.npc[o].CanBeChasedBy(this, false))
                                    {
                                        float posX = Main.npc[o].position.X + (Main.npc[o].width / 2);
                                        float posY = Main.npc[o].position.Y + (Main.npc[o].height / 2);
                                        float Distance = Math.Abs(projectile.position.X + (projectile.width / 2) - posX) + Math.Abs(projectile.position.Y + (projectile.height / 2) - posY);
                                        if (Distance < Range && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[o].position, Main.npc[o].width, Main.npc[o].height))
                                        {
                                            Range = Distance;
                                            PosX = posX;
                                            PosY = posY;
                                            flag4 = true;
                                        }
                                    }
                                }
                            }

                        }
                        if (flag4)
                        {
                            projectile.localAI[1] = 0f;
                            float num229 = 14f;
                            Vector2 vector19 = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
                            float velX = PosX - vector19.X;
                            float velY = PosY - vector19.Y;
                            float sqrRoot = (float)Math.Sqrt(velX * velX + velY * velY);
                            sqrRoot = num229 / sqrRoot;
                            velX *= sqrRoot;
                            velY *= sqrRoot;
                            Projectile.NewProjectile(projectile.GetSource_FromThis(), vector19.X, vector19.Y, velX, velY, ProjectileType<PhantomTentacle>(), projectile.damage, projectile.knockBack, Main.myPlayer, 0f, 0f);
                        }
                        return;
                    }
                case ProjectileID.FormatC:
                    {
                        bool flag4 = false;
                        int mult = 1;
                        projectile.scale = 1f + (float)projectile.damage / 1000;
                        projectile.localAI[1] += 1f;
                        if (projectile.localAI[1] >= 36f && projectile.damage <= 300)
                        {
                            mult += 1;
                            projectile.damage += projectile.damage / mult;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, (int)projectile.position.X, (int)projectile.position.Y);
                            for (int i = 0; i < 25; i++)
                            {
                                // Create a new dust
                                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 64, 10f, 10f, 0, default, 2f);
                                dust.velocity *= Main.rand.NextFloat(-1.5f, 1.5f);
                                dust.noGravity = true;
                            }
                            projectile.localAI[1] = 45f;
                            flag4 = true;
                        }
                        if (flag4)
                        {
                            projectile.localAI[1] = 0f;
                        }
                        return;
                    }
            }                    
        }
        private static int tillinsta = 0;
        private static int shootDelay = 0;


        public int HitCount = 0;
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.type == ProjectileID.TinyEater)
            {
                TRAEDebuff.Apply<Corrupted>(target, 181, 1);
            }
            if (projectile.type == ProjectileID.ChainGuillotine && crit)
            {
                player.HealEffect(1, true);
                player.statLife += 1;
            }
            if (player.HasBuff(BuffID.WeaponImbueNanites) && (projectile.DamageType == DamageClass.Melee || projectile.aiStyle == 165 || projectile.type == ProjectileType<WhipProjectile>()))
            {
                player.AddBuff(BuffType<NanoHealing>(), 60, false);
            }
            switch (projectile.type)
            {
                // melee
                case ProjectileID.Chik:
                    {
                        int shards = damage / 10;
                        for (int i = 0; i < shards; i++)
                        {
                            float velX = (0f - projectile.velocity.X) * Main.rand.Next(40, 70) * 0.01f + Main.rand.Next(-20, 21) * 0.5f;
                            float velY = (0f - projectile.velocity.Y) * Main.rand.Next(40, 70) * 0.01f + Main.rand.Next(-20, 21) * 0.5f;
                            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + velX, projectile.position.Y + velY, velX, velY, ProjectileID.CrystalShard, 1, 0, projectile.owner, 0f, 0f);
                        }
                        break;
                    }
                case ProjectileID.Cascade:
                    {
                        ++HitCount;
                        if (HitCount >= 5)
                            projectile.Kill();
                        break;
                    }
                case ProjectileID.CrimsonYoyo:
                    {
                        player.lifeRegenCount += 30;
                        break;
                    }
                case ProjectileID.Sunfury:

                    TRAEDebuff.Apply<HeavyBurn>(target, 120, 1);
                    break;
            }
        }
        public override bool PreKill(Projectile projectile, int timeLeft)
        { 
            switch (projectile.type)
            {
                case ProjectileID.Cascade:
                    {
                        projectile.position.X += projectile.width / 2;
                        projectile.position.Y += projectile.height / 2;
                        projectile.width = projectile.height = 125;
                        projectile.position.X -= projectile.width / 2;
                        projectile.position.Y -= projectile.height / 2;
                        projectile.position.X += projectile.width / 2;
                        projectile.position.Y += projectile.height / 2;
                        projectile.width = projectile.height = 125;
                        projectile.position.X -= (projectile.width / 2);
                        projectile.position.Y -= (projectile.height / 2);
                        for (int k = 0; k < 200; k++)
                        {
                            NPC nPC = Main.npc[k];
                            if (nPC.active && !nPC.friendly && nPC.damage > 0 && !nPC.dontTakeDamage && Vector2.Distance(projectile.Center, nPC.Center) <= 125)
                            {
                                Main.player[projectile.owner].ApplyDamageToNPC(nPC, (int)(projectile.damage * 2), 0f, 0, crit: false);
                                if (nPC.FindBuffIndex(BuffID.OnFire) == -1)
                                {
                                    nPC.AddBuff(BuffID.OnFire, 120);
                                }
                            }
                        }
                        TRAEMethods.DefaultExplosion(projectile);
                        return false;
                    }
                case ProjectileID.ToxicFlask:
                    {
                        for (int num332 = 0; num332 < 1000; num332++)
                        {
                            if (Main.projectile[num332].active && Main.projectile[num332].owner == projectile.owner && Main.projectile[num332].type == ProjectileType<ToxicCloud>())
                            {
                                Main.projectile[num332].ai[1] = 600f;
                            }
                        }
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item107, projectile.position);
                        Gore.NewGore(projectile.GetSource_FromThis(), projectile.Center, -projectile.oldVelocity * 0.2f, 704);
                        Gore.NewGore(projectile.GetSource_FromThis(), projectile.Center, -projectile.oldVelocity * 0.2f, 705);
                        if (projectile.owner == Main.myPlayer)
                        {
                            int ToxicCloudsSpawned = Main.rand.Next(34, 37);
                            for (int num375 = 0; num375 < ToxicCloudsSpawned; num375++)
                            {
                                Vector2 vector22 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                                vector22.Normalize();
                                vector22 *= Main.rand.Next(10, 101) * 0.02f;
                                Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, vector22.X, vector22.Y, ProjectileType<ToxicCloud>(), projectile.damage, 1f, projectile.owner);
                            }
                        }
                    }
                    return false;               
            }
            return true;
        }     
    }
}