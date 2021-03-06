using TRAEProject.NewContent.Projectiles;
using TRAEProject.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Projectiles
{
    public class MagicProjectile : GlobalProjectile
    {
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
                    projectile.penetrate = 15; // up from 12
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
                    projectile.timeLeft = 882; // oddly specific but this is apparently equal to 10 seconds for this weapon. Reason for this is in the code probably
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
                    projectile.GetGlobalProjectile<ProjectileStats>().DamageFallon = 1.42f;
                    break;              
                case ProjectileID.WaterStream:
                    projectile.penetrate = 1;
                    break;
                case ProjectileID.RainbowFront:
                case ProjectileID.RainbowBack:
                    projectile.usesIDStaticNPCImmunity = true;
                    projectile.idStaticNPCHitCooldown = 10;
                    break;
			    case 244:
				  case 238:
					projectile.timeLeft = 900;
					break;			
				case ProjectileID.BloodRain:
                case ProjectileID.RainFriendly:
                    projectile.penetrate = 1;
                    break;
                case ProjectileID.ClingerStaff:
                    projectile.penetrate = 40;
                    break;
                case ProjectileID.Blizzard:
                    projectile.timeLeft = 150;
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
                    projectile.extraUpdates = 1;
                    break;            
                case ProjectileID.GoldenShowerFriendly:
                    projectile.penetrate = 2; // down from 5

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
                case ProjectileID.BoulderStaffOfEarth:
                    projectile.penetrate = 4;
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = -1;
                    break;
            }
        }

        public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox)
        {
            switch (projectile.type)
            {
                case ProjectileID.Blizzard:
                    hitbox.Width = 50;
                    hitbox.Height = 50;
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

        public override bool PreAI(Projectile projectile)
        {
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
                float BloodRainDelay = 13f; // Fire rate. Vanilla value = 10f
                if (projectile.ai[0] > BloodRainDelay)
                {
                    projectile.ai[0] = 0f;
                    if (projectile.owner == Main.myPlayer)
                    {
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
				{
				if (projectile.timeLeft < 120)
				projectile.ai[1] = 36000f;
				}
				projectile.ai[0] -= 0.5f;
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