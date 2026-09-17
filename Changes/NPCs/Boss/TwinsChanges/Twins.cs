using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.NPCs.Miniboss.Santa;
using TRAEProject.Common;
using TRAEProject.NewContent.Items.FlamethrowerAmmo;
using TRAEProject.NewContent.Projectiles.KinnaraFeather;
using static AssGen.Assets;
using static System.Net.Mime.MediaTypeNames;
using static Terraria.ModLoader.ModContent;
using static tModPorter.ProgressUpdate;

namespace TRAEProject.Changes.NPCs.Boss.TwinsChanges
{
    public class Twins : GlobalNPC
    {
        static Asset<Effect> tintShader;
        public static Color EyeLaserViolet => new(104f / 255f, 3f / 255f, 253f / 255);
        public static Color CursedFlamesLime => new(179f / 255, 252f / 255f, 0, 1f);

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NoMultiplayerSmoothingByType[NPCID.Retinazer] = true;
        }

        public override void SetDefaults(NPC npc)
        {
            if (GetInstance<BossConfig>().TwinsRework && !Main.zenithWorld)
            {
                if (npc.type == NPCID.Retinazer)
                {
            


                    npc.lifeMax = (int)(npc.lifeMax * ((float)14000 / 20000));
                }
                else if (npc.type == NPCID.Spazmatism)
                {
                    npc.lifeMax = (int)(npc.lifeMax * ((float)20000 / 23000));
                }
            }
        }
        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
        {
            if (GetInstance<BossConfig>().TwinsRework && !Main.zenithWorld && Main.masterMode)
            {
                // Balance numbers are finely tuned to land at 34770 for Spaz and 24300 for Ret on Master singleplayer
                if (npc.type == NPCID.Spazmatism)
                {
                    balance = 0.90909090909f;
                    npc.lifeMax = (int)(npc.lifeMax * balance);
                }
                if (npc.type == NPCID.Retinazer)
                {
                    balance = 0.90764f;
                    npc.lifeMax = (int)(npc.lifeMax * balance);
                }
            }
        }

        public static void Move(NPC npc)
        {
            int abs = (int)Math.Abs(npc.ai[3]);
            Vector2 goHere = Main.player[npc.target].Center + new Vector2(abs % 10 == 2 ? 40 : -450, abs / 10 == 2 ? 300 : -360);
            FlyTo(npc, goHere, true);
            if (npc.ai[3] >= 0)
            {
                npc.ai[3] = -11;
            }
            /*
            if(npc.ai[3] != -1)
            {
                if(npc.ai[3] == 0)
                {
                    npc.localAI[2] *= -1;
                }
                else
                {
                    npc.localAI[3] *= -1;
                }
                npc.ai[3] = -1;
            }
            */
            if (npc.ai[2] % 80 == 0 && npc.ai[2] % 160 != 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int r = Main.rand.Next(2);
                    if (r == 0)
                    {
                        switch (npc.ai[3])
                        {
                            case -11:
                                npc.ai[3] = -21;
                                break;
                            case -12:
                                npc.ai[3] = -22;
                                break;
                            case -21:
                                npc.ai[3] = -11;
                                break;
                            case -22:
                                npc.ai[3] = -12;
                                break;
                        }
                    }
                    else
                    {
                        switch (npc.ai[3])
                        {
                            case -11:
                                npc.ai[3] = -12;
                                break;
                            case -12:
                                npc.ai[3] = -21;
                                break;
                            case -21:
                                npc.ai[3] = -22;
                                break;
                            case -22:
                                npc.ai[3] = -21;
                                break;
                        }
                    }
                    npc.netUpdate = true;
                }
            }
        }
        public static void FlyTo(NPC npc, Vector2 goHere, bool phase2 = false)
        {
            float topSpeed = 18f;
            float acceleration = 0.05f;
            
            if (phase2)
            {
                float Distance = npc.Distance(Main.player[npc.target].Center);
                if (Distance > 800f)
                {
                    topSpeed *= Distance / 800f;
                }
                acceleration *= 5;
                npc.damage = 0;
                //if((goHere - npc.Center).Length() < acceleration)
                //{
                //	npc.Center = goHere;
                //	npc.velocity = Main.player[npc.target].velocity;
                //	return;
                //}
            }
            if (Main.expertMode)
            {
                topSpeed *= 1.25f;
                acceleration *= 1.15f;
            }
            if (Main.masterMode && !phase2)
            {
                topSpeed *= 1.25f;
                acceleration *= 5f;

            }
            if (Main.getGoodWorld)
            {
                topSpeed *= 1.15f;
                acceleration *= 1.15f;
            }
            else if (npc.ai[1] == 1f) // spin attack
            {
                topSpeed = Main.masterMode ? 15f : 5f;
                
            }
            Vector2 targetVel = (goHere - npc.Center).SafeNormalize(Vector2.UnitY) * topSpeed;
            float velX = targetVel.X;
            float velY = targetVel.Y;
            if (npc.velocity.X < velX)
            {
                npc.velocity.X += acceleration;
                if (npc.velocity.X < 0f && velX > 0f)
                {
                    npc.velocity.X += acceleration;
                }
            }
            else if (npc.velocity.X > velX)
            {
                npc.velocity.X -= acceleration;
                if (npc.velocity.X > 0f && velX < 0f)
                {
                    npc.velocity.X -= acceleration;
                }
            }
            if (npc.velocity.Y < velY)
            {
                npc.velocity.Y += acceleration;
                if (npc.velocity.Y < 0f && velY > 0f)
                {
                    npc.velocity.Y += acceleration; // so that he doesnt fly way up offscreen
                    if (!phase2)
                        npc.velocity.Y += acceleration * 2; // so that he doesnt fly way up offscreen
                }
            }
            else if (npc.velocity.Y > velY)
            {
                npc.velocity.Y -= acceleration;
                if (npc.velocity.Y > 0f && velY < 0f)
                {
                    npc.velocity.Y -= acceleration;
                }
            }
            ;
        }

        const int masterDefenseUp = 8;
        const float retMasterDamageNerf = 0.78f;

        const int retLaserCooldownPhase1 = 240;
        const int retLaserCooldownPhase2 = 120;
        const int masterSecondShotTime = 10;
 

      

        const int retShotsFiredPhase1 = 3;
        const int retShotsFiredPhase2 = 4;
        const float masterRetNerfedShootSpeed = 0.25f;

        const float spinAttackDuration = 240f;
        const int spinAttackDelay = 5;

        const int delayBeforeRapidFire = 180;
        
        const int retRapidfireRatePhase2 = 20;
        const int rapidShotsFired = 16;
        const int masterBonusRapidShots = 4;
      
        const float RetshootSpeed = 12.5f;

        const float otherTwinhealthPercentforPhase2 = 0.05f;
        const float otherTwinhealthPercentforPhase2Master = 0.25f;
        const float phaseTransitionAt = 0.4f;
        const float phaseTransitionAtMaster = 0.55f;

        const float phase1SpazChargeDuration = 40f;
        const int phase1SpazCharges = 9; // vanilla value, ai[3] references this 
        public const int masterFlamesDamage = 132;

        const float minSize = 1.375f;
        const float maxSize = 1.485f;
    
        public override bool PreAI(NPC npc)
        {
            if (GetInstance<BossConfig>().TwinsRework && !Main.zenithWorld)
            {
 
                if (npc.type == NPCID.Retinazer)
                {
                    if (Main.masterMode)
                    {
                        if (npc.ai[0] != 0)
                        {
  
                            npc.defense = npc.defDefense + 10 + masterDefenseUp;
                            npc.HitSound = SoundID.NPCHit4;

                        }
                        else
                            npc.defense = npc.defDefense + masterDefenseUp;

                    }
 
                    npc.HitSound = SoundID.NPCHit1;
                    if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
                    {
                        npc.TargetClosest();
                    }
                    bool dead = Main.player[npc.target].dead;
                    //RetPhase3.Rotate(npc);
                    if (Main.expertMode)
                    {

                        if (NPC.CountNPCS(NPCID.Spazmatism) <= 0 && npc.ai[0] < 4)
                        {
                            npc.ai[0] = 4f;
                            npc.ai[1] = 0f;
                            npc.ai[2] = 0f;
                            npc.ai[3] = 0f;
                            npc.netUpdate = true;
                        }

                        if (npc.ai[0] == 0)
                        {
                            float phaseTransition = Main.masterMode ? phaseTransitionAtMaster : phaseTransitionAt;
                            float otherTwinsPhaseTransition = Main.masterMode ? otherTwinhealthPercentforPhase2Master : otherTwinhealthPercentforPhase2;
                            int spaz = NPC.FindFirstNPC(NPCID.Spazmatism);
                            
                            
                            if (npc.GetLifePercent() < phaseTransition || (spaz != -1 && Main.npc[spaz].GetLifePercent() < otherTwinsPhaseTransition))
                            {

                                npc.ai[0] = 1f;
                                npc.ai[1] = 0f;
                                npc.ai[2] = 0f;
                                npc.ai[3] = 0f;
                                npc.netUpdate = true;
                            }
                            

                        }
                    }





           
                    if (npc.ai[0] < 5)
                    {
                        float rotateTowards = TRAEMethods.PredictiveAimWithOffset(GetPupilPosition(npc), RetshootSpeed * 100, Main.player[npc.target].Center, Main.player[npc.target].velocity, npc.ai[1] == 0 ? 60 : 45) - MathF.PI / 2;
                        float rotSpeed = 0.05f;
                        if (npc.ai[1] == 0)
                        {
                            rotSpeed *= 3;
                        }
                        if (npc.ai[0] == 0 && npc.ai[1] == 1)
                        {
                            npc.rotation += rotSpeed;
                        }
                        else if (!float.IsNaN(rotateTowards))
                        {
                            npc.rotation.SlowRotation(rotateTowards, rotSpeed);
                        }
                    }
                    if (Main.rand.Next(5) == 0)
                    {
                        int num402 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + npc.height * 0.25f), npc.width, (int)(npc.height * 0.5f), 5, npc.velocity.X, 2f);
                        Main.dust[num402].velocity.X *= 0.5f;
                        Main.dust[num402].velocity.Y *= 0.1f;
                    }
                    if (Main.netMode != 1 && !Main.dayTime && !dead && npc.timeLeft < 10)
                    {
                        for (int num403 = 0; num403 < 200; num403++)
                        {
                            if (num403 != npc.whoAmI && Main.npc[num403].active && (Main.npc[num403].type == 125 || Main.npc[num403].type == 126))
                            {
                                npc.DiscourageDespawn(Main.npc[num403].timeLeft - 1);
                            }
                        }
                    }
                    if (Main.dayTime || dead)
                    {
                        npc.velocity.Y -= 0.04f;
                        npc.EncourageDespawn(10);
                        return false;
                    }
                    if (npc.ai[0] == 4 || npc.ai[0] == 5)
                    {
                        npc.defense = npc.defDefense + 50;
                        RetPhase3.Start(npc);
                        return false;
                    }
                    else if (npc.ai[0] > 5)
                    {
 
                        RetPhase3.Update(npc);
                        return false;
                    }
                    if (npc.ai[0] == 0f)
                    {
                   
                        int side = 1;
                        if (npc.Center.X < Main.player[npc.target].Center.X)
                        {
                            side = -1;
                        }
                        Vector2 goHere = Main.player[npc.target].Center + new Vector2(side * 300, -250);
                        FlyTo(npc, goHere, false);
                        if (npc.ai[1] == 0f)
                        {
                            npc.ai[2] += 1f;
                            if (npc.ai[2] >= retLaserCooldownPhase1 * retShotsFiredPhase1)
                            {
                                npc.ai[1] = 1f;
                                npc.ai[2] = 0f;
                                npc.ai[3] = 0f;
                                npc.target = 255;
                                npc.netUpdate = true;
                            }
                            else
                            {
                                if (!Main.player[npc.target].dead)
                                {
                                    npc.ai[3] += 1f;
                                }

                                if (npc.ai[3] >= retLaserCooldownPhase1)
                                {

                                    if (Main.masterMode && ((npc.ai[2] == retLaserCooldownPhase1) || (npc.ai[2] == retLaserCooldownPhase1 * 2.25f)))
                                        npc.ai[3] -= retLaserCooldownPhase1 / 4;
                                    else
                                        npc.ai[3] = 0;

                               
                                    Vector2 shotPos = GetPupilPosition(npc) + TRAEMethods.PolarVector(30, npc.rotation + MathF.PI / 2) + npc.velocity * 2f;
                                    Vector2 particlePos = GetPupilPosition(npc);
                                    Vector2 shotVel = TRAEMethods.PolarVector(RetshootSpeed, npc.rotation + MathF.PI / 2);
                                    RetPhase3.EyeLaserShootDust(shotVel, particlePos);
                                    if (Main.netMode != 1)
                                    {
                                        int damage = npc.GetAttackDamage_ForProjectiles(20f, 19f);
                                        if (Main.masterMode)
                                            damage = (int)(damage * retMasterDamageNerf);
                                        int num413 = Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), shotPos, shotVel, ProjectileID.EyeLaser, damage, 0f, Main.myPlayer);
                                        
                                    }
                                }
                            }
                        }
                        else if (npc.ai[1] == 1f)
                        {
                            //spin attack
                            
                            //if (Main.masterMode)
                            //{
                            //    npc.HitSound = SoundID.NPCHit53;
                            //    npc.defense = npc.defDefense + 10;
                            //}
                    

                            npc.ai[2] += 1f;
                            if (npc.ai[2] >= spinAttackDuration)
                            {
                                npc.ai[2] = 0;
                                npc.ai[1] = 0;
                            }
                            if (npc.ai[2] % spinAttackDelay == 0)
                            {
                
                                Vector2 particlePos = GetPupilPosition(npc);
                                Vector2 shotVel = TRAEMethods.PolarVector(RetshootSpeed, npc.rotation + MathF.PI / 2);
                                RetPhase3.EyeLaserShootDust(shotVel, particlePos);
                                if (Main.netMode != 1)
                                {
                                    int damage = npc.GetAttackDamage_ForProjectiles(20f, 19f);
                                    if (Main.masterMode)
                                        damage = (int)(damage * retMasterDamageNerf);
                                    int num413 = Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), GetPupilPosition(npc) + TRAEMethods.PolarVector(30, npc.rotation + MathF.PI / 2), shotVel, ProjectileID.EyeLaser, damage, 0f, Main.myPlayer);
                                }
                            }
                  
                
                        }
                        if (NPC.CountNPCS(NPCID.Spazmatism) <= 0)
                        {
                            npc.ai[0] = 4f;
                            npc.ai[1] = 0f;
                            npc.ai[2] = 0f;
                            npc.ai[3] = 0f;
                            npc.netUpdate = true;
                        }
                  

                        return false;
                    }
                    if (npc.ai[0] == 1f || npc.ai[0] == 2f)
                    {
                        if (npc.ai[0] == 1f)
                        {
                            npc.ai[2] += 0.005f;
                            if (npc.ai[2] > 0.5)
                            {
                                npc.ai[2] = 0.5f;
                            }
                        }
                        else
                        {
                            npc.ai[2] -= 0.005f;
                            if (npc.ai[2] < 0f)
                            {
                                npc.ai[2] = 0f;
                            }
                        }
                        npc.rotation += npc.ai[2];
                        npc.ai[1] += 1f;
                        if (npc.ai[1] >= 100f)
                        {
                            npc.ai[0] += 1f;
                            npc.ai[1] = 0f;
                            if (npc.ai[0] == 3f)
                            {
                                npc.ai[2] = 0f;
                            }
                            else
                            {
                                //SoundEngine.PlaySound(SoundID.NPCHit, npc.Center);
                                for (int num418 = 0; num418 < 2; num418++)
                                {
                                    Gore.NewGore(npc.GetSource_ReleaseEntity(), npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 143);
                                    Gore.NewGore(npc.GetSource_ReleaseEntity(), npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 7);
                                    Gore.NewGore(npc.GetSource_ReleaseEntity(), npc.position, new Vector2(Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f), 6);
                                }
                                for (int num419 = 0; num419 < 20; num419++)
                                {
                                    Dust.NewDust(npc.position, npc.width, npc.height, 5, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f);
                                }
                                npc.localAI[2] = 450;
                                npc.localAI[3] = -300;
                                SoundEngine.PlaySound(SoundID.ForceRoar, npc.Center);
                            }
                        }
                        Dust.NewDust(npc.position, npc.width, npc.height, 5, Main.rand.Next(-30, 31) * 0.2f, Main.rand.Next(-30, 31) * 0.2f);
                        npc.velocity.X *= 0.98f;
                        npc.velocity.Y *= 0.98f;
                        if (npc.velocity.X > -0.1 && npc.velocity.X < 0.1)
                        {
                            npc.velocity.X = 0f;
                        }
                        if (npc.velocity.Y > -0.1 && npc.velocity.Y < 0.1)
                        {
                            npc.velocity.Y = 0f;
                        }
                        return false;
                    }
                
                    if (npc.ai[1] == 0f) // ret phase 2 
                    {
                       
                        npc.damage = (int)(npc.defDamage * 1.5);
                        npc.defense = npc.defDefense + 10;
                        npc.HitSound = SoundID.NPCHit4;
                        npc.ai[2] += 1f;
                      
                        int firingDuration = retLaserCooldownPhase2 * retShotsFiredPhase2;
                        int cycleTime = firingDuration + delayBeforeRapidFire + retRapidfireRatePhase2 * rapidShotsFired;
                        if (Main.masterMode)
                        {
                            cycleTime += retRapidfireRatePhase2 * masterBonusRapidShots;
                            if ((int)npc.ai[2] <= firingDuration)
                                npc.ai[2] -= masterRetNerfedShootSpeed;
                        }

                        //if (npc.ai[2] == retLaserCooldownPhase2 * retShotsFiredPhase2)
                        //{

                        //    npc.velocity *= 0.2f;

                        //}
                      
                        if (npc.ai[2] > retLaserCooldownPhase2 * retShotsFiredPhase2)
                        {
                            Player target = Main.player[npc.target];

                            Dust.NewDustPerfect(GetLaserCannonPosition(npc), DustID.TheDestroyer, Vector2.Zero, 0, Color.White with { A = 0 }, 1f);
                            if (npc.velocity.Length() > 16f && Main.masterMode)
                            {
                                 npc.velocity = npc.oldVelocity + target.velocity / 2; // more aggressively tracks the player by keeping up with their speed
                            }
                            float speed = 1.25f;
                            int num425 = 1;
                            if (npc.position.X + npc.width / 2 < Main.player[npc.target].position.X + Main.player[npc.target].width)
                            {
                                num425 = -1;
                            }
                            Vector2 retPosition = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                            float playerpositionX = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 + num425 * 180 - retPosition.X;
                            float playerpositionY = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - retPosition.Y;
                            float playerDdistanceToRet = MathF.Sqrt((float)(playerpositionX * playerpositionX + playerpositionY * playerpositionY));
                            if (Main.expertMode)
                            {
                                if (playerDdistanceToRet > 300f)
                                {
                                    speed += 0.5f;
                                }
                                if (playerDdistanceToRet > 400f)
                                {
                                    speed += 0.5f;
                                }
                                if (playerDdistanceToRet > 500f)
                                {
                                    speed += 0.75f;
                                }
                                if (playerDdistanceToRet > 600f)
                                {
                                    speed += 0.75f;
                                }
                                if (playerDdistanceToRet > 700f)
                                {
                                    speed += 1.5f;
                                }
                                if (playerDdistanceToRet > 800f)
                                {
                                    speed += 1.5f;
                                }
                            }

                            speed *= 2f; float accel = Main.expertMode ? 0.16f : 0.125f;

                            playerDdistanceToRet /= speed;
                            playerpositionX *= playerDdistanceToRet;
                            playerpositionY *= playerDdistanceToRet;
                            if (npc.velocity.X < playerpositionX)
                            {
                                ref float x1 = ref npc.velocity.X; // could maybe be simplified to npc.velocity.X += accel 
                                x1 += accel;
                                if (npc.velocity.X < 0f && playerpositionX > 0f)
                                {
                                    ref float x2 = ref npc.velocity.X;
                                    x2 += accel;
                                }
                            }
                            else if (npc.velocity.X > playerpositionX)
                            {
                                ref float x3 = ref npc.velocity.X;
                                x3 -= accel;
                                if (npc.velocity.X > 0f && playerpositionX < 0f)
                                {
                                    ref float x4 = ref npc.velocity.X;
                                    x4 -= accel;
                                }
                            }
                            if (npc.velocity.Y < playerpositionY)
                            {
                                ref float x5 = ref npc.velocity.Y;
                                x5 += accel;
                                if (npc.velocity.Y < 0f && playerpositionY > 0f)
                                {
                                    ref float x6 = ref npc.velocity.Y;
                                    x6 += accel;
                                }
                            }
                            else if (npc.velocity.Y > playerpositionY)
                            {
                                ref float x7 = ref npc.velocity.Y;
                                x7 -= accel;
                                if (npc.velocity.Y > 0f && playerpositionY < 0f)
                                {
                                    ref float x8 = ref npc.velocity.Y;
                                    x8 -= accel;
                                }
                            }
                            ref float x = ref npc.ai[2];
                            x += 1f;
                          
                            if ((int)npc.ai[2] >= firingDuration + delayBeforeRapidFire && (int)npc.ai[2] % (retRapidfireRatePhase2) == 0)
                            {
                                Dust dust = Dust.NewDustPerfect(GetLaserCannonPosition(npc), DustID.TheDestroyer, Vector2.Zero, 0, Color.White with { A = 0 }, 1f);
                                dust.noGravity = true;
                   


                                Vector2 vector116 = new(npc.Center.X + npc.direction * 50, npc.Center.Y + Main.rand.Next(35, 90));
                                float num959 = target.Center.X - vector116.X + Main.rand.Next(-40, 41);
                                float num960 = target.Center.Y - vector116.Y + Main.rand.Next(-40, 41);
                                num959 += Main.rand.Next(-40, 41);
                                num960 += Main.rand.Next(-40, 41);
                                float num961 = MathF.Sqrt(num959 * num959 + num960 * num960);
                                float num962 = 9f;
                                num961 = num962 / num961;
                                num959 *= num961;
                                num960 *= num961;
                                Vector2 shotVel = new(num959, num960);
                                Vector2 pos = GetPupilPosition(npc) + TRAEMethods.PolarVector(46, npc.rotation + MathF.PI / 2) + npc.velocity * 2f;
                                if (Main.netMode != 1)
                                {
                                    int damage = npc.GetAttackDamage_ForProjectiles(25f, 23f);
                                    if (Main.masterMode)
                                        damage = (int)(damage * retMasterDamageNerf);
                                    Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), pos.X, pos.Y, num959, num960, ProjectileID.DeathLaser, damage, 0f, Main.myPlayer);
                                }
                              
                                RetPhase3.DeathLaserShootDust(shotVel, GetLaserCannonPosition(npc));
                            }
                            if (npc.ai[2] >= cycleTime)
                            {
                                npc.ai[2] = 0;
                                npc.netUpdate = true;
                            }
                        }
                        else
                        {
                            //TRAEMethods.ServerClientCheck(npc.localAI[2] + ", " + npc.localAI[3]);
                            Move(npc);
                            int secondShot = 130;
                            if ((int)npc.ai[2] % retLaserCooldownPhase2 == 0
                                || (Main.masterMode && (int)npc.ai[2] % secondShot == 0)
                                )
                            {
                                if (npc.ai[2] > secondShot)
                                    secondShot = (int)npc.ai[2] + masterSecondShotTime;
                                Vector2 shotVel = TRAEMethods.PolarVector(RetshootSpeed, npc.rotation + MathF.PI / 2);
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {

                                    int damage = npc.GetAttackDamage_ForProjectiles(25f, 23f);
                                    if (Main.masterMode)
                                        damage = (int)(damage * retMasterDamageNerf);
                                    int num413 = Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), GetPupilPosition(npc) + TRAEMethods.PolarVector(46, npc.rotation + MathF.PI / 2) + npc.velocity * 2f, shotVel, ProjectileID.DeathLaser, damage, 0f, Main.myPlayer);
                                }
                                RetPhase3.DeathLaserShootDust(shotVel, Twins.GetLaserCannonPosition(npc));
                                

                            }
                        }
                        if (NPC.CountNPCS(NPCID.Spazmatism) <= 0 && Main.expertMode && !dead)
                        {
                            npc.ai[0] = 4f;
                            npc.ai[1] = 0f;
                            npc.ai[2] = 0f;
                            npc.ai[3] = 0f;
                            npc.netUpdate = true;
                        }
                        return false;
                    }

                    return false;
                }
                if (npc.type == NPCID.Spazmatism)
                {
                
                    if (npc.ai[0] == 0)
                    {
                 
                        Player player = Main.player[npc.target];
                        float timerIncr = 1f;
                        
                        if (Main.masterMode)
                        {
                            
                            if (npc.ai[1] == 2f || npc.ai[1] == 1f) // charge state
                            {
                                //npc.HitSound = SoundID.NPCHit53;
                         
                         
                                //npc.defense = npc.defDefense + 18;
                                switch (npc.ai[2])
                                {
                                    case 0:
                                        if (Main.netMode != NetmodeID.MultiplayerClient)
                                        {
                                            int numberOfProjectiles = 2;
                                            float rotation = MathHelper.ToRadians(14);
                                            Vector2 shootFrom = GetPupilPosition(npc);
                                            float shootSpeed = 6.75f;
                                        
                                            for (int i = 0; i < numberOfProjectiles + 1; i += 2)
                                            {
                                                float posX = npc.Center.X;
                                                float posY = npc.Center.Y;
                                                Vector2 vel = TRAEMethods.PolarVector(shootSpeed, npc.rotation + MathF.PI / 2);

                                                Vector2 perturbedSpeed = vel.RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)((float)i / (float)numberOfProjectiles))); // Watch out for dividing by 0 if there is only 1 projectile.

                                                int damage = masterFlamesDamage / 6; // divide by 6 because that's how master calculates damage
                                                Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), shootFrom, perturbedSpeed, ProjectileID.EyeFire, damage, 0);
                                            }
                                            int dustsToMake = 25;

                                            for (int i = 0; i < dustsToMake; i++)
                                            {
                                                double radius = 2 * Math.PI / dustsToMake;

                                                Vector2 speed = new(5, 5);
                                                Dust d = Dust.NewDustPerfect(GetPupilPosition(npc), DustID.CursedTorch, speed.RotatedBy(radius * i) + npc.velocity, Scale: 2.5f);

                                                d.noGravity = true;
                                            }
                                        }
                                        break;
                                    case < 10f:
                                        npc.ai[2] -= 0.75f;
                                        if (npc.velocity.Length() > 3)
                                            npc.velocity += player.velocity * 0.67f / 60; // more aggressively tracks the player by keeping up with their speed
                                        
                                        break;
                                    
                           
                                        


                                }

                                npc.velocity += npc.oldVelocity * 3;
                            }
                        }

                      
                        if (npc.ai[1] == 0f) // fireballs
                        {
                            
                            if (npc.ai[3] + timerIncr >= 60 && npc.ai[2] <= 540f)
                            {                                 
                                CursedFlameShootDust(GetPupilPosition(npc));
                            }
                             if (Main.masterMode && 
                                ((int)npc.ai[2] > 480f && (int)npc.ai[2] <= 540f)
                                ||
                                ((int)npc.ai[2] > 300f && (int)npc.ai[2] <= 360f)
                                )
                            {
                      
                                if ((int)npc.ai[2] == 540f || (int)npc.ai[2] == 360f)
                                {
                                    for (int i = 0; i < 25; i++)
                                    {
                                        double radius = 2 * Math.PI / 25;
                                        Vector2 speed = new(5, 5);
                                        Dust d = Dust.NewDustPerfect(GetPupilPosition(npc), DustID.CursedTorch, speed.RotatedBy(radius * i) + npc.velocity, Scale: 2.5f);
                                        d.noGravity = true;
                                    }
                                    int numberOfProjectiles = 2;
                                    float rotation = MathHelper.ToRadians(26);
                                    Vector2 shootFrom = npc.Center + TRAEMethods.PolarVector(14, npc.rotation + MathF.PI / 2);
                                    float shootSpeed = 13.5f;
                                    if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        for (int i = 0; i < numberOfProjectiles + 1; i += 2)
                                        {
                                            float posX = npc.Center.X;
                                            float posY = npc.Center.Y;
                                            float speedX = player.Center.X - posX;
                                            float speedY = player.Center.Y - posY;
                                            Vector2 vel = TRAEMethods.PolarVector(shootSpeed, npc.rotation + MathF.PI / 2);
                                            float count = -1 + i;
                                            Vector2 perturbedSpeed = vel.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberOfProjectiles))); // Watch out for dividing by 0 if there is only 1 projectile.
                                            Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), shootFrom, perturbedSpeed, ProjectileID.CursedFlameHostile, 22, 0);
                                        }
                                    }
                                }
                                npc.ai[3] -= 0.25f;
                                npc.ai[2] -= 0.25f;





                            }
                            if (Main.masterMode && npc.ai[2] >= 570)
                            {
                                npc.velocity *= 0.966f; // slow him down before he does the first charge, else the flame jets almost always hit
                            }
                            if (npc.ai[2] >= 600)
                            {

                                npc.ai[2] = 0;
                            }
                        }

                      
                           
                    }

                  
                    if (npc.ai[0] == 3f)
                    {
                        npc.rotation = npc.DirectionTo(Main.player[npc.target].Center).ToRotation() - 1.567f;
                        npc.TargetClosest();
                        if (npc.ai[1] == 0f)
                        {
                            float speed = 2.4f;
                            int num425 = 1;
                            if (npc.position.X + npc.width / 2 < Main.player[npc.target].position.X + Main.player[npc.target].width)
                            {
                                num425 = -1;
                            }
                            Vector2 spazposition = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                            float playerpositionX = Main.player[npc.target].position.X + Main.player[npc.target].width / 2 + num425 * 180 - spazposition.X;
                            float playerpositionY = Main.player[npc.target].position.Y + Main.player[npc.target].height / 2 - spazposition.Y;
                            float playerpositiontospaz = MathF.Sqrt((float)(playerpositionX * playerpositionX + playerpositionY * playerpositionY));
                            if (Main.expertMode)
                            {
                                if (playerpositiontospaz > 300f)
                                {
                                    speed += 0.5f;
                                }
                                if (playerpositiontospaz > 400f)
                                {
                                    speed += 0.5f;
                                }
                                if (playerpositiontospaz > 500f)
                                {
                                    speed += 0.75f;
                                }
                                if (playerpositiontospaz > 600f)
                                {
                                    speed += 0.75f;
                                }
                                if (playerpositiontospaz > 700f)
                                {
                                    speed += 1.5f;
                                }
                                if (playerpositiontospaz > 800f)
                                {
                                    speed += 1.5f;
                                }
                            }

                            speed *= 2f; float accel = 0.2f;

                            playerpositiontospaz /= speed;
                            playerpositionX *= playerpositiontospaz;
                            playerpositionY *= playerpositiontospaz;
                            if (npc.velocity.X < playerpositionX)
                            {
                                ref float x1 = ref npc.velocity.X; // could maybe be simplified to npc.velocity.X += accel 
                                x1 += accel;
                                if (npc.velocity.X < 0f && playerpositionX > 0f)
                                {
                                    ref float x2 = ref npc.velocity.X;
                                    x2 += accel;
                                }
                            }
                            else if (npc.velocity.X > playerpositionX)
                            {
                                ref float x3 = ref npc.velocity.X;
                                x3 -= accel;
                                if (npc.velocity.X > 0f && playerpositionX < 0f)
                                {
                                    ref float x4 = ref npc.velocity.X;
                                    x4 -= accel;
                                }
                            }
                            if (npc.velocity.Y < playerpositionY)
                            {
                                ref float x5 = ref npc.velocity.Y;
                                x5 += accel;
                                if (npc.velocity.Y < 0f && playerpositionY > 0f)
                                {
                                    ref float x6 = ref npc.velocity.Y;
                                    x6 += accel;
                                }
                            }
                            else if (npc.velocity.Y > playerpositionY)
                            {
                                ref float x7 = ref npc.velocity.Y;
                                x7 -= accel;
                                if (npc.velocity.Y > 0f && playerpositionY < 0f)
                                {
                                    ref float x8 = ref npc.velocity.Y;
                                    x8 -= accel;
                                }
                            }
                            ref float x = ref npc.ai[2];
                            x += 1f;
                    
                            if (npc.ai[2] >= 300f)
                            {
                                npc.ai[1] = 1f;
                                npc.ai[2] = 0f;
                                npc.ai[3] = 0f;
                                npc.target = 255;
                                npc.netUpdate = true;
                            }
                        
                            if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                            {
                                ref float y = ref npc.localAI[2];
                                y += 2f;

                                if (npc.localAI[2] > 22f)
                                {
                                    npc.localAI[2] = 0f;
                                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item34 with { MaxInstances = 0 }, npc.position);
                                }
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    ref float x9 = ref npc.localAI[1];
                                    x9 += 2f;
      
                                    if (npc.soundDelay <= 0)
                                    {
                                        Terraria.Audio.SoundEngine.PlaySound(SoundID.ForceRoarPitched with { MaxInstances = 0 }, npc.Center);
                                        npc.soundDelay = 300;
                                    }
                                     

                                    if (npc.localAI[1] > 8f)
                                    {
                                        npc.localAI[1] = 0f;
                                        float shootVel = 5.5f;
                                        int attackDamage_ForProjectiles7 = npc.GetAttackDamage_ForProjectiles(30f, 27f);

                                        Vector2 vector51 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                                        float num483 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector51.X;
                                        float num484 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector51.Y;
                                        float num485 = (float)Math.Sqrt(num483 * num483 + num484 * num484);
                                        num485 = shootVel / num485;
                                        num483 *= num485;
                                        num484 *= num485;
                                        num484 += (float)Main.rand.Next(-40, 41) * 0.01f;
                                        num483 += (float)Main.rand.Next(-40, 41) * 0.01f;
                                        num484 += npc.velocity.Y * 0.5f;
                                        num483 += npc.velocity.X * 0.5f;
                                        vector51.X -= num483 * 1f;
                                        vector51.Y -= num484 * 1f;
                                        Vector2 velocity = new(num483, num484);
                                        float cosine = velocity.X / velocity.Length();
                                        float sin = velocity.Y / velocity.Length();
                                        int offSetX = 50;

                                        int offSetY = 50;
                             
                                        Vector2 offset = new Vector2( offSetX * cosine, offSetY * sin);
                                        int num489 = Projectile.NewProjectile(npc.GetSource_FromAI(), vector51 + offset, velocity, 101, attackDamage_ForProjectiles7, 0f, Main.myPlayer);


                                    }
                                }
                            }
                            return false;
                        } // flamethrower

                        if (npc.ai[1] == 2f) // charges
                        {
                            Player player = Main.player[npc.target];
                
                            
                            if (Main.masterMode)
                            {

                                if (npc.ai[1] == 2f || npc.ai[1] == 1f) // charge state
                                {

                                    //npc.HitSound = SoundID.NPCHit53;


                                    //npc.defense = npc.defDefense + 18;

                                    if (npc.ai[2] == 60f && Main.netMode != NetmodeID.MultiplayerClient)
                                    {
                                        int numberOfProjectiles = 3;
                                        float rotation = MathHelper.ToRadians(21);
                                        Vector2 shootFrom = GetPupilPosition(npc);
                                        float shootSpeed = 9.5f;
                                        for (int i = 0; i < numberOfProjectiles; i += 1)
                                        {
                                            float posX = npc.Center.X;
                                            float posY = npc.Center.Y;
                                            float speedX = player.Center.X - posX;
                                            float speedY = player.Center.Y - posY;
                                            Vector2 vel = TRAEMethods.PolarVector(shootSpeed, npc.rotation + MathF.PI / 2);
                                            float count = -1 + i;
                                            Vector2 perturbedSpeed = vel.RotatedByRandom(MathHelper.Lerp(-rotation, rotation, i / (numberOfProjectiles))); // Watch out for dividing by 0 if there is only 1 projectile.
                                            int damage = masterFlamesDamage / 6;
                                            Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), shootFrom, perturbedSpeed, ProjectileID.CursedFlameHostile, damage, 0, 0);
                                        }
                                        int dustsToMake = 25;

                                        for (int i = 0; i < dustsToMake; i++)
                                        {
                                            double radius = 2 * Math.PI / dustsToMake;

                                            Vector2 speed = new(5, 5);
                                            Dust d = Dust.NewDustPerfect(GetPupilPosition(npc), DustID.CursedTorch, speed.RotatedBy(radius * i) + npc.velocity, Scale: 2.5f);

                                            d.noGravity = true;
                                        }

                                    }


                                    // npc.ai[2] -= 0.25f; // important that this is last


                                    //npc.velocity += npc.oldVelocity * 3;
                                }
                            }
                        }
                     
                    }
                    if (npc.ai[0] >= 4f)
                    {
                       

                       
                        if (npc.ai[0] == 4f || npc.ai[0] == 5f)
                        {
                            npc.dontTakeDamage = true;
                            SpazPhase3.Start(npc);
                        }
                        else
                        {
                            npc.dontTakeDamage = false;
                            SpazPhase3.Update(npc);
                        }
                        return false;
                    }
                }
            }
            return base.PreAI(npc);
        }

        public override void AI(NPC npc)
        {

      
            if (GetInstance<BossConfig>().TwinsRework && !Main.zenithWorld)
            {
         
                if (npc.type == NPCID.Spazmatism)
                {   if (Main.masterMode)
                    {
                        
                        if (npc.ai[0] > 1)
                        {
                
                            npc.damage = (int)(npc.defDamage * 1.5);
                            npc.defense = npc.defDefense + 18 + masterDefenseUp;
                            npc.HitSound = SoundID.NPCHit4;

                        }
                        else 
                            npc.defense = npc.defDefense + masterDefenseUp;

                    }
                    


                    if (Main.expertMode)
                    {
                        bool dead = Main.player[npc.target].dead;
                        if (NPC.CountNPCS(NPCID.Retinazer) <= 0 && npc.ai[0] < 4 && Main.expertMode && !dead)
                        {
                            npc.ai[0] = 4f;
                            npc.ai[1] = 0f;
                            npc.ai[2] = 0f;
                            npc.ai[3] = 0f;
                            npc.netUpdate = true;
                        }
                        if (npc.ai[0] == 0)
                        {

                            int ret = NPC.FindFirstNPC(NPCID.Retinazer);

                            float phaseTransition = Main.masterMode ? phaseTransitionAtMaster : phaseTransitionAt;
                            float otherTwinsPhaseTransition = Main.masterMode ? otherTwinhealthPercentforPhase2Master : otherTwinhealthPercentforPhase2;
                        
                            if (npc.GetLifePercent() < phaseTransition || (ret != -1 && Main.npc[ret].GetLifePercent() < otherTwinsPhaseTransition))
                            {

                                npc.ai[0] = 1f;
                                npc.ai[1] = 0f;
                                npc.ai[2] = 0f;
                                npc.ai[3] = 0f;
                                npc.netUpdate = true;
                            }

                        }
                    

                    }
                }
            }
        }
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (GetInstance<BossConfig>().TwinsRework && !Main.zenithWorld)
            {

                if (npc.type == NPCID.Retinazer && npc.ai[0] >= 4f)
                {
                    RetPhase3.Phase3Draw(npc, spriteBatch, screenPos, drawColor);
                    return false;
                }
                if (npc.type == NPCID.Spazmatism)
                {

                    
                    if (npc.ai[0] >= 4)
                    {
                        SpazPhase3.Phase3Draw(npc, spriteBatch, screenPos, drawColor);
                        return false;
                    }
                    SpazPhase3.DrawSpazmatismInnerMouthGlow(npc, screenPos);
                    return true;
                }
            }

            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }
        public static void FadeLightBall(Vector2 drawPos, Color drawColor, float scale)
        {
            Texture2D tex = ModContent.Request<Texture2D>("TRAEProject/Assets/SpecialTextures/GlowBallSmallPremultiplied").Value;
            Main.EntitySpriteDraw(tex, drawPos, null, drawColor, Main.rand.NextFloat(MathF.Tau), tex.Size() / 2, scale, SpriteEffects.None);
            //white
            drawColor.R = drawColor.G = drawColor.B = drawColor.A;
            drawColor.A = 0;

            scale *= 0.4f;
            Main.EntitySpriteDraw(tex, drawPos, null, drawColor, Main.rand.NextFloat(MathF.Tau), tex.Size() / 2, scale, SpriteEffects.None);
        }
        public static void ShootTelegraphNew(Vector2 position, Color color, float progress, float rotationDirection)
        {
            float opacity = Utils.GetLerpValue(0f, .6f, progress, true) * 0.55f;
            Vector2 drawPos = position - Main.screenPosition;
            color.A = 0;
            Color additiveWhite = new(255, 255, 255, 0);
            float scale = TRAEMethods.RemapEased(progress, 0.025f, .1f, 0.5f, 1.6f, TRAEMethods.EaseOutQuad);
            FadeLightBall(drawPos, color * 0.5f * opacity, scale);
            FadeLightBall(drawPos, additiveWhite * 0.5f * opacity, scale * 0.5f);
            float rotation = TRAEMethods.RemapEased(progress, 0, 1, 3 * rotationDirection, MathF.PI / 4f, TRAEMethods.EaseOutQuad);
            TRAEMethods.DrawPrettyStarSparkle(opacity, drawPos, additiveWhite, color, Vector2.One * scale, rotation: rotation);
        }
        public static void CursedFlameShootDust(Vector2 origin)
        {
            Color additiveWhite = new(255, 255, 255, 0);
            float scale = 1.6f;
            Vector2 scaleVec = new(scale);
            GlowBall.NewGlowBall(origin, CursedFlamesLime * 0.5f, scaleVec, null, 20, null, 0.55f);
            GlowBall.NewGlowBall(origin, additiveWhite * 0.5f, scaleVec * .5f, null, 20, null, 0.55f);
            for (int i = 0; i < 5; i++)
            {
                Sparkle.NewSparkle(origin, Twins.CursedFlamesLime, new Vector2(1.6f), Vector2.Zero, 20, new Vector2(1f), Vector2.Zero, 1f, MathF.PI / 4f, 1f);
            }
        }
        public static void ImFuckingInvincible(Vector2 position, Color color, float progress)
        {
            float scale = MathHelper.Lerp(2f, 1f, progress);
            Color drawCol = color * Utils.GetLerpValue(0, .6f, progress, true);
            position -= Main.screenPosition;
            //FadeLightBall(position, drawCol, scale);
            Texture2D ring = ModContent.Request<Texture2D>("TRAEProject/Assets/SpecialTextures/RingPremultiplied").Value;
            drawCol = color * Utils.GetLerpValue(0.4f, 1f, progress, true);
            drawCol.A = 0;
            scale = MathHelper.Lerp(3f, 0.01f, progress);
            Main.EntitySpriteDraw(ring, position, null, drawCol, Main.rand.NextFloat(MathF.Tau), ring.Size() / 2, scale, SpriteEffects.None);
            Texture2D bubble = ModContent.Request<Texture2D>("TRAEProject/Assets/SpecialTextures/BubbleGlow").Value;
            Main.EntitySpriteDraw(bubble, position + new Vector2(5, 2), null, Color.White * 0.5f, MathF.PI, bubble.Size() / 2, scale, SpriteEffects.None);

        }
        public static void ShootTelegraph(Vector2 position, Color color, float progress)
        {
            float opacity = Utils.GetLerpValue(0f, .4f, progress, true);
            Vector2 drawPos = position - Main.screenPosition;
            color.A = 0;
            Color additiveWhite = new(255, 255, 255, 0);
            float scale = TRAEMethods.RemapEased(progress, 0, .5f, 0.5f, 2f, TRAEMethods.EaseOutQuad);
            FadeLightBall(drawPos, color * 0.5f * opacity, scale);
            FadeLightBall(drawPos, additiveWhite * 0.5f * opacity, scale * 0.5f);
            if (!Main.gamePaused && progress < .7f)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 offset = Main.rand.NextFloat(MathF.Tau).ToRotationVector2();
                    float dist = Main.rand.NextFloat(60f, 70f);
                    //   GlowyVioletDust(position + offset * dist, -offset * dist * 0.1f, progress * 3f);
                }
            }
            //  for (int i = -1; i < 2; i+=2)
            {
                //  float rotationOffset = MathF.PI * 0.25f - progress * MathF.PI * 0.25f * i;
                float rotation = TRAEMethods.RemapEased(progress, 0, 1, -2f, 0f, TRAEMethods.EaseOutQuad);
                TRAEMethods.DrawPrettyStarSparkle(opacity, drawPos, additiveWhite, color, Vector2.One * scale, rotation: rotation);
            }
        }
        static void GlowyVioletDust(Vector2 pos, Vector2 vel, float scale)
        {
            Color violet = Twins.EyeLaserViolet;
            Dust dust = Dust.NewDustPerfect(pos, DustID.RainbowMk2, vel, 0, violet, scale);
            dust.noGravity = true;
            dust = Dust.CloneDust(dust);
            dust.color = Color.White with { A = 0 };
            dust.scale *= 0.6f;
        }
        public static void ShootTelegraphOld(Vector2 position, Color color, float progress)
        {
            float scale = MathHelper.Lerp(2f, 1f, progress);
            Color drawCol = color * Utils.GetLerpValue(0, .6f, progress, true);
            position -= Main.screenPosition;
          
            FadeLightBall(position, drawCol, scale);
            Texture2D ring = ModContent.Request<Texture2D>("TRAEProject/Assets/SpecialTextures/RingPremultiplied").Value;
            drawCol = color * Utils.GetLerpValue(0.5f, 1f, progress, true);
            drawCol.A = 0;
            scale = MathHelper.Lerp(3f, 0.01f, progress);
            Main.EntitySpriteDraw(ring, position, null, drawCol, Main.rand.NextFloat(MathF.Tau), ring.Size() / 2, scale, SpriteEffects.None);
        }
        public static Vector2 GetLaserCannonPosition(NPC npc)
        {
            Vector2 halfSize = new(55f, 107f);
            float num35 = 0f;
            float num36 = Main.NPCAddHeight(npc);
            Vector2 Pos = new(
                npc.position.X + npc.width / 2 - TextureAssets.Npc[npc.type].Width() * npc.scale / 2f + halfSize.X * npc.scale,
                npc.position.Y + npc.height - TextureAssets.Npc[npc.type].Height() * npc.scale / Main.npcFrameCount[npc.type] + 4f + halfSize.Y * npc.scale + num36 + num35);
            Pos += (Vector2.UnitX * (30 + TextureAssets.Npc[npc.type].Width() / 2)).RotatedBy(npc.rotation + MathF.PI / 2);
            return Pos;
        }
        public static Vector2 GetPupilPosition(NPC npc)
        {
            Vector2 halfSize = new(55f, 107f);
            float num35 = 0f;
            float num36 = Main.NPCAddHeight(npc);
            Vector2 Pos = new(
                npc.position.X + npc.width / 2 - TextureAssets.Npc[npc.type].Width() * npc.scale / 2f + halfSize.X * npc.scale,
                npc.position.Y + npc.height - TextureAssets.Npc[npc.type].Height() * npc.scale / Main.npcFrameCount[npc.type] + 4f + halfSize.Y * npc.scale + num36 + num35);
            Pos += (Vector2.UnitX * TextureAssets.Npc[npc.type].Width() / 2).RotatedBy(npc.rotation + MathF.PI / 2);
            return Pos;
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (GetInstance<BossConfig>().TwinsRework && !Main.zenithWorld)
            {
                if (npc.type == NPCID.Retinazer)
                {
                    //if (Main.masterMode)
                    //{
                    //    //spin attack
                    //    if (npc.ai[1] == 1f)
                    //    {



                    //        int c = (int)npc.ai[3];
                    //        Color color = new(c, c, c, c);
                    //        Vector2 halfSize = new(55f, 107f);
                    //        float num35 = 0f;
                    //        float num36 = Main.NPCAddHeight(npc);

                    //        float progress = npc.ai[2] / spinAttackDuration;


                    //        float size = maxSize; // default to the max
                    //        if (progress <= 0.25f)
                    //        {
                    //            ;


                    //            size = MathHelper.Lerp(minSize, maxSize, progress / 0.25f);
                    //        }
                    //        else if (progress >= 0.75f)
                    //        {

                    //            size = MathHelper.Lerp(maxSize, minSize, (progress - 0.75f) / 0.25f);


                    //        }
                    //        int offSetShield = 2;
                    //        Vector2 pos = npc.Center + npc.rotation.ToRotationVector2() * offSetShield * npc.direction;
                    //        Lighting.AddLight(npc.Center, DustID.PurpleTorch);



                            


                    //    }
                    //}
                    if (npc.ai[0] == 0f && npc.ai[1] == 0)
                    {
                        Texture2D eyeGlow = Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/TwinsChanges/Retinizer_Glow").Value;
                        int c = (int)npc.ai[3];
                        Color color = new(c, c, c, c);
                        Vector2 halfSize = new(55f, 107f);
                        float num35 = 0f;
                        float num36 = Main.NPCAddHeight(npc);
                        Vector2 Pos = new(
                            npc.position.X - screenPos.X + npc.width / 2 - TextureAssets.Npc[npc.type].Width() * npc.scale / 2f + halfSize.X * npc.scale,
                            npc.position.Y - screenPos.Y + npc.height - TextureAssets.Npc[npc.type].Height() * npc.scale / Main.npcFrameCount[npc.type] + 4f + halfSize.Y * npc.scale + num36 + num35);
                        // spriteBatch.Draw(eyeGlow, Pos, npc.frame, color, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);
                        Pos += (Vector2.UnitX * TextureAssets.Npc[npc.type].Width() / 2).RotatedBy(npc.rotation + MathF.PI / 2);
                        float progress = Utils.GetLerpValue(100f, 239f, npc.ai[3], true);
                        Color violet = new(104f / 255f, 3f / 255f, 253f / 255);
                        Color purple = new(130f / 255f, 25f / 255f, 183f / 255f);
                        Pos += screenPos;
                        //ShootTelegraphNew(Pos, violet, progress, -1);
                        ShootTelegraphOld(Pos, violet * .75f, progress);
                    }
                    if (npc.ai[0] != 0f && npc.ai[2] < retLaserCooldownPhase2 * retShotsFiredPhase2 && npc.ai[0] != 6f)
                    {
                        Texture2D eyeGlow = Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/TwinsChanges/Retinizer_Glow").Value;
                        int c = (int)npc.ai[2] % 120;
                        Color color = new(c, c, c, c);
                        Vector2 halfSize = new(55f, 107f);
                        float num35 = 0f;
                        float num36 = Main.NPCAddHeight(npc);
                        Vector2 Pos = GetLaserCannonPosition(npc);
                        // spriteBatch.Draw(eyeGlow, Pos, npc.frame, color, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);

                        float progress = Utils.GetLerpValue(1f, 120f, npc.ai[2] % 120, true);

                        Color red = Color.Red;
                        Color purple = new(130f / 255f, 25f / 255f, 183f / 255f);

                        //ShootTelegraphNew(Pos, violet, progress, -1);
                        ShootTelegraphOld(Pos, red * .75f, progress);
                    }
                    if (RetPhase3.ShootingInPhase3(npc) && npc.ai[0] == 6f)
                    {
 
                        Texture2D eyeGlow = Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/TwinsChanges/Retinizer_Glow").Value;
                        int c = (int)npc.ai[2] % 120;
                        Color color = new(c, c, c, c);
                        Vector2 halfSize = new(55f, 107f);
                        float num35 = 0f;
                        float num36 = Main.NPCAddHeight(npc);
                        Vector2 Pos = GetLaserCannonPosition(npc);
                        // spriteBatch.Draw(eyeGlow, Pos, npc.frame, color, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);

                        float progress = RetPhase3.GetProgress(npc);
                        
                        Color red = Color.Red;
                        Color purple = new(130f / 255f, 25f / 255f, 183f / 255f);

                        //ShootTelegraphNew(Pos, violet, progress, -1);
                        ShootTelegraphOld(Pos, red * .75f, progress);
                    }
                }
                if (npc.type == NPCID.Spazmatism)
                {
                    if (npc.ai[0] == 0f && npc.ai[1] == 0 && npc.ai[2] < 540)
                    {
                        Vector2 halfSize = new(55f, 107f);
                        float num35 = 0f;
                        float num36 = Main.NPCAddHeight(npc);
                        Vector2 Pos = new(
                            npc.position.X - screenPos.X + npc.width / 2 - TextureAssets.Npc[npc.type].Width() * npc.scale / 2f + halfSize.X * npc.scale,
                            npc.position.Y - screenPos.Y + npc.height - TextureAssets.Npc[npc.type].Height() * npc.scale / Main.npcFrameCount[npc.type] + 4f + halfSize.Y * npc.scale + num36 + num35);
                        // spriteBatch.Draw(eyeGlow, Pos, npc.frame, color, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);
                        Pos += (Vector2.UnitX * TextureAssets.Npc[npc.type].Width() / 2).RotatedBy(npc.rotation + MathF.PI / 2);
                        Pos += screenPos;

                        float progress = Utils.GetLerpValue(10f, 60f, npc.ai[3], true);
                        ShootTelegraphNew(Pos, CursedFlamesLime, progress, 1);
                        //ShootTelegraphOld(Pos, CursedFlamesLime * 0.75f, progress);

                    }
                    //if (Main.masterMode && npc.ai[0] != 4f )
                    //{

                    //    if (npc.ai[1] == 2f || npc.ai[1] == 1f) // charge state
                    //    {
                    //        //if (npc.ai[0] == 0)
                    //        //{
                    //        //    int c = (int)npc.ai[3];
                    //        //    Color color = new(c, c, c, c);
                    //        //    Vector2 halfSize = new(55f, 107f);
                    //        //    float num35 = 0f;
                    //        //    float num36 = Main.NPCAddHeight(npc);

                    //        //    float progress = npc.ai[2] / phase1SpazChargeDuration;


                    //        //    float size = maxSize; // default to the max

                    //        //    if (npc.ai[3] == 0)
                    //        //    {

                    //        //        size = MathHelper.Lerp(minSize, maxSize, progress);
                    //        //    }
                    //        //    else if (npc.ai[3] == phase1SpazCharges)
                    //        //    {
                    //        //        // 0 -> 20: size 1.5 -> 1.0
                    //        //        size = MathHelper.Lerp(maxSize, minSize, progress);
                    //        //    }
                    //        //    int offSetShield = 2;
                    //        //    Vector2 pos = npc.Center + npc.rotation.ToRotationVector2() * offSetShield * npc.direction;
                    //        //    Lighting.AddLight(npc.Center, DustID.GreenTorch); 
                    //        //    //ImFuckingInvincible(pos, Color.LimeGreen, size * npc.scale);

                    //        //}
        


                    //    }

                    //} 
                }
            }
            base.PostDraw(npc, spriteBatch, screenPos, drawColor);
        }
        public static void LoadShaderIfNeeded()
        {
            if (tintShader == null)
            {
                tintShader = ModContent.Request<Effect>("TRAEProject/Changes/NPCs/Boss/TwinsChanges/ColorTint", AssetRequestMode.ImmediateLoad);
            }
        }
        public static void ModifyShaderParams(float normalizedTintAmount, Color colorToTintTowards)
        {
            LoadShaderIfNeeded();
            tintShader.Value.Parameters["normalizedTintAmount"].SetValue(normalizedTintAmount);
            tintShader.Value.Parameters["colorToTintTowards"].SetValue(colorToTintTowards.ToVector4());
        }
        public static void RestartSpritebatchForShaderDraw(SpriteBatch spriteBatch, float normalizedTintAmount, Color colorToTintTowards)
        {
            LoadShaderIfNeeded();
            tintShader.Value.Parameters["normalizedTintAmount"].SetValue(normalizedTintAmount);
            tintShader.Value.Parameters["colorToTintTowards"].SetValue(colorToTintTowards.ToVector4());
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, tintShader.Value, Main.Transform);
        }
        public static void RestartSpritebatchForVanillaDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            //if (npc.type == NPCID.Spazmatism)
            //{
            //    if (Main.masterMode)
            //    { 
            //        if ((npc.ai[1] == 2f || npc.ai[1] == 1f) && npc.ai[0] == 0 && npc.lifeRegen < 0)
            //        {
            //            npc.lifeRegen /= 10;
            //        }
            //    }
            //}
        }
    }
    public class TwinProjecileChanges : GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if (GetInstance<BossConfig>().TwinsRework && !Main.zenithWorld)
            {
                if (projectile.type == ProjectileID.DeathLaser)
                {
                    projectile.tileCollide = false;
                }
                if (projectile.type == ProjectileID.CursedFlameHostile)
                {
                    projectile.timeLeft = 240;
                }
            }
        }
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            //if (GetInstance<BossConfig>().TwinsRework && !Main.zenithWorld && Main.masterMode)
            //{
            //    if (projectile.type == ProjectileID.CursedFlameHostile)
            //    {
            //        NPC spaz = Main.npc[NPC.FindFirstNPC(NPCID.Spazmatism)];
            //        Player player = Main.player[spaz.target];
            //        projectile.velocity += player.velocity;
            //    }
            //}
        }
 
        public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
        {
            if (GetInstance<BossConfig>().TwinsRework && !Main.zenithWorld && Main.masterMode)
            {
                if (NPC.AnyNPCs(NPCID.Spazmatism) && (projectile.type == ProjectileID.CursedFlameHostile || projectile.type == ProjectileID.EyeFire))
                {
                    modifiers.SourceDamage *= 0.9f;  

                }
            }
        }
        
 
        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
            //if (GetInstance<BossConfig>().TwinsRework && !Main.zenithWorld && Main.masterMode)
            //{
            //    if (projectile.type == ProjectileID.CursedFlameHostile || projectile.type == ProjectileID.EyeFire)
            //    {
            //        int cursedInfernoID = target.FindBuffIndex(BuffID.CursedInferno);
            //        if (cursedInfernoID != -1)
            //        {
            //            target.buffTime[cursedInfernoID] += cursedInfernoIncreasedDurationOnMaster;
            //        }
            //        else
            //            target.AddBuff(BuffID.CursedInferno,  cursedInfernoIncreasedDurationOnMaster);
            //    }
            //}
        }
    }
}