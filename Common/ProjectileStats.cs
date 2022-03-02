using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Summoner.Whip;

using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Common
{
    public class ProjectileStats : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public int AnchorHit = 0;
        public int HitCount = 0;

        // Damage
        public float DamageFalloff = 0f; // How much damage the projectile loses every time it hits an enemy. 
        public float DamageFallon = 1f; // How much damage the projectile gains every time it hits an enemy. 
        public float DirectDamage = 1f; // how much damage the projectile deals when it hits an enemy, independent of the weapon.
        public bool IgnoresDefense = false; // self-explanatory
        public int armorPenetration = 0; //how much defense the projectile ignores
        public bool cantCrit = false; // self-explanatory
        public bool dontHitTheSameEnemyMultipleTimes = false;// self-explanatory
        // Bouncing
		public bool onlyBounceOnce = false;
        public bool BouncesOffTiles = false;
        public bool BouncesBackOffTiles = false;
        public float DamageLossOffATileBounce = 0f;
        public bool BouncesOffEnemies = false;
        public bool SmartBouncesOffTiles = false;
        public bool SmartBouncesOffEnemies = false;
        // AI
        public bool homesIn = false;
        public float homingRange = 300f;
        public bool goThroughWallsUntilReachingThePlayer = false; 
        // Adding Buffs
        public int AddsBuff = 0; // Adds a buff when hitting a target
        public int AddsBuffChance = 1; // 1 in [variable] chance of that buff being applied to the target
        public int AddsBuffDuration = 300; // Measured in ticks, since the game runs at 60 frames per second, this base value is 5 seconds.
        public bool BuffDurationScalesWithMeleeSpeed = false; // If true, the Duration gets multiplied by your extra melee speed
        //
        // Explosion
        public bool explodes = false; // set to true to make the projectile explode. 
        public int ExplosionRadius = 80; // Hitbox size of the base explosion. Base value is 80.
        public float ExplosionDamage = 1f; // Makes the explosion deal Increased/decreased damage. 
        public bool DontRunThisAgain = false;
        public bool dontExplodeOnTiles = false;
        public bool UsesDefaultExplosion = false; // Regular rocket Explosions. Helpful if you are too lazy/don't need to create a special explosion effect.
        //
		 public override void SetStaticDefaults()
        {
            IL.Terraria.Projectile.Damage += DamageHook;
        }

        int extraAP = 0;
        private void DamageHook(ILContext il)
        {
            var c = new ILCursor(il);
            c.Emit(OpCodes.Ldarg_0);
            c.EmitDelegate<Action<Projectile>>((projectile) =>
            {
                if (projectile.GetGlobalProjectile<ProjectileStats>().armorPenetration > 0)
                {
                    Main.player[projectile.owner].armorPenetration += projectile.GetGlobalProjectile<ProjectileStats>().armorPenetration;
                    projectile.GetGlobalProjectile<ProjectileStats>().extraAP += projectile.GetGlobalProjectile<ProjectileStats>().armorPenetration;
                }
            });
        }
        
        public float timer = 0;
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            if (ProjectileID.Sets.IsAWhip[projectile.type] || projectile.type == ProjectileType<WhipProjectile>())
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == projectile.owner && Main.projectile[i].type == projectile.type && Main.projectile[i].whoAmI != projectile.whoAmI)
                    {
                        Main.projectile[i].Kill();
                    }
                }
            }
            if (goThroughWallsUntilReachingThePlayer)
            {
                if (projectile.position.Y > player.position.Y)
                {
                    projectile.tileCollide = true;
                }
            }
            if (homesIn)
            {
                NPC target = null;
                if(TRAEMethods.ClosestNPC(ref target, homingRange, projectile.Center))
                {
                    float scaleFactor2 = projectile.velocity.Length();
                    Vector2 diff = target.Center - projectile.Center;
                    diff.Normalize();
                    diff *= scaleFactor2;
                    projectile.velocity = (projectile.velocity * 24f + diff) / 25f;
                    projectile.velocity.Normalize();
                    projectile.velocity *= scaleFactor2;
                }
            }
            if (explodes && projectile.timeLeft < 3)
            {
                TRAEMethods.Explode(projectile, ExplosionRadius, ExplosionDamage);
                if (UsesDefaultExplosion)
                {
                    TRAEMethods.DefaultExplosion(projectile);
                }
            }
            switch (projectile.type)
            {
                case ProjectileID.NebulaSphere:
                    {
                        if (Main.expertMode)
                        {
                            bool flag4 = false;
                            projectile.localAI[1] += 1f;
                            if (projectile.localAI[1] >= 6f)
                            {
                                projectile.localAI[1] = 6f;
                                flag4 = true;
                            }
                            if (flag4)
                            {
                                projectile.localAI[1] = 0f;
                                projectile.height += 1;
                                projectile.width += 1;
                            }
                            projectile.scale += 0.005f;
                        }
                        break;
                    }
                case ProjectileID.SharknadoBolt:
                    projectile.hostile = false;
                    break;
                case ProjectileID.Sharknado:
                case ProjectileID.Cthulunado:
                    projectile.localAI[1] += 1f; // cannot damage the player before 60 updates
                    if (projectile.localAI[1] <= 60f)
                    {
                        projectile.hostile = false;
                    }
                    else
                    {
                        projectile.hostile = true;
                    }
                    break;
                case ProjectileID.TitaniumStormShard:
                    timer += 0.01f;
                    if (timer <= 1f)
                    {
                        projectile.scale = timer;
                        projectile.damage = 0;
                    }
                    else
					{
                        projectile.damage = 70;         
					    projectile.position.X += projectile.width / 2;
                        projectile.position.Y += projectile.height / 2;
                        projectile.width = projectile.height = 70;
                        projectile.position.X -= projectile.width / 2;
                        projectile.position.Y -= projectile.height / 2;
                        projectile.position.X += projectile.width / 2;
                        projectile.position.Y += projectile.height / 2;
                        projectile.width = projectile.height = 70;
                        projectile.position.X -= (projectile.width / 2);
                        projectile.position.Y -= (projectile.height / 2);
					}
					return;
				}
             

        }
    

    public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (explodes && !dontExplodeOnTiles) // If you want a projectile that doesn't explode in contact with tiles, make the second variable.//
            {
                TRAEMethods.Explode(projectile, ExplosionRadius, ExplosionDamage);
                if (UsesDefaultExplosion)
                {
                    TRAEMethods.DefaultExplosion(projectile);
                }
                return false;
            }
            if (BouncesOffTiles)
            {
				if (onlyBounceOnce)
				{
					BouncesOffTiles = false;
				}
                projectile.velocity.Y = -projectile.oldVelocity.Y;
				return false;
            }
            if (BouncesBackOffTiles)
            { 
                projectile.velocity.X = -projectile.oldVelocity.X;
                projectile.velocity.Y = -projectile.oldVelocity.Y;
            }
            if (DamageLossOffATileBounce > 0)
                projectile.damage -= (int)(projectile.damage * DamageLossOffATileBounce);
            if (SmartBouncesOffTiles)
            {
                int[] array = new int[10];
                int num6 = 0;
                int Range = 700;
                int num8 = 20;
                for (int j = 0; j < 200; j++)
                {
                    if (Main.npc[j].CanBeChasedBy(this, false) && projectile.localNPCImmunity[j] != 1)
                    {
                        float DistanceBetweenProjectileAndEnemy = (projectile.Center - Main.npc[j].Center).Length();
                        if (DistanceBetweenProjectileAndEnemy > num8 && DistanceBetweenProjectileAndEnemy < Range && Collision.CanHitLine(projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                        {
                            array[num6] = j;
                            num6++;
                            if (num6 >= 9)
                            {
                                break;
                            }
                        }
                    }
                }
                if (num6 > 0)
                {
                    num6 = Main.rand.Next(num6);
                    Vector2 value2 = Main.npc[array[num6]].Center - projectile.Center;
                    float scaleFactor2 = projectile.velocity.Length();
                    value2.Normalize();
                    projectile.velocity = value2 * scaleFactor2;
                    projectile.netUpdate = true;
                }
                return false;
            }
            return true;
        }

        public override void ModifyHitPlayer(Projectile projectile, Player target, ref int damage, ref bool crit) // SIMPLIFY THIS
        {
            switch (projectile.type)
            {
                case ProjectileID.DeathLaser:
                    {
                        foreach (NPC enemy in Main.npc)
                        {
                            if (enemy.type == NPCID.Retinazer && Main.expertMode)
                            {
                                damage *= 90;
                                damage /= 100;
                            }
                        }
                    }
                    return;
                case ProjectileID.EyeLaser:
                case ProjectileID.EyeFire:
                    {
                        if (Main.expertMode && ServerConfig.Instance.MechChanges)
                        {
                            damage *= 90;
                            damage /= 100;
                        }
                    }
                    return;
                case ProjectileID.StardustSoldierLaser:
                    {
                        damage *= 80;
                        damage /= 100;
                    }
                    return;
                case ProjectileID.StardustJellyfishSmall:
                    {
                        damage *= 65;
                        damage /= 100;
                    }
                    return;
                case ProjectileID.NebulaLaser:
                    {
                        damage *= 85;
                        damage /= 100;
                    }
                    return;
                case ProjectileID.NebulaBolt:
                    {
                        damage *= 80;
                        damage /= 100;
                    }
                    return;
            }
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit,ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            if (armorPenetration > 0)
            {
                player.armorPenetration += armorPenetration;
                extraAP += armorPenetration;
            }
            damage = (int)(damage * DirectDamage);
            if (IgnoresDefense && target.type != NPCID.DungeonGuardian)
            {
                int finalDefense = target.defense - player.armorPenetration;
                target.ichor = false;
                target.betsysCurse = false;
                if (finalDefense < 0)
                {
                    finalDefense = 0;
                }
                damage += finalDefense / 2;
            }
			if (cantCrit)
			{
				crit = false;
			}
        }
        public override void OnHitPlayer(Projectile projectile, Player target, int damage, bool crit)
        {
            if (explodes)
            {
                TRAEMethods.Explode(projectile, ExplosionRadius, ExplosionDamage);
                if (UsesDefaultExplosion)
                {
                    TRAEMethods.DefaultExplosion(projectile);
                }

            }
            switch (projectile.type)
            {
                case ProjectileID.EyeLaser:
                    if (Main.expertMode)
                    {
                        foreach (NPC enemy in Main.npc)
                        {
                            if (enemy.type == NPCID.Retinazer)
                            {
                                int length = Main.rand.Next(90, 180);
                                target.AddBuff(BuffID.BrokenArmor, length, false);
                            }
                        }
                    }
                    return;
                case ProjectileID.DeathLaser:
                    {
                        if (Main.expertMode)
                        {
                            foreach (NPC enemy in Main.npc)
                            {
                                if (enemy.type == NPCID.Retinazer)
                                {
                                        int length = Main.rand.Next(90, 180);
                                        target.AddBuff(BuffID.BrokenArmor, length, false);
                                }
                            }
                        }
                        return;
                    }
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if(extraAP > 0)
            {
                Main.player[projectile.owner].armorPenetration -= extraAP;
                extraAP = 0;
            }


            if (dontHitTheSameEnemyMultipleTimes)
                projectile.localNPCImmunity[target.whoAmI] = -1; // this makes the enemy invincible to the projectile.
            if (explodes)
            {
                TRAEMethods.Explode(projectile, ExplosionRadius, ExplosionDamage);
                if (UsesDefaultExplosion)
                {
                    TRAEMethods.DefaultExplosion(projectile);
                }
            }
            if (BouncesOffEnemies)
            {
                projectile.velocity.X = -projectile.oldVelocity.X;
                projectile.velocity.Y = -projectile.oldVelocity.Y;
            }
            if (SmartBouncesOffEnemies)
            {
                projectile.localNPCImmunity[target.whoAmI] = -1;
                target.immune[projectile.owner] = 0;
                int[] array = new int[10];
                int num6 = 0;
                int num7 = 700;
                int num8 = 20;
                for (int j = 0; j < 200; j++)
                {
                    if (Main.npc[j].CanBeChasedBy(this, false) || Main.npc[j].type == NPCID.DetonatingBubble && projectile.localNPCImmunity[target.whoAmI] != -1/*see "dontHitTheSameEnemyMultipleTimes" above*/)
                    {
                        float num9 = (projectile.Center - Main.npc[j].Center).Length();
                        if (num9 > num8 && num9 < num7 && Collision.CanHitLine(projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                        {
                            array[num6] = j;
                            num6++;
                            if (num6 >= 9)
                            {
                                break;
                            }
                        }
                    }
                }
                if (num6 > 0)
                {
                    num6 = Main.rand.Next(num6);
                    Vector2 value2 = Main.npc[array[num6]].Center - projectile.Center;
                    float scaleFactor2 = projectile.velocity.Length();
                    value2.Normalize();
                    projectile.velocity = value2 * scaleFactor2;
                    projectile.netUpdate = true;
                }
                return;
            }        
            Player player = Main.player[projectile.owner];
            // Damage Fall off
            projectile.damage -= (int)(projectile.damage * DamageFalloff);
            projectile.damage = (int)(projectile.damage * DamageFallon);
            //
            if (Main.rand.Next(AddsBuffChance) == 0)
            {
                int length = BuffDurationScalesWithMeleeSpeed ? (int)(AddsBuffDuration * (1 + player.meleeSpeed)) : AddsBuffDuration; 
                target.AddBuff(AddsBuff, length, false);          
            }          
        }
        public override bool PreKill(Projectile projectile, int timeLeft)
        { 
            switch (projectile.type)
            {
               
                case ProjectileID.DirtBall:
                    {
                        Item.NewItem(projectile.GetItemSource_DropAsItem(), projectile.getRect(), ItemID.DirtBlock, 1);
                        return false;
                    }
             
                case ProjectileID.StarCloakStar:
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, projectile.position);
                        int DustCount = 30;
                        int[] DustTypes = { 15, 57, 58 };
                        for (int i = 0; i < DustCount; ++i)
                        {
                            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, Main.rand.Next(DustTypes), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 150, default(Color), 1.5f);
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
                case ProjectileID.VortexVortexLightning:
                    {
                        int stormChance = Main.rand.Next(0, 2);
                        if (stormChance == 0 && Main.expertMode)
                        {
                            NPC.NewNPC(projectile.GetNPCSource_FromThis(), (int)projectile.position.X, (int)projectile.position.Y, NPCID.VortexRifleman);
                        }
                    }
                    return;             
            }    
        }        
    }
}