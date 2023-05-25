using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NPCs.Boss
{
    public class QueenBee : GlobalNPC
    {
		public override bool InstancePerEntity => true;
		int stingercounter = 0;
        public override void SetDefaults(NPC npc)
        {
			if (npc.type >= 210 && npc.type <= 211)
			{
				npc.lifeMax = 5;
				npc.defense = 0;
			}

		}
        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
        {				
			if (npc.type >= 210 && npc.type <= 211)
				target.ApplyDamageToNPC(npc, npc.life + (npc.defense + 1) / 2, 0, 0, false);

		}
        public override bool PreAI(NPC npc)
        {
			if (GetInstance<TRAEConfig>().QBChanges)
			{
				if (npc.type == NPCID.QueenBee)
				{
					if (Main.expertMode)
					{
						float healthPercent = (float)(npc.life / (float)npc.lifeMax);

						float bonusDefense = 12 * (1 - healthPercent);
						npc.defense = npc.defDefense + (int)bonusDefense;
					}
					if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
					{
						npc.TargetClosest();
					}
					bool dead4 = Main.player[npc.target].dead;
					float EnrageBonus = 0f;
					int bonus = (npc.life < npc.lifeMax * 0.375) ? 2 : 1;
					if ((double)(npc.position.Y / 16f) < Main.worldSurface)
					{
						EnrageBonus += 1f;
					}
					if (!Main.player[npc.target].ZoneJungle)
					{
						EnrageBonus += 1f;
					}
					if (Main.getGoodWorld)
					{
						EnrageBonus += 0.5f;
					}
					float num617 = Vector2.Distance(npc.Center, Main.player[npc.target].Center);
					if (npc.ai[0] != 5f)
					{
						if (npc.timeLeft < 60)
						{
							npc.timeLeft = 60;
						}
						if (num617 > 3000f)
						{
							npc.ai[0] = 4f;
							npc.netUpdate = true;
						}
					}
					if (dead4)
					{
						npc.ai[0] = 5f;
						npc.netUpdate = true;
					}
					if (npc.ai[0] == 5f)
					{
						npc.velocity.Y *= 0.98f;
						if (npc.velocity.X < 0f)
						{
							npc.direction = -1;
						}
						else
						{
							npc.direction = 1;
						}
						npc.spriteDirection = npc.direction;
						if (npc.position.X < (Main.maxTilesX * 8))
						{
							if (npc.velocity.X > 0f)
							{
								npc.velocity.X *= 0.98f;
							}
							else
							{
								npc.localAI[0] = 1f;
							}
							npc.velocity.X -= 0.08f;
						}
						else
						{
							if (npc.velocity.X < 0f)
							{
								npc.velocity.X *= 0.98f;
							}
							else
							{
								npc.localAI[0] = 1f;
							}
							npc.velocity.X += 0.08f;
						}
						npc.EncourageDespawn(10);
					}
					else if (npc.ai[0] == -1f)
					{
						if (Main.netMode == 1)
						{
							return false;
						}
						float num618 = npc.ai[1];
						int num619;
						do
						{
							num619 = Main.rand.Next(3);
							switch (num619)
							{
								case 1:
									num619 = 2;
									break;
								case 2:
									num619 = 3;
									break;
							}
						}
						while (num619 == num618);
						npc.ai[0] = num619;
						npc.ai[1] = 0f;
						npc.ai[2] = 0f;
						npc.netUpdate = true;
					}
					else if (npc.ai[0] == 0f)
					{
						int num620 = 2;
						if (Main.expertMode)
						{
							if (npc.life < npc.lifeMax / 2)
							{
								num620++;
							}
							if (npc.life < npc.lifeMax / 3)
							{
								num620++;
							}
							if (npc.life < npc.lifeMax / 5)
							{
								num620++;
							}
						}
						num620 += (int)(1f * EnrageBonus);
						if (npc.ai[1] > (2 * num620) && npc.ai[1] % 2f == 0f)
						{
							npc.ai[0] = -1f;
							npc.ai[1] = 0f;
							npc.ai[2] = 0f;
							npc.netUpdate = true;
							return false;
						}
						if (npc.ai[1] % 2f == 0f)
						{
							npc.TargetClosest();
							float num621 = 20f;
							num621 += 20f * EnrageBonus;
							if (Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) < num621)
							{
								npc.localAI[0] = 1f;
								npc.ai[1] += 1f;
								npc.ai[2] = 0f;
								npc.netUpdate = true;
								float num622 = 12f;
								if (Main.masterMode)
								{
									num622 = 24f;
								}
								else if (Main.expertMode)
								{
									num622 = 16f;
									if ((double)npc.life < (double)npc.lifeMax * 0.75)
									{
										num622 += 2f;
									}
									if ((double)npc.life < (double)npc.lifeMax * 0.5)
									{
										num622 += 2f;
									}
									if ((double)npc.life < (double)npc.lifeMax * 0.25)
									{
										num622 += 2f;
									}
									if ((double)npc.life < (double)npc.lifeMax * 0.1)
									{
										num622 += 2f;
									}
								}

								num622 += 7f * EnrageBonus;
								Vector2 vector76 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
								float num623 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector76.X;
								float num624 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector76.Y;
								float num625 = MathF.Sqrt(num623 * num623 + num624 * num624);
								num625 = num622 / num625;
								npc.velocity.X = num623 * num625;
								npc.velocity.Y = num624 * num625;
								npc.spriteDirection = npc.direction;
								SoundEngine.PlaySound(SoundID.Roar with { MaxInstances = 0 }, npc.position);
								return false;
							}
							npc.localAI[0] = 0f;
							float num626 = 12f;
							float num627 = 0.15f;
							if (Main.masterMode)
							{
								num626 = 16f;
								num627 = 0.4f;
							}
							else if (Main.expertMode)
							{
								if ((double)npc.life < (double)npc.lifeMax * 0.75)
								{
									num626 += 1f;
									num627 += 0.05f;
								}
								if ((double)npc.life < (double)npc.lifeMax * 0.5)
								{
									num626 += 1f;
									num627 += 0.05f;
								}
								if ((double)npc.life < (double)npc.lifeMax * 0.25)
								{
									num626 += 2f;
									num627 += 0.05f;
								}
								if ((double)npc.life < (double)npc.lifeMax * 0.1)
								{
									num626 += 2f;
									num627 += 0.1f;
								}
							}


							num626 += 3f * EnrageBonus;
							num627 += 0.5f * EnrageBonus;
							if (npc.position.Y + (float)(npc.height / 2) < Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2))
							{
								npc.velocity.Y += num627;
							}
							else
							{
								npc.velocity.Y -= num627;
							}
							if (npc.velocity.Y < 0f - num626)
							{
								npc.velocity.Y = 0f - num626;
							}
							if (npc.velocity.Y > num626)
							{
								npc.velocity.Y = num626;
							}
							if (Math.Abs(npc.position.X + (float)(npc.width / 2) - (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))) > 600f)
							{
								npc.velocity.X += 0.15f * (float)npc.direction;
							}
							else if (Math.Abs(npc.position.X + (float)(npc.width / 2) - (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))) < 300f)
							{
								npc.velocity.X -= 0.15f * (float)npc.direction;
							}
							else
							{
								npc.velocity.X *= 0.8f;
							}
							if (npc.velocity.X < -16f)
							{
								npc.velocity.X = -16f;
							}
							if (npc.velocity.X > 16f)
							{
								npc.velocity.X = 16f;
							}
							npc.spriteDirection = npc.direction;
							return false;
						}
						if (npc.velocity.X < 0f)
						{
							npc.direction = -1;
						}
						else
						{
							npc.direction = 1;
						}
						npc.spriteDirection = npc.direction;
						int num628 = 600;
						if (Main.expertMode)
						{
							if ((double)npc.life < (double)npc.lifeMax * 0.1)
							{
								num628 = 300;
							}
							else if ((double)npc.life < (double)npc.lifeMax * 0.25)
							{
								num628 = 450;
							}
							else if ((double)npc.life < (double)npc.lifeMax * 0.5)
							{
								num628 = 500;
							}
							else if ((double)npc.life < (double)npc.lifeMax * 0.75)
							{
								num628 = 550;
							}
						}
						int num629 = 1;
						if (npc.position.X + (float)(npc.width / 2) < Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))
						{
							num629 = -1;
						}
						num628 -= (int)(100f * EnrageBonus);
						bool flag35 = false;
						if (npc.direction == num629 && Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) > (float)num628)
						{
							npc.ai[2] = 1f;
							flag35 = true;
						}
						if (Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) > (float)num628 * 1.5f)
						{
							npc.ai[2] = 1f;
							flag35 = true;
						}
						if (EnrageBonus > 0f && flag35)
						{
							npc.velocity *= 0.5f;
						}
						if (npc.ai[2] == 1f)
						{
							npc.TargetClosest();
							npc.spriteDirection = npc.direction;
							npc.localAI[0] = 0f;
							npc.velocity *= 0.9f;
							float num630 = 0.1f;
							if (Main.expertMode)
							{
								if (npc.life < npc.lifeMax / 2)
								{
									npc.velocity *= 0.9f;
									num630 += 0.05f;
								}
								if (npc.life < npc.lifeMax / 3)
								{
									npc.velocity *= 0.9f;
									num630 += 0.05f;
								}
								if (npc.life < npc.lifeMax / 5)
								{
									npc.velocity *= 0.9f;
									num630 += 0.05f;
								}
							}
							if (EnrageBonus > 0f)
							{
								npc.velocity *= 0.7f;
							}
							if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num630)
							{
								npc.ai[2] = 0f;
								npc.ai[1] += 1f;
								npc.netUpdate = true;
							}
						}
						else
						{
							npc.localAI[0] = 1f;
						}
					}
					else if (npc.ai[0] == 2f)
					{
						npc.TargetClosest();
						npc.spriteDirection = npc.direction;
						float num631 = 12f;
						float num632 = 0.07f;
						if (Main.expertMode)
						{
							num632 = 0.1f;
						}
						Vector2 vector77 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
						float num633 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector77.X;
						float num634 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - 200f - vector77.Y;
						float num635 = MathF.Sqrt(num633 * num633 + num634 * num634);
						if (num635 < 200f)
						{
							npc.ai[0] = 1f;
							npc.ai[1] = 0f;
							npc.netUpdate = true;
							return false;
						}
						num635 = num631 / num635;
						if (npc.velocity.X < num633)
						{
							npc.velocity.X += num632;
							if (npc.velocity.X < 0f && num633 > 0f)
							{
								npc.velocity.X += num632;
							}
						}
						else if (npc.velocity.X > num633)
						{
							npc.velocity.X -= num632;
							if (npc.velocity.X > 0f && num633 < 0f)
							{
								npc.velocity.X -= num632;
							}
						}
						if (npc.velocity.Y < num634)
						{
							npc.velocity.Y += num632;
							if (npc.velocity.Y < 0f && num634 > 0f)
							{
								npc.velocity.Y += num632;
							}
						}
						else if (npc.velocity.Y > num634)
						{
							npc.velocity.Y -= num632;
							if (npc.velocity.Y > 0f && num634 < 0f)
							{
								npc.velocity.Y -= num632;
							}
						}
					}
					else if (npc.ai[0] == 1f)
					{
						npc.localAI[0] = 0f;
						npc.TargetClosest();
						Vector2 vector78 = new Vector2(npc.position.X + (float)(npc.width / 2) + (float)(Main.rand.Next(20) * npc.direction), npc.position.Y + (float)npc.height * 0.8f);
						Vector2 vector79 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
						float num636 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector79.X;
						float num637 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector79.Y;
						float num638 = MathF.Sqrt(num636 * num636 + num637 * num637);
						npc.ai[1] += 1f;
						if (Main.expertMode)
						{
							int num639 = 0;
							for (int num640 = 0; num640 < 255; num640++)
							{
								if (Main.player[num640].active && !Main.player[num640].dead && (npc.Center - Main.player[num640].Center).Length() < 1000f)
								{
									num639++;
								}
							}
							npc.ai[1] += num639 / 2;
							if ((double)npc.life < (double)npc.lifeMax * 0.75)
							{
								npc.ai[1] += 0.25f;
							}
							if ((double)npc.life < (double)npc.lifeMax * 0.5)
							{
								npc.ai[1] += 0.25f;
							}
							if ((double)npc.life < (double)npc.lifeMax * 0.25)
							{
								npc.ai[1] += 0.25f;
							}
							if ((double)npc.life < (double)npc.lifeMax * 0.1)
							{
								npc.ai[1] += 0.25f;
							}
						}
						bool flag36 = false;
						int num641 = (int)(40f - 18f * EnrageBonus);
						if (Main.masterMode)
							num641 /= 2;
						if (npc.ai[1] > (float)num641)
						{
							npc.ai[1] = 0f;
							npc.ai[2]++;
							flag36 = true;
						}
						if (Collision.CanHit(vector78, 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && flag36)
						{
							SoundEngine.PlaySound(SoundID.NPCHit1 with { MaxInstances = 0 }, npc.position);
							if (Main.netMode != 1)
							{
								int num642 = Main.rand.Next(210, 212);
								int num643 = NPC.NewNPC(npc.GetSource_FromAI(), (int)vector78.X, (int)vector78.Y, num642);
								Main.npc[num643].velocity = Main.player[npc.target].Center - npc.Center;
								Main.npc[num643].velocity.Normalize();
								NPC nPC = Main.npc[num643];
								nPC.velocity *= 5f;
								Main.npc[num643].localAI[0] = 60f;
								Main.npc[num643].netUpdate = true;
							}
						}
						if (num638 > 400f || !Collision.CanHit(new Vector2(vector78.X, vector78.Y - 30f), 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
						{
							float num644 = 14f;
							float num645 = 0.1f;
							vector79 = vector78;
							num636 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector79.X;
							num637 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector79.Y;
							num638 = MathF.Sqrt(num636 * num636 + num637 * num637);
							num638 = num644 / num638;
							if (npc.velocity.X < num636)
							{
								npc.velocity.X += num645;
								if (npc.velocity.X < 0f && num636 > 0f)
								{
									npc.velocity.X += num645;
								}
							}
							else if (npc.velocity.X > num636)
							{
								npc.velocity.X -= num645;
								if (npc.velocity.X > 0f && num636 < 0f)
								{
									npc.velocity.X -= num645;
								}
							}
							if (npc.velocity.Y < num637)
							{
								npc.velocity.Y += num645;
								if (npc.velocity.Y < 0f && num637 > 0f)
								{
									npc.velocity.Y += num645;
								}
							}
							else if (npc.velocity.Y > num637)
							{
								npc.velocity.Y -= num645;
								if (npc.velocity.Y > 0f && num637 < 0f)
								{
									npc.velocity.Y -= num645;
								}
							}
						}
						else
						{
							npc.velocity *= 0.9f;
						}
						npc.spriteDirection = npc.direction;
						float maxbees = 5f;
						if (npc.ai[2] > maxbees)
						{
							if (npc.life < npc.lifeMax * 0.75f && Main.masterMode)
							{
								Vector2 vector80 = new Vector2(npc.position.X + (npc.width / 2) + (Main.rand.Next(20) * npc.direction), npc.position.Y + npc.height * 0.8f);
								float num652 = 13f;
								num652 += 7f * EnrageBonus;
								float num655 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector80.X;
								float num656 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector80.Y;
								float num657 = MathF.Sqrt(num655 * num655 + num656 * num656);
								num657 = num652 / num657;
								num655 *= num657;
								num656 *= num657;
								int num658 = 11;
								int num659 = 719;
								for (int i = 1; i <= 4 + bonus; i++)
								{
									float radians = 0.5f;
									if (bonus == 2)
										radians = 0.33f;
									Vector2 direction = new Vector2(num655, num656).RotatedBy(2 * Math.PI - radians * Math.PI * i);
									Projectile.NewProjectile(npc.GetSource_FromThis(), vector80, direction, num659, num658, 1f);
								}

							}
							npc.ai[0] = -1f;
							npc.ai[1] = 1f;
							npc.netUpdate = true;
						}
					}
					else if (npc.ai[0] == 3f)
					{
						float num646 = 4f;
						float num647 = 0.05f;
						if (Main.expertMode)
						{
							num646 = 6f;
							num647 = 0.075f;
						}
						num647 += 0.2f * EnrageBonus;
						num646 += 6f * EnrageBonus;
						Vector2 vector80 = new Vector2(npc.position.X + (float)(npc.width / 2) + (float)(Main.rand.Next(20) * npc.direction), npc.position.Y + (float)npc.height * 0.8f);
						Vector2 vector81 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
						float num648 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector81.X;
						float num649 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - 300f - vector81.Y;
						float num650 = MathF.Sqrt(num648 * num648 + num649 * num649);
						npc.ai[1] += 1f;
						int max = 40;
						if (Main.expertMode)
						{
							max = ((npc.life < npc.lifeMax * 0.1) ? 15 : ((npc.life < npc.lifeMax / 3) ? 25 : ((npc.life >= npc.lifeMax / 2) ? 35 : 30)));
						}
						max -= (int)(5f * EnrageBonus);
						if (npc.ai[1] % max == (max - 1) && npc.position.Y + (float)npc.height < Main.player[npc.target].position.Y && Collision.CanHit(vector80, 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
						{

							if (Main.netMode != 1)
							{
								float num652 = 8f;
								if (Main.expertMode)
								{
									num652 += 2f;
								}
								if (Main.expertMode && (double)npc.life < (double)npc.lifeMax * 0.1)
								{
									num652 += 3f;
								}
								num652 += 7f * EnrageBonus;
								int num653 = (int)(80f - 39f * EnrageBonus);
								int num654 = (int)(40f - 19f * EnrageBonus);
								float num655 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector80.X + (float)Main.rand.Next(-num653, num653 + 1);
								float num656 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector80.Y + (float)Main.rand.Next(-num654, num654 + 1);
								float num657 = MathF.Sqrt(num655 * num655 + num656 * num656);
								num657 = num652 / num657;
								num655 *= num657;
								num656 *= num657;
								int num658 = 11;
								int num659 = 719;
								if (Main.masterMode)
								{
									stingercounter++;
								}
								if (stingercounter >= 4)
								{
									SoundEngine.PlaySound(SoundID.Item97 with { MaxInstances = 0 }, npc.position);
									stingercounter = 0;
									float numberProjectiles = 3;
									float rotation = MathHelper.ToRadians(16 / bonus);
									for (int i = 0; i < numberProjectiles; i++)
									{
										Vector2 perturbedSpeed = new Vector2(num655, num656).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * (1 + 0.1f * bonus);
										Projectile.NewProjectile(npc.GetSource_FromThis(), vector80.X, vector80.Y, perturbedSpeed.X, perturbedSpeed.Y, num659, num658, 0f, Main.myPlayer);

									}
								}
								else
								{
									SoundEngine.PlaySound(SoundID.Item17 with { MaxInstances = 0 }, npc.position);
									Projectile.NewProjectile(npc.GetSource_FromThis(), vector80.X, vector80.Y, num655, num656, num659, num658, 0f, Main.myPlayer);
								}

							}
						}
						if (!Collision.CanHit(new Vector2(vector80.X, vector80.Y - 30f), 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
						{
							num646 = 14f;
							num647 = 0.1f;
							if (EnrageBonus > 0f)
							{
								num647 = 0.5f;
							}
							vector81 = vector80;
							num648 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector81.X;
							num649 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector81.Y;
							num650 = MathF.Sqrt(num648 * num648 + num649 * num649);
							num650 = num646 / num650;
							if (npc.velocity.X < num648)
							{
								npc.velocity.X += num647;
								if (npc.velocity.X < 0f && num648 > 0f)
								{
									npc.velocity.X += num647;
								}
							}
							else if (npc.velocity.X > num648)
							{
								npc.velocity.X -= num647;
								if (npc.velocity.X > 0f && num648 < 0f)
								{
									npc.velocity.X -= num647;
								}
							}
							if (npc.velocity.Y < num649)
							{
								npc.velocity.Y += num647;
								if (npc.velocity.Y < 0f && num649 > 0f)
								{
									npc.velocity.Y += num647;
								}
							}
							else if (npc.velocity.Y > num649)
							{
								npc.velocity.Y -= num647;
								if (npc.velocity.Y > 0f && num649 < 0f)
								{
									npc.velocity.Y -= num647;
								}
							}
						}
						else if (num650 > 100f)
						{
							npc.TargetClosest();
							npc.spriteDirection = npc.direction;
							num650 = num646 / num650;
							if (npc.velocity.X < num648)
							{
								npc.velocity.X += num647;
								if (npc.velocity.X < 0f && num648 > 0f)
								{
									npc.velocity.X += num647 * 2f;
								}
							}
							else if (npc.velocity.X > num648)
							{
								npc.velocity.X -= num647;
								if (npc.velocity.X > 0f && num648 < 0f)
								{
									npc.velocity.X -= num647 * 2f;
								}
							}
							if (npc.velocity.Y < num649)
							{
								npc.velocity.Y += num647;
								if (npc.velocity.Y < 0f && num649 > 0f)
								{
									npc.velocity.Y += num647 * 2f;
								}
							}
							else if (npc.velocity.Y > num649)
							{
								npc.velocity.Y -= num647;
								if (npc.velocity.Y > 0f && num649 < 0f)
								{
									npc.velocity.Y -= num647 * 2f;
								}
							}
						}
						float num661 = 20f;
						num661 -= 5f * EnrageBonus;
						if (npc.ai[1] > max * num661)
						{

							npc.ai[0] = -1f;
							npc.ai[1] = 3f;
							npc.netUpdate = true;
						}
					}
					else if (npc.ai[0] == 4f)
					{
						npc.localAI[0] = 1f;
						float num662 = 14f;
						float num663 = 14f;
						Vector2 vector82 = Main.player[npc.target].Center - npc.Center;
						vector82.Normalize();
						vector82 *= num662;
						npc.velocity = (npc.velocity * num663 + vector82) / (num663 + 1f);
						if (npc.velocity.X < 0f)
						{
							npc.direction = -1;
						}
						else
						{
							npc.direction = 1;
						}
						npc.spriteDirection = npc.direction;
						if (num617 < 2000f)
						{
							npc.ai[0] = -1f;
							npc.localAI[0] = 0f;
						}
					}
					return false;
				}
			}
			return true;
        }
    }
}