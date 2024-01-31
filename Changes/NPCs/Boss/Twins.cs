using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

using static Terraria.ModLoader.ModContent;
using TRAEProject.Changes.NPCs.Miniboss.Santa;
using MonoMod.Cil;

namespace TRAEProject.Changes.NPCs.Boss 
{
    public class Twins : GlobalNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NoMultiplayerSmoothingByType[NPCID.Retinazer] = true;
        }

        
        public override void SetDefaults(NPC npc)
        {
            if (GetInstance<TRAEConfig>().TwinsRework && !Main.zenithWorld)
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
        /*
        public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
        {
            if (GetInstance<TRAEConfig>().TwinsRework && !Main.masterMode)
            {

                switch (npc.type)
                {
                    case NPCID.Retinazer:
                        npc.lifeMax = (int)(npc.lifeMax * 20000 / 21000 * bossAdjustment);

                        break;
                    case NPCID.Spazmatism:
                        if (Main.expertMode)
                        {
                            npc.lifeMax = (int)(npc.lifeMax * 30000 / 34500 * bossAdjustment);
                        }
                        break;
                }
            }
        }
        */
        public static void Move(NPC npc)
        {
            int abs = (int)Math.Abs(npc.ai[3]);
            Vector2 goHere = Main.player[npc.target].Center + new Vector2(abs % 10 == 2 ? 25 : -250, abs / 10 == 2 ? 200 : -200);
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
                acceleration *= 10;
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
            if (Main.getGoodWorld)
            {
                topSpeed *= 1.15f;
                acceleration *= 1.15f;
            }
            else if(npc.ai[1] == 1f)
            {
                topSpeed = 5f;
                acceleration = 0.06f;
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
                    npc.velocity.Y += acceleration;
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
        }
        public override bool PreAI(NPC npc)
        {
            if (GetInstance<TRAEConfig>().TwinsRework && !Main.zenithWorld)
            {
                if (npc.type == NPCID.Retinazer)
                {
                    if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
                    {
                        npc.TargetClosest();
                    }
                    bool dead2 = Main.player[npc.target].dead;

                    float shootSpeed = 12f;
                     RetPhase3.Rotate(npc);
                    if (npc.ai[0] < 5)
                    {
                        float rotateTowards = TRAEMethods.PredictiveAimWithOffset(npc.Center, shootSpeed * 3, Main.player[npc.target].Center, Main.player[npc.target].velocity, npc.ai[1] == 0 ? 25 * 9 : 15 * 9) - MathF.PI / 2;
                        float rotSpeed = 0.1f;
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
                        int num402 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + (float)npc.height * 0.25f), npc.width, (int)((float)npc.height * 0.5f), 5, npc.velocity.X, 2f);
                        Main.dust[num402].velocity.X *= 0.5f;
                        Main.dust[num402].velocity.Y *= 0.1f;
                    }
                    if (Main.netMode != 1 && !Main.dayTime && !dead2 && npc.timeLeft < 10)
                    {
                        for (int num403 = 0; num403 < 200; num403++)
                        {
                            if (num403 != npc.whoAmI && Main.npc[num403].active && (Main.npc[num403].type == 125 || Main.npc[num403].type == 126))
                            {
                                npc.DiscourageDespawn(Main.npc[num403].timeLeft - 1);
                            }
                        }
                    }
                    if (Main.dayTime || dead2)
                    {
                        npc.velocity.Y -= 0.04f;
                        npc.EncourageDespawn(10);
                        return false;
                    }
                    if (npc.ai[0] == 4 || npc.ai[0] == 5)
                    {
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
                        Vector2 goHere = Main.player[npc.target].Center + new Vector2(side * 450, -300);
                        FlyTo(npc, goHere, false);
                        if (npc.ai[1] == 0f)
                        {
                            npc.ai[2] += 1f;
                            if (npc.ai[2] >= 600f)
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
                                if (npc.ai[3] >= 180f)
                                {
                                    npc.ai[3] = 0f;
                                    if (Main.netMode != 1)
                                    {
                                        int attackDamage_ForProjectiles3 = npc.GetAttackDamage_ForProjectiles(20f, 19f);
                                        int num413 = Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), npc.Center + TRAEMethods.PolarVector(15 * 9, npc.rotation + MathF.PI / 2), TRAEMethods.PolarVector(shootSpeed, npc.rotation + MathF.PI / 2), ProjectileID.EyeLaser, attackDamage_ForProjectiles3, 0f, Main.myPlayer);
                                    }
                                }
                            }
                        }
                        else if (npc.ai[1] == 1f)
                        {
                            //spin attack
                            npc.ai[2] += 1f;
                            if (npc.ai[2] >= 240f)
                            {
                                npc.ai[2] = 0;
                                npc.ai[1] = 0;
                            }
                            if (npc.ai[2] % 4 == 0)
                            {
                                if (Main.netMode != 1)
                                {
                                    int attackDamage_ForProjectiles3 = npc.GetAttackDamage_ForProjectiles(20f, 19f);
                                    int num413 = Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), npc.Center + TRAEMethods.PolarVector(15 * 9, npc.rotation + MathF.PI / 2), TRAEMethods.PolarVector(shootSpeed, npc.rotation + MathF.PI / 2), ProjectileID.EyeLaser, attackDamage_ForProjectiles3, 0f, Main.myPlayer);
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
                        else
                        {
                            float spazHealth = -1;
                            for (int spazIndex = 0; spazIndex < 200; spazIndex++)
                            {
                                if (Main.npc[spazIndex].active && Main.npc[spazIndex].type == NPCID.Spazmatism)
                                {
                                    spazHealth = Main.npc[spazIndex].life / (float)Main.npc[spazIndex].lifeMax;
                                    break;
                                }
                            }
                            if ((double)npc.life < (double)npc.lifeMax * 0.4 || (spazHealth != -1 && spazHealth < 0.05f))
                            {
                                npc.ai[0] = 1f;
                                npc.ai[1] = 0f;
                                npc.ai[2] = 0f;
                                npc.ai[3] = 0f;
                                npc.netUpdate = true;
                            }
                        }

                        return false;
                    }
                    if (npc.ai[0] == 1f || npc.ai[0] == 2f)
                    {
                        if (npc.ai[0] == 1f)
                        {
                            npc.ai[2] += 0.005f;
                            if ((double)npc.ai[2] > 0.5)
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
                                    Gore.NewGore(npc.GetSource_ReleaseEntity(), npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 143);
                                    Gore.NewGore(npc.GetSource_ReleaseEntity(), npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 7);
                                    Gore.NewGore(npc.GetSource_ReleaseEntity(), npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 6);
                                }
                                for (int num419 = 0; num419 < 20; num419++)
                                {
                                    Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
                                }
                                npc.localAI[2] = 450;
                                npc.localAI[3] = -300;
                                //SoundEngine.PlaySound(15, npc.Center);
                            }
                        }
                        Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
                        npc.velocity.X *= 0.98f;
                        npc.velocity.Y *= 0.98f;
                        if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
                        {
                            npc.velocity.X = 0f;
                        }
                        if ((double)npc.velocity.Y > -0.1 && (double)npc.velocity.Y < 0.1)
                        {
                            npc.velocity.Y = 0f;
                        }
                        return false;
                    }
                    npc.damage = (int)((double)npc.defDamage * 1.5);
                    npc.defense = npc.defDefense + 10;
                    npc.HitSound = SoundID.NPCHit4;
                    if (npc.ai[1] == 0f)
                    {
                        int fireRate = 120;
                        int shotsFired = 4;
                        int delayBeforeRapidFire = 60;
                        int RapidfireRate = 15;
                        int rapidShotsFired = 8;
                        npc.ai[2] += 1f;
 
                        if (npc.ai[2] > fireRate * shotsFired)
                        {

                            npc.velocity = Vector2.Zero;
                            if (npc.ai[2] >= fireRate * shotsFired + delayBeforeRapidFire && npc.ai[2] % RapidfireRate == 0)
                            {
                                if (Main.netMode != 1)
                                {
                                    Player target = Main.player[npc.target];
                                    Vector2 vector116 = new Vector2(npc.Center.X + (npc.direction * 50), npc.Center.Y + Main.rand.Next(35, 90));
                                    float num959 = target.Center.X - vector116.X + Main.rand.Next(-40, 41);
                                    float num960 = target.Center.Y - vector116.Y + Main.rand.Next(-40, 41);
                                    num959 += Main.rand.Next(-40, 41);
                                    num960 += Main.rand.Next(-40, 41);
                                    float num961 = MathF.Sqrt(num959 * num959 + num960 * num960);
                                    float num962 = 9f;
                                    num961 = num962 / num961;
                                    num959 *= num961;
                                    num960 *= num961;
                                    Vector2 pos = npc.Center + TRAEMethods.PolarVector(25 * 9, npc.rotation + MathF.PI / 2);
                                    int attackDamage_ForProjectiles3 = npc.GetAttackDamage_ForProjectiles(25f, 23f);
                                    Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), pos.X, pos.Y, num959, num960, ProjectileID.DeathLaser, attackDamage_ForProjectiles3, 0f, Main.myPlayer);
                                }
                            }
                            if (npc.ai[2] >= fireRate * shotsFired + delayBeforeRapidFire + RapidfireRate * rapidShotsFired)
                            {
                                npc.ai[2] = 0;
                                npc.netUpdate = true;
                            }
                        }
                        else
                        {
                            //TRAEMethods.ServerClientCheck(npc.localAI[2] + ", " + npc.localAI[3]);
                            Move(npc);

                            if (npc.ai[2] % fireRate == 0)
                            {
                                if (Main.netMode != NetmodeID.MultiplayerClient)
                                {
                                    int attackDamage_ForProjectiles3 = npc.GetAttackDamage_ForProjectiles(25f, 23f);
                                    int num413 = Projectile.NewProjectile(npc.GetSource_ReleaseEntity(), npc.Center + TRAEMethods.PolarVector(25 * 9, npc.rotation + MathF.PI / 2), TRAEMethods.PolarVector(shootSpeed, npc.rotation + MathF.PI / 2), ProjectileID.DeathLaser, attackDamage_ForProjectiles3, 0f, Main.myPlayer);
                                }
                            }
                        }
                        if (NPC.CountNPCS(NPCID.Spazmatism) <= 0 && Main.expertMode)
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
                    if (npc.ai[0] >= 4f)
                    {

                        if (npc.ai[0] == 4f || npc.ai[0] == 5f)
                        {
                            SpazPhase3.Start(npc);
                        }
                        else
                        {
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
            if (GetInstance<TRAEConfig>().TwinsRework && !Main.zenithWorld)
            {
                if (npc.type == NPCID.Spazmatism)
                {   // shoot fireballs slower
                    if (npc.ai[0] == 0f)
                    {
                        if (npc.ai[1] == 0f)
                        {
                            if (npc.ai[2] <= 600)
                            {
                                if (Main.expertMode && (double)npc.life < (double)npc.lifeMax * 0.8)
                                {
                                    npc.ai[3] -= 0.6f; // goes up 1.6 on expert mode when below 80% hp... let me just get rid of that real quick
                                }
                                if (!Main.player[npc.target].dead)
                                {
                                    npc.ai[3] -= 0.25f; // goes up by 1 normally, so 25% slower
                                }
                            }
                        }
                    }
                    //
                    if (Main.expertMode)
                    {
                        if (NPC.CountNPCS(NPCID.Retinazer) <= 0 && npc.ai[0] < 4 && Main.expertMode)
                        {
                            npc.ai[0] = 4f;
                            npc.ai[1] = 0f;
                            npc.ai[2] = 0f;
                            npc.ai[3] = 0f;
                            npc.netUpdate = true;
                        }
                        if (npc.ai[1] == 0f && npc.ai[0] != 1f && npc.ai[0] != 2f && npc.ai[0] != 0f)
                        {
                            npc.defense = 28;
                            if (npc.ai[1] == 0f)
                            {
                                float speed = 2.4f;
                                int num425 = 1;
                                if (npc.position.X + (float)(npc.width / 2) < Main.player[npc.target].position.X + (float)Main.player[npc.target].width)
                                {
                                    num425 = -1;
                                }
                                Vector2 spazposition = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                                float playerpositionX = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) + (float)(num425 * 180) - spazposition.X;
                                float playerpositionY = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - spazposition.Y;
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

                                speed *= 2f;							float accel = 0.2f;

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
                                        if ((double)npc.life < (double)npc.lifeMax * 0.75)
                                        {
                                            ref float x10 = ref npc.localAI[1];
                                            x10 += 1f;
                                        }
                                        if ((double)npc.life < (double)npc.lifeMax * 0.5)
                                        {
                                            ref float y1 = ref npc.localAI[1];
                                            y1 += 1f;
                                        }
                                        if ((double)npc.life < (double)npc.lifeMax * 0.25)
                                        {
                                            ref float y2 = ref npc.localAI[1];
                                            y2 += 1f;
                                        }
                                        if ((double)npc.life < (double)npc.lifeMax * 0.1)
                                        {
                                            ref float y3 = ref npc.localAI[1];
                                            y3 += 2f;
                                        }
                                        if (npc.soundDelay <= 0)
                                        {
                                            Terraria.Audio.SoundEngine.PlaySound(SoundID.ForceRoarPitched with { MaxInstances = 0 }, npc.Center);
                                            npc.soundDelay = 240;
                                        }
                                        if (npc.localAI[1] > 8f)
                                        {



                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (npc.ai[0] == 0)
                    {
                        float retHealth = -1;
                        for (int retIndex = 0; retIndex < 200; retIndex++)
                        {
                            if (Main.npc[retIndex].active && Main.npc[retIndex].type == NPCID.Retinazer)
                            {
                                retHealth = (float)Main.npc[retIndex].life / (float)Main.npc[retIndex].lifeMax;
                                break;
                            }
                        }
                        if ((double)npc.life < (double)npc.lifeMax * 0.4 || (retHealth != -1 && retHealth < 0.05f))
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
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (GetInstance<TRAEConfig>().TwinsRework && !Main.zenithWorld)
            {
 
                if (npc.type == NPCID.Retinazer && npc.ai[0] >= 4f)
                {
                    RetPhase3.Phase3Draw(npc, spriteBatch, screenPos, drawColor);
                    return true;
                }
                if (npc.type == NPCID.Spazmatism && npc.ai[0] >= 4f)
                {
                    SpazPhase3.Phase3Draw(npc, spriteBatch, screenPos, drawColor);
                    return true;
                }
            }
            
            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }
        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (GetInstance<TRAEConfig>().TwinsRework && !Main.zenithWorld)
            {
                if (npc.type == NPCID.Retinazer)
                {
                    if (npc.ai[0] == 0f && npc.ai[1] == 0)
                    {
                        Texture2D eyeGlow = ModContent.Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/Retinizer_Glow").Value;
                        int c = (int)npc.ai[3];
                        Color color = new Color(c, c, c, c);
                        Vector2 halfSize = new Vector2(55f, 107f);
                        float num35 = 0f;
                        float num36 = Main.NPCAddHeight(npc);
                        Vector2 Pos = new Vector2(
                            npc.position.X - screenPos.X + (float)(npc.width / 2) - (float)TextureAssets.Npc[npc.type].Width() * npc.scale / 2f + halfSize.X * npc.scale,
                            npc.position.Y - screenPos.Y + (float)npc.height - (float)TextureAssets.Npc[npc.type].Height() * npc.scale / (float)Main.npcFrameCount[npc.type] + 4f + halfSize.Y * npc.scale + num36 + num35);
                        spriteBatch.Draw(eyeGlow, Pos, npc.frame, color, npc.rotation, halfSize, npc.scale, SpriteEffects.None, 0f);
                    }
                }
            }
            base.PostDraw(npc, spriteBatch, screenPos, drawColor);
        }
    }
    public class TwinProjecileChanges: GlobalProjectile
    {
        public override void SetDefaults(Projectile projectile)
        {
            if (GetInstance<TRAEConfig>().TwinsRework && !Main.zenithWorld)
            {
                if (projectile.type == ProjectileID.DeathLaser)
                {
                    projectile.tileCollide = false;
                }
            }
        }
    }
}