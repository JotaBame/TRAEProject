using System;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs.Boss.Prime
{
    public class PrimeCannon : GlobalNPC
    {
        public override void SetDefaults(NPC npc)
        {
            if(npc.type == NPCID.PrimeCannon && GetInstance<TRAEConfig>().PrimeRework && !Main.zenithWorld)
            {
                npc.lifeMax = (int)(npc.lifeMax * ((float)PrimeStats.cannonHealth / 7000));
            }
        }
        public override void AI(NPC npc)
        {
            if(npc.type == NPCID.PrimeCannon && npc.ai[2] == 0 && GetInstance<TRAEConfig>().PrimeRework && !Main.zenithWorld) 
            {                npc.damage = 0;

                npc.localAI[0] += 2f;
                if (Main.npc[(int)npc.ai[1]].ai[1] != 0f) 
                {
                    npc.localAI[0] += 2f;
                }
                if(!SkeletronPrime.KeepPhase1Arms(Main.npc[(int)npc.ai[1]]))
                {
                    npc.ai[2] += 10f;
                    if (npc.ai[2] > 50f || Main.netMode != NetmodeID.Server) 
                    {
                        npc.life = -1;
                        npc.HitEffect();
                        npc.active = false;
                        
                    }
                }
            }
        }
    }
    public class PrimeProjectiles : GlobalProjectile
    {
        public override void SetDefaults(Projectile entity)
        {
            if (entity.type == ProjectileID.BombSkeletronPrime && GetInstance<TRAEConfig>().PrimeRework && !Main.zenithWorld)
            {
                entity.timeLeft = 180;
            }
        }
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            if (projectile.type == ProjectileID.BombSkeletronPrime && GetInstance<TRAEConfig>().PrimeRework && !Main.zenithWorld)
            {
               
            }
        }
        public override bool CanHitPlayer(Projectile projectile, Player target)
        {
            
            return base.CanHitPlayer(projectile, target); 
        }
        /*
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if(projectile.type == ProjectileID.BombSkeletronPrime)
            {
                Main.NewText("col");
                for(int i = 0; i < Main.player.Length; i++)
                {
                    if(projectile.tileCollide && Main.player[i].active && Main.player[i].Bottom.Y > projectile.Bottom.Y && MathF.Abs(Main.player[i].Center.X - projectile.Center.X) < 1000f)
                    {
                        Main.NewText("fall through");
                        return false;
                    }
                }   
            }
            return base.OnTileCollide(projectile, oldVelocity);
        }
        */
        void AI_016(Projectile projectile) 
        {
            projectile.tileCollide = true;
           projectile.DamageType = DamageClass.Default;

            for (int i = 0; i < Main.player.Length; i++)
            {
                if(Main.player[i].active && Main.player[i].getRect().Intersects(projectile.getRect()))
                {
                    projectile.Kill();
                }
                if(projectile.tileCollide && Main.player[i].active && Main.player[i].Bottom.Y > projectile.Bottom.Y && MathF.Abs(Main.player[i].Center.X - projectile.Center.X) < 1000f)
                {
                    projectile.tileCollide = false;
                }
            }
			if (projectile.tileCollide) 
            {
				int num = (int)(projectile.Center.X / 16f);
				int num2 = (int)(projectile.Center.Y / 16f);
				if (WorldGen.InWorld(num, num2)) 
                {
					Tile tile = Main.tile[num, num2];
					if (tile != null && tile.HasTile && (TileID.Sets.Platforms[tile.TileType] || tile.TileType == 380)) 
                    {
						projectile.Kill();
						return;
					}
				}
			}
            
            if (projectile.velocity.Y > 10f)
                projectile.velocity.Y = 10f;

            if (projectile.localAI[0] == 0f) 
            {
                projectile.localAI[0] = 1f;
                SoundEngine.PlaySound(SoundID.Item10, projectile.position);
            }

            projectile.frameCounter++;
            if (projectile.frameCounter > 3) 
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }

            if (projectile.frame > 1)
                projectile.frame = 0;

            if (projectile.velocity.Y == 0f) 
            {
                projectile.position.X += projectile.width / 2;
                projectile.position.Y += projectile.height / 2;
                projectile.width = 128;
                projectile.height = 128;
                projectile.position.X -= projectile.width / 2;
                projectile.position.Y -= projectile.height / 2;
                projectile.damage = 40;
                projectile.knockBack = 8f;
                projectile.timeLeft = 3;
                projectile.netUpdate = true;
            }
			
			if (projectile.owner == Main.myPlayer && projectile.timeLeft <= 3) 
            {
				projectile.tileCollide = false;
				projectile.ai[1] = 0f;
				projectile.alpha = 255;
			}
			else 
            {
                projectile.damage = 0;
				
					

				if (Main.rand.NextBool(2)) 
                {
					int num27 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100);
					Main.dust[num27].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
					Main.dust[num27].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
					Main.dust[num27].noGravity = true;
					Main.dust[num27].position = projectile.Center + new Vector2(0f, -projectile.height / 2).RotatedBy(projectile.rotation) * 1.1f;
					int num28 = 6;
					

					Dust dust8 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, num28, 0f, 0f, 100);
					dust8.scale = 1f + (float)Main.rand.Next(5) * 0.1f;
					dust8.noGravity = true;
					dust8.position = projectile.Center + new Vector2(0f, -projectile.height / 2 - 6).RotatedBy(projectile.rotation) * 1.1f;
				}
			}

			projectile.ai[0] += 1f;
            if (projectile.ai[0] > 5f) 
            {
				projectile.ai[0] = 10f;
				if (projectile.velocity.Y == 0f && projectile.velocity.X != 0f) 
                {
					projectile.velocity.X *= 0.97f;

					if ((double)projectile.velocity.X > -0.01 && (double)projectile.velocity.X < 0.01) 
                    {
						projectile.velocity.X = 0f;
						projectile.netUpdate = true;
					}
				}
				projectile.velocity.Y += 0.2f;
			}
			projectile.rotation += projectile.velocity.X * 0.1f;
		}
        
        public override bool PreAI(Projectile projectile)
        {
            if(projectile.type == ProjectileID.BombSkeletronPrime && GetInstance<TRAEConfig>().PrimeRework && !Main.zenithWorld)
            {
                AI_016(projectile);
                return false;
            }
            return base.PreAI(projectile);
        }
    }
}