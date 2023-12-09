
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Boss.Prime
{
    public class PrimeVice : GlobalNPC
    {
        public override bool PreAI(NPC npc)
        {
            if(npc.type == NPCID.PrimeVice)
            {
                Prime_Vice_AI(npc);
                return false;
            }
            return base.PreAI(npc);
        }
        static void Vice_Rotate(NPC npc, float xOffset = 200f)
        {
            Vector2 vector56 = new Vector2(npc.Center.X, npc.Center.Y );
            float num512 = Main.npc[(int)npc.ai[1]].Center.X - xOffset * npc.ai[0] - vector56.X;
            float num513 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector56.Y;
            npc.rotation = (float)Math.Atan2(num513, num512) + 1.57f;
        }
        static void Prime_Vice_AI(NPC npc)
        {
            npc.spriteDirection = -(int)npc.ai[0];
            Vector2 vector52 = new Vector2(npc.Center.X, npc.Center.Y);
            float distX = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0] - vector52.X;
            float distY = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector52.Y;
            float dist = (float)Math.Sqrt(distX * distX + distY * distY);
            if (npc.ai[2] != 99f) 
            {
                if (dist > 800f)
                    npc.ai[2] = 99f;
            }
            else if (dist < 400f) 
            {
                npc.ai[2] = 0f;
            }

            if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].aiStyle != 32 || (npc.ai[0] == -1 && !SkeletronPrime.KeepPhase1Arms(Main.npc[(int)npc.ai[1]])) || (npc.ai[0] == 1 && !SkeletronPrime.KeepPhase2Arms(Main.npc[(int)npc.ai[1]]))) 
            {
                npc.ai[2] += 10f;
                if (npc.ai[2] > 50f || Main.netMode != NetmodeID.Server) 
                {
                    npc.life = -1;
                    npc.HitEffect();
                    npc.active = false;
                }
            }

            if(npc.ai[2] == 99f) 
            {
                //return mode
                if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y) 
                {
                    if (npc.velocity.Y > 0f)
                        npc.velocity.Y *= 0.96f;

                    npc.velocity.Y -= 0.1f;
                    if (npc.velocity.Y > 8f)
                        npc.velocity.Y = 8f;
                }
                else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y) 
                {
                    if (npc.velocity.Y < 0f)
                        npc.velocity.Y *= 0.96f;

                    npc.velocity.Y += 0.1f;
                    if (npc.velocity.Y < -8f)
                        npc.velocity.Y = -8f;
                }

                if (npc.Center.X > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2)) 
                {
                    if (npc.velocity.X > 0f)
                        npc.velocity.X *= 0.96f;

                    npc.velocity.X -= 0.5f;
                    if (npc.velocity.X > 12f)
                        npc.velocity.X = 12f;
                }

                if (npc.Center.X < Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2)) 
                {
                    if (npc.velocity.X < 0f)
                        npc.velocity.X *= 0.96f;

                    npc.velocity.X += 0.5f;
                    if (npc.velocity.X < -12f)
                        npc.velocity.X = -12f;
                }
            }
            else if (npc.ai[2] == 0f || npc.ai[2] == 3f) 
            {
                if (Main.npc[(int)npc.ai[1]].ai[1] == 3f)
                    npc.EncourageDespawn(10);
                if (Main.npc[(int)npc.ai[1]].ai[1] != 0f) 
                {
                    //prime spinning
                    npc.TargetClosest();
                    npc.TargetClosest();
                    if (Main.player[npc.target].dead)
                    {
                        npc.velocity.Y += 0.1f;
                        if (npc.velocity.Y > 16f)
                            npc.velocity.Y = 16f;
                    }
                    else
                    {
                        float distX2 = Main.player[npc.target].Center.X - npc.Center.X;
                        float distY2 = Main.player[npc.target].Center.Y - npc.Center.Y;
                        float dist2 = (float)Math.Sqrt(distX2 * distX2 + distY2 * distY2);
                        float multiplier = 12f / dist2;
                        float speedX = distX2 * multiplier;
                        float SpeedY = distY2 * multiplier;
                        npc.rotation = (float)Math.Atan2(SpeedY, speedX) - 1.57f;
                        if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < 2f) 
                        {
                            npc.velocity.X = speedX;
                            npc.velocity.Y = SpeedY;
                            npc.netUpdate = true;
                        }
                        else 
                        {
                            npc.velocity *= 0.97f;
                        }

                        npc.ai[3] += 1f;
                        if (npc.ai[3] >= 600f) 
                        {
                            npc.ai[2] = 0f;
                            npc.ai[3] = 0f;
                            npc.netUpdate = true;
                        }
                    }
                }
                else 
                {
                    npc.ai[3] += 1f;
                    if (npc.ai[3] >= 600f) 
                    {
                        npc.ai[2] += 1f;
                        npc.ai[3] = 0f;
                        npc.netUpdate = true;
                    }

                    if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y + 300f) 
                    {
                        if (npc.velocity.Y > 0f)
                            npc.velocity.Y *= 0.96f;

                        npc.velocity.Y -= 0.1f;
                        if (npc.velocity.Y > 3f)
                            npc.velocity.Y = 3f;
                    }
                    else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y + 230f) 
                    {
                        if (npc.velocity.Y < 0f)
                            npc.velocity.Y *= 0.96f;

                        npc.velocity.Y += 0.1f;
                        if (npc.velocity.Y < -3f)
                            npc.velocity.Y = -3f;
                    }

                    if ((npc.ai[0] == -1 && npc.Center.X > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) + 250f) || (npc.ai[0] == 1 && npc.Center.X > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 250f)) 
                    {
                        if (npc.velocity.X > 0f)
                            npc.velocity.X *= 0.94f;

                        npc.velocity.X -= 0.3f;
                        if (npc.velocity.X > 9f)
                            npc.velocity.X = 9f;
                    }

                    if ((npc.ai[0] == -1 && npc.Center.X < Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2)) || (npc.ai[0] == 1 && npc.Center.X > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2)))
                    {
                        if (npc.velocity.X < 0f)
                            npc.velocity.X *= 0.94f;

                        npc.velocity.X += 0.2f;
                        if (npc.velocity.X < -8f)
                            npc.velocity.X = -8f;
                    }
                }
                Vice_Rotate(npc, 200f);
            }
            else if (npc.ai[2] == 1f) 
            {
                //vice rises
                if (npc.velocity.Y > 0f)
                    npc.velocity.Y *= 0.9f;

                Vice_Rotate(npc, 280f);
                npc.velocity.X = (npc.velocity.X * 5f + Main.npc[(int)npc.ai[1]].velocity.X) / 6f;
                npc.velocity.X += 0.5f;
                npc.velocity.Y -= 0.5f;
                if (npc.velocity.Y < -9f)
                    npc.velocity.Y = -9f;

                if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 280f) 
                {
                    npc.TargetClosest();
                    npc.ai[2] = 2f;
                    float num509 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - npc.Center.X;
                    float num510 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - npc.Center.Y;
                    float num511 = (float)Math.Sqrt(num509 * num509 + num510 * num510);
                    num511 = 20f / num511;
                    npc.velocity.X = num509 * num511;
                    npc.velocity.Y = num510 * num511;
                    npc.netUpdate = true;
                }
            }
            else if (npc.ai[2] == 2f) 
            {
                if (npc.position.Y > Main.player[npc.target].position.Y || npc.velocity.Y < 0f) {
                    if (npc.ai[3] >= 4f) 
                    {
                        npc.ai[2] = 3f;
                        npc.ai[3] = 0f;
                    }
                    else 
                    {
                        npc.ai[2] = 1f;
                        npc.ai[3] += 1f;
                    }
                }
            }
            else if (npc.ai[2] == 4f) 
            {
                
                Vice_Rotate(npc, 200f);
                npc.velocity.Y = (npc.velocity.Y * 5f + Main.npc[(int)npc.ai[1]].velocity.Y) / 6f;
                npc.velocity.X += 0.5f;
                if (npc.velocity.X > 12f)
                    npc.velocity.X = 12f;

                if (npc.Center.X < Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - 500f || npc.Center.X > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) + 500f) {
                    npc.TargetClosest();
                    npc.ai[2] = 5f;
                    Vector2 vector56 = new Vector2(npc.Center.X, npc.Center.Y );
                    float num512 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector56.X;
                    float num513 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector56.Y;
                    float num514 = (float)Math.Sqrt(num512 * num512 + num513 * num513);
                    num514 = 17f / num514;
                    npc.velocity.X = num512 * num514;
                    npc.velocity.Y = num513 * num514;
                    npc.netUpdate = true;
                }
            }
            else if (npc.ai[2] == 5f && npc.Center.X < Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - 100f) 
            {
                if (npc.ai[3] >= 4f) 
                {
                    npc.ai[2] = 0f;
                    npc.ai[3] = 0f;
                }
                else 
                {
                    npc.ai[2] = 4f;
                    npc.ai[3] += 1f;
                }
            }
        }
    }
}