using Microsoft.Xna.Framework;
using Terraria;
using System;
using Terraria.ID;
using TRAEProject.Common;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs.Boss
{
    public class ChangesFishron : GlobalNPC
    {
		public override bool InstancePerEntity => true;
	     public override void SetDefaults(NPC npc)
        {
            switch (npc.type)
            {
					case NPCID.DukeFishron:
						{
							npc.GetGlobalNPC<Freeze>().freezeImmune = true;
						}
						return;
				}
		}
        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {
			if (GetInstance<TRAEConfig>().FishronChanges)
			{
				switch (npc.type)
				{
					case NPCID.DukeFishron:
						{
							target.ClearBuff(BuffID.Rabies);
							return;
						}
				}
			}
		}
		public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)/* tModPorter Note:bossAdjustment -> balance (bossAdjustment is different, see the docs for details) */
        {
			if (GetInstance<TRAEConfig>().FishronChanges)
			{
				switch (npc.type)
				{
					case NPCID.DukeFishron:
						npc.defDefense = 60;
						npc.defDamage = (int)(npc.defDamage * 0.9);
						return;
				}
			}
        }
		float phase3NadoTimer = 0f;
        public override bool PreAI(NPC npc)
        {
            if (GetInstance<TRAEConfig>().FishronChanges)
			{ 
            switch (npc.type)
			{
				case NPCID.DetonatingBubble:
					{
						if (npc.target == 255)
						{
							npc.TargetClosest();
							npc.ai[3] = Main.rand.Next(80, 121) / 100f;
							float scaleFactor = Main.rand.Next(165, 265) / 15f;
							npc.velocity = Vector2.Normalize(Main.player[npc.target].Center - npc.Center + new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101))) * scaleFactor;
							npc.netUpdate = true;
						}
						Vector2 vector122 = Vector2.Normalize(Main.player[npc.target].Center - npc.Center);
						npc.velocity = (npc.velocity * 40f + vector122 * 24f /*up from 20*/) / 41f;
						npc.scale = npc.ai[3];
						npc.alpha -= 30;
						if (npc.alpha < 50)
						{
							npc.alpha = 50;
						}
						npc.alpha = 50;
						npc.velocity.X = (npc.velocity.X * 600f + Main.rand.Next(-100, 110) * 0.1f) / 610f;// up from 50, for some reason there isn't a noticeable difference unless the numbers are this big
						npc.velocity.Y = (npc.velocity.Y * 600f + -0.25f + Main.rand.Next(-100, 110) * 0.2f) / 610f; // up from 50;
						if (npc.velocity.Y > 0f)
						{
							npc.velocity.Y -= 0.04f;
						}
						if (npc.ai[0] == 0f)
						{
							int num1041 = 40;
							Rectangle rect = npc.getRect();
							rect.X -= num1041 + npc.width / 2;
							rect.Y -= num1041 + npc.height / 2;
							rect.Width += num1041 * 2;
							rect.Height += num1041 * 2;
							for (int num1042 = 0; num1042 < 255; num1042++)
							{
								Player player3 = Main.player[num1042];
								if (player3.active && !player3.dead && rect.Intersects(player3.getRect()))
								{
									npc.ai[0] = 1f;
									npc.ai[1] = 4f;
									npc.netUpdate = true;
									break;
								}
							}
							if (npc.justHit || npc.ai[0] == 1f)
							{
								npc.dontTakeDamage = true;
								npc.position = npc.Center;
								npc.width = (npc.height = 100);
								npc.position = new Vector2(npc.position.X - (float)(npc.width / 2), npc.position.Y - (float)(npc.height / 2));
							}
						}
						if (npc.ai[0] == 0f)
						{
							npc.ai[1]++;
							if (npc.ai[1] >= 400f)
							{
								npc.ai[0] = 1f;
								npc.ai[1] = 4f;
							}
						}
						if (npc.ai[0] == 1f)
						{
							npc.ai[1]--;
							if (npc.ai[1] <= 0f)
							{
								npc.life = 0;
								npc.HitEffect();
								npc.active = false;
								return false;
							}
						}
						return false;
					}
				case NPCID.DukeFishron:
                        {
                            npc.dontTakeDamage = false;

                            bool expertMode = Main.expertMode;
						bool MasterMode = Main.masterMode;
						float num = expertMode ? 1.1f : 1f;
						bool flag = expertMode ? npc.life <= npc.lifeMax * 0.65 : npc.life <= npc.lifeMax * 0.5;
						bool flag2 = expertMode && npc.life <= npc.lifeMax * 0.25;
						bool flag3 = npc.ai[0] > 4f;
						bool PhaseThree = npc.ai[0] > 9f;
						bool flag5 = npc.ai[3] < 10f;
						if (PhaseThree)
						{
							npc.damage = (int)(npc.defDamage * 1.1f * num);
							npc.defense = 40;
						}
						else if (flag3)
						{
							npc.damage = (int)(npc.defDamage * 1.1f * num);
							npc.defense = (int)(npc.defDefense * 0.8f);
						}
						else
						{
							npc.damage = npc.defDamage;
							npc.defense = npc.defDefense;
						}
						int num2 = expertMode ? 40 : 60;
						float num3 = expertMode ? 0.55f : 0.45f;
						float scaleFactor = expertMode ? 8.5f : 7.5f;
						if (PhaseThree)
						{
							num3 = 0.7f;
							scaleFactor = 12f;
							num2 = 30;
						}
						else if (flag3 && flag5)
						{
							num3 = (expertMode ? 0.6f : 0.5f);
							scaleFactor = (expertMode ? 10f : 8f);
							num2 = (expertMode ? 40 : 20);
						}
						else if (flag5 && !flag3 && !PhaseThree)
						{
							num2 = 30;
						}
						int lungeLength = MasterMode ? 38 : 30; // up from 28/30
						float lungeVelocity = MasterMode ? 20f : 17f; // up from 17/16
						if (PhaseThree && MasterMode)
						{
							lungeLength = 38;
							lungeVelocity = 24f;
						}
						else if (flag5 && flag3)
						{
							lungeLength = MasterMode ? 38 : 30;
							if (MasterMode)
							{
								lungeVelocity = 21f;
							}
						}
						int BubbleLimit = MasterMode ? 160 : 80; // up from 80
						int BubbleDelay = MasterMode ? 6 : 4; // The higher the value the more delay between the bubbles; up from 4
						float num8 = 0.3f;
						float scaleFactor2 = 5f;
						int num9 = 90;
						int Phase2Transition = 180; // transition to phase 2
						int Phase3Transition = MasterMode ? 90 : 180; // transition to phase 3
						int num12 = 30;
						int bubbleAttackDuratioPhase2 = MasterMode ? 240 : 120; // up from 120
						int bubbledelayPhase2 = MasterMode ? 8 : 4; // vanilla value = 4
						int phase3NadoDelay = 360;
						float scaleFactor3 = 6f;
						float scaleFactor4 = 20f;
						float num15 = MathF.PI * 2f / (bubbleAttackDuratioPhase2 / 2);
						int num16 = 75;
						Vector2 center = npc.Center;
						Player player = Main.player[npc.target];
						
						if (npc.target < 0 || npc.target == 255 || player.dead || !player.active || Vector2.Distance(player.Center, center) > 5600f)
						{
							npc.TargetClosest();
							player = Main.player[npc.target];
							npc.netUpdate = true;
						}
						if (player.dead)
							{
								npc.EncourageDespawn(180);
							}
						if (player.position.Y < 800f || player.position.Y > Main.worldSurface * 16.0 || (player.position.X > 6400f && player.position.X < (Main.maxTilesX * 16 - 6400)))
						{
							num2 = 20;
							npc.damage = npc.defDamage * 2;
							npc.defense = npc.defDefense * 2;
							npc.ai[3] = 0f;
							lungeVelocity += 6f;
						}
						if (npc.localAI[0] == 0f)
						{
							npc.localAI[0] = 1f;
							npc.alpha = 255;
							npc.rotation = 0f;
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								npc.ai[0] = -1f;
								npc.netUpdate = true;
							}
						}
						float num17 = MathF.Atan2(player.Center.Y - center.Y, player.Center.X - center.X);
						if (npc.spriteDirection == 1)
						{
							num17 += MathF.PI;
						}
						if (num17 < 0f)
						{
							num17 += MathF.PI * 2f;
						}
						if (num17 > MathF.PI * 2f)
						{
							num17 -= MathF.PI * 2f;
						}
						if (npc.ai[0] == -1f)
						{
							num17 = 0f;
						}
						if (npc.ai[0] == 3f)
						{
							num17 = 0f;
						}
						if (npc.ai[0] == 4f)
						{
							num17 = 0f;
						}
						if (npc.ai[0] == 8f)
						{
							num17 = 0f;
						}
						float num18 = 0.04f;
						if (npc.ai[0] == 1f || npc.ai[0] == 6f)
						{
							num18 = 0f;
						}
						if (npc.ai[0] == 7f)
						{
							num18 = 0f;
						}
						if (npc.ai[0] == 3f)
						{
							num18 = 0.01f;
						}
						if (npc.ai[0] == 4f)
						{
							num18 = 0.01f;
						}
						if (npc.ai[0] == 8f)
						{
							num18 = 0.01f;
						}
						if (npc.rotation < num17)
						{
							if ((double)(num17 - npc.rotation) > Math.PI)
							{
								npc.rotation -= num18;
							}
							else
							{
								npc.rotation += num18;
							}
						}
						if (npc.rotation > num17)
						{
							if ((double)(npc.rotation - num17) > Math.PI)
							{
								npc.rotation += num18;
							}
							else
							{
								npc.rotation -= num18;
							}
						}
						if (npc.rotation > num17 - num18 && npc.rotation < num17 + num18)
						{
							npc.rotation = num17;
						}
						if (npc.rotation < 0f)
						{
							npc.rotation += MathF.PI * 2f;
						}
						if (npc.rotation > MathF.PI * 2f)
						{
							npc.rotation -= MathF.PI * 2f;
						}
						if (npc.rotation > num17 - num18 && npc.rotation < num17 + num18)
						{
							npc.rotation = num17;
						}
						if (npc.ai[0] != -1f && npc.ai[0] < 9f)
						{
							if (Collision.SolidCollision(npc.position, npc.width, npc.height))
							{
								npc.alpha += 15;
							}
							else
							{
								npc.alpha -= 15;
							}
							if (npc.alpha < 0)
							{
								npc.alpha = 0;
							}
							if (npc.alpha > 150)
							{
								npc.alpha = 150;
							}
						}
						if (npc.ai[0] == -1f)
						{
							npc.velocity *= 0.98f;
							int num19 = Math.Sign(player.Center.X - center.X);
							if (num19 != 0)
							{
								npc.direction = num19;
								npc.spriteDirection = -npc.direction;
							}
							if (npc.ai[2] > 20f)
							{
								npc.velocity.Y = -2f;
								npc.alpha -= 5;
								if (Collision.SolidCollision(npc.position, npc.width, npc.height))
								{
									npc.alpha += 15;
								}
								if (npc.alpha < 0)
								{
									npc.alpha = 0;
								}
								if (npc.alpha > 150)
								{
									npc.alpha = 150;
								}
							}
							if (npc.ai[2] == (num9 - 30))
							{
								int num20 = 36;
								for (int i = 0; i < num20; i++)
								{
									Vector2 value = (Vector2.Normalize(npc.velocity) * new Vector2((float)npc.width / 2f, npc.height) * 0.75f * 0.5f).RotatedBy((float)(i - (num20 / 2 - 1)) * (MathF.PI * 2f) / (float)num20) + npc.Center;
									Vector2 value2 = value - npc.Center;
									int num21 = Dust.NewDust(value + value2, 0, 0, DustID.DungeonWater, value2.X * 2f, value2.Y * 2f, 100, default, 1.4f);
									Main.dust[num21].noGravity = true;
									Main.dust[num21].noLight = true;
									Main.dust[num21].velocity = Vector2.Normalize(value2) * 3f;
								}
								Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie20 with { MaxInstances = 0 }, npc.Center);
							}
							npc.ai[2] += 1f;
							if (npc.ai[2] >= (float)num16)
							{
								npc.ai[0] = 0f;
								npc.ai[1] = 0f;
								npc.ai[2] = 0f;
								npc.netUpdate = true;
							}
						}
						else if (npc.ai[0] == 0f && !player.dead)
						{
							if (npc.ai[1] == 0f)
							{
								npc.ai[1] = 300 * Math.Sign((center - player.Center).X);
							}
							Vector2 vector = Vector2.Normalize(player.Center + new Vector2(npc.ai[1], -200f) - center - npc.velocity) * scaleFactor;
							if (npc.velocity.X < vector.X)
							{
								npc.velocity.X += num3;
								if (npc.velocity.X < 0f && vector.X > 0f)
								{
									npc.velocity.X += num3;
								}
							}
							else if (npc.velocity.X > vector.X)
							{
								npc.velocity.X -= num3;
								if (npc.velocity.X > 0f && vector.X < 0f)
								{
									npc.velocity.X -= num3;
								}
							}
							if (npc.velocity.Y < vector.Y)
							{
								npc.velocity.Y += num3;
								if (npc.velocity.Y < 0f && vector.Y > 0f)
								{
									npc.velocity.Y += num3;
								}
							}
							else if (npc.velocity.Y > vector.Y)
							{
								npc.velocity.Y -= num3;
								if (npc.velocity.Y > 0f && vector.Y < 0f)
								{
									npc.velocity.Y -= num3;
								}
							}
							int num22 = Math.Sign(player.Center.X - center.X);
							if (num22 != 0)
							{
								if (npc.ai[2] == 0f && num22 != npc.direction)
								{
									npc.rotation += MathF.PI;
								}
								npc.direction = num22;
								if (npc.spriteDirection != -npc.direction)
								{
									npc.rotation += MathF.PI;
								}
								npc.spriteDirection = -npc.direction;
							}
							npc.ai[2] += 1f;
							if (!(npc.ai[2] >= (float)num2))
							{
								return false;
							}
							int num23 = 0;
							switch ((int)npc.ai[3])
							{
								case 0:
								case 1:
								case 2:
								case 3:
								case 4:
								case 5:
								case 6:
								case 7:
								case 8:
								case 9:
									num23 = 1;
									break;
								case 10:
									npc.ai[3] = 1f;
									num23 = 2;
									break;
								case 11:
									npc.ai[3] = 0f;
									num23 = 3;
									break;
							}
							if (flag)
							{
								num23 = 4;
							}
							switch (num23)
							{
								case 1:
									npc.ai[0] = 1f;
									npc.ai[1] = 0f;
									npc.ai[2] = 0f;
									npc.velocity = Vector2.Normalize(player.Center - center) * lungeVelocity;
									npc.rotation = MathF.Atan2(npc.velocity.Y, npc.velocity.X);
									if (num22 != 0)
									{
										npc.direction = num22;
										if (npc.spriteDirection == 1)
										{
											npc.rotation += MathF.PI;
										}
										npc.spriteDirection = -npc.direction;
									}
									break;
								case 2:
									npc.ai[0] = 2f;
									npc.ai[1] = 0f;
									npc.ai[2] = 0f;
									break;
								case 3:
									npc.ai[0] = 3f;
									npc.ai[1] = 0f;
									npc.ai[2] = 0f;
									break;
								case 4:
									npc.ai[0] = 4f;
									npc.ai[1] = 0f;
									npc.ai[2] = 0f;
									break;
							}
							npc.netUpdate = true;
						}
						else if (npc.ai[0] == 1f)
						{
							int num24 = 7;
							for (int j = 0; j < num24; j++)
							{
								Vector2 value3 = (Vector2.Normalize(npc.velocity) * new Vector2((float)(npc.width + 50) / 2f, npc.height) * 0.75f).RotatedBy((double)(j - (num24 / 2 - 1)) * Math.PI / (double)(float)num24) + center;
								Vector2 value4 = ((float)(Main.rand.NextDouble() * 3.1415927410125732) - MathF.PI / 2f).ToRotationVector2() * Main.rand.Next(3, 8);
								int num25 = Dust.NewDust(value3 + value4, 0, 0, DustID.DungeonWater, value4.X * 2f, value4.Y * 2f, 100, default, 1.4f);
								Main.dust[num25].noGravity = true;
								Main.dust[num25].noLight = true;
								Main.dust[num25].velocity /= 4f;
								Main.dust[num25].velocity -= npc.velocity;
							}
							npc.ai[2] += 1f;
							if (npc.ai[2] >= (float)lungeLength)
							{
								npc.ai[0] = 0f;
								npc.ai[1] = 0f;
								npc.ai[2] = 0f;
								npc.ai[3] += 2f;
								npc.netUpdate = true;
							}
						}
						else if (npc.ai[0] == 2f)
						{
							if (npc.ai[1] == 0f)
							{
								npc.ai[1] = 300 * Math.Sign((center - player.Center).X);
							}
							Vector2 vector2 = Vector2.Normalize(player.Center + new Vector2(npc.ai[1], -200f) - center - npc.velocity) * scaleFactor2;
							if (npc.velocity.X < vector2.X)
							{
								npc.velocity.X += num8;
								if (npc.velocity.X < 0f && vector2.X > 0f)
								{
									npc.velocity.X += num8;
								}
							}
							else if (npc.velocity.X > vector2.X)
							{
								npc.velocity.X -= num8;
								if (npc.velocity.X > 0f && vector2.X < 0f)
								{
									npc.velocity.X -= num8;
								}
							}
							if (npc.velocity.Y < vector2.Y)
							{
								npc.velocity.Y += num8;
								if (npc.velocity.Y < 0f && vector2.Y > 0f)
								{
									npc.velocity.Y += num8;
								}
							}
							else if (npc.velocity.Y > vector2.Y)
							{
								npc.velocity.Y -= num8;
								if (npc.velocity.Y > 0f && vector2.Y < 0f)
								{
									npc.velocity.Y -= num8;
								}
							}
							// SPAWNS DETONATING BUBBLES
							if (npc.ai[2] == 0f)
							{
								Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie20 with { MaxInstances = 0 }, npc.Center);
							}
							if (npc.ai[2] % BubbleDelay == 0f)
							{
								Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath19 with { MaxInstances = 0 }, npc.Center);
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									Vector2 vector3 = Vector2.Normalize(player.Center - center) * (npc.width + 20) / 2f + center;
									NPC.NewNPC(NPC.GetBossSpawnSource(npc.target), (int)vector3.X, (int)vector3.Y + 45, 371);
								}
							}
							int num26 = Math.Sign(player.Center.X - center.X);
							if (num26 != 0)
							{
								npc.direction = num26;
								if (npc.spriteDirection != -npc.direction)
								{
									npc.rotation += MathF.PI;
								}
								npc.spriteDirection = -npc.direction;
							}
							npc.ai[2] += 1f;
							if (npc.ai[2] >= (float)BubbleLimit)
							{
								npc.ai[0] = 0f;
								npc.ai[1] = 0f;
								npc.ai[2] = 0f;
								npc.netUpdate = true;
							}
						}
						/// CREATE SHARKNADOS
						else if (npc.ai[0] == 3f)
						{
							npc.velocity *= 0.98f;
							npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, 0f, 0.02f);
							if (npc.ai[2] == (num9 - 30))
							{
								Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie9 with { MaxInstances = 0 }, npc.Center);
							}
							if (Main.netMode != NetmodeID.MultiplayerClient && npc.ai[2] == (num9 - 30))
							{
								Vector2 vector4 = npc.rotation.ToRotationVector2() * (Vector2.UnitX * npc.direction) * (npc.width + 20) / 2f + center;
								Projectile.NewProjectile(npc.GetSource_FromThis(), vector4.X, vector4.Y, npc.direction * 2, 8f, ProjectileID.SharknadoBolt, 0, 0f, Main.myPlayer);
								Projectile.NewProjectile(npc.GetSource_FromThis(), vector4.X, vector4.Y, -npc.direction * 2, 8f, ProjectileID.SharknadoBolt, 0, 0f, Main.myPlayer);
							}
							npc.ai[2] += 1f;
							if (npc.ai[2] >= num9)
							{
								npc.ai[0] = 0f;
								npc.ai[1] = 0f;
								npc.ai[2] = 0f;
								npc.netUpdate = true;
							}
						}
						else if (npc.ai[0] == 4f)
						{
								npc.dontTakeDamage = true;
							npc.velocity *= 0.98f;
							npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, 0f, 0.02f);
							if (npc.ai[2] == (Phase2Transition - 60))
							{
								Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie20 with { MaxInstances = 0 }, npc.Center);
							}
							npc.ai[2] += 1f;
							if (npc.ai[2] >= (float)Phase2Transition)
							{
								npc.ai[0] = 5f;
								npc.ai[1] = 0f;
								npc.ai[2] = 0f;
								npc.ai[3] = 0f;
								npc.netUpdate = true;
							}
						}
						/// CHARGES
						else if (npc.ai[0] == 5f && !player.dead)
						{
							if (npc.ai[1] == 0f)
							{
								npc.ai[1] = 300 * Math.Sign((center - player.Center).X);
							}
							Vector2 vector5 = Vector2.Normalize(player.Center + new Vector2(npc.ai[1], -200f) - center - npc.velocity) * scaleFactor;
							if (npc.velocity.X < vector5.X)
							{
								npc.velocity.X += num3;
								if (npc.velocity.X < 0f && vector5.X > 0f)
								{
									npc.velocity.X += num3;
								}
							}
							else if (npc.velocity.X > vector5.X)
							{
								npc.velocity.X -= num3;
								if (npc.velocity.X > 0f && vector5.X < 0f)
								{
									npc.velocity.X -= num3;
								}
							}
							if (npc.velocity.Y < vector5.Y)
							{
								npc.velocity.Y += num3;
								if (npc.velocity.Y < 0f && vector5.Y > 0f)
								{
									npc.velocity.Y += num3;
								}
							}
							else if (npc.velocity.Y > vector5.Y)
							{
								npc.velocity.Y -= num3;
								if (npc.velocity.Y > 0f && vector5.Y < 0f)
								{
									npc.velocity.Y -= num3;
								}
							}
							int num27 = Math.Sign(player.Center.X - center.X);
							if (num27 != 0)
							{
								if (npc.ai[2] == 0f && num27 != npc.direction)
								{
									npc.rotation += MathF.PI;
								}
								npc.direction = num27;
								if (npc.spriteDirection != -npc.direction)
								{
									npc.rotation += MathF.PI;
								}
								npc.spriteDirection = -npc.direction;
							}
							npc.ai[2] += 1f;
							if (!(npc.ai[2] >= (float)num2))
							{
								return false;
							}
							int num28 = 0;
							switch ((int)npc.ai[3])
							{
								case 0:
								case 1:
								case 2:
								case 3:
								case 4:
								case 5:
									num28 = 1;
									break;
								case 6:
									npc.ai[3] = 1f;
									num28 = 2;
									break;
								case 7:
									npc.ai[3] = 0f;
									num28 = 3;
									break;
							}
							if (flag2)
							{
								num28 = 4;
							}
							switch (num28)
							{
								case 1:
									npc.ai[0] = 6f;
									npc.ai[1] = 0f;
									npc.ai[2] = 0f;
									npc.velocity = Vector2.Normalize(player.Center - center) * lungeVelocity;
									npc.rotation = MathF.Atan2(npc.velocity.Y, npc.velocity.X);
									if (num27 != 0)
									{
										npc.direction = num27;
										if (npc.spriteDirection == 1)
										{
											npc.rotation += MathF.PI;
										}
										npc.spriteDirection = -npc.direction;
									}
									break;
								case 2:
									npc.velocity = Vector2.Normalize(player.Center - center) * scaleFactor4;
									npc.rotation = MathF.Atan2(npc.velocity.Y, npc.velocity.X);
									if (num27 != 0)
									{
										npc.direction = num27;
										if (npc.spriteDirection == 1)
										{
											npc.rotation += MathF.PI;
										}
										npc.spriteDirection = -npc.direction;
									}
									npc.ai[0] = 7f;
									npc.ai[1] = 0f;
									npc.ai[2] = 0f;
									break;
								case 3:
									npc.ai[0] = 8f;
									npc.ai[1] = 0f;
									npc.ai[2] = 0f;
									break;
								case 4:
									npc.ai[0] = 9f;
									npc.ai[1] = 0f;
									npc.ai[2] = 0f;
									break;
							}
							npc.netUpdate = true;
						}
						else if (npc.ai[0] == 6f)
						{
							int num29 = 7;
							for (int k = 0; k < num29; k++)
							{
								Vector2 value5 = (Vector2.Normalize(npc.velocity) * new Vector2((float)(npc.width + 50) / 2f, npc.height) * 0.75f).RotatedBy((k - (num29 / 2 - 1)) * Math.PI / (float)num29) + center;
								Vector2 value6 = ((float)(Main.rand.NextDouble() * 3.1415927410125732) - MathF.PI / 2f).ToRotationVector2() * Main.rand.Next(3, 8);
								int num30 = Dust.NewDust(value5 + value6, 0, 0, DustID.DungeonWater, value6.X * 2f, value6.Y * 2f, 100, default, 1.4f);
								Main.dust[num30].noGravity = true;
								Main.dust[num30].noLight = true;
								Main.dust[num30].velocity /= 4f;
								Main.dust[num30].velocity -= npc.velocity;
							}
							npc.ai[2] += 1f;
							if (npc.ai[2] >= lungeLength)
							{
								npc.ai[0] = 5f;
								npc.ai[1] = 0f;
								npc.ai[2] = 0f;
								npc.ai[3] += 2f;
								npc.netUpdate = true;
							}
						}
						// PHASE 2 SPAWN DETONATING BUBBLES
						else if (npc.ai[0] == 7f)
						{
							if (npc.ai[2] == 0f)
							{
								Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie20 with { MaxInstances = 0 }, npc.Center);
							}
							if (npc.ai[2] % bubbledelayPhase2 == 0f)
							{
								Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath19 with { MaxInstances = 0 }, npc.Center);
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									Vector2 vector6 = Vector2.Normalize(npc.velocity) * (npc.width + 20) / 2f + center;
									int num31 = NPC.NewNPC(NPC.GetBossSpawnSource(npc.target), (int)vector6.X, (int)vector6.Y + 45, 371);
									Main.npc[num31].target = npc.target;
									Main.npc[num31].velocity = Vector2.Normalize(npc.velocity).RotatedBy(MathF.PI / 2f * (float)npc.direction) * scaleFactor3;
									Main.npc[num31].netUpdate = true;
									Main.npc[num31].ai[3] = (float)Main.rand.Next(80, 121) / 100f;
								}
							}
							npc.velocity = npc.velocity.RotatedBy((0f - num15) * (float)npc.direction);
							npc.rotation -= num15 * (float)npc.direction;
							npc.ai[2] += 1f;
							if (npc.ai[2] >= (float)bubbleAttackDuratioPhase2)
							{
								npc.ai[0] = 5f;
								npc.ai[1] = 0f;
								npc.ai[2] = 0f;
								npc.netUpdate = true;
							}
						}
						// SPAWN CTHULHUNADOES
						else if (npc.ai[0] == 8f)
						{
							npc.velocity *= 0.98f;
							npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, 0f, 0.02f);
							if (npc.ai[2] == (float)(num9 - 30))
							{
								Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie20 with { MaxInstances = 0 }, npc.Center);
							}
							if (Main.netMode != NetmodeID.MultiplayerClient && npc.ai[2] == (float)(num9 - 30))
							{
								Projectile.NewProjectile(npc.GetSource_FromThis(), center.X, center.Y, 0f, 0f, ProjectileID.SharknadoBolt, 0, 0f, Main.myPlayer, 1f, npc.target + 1);
							}
							npc.ai[2] += 1f;
							if (npc.ai[2] >= (float)num9)
							{
								npc.ai[0] = 5f;
								npc.ai[1] = 0f;
								npc.ai[2] = 0f;
								npc.netUpdate = true;
							}
						}
						// phase 3 transition
						else if (npc.ai[0] == 9f)
						{
							npc.dontTakeDamage = true;
							if (npc.ai[2] < (float)(Phase3Transition - 45))
							{
								if (Collision.SolidCollision(npc.position, npc.width, npc.height))
								{
									npc.alpha += 15;
								}
								else
								{
									npc.alpha -= 15;
								}
								if (npc.alpha < 0)
								{
									npc.alpha = 0;
								}
								if (npc.alpha > 150)
								{
									npc.alpha = 150;
								}
							}
							else if (npc.alpha < 255)
							{
								npc.alpha += 4;
								if (npc.alpha > 255)
								{
									npc.alpha = 255;
								}
							}
							npc.velocity *= 0.98f;
							npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, 0f, 0.02f);
							if (npc.ai[2] == (Phase3Transition - 30))
							{
								Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie20 with { MaxInstances = 0 }, npc.Center);
							}
							npc.ai[2] += 1f;
							if (npc.ai[2] >= Phase3Transition)
							{
								npc.ai[0] = 10f;
								npc.ai[1] = 0f;
								npc.ai[2] = 0f;
								npc.ai[3] = 0f;
								npc.netUpdate = true;
							}
						}
						else if (npc.ai[0] == 10f && !player.dead)
						{
							if (Main.masterMode)
								++phase3NadoTimer;
							if (phase3NadoTimer >= phase3NadoDelay)
							{
								Projectile.NewProjectile(npc.GetSource_FromThis(), center.X, center.Y, 0f, 0f, ProjectileID.SharknadoBolt, 0, 0f, Main.myPlayer, 1f, npc.target + 1);
								phase3NadoTimer = 0;
							}
							npc.dontTakeDamage = false;
							npc.chaseable = false;
							if (npc.alpha < 255)
							{
								npc.alpha += 25;
								if (npc.alpha > 255)
								{
									npc.alpha = 255;
								}
							}
							if (npc.ai[1] == 0f)
							{
								npc.ai[1] = 360 * Math.Sign((center - player.Center).X);
							}
							Vector2 desiredVelocity = Vector2.Normalize(player.Center + new Vector2(npc.ai[1], -200f) - center - npc.velocity) * scaleFactor;
							npc.SimpleFlyMovement(desiredVelocity, num3);
							int num32 = Math.Sign(player.Center.X - center.X);
							if (num32 != 0)
							{
								if (npc.ai[2] == 0f && num32 != npc.direction)
								{
									npc.rotation += MathF.PI;
									for (int l = 0; l < npc.oldPos.Length; l++)
									{
										npc.oldPos[l] = Vector2.Zero;
									}
								}
								npc.direction = num32;
								if (npc.spriteDirection != -npc.direction)
								{
									npc.rotation += MathF.PI;
								}
								npc.spriteDirection = -npc.direction;
							}
							npc.ai[2] += 1f;
							if (!(npc.ai[2] >= (float)num2))
							{
								return false;
							}
							int num33 = 0;
							switch ((int)npc.ai[3])
							{
								case 0:
								case 2:
								case 3:
								case 5:
								case 6:
								case 7:
									num33 = 1;
									break;
								case 1:
								case 4:
								case 8:
									num33 = 2;
									break;
							}
							switch (num33)
							{
								case 1:
									npc.ai[0] = 11f;
									npc.ai[1] = 0f;
									npc.ai[2] = 0f;
									npc.velocity = Vector2.Normalize(player.Center - center) * lungeVelocity;
									npc.rotation = MathF.Atan2(npc.velocity.Y, npc.velocity.X);
									if (num32 != 0)
									{
										npc.direction = num32;
										if (npc.spriteDirection == 1)
										{
											npc.rotation += MathF.PI;
										}
										npc.spriteDirection = -npc.direction;
									}
									break;
								case 2:
									npc.ai[0] = 12f;
									npc.ai[1] = 0f;
									npc.ai[2] = 0f;
									break;
								case 3:
									npc.ai[0] = 13f;
									npc.ai[1] = 0f;
									npc.ai[2] = 0f;
									break;
							}
							npc.netUpdate = true;
						}
						else if (npc.ai[0] == 11f)
						{
							npc.dontTakeDamage = false;
							npc.chaseable = true;
							npc.alpha -= 25;
							if (npc.alpha < 0)
							{
								npc.alpha = 0;
							}
							int num34 = 7;
							for (int m = 0; m < num34; m++)
							{
								Vector2 value7 = (Vector2.Normalize(npc.velocity) * new Vector2((float)(npc.width + 50) / 2f, npc.height) * 0.75f).RotatedBy((double)(m - (num34 / 2 - 1)) * Math.PI / (double)(float)num34) + center;
								Vector2 value8 = ((float)(Main.rand.NextDouble() * 3.1415927410125732) - MathF.PI / 2f).ToRotationVector2() * Main.rand.Next(3, 8);
								int num35 = Dust.NewDust(value7 + value8, 0, 0, DustID.DungeonWater, value8.X * 2f, value8.Y * 2f, 100, default, 1.4f);
								Main.dust[num35].noGravity = true;
								Main.dust[num35].noLight = true;
								Main.dust[num35].velocity /= 4f;
								Main.dust[num35].velocity -= npc.velocity;
							}
							npc.ai[2] += 1f;
							if (npc.ai[2] >= (float)lungeLength)
							{
								npc.ai[0] = 10f;
								npc.ai[1] = 0f;
								npc.ai[2] = 0f;
								npc.ai[3] += 1f;
								npc.netUpdate = true;
							}
						}
						else if (npc.ai[0] == 12f)
						{
							npc.dontTakeDamage = true;
							npc.chaseable = false;
							if (npc.alpha < 255)
							{
								npc.alpha += 17;
								if (npc.alpha > 255)
								{
									npc.alpha = 255;
								}
							}
							npc.velocity *= 0.98f;
							npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, 0f, 0.02f);
							if (npc.ai[2] == (float)(num12 / 2))
							{
								Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie20 with { MaxInstances = 0 }, npc.Center);
							}
							if (Main.netMode != NetmodeID.MultiplayerClient && npc.ai[2] == (float)(num12 / 2))
							{
								if (npc.ai[1] == 0f)
								{
									npc.ai[1] = 300 * Math.Sign((center - player.Center).X);
								}
								Vector2 vector7 = player.Center + new Vector2(0f - npc.ai[1], -200f);
								Vector2 vector9 = npc.Center = vector7;
								center = vector9;
								int num36 = Math.Sign(player.Center.X - center.X);
								if (num36 != 0)
								{
									if (npc.ai[2] == 0f && num36 != npc.direction)
									{
										npc.rotation += MathF.PI;
										for (int n = 0; n < npc.oldPos.Length; n++)
										{
											npc.oldPos[n] = Vector2.Zero;
										}
									}
									npc.direction = num36;
									if (npc.spriteDirection != -npc.direction)
									{
										npc.rotation += MathF.PI;
									}
									npc.spriteDirection = -npc.direction;
								}
							}
							npc.ai[2] += 1f;
							if (npc.ai[2] >= (float)num12)
							{
								npc.ai[0] = 10f;
								npc.ai[1] = 0f;
								npc.ai[2] = 0f;
								npc.ai[3] += 1f;
								if (npc.ai[3] >= 9f)
								{
									npc.ai[3] = 0f;
								}
								npc.netUpdate = true;
							}
						}
						else if (npc.ai[0] == 13f)
						{
							if (npc.ai[2] == 0f)
							{
								Terraria.Audio.SoundEngine.PlaySound(SoundID.Zombie20 with { MaxInstances = 0 }, npc.Center);
							}
							npc.velocity = npc.velocity.RotatedBy((0f - num15) * (float)npc.direction);
							npc.rotation -= num15 * (float)npc.direction;
							npc.ai[2] += 1f;
							if (npc.ai[2] >= (float)bubbleAttackDuratioPhase2)
							{
								npc.ai[0] = 10f;
								npc.ai[1] = 0f;
								npc.ai[2] = 0f;
								npc.ai[3] += 1f;
								npc.netUpdate = true;
							}
						}
					}
					return false;
			}
            }
            return true;
		}
			
		
    }
}