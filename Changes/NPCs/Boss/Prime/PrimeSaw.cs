using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs.Boss.Prime
{
    public class PrimeSaw : GlobalNPC
    {
        public override void SetDefaults(NPC npc)
        {
            if(npc.type == NPCID.PrimeSaw && GetInstance<BossConfig>().PrimeRework && !Main.zenithWorld)
            {
                npc.lifeMax = (int)(npc.lifeMax * ((float)PrimeStats.sawHealth / 9000));
            }
        }
        public override bool PreAI(NPC npc)
        {
            if(npc.type == NPCID.PrimeSaw && GetInstance<BossConfig>().PrimeRework && !Main.zenithWorld)
            {
                Prime_Saw_AI(npc);
                return false;
            }
            return base.PreAI(npc);
        }
        static void Prime_Saw_AI(NPC npc)
        {
            float distX = Main.npc[(int)npc.ai[1]].Center.X - 200f * npc.ai[0] - npc.Center.X;
            float distY = Main.npc[(int)npc.ai[1]].Center.Y + 230f - npc.Center.Y;
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

            if (npc.ai[2] == 99f) 
            {
                //Main.NewText("Returning");
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

                if (npc.position.X + (float)(npc.width / 2) > Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2)) 
                {
                    if (npc.velocity.X > 0f)
                        npc.velocity.X *= 0.96f;

                    npc.velocity.X -= 0.5f;
                    if (npc.velocity.X > 12f)
                        npc.velocity.X = 12f;
                }

                if (npc.position.X + (float)(npc.width / 2) < Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2)) 
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
                //Main.NewText("Idle");
                if (Main.npc[(int)npc.ai[1]].ai[1] == 3f)
                    npc.EncourageDespawn(10);

                if (Main.npc[(int)npc.ai[1]].ai[1] != 0f) 
                {
                    //primeSpinning
                    //slowly move to player
                    AproachPlayer(npc);
                }
                else 
                {
                    //prime not spinning
                    //be passive
                    npc.ai[3] += 1f;
                    if (npc.ai[3] >= 300f) 
                    {
                        npc.ai[2] += 1f;
                        npc.ai[3] = 0f;
                        npc.netUpdate = true;
                    }

                    if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y + 320f) 
                    {
                        if (npc.velocity.Y > 0f)
                            npc.velocity.Y *= 0.96f;

                        npc.velocity.Y -= 0.04f;
                        if (npc.velocity.Y > 3f)
                            npc.velocity.Y = 3f;
                    }
                    else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y + 260f) 
                    {
                        if (npc.velocity.Y < 0f)
                            npc.velocity.Y *= 0.96f;

                        npc.velocity.Y += 0.04f;
                        if (npc.velocity.Y < -3f)
                            npc.velocity.Y = -3f;
                    }

                    if (npc.Center.X > Main.npc[(int)npc.ai[1]].Center.X) 
                    {
                        if (npc.velocity.X > 0f)
                            npc.velocity.X *= 0.96f;

                        npc.velocity.X -= 0.3f;
                        if (npc.velocity.X > 12f)
                            npc.velocity.X = 12f;
                    }

                    if (npc.Center.X < Main.npc[(int)npc.ai[1]].Center.X - 250f) 
                    {
                        if (npc.velocity.X < 0f)
                            npc.velocity.X *= 0.96f;

                        npc.velocity.X += 0.3f;
                        if (npc.velocity.X < -12f)
                            npc.velocity.X = -12f;
                    }
                }

                RotateArm(npc);
            }
            else if (npc.ai[2] == 1f) 
            {
                //Main.NewText("Preparing Swipe");
                //lift up

                npc.TargetClosest();
                npc.velocity.X *= 0.95f;
                float accY = 0.4f;
                npc.velocity.Y += accY * MathF.Sign(Main.player[npc.target].Center.Y - npc.Center.Y);

                RotateArm(npc);

                if (MathF.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) < 20) 
                {
                    //swipe at player
                    npc.velocity.Y = 0;
                    npc.velocity.X = 22f * MathF.Sign(Main.player[npc.target].Center.X - npc.Center.X);
                    npc.ai[2] = 2f * MathF.Sign(Main.player[npc.target].Center.X - npc.Center.X);
                    npc.netUpdate = true;
                }
            }
            else if (npc.ai[2] == 2f) 
            {
                //Main.NewText("Swiping Right");
                //during swipe logic
                if (npc.Center.X > Main.player[npc.target].Center.X + 200f)
                    npc.ai[2] = 3f;
            }
            else if (npc.ai[2] == -2f)
            {
                //Main.NewText("Swiping Left");
                //during swipe logic
                if (npc.Center.X < Main.player[npc.target].Center.X - 200f)
                    npc.ai[2] = 3f;
            } 
            else if (npc.ai[2] == 4f) 
            {
                //Main.NewText("Approaches");
                AproachPlayer(npc);

                RotateArm(npc);
            }
            else if (npc.ai[2] == 5f && ((npc.velocity.X > 0f && npc.position.X + (float)(npc.width / 2) > Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2)) || (npc.velocity.X < 0f && npc.position.X + (float)(npc.width / 2) < Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2)))) 
            {
                npc.ai[2] = 0f;
            }
        }
        static void RotateArm(NPC npc)
        {
            float num494 = Main.npc[(int)npc.ai[1]].Center.X - 200f * npc.ai[0] - npc.Center.X;
            float num495 = Main.npc[(int)npc.ai[1]].position.Y + 230f - npc.Center.Y;
            npc.rotation = (float)Math.Atan2(num495, num494) + 1.57f;
        }
        static void AproachPlayer(NPC npc)
        {
            float speed = PrimeStats.sawApproachSpeed; //vanilla value 7f
            float acc = PrimeStats.sawApproachAcc; //vanilla value 0.05f
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
                float multiplier = speed / dist2;
                float toSpeedX = distX2 * multiplier;
                float toSpeedY = distY2 * multiplier;
                npc.rotation = (float)Math.Atan2(toSpeedY, toSpeedX) - 1.57f;
                if (npc.velocity.X > toSpeedX) 
                {
                    if (npc.velocity.X > 0f)
                        npc.velocity.X *= 0.97f;

                    npc.velocity.X -= acc;
                }

                if (npc.velocity.X < toSpeedX) 
                {
                    if (npc.velocity.X < 0f)
                        npc.velocity.X *= 0.97f;

                    npc.velocity.X += acc;
                }

                if (npc.velocity.Y > toSpeedY) 
                {
                    if (npc.velocity.Y > 0f)
                        npc.velocity.Y *= 0.97f;

                    npc.velocity.Y -= acc;
                }

                if (npc.velocity.Y < toSpeedY) 
                {
                    if (npc.velocity.Y < 0f)
                        npc.velocity.Y *= 0.97f;

                    npc.velocity.Y += acc;
                }
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
}