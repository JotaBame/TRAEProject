using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs.Boss.Prime
{
    public class PrimeLaser : GlobalNPC
    {
        
        public override void SetDefaults(NPC npc)
        {
            if(npc.type == NPCID.PrimeLaser && GetInstance<TRAEConfig>().PrimeRework && !Main.zenithWorld)
            {

                npc.lifeMax = (int)(npc.lifeMax * ((float)PrimeStats.laserHealth / 6000));
            }
        }
        public override bool PreAI(NPC npc)
        {
            if(npc.type == NPCID.PrimeLaser && GetInstance<TRAEConfig>().PrimeRework && !Main.zenithWorld)
            {                npc.damage = 0;

                Prime_Laser_AI(npc);
                return false;
            }
            return base.PreAI(npc);
        }
        static void Prime_Laser_AI(NPC npc)
        {
            npc.spriteDirection = -(int)npc.ai[0];
            if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].aiStyle != 32 || !SkeletronPrime.KeepPhase1Arms(Main.npc[(int)npc.ai[1]])) 
            {
                npc.ai[2] += 10f;
                if (npc.ai[2] > 50f || Main.netMode != NetmodeID.Server) 
                {
                    npc.life = -1;
                    npc.HitEffect();
                    npc.active = false;
                }
            }

            if (npc.ai[2] == 0f || npc.ai[2] == 3f) 
            {
                if (Main.npc[(int)npc.ai[1]].ai[1] == 3f)
                    npc.EncourageDespawn(10);

                if (Main.npc[(int)npc.ai[1]].ai[1] != 0f) 
                {
                    npc.localAI[0] += 3f;
                    if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y - 100f) 
                    {
                        if (npc.velocity.Y > 0f)
                            npc.velocity.Y *= 0.96f;

                        npc.velocity.Y -= 0.07f;
                        if (npc.velocity.Y > 6f)
                            npc.velocity.Y = 6f;
                    }
                    else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 100f) 
                    {
                        if (npc.velocity.Y < 0f)
                            npc.velocity.Y *= 0.96f;

                        npc.velocity.Y += 0.07f;
                        if (npc.velocity.Y < -6f)
                            npc.velocity.Y = -6f;
                    }

                    if (npc.position.X + (float)(npc.width / 2) > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 120f * npc.ai[0]) 
                    {
                        if (npc.velocity.X > 0f)
                            npc.velocity.X *= 0.96f;

                        npc.velocity.X -= 0.1f;
                        if (npc.velocity.X > 8f)
                            npc.velocity.X = 8f;
                    }

                    if (npc.position.X + (float)(npc.width / 2) < Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 120f * npc.ai[0]) 
                    {
                        if (npc.velocity.X < 0f)
                            npc.velocity.X *= 0.96f;

                        npc.velocity.X += 0.1f;
                        if (npc.velocity.X < -8f)
                            npc.velocity.X = -8f;
                    }
                }
                else 
                {
                    npc.ai[3] += 1f;
                    if (npc.ai[3] >= 800f)
                    {
                        npc.ai[2] += 1f;
                        npc.ai[3] = 0f;
                        npc.netUpdate = true;
                    }

                    if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y - 100f) 
                    {
                        if (npc.velocity.Y > 0f)
                            npc.velocity.Y *= 0.96f;

                        npc.velocity.Y -= 0.1f;
                        if (npc.velocity.Y > 3f)
                            npc.velocity.Y = 3f;
                    }
                    else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 100f) 
                    {
                        if (npc.velocity.Y < 0f)
                            npc.velocity.Y *= 0.96f;

                        npc.velocity.Y += 0.1f;
                        if (npc.velocity.Y < -3f)
                            npc.velocity.Y = -3f;
                    }

                    if (npc.position.X + (float)(npc.width / 2) > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 180f * npc.ai[0]) 
                    {
                        if (npc.velocity.X > 0f)
                            npc.velocity.X *= 0.96f;

                        npc.velocity.X -= 0.14f;
                        if (npc.velocity.X > 8f)
                            npc.velocity.X = 8f;
                    }

                    if (npc.position.X + (float)(npc.width / 2) < Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 180f * npc.ai[0]) 
                    {
                        if (npc.velocity.X < 0f)
                            npc.velocity.X *= 0.96f;

                        npc.velocity.X += 0.14f;
                        if (npc.velocity.X < -8f)
                            npc.velocity.X = -8f;
                    }
                }

                npc.TargetClosest();
                float num529 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - npc.Center.X;
                float num530 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - npc.Center.Y;
                float num531 = (float)Math.Sqrt(num529 * num529 + num530 * num530);
                npc.rotation = (float)Math.Atan2(num530, num529) - 1.57f;
                if (Main.netMode != NetmodeID.MultiplayerClient) 
                {
                    npc.localAI[0] += 1f;
                    if (npc.localAI[0] > 200f) 
                    {
                        npc.localAI[0] = 0f;
                        int damage = 25;
                        float dir = npc.rotation + MathF.PI / 2f;
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + TRAEMethods.PolarVector(12f, dir), TRAEMethods.PolarVector(8f, dir), ProjectileID.DeathLaser, damage, 0f, Main.myPlayer);
                        RetPhase3.DeathLaserShootDust(TRAEMethods.PolarVector(8f, dir), npc.Center + TRAEMethods.PolarVector(12f, dir));
                    }
                }
            }
            else 
            {
                if (npc.ai[2] != 1f)
                    return;

                npc.ai[3] += 1f;
                if (npc.ai[3] >= 200f) 
                {
                    npc.localAI[0] = 0f;
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                    npc.netUpdate = true;
                }

                Vector2 vector60 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float num536 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - 350f - vector60.X;
                float num537 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - 20f - vector60.Y;
                float num538 = (float)Math.Sqrt(num536 * num536 + num537 * num537);
                num538 = 7f / num538;
                num536 *= num538;
                num537 *= num538;
                if (npc.velocity.X > num536) 
                {
                    if (npc.velocity.X > 0f)
                        npc.velocity.X *= 0.9f;

                    npc.velocity.X -= 0.1f;
                }

                if (npc.velocity.X < num536) 
                {
                    if (npc.velocity.X < 0f)
                        npc.velocity.X *= 0.9f;

                    npc.velocity.X += 0.1f;
                }

                if (npc.velocity.Y > num537) 
                {
                    if (npc.velocity.Y > 0f)
                        npc.velocity.Y *= 0.9f;

                    npc.velocity.Y -= 0.03f;
                }

                if (npc.velocity.Y < num537) 
                {
                    if (npc.velocity.Y < 0f)
                        npc.velocity.Y *= 0.9f;

                    npc.velocity.Y += 0.03f;
                }

                npc.TargetClosest();
                vector60 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                num536 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector60.X;
                num537 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector60.Y;
                npc.rotation = (float)Math.Atan2(num537, num536) - 1.57f;
                if (Main.netMode == NetmodeID.MultiplayerClient) 
                {
                    npc.localAI[0] += 1f;
                    if (npc.localAI[0] > 80f) 
                    {
                        npc.localAI[0] = 0f;
                        int damage = 25;
                        float dir = npc.rotation + MathF.PI / 2f;
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + TRAEMethods.PolarVector(12f, dir), TRAEMethods.PolarVector(8f, dir), ProjectileID.DeathLaser, damage, 0f, Main.myPlayer);
                        RetPhase3.DeathLaserShootDust(TRAEMethods.PolarVector(8f, dir), npc.Center + TRAEMethods.PolarVector(12f, dir));
                    }
                }
            }
        }
    }
}