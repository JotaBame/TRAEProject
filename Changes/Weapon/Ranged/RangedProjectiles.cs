using TRAEProject.NewContent.Buffs;
using TRAEProject.Common;
using TRAEProject.NewContent.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using TRAEProject.Common.ModPlayers;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;
using TRAEProject.Changes.Accesory;
using static Terraria.ModLoader.PlayerDrawLayer;
using Terraria.WorldBuilding;
 
namespace TRAEProject.Changes.Projectiles
{
    public class RangedProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile projectile)
        {
            switch (projectile.type)
            {
 
                case ProjectileID.NailFriendly:
                    projectile.penetrate = 3;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    return;
                case ProjectileID.Stynger:
                    projectile.GetGlobalProjectile<ProjectileStats>().DirectDamage = 1.25f;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    return;
                case ProjectileID.StyngerShrapnel:
                    projectile.GetGlobalProjectile<ProjectileStats>().DirectDamage = 0.75f;
                    projectile.penetrate = 2; 
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    return;
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
                case ProjectileID.ClusterSnowmanFragmentsI:
                    projectile.penetrate = 2;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    return;
                case ProjectileID.SuperStarSlash:
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = -1; projectile.usesIDStaticNPCImmunity = false;
                    projectile.GetGlobalProjectile<ProjectileStats>().DirectDamage /= 0.75f;
                     return;


                case ProjectileID.BoneArrowFromMerchant:
                    projectile.penetrate = 1;
                    return;
                case ProjectileID.CursedDartFlame:
                    projectile.timeLeft = 75;
                    projectile.penetrate = 1;
                    return;
                case ProjectileID.IchorDart:
                    projectile.GetGlobalProjectile<ProjectileStats>().DirectDamage = 0.8f;
                    return;
                case ProjectileID.CrystalDart:
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.2f;
                    projectile.GetGlobalProjectile<ProjectileStats>().DamageLossOffATileBounce = 0.2f;
                    return;
                case ProjectileID.JestersArrow:
                    projectile.penetrate = 5;      
					projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 20;
                    return;
                case ProjectileID.Stake:
                    projectile.penetrate = 7;
                    return;
                case ProjectileID.CursedArrow:
                    projectile.extraUpdates = 1;
                    return;
                case ProjectileID.UnholyArrow:   
					projectile.penetrate = 3;   				
					projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 20;
                    return;
                case ProjectileID.VortexBeaterRocket:
                    projectile.penetrate = 5;
                    projectile.scale = 1.15f;
                    projectile.GetGlobalProjectile<ProjectileStats>().explodes = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().ExplosionRadius = 240;
                    projectile.GetGlobalProjectile<ProjectileStats>().DirectDamage = 2f;
                    return;
                case ProjectileID.HellfireArrow:
                    projectile.penetrate = 4;
                    projectile.extraUpdates = 1;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    return;
                case ProjectileID.IchorBullet:
                    projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;
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
                    return;
                case ProjectileID.Grenade:
                    projectile.penetrate = 5;
                     return;
                case ProjectileID.MechanicalPiranha:
                    projectile.ContinuouslyUpdateDamageStats = true;
                    break;
      
            }
        }
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.arrow && !player.HeldItem.IsAir && (player.HeldItem.type == ItemID.Tsunami || player.HeldItem.type == ItemID.MythrilRepeater))
            {
                projectile.extraUpdates += 1;
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
        bool dontDoThisAgain = false;
        public override bool PreAI(Projectile projectile)
        {
            if(projectile.type == ProjectileID.FairyQueenRangedItemShot)
            {
                //Main.NewText(projectile.ai[1]);
            }
            Player player = Main.player[projectile.owner];
            if (projectile.type == ProjectileID.NailFriendly)
            {
                if (projectile.ai[1] == 90f)
                {
                    projectile.ai[1] = 91f;
                    SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, projectile.position);
                    for (int num760 = 0; num760 < 10; num760++)
                    {
                        int num761 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1.3f);
                        Dust dust2 = Main.dust[num761];
                        dust2.velocity *= 1.4f;
                    }
                    for (int num762 = 0; num762 < 6; num762++)
                    {
                        int num763 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2.1f);
                        Main.dust[num763].noGravity = true;
                        Dust dust2 = Main.dust[num763];
                        dust2.velocity *= 4.6f;
                        num763 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1.3f);
                        dust2 = Main.dust[num763];
                        dust2.velocity *= 3.3f;
                        if (Main.rand.Next(2) == 0)
                        {
                            num763 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1.1f);
                            dust2 = Main.dust[num763];
                            dust2.velocity *= 2.7f;
                        }
                    }
                    if (projectile.owner == Main.myPlayer)
                    {
                        projectile.position.X += projectile.width / 2;
                        projectile.position.Y += projectile.height / 2;
                        projectile.width = 112;
                        projectile.height = 112;
                        projectile.position.X -= projectile.width / 2;
                        projectile.position.Y -= projectile.height / 2;
                        projectile.ai[0] = 2f;
                        projectile.alpha = 255;
                        projectile.penetrate = 3;
                        projectile.timeLeft = 3;
                    }
                    return false;
                }

            }
        
            if (!dontDoThisAgain)
            {
                dontDoThisAgain = true;
                if (projectile.CountsAsClass(DamageClass.Ranged) && (player.GetModPlayer<RangedStats>().GunScope || player.GetModPlayer<RangedStats>().AlphaScope > 0 || player.GetModPlayer<RangedStats>().ReconScope > 0))
                {
                    if (projectile.owner == player.whoAmI && projectile.type != ProjectileID.Phantasm && projectile.type != ProjectileID.VortexBeater && projectile.type != ProjectileID.DD2PhoenixBow)
                    {
                        projectile.extraUpdates += 1;
                    }
                }
            }
            if (projectile.type == ProjectileID.ClusterSnowmanFragmentsI)
            {
                timer++;
            }
               if (projectile.type == ProjectileID.ChlorophyteArrow)
			{
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
                ++ChloroBulletTime;
                if (ChloroBulletTime <= 30)
                {
                    float num18 = projectile.position.X;
                    float num19 = projectile.position.Y;
                    float homingRange = 450f; // down from 600
                    bool flag3 = false;
                    for (int m = 0; m < 200; m++)
                    {
                        NPC npc = Main.npc[m];
                        if (npc.CanBeChasedBy(this))
                        {
                            float num21 = npc.Center.X;
                            float num22 = npc.Center.Y;
                            float num23 = Math.Abs(projectile.Center.X - num21) + Math.Abs(projectile.Center.Y - num22);
                            if (num23 < homingRange && Collision.CanHit(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                            {
                                homingRange = num23;
                                num18 = num21;
                                num19 = num22;
                                flag3 = true;
                            }
                        }
                    }

                    if (!flag3)
                    {
                        num18 = projectile.Center.X + projectile.velocity.X * 100f;
                        num19 = projectile.Center.Y + projectile.velocity.Y * 100f;
                    }
                    float num24 = 16f;
                    Vector2 vector = new(projectile.Center.X, projectile.Center.Y);
                    float num25 = num18 - vector.X;
                    float num26 = num19 - vector.Y;
                    float num27 = (float)Math.Sqrt(num25 * num25 + num26 * num26);
                    num27 = num24 / num27;
                    num25 *= num27;
                    num26 *= num27;
                    projectile.velocity.X = (projectile.velocity.X * 8f + num25) / 9f;
                    projectile.velocity.Y = (projectile.velocity.Y * 8f + num26) / 9f;
                    return false;
                }
                return true;
            }

            if (projectile.type == ProjectileID.ChlorophyteBullet)
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

                ++ChloroBulletTime; // once this reaches 30, the bullet loses its homing
                if (ChloroBulletTime > 4 && ChloroBulletTime <= 30)
                {
                    for (int i = 0; i < 9; i++) // vanilla value is 10, once the homing disappears the trail loses intensity
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
                    float num18 = projectile.position.X;
                    float num19 = projectile.position.Y;
                    float homingRange = 450f; // down from 600
                    bool flag3 = false;
                    for (int m = 0; m < 200; m++)
                    {
                        NPC npc = Main.npc[m];
                        if (npc.CanBeChasedBy(this))
                        {
                            float num21 = npc.Center.X;
                            float num22 = npc.Center.Y;
                            float num23 = Math.Abs(projectile.Center.X - num21) + Math.Abs(projectile.Center.Y - num22);
                            if (num23 < homingRange && Collision.CanHit(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
                            {
                                homingRange = num23;
                                num18 = num21;
                                num19 = num22;
                                flag3 = true;
                            }
                        }
                    }

                    if (!flag3)
                    {
                        num18 = projectile.Center.X + projectile.velocity.X * 100f;
                        num19 = projectile.Center.Y + projectile.velocity.Y * 100f;
                    }
                    float num24 = 16f;
                    Vector2 vector = new(projectile.Center.X, projectile.Center.Y);
                    float num25 = num18 - vector.X;
                    float num26 = num19 - vector.Y;
                    float num27 = (float)Math.Sqrt(num25 * num25 + num26 * num26);
                    num27 = num24 / num27;
                    num25 *= num27;
                    num26 *= num27;
                    projectile.velocity.X = (projectile.velocity.X * 8f + num25) / 9f;
                    projectile.velocity.Y = (projectile.velocity.Y * 8f + num26) / 9f;
                    return false;
                }
                if (ChloroBulletTime >= 30) // Due to the bullet having 2 extraUpdates, this happens after 8 frames.
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
                        SoundEngine.PlaySound(SoundID.Item5 with { MaxInstances = 0 }, projectile.position);
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
            if (projectile.type == ProjectileID.DD2PhoenixBow)
            {
                float num = (float)Math.PI / 2f;
                Vector2 mountedCenter = player.MountedCenter;
                Vector2 vector = player.RotatedRelativePoint(mountedCenter);
                int num2 = 2;
                float num3 = 0f;


                num = 0f;
                if (projectile.spriteDirection == -1)
                {
                    num = (float)Math.PI;
                }
                projectile.ai[0] += 1f;
                int itemAnimationMax = player.itemAnimationMax;
                projectile.ai[1] -= 1f;
                bool flag14 = false;
                if (projectile.ai[1] <= 0f)
                {
                    projectile.ai[1] = itemAnimationMax;
                    flag14 = true;
                }
                bool canShoot4 = player.channel && player.HasAmmo(player.inventory[player.selectedItem]) && !player.noItems && !player.CCed;
                if (projectile.localAI[0] > 0f)
                {
                    projectile.localAI[0] -= 1f;
                }
                if (projectile.soundDelay <= 0 && canShoot4)
                {
                    projectile.soundDelay = itemAnimationMax;
                    if (projectile.ai[0] != 1f)
                    {
                        SoundEngine.PlaySound(SoundID.Item5, projectile.position);
                    }
                    projectile.localAI[0] = 12f;
                }
                if (flag14 && Main.myPlayer == projectile.owner)
                {
                    int projToShoot4 = 14;
                    float speed4 = 12f;
                    int Damage4 = player.GetWeaponDamage(player.inventory[player.selectedItem]);
                    float KnockBack4 = player.inventory[player.selectedItem].knockBack;
                    int projCount = 2;
                    float num79 = 1.5f;
                    if (canShoot4)
                    {
                        player.PickAmmo(player.inventory[player.selectedItem], out projToShoot4, out speed4, out Damage4, out KnockBack4, out var usedAmmoItemId4);
                        IEntitySource projectileSource_Item_WithPotentialAmmo4 = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, usedAmmoItemId4);
                        KnockBack4 = player.GetWeaponKnockback(player.inventory[player.selectedItem], KnockBack4);
                        if (projToShoot4 == 1)
                        {
                            projToShoot4 = 2;
                        }

                        float num80 = player.inventory[player.selectedItem].shootSpeed * projectile.scale;
                        Vector2 vector34 = vector;
                        Vector2 value12 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - vector34;
                        if (player.gravDir == -1f)
                        {
                            value12.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector34.Y;
                        }
                        Vector2 vector35 = Vector2.Normalize(value12);
                        if (float.IsNaN(vector35.X) || float.IsNaN(vector35.Y))
                        {
                            vector35 = -Vector2.UnitY;
                        }
                        vector35 *= num80;
                        if (vector35.X != projectile.velocity.X || vector35.Y != projectile.velocity.Y)
                        {
                            projectile.netUpdate = true;
                        }
                        projectile.velocity = vector35 * 0.55f;
                        for (int num81 = 0; num81 < projCount; num81++)
                        {
                            Vector2 vector36 = Vector2.Normalize(projectile.velocity) * speed4;
                            vector36 += Main.rand.NextVector2Square(0f - num79 * player.GetModPlayer<RangedStats>().spreadModifier, num79 * player.GetModPlayer<RangedStats>().spreadModifier);
                            if (float.IsNaN(vector36.X) || float.IsNaN(vector36.Y))
                            {
                                vector36 = -Vector2.UnitY;
                            }
                            Vector2 vector37 = vector34;
                            int num82 = Projectile.NewProjectile(projectileSource_Item_WithPotentialAmmo4, vector37.X, vector37.Y, vector36.X, vector36.Y, projToShoot4, Damage4, KnockBack4, projectile.owner);
                            Main.projectile[num82].noDropItem = true;
                        }
                        if (++player.phantomPhoneixCounter >= 3)
                        {
                            player.phantomPhoneixCounter = 0;
                             Damage4 *= 2;
                            num79 = 0f;
                            projectile.ai[1] *= 1.5f;
                            projToShoot4 = 706;
                            speed4 = 16f; 
                            Vector2 vector36 = Vector2.Normalize(projectile.velocity) * speed4;
                            vector36 += Main.rand.NextVector2Square(0f - num79, num79);
                            if (float.IsNaN(vector36.X) || float.IsNaN(vector36.Y))
                            {
                                vector36 = -Vector2.UnitY;
                            }
                            Vector2 vector37 = vector34;
                            int num82 = Projectile.NewProjectile(projectileSource_Item_WithPotentialAmmo4, vector37.X, vector37.Y, vector36.X, vector36.Y, projToShoot4, Damage4, KnockBack4, projectile.owner);

                        }
                    }
                    else
                    {
                        projectile.Kill();
                    }
                }

                projectile.position = player.RotatedRelativePoint(mountedCenter, reverseRotation: false, addGfxOffY: false) - projectile.Size / 2f;
                projectile.rotation = projectile.velocity.ToRotation() + num;
                projectile.spriteDirection = projectile.direction;
                projectile.timeLeft = 2;
                player.ChangeDir(projectile.direction);
                player.heldProj = projectile.whoAmI;
                player.SetDummyItemTime(num2);
                player.itemRotation = MathHelper.WrapAngle((float)Math.Atan2(projectile.velocity.Y * (float)projectile.direction, projectile.velocity.X * (float)projectile.direction) + num3);

                return false;
            }

            // VBEATER PROJECTILE CODE
            if (projectile.type == 615)
            {
                Vector2 vector = player.RotatedRelativePoint(player.MountedCenter);

                float num = 0f;
                if (projectile.spriteDirection == -1)
                {
                    num = (float)Math.PI;
                }
                projectile.ai[0] += 1f;
                int num52 = 0;
                if (projectile.ai[0] >= 40f)
                {
                    num52++;
                }
                if (projectile.ai[0] >= 80f)
                {
                    num52++;
                }
                if (projectile.ai[0] >= 120f)
                {
                    num52++;
                }
                int num53 = 5;
                int num54 = 0;
                projectile.ai[1] -= 1f;
                bool flag11 = false;
                int num55 = -1;
                if (projectile.ai[1] <= 0f)
                {
                    projectile.ai[1] = num53 - num54 * num52;
                    flag11 = true;
                    int num56 = (int)projectile.ai[0] / (num53 - num54 * num52);
                    if (num56 % 7 == 0)
                    {
                        num55 = 0;
                    }
                }
                projectile.frameCounter += 1 + num52;
                if (projectile.frameCounter >= 4)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                    if (projectile.frame >= Main.projFrames[projectile.type])
                    {
                        projectile.frame = 0;
                    }
                }
                if (projectile.soundDelay <= 0)
                {
                    projectile.soundDelay = num53 - num54 * num52;
                    if (projectile.ai[0] != 1f)
                    {
                        SoundEngine.PlaySound(SoundID.Item36, projectile.position);
                    }
                }
                if (flag11 && Main.myPlayer == projectile.owner)
                {
                    bool canShoot = player.channel && player.HasAmmo(player.inventory[player.selectedItem]) && !player.noItems && !player.CCed;
                    int projToShoot = 14;
                    float speed = 14f;
                    int Damage = player.GetWeaponDamage(player.inventory[player.selectedItem]);
                    float KnockBack = player.inventory[player.selectedItem].knockBack;
                    if (canShoot)
                    {
                        bool HasAmmo = player.PickAmmo(player.inventory[player.selectedItem], out projToShoot, out speed, out Damage, out KnockBack, out var usedAmmoItemId);

                         IEntitySource projectileSource_Item_WithPotentialAmmo = player.GetSource_ItemUse_WithPotentialAmmo(player.HeldItem, usedAmmoItemId);
                        KnockBack = player.GetWeaponKnockback(player.inventory[player.selectedItem], KnockBack);
                        float num57 = player.inventory[player.selectedItem].shootSpeed * projectile.scale;
                        Vector2 vector25 = vector;
                        Vector2 value9 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) - vector25;
                        if (player.gravDir == -1f)
                        {
                            value9.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector25.Y;
                        }
                        Vector2 spinningpoint6 = Vector2.Normalize(value9);
                        if (float.IsNaN(spinningpoint6.X) || float.IsNaN(spinningpoint6.Y))
                        {
                            spinningpoint6 = -Vector2.UnitY;
                        }
                        spinningpoint6 *= num57;
                        spinningpoint6 = spinningpoint6.RotatedBy(Main.rand.NextDouble() * 0.13089969754219055 * player.GetModPlayer<RangedStats>().spreadModifier - 0.06544984877109528 * player.GetModPlayer<RangedStats>().spreadModifier) ;
                        if (spinningpoint6.X != projectile.velocity.X || spinningpoint6.Y != projectile.velocity.Y)
                        {
                            projectile.netUpdate = true;
                        }
                        projectile.velocity = spinningpoint6;
                        for (int n = 0; n < 1; n++)
                        {
                            Vector2 spinningpoint7 = Vector2.Normalize(projectile.velocity) * speed;
                            spinningpoint7 = spinningpoint7.RotatedBy(Main.rand.NextDouble() * 0.19634954631328583 * player.GetModPlayer<RangedStats>().spreadModifier - 0.09817477315664291 * player.GetModPlayer<RangedStats>().spreadModifier) ;
                            if (float.IsNaN(spinningpoint7.X) || float.IsNaN(spinningpoint7.Y))
                            {
                                spinningpoint7 = -Vector2.UnitY;
                            }
                            Projectile.NewProjectile(projectileSource_Item_WithPotentialAmmo, vector25.X, vector25.Y, spinningpoint7.X, spinningpoint7.Y, projToShoot, Damage, KnockBack, projectile.owner);
                        }
                        if (num55 == 0)
                        {
                            projToShoot = 616;
                            speed = 8f;
                            for (int num58 = 0; num58 < 1; num58++)
                            {
                                Vector2 spinningpoint8 = Vector2.Normalize(projectile.velocity) * speed;
                                spinningpoint8 = spinningpoint8.RotatedBy(Main.rand.NextDouble() * 0.39269909262657166 - 0.19634954631328583);
                                if (float.IsNaN(spinningpoint8.X) || float.IsNaN(spinningpoint8.Y))
                                {
                                    spinningpoint8 = -Vector2.UnitY;
                                }
                                Projectile.NewProjectile(projectileSource_Item_WithPotentialAmmo, vector25.X, vector25.Y, spinningpoint8.X, spinningpoint8.Y, projToShoot, Damage + 20, KnockBack * 1.25f, projectile.owner);
                            }
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
                projectile.position.Y += player.gravDir * 2f;
            }




            if (projectile.type == 479 && Main.myPlayer == projectile.owner)
            {
                projectile.alpha = 0;
             projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

                if (projectile.ai[1] >= 0f)
                {
                    projectile.maxPenetrate = (projectile.penetrate = -1);
                }
                else if (projectile.penetrate < 0)
                {
                    projectile.maxPenetrate = (projectile.penetrate = 1);
                }
                if (projectile.ai[1] >= 0f)
                {
                    projectile.ai[1] += 1f;
                }
                if (projectile.ai[1] > (float)Main.rand.Next(5, 30))
                {
                    projectile.ai[1] = -1000f;
                    float num227 = projectile.velocity.Length();
                    Vector2 vector33 = projectile.velocity;
                    vector33.Normalize();
                    int num228 = Main.rand.Next(2, 4);
                    if (Main.rand.NextBool(4))
                    {
                        num228++;
                    }
                    for (int num229 = 0; num229 < num228; num229++)
                    {
                        Vector2 vector34 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                        vector34.Normalize();
                        vector34 += vector33 * 2f ;
                        vector34.Normalize();
                        vector34 *= num227;
                       Projectile.NewProjectile(player.GetSource_FromThis(), projectile.Center.X, projectile.Center.Y, vector34.X, vector34.Y * player.GetModPlayer<RangedStats>().spreadModifier, projectile.type, (int)((float)projectile.damage * 0.9f), projectile.knockBack, projectile.owner, 0f, -1000f);
                        Vector2 zero2 = Vector2.Zero;
                        Dust dust40;
                        for (int num230 = 0; num230 < 4; num230++)
                        {
                            zero2 = vector34 * (0.4f + (float)num230 * 0.075f);
                            dust40 = Main.dust[Dust.NewDust(projectile.Center + zero2 * 1.5f, 2, 2, 170, zero2.X, zero2.Y)];
                            dust40.noGravity = true;
                        }
                        zero2 = vector34 * 0.2f;
                        dust40 = Main.dust[Dust.NewDust(projectile.Center, 2, 2, 170, 0f - zero2.X, 0f - zero2.Y)];
                        dust40.noGravity = true;
                    }
                }
                return false;
            }
            return true;
        }
        public override bool? CanHitNPC(Projectile projectile, NPC target)
        {
            if (projectile.type == ProjectileID.ClusterSnowmanFragmentsI && timer < 15)
            {
                return false;
            }
            return null;
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
                    break;
                case ProjectileID.Xenopopper:
                    if(Main.player[projectile.owner].channel && projectile.timeLeft < 2)
                    {
                        projectile.timeLeft = 2;
                    }
                    break;
       
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
            }
            return true;
        }

        private static int tillinsta = 0;
        private static int shootDelay = 0;
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (projectile.type == ProjectileID.NailFriendly)
            {
              modifiers.SourceDamage /= projectile.ai[0] != 2f ? 0.4f : 1.35f; // makes it deal 37,5% or 100% damage.
            }
            return; 
        }
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            if (projectile.type == ProjectileID.NailFriendly)
            {
                modifiers.SourceDamage /= projectile.ai[0] != 2f ? 0.4f : 1.35f; // makes it deal 37,5% or 100% damage.
            }
            return;
        }
    
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
           
            Player player = Main.player[projectile.owner];
            switch (projectile.type)
            {
                case ProjectileID.VenomArrow:
                case ProjectileID.VenomBullet:
                    float range = 100f; 
                    for (int k = 0; k < 200; k++)
                    {
                        int NPCLimit = 0;
                        NPC nPC = Main.npc[k];
                        if (nPC.active && !nPC.friendly && nPC.damage > 0 && !nPC.dontTakeDamage && Vector2.Distance(target.Center, nPC.Center) <= range)
                        {
                            ++NPCLimit;
                            if (NPCLimit < 3)
                            {

                                nPC.AddBuff(BuffID.Venom, 120);

                            }
                        }
                    }
                    break;
                case ProjectileID.StyngerShrapnel:
                    SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, projectile.position);
                    for (int num958 = 0; num958 < 7; num958++)
                    {
                        int num959 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                        Dust dust2 = Main.dust[num959];
                        dust2.velocity *= 0.8f;
                    }
                    for (int num960 = 0; num960 < 2; num960++)
                    {
                        int num961 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2.5f);
                        Main.dust[num961].noGravity = true;
                        Dust dust2 = Main.dust[num961];
                        dust2.velocity *= 2.5f;
                        num961 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                        dust2 = Main.dust[num961];
                        dust2.velocity *= 1.5f;
                    }
                    int num962 = Gore.NewGore(projectile.GetSource_FromThis(), new Vector2(projectile.position.X, projectile.position.Y), default(Vector2), Main.rand.Next(61, 64));
                    Gore gore2 = Main.gore[num962];
                    gore2.velocity *= 0.2f;
                    Main.gore[num962].velocity.X += Main.rand.Next(-1, 2);
                    Main.gore[num962].velocity.Y += Main.rand.Next(-1, 2);
                    projectile.position.X += projectile.width / 2;
                    projectile.position.Y += projectile.height / 2;
                    projectile.width = 100;
                    projectile.height = 100; 
                    projectile.alpha = 255;

                    projectile.position.X -= projectile.width / 2;
                    projectile.position.Y -= projectile.height / 2;
                    return;
                case ProjectileID.UnholyArrow:
                case ProjectileID.PoisonDartBlowgun:
                    {
                        int Duration = Main.rand.Next(200, 500);
                        target.AddBuff(BuffID.Poisoned, Duration, false);
                        if (Main.rand.NextBool(16))
                        {
                            for (int num840 = 0; num840 < 15; num840++)
                            {
                                Dust dust54 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Venom, 0f, 0f);
                                dust54.fadeIn = 0f;
                                Dust dust = dust54;
                                dust.velocity *= 0.5f;
                            }
                            target.AddBuff(BuffID.Venom, 60, false);
                        }
                        return;
                    }
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
                case ProjectileID.NailFriendly:
                    return false;
                case ProjectileID.BoneArrowFromMerchant:
                    {
                        if (Main.myPlayer == Main.player[projectile.owner].whoAmI && Main.rand.NextBool(2))
                        {
                            float num602 = (0f - projectile.velocity.X) * (float)Main.rand.Next(40, 70) * 0.01f + (float)Main.rand.Next(-20, 21) * 0.4f;
                            float num603 = (0f - projectile.velocity.Y) * (float)Main.rand.Next(40, 70) * 0.01f + (float)Main.rand.Next(-20, 21) * 0.4f;
                            Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.position.X + num602, projectile.position.Y + num603, num602, num603, ProjectileID.Bone, (int)((double)projectile.damage * 0.5), 0f, projectile.owner);
                        }
                    }
                    return true;
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
                        SoundEngine.PlaySound(SoundID.Item62 with { MaxInstances = 0 }, projectile.position);
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
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, projectile.position);
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
               


            }
            return true;
        }
        public override void OnKill(Projectile projectile, int timeLeft)
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