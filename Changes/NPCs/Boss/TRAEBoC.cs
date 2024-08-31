using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NPCs.Boss
{
    public class BrainOfCthulhu : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void SetStaticDefaults()
        {
           
         }
        public override void SetDefaults(NPC npc)
        {
            if (npc.type == NPCID.Creeper)
            {
                npc.buffImmune[BuffID.Poisoned] = true;
                npc.buffImmune[BuffID.OnFire] = true;

                npc.lifeMax = 150; // up from 100
             }
            if (npc.type == NPCID.BrainofCthulhu)
            {
                npc.lifeMax = 1650; // up from 100
             }

        }
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (GetInstance<BossConfig>().BoCChanges)
            {
                if (projectile.type == ProjectileID.VampireFrog && npc.type == NPCID.BrainofCthulhu)
                    modifiers.FinalDamage *= 0.67f;
            }
        }
        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            if (GetInstance<BossConfig>().BoCChanges && npc.type == NPCID.BrainofCthulhu )
            {

               
                if (Main.masterMode)
                {
                    npc.localAI[1] = 500;
                    int buff = Main.rand.NextFromList(BuffID.Weak, BuffID.BrokenArmor, BuffID.Slow, BuffID.Bleeding);
                    int duration = Main.rand.Next(240, 360);
                    if (buff == BuffID.Slow)
                        duration = duration * 2 / 5;
                    if (buff == BuffID.Bleeding)
                        duration = duration * 4 / 3;
                    target.AddBuff(buff, duration);
                }
            }
        }

        
        public override bool PreAI(NPC npc)
        {
            if (GetInstance<BossConfig>().BoCChanges)
            {
                if (npc.type == NPCID.BrainofCthulhu)
                {
                    npc.knockBackResist = 0.45f;
                    NPC.crimsonBoss = npc.whoAmI;
                    if (Main.netMode != 1 && npc.localAI[0] == 0f)
                    {
                        npc.localAI[0] = 1f;
                        int brainOfCthuluCreepersCount = NPC.GetBrainOfCthuluCreepersCount(); // lowered from 40/20 to 30/15 (hopefully)
                        for (int num837 = 0; num837 < brainOfCthuluCreepersCount; num837++)
                        {
                            float x2 = npc.Center.X;
                            float y4 = npc.Center.Y;
                            x2 += (float)Main.rand.Next(-npc.width, npc.width);
                            y4 += (float)Main.rand.Next(-npc.height, npc.height);
                            int num838 = NPC.NewNPC(npc.GetSource_FromAI(), (int)x2, (int)y4, NPCID.Creeper);
                            Main.npc[num838].velocity = new Vector2((float)Main.rand.Next(-30, 31) * 0.1f, (float)Main.rand.Next(-30, 31) * 0.1f);
                            Main.npc[num838].netUpdate = true;
                        }
                    }
                    if (Main.netMode != 1)
                    {
                        // despawn it
                        npc.TargetClosest();
                        int num839 = 6000;
                        if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) + Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) > (float)num839)
                        {
                            npc.active = false;
                            npc.life = 0;
                            if (Main.netMode == 2)
                            {
                                NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
                            }
                        }
                    }
                    if (npc.ai[0] < 0f)
                    {

                        if (Main.getGoodWorld)
                        {
                            NPC.brainOfGravity = npc.whoAmI;
                        }
                        // phase 2 transition
                        if (npc.localAI[2] == 0f)
                        {
                            SoundEngine.PlaySound(SoundID.NPCHit9, npc.Center);
                            npc.localAI[2] = 1f;
                            Gore.NewGore(npc.GetSource_FromAI(), npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 392);
                            Gore.NewGore(npc.GetSource_FromAI(), npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 393);
                            Gore.NewGore(npc.GetSource_FromAI(), npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 394);
                            Gore.NewGore(npc.GetSource_FromAI(), npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 395);
                            for (int num840 = 0; num840 < 20; num840++)
                            {
                                Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
                            }
                            SoundEngine.PlaySound(SoundID.Roar, npc.Center);
                        }
                        // teleporting code
                        npc.dontTakeDamage = false;
                        npc.TargetClosest();
                        Vector2 vector104 = new Vector2(npc.Center.X, npc.Center.Y);
                        float num841 = Main.player[npc.target].Center.X - vector104.X;
                        float num842 = Main.player[npc.target].Center.Y - vector104.Y;
                        float num843 = (float)Math.Sqrt(num841 * num841 + num842 * num842);
                        float baseSpeed = 11.25f; // up from 8
                        if (Main.masterMode )
                        {
                            if (npc.life >= (int)(npc.lifeMax / 2))
                            {
                                npc.knockBackResist = 0f;

                            }
                            else
                            {
                                baseSpeed *= 1.1f;
                                if (npc.localAI[2] == 1f)
                                {
                                    SoundEngine.PlaySound(SoundID.ForceRoarPitched, npc.Center);
                                    npc.localAI[2] = 2f;
                                }
                            }
                        }
         
                        num843 = baseSpeed / num843;
                        num841 *= num843;
                        num842 *= num843;
                        npc.velocity.X = (npc.velocity.X * 50f + num841) / 51f;
                        npc.velocity.Y = (npc.velocity.Y * 50f + num842) / 51f;

                        if (npc.ai[0] == -1f)
                        {
                            if (Main.netMode != 1)
                            {
                                npc.localAI[1] += 1f;
                                //if (Main.masterMode && npc.life <= (int)(npc.lifeMax * 2))
                                //    npc.localAI[1] += 0.5f;
                                if (npc.justHit)
                                {
                                    npc.localAI[1] += Main.rand.Next(3);
                                }
                                int num845 = 60 + Main.rand.Next(120);
                                if (Main.netMode != 0)
                                {
                                    num845 += Main.rand.Next(30, 90);
                                }
                                if (npc.localAI[1] >= (float)num845)
                                {
                                    npc.localAI[1] = 0f;
                                    npc.TargetClosest();
                                    int teleportTime = 0;
                                    Player player2 = Main.player[npc.target];
                                    do
                                    {
                                        teleportTime++;
                                         int num847 = (int)player2.Center.X / 16;
                                        int num848 = (int)player2.Center.Y / 16;
                                        int minValue = 16;
                                        int maxValue = 16;
                                        float num850 = 16f;
                                        int num851 = Main.rand.Next(minValue, maxValue);
                                        int num852 = Main.rand.Next(minValue, maxValue);
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            num851 *= -1;
                                        }
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            num852 *= -1;
                                        }
                                        Vector2 v = new Vector2(num851 * 16, num852 * 16);
                                        if (Vector2.Dot(player2.velocity.SafeNormalize(Vector2.UnitY), v.SafeNormalize(Vector2.UnitY)) > 0f)
                                        {
                                            v += v.SafeNormalize(Vector2.Zero) * num850 * player2.velocity.Length();
                                        }
                                        num847 += (int)(v.X / 16f);
                                        num848 += (int)(v.Y / 16f);
                                        if (teleportTime > 100 || !WorldGen.SolidTile(num847, num848))
                                        {
                                            npc.ai[3] = 0f;
                                            npc.ai[0] = -2f;
                                            npc.ai[1] = num847;
                                            npc.ai[2] = num848;
                                            npc.netUpdate = true;
                                            npc.netSpam = 0;
                                            break;
                                        }
                                    }
                                    while (teleportTime <= 100);
                                }
                            }
                        }
                        else if (npc.ai[0] == -2f)
                        {
                            // teleport timer
                            npc.velocity *= 0.9f;
                            if (Main.netMode != 0)
                            {
                                npc.ai[3] += 15f;
                            }
                            else
                            {
                                npc.ai[3] += 25f;
                            }
                           
                            //if (Main.masterMode)

                            //{
                            //    if (npc.life <= (int)(npc.lifeMax * 2 / 3))
                            //    {

                            //        npc.ai[3] += 12.5f;
                            //    }
                            //}
 
                            // teleports when it reaches 255
                            if (npc.ai[3] >= 255f)
                            {
                                npc.ai[3] = 255f;
                                npc.position.X = npc.ai[1] * 16f - (float)(npc.width / 2);
                                npc.position.Y = npc.ai[2] * 16f - (float)(npc.height / 2);
                                SoundEngine.PlaySound(SoundID.Item8, npc.Center);
                                npc.ai[0] = -3f;
                                npc.netUpdate = true;
                                npc.netSpam = 0;
                            }
                            npc.alpha = (int)npc.ai[3];
                        }
                        else if (npc.ai[0] == -3f)
                        {
                            if (Main.netMode != 0)
                            {
                                npc.ai[3] -= 15f;
                            }
                            else
                            {
                                npc.ai[3] -= 25f;
                            }
                            if (npc.ai[3] <= 0f)
                            {
                                npc.ai[3] = 0f;
                                npc.ai[0] = -1f;
                                npc.netUpdate = true;
                                npc.netSpam = 0;
                            }
                            npc.alpha = (int)npc.ai[3];
                        }
                    }
                    else
                    {
                        // general movement
                        npc.TargetClosest();
                        Vector2 vector105 = new Vector2(npc.Center.X, npc.Center.Y);
                        float num853 = Main.player[npc.target].Center.X - vector105.X;
                        float num854 = Main.player[npc.target].Center.Y - vector105.Y;
                        float num855 = (float)Math.Sqrt(num853 * num853 + num854 * num854);
                        float phase1MoveSpeed = 1f; // up from 1
                        if (Main.getGoodWorld)
                        {
                            phase1MoveSpeed *= 3f;
                        }
                        if (num855 < phase1MoveSpeed)
                        {
                            npc.velocity.X = num853;
                            npc.velocity.Y = num854;
                        }
                        else
                        {
                            num855 = phase1MoveSpeed / num855;
                            npc.velocity.X = num853 * num855;
                            npc.velocity.Y = num854 * num855;
                        }
                        if (npc.ai[0] == 0f)
                        {
                            if (Main.netMode != 1)
                            {
                                int num857 = 0;
                                for (int num858 = 0; num858 < 200; num858++)
                                {
                                    if (Main.npc[num858].active && Main.npc[num858].type == 267)
                                    {
                                        num857++;
                                    }
                                }
                                if (num857 == 0)
                                {
                                    npc.ai[0] = -1f;
                                    npc.localAI[1] = 0f;
                                    npc.alpha = 0;
                                    npc.netUpdate = true;
                                }
                                npc.localAI[1] += 1f;
                                if (npc.localAI[1] >= (float)(120 + Main.rand.Next(300)))
                                {
                                    npc.localAI[1] = 0f;
                                    npc.TargetClosest();
                                    int num859 = 0;
                                    Player player3 = Main.player[npc.target];
                                    do
                                    {
                                        num859++;
                                        int num860 = (int)player3.Center.X / 16;
                                        int num861 = (int)player3.Center.Y / 16;
                                        int minValue2 = 14;
                                        int num862 = 40;
                                        float num863 = 16f;
                                        int num864 = Main.rand.Next(minValue2, num862 + 1);
                                        int num865 = Main.rand.Next(minValue2, num862 + 1);
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            num864 *= -1;
                                        }
                                        if (Main.rand.Next(2) == 0)
                                        {
                                            num865 *= -1;
                                        }
                                        Vector2 v2 = new Vector2(num864 * 16, num865 * 16);
                                        if (Vector2.Dot(player3.velocity.SafeNormalize(Vector2.UnitY), v2.SafeNormalize(Vector2.UnitY)) > 0f)
                                        {
                                            v2 += v2.SafeNormalize(Vector2.Zero) * num863 * player3.velocity.Length();
                                        }
                                        num860 += (int)(v2.X / 16f);
                                        num861 += (int)(v2.Y / 16f);
                                        if (num859 > 100 || (!WorldGen.SolidTile(num860, num861) && (num859 > 75 || Collision.CanHit(new Vector2(num860 * 16, num861 * 16), 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))))
                                        {
                                            npc.ai[0] = 1f;
                                            npc.ai[1] = num860;
                                            npc.ai[2] = num861;
                                            npc.netUpdate = true;
                                            break;
                                        }
                                    }
                                    while (num859 <= 100);
                                }
                            }
                        }
                        else if (npc.ai[0] == 1f)
                        {
                            npc.alpha += 5;
                            if (npc.alpha >= 255)
                            {
                                SoundEngine.PlaySound(SoundID.Item8, npc.Center);
                                npc.alpha = 255;
                                npc.position.X = npc.ai[1] * 16f - (float)(npc.width / 2);
                                npc.position.Y = npc.ai[2] * 16f - (float)(npc.height / 2);
                                npc.ai[0] = 2f;
                            }
                        }
                        else if (npc.ai[0] == 2f)
                        {
                            npc.alpha -= 5;
                            if (npc.alpha <= 0)
                            {
                                npc.alpha = 0;
                                npc.ai[0] = 0f;
                            }
                        }
                    }
                    if (Main.player[npc.target].dead || !Main.player[npc.target].ZoneCrimson)
                    {
                        if (npc.localAI[3] < 120f)
                        {
                            npc.localAI[3]++;
                        }
                        if (npc.localAI[3] > 60f)
                        {
                            npc.velocity.Y += (npc.localAI[3] - 60f) * 0.25f;
                        }
                        npc.ai[0] = 2f;
                        npc.alpha = 10;
                    }
                    else if (npc.localAI[3] > 0f)
                    {
                        npc.localAI[3]--;
                    }
                    return false;
                }
                if (npc.type == NPCID.Creeper)
                {
                    if (NPC.crimsonBoss < 0)
                    {
                        npc.active = false;
                        npc.netUpdate = true;
                        return false;
                    }
                    if (npc.ai[0] == 0f)
                    {
                        npc.ai[1] = 0f;
                        Vector2 vector106 = new Vector2(npc.Center.X, npc.Center.Y);
                        float num866 = Main.npc[NPC.crimsonBoss].Center.X - vector106.X;
                        float num867 = Main.npc[NPC.crimsonBoss].Center.Y - vector106.Y;
                        float creeperVel = (float)Math.Sqrt(num866 * num866 + num867 * num867);
                        if (creeperVel > 90f)
                        {
                            creeperVel = 8f / creeperVel; // vanilla value: 8
                            num866 *= creeperVel;
                            num867 *= creeperVel;
                            npc.velocity.X = (npc.velocity.X * 15f + num866) / 16f;
                            npc.velocity.Y = (npc.velocity.Y * 15f + num867) / 16f;
                            return false;
                        }
                        if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < 8f)
                        {
                            npc.velocity.Y *= 1.05f;
                            npc.velocity.X *= 1.05f;
                        }
                        if (Main.netMode != 1 && ((Main.expertMode && Main.rand.Next(80) == 0) || Main.rand.Next(160) == 0))
                        {
                            npc.TargetClosest();
                            vector106 = new Vector2(npc.Center.X, npc.Center.Y);
                            num866 = Main.player[npc.target].Center.X - vector106.X;
                            num867 = Main.player[npc.target].Center.Y - vector106.Y;
                            creeperVel = (float)Math.Sqrt(num866 * num866 + num867 * num867);
                            creeperVel = 8f / creeperVel;
                            npc.velocity.X = num866 * creeperVel;
                            npc.velocity.Y = num867 * creeperVel;
                            npc.ai[0] = 1f;
                            npc.netUpdate = true;
                        }
                        return false;
                    }
                    if (Main.expertMode)
                    {
                        Vector2 vector107 = Main.player[npc.target].Center - npc.Center;
                        vector107.Normalize();
                        if (Main.masterMode)
                        {
                            vector107 *= 12f;
                            npc.velocity = (npc.velocity * 39f + vector107) / 40f;
                        }
                        else if (Main.getGoodWorld)
                        {
                            vector107 *= 12f;
                            npc.velocity = (npc.velocity * 49f + vector107) / 50f;
                        }
                        else
                        {
                            vector107 *= 9f;
                            npc.velocity = (npc.velocity * 99f + vector107) / 100f;
                        }
                    }
                    Vector2 vector108 = new Vector2(npc.Center.X, npc.Center.Y);
                    float num869 = Main.npc[NPC.crimsonBoss].Center.X - vector108.X;
                    float num870 = Main.npc[NPC.crimsonBoss].Center.Y - vector108.Y;
                    float num871 = (float)Math.Sqrt(num869 * num869 + num870 * num870);
                    if (num871 > 700f)
                    {
                        npc.ai[0] = 0f;
                    }
                    else
                    {
                        if (!npc.justHit)
                        {
                            return false;
                        }
                        if (npc.knockBackResist == 0f)
                        {
                            npc.ai[1] += 1f;
                            if (npc.ai[1] > 5f)
                            {
                                npc.ai[0] = 0f;
                            }
                        }
                        else
                        {
                            npc.ai[0] = 0f;
                        }
                        return false;
                    }

                }
                
            }
            return true;
        }
    }
}