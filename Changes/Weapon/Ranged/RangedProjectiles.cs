using TRAEProject.NewContent.Buffs;
using TRAEProject.Common;
using TRAEProject.NewContent.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;

namespace TRAEProject.Changes.Projectiles
{
    public class RangedProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile projectile)
        {
            switch (projectile.type)
            {
                case ProjectileID.Hellwing:
                    projectile.penetrate = 1;
                    projectile.GetGlobalProjectile<ProjectileStats>().homesIn = true;
                    return;
				case ProjectileID.CandyCorn:
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    return;
                case ProjectileID.CursedBullet:
                    projectile.extraUpdates = 1;
                    return;
                case ProjectileID.CrystalShard:
                    projectile.GetGlobalProjectile<ProjectileStats>().DirectDamage = 0.5f;
                    projectile.GetGlobalProjectile<ProjectileStats>().IgnoresDefense = true;
                    return;
                case ProjectileID.Electrosphere:
                    projectile.usesIDStaticNPCImmunity = true;
                    projectile.idStaticNPCHitCooldown = 10;
                    return;
                case ProjectileID.CursedDartFlame:
                    projectile.timeLeft = 75;
                    projectile.penetrate = 1;
                    return;
                case ProjectileID.IchorDart:
                    projectile.GetGlobalProjectile<ProjectileStats>().DirectDamage = 0.8f;
                    return;
                case ProjectileID.CrystalDart:
                    projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.2f;
                    projectile.GetGlobalProjectile<ProjectileStats>().DamageLossOffATileBounce = 0.2f;
                    return;
                case ProjectileID.JestersArrow:
                    projectile.penetrate = 7;      
					projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 20;
                    return;
                case ProjectileID.CursedArrow:
                    projectile.extraUpdates = 1;
                    return;
                case ProjectileID.UnholyArrow:     
					projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 20;
                    return;
                case ProjectileID.VortexBeaterRocket:
                    projectile.penetrate = -1;
                    projectile.scale = 1.15f;
                    projectile.GetGlobalProjectile<ProjectileStats>().explodes = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().ExplosionRadius = 180;
                    projectile.GetGlobalProjectile<ProjectileStats>().DirectDamage = 1.8f;
                    return;
                case ProjectileID.HellfireArrow:
                    projectile.penetrate = 4;
                    projectile.extraUpdates = 1;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    return;
                case ProjectileID.IchorBullet:
                    projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().onlyBounceOnce = true;
                    return;
                case ProjectileID.GrenadeI:
                case ProjectileID.GrenadeII:
                case ProjectileID.GrenadeIII:
                case ProjectileID.GrenadeIV:
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    return;
                case ProjectileID.ToxicBubble:
                    projectile.timeLeft = 120;
                    return;
                case ProjectileID.BoneArrow:
                    projectile.penetrate = 3; 
                    projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().onlyBounceOnce = true;
                    return;
                case ProjectileID.HallowStar:
                    projectile.penetrate = -1;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    projectile.tileCollide = false;
                    projectile.GetGlobalProjectile<ProjectileStats>().explodes = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().ExplosionRadius = 80;
                    projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.25f;
                    return;
            }
        }
        public override bool TileCollideStyle(Projectile projectile, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            switch (projectile.type)
            {
                case ProjectileID.ProximityMineI:
                case ProjectileID.ProximityMineII:
                case ProjectileID.ProximityMineIII:
                case ProjectileID.ProximityMineIV:
                    fallThrough = false; // prevents these projectiles from falling through platforms
                    return true;
            }
            return true;
        }
        int ChloroBulletTime = 0;
        readonly int bulletsPerRocket = 4; //making this 7 will make it just like vanilla
         int timer = 0; 
        readonly int fireRate = 7; //making this 5 will make it just like vanilla

        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
			if (projectile.type == ProjectileID.ChlorophyteArrow)
			{
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
                ++ChloroBulletTime;
                if (ChloroBulletTime <= 24)
                {
                    Vector2 move = Vector2.Zero;
                    bool target = false;
                    float distance = 300f;
                    for (int k = 0; k < 200; k++)
                    {
                        if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && !Main.npc[k].immortal && projectile.localNPCImmunity[k] != 1)
                        {
                            Vector2 newMove = Main.npc[k].Center - projectile.Center;
                            float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                            if (distanceTo < distance)
                            {
                                move = newMove;
                                target = true;
                                distance = distanceTo;
                            }
                            if (target)
                            {
                                float scaleFactor2 = projectile.velocity.Length();
                                move.Normalize();
                                move *= scaleFactor2;
                                projectile.velocity = (projectile.velocity * 24f + move) / 25f;
                                projectile.velocity.Normalize();
                                projectile.velocity *= scaleFactor2;
                            }
                        }

                    }
                    return false;
                }
                return true;
            }

            if (projectile.type == ProjectileID.ChlorophyteBullet)
            {
                ++ChloroBulletTime; // once this reaches 24, the bullet loses its homing
                if (ChloroBulletTime >= 24) // Due to the bullet having 2 extraUpdates, this happens after 8 frames.
                {
                    // the code for aiStyle 1
					while (projectile.velocity.X >= 16f || projectile.velocity.X <= -16f || projectile.velocity.Y >= 16f || projectile.velocity.Y < -16f)
                    {
                        projectile.velocity.X *= 0.97f;
                        projectile.velocity.Y *= 0.97f;
                    }
					//
					// Dust Trail
                    for (int i = 0; i < 5; i++) // vanilla value is 10, once the homing disappears the trail loses intensity
                    {
                        float x2 = projectile.position.X - projectile.velocity.X / 10f * i;
                        float y2 = projectile.position.Y - projectile.velocity.Y / 10f * i;
                        int Dust = Terraria.Dust.NewDust(new Vector2(x2, y2), 1, 1, 75);
                        Main.dust[Dust].alpha = projectile.alpha;
                        Main.dust[Dust].position.X = x2;
                        Main.dust[Dust].position.Y = y2;
                        Main.dust[Dust].velocity *= 0f;
                        Main.dust[Dust].noGravity = true;
                    }
					//
                    return false;
                }
                return true;
            }
            if (projectile.type == ProjectileID.Phantasm)
            {
                Vector2 vector = player.RotatedRelativePoint(player.MountedCenter);
                float num = 0f;
                if (projectile.spriteDirection == -1)
                {
                    num = (float)Math.PI;
                }
                projectile.ai[0] += 1f;
                int speedMultiplier = 0;
                if (projectile.ai[0] >= 40f)
                {
                    speedMultiplier++;
                }
                if (projectile.ai[0] >= 80f)
                {
                    speedMultiplier++;
                }
                if (projectile.ai[0] >= 120f)
                {
                    speedMultiplier++;
                }
                int num68 = 24;
                int num69 = 2;
                projectile.ai[1] -= 1f;
                bool flag13 = false;
                if (projectile.ai[1] <= 0f)
                {
                    projectile.ai[1] = num68 - num69 * speedMultiplier;
                    flag13 = true;
                    _ = (int)projectile.ai[0] / (num68 - num69 * speedMultiplier);
                }
                bool canShoot3 = player.channel && player.HasAmmo(player.inventory[player.selectedItem]) && !player.noItems && !player.CCed;
                if (projectile.localAI[0] > 0f)
                {
                    projectile.localAI[0] -= 1f;
                }
                if (projectile.soundDelay <= 0 && canShoot3)
                {
                    projectile.soundDelay = num68 - num69 * speedMultiplier;
                    if (projectile.ai[0] != 1f)
                    {
                        SoundEngine.PlaySound(SoundID.Item5, projectile.position);
                    }
                    projectile.localAI[0] = 12f;
                }
                player.phantasmTime = 2;
                if (flag13 && Main.myPlayer == projectile.owner)
                {
                    int projToShoot3 = 14; 
                    float speed3 = 14f;
                    int Damage3 = player.GetWeaponDamage(player.inventory[player.selectedItem]);
                    float KnockBack3 = player.inventory[player.selectedItem].knockBack;
                    if (canShoot3)
                    {
                        bool HasAmmo = player.PickAmmo(player.inventory[player.selectedItem], out projToShoot3, out speed3, out Damage3, out KnockBack3, out var usedAmmoItemId);
                        IEntitySource projectileSource_Item_WithPotentialAmmo3 = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, usedAmmoItemId);
                        KnockBack3 = player.GetWeaponKnockback(player.inventory[player.selectedItem], KnockBack3);
                        float num70 = player.inventory[player.selectedItem].shootSpeed * projectile.scale;
                        Vector2 vector30 = vector;
                        Vector2 value11 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - vector30;
                        if (player.gravDir == -1f)
                        {
                            value11.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector30.Y;
                        }
                        Vector2 vector31 = Vector2.Normalize(value11);
                        if (float.IsNaN(vector31.X) || float.IsNaN(vector31.Y))
                        {
                            vector31 = -Vector2.UnitY;
                        }
                        vector31 *= num70;
                        if (vector31.X != projectile.velocity.X || vector31.Y != projectile.velocity.Y)
                        {
                            projectile.netUpdate = true;
                        }
                        projectile.velocity = vector31 * 0.55f;
                        for (int num71 = 0; num71 < 3; num71++)
                        {
                            Vector2 vector32 = Vector2.Normalize(projectile.velocity) * speed3 * (0.6f + Main.rand.NextFloat() * 0.8f);
                            if (float.IsNaN(vector32.X) || float.IsNaN(vector32.Y))
                            {
                                vector32 = -Vector2.UnitY;
                            }
                            Vector2 vector33 = vector30 + Utils.RandomVector2(Main.rand, -15f, 15f);
                            int num72 = Projectile.NewProjectile(projectileSource_Item_WithPotentialAmmo3, vector33.X, vector33.Y, vector32.X, vector32.Y, projToShoot3, Damage3, KnockBack3, projectile.owner);
                            Main.projectile[num72].noDropItem = true;
                        }
                    }
                    else
                    {
                        projectile.Kill();
                    }
                }
                projectile.position = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false) - projectile.Size / 2f;
                projectile.rotation = projectile.velocity.ToRotation() + num;
                projectile.spriteDirection = projectile.direction;
                projectile.timeLeft = 2;
                player.ChangeDir(projectile.direction);
                player.heldProj = projectile.whoAmI;
                int num2 = 2;
                float num3 = 0f;
                player.SetDummyItemTime(num2);
                player.itemRotation = MathHelper.WrapAngle((float)Math.Atan2(projectile.velocity.Y * (float)projectile.direction, projectile.velocity.X * (float)projectile.direction) + num3);
                return false;
            }
            if (projectile.type == 615)
            {
                timer++;
                //determine fire rate
                if (timer % fireRate == 0)
                {
                    projectile.ai[1] = -1; //forces VBeater to fire
                }
                else
                {
                    projectile.ai[1] = 3; //forces VBeater to not fire
                }
                projectile.ai[0] = 5; //forces VBeater not to fire a rocket
                if ((timer / fireRate) % bulletsPerRocket == 0)
                {
                    projectile.ai[0] = -1; //forces VBeater to fire a rocket
                }

                //this code preserves the buildup animation
                if (timer >= 40f)
                {
                    projectile.ai[0] += 35 * 2;
                }
                if (timer >= 80f)
                {
                    projectile.ai[0] += 35;
                }
                if (timer >= 120f)
                {
                    projectile.ai[0] += 35;
                }
            }
            return true;
        }
        
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            switch (projectile.type)
            {
                case ProjectileID.ToxicBubble:
                    {
                        if (projectile.scale < 1.5f)
                        {
                            projectile.scale *= 1.004f;
                        }
                    }
                    return;
    
            }                    
        }
       
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            switch (projectile.type)
            {
                case ProjectileID.ProximityMineI:
                case ProjectileID.ProximityMineII:
                case ProjectileID.ProximityMineIII:
                case ProjectileID.ProximityMineIV:
                    projectile.velocity.X *= 0f;
                    projectile.velocity.Y *= 0f;
                    return false;      
                case ProjectileID.CursedBullet:
                    {
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X, projectile.position.Y, 0f, 0f, ProjectileID.CursedDartFlame, (projectile.damage * 1), 0, projectile.owner, 0f, 0f);
                        return true;
                    }
                case ProjectileID.NanoBullet:
                case ProjectileID.ChlorophyteArrow:
                    {
                        projectile.Kill();
						return false;
                    }
            }
            return true;
        }

        private static int tillinsta = 0;
        private static int shootDelay = 0;
           
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
           
            Player player = Main.player[projectile.owner];
            switch (projectile.type)
            {
                case ProjectileID.NanoBullet:
                    {
                        player.AddBuff(BuffType<NanoHealing>(), 60, false);
                        return;
                    }
                      
                // melee
            }               
        }
        public override bool PreKill(Projectile projectile, int timeLeft)
        { 
            switch (projectile.type)
            {
                case ProjectileID.BeeArrow:
                    {
                        int beeCount = Main.rand.Next(2, 3);
                  
                        for (int i = 0; i < beeCount; i++)
                        {
                            Vector2 vector56 = projectile.oldVelocity;
                            vector56.Normalize();
                            vector56 *= 8f;
                            float X = (float)Main.rand.Next(-35, 36) * 0.01f;
                            float Y = (float)Main.rand.Next(-35, 36) * 0.01f;
                            X += projectile.oldVelocity.X / 6f;
                            Y += projectile.oldVelocity.Y / 6f;
                            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, X, Y, ProjectileID.Bee, projectile.damage / 4, projectile.knockBack, projectile.owner);
                        }
                    }
                    return false;
                case ProjectileID.Beenade:
                    {
                        int beeCount = Main.rand.Next(6, 10);

                        for (int i = 0; i < beeCount; i++)
                        {
                            Vector2 vector56 = projectile.oldVelocity;
                            vector56.Normalize();
                            vector56 *= 8f;
                            float X = (float)Main.rand.Next(-35, 36) * 0.01f;
                            float Y = (float)Main.rand.Next(-35, 36) * 0.01f;
                            X += projectile.oldVelocity.X / 6f;
                            Y += projectile.oldVelocity.Y / 6f;
                            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, X, Y, ProjectileID.Bee, projectile.damage, projectile.knockBack, projectile.owner);
                        }
                    }
                    return false;
                case ProjectileID.ClusterSnowmanFragmentsI:
                    {
                        SoundEngine.PlaySound(SoundID.Item62, projectile.position);
                        Color transparent6 = Color.Transparent;
                        for (int num840 = 0; num840 < 15; num840++)
                        {
                            Dust dust54 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 100, transparent6, 0.8f);
                            dust54.fadeIn = 0f;
                            Dust dust = dust54;
                            dust.velocity *= 0.5f;
                        }
                        for (int num841 = 0; num841 < 5; num841++)
                        {
                            Dust dust55 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 228, 0f, 0f, 100, transparent6, 2.5f);
                            dust55.noGravity = true;
                            Dust dust = dust55;
                            dust.velocity *= 2.5f;
                            dust55 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 228, 0f, 0f, 100, transparent6, 1.1f);
                            dust = dust55;
                            dust.velocity *= 2f;
                            dust55.noGravity = true;
                        }
                        for (int num842 = 0; num842 < 3; num842++)
                        {
                            Dust dust56 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 226, 0f, 0f, 100, transparent6, 1.1f);
                            Dust dust = dust56;
                            dust.velocity *= 2f;
                            dust56.noGravity = true;
                        }
                        for (int num843 = -1; num843 <= 1; num843 += 2)
                        {
                            for (int num844 = -1; num844 <= 1; num844 += 2)
                            {
                                if (Main.rand.Next(5) == 0)
                                {
                                    Gore gore11 = Gore.NewGoreDirect(projectile.GetSource_FromThis(), projectile.position, Vector2.Zero, Main.rand.Next(61, 64));
                                    Gore gore = gore11;
                                    gore.velocity *= 0.2f;
                                    gore = gore11;
                                    gore.scale *= 0.65f;
                                    gore = gore11;
                                    gore.velocity += new Vector2(num843, num844) * 0.5f;
                                }
                            }
                        }
                        return false;
                    }
                case ProjectileID.VortexBeaterRocket:
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, projectile.position);
                        for (int i = 0; i < 4; i++)
                        {
                            Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default, 1.5f);
                        }
                        for (int i = 0; i < 40; i++)
                        {
                            int Dust = Terraria.Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 229, 0f, 0f, 200, default, 2.5f);
                            Main.dust[Dust].noGravity = true;
                            Dust dust = Main.dust[Dust];
                            dust.velocity *= 2f;
                            Dust = Terraria.Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 229, 0f, 0f, 200, default, 1.5f);
                            dust = Main.dust[Dust];
                            dust.velocity *= 1.2f;
                            Main.dust[Dust].noGravity = true;
                        }
                        for (int i = 0; i < 1; i++)
                        {
                            int num371 = Gore.NewGore(projectile.GetSource_FromThis(), projectile.position + new Vector2(projectile.width * Main.rand.Next(100) / 100f, projectile.height * Main.rand.Next(100) / 100f) - Vector2.One * 10f, default, Main.rand.Next(61, 64));
                            Gore gore = Main.gore[num371];
                            gore.velocity *= 0.3f;
                            Main.gore[num371].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
                            Main.gore[num371].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
                        }
                        return false;
                    }        
               
                case ProjectileID.HolyArrow:
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, projectile.position);
                        for (int i = 0; i < 10; ++i)
                        {
                            Dust.NewDust(projectile.position, projectile.width, projectile.height, 58, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 150, default, 1.2f);
                        }
                        for (int i = 0; i < 3; ++i)
                        {
                            Gore.NewGore(projectile.GetSource_FromThis(), projectile.position, new Vector2(projectile.velocity.X * 0.05f, projectile.velocity.Y * 0.05f), Main.rand.Next(16, 18), 1f);
                        }
                        int[] spread = { 1 };
                        TRAEMethods.SpawnProjectilesFromAbove(Main.player[projectile.owner], projectile.Center, 1, 400, 750, spread, 22, ProjectileID.HallowStar, (int)(projectile.damage * 0.5), projectile.knockBack, projectile.owner);
                        return false;
                    }
                case ProjectileID.HallowStar:
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, projectile.position);
                        int DustCount = 30;
                        int[] DustTypes = { 15, 57, 58 };
                        for (int i = 0; i < DustCount; ++i)
                        {
                            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, Main.rand.Next(DustTypes), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 150, default, 1.5f);
                            dust.noGravity = true;
                        }
                        return false;
                    }
            }
            return true;
        }
        public override void Kill(Projectile projectile, int timeLeft)
        {
            switch (projectile.type)
            {               
                case ProjectileID.ToxicBubble:
                    {
                        int drops = (int)(1 * Math.Pow(projectile.scale, 3));
                        for (int i = 0; i < drops; i++)
                        {
                            float velX = Main.rand.Next(-2, 2);
                            float velY = Main.rand.NextFloat(10, 20);
                            int buble = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + velX, projectile.position.Y, velX, velY, ProjectileType<ToxicDrop>(), (int)(projectile.damage * 0.67), 0, projectile.owner, 0f, 0f);
                            Main.projectile[buble].scale *= projectile.scale;
                        }
                        return;
                    }
            }    
        }        
    }
}