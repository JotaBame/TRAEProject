using Microsoft.Xna.Framework;
using System;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Weapons.Summoner.Whip;

using static Terraria.ModLoader.ModContent;
using TRAEProject.Changes.Accesory;
using Terraria.Audio;
using TRAEProject.NewContent.Items.Weapons.Magic.DreamEater;

namespace TRAEProject.Common
{
    public class ProjectileStats : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public int AnchorHit = 0;
        public int HitCount = 0;

        // Damage
        public int maxHits = -1; 
        public float CritDamage = 0f; // Bonus damage on a critical strike. Stacks additively with other damage sources

        public float DamageFalloff = 0f; // How much damage the projectile loses every time it hits an enemy. 
        public float DamageFallon = 1f; // How much damage the projectile gains every time it hits an enemy. 
        public float DirectDamage = 1f; // how much damage the projectile deals when it hits an enemy, independent of the weapon.
        public float FirstHitDamage = 1f;
        public bool cantCrit = false; // self-explanatory
        public bool dontHitTheSameEnemyMultipleTimes = false;// self-explanatory
                                                             // Bouncing
        public bool BouncesOffTiles = false;
        public bool BouncesBackOffTiles = false;
        public int MaxBounces = -1;

        public float DamageLossOffATileBounce = 0f;
        public bool BouncesOffEnemies = false;
        public bool SmartBouncesOffTiles = false;
        public bool SmartBouncesOffEnemies = false;
        // AI
        public bool homesIn = false;
        public float homingRange = 400f;
        public bool goThroughWallsUntilReachingThePlayer = false;
        // Adding Buffs
        public int AddsBuff = 0; // Adds a buff when hitting a target
        public int AddsBuffChance = 1; // 1 in [variable] chance of that buff being applied to the target
        public int AddedBuffDuration = 300; // Measured in ticks, since the game runs at 60 frames per second, this base value is 5 seconds.
        public int AddedBuffMinDuration = 0; // equals the max duration if it's left at 0. Used when you want some randomness in the duration.
        // Explosion
        public bool explodes = false; // set to true to make the projectile explode. 
        public int ExplosionRadius = 80; // Hitbox size of the base explosion. Base value is 80.
        public float ExplosionDamage = 1f; // Makes the explosion deal Increased/decreased damage. 
        public bool FirstHit = false;
        public bool dontExplodeOnTiles = false;
        public bool UsesDefaultExplosion = false; // Regular rocket Explosions. Helpful if you are too lazy/don't need to create a special explosion effect.
        public bool heavyCritter = false;
        public float timer = 0;
        public override void AI(Projectile projectile)
        {
             Player player = Main.player[projectile.owner];
            if (AddedBuffMinDuration == 0)
                AddedBuffMinDuration = AddedBuffDuration;
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
                if (TRAEMethods.ClosestNPC(ref target, homingRange, projectile.Center))
                {
                    float scaleFactor2 = projectile.velocity.Length();
                    Vector2 diff = target.Center - projectile.Center;
                    diff.Normalize();
                    diff *= scaleFactor2;
                    projectile.velocity = (projectile.velocity * 8f + diff) / 9f;
                    projectile.velocity.Normalize();
                    projectile.velocity *= scaleFactor2;
                }
            }
            if (explodes && projectile.timeLeft < 3)
            {
                TRAEMethods.Explode(projectile, ExplosionRadius);
                if (UsesDefaultExplosion)
                {
                    TRAEMethods.DefaultExplosion(projectile);
                }
            }


        }


        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {

            //if you ever want to add it back
            //DreamEater.ShotTileCollision(projectile, oldVelocity);

            if (explodes && !dontExplodeOnTiles) // If you want a projectile that doesn't explode in contact with tiles, make the second variable true.//
            {
                TRAEMethods.Explode(projectile, ExplosionRadius);
                if (UsesDefaultExplosion)
                {
                    TRAEMethods.DefaultExplosion(projectile);
                }
                return false;
            }
 
            if (MaxBounces > 0 || MaxBounces == -1)
            {
                if (MaxBounces > 0)
                  MaxBounces--;
                if (BouncesOffTiles)
                {
                    SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, projectile.Center);

                    // If the projectile hits the left or right side of the tile, reverse the X velocity
                    if (Math.Abs(projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                    {
                        projectile.velocity.X = -oldVelocity.X;
                    }

                    // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
                    if (Math.Abs(projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                    {
                        projectile.velocity.Y = -oldVelocity.Y;
                    }
                    return false;
                }
                if (BouncesBackOffTiles)
                {
                    SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, projectile.Center);

                    projectile.velocity.X = -projectile.oldVelocity.X;
                    projectile.velocity.Y = -projectile.oldVelocity.Y;
                }
                if (DamageLossOffATileBounce > 0)
                    projectile.damage -= (int)(projectile.damage * DamageLossOffATileBounce);
                if (SmartBouncesOffTiles)
                {
                    SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, projectile.Center);

                    int[] array = new int[10];
                    int num6 = 0;
                    int Range = 700;
                    int num8 = 20;
                    for (int j = 0; j < 200; j++)
                    {
                        if (Main.npc[j].CanBeChasedBy(this, false) && projectile.localNPCImmunity[j] != -1)
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

            }
            return true;

        }
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!FirstHit)
            {
                FirstHit = true;
                modifiers.FinalDamage *= FirstHitDamage;
            }
            Player player = Main.player[projectile.owner];
            modifiers.FinalDamage *= DirectDamage;

            if (cantCrit)
			{
				modifiers.DisableCrit();
			}

            if (heavyCritter)
            {
                modifiers.CritDamage *= 1.25f;

            }
            if (explodes)
            {

                TRAEMethods.Explode(projectile, ExplosionRadius);
                if (UsesDefaultExplosion)
                {
                    TRAEMethods.DefaultExplosion(projectile);
                }
            }

        }
        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            if (explodes)
            {
                TRAEMethods.Explode(projectile, ExplosionRadius);
                if (UsesDefaultExplosion)
                {
                    TRAEMethods.DefaultExplosion(projectile);
                }

            }

            
        }
        int hits = 0;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            
            if (maxHits > -1)
            {
                hits++;
                if (hits == maxHits)
                    projectile.damage = 0;

            }
            if (AddsBuff != 0 && Main.rand.NextBool(AddsBuffChance))
            {
                int duration = Main.rand.Next(AddedBuffMinDuration, AddedBuffDuration + 1);
                target.AddBuff(AddsBuff, duration);
            }
            if (dontHitTheSameEnemyMultipleTimes)
                projectile.localNPCImmunity[target.whoAmI] = -1; // this makes the enemy invincible to the projectile.

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
                    if ((Main.npc[j].CanBeChasedBy(this, false) || Main.npc[j].type == NPCID.DetonatingBubble) && (!projectile.usesLocalNPCImmunity || projectile.localNPCImmunity[Main.npc[j].whoAmI] == 0))
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
     
        }
        public override bool PreKill(Projectile projectile, int timeLeft)
        { 
            switch (projectile.type)
            {
               
                case ProjectileID.DirtBall:
                    {
                        Item.NewItem(projectile.GetSource_DropAsItem(), projectile.getRect(), ItemID.DirtBlock, 1);
                        return false;
                    }
             
                case ProjectileID.StarCloakStar:
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, projectile.position);
                        int DustCount = 30;
                        int[] DustTypes = { 15, 57, 58 };
                        for (int i = 0; i < DustCount; ++i)
                        {
                            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, Main.rand.Next(DustTypes), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 150, default, 1.5f);
                            dust.noGravity = true;
                        }
                        for (int i = 0; i < DustCount / 2; i++)
                        {
                            Color sparkleColor = Color.Lerp(Color.Purple, Color.HotPink, Main.rand.NextFloat());
                            Vector2 sparkleScale = new Vector2(2, 1);
                            Vector2 sparkleVel = TRAEMethods.PolarVector(Main.rand.NextFloat() * 8 + 2, Main.rand.NextFloat() * MathF.Tau);
                            Sparkle.NewSparkle(projectile.Center - sparkleVel, sparkleColor, sparkleScale, sparkleVel, Main.rand.Next(20, 40), sparkleScale, null, 1, sparkleVel.ToRotation(), .9f);
                        }
                        return false;
                    }
            }
            return true;
        }       
    }
}