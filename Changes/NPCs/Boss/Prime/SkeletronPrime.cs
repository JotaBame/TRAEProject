
using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.NPCs.Boss.Prime
{
    public class PrimeProjectiles : GlobalProjectile
    {
        public override bool CanHitPlayer(Projectile projectile, Player target)
        {
            
            return base.CanHitPlayer(projectile, target);
        }
        public override void AI(Projectile projectile)
        {
            if(projectile.type == ProjectileID.BombSkeletronPrime)
            {
                Rectangle rect = projectile.getRect();
                for(int i = 0; i < Main.player.Length; i++)
                {
                    if(Main.player[i].getRect().Intersects(rect))
                    {
                        projectile.Kill();
                        break;
                    }
                }
            }
        }
    }
    public class SkeletronPrime : GlobalNPC
    {
        const int armSpawnTime = 60; 
        public static bool Phase0(NPC npc)
        {
            return npc.ai[0] == 0;
        }
        public static bool Phase1(NPC npc)
        {
            return npc.ai[0] == armSpawnTime;
        }
        public static bool Phase2(NPC npc)
        {
            return npc.ai[0] == armSpawnTime * 2;
        }
        public static bool Phase3(NPC npc)
        {
            return npc.ai[0] == armSpawnTime * 3;
        }
        public static bool SummoningPhase(NPC npc)
        {
            return npc.ai[0] % armSpawnTime != 0;
        }
        public static bool KeepPhase1Arms(NPC npc)
        {
            return Phase1(npc) || npc.ai[0] < armSpawnTime || npc.ai[0] > armSpawnTime * 2;
        }

        public static bool KeepPhase2Arms(NPC npc)
        {
            return Phase2(npc) || npc.ai[0] < armSpawnTime * 2;
        }
        public override void FindFrame(NPC npc, int frameHeight)
        {
            if(npc.type == NPCID.SkeletronPrime)
            {
                npc.frame.Width = 140;
                npc.frame.X = 0;
                if(SummoningPhase(npc))
                {
                    npc.frame.Y = frameHeight * 1;
                    int xShift = Main.rand.Next(-10, 11);
                    int yShift = Main.rand.Next(-10, 11);
                    npc.frame.Y += yShift;
                    if(xShift < 0)
                    {
                        npc.frame.Width += xShift;
                    }
                    else
                    {
                        npc.frame.X += xShift;
                        npc.frame.Width -= xShift;
                    }
                }
            }
            base.FindFrame(npc, frameHeight);
        }
        public override bool PreAI(NPC npc)
        {
            if(npc.type == NPCID.SkeletronPrime)
            {
                Prime_AI(npc);
                return false;
            }
            return base.PreAI(npc);
        }
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (npc.type == NPCID.SkeletronPrime)
            {
                npc.TargetClosest();

                int npcIndex = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeVice, npc.whoAmI);
                Main.npc[npcIndex].ai[0] = -1f;
                Main.npc[npcIndex].ai[1] = npc.whoAmI;
                Main.npc[npcIndex].target = npc.target;
                Main.npc[npcIndex].netUpdate = true;
            }
        }
        static void Prime_AI(NPC npc)
        {
            npc.damage = npc.defDamage;
            npc.defense = npc.defDefense;
            float lifeRatio = (float)npc.life / npc.lifeMax;
            if(Phase0(npc) && lifeRatio < 0.96f)
            {
                npc.ai[0]++;
                SoundEngine.PlaySound(SoundID.Roar, npc.Center);
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.TargetClosest();
             
                    int npcIndex = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeSaw, npc.whoAmI);
                    Main.npc[npcIndex].ai[0] = 1f;
                    Main.npc[npcIndex].ai[1] = npc.whoAmI;
                    Main.npc[npcIndex].target = npc.target;
                    Main.npc[npcIndex].netUpdate = true;
                    npcIndex = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeCannon, npc.whoAmI);
                    Main.npc[npcIndex].ai[0] = -1f;
                    Main.npc[npcIndex].ai[1] = npc.whoAmI;
                    Main.npc[npcIndex].target = npc.target;
                    Main.npc[npcIndex].ai[3] = 150f;
                    Main.npc[npcIndex].netUpdate = true;
                    npcIndex = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeLaser, npc.whoAmI);
                    Main.npc[npcIndex].ai[0] = 1f;
                    Main.npc[npcIndex].ai[1] = npc.whoAmI;
                    Main.npc[npcIndex].target = npc.target;
                    Main.npc[npcIndex].netUpdate = true;
                    Main.npc[npcIndex].ai[3] = 150f;
                }
            }
            if(Phase1(npc) && ((lifeRatio < 0.67f && (Main.expertMode || !PrimeStats.lockPhase3ToExpert)) || lifeRatio < 0.5f))
            {
                npc.ai[0]++;
                SoundEngine.PlaySound(SoundID.Roar, npc.Center);
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.TargetClosest();
                    int npcIndex = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PrimeRail>(), npc.whoAmI);
                    Main.npc[npcIndex].ai[0] = 0;
                    Main.npc[npcIndex].ai[1] = npc.whoAmI;
                    Main.npc[npcIndex].target = npc.target;
                    Main.npc[npcIndex].netUpdate = true;
                    npcIndex = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PrimeMace>(), npc.whoAmI);
                    Main.npc[npcIndex].ai[1] = npc.whoAmI;
                    Main.npc[npcIndex].target = npc.target;
                    Main.npc[npcIndex].netUpdate = true;
                    npcIndex = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeVice, npc.whoAmI);
                    Main.npc[npcIndex].ai[0] = 1f;
                    Main.npc[npcIndex].ai[1] = npc.whoAmI;
                    Main.npc[npcIndex].target = npc.target;
                    Main.npc[npcIndex].netUpdate = true;
                    npcIndex = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PrimeLauncher>(), npc.whoAmI);
                    Main.npc[npcIndex].ai[0] = 0;
                    Main.npc[npcIndex].ai[1] = npc.whoAmI;
                    Main.npc[npcIndex].target = npc.target;
                    Main.npc[npcIndex].netUpdate = true;
                }
            }
            if(Phase2(npc) && lifeRatio < 0.22f && (Main.expertMode || !PrimeStats.lockPhase3ToExpert))
            {
                npc.ai[0]++;
                SoundEngine.PlaySound(SoundID.ForceRoarPitched, npc.Center);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.TargetClosest();
                    int npcIndex = 0;

                  npcIndex = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeSaw, npc.whoAmI);
                    Main.npc[npcIndex].ai[0] = 1f;
                    Main.npc[npcIndex].ai[1] = npc.whoAmI;
                    Main.npc[npcIndex].target = npc.target;
                    Main.npc[npcIndex].netUpdate = true;
                    npcIndex = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeCannon, npc.whoAmI);
                    Main.npc[npcIndex].ai[0] = -1f;
                    Main.npc[npcIndex].ai[1] = npc.whoAmI;
                    Main.npc[npcIndex].target = npc.target;
                    Main.npc[npcIndex].ai[3] = 150f;
                    Main.npc[npcIndex].netUpdate = true;
                    npcIndex = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeLaser, npc.whoAmI);
                    Main.npc[npcIndex].ai[0] = 1f;
                    Main.npc[npcIndex].ai[1] = npc.whoAmI;
                    Main.npc[npcIndex].target = npc.target;
                    Main.npc[npcIndex].netUpdate = true;
                    Main.npc[npcIndex].ai[3] = 150f;
                    npcIndex = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PrimeRail>(), npc.whoAmI);
                    Main.npc[npcIndex].ai[0] = 1;
                    Main.npc[npcIndex].ai[1] = npc.whoAmI;
                    Main.npc[npcIndex].target = npc.target;
                    Main.npc[npcIndex].netUpdate = true;
                    npcIndex = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PrimeMace>(), npc.whoAmI);
                    Main.npc[npcIndex].ai[1] = npc.whoAmI * -1f;
                    Main.npc[npcIndex].target = npc.target;
                    Main.npc[npcIndex].netUpdate = true;
                    npcIndex = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PrimeLauncher>(), npc.whoAmI);
                    Main.npc[npcIndex].ai[0] = 1;
                    Main.npc[npcIndex].ai[1] = npc.whoAmI;
                    Main.npc[npcIndex].target = npc.target;
                    Main.npc[npcIndex].netUpdate = true;
                }
            }
            if(SummoningPhase(npc))
            {
                npc.ai[0]++;
                npc.velocity *= 0.99f;
                npc.rotation = npc.velocity.X / 15f;
                npc.ai[2] = 0;
                npc.ai[1] = 0;
                return;
            }

            if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 6000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 6000f) 
            {
                npc.TargetClosest();
                if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 6000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 6000f)
                    npc.ai[1] = 3f;
            }

            if (Main.dayTime && npc.ai[1] != 3f && npc.ai[1] != 2f) 
            {
                npc.ai[1] = 2f;
                SoundEngine.PlaySound(SoundID.Roar, npc.Center);
            }

            if (npc.ai[1] == 0f) 
            {
                //hover and let arms do work
                npc.ai[2] += 1f;
                if(Phase0(npc))
                {
                    npc.ai[2] = 0;
                }
                if (npc.ai[2] >= 600f) 
                {
                    npc.ai[2] = 0f;
                    npc.ai[1] = 1f;
                    npc.TargetClosest();
                    npc.netUpdate = true;
                }

                npc.rotation = npc.velocity.X / 15f;
                float verticalAcc = 0.1f;
                float maxVertSpeed = 2f;
                float horiAcc = 0.1f;
                float maxHoriSpeed = 8f;
                if (Main.expertMode) 
                {
                    verticalAcc = 0.03f;
                    maxVertSpeed = 4f;
                    horiAcc = 0.07f;
                    maxHoriSpeed = 9.5f;
                }

                if (npc.Center.Y > Main.player[npc.target].Center.Y - 200f) 
                {
                    if (npc.velocity.Y > 0f)
                        npc.velocity.Y *= 0.98f;

                    npc.velocity.Y -= verticalAcc;
                    if (npc.velocity.Y > maxVertSpeed)
                        npc.velocity.Y = maxVertSpeed;
                }
                else if (npc.Center.Y < Main.player[npc.target].Center.Y - 500f) 
                {
                    if (npc.velocity.Y < 0f)
                        npc.velocity.Y *= 0.98f;

                    npc.velocity.Y += verticalAcc;
                    if (npc.velocity.Y < 0f - maxVertSpeed)
                        npc.velocity.Y = 0f - maxVertSpeed;
                }

                if (npc.Center.X > Main.player[npc.target].Center.X + 100f) 
                {
                    if (npc.velocity.X > 0f)
                        npc.velocity.X *= 0.98f;

                    npc.velocity.X -= horiAcc;
                    if (npc.velocity.X > maxHoriSpeed)
                        npc.velocity.X = maxHoriSpeed;
                }

                if (npc.Center.X < Main.player[npc.target].Center.X - 100f) 
                {
                    if (npc.velocity.X < 0f)
                        npc.velocity.X *= 0.98f;

                    npc.velocity.X += horiAcc;
                    if (npc.velocity.X < 0f - maxHoriSpeed)
                        npc.velocity.X = 0f - maxHoriSpeed;
                }
            }
            else if (npc.ai[1] == 1f) 
            {
                //charge at player
                npc.defense *= 2;
                npc.damage *= 2;
                npc.ai[2] += 1f;
                if (npc.ai[2] == 2f)
                    SoundEngine.PlaySound(SoundID.Roar, npc.Center);

                if (npc.ai[2] >= 400f) 
                {
                    npc.ai[2] = 0f;
                    npc.ai[1] = 0f;
                }

                npc.rotation += npc.direction * 0.3f;
                float distX = Main.player[npc.target].Center.X - npc.Center.X;
                float distY = Main.player[npc.target].Center.Y - npc.Center.Y;
                float dist = (float)Math.Sqrt(distX * distX + distY * distY);
                float speed = PrimeStats.primeSpinBaseSpeed;
                if (dist > 150f)
                    speed *= 1.05f; //vanilla value 1.05

                if (dist > 200f)
                    speed *= PrimeStats.primeSpinBonusSpeedFromDist; //vanilla value 1.1

                if (dist > 250f)
                    speed *= PrimeStats.primeSpinBonusSpeedFromDist; //vanilla value 1.1

                if (dist > 300f)
                    speed *= PrimeStats.primeSpinBonusSpeedFromDist; //vanilla value 1.1

                if (dist > 350f)
                    speed *= PrimeStats.primeSpinBonusSpeedFromDist; //vanilla value 1.1

                if (dist > 400f)
                    speed *= PrimeStats.primeSpinBonusSpeedFromDist; //vanilla value 1.1

                if (dist > 450f)
                    speed *= PrimeStats.primeSpinBonusSpeedFromDist; //vanilla value 1.1

                if (dist > 500f)
                    speed *= PrimeStats.primeSpinBonusSpeedFromDist; //vanilla value 1.1

                if (dist > 550f)
                    speed *= PrimeStats.primeSpinBonusSpeedFromDist; //vanilla value 1.1

                if (dist > 600f)
                    speed *= PrimeStats.primeSpinBonusSpeedFromDist; //vanilla value 1.1

                float speed2 = speed / dist;
                npc.velocity.X = distX * speed2;
                npc.velocity.Y = distY * speed2;
            }
            else if (npc.ai[1] == 2f) 
            {
                //Day enragement
                npc.damage = 1000;
                npc.defense = 9999;
                npc.rotation += npc.direction * 0.3f;
                float distX = Main.player[npc.target].Center.X - npc.Center.X;
                float distY = Main.player[npc.target].Center.Y - npc.Center.Y;
                float dist = (float)Math.Sqrt(distX * distX + distY * distY);
                float speed = 10f;
                speed += dist / 100f;
                if (speed < 8f)
                    speed = 8f;

                if (speed > 32f)
                    speed = 32f;

                float speed2 = speed / dist;
                npc.velocity.X = distX * speed2;
                npc.velocity.Y = distY * speed2;
            }
            else if (npc.ai[1] == 3f) 
            {
                //despwan
                npc.velocity.Y += 0.1f;
                if (npc.velocity.Y < 0f)
                    npc.velocity.Y *= 0.95f;

                npc.velocity.X *= 0.95f;
                npc.EncourageDespawn(500);
            }
        }

    }
}