using TRAEProject.NewContent.Projectiles;
using TRAEProject.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using static Terraria.ModLoader.ModContent;
using static Terraria.ModLoader.PlayerDrawLayer;
using System;

namespace TRAEProject.Changes.Items
{
    public class MagicProjectile : GlobalProjectile
    {
        public float manaDrain = 0;
        public int DrainManaPassively = 0;
        public int DrainManaOnHit = 0;
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.aiStyle == 99)
            {
                projectile.usesIDStaticNPCImmunity = true;
                projectile.idStaticNPCHitCooldown = 10;
            }
            //
            switch (projectile.type)
            {
                // 
                // Mage
                case ProjectileID.MagicDagger:
                    projectile.penetrate = 3;
                    break;
                case ProjectileID.BookOfSkullsSkull:
                    projectile.timeLeft = 180;
                    break;
                case ProjectileID.ShadowBeamFriendly:
                    projectile.GetGlobalProjectile<ProjectileStats>().SmartBouncesOffEnemies = true;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().dontHitTheSameEnemyMultipleTimes = true;
                    break;
                case ProjectileID.WeatherPainShot:
                    projectile.penetrate = -1;
                    DrainManaOnHit = TRAEMagicItem.WeatherPainOnHitCost;
                    projectile.idStaticNPCHitCooldown = 30; // up from 25
                    DrainManaPassively = TRAEMagicItem.WeatherPainPassiveCost;
                    projectile.timeLeft = 3600; // this is irrelevant, its duration is set through code, check AI
                    break;
                case ProjectileID.ManaCloakStar:
                    projectile.penetrate = 2;
                    projectile.GetGlobalProjectile<ProjectileStats>().homesIn = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().homingRange = 600f;
                    projectile.GetGlobalProjectile<ProjectileStats>().dontHitTheSameEnemyMultipleTimes = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().cantCrit = true;
                    projectile.tileCollide = false;
                    projectile.timeLeft = 120;
                    break;
                case ProjectileID.EighthNote:
                case ProjectileID.TiedEighthNote:
                case ProjectileID.QuarterNote:
                    projectile.penetrate = 5;
                    break;
                case ProjectileID.Typhoon:
                    DrainManaOnHit = TRAEMagicItem.TyphoonOnHitCost;                    
					DrainManaPassively = TRAEMagicItem.TyphoonPassiveCost; // this has extra updates, it's 24 mana per second

                    projectile.timeLeft = 1800;
                    break;
                case ProjectileID.ToxicFlask:
                    projectile.timeLeft = 75;
                    break;
                case ProjectileID.FlowerPetal: // what the fuck is this projectile, why can't i remember
                    projectile.usesLocalNPCImmunity = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().homesIn = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().dontHitTheSameEnemyMultipleTimes = true;
                    break;
                case ProjectileID.SharpTears:
                    projectile.penetrate = 5;
                    break;
                case ProjectileID.WaterStream:
                    projectile.penetrate = 1;
                    break;     
				case ProjectileID.ClingerStaff:
                    DrainManaOnHit = TRAEMagicItem.ClingerOnHitCost;
                    DrainManaPassively = TRAEMagicItem.ClingerPassiveCost;
                    break;

                case ProjectileID.RainbowFront:
                case ProjectileID.RainbowBack:
                    projectile.usesIDStaticNPCImmunity = true;
                    projectile.idStaticNPCHitCooldown = 10;
                    DrainManaOnHit = TRAEMagicItem.RainbowOnHitCost;
                    DrainManaPassively = TRAEMagicItem.RainbowPassiveCost;
                    break;
			    case 244:
				  case 238:
					projectile.timeLeft = 1200;
					break;			
				case ProjectileID.BloodRain:
                case ProjectileID.RainFriendly:
                    projectile.penetrate = 1;
                    break;
           
                case ProjectileID.Blizzard:
                    projectile.timeLeft = 150;
                    projectile.GetGlobalProjectile<ProjectileStats>().homesIn = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().homingRange = 500f;
                    break;
                case ProjectileID.Meteor1:
                case ProjectileID.Meteor2:
                case ProjectileID.Meteor3:
                    projectile.tileCollide = false;
                    projectile.GetGlobalProjectile<ProjectileStats>().goThroughWallsUntilReachingThePlayer = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().homesIn = true;
                    projectile.GetGlobalProjectile<ProjectileStats>().homingRange = 100f;
                    break;
                case ProjectileID.ShadowFlame:
		projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    break;
				 case ProjectileID.Wasp:
				 	projectile.penetrate = 2;
					projectile.timeLeft = 120;
					projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = 10;
                    break;
				case ProjectileID.NebulaArcanum:
                    projectile.extraUpdates = 2;
                    break;            
                case ProjectileID.GoldenShowerFriendly:
                    projectile.penetrate = 3; // down from 5
                    //projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.05f;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = -1;
                    break;
				case ProjectileID.FrostBoltStaff:
                    projectile.penetrate = 2;
                    break;
                case ProjectileID.SapphireBolt:
                case ProjectileID.EmeraldBolt:
                case ProjectileID.AmberBolt:
                case ProjectileID.RubyBolt:
                case ProjectileID.DiamondBolt:
                    projectile.penetrate = 2;
                    projectile.GetGlobalProjectile<ProjectileStats>().dontHitTheSameEnemyMultipleTimes = true;
                    projectile.usesLocalNPCImmunity = true;
                    break;
                case ProjectileID.CursedFlameFriendly:
                    projectile.GetGlobalProjectile<ProjectileStats>().DamageLossOffATileBounce = 0.25f;
                    projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff= 0.25f;
                    projectile.usesLocalNPCImmunity = true;
                    break;
                case ProjectileID.BoulderStaffOfEarth:
                    projectile.penetrate = 4;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = -1;
                    break;
                case ProjectileID.InfernoFriendlyBolt:
                    projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.Daybreak;
                    projectile.GetGlobalProjectile<ProjectileStats>().AddsBuffDuration = 240;
                    break;
                case ProjectileID.InfernoFriendlyBlast:              
                    projectile.penetrate = 16;
                    break;
            }
        }

        public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox)
        {
            switch (projectile.type)
            {
                case ProjectileID.Blizzard:
                    hitbox.Width = hitbox.Height = 50;
                    return;
            }
        }
        public override bool TileCollideStyle(Projectile projectile, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            switch (projectile.type)
            {
				case ProjectileID.BallofFrost:
                    fallThrough = false; // prevents these projectiles from falling through platforms
                    return true;
            }
            return true;
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (projectile.type == ProjectileID.CursedFlameFriendly)
                projectile.penetrate -= 1;
            return true;
        }
        public override bool? CanDamage(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            if (player.statMana < DrainManaOnHit * player.manaCost)
            {
                return false;
            }

            return null;
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];

            if (DrainManaOnHit > 0)
            {
                if (player.statMana < DrainManaOnHit * player.manaCost)
                {
                    projectile.Kill();
                }
                player.UseManaOverloadable((int)(DrainManaOnHit * player.manaCost));

            }
        }
        public override bool PreAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.type == ProjectileID.MagnetSphereBall)
            {

                if (projectile.ai[0] == 0f)
                {
                    projectile.ai[0] = projectile.velocity.X;
                    projectile.ai[1] = projectile.velocity.Y;
                }
                if (projectile.velocity.X > 0f)
                {
                    projectile.rotation += (Math.Abs(projectile.velocity.Y) + Math.Abs(projectile.velocity.X)) * 0.001f;
                }
                else
                {
                    projectile.rotation -= (Math.Abs(projectile.velocity.Y) + Math.Abs(projectile.velocity.X)) * 0.001f;
                }
                projectile.frameCounter++;
                if (projectile.frameCounter > 6)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                    if (projectile.frame > 4)
                    {
                        projectile.frame = 0;
                    }
                }
                if (projectile.velocity.Length() > 2f)
                {
                    projectile.velocity *= 0.98f;
                }
                for (int i = 0; i < 1000; i++)
                {
                    if (i != projectile.whoAmI)
                    {
                        Projectile Projectile = Main.projectile[i];
                        if (Projectile.active && Projectile.owner == projectile.owner && Projectile.type == projectile.type && projectile.timeLeft > Main.projectile[i].timeLeft && Main.projectile[i].timeLeft > 30)
                        {
                            Main.projectile[i].timeLeft = 30;
                        }
                    }
                }

                int[] array = new int[20];
                int num = 0;
                float range = 400f;
                bool flag = false;
                float num3 = 0f;
                float num4 = 0f;
                for (int i = 0; i < 200; i++)
                {
                    if (!Main.npc[i].CanBeChasedBy(this))
                    {
                        continue;
                    }
                    float num5 = Main.npc[i].position.X + (float)(Main.npc[i].width / 2);
                    float num6 = Main.npc[i].position.Y + (float)(Main.npc[i].height / 2);
                    float num7 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num5) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num6);
                    if (num7 < range && Collision.CanHit(projectile.Center, 1, 1, Main.npc[i].Center, 1, 1))
                    {
                        if (num < 20)
                        {
                            array[num] = i;
                            num++;
                            num3 = num5;
                            num4 = num6;
                        }
                        flag = true;
                    }
                }
                if (projectile.timeLeft < 30)
                {
                    flag = false;
                }
                if (flag)
                {
                    int num8 = Main.rand.Next(num);
                    num8 = array[num8];
                    num3 = Main.npc[num8].position.X + (float)(Main.npc[num8].width / 2);
                    num4 = Main.npc[num8].position.Y + (float)(Main.npc[num8].height / 2);
                    projectile.localAI[0] += 1f;
                    if (projectile.localAI[0] > 8f)
                    {
                        projectile.localAI[0] = 0f;
                        if (player.statMana >= (int)(8 * player.manaCost))
                        {
                            player.statMana -= (int)(8 * player.manaCost);
                            float num9 = 6f;
                            Vector2 vector = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                            vector += projectile.velocity * 4f;
                            float num10 = num3 - vector.X;
                            float num11 = num4 - vector.Y;
                            float num12 = (float)Math.Sqrt(num10 * num10 + num11 * num11);
                            float num13 = num12;
                            num12 = num9 / num12;
                            num10 *= num12;
                            num11 *= num12;
                            Projectile.NewProjectile(projectile.GetSource_FromThis(), vector.X, vector.Y, num10, num11, 255, projectile.damage, projectile.knockBack, projectile.owner);
                        }
                    }
                }
                return false;
            }            
            // Crimson Rod Change

            if (projectile.type == 244)
            {
                int PosX = (int)projectile.Center.X;
                int PosY = (int)(projectile.position.Y + projectile.height);
                projectile.frameCounter++;
                if (projectile.frameCounter > 8)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                    if ((projectile.frame > 2) || projectile.frame > 5)
                    {
                        projectile.frame = 0;
                    }
                }
                projectile.ai[1] += 1f;
                if (projectile.ai[1] >= 3600f)
                {
                    projectile.alpha += 5;
                    if (projectile.alpha > 255)
                    {
                        projectile.alpha = 255;
                        projectile.Kill();
                    }
                }
                projectile.ai[0] += 1f;
                float BloodRainDelay = 12f; // Fire rate. Vanilla value = 10f
                if (projectile.ai[0] > BloodRainDelay)
                {
                    projectile.ai[0] = 0f;
                    if (projectile.owner == Main.myPlayer && player.statMana >= 6)
                    {
                        player.statMana -= 3;
                        PosX += Main.rand.Next(-14, 15);
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), PosX, PosY, 0f, 5f, ProjectileID.BloodRain, projectile.damage, 0f, projectile.owner);
                    }
                }
                projectile.localAI[0] += 1f;
                if (!(projectile.localAI[0] >= 10f))
                {
                    return false;
                }
                projectile.localAI[0] = 0f;
                int CloudLimit = 0;
                int ExtraCloud = 0;
                float FoundCloudTimer = 0f;
                int Cloud = projectile.type;
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == projectile.owner && Main.projectile[i].type == Cloud && Main.projectile[i].ai[1] < 3600f)
                    {
                        CloudLimit++;
                        if (Main.projectile[i].ai[1] > FoundCloudTimer)
                        {
                            ExtraCloud = i;
                            FoundCloudTimer = Main.projectile[i].ai[1];
                        }
                    }
                }
                if (CloudLimit > 2 || projectile.timeLeft < 120) // only 2 clouds
                {
                    Main.projectile[ExtraCloud].netUpdate = true;
                    Main.projectile[ExtraCloud].ai[1] = 36000f; // the cloud will then disappear
                }
                return false;
            }
			if (projectile.type == 238) // nimbus cloud
            {
                float var = projectile.Center.X;
                if (projectile.ai[0] >= 8f)
                {
                    projectile.ai[0] = 0f;
                    if (projectile.owner == Main.myPlayer && player.statMana >= 4)
                    {
                        player.statMana -= 2;
                        var += Main.rand.Next(-14, 15);
                        Projectile.NewProjectile(projectile.GetSource_FromThis(), var, projectile.Center.Y, 0f, 5f, 239, projectile.damage, 0f, projectile.owner);
                    }
                    return false;
                }

                if (projectile.timeLeft < 120)
                    projectile.ai[1] = 36000f;

                projectile.ai[0] -= 0.5f;
            }   
            if (DrainManaPassively > 0 && (projectile.timeLeft % (projectile.extraUpdates + 1)) == 0)
            {
                manaDrain += (int)(DrainManaPassively * player.manaCost);
                while(manaDrain >= 60)
                {
                    manaDrain -= 60;
                    player.UseManaOverloadable(1);
                }
                if (player.statMana <= 0)
                {
                    projectile.Kill();
                }
            }    
            return true;
        }
        
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];           
            switch (projectile.type)
            {
                case ProjectileID.UnholyTridentFriendly:
                    projectile.ai[0] += 1f;
                    if (projectile.ai[0] >= 30.0)
                    {
                        if (projectile.ai[0] < 100.0)
                            projectile.velocity = Vector2.Multiply(projectile.velocity, 1.06f);
                        else
                            projectile.ai[0] = 200f;
                    }
                    return;
                case ProjectileID.BloodRain:
                    projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
                    return;
                case ProjectileID.SharpTears:
                    projectile.ai[0] -= 0.8f;
                    return;
                case ProjectileID.WeatherPainShot:
                    projectile.ai[1] -= 0.75f;
                    return;
            }                    
        }    
       
        public override bool PreKill(Projectile projectile, int timeLeft)
        { 
            switch (projectile.type)
            {                
                case ProjectileID.ToxicFlask:
                    {
                        for (int num332 = 0; num332 < 1000; num332++)
                        {
                            if (Main.projectile[num332].active && Main.projectile[num332].owner == projectile.owner && Main.projectile[num332].type == ProjectileType<ToxicCloud>())
                            {
                                Main.projectile[num332].ai[1] = 600f;
                            }
                        }
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item107 with { MaxInstances = 0 }, projectile.position);
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