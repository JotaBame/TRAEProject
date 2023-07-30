using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace TRAEProject.NPCs.Boss
{

	public class EyeOfCthlhu : GlobalNPC
	{
		public override bool InstancePerEntity => true;
        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.masterMode && npc.type == NPCID.ServantofCthulhu && Main.rand.NextBool(3))
            {
                target.AddBuff(BuffID.Bleeding, Main.rand.Next(200, 300)); // ends up being 5-6 seconds
            }
        }
        public override bool PreAI(NPC npc)
		{
			if (GetInstance<TRAEConfig>().EOCChanges)
			{
                if (npc.type == NPCID.EyeofCthulhu)
                {
                    bool lowOnHealth = false;
                    if (Main.expertMode && npc.life < (float)(npc.lifeMax * 0.12f))
                    {
                        lowOnHealth = true;
                    }
                    bool veryLowOnHealth = false;
                    if (Main.expertMode && npc.life < (float)(npc.lifeMax * 0.04f))
                    {
                        veryLowOnHealth = true;
                    }
                    float madDashLengthInFrames = 20f;
                    if (veryLowOnHealth)
                    {
                        madDashLengthInFrames = 10f; // up from 10
                    }
                    if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
                    {
                        npc.TargetClosest();
                    }
                    bool dead = Main.player[npc.target].dead;
                    float distanceX = npc.position.X + (float)(npc.width / 2) - Main.player[npc.target].position.X - (float)(Main.player[npc.target].width / 2);
                    float distanceY = npc.position.Y + (float)npc.height - 59f - Main.player[npc.target].position.Y - (float)(Main.player[npc.target].height / 2);
                    float distance = (float)Math.Atan2(distanceY, distanceX) + 1.57f;
                    if (distance < 0f)
                    {
                        distance += 6.283f;
                    }
                    else if ((double)distance > 6.283)
                    {
                        distance -= 6.283f;
                    }
                    float num8 = 0f;
                    if (npc.ai[0] == 0f && npc.ai[1] == 0f)
                    {
                        num8 = 0.02f;
                    }
                    if (npc.ai[0] == 0f && npc.ai[1] == 2f && npc.ai[2] > 40f)
                    {
                        num8 = 0.05f;
                    }
                    if (npc.ai[0] == 3f && npc.ai[1] == 0f)
                    {
                        num8 = 0.05f;
                    }
                    if (npc.ai[0] == 3f && npc.ai[1] == 2f && npc.ai[2] > 40f)
                    {
                        num8 = 0.08f;
                    }
                    if (npc.ai[0] == 3f && npc.ai[1] == 4f && npc.ai[2] > madDashLengthInFrames)
                    {
                        num8 = 0.15f;
                    }
                    if (npc.ai[0] == 3f && npc.ai[1] == 5f)
                    {
                        num8 = 0.05f;
                    }
                    if (Main.expertMode)
                    {
                        num8 *= 1.5f;
                    }
                    if (veryLowOnHealth && Main.expertMode)
                    {
                        num8 = 0f;
                    }
                    if (npc.rotation < distance)
                    {
                        if ((double)(distance - npc.rotation) > 3.1415)
                        {
                            npc.rotation -= num8;
                        }
                        else
                        {
                            npc.rotation += num8;
                        }
                    }
                    else if (npc.rotation > distance)
                    {
                        if ((double)(npc.rotation - distance) > 3.1415)
                        {
                            npc.rotation += num8;
                        }
                        else
                        {
                            npc.rotation -= num8;
                        }
                    }
                    if (npc.rotation > distance - num8 && npc.rotation < distance + num8)
                    {
                        npc.rotation = distance;
                    }
                    if (npc.rotation < 0f)
                    {
                        npc.rotation += 6.283f;
                    }
                    else if ((double)npc.rotation > 6.283)
                    {
                       npc.rotation -= 6.283f;
                    }
                    if (npc.rotation > distance - num8 &&npc.rotation < distance + num8)
                    {
                       npc.rotation = distance;
                    }
                    if (Main.rand.NextBool(5))
                    {
                        int num9 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + (float)npc.height * 0.25f), npc.width, (int)((float)npc.height * 0.5f), 5, npc.velocity.X, 2f);
                        Main.dust[num9].velocity.X *= 0.5f;
                        Main.dust[num9].velocity.Y *= 0.1f;
                    }
                    npc.reflectsProjectiles = false;
                    if (Main.IsItDay() || dead)
                    {
                        npc.velocity.Y -= 0.04f;
                        npc.EncourageDespawn(10);
                        return false;
                    }
                    if (npc.ai[0] == 0f)
                    {
                        if (npc.ai[1] == 0f)
                        {
                            float num10 = 5f;
                            float num11 = 0.04f;
                            if (Main.expertMode)
                            {
                                num11 = 0.15f;
                                num10 = 7f;
                            }
                            if (Main.getGoodWorld)
                            {
                                num11 += 0.05f;
                                num10 += 1f;
                            }
                            Vector2 vector = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                            float num12 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector.X;
                            float num13 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - 200f - vector.Y;
                            float num14 = (float)Math.Sqrt(num12 * num12 + num13 * num13);
                            float num15 = num14;
                            num14 = num10 / num14;
                            num12 *= num14;
                            num13 *= num14;
                            if (npc.velocity.X < num12)
                            {
                                npc.velocity.X += num11;
                                if (npc.velocity.X < 0f && num12 > 0f)
                                {
                                    npc.velocity.X += num11;
                                }
                            }
                            else if (npc.velocity.X > num12)
                            {
                                npc.velocity.X -= num11;
                                if (npc.velocity.X > 0f && num12 < 0f)
                                {
                                    npc.velocity.X -= num11;
                                }
                            }
                            if (npc.velocity.Y < num13)
                            {
                                npc.velocity.Y += num11;
                                if (npc.velocity.Y < 0f && num13 > 0f)
                                {
                                    npc.velocity.Y += num11;
                                }
                            }
                            else if (npc.velocity.Y > num13)
                            {
                                npc.velocity.Y -= num11;
                                if (npc.velocity.Y > 0f && num13 < 0f)
                                {
                                    npc.velocity.Y -= num11;
                                }
                            }
                            npc.ai[2] += 1f;
                            float num16 = 600f;
                            if (Main.expertMode)
                            {
                                num16 *= 0.35f;
                            }

                            if (npc.ai[2] >= num16)
                            {
                                npc.ai[1] = 1f;
                                npc.ai[2] = 0f;
                                npc.ai[3] = 0f;
                                npc.target = 255;
                                npc.netUpdate = true;
                            }
                            else if ((npc.position.Y + (float)npc.height < Main.player[npc.target].position.Y && num15 < 500f) || (Main.expertMode && num15 < 500f))
                            {
                                if (!Main.player[npc.target].dead)
                                {
                                    npc.ai[3] += 1f;
                                }
                                float num17 = 110f;
                                if (Main.expertMode)
                                {
                                    num17 *= 0.4f;
                                }
                                if (Main.getGoodWorld)
                                {
                                    num17 *= 0.8f;
                                }
                                if (npc.ai[3] >= num17)
                                {
                                    npc.ai[3] = 0f;
                                   npc.rotation = distance;
                                    float num18 = 5f;
                                    if (Main.expertMode)
                                    {
                                        num18 = 6f;
                                    }
                                    float num19 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector.X;
                                    float num20 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector.Y;
                                    float num21 = (float)Math.Sqrt(num19 * num19 + num20 * num20);
                                    num21 = num18 / num21;
                                    Vector2 vector2 = vector;
                                    Vector2 vector3 = default(Vector2);
                                    vector3.X = num19 * num21;
                                    vector3.Y = num20 * num21;
                                    vector2.X += vector3.X * 10f;
                                    vector2.Y += vector3.Y * 10f;
                                    if (Main.netMode != 1)
                                    {
                                        SoundEngine.PlaySound(SoundID.NPCHit1, npc.Center);

                                        int num22 = NPC.NewNPC(npc.GetSource_FromAI(), (int)vector2.X, (int)vector2.Y, 5);
                                        Main.npc[num22].velocity.X = vector3.X;
                                        Main.npc[num22].velocity.Y = vector3.Y;
                                        if (Main.netMode == 2 && num22 < 200)
                                        {
                                            NetMessage.SendData(23, -1, -1, null, num22);
                                        }
                                    }
                                    for (int m = 0; m < 10; m++)
                                    {
                                        Dust.NewDust(vector2, 20, 20, 5, vector3.X * 0.4f, vector3.Y * 0.4f);
                                    }
                                }
                            }
                        }
                        else if (npc.ai[1] == 1f)
                        {
                           npc.rotation = distance;
                            float velocity = 7.5f; // up from 6
                           
                            if (Main.expertMode)
                            {
                                velocity *= 1.15f; // before it increased this by 1
                            }
                            if (Main.masterMode)
                            {
                                velocity *= 1.25f; // before it increased this by 1
                            }
                            if (Main.getGoodWorld)
                            {
                                velocity *= 1.15f; // before it increased this by 1
                            }
                            Vector2 vector4 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                            float num24 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector4.X;
                            float num25 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector4.Y;
                            float num26 = (float)Math.Sqrt(num24 * num24 + num25 * num25);
                            num26 = velocity / num26;
                            npc.velocity.X = num24 * num26;
                            npc.velocity.Y = num25 * num26;
                            npc.ai[1] = 2f;
                            npc.netUpdate = true;
                            if (npc.netSpam > 10)
                            {
                                npc.netSpam = 10;
                            }
                        }
                        else if (npc.ai[1] == 2f)
                        {
                            npc.ai[2] += 1f;
           
                            if (npc.ai[2] >= 30f)
                            {
                                npc.velocity *= 0.98f; // 
                                if (Main.expertMode)
                                {
                                    npc.velocity *= 0.985f;
                                }
                                if (Main.getGoodWorld)
                                {
                                    npc.velocity *= 0.99f;
                                }
                                if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
                                {
                                    npc.velocity.X = 0f;
                                }
                                if ((double)npc.velocity.Y > -0.1 && (double)npc.velocity.Y < 0.1)
                                {
                                    npc.velocity.Y = 0f;
                                }
                            }
                            else
                            {
                               npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - 1.57f;
                            }
                            int timebetweenattacks = 150;
                            if (Main.expertMode)
                            {
                                timebetweenattacks = 100;
                            }
                            if (Main.getGoodWorld)
                            {
                                timebetweenattacks -= 15;
                            }
                            if (npc.ai[2] >= (float)timebetweenattacks)
                            {
                                npc.ai[3] += 1f;
                                npc.ai[2] = 0f;
                                npc.target = 255;
                               npc.rotation = distance;
                                if (npc.ai[3] >= 3f)
                                {
                                    npc.ai[1] = 0f;
                                    npc.ai[3] = 0f;
                                }
                                else
                                {
                                    npc.ai[1] = 1f;
                                }
                            }
                        }
                        float num28 = 0.5f;
                        if (Main.expertMode)
                        {
                            num28 = 0.65f;
                        }
                        if ((float)npc.life < (float)npc.lifeMax * num28)
                        {
                            npc.ai[0] = 1f;
                            npc.ai[1] = 0f;
                            npc.ai[2] = 0f;
                            npc.ai[3] = 0f;
                            npc.netUpdate = true;
                            if (npc.netSpam > 10)
                            {
                                npc.netSpam = 10;
                            }
                        }
                        return false;
                    }
                    if (npc.ai[0] == 1f || npc.ai[0] == 2f)
                    {
                        if (npc.life < (float)(npc.lifeMax * 0.5f))
                             npc.velocity *= 0.9f;
                        if (npc.ai[0] == 1f || npc.ai[3] == 1f)
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
                        if (Main.masterMode && npc.life < npc.lifeMax / 2)
                            npc.defense = 18;

                        if (Main.getGoodWorld)
                        {
                            npc.reflectsProjectiles = true;
                        }
                        int eyeSpawnTimer = 20;
                        if (Main.getGoodWorld && npc.life < npc.lifeMax / 3)
                        {
                            eyeSpawnTimer = 10;
                        }
                        if (Main.expertMode && npc.ai[1] % (float)eyeSpawnTimer == 0f)
                        {
                            float num30 = 5f;
                            Vector2 vector5 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                            float num31 = Main.rand.Next(-200, 200);
                            float num32 = Main.rand.Next(-200, 200);
                            if (Main.getGoodWorld)
                            {
                                num31 *= 3f;
                                num32 *= 3f;
                            }
                            float num33 = (float)Math.Sqrt(num31 * num31 + num32 * num32);
                            num33 = num30 / num33;
                            Vector2 vector6 = vector5;
                            Vector2 vector7 = default(Vector2);
                            vector7.X = num31 * num33;
                            vector7.Y = num32 * num33;
                            vector6.X += vector7.X * 10f;
                            vector6.Y += vector7.Y * 10f;
                            if (Main.netMode != 1)
                            {
                                int num34 = NPC.NewNPC(npc.GetSource_FromAI(), (int)vector6.X, (int)vector6.Y, 5);
                                Main.npc[num34].velocity.X = vector7.X;
                                Main.npc[num34].velocity.Y = vector7.Y;
                                if (Main.netMode == 2 && num34 < 200)
                                {
                                    NetMessage.SendData(23, -1, -1, null, num34);
                                }
                            }
                            for (int n = 0; n < 10; n++)
                            {
                                Dust.NewDust(vector6, 20, 20, 5, vector7.X * 0.4f, vector7.Y * 0.4f);
                            }
                        }
                        if (npc.ai[1] >= 100f)
                        {
                            if (npc.ai[3] == 1f)
                            {
                                npc.ai[3] = 0f;
                                npc.ai[1] = 0f;
                            }
                            else
                            {
                                npc.ai[0] += 1f;
                                npc.ai[1] = 0f;
                                if (npc.ai[0] == 3f)
                                {
                                    npc.ai[2] = 0f;
                                }
                                else
                                {
                                    SoundEngine.PlaySound(SoundID.NPCHit1, npc.Center);
                                    for (int num35 = 0; num35 < 2; num35++)
                                    {
                                        Gore.NewGore(npc.GetSource_FromAI(), npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 8);
                                        Gore.NewGore(npc.GetSource_FromAI(), npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 7);
                                        Gore.NewGore(npc.GetSource_FromAI(), npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 6);
                                    }
                                    for (int num36 = 0; num36 < 20; num36++)
                                    {
                                        Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
                                    }
                                    SoundEngine.PlaySound(SoundID.Roar, npc.Center);
                                }
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
					npc.defense = 0;
                    int num37 = 23;
                    int expertDamage = 23;
					if (Main.masterMode)
						npc.defense = 5;
                    npc.damage = npc.GetAttackDamage_LerpBetweenFinalValues(num37, expertDamage);
                    npc.damage = npc.GetAttackDamage_ScaledByStrength(npc.damage);
                    if (npc.ai[1] == 0f && lowOnHealth)
                    {
                        npc.ai[1] = 5f;
                    }
                    if (npc.ai[1] == 0f)
                    {
                        float num39 = 6f;
                        float num40 = 0.07f;
                        Vector2 vector8 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                        float num41 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector8.X;
                        float num42 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - 120f - vector8.Y;
                        float num43 = (float)Math.Sqrt(num41 * num41 + num42 * num42);
                        if (num43 > 400f && Main.expertMode)
                        {
                            num39 += 1f;
                            num40 += 0.05f;
                            if (num43 > 600f)
                            {
                                num39 += 1f;
                                num40 += 0.05f;
                                if (num43 > 800f)
                                {
                                    num39 += 1f;
                                    num40 += 0.05f;
                                }
                            }
                        }
                        if (Main.getGoodWorld)
                        {
                            num39 += 1f;
                            num40 += 0.1f;
                        }
                        num43 = num39 / num43;
                        num41 *= num43;
                        num42 *= num43;
                        if (npc.velocity.X < num41)
                        {
                            npc.velocity.X += num40;
                            if (npc.velocity.X < 0f && num41 > 0f)
                            {
                                npc.velocity.X += num40;
                            }
                        }
                        else if (npc.velocity.X > num41)
                        {
                            npc.velocity.X -= num40;
                            if (npc.velocity.X > 0f && num41 < 0f)
                            {
                                npc.velocity.X -= num40;
                            }
                        }
                        if (npc.velocity.Y < num42)
                        {
                            npc.velocity.Y += num40;
                            if (npc.velocity.Y < 0f && num42 > 0f)
                            {
                                npc.velocity.Y += num40;
                            }
                        }
                        else if (npc.velocity.Y > num42)
                        {
                            npc.velocity.Y -= num40;
                            if (npc.velocity.Y > 0f && num42 < 0f)
                            {
                                npc.velocity.Y -= num40;
                            }
                        }
                        npc.ai[2] += 1f;
                        if (npc.ai[2] >= 200f)
                        {
                            npc.ai[1] = 1f;
                            npc.ai[2] = 0f;
                            npc.ai[3] = 0f;
                            if (Main.expertMode && (double)npc.life < (double)npc.lifeMax * 0.35)
                            {
                                npc.ai[1] = 3f;
                            }
                            npc.target = 255;
                            npc.netUpdate = true;
                        }
                        if (Main.expertMode && veryLowOnHealth)
                        {
                            npc.TargetClosest();
                            npc.netUpdate = true;
                            npc.ai[1] = 3f;
                            npc.ai[2] = 0f;
                            npc.ai[3] -= 1000f;
                        }
                    }
                    else if (npc.ai[1] == 1f)
                    {
                        SoundEngine.PlaySound(SoundID.Roar, npc.Center);
                       npc.rotation = distance;
                        float num44 = 8.5f; // up from 6.8
                        if (Main.expertMode && npc.ai[3] == 1f)
                        {
                            num44 *= 1.15f;
                        }
                        if (Main.expertMode && npc.ai[3] == 2f)
                        {
                            num44 *= 1.3f;
                        }
                        if (Main.masterMode)
                            num44 *= 1.5f;
                        if (Main.getGoodWorld)
                        {
                            num44 *= 1.2f;
                        }
                        Vector2 vector9 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                        float num45 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector9.X;
                        float num46 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector9.Y;
                        float num47 = (float)Math.Sqrt(num45 * num45 + num46 * num46);
                        num47 = num44 / num47;
                        npc.velocity.X = num45 * num47;
                        npc.velocity.Y = num46 * num47;
                        npc.ai[1] = 2f;
                        npc.netUpdate = true;
                        if (npc.netSpam > 10)
                        {
                            npc.netSpam = 10;
                        }
                    }
                    else if (npc.ai[1] == 2f)
                    {
                        float chargeLengthInFrames = 30f;
                        npc.ai[2] += 1f;
                        if (Main.expertMode)
                        {
                            chargeLengthInFrames = 40f;
                        }
                        if (Main.masterMode)
                        {
                            chargeLengthInFrames = 24f;
                        }
                        if (npc.ai[2] >= chargeLengthInFrames)
                        {
                            npc.velocity *= 0.97f;
                            if (Main.expertMode)
                            {
                                npc.velocity *= 0.98f;
                            }
                            if (Main.masterMode)
                            {
                                npc.velocity *= 0.99f;
                            }
                            if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
                            {
                                npc.velocity.X = 0f;
                            }
                            if ((double)npc.velocity.Y > -0.1 && (double)npc.velocity.Y < 0.1)
                            {
                                npc.velocity.Y = 0f;
                            }
                        }
                        else
                        {
                           npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - 1.57f;
                        }
                        int num49 = 130;
                        if (Main.expertMode)
                        {
                            num49 = 90;
                        }
                        if (Main.masterMode)
                        {
                            num49 = 70;
                        }
                        if (npc.ai[2] >= (float)num49)
                        {
                            npc.ai[3] += 1f;
                            npc.ai[2] = 0f;
                            npc.target = 255;
                           npc.rotation = distance;
                            if (npc.ai[3] >= 3f)
                            {
                                npc.ai[1] = 0f;
                                npc.ai[3] = 0f;
                                if (Main.expertMode && Main.netMode != 1 && (double)npc.life < (double)npc.lifeMax * 0.5)
                                {
                                    npc.ai[1] = 3f;
                                    npc.ai[3] += Main.rand.Next(1, 4);
                                }
                                npc.netUpdate = true;
                                if (npc.netSpam > 10)
                                {
                                    npc.netSpam = 10;
                                }
                            }
                            else
                            {
                                npc.ai[1] = 1f;
                            }
                        }
                    }
                    else if (npc.ai[1] == 3f)
                    {
                        if (npc.ai[3] == 4f && lowOnHealth && npc.Center.Y > Main.player[npc.target].Center.Y)
                        {
                            npc.TargetClosest();
                            npc.ai[1] = 0f;
                            npc.ai[2] = 0f;
                            npc.ai[3] = 0f;
                            npc.netUpdate = true;
                            if (npc.netSpam > 10)
                            {
                                npc.netSpam = 10;
                            }
                        }
                        else if (Main.netMode != 1)
                        {
                            npc.TargetClosest();
                            float num50 = 20f; 
                            Vector2 vector10 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                            float num51 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector10.X;
                            float num52 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector10.Y;
                            float num53 = Math.Abs(Main.player[npc.target].velocity.X) + Math.Abs(Main.player[npc.target].velocity.Y) / 4f;
                            num53 += 10f - num53;
                            if (num53 < 5f)
                            {
                                num53 = 5f;
                            }
                            if (num53 > 15f)
                            {
                                num53 = 15f;
                            }
                            if (npc.ai[2] == -1f && !veryLowOnHealth)
                            {
                                num53 *= 4f;
                                num50 *= 1.3f;
                            }
                            if (veryLowOnHealth)
                            {
                                num53 *= 2f;
                            }
                            num51 -= Main.player[npc.target].velocity.X * num53;
                            num52 -= Main.player[npc.target].velocity.Y * num53 / 4f;
                            num51 *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                            num52 *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                            if (veryLowOnHealth)
                            {
                                num51 *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                                num52 *= 1f + Main.rand.Next(-10, 11) * 0.01f;
                            }
                            float num54 = (float)Math.Sqrt(num51 * num51 + num52 * num52);
                            float num55 = num54;
                            num54 = num50 / num54;
                            npc.velocity.X = num51 * num54;
                            npc.velocity.Y = num52 * num54;
                            npc.velocity.X += Main.rand.Next(-20, 21) * 0.1f;
                            npc.velocity.Y += Main.rand.Next(-20, 21) * 0.1f;
                            if (veryLowOnHealth)
                            {
                                npc.velocity.X += Main.rand.Next(-50, 51) * 0.1f;
                                npc.velocity.Y += Main.rand.Next(-50, 51) * 0.1f;
                                float num56 = Math.Abs(npc.velocity.X);
                                float num57 = Math.Abs(npc.velocity.Y);
                                if (npc.Center.X > Main.player[npc.target].Center.X)
                                {
                                    num57 *= -1f;
                                }
                                if (npc.Center.Y > Main.player[npc.target].Center.Y)
                                {
                                    num56 *= -1f;
                                }
                                npc.velocity.X = num57 + npc.velocity.X;
                                npc.velocity.Y = num56 + npc.velocity.Y;
                                npc.velocity.Normalize();
                                npc.velocity *= num50;
                                npc.velocity.X += Main.rand.Next(-20, 21) * 0.1f;
                                npc.velocity.Y += Main.rand.Next(-20, 21) * 0.1f;
                            }
                            else if (num55 < 100f)
                            {
                                if (Math.Abs(npc.velocity.X) > Math.Abs(npc.velocity.Y))
                                {
                                    float num58 = Math.Abs(npc.velocity.X);
                                    float num59 = Math.Abs(npc.velocity.Y);
                                    if (npc.Center.X > Main.player[npc.target].Center.X)
                                    {
                                        num59 *= -1f;
                                    }
                                    if (npc.Center.Y > Main.player[npc.target].Center.Y)
                                    {
                                        num58 *= -1f;
                                    }
                                    npc.velocity.X = num59;
                                    npc.velocity.Y = num58;
                                }
                            }
                            else if (Math.Abs(npc.velocity.X) > Math.Abs(npc.velocity.Y))
                            {
                                float num60 = (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) / 2f;
                                float num61 = num60;
                                if (npc.Center.X > Main.player[npc.target].Center.X)
                                {
                                    num61 *= -1f;
                                }
                                if (npc.Center.Y > Main.player[npc.target].Center.Y)
                                {
                                    num60 *= -1f;
                                }
                                npc.velocity.X = num61;
                                npc.velocity.Y = num60;
                            }
                            npc.ai[1] = 4f;
                            npc.netUpdate = true;
                            if (npc.netSpam > 10)
                            {
                                npc.netSpam = 10;
                            }
                        }
                    }
                    else if (npc.ai[1] == 4f)
                    {
                        if (npc.ai[2] == 0f)
                        {
                            SoundEngine.PlaySound(SoundID.ForceRoarPitched, npc.Center);
                        }
                        float chargeLength = madDashLengthInFrames;
                        npc.ai[2] += 1f;
                        if (npc.ai[2] == chargeLength && Vector2.Distance(npc.position, Main.player[npc.target].position) < 200f)
                        {
                            npc.ai[2] -= 1f;
                        }
                        if (npc.ai[2] >= chargeLength)
                        {
                            npc.velocity *= 0.99f; 
                            if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
                            {
                                npc.velocity.X = 0f;
                            }
                            if ((double)npc.velocity.Y > -0.1 && (double)npc.velocity.Y < 0.1)
                            {
                                npc.velocity.Y = 0f;
                            }
                        }
                        else
                        {
                           npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - 1.57f;
                        }
                        float cooldownBetweenMadDashes = chargeLength + 13f;
                        if (npc.ai[2] >= cooldownBetweenMadDashes)
                        {
                            npc.netUpdate = true;
                            if (npc.netSpam > 10)
                            {
                                npc.netSpam = 10;
                            }
                            npc.ai[3] += 1f;
                            npc.ai[2] = 0f;
                            if (npc.ai[3] >= 5f)
                            {
                                npc.ai[1] = 0f;
                                npc.ai[3] = 0f;

                                if (npc.target >= 0 && Main.getGoodWorld && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, npc.width, npc.height))
                                {
                                    SoundEngine.PlaySound(SoundID.NPCHit1, npc.Center);
                                    npc.ai[0] = 2f;
                                    npc.ai[1] = 0f;
                                    npc.ai[2] = 0f;
                                    npc.ai[3] = 1f;
                                    npc.netUpdate = true;
                                }
                                else if (npc.target >= 0 && Main.masterMode)
                                {
                                    npc.ai[0] = 2f;
                                    npc.ai[2] = 0f;
                                    npc.ai[3] = 1f;
                                    npc.netUpdate = true;

                                }
                            }
                            else
                            {
                                npc.ai[1] = 3f;
                            }
                        }
                    }
                    else if (npc.ai[1] == 5f)
                    {
                        float num64 = 600f;
                        float num65 = 9f;
                        float num66 = 0.3f;
                        Vector2 vector11 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                        float num67 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector11.X;
                        float num68 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) + num64 - vector11.Y;
                        float num69 = (float)Math.Sqrt(num67 * num67 + num68 * num68);
                        num69 = num65 / num69;
                        num67 *= num69;
                        num68 *= num69;
                        if (npc.velocity.X < num67)
                        {
                            npc.velocity.X += num66;
                            if (npc.velocity.X < 0f && num67 > 0f)
                            {
                                npc.velocity.X += num66;
                            }
                        }
                        else if (npc.velocity.X > num67)
                        {
                            npc.velocity.X -= num66;
                            if (npc.velocity.X > 0f && num67 < 0f)
                            {
                                npc.velocity.X -= num66;
                            }
                        }
                        if (npc.velocity.Y < num68)
                        {
                            npc.velocity.Y += num66;
                            if (npc.velocity.Y < 0f && num68 > 0f)
                            {
                                npc.velocity.Y += num66;
                            }
                        }
                        else if (npc.velocity.Y > num68)
                        {
                            npc.velocity.Y -= num66;
                            if (npc.velocity.Y > 0f && num68 < 0f)
                            {
                                npc.velocity.Y -= num66;
                            }
                        }
                        npc.ai[2] += 1f;
                        if (npc.ai[2] >= 70f)
                        {
                            npc.TargetClosest();
                            npc.ai[1] = 3f;
                            npc.ai[2] = -1f;
                            npc.ai[3] = Main.rand.Next(-3, 1);
                            npc.netUpdate = true;
                        }
                    }
                    if (veryLowOnHealth && npc.ai[1] == 5f)
                    {
                        npc.ai[1] = 3f;
                    }
                    return false;
                }
            }
			return true;
		}
		
	}
}
	
