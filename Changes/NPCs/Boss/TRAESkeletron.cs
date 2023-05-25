using Microsoft.Xna.Framework;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs.Boss
{
	public class Skeletron : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public override void SetDefaults(NPC npc)
        {
            if (GetInstance<TRAEConfig>().SkeletronChanges)
            {
                if (npc.type == NPCID.SkeletronHead)
                {
                    npc.lifeMax = (int)(npc.lifeMax * ((float)3500 / 4400));
                }
                if (npc.type == NPCID.SkeletronHand)
                {
                    npc.lifeMax = (int)(npc.lifeMax * ((float)800 / 600));
                    // basically we take HP off of the head but add it back to the hands
                }
            }
        }

        void RageDusts(NPC npc)
		{
            SoundEngine.PlaySound(SoundID.ForceRoarPitched with { MaxInstances = 0 }, npc.Center);
            Vector2 spinningpoint1 = ((float)Main.rand.NextDouble() * 6.283185f).ToRotationVector2();
            Vector2 spinningpoint2 = spinningpoint1;
            float fourToSix = Main.rand.Next(2, 3) * 2;
            int ten = 10;
            float OneOrMinusOne = Main.rand.Next(2) == 0 ? 1f : -1f; // one in three chance of it being 1
            bool flag = true;
            for (int i = 0; i < ten * fourToSix; ++i) // makes 20 or 30 dusts total
            {
                if (i % ten == 0)
                {
                    spinningpoint2 = spinningpoint2.RotatedBy(OneOrMinusOne * (6.28318548202515 / fourToSix), default);
                    spinningpoint1 = spinningpoint2;
                    flag = !flag;
                }
                else
                {
                    float num4 = 6.283185f / (ten * fourToSix);
                    spinningpoint1 = spinningpoint1.RotatedBy(num4 * OneOrMinusOne * 3.0, default);
                }
                float num5 = MathHelper.Lerp(7.5f, 60f, i % ten / ten);
                int index2 = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y), 6, 6, DustID.TreasureSparkle, 0.0f, 0.0f, 100, default, 3f);
                Dust dust1 = Main.dust[index2];
                dust1.velocity = Vector2.Multiply(dust1.velocity, 0.1f);
                Dust dust2 = Main.dust[index2];
                dust2.velocity = Vector2.Add(dust2.velocity, Vector2.Multiply(spinningpoint1, num5));
                if (flag)
                    Main.dust[index2].scale = 0.9f;
                Main.dust[index2].noGravity = true;
            }
        }
        public override bool PreAI(NPC npc)
        {
			if (GetInstance<TRAEConfig>().SkeletronChanges)
			{
				if (npc.type == NPCID.SkeletronHead)
				{
					
					npc.defense = npc.defDefense;
					if (npc.ai[0] == 0f && Main.netMode != NetmodeID.MultiplayerClient)
					{
						npc.TargetClosest();
						npc.ai[0] = 1f;
						int Hand = NPC.NewNPC(NPC.GetBossSpawnSource(npc.target), (int)(npc.position.X + (npc.width / 2)), (int)npc.position.Y + npc.height / 2, NPCID.SkeletronHand, npc.whoAmI);
						Main.npc[Hand].ai[0] = -1f;
						Main.npc[Hand].ai[1] = npc.whoAmI;
						Main.npc[Hand].target = npc.target;
						Main.npc[Hand].netUpdate = true;
						Hand = NPC.NewNPC(NPC.GetBossSpawnSource(npc.target), (int)(npc.position.X + (npc.width / 2)), (int)npc.position.Y + npc.height / 2, 36, npc.whoAmI);
						Main.npc[Hand].ai[0] = 1f;
						Main.npc[Hand].ai[1] = npc.whoAmI;
						Main.npc[Hand].ai[3] = 150f;
						Main.npc[Hand].target = npc.target;
						Main.npc[Hand].netUpdate = true;


					}
					// TURN INTO DG MODE?
					if ((/*type == 68 ||*/ Main.netMode == 1) && npc.localAI[0] == 0f)
					{
						npc.localAI[0] = 1f;
						SoundEngine.PlaySound(SoundID.Roar with { MaxInstances = 0 }, npc.Center);
					}
					if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
					{
						npc.TargetClosest();
						if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > 2000f || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
						{
							npc.ai[1] = 3f;
						}
					}
					if (Main.dayTime && npc.ai[1] != 3f && npc.ai[1] != 2f)
					{
						npc.ai[1] = 2f;
						SoundEngine.PlaySound(SoundID.Roar with { MaxInstances = 0 }, npc.Center);
					}
					int Hands = 0;
					// expert changes (more defense with more hands, shoot skulls)
					if (Main.expertMode)
					{
						for (int num158 = 0; num158 < 200; num158++)
						{
							if (Main.npc[num158].active && Main.npc[num158].type == NPCID.SkeletronHand)
							{
								Hands++;
							}
						}
						if (Hands > 0)
						{
							npc.defense += Hands * 50;
							if (Main.rand.Next(10) < 3 * Hands)
							{
								Rectangle r3 = Utils.CenteredRectangle(npc.Center, Vector2.One * npc.width);
								int num3 = Dust.NewDust(r3.TopLeft(), r3.Width, r3.Height, 204, 0f, 0f, 150, default, 0.3f);
								Main.dust[num3].fadeIn = 1f;
								Main.dust[num3].velocity *= 0.1f;
								Main.dust[num3].noLight = true;
							}
						}
						if ((Hands < 2 || (double)npc.life < (double)npc.lifeMax * 0.75) && npc.ai[1] == 0f)
						{
							float fireRate = 80f;
							if (Hands == 0)
							{
								float ShadowflameSkullRate = 480f;
								if (Main.masterMode && Main.netMode != 1 && npc.ai[2] % ShadowflameSkullRate == 0f)
								{
									float baseVelocity = 6f;
									float playerX2 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - npc.Center.X + Main.rand.Next(-20, 21);
									float playerY2 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - npc.Center.Y + Main.rand.Next(-20, 21);
									float distance2 = (float)Math.Sqrt(playerX2 * playerX2 + playerY2 * playerY2);
									distance2 = baseVelocity / distance2;
									playerX2 *= distance2;
									playerY2 *= distance2;
									int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(22f, 22f);
									Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center.X, npc.Center.Y, playerX2, playerY2, ProjectileType<ShadowflameSkull>(), attackDamage_ForProjectiles, 0f, Main.myPlayer, -1f);
								}
								fireRate /= 2f;
							}
							if (Main.getGoodWorld)
							{
								fireRate *= 0.8f;
							}
							if (Main.netMode != NetmodeID.MultiplayerClient && npc.ai[2] % fireRate == 0f)
							{
								Vector2 center3 = npc.Center;
								if (Collision.CanHit(center3, 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
								{
									float baseVelocity = 3f;
									if (Hands == 0)
									{
										baseVelocity += 2f;
									}
									float playerX2 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - center3.X + Main.rand.Next(-20, 21);
									float playerY2 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - center3.Y + Main.rand.Next(-20, 21);
									float distance2 = (float)Math.Sqrt(playerX2 * playerX2 + playerY2 * playerY2);
									distance2 = baseVelocity / distance2;
									playerX2 *= distance2;
									playerY2 *= distance2;
									Vector2 totalVelocity = new Vector2(playerX2 * 1f + Main.rand.Next(-50, 51) * 0.01f, playerY2 * 1f + Main.rand.Next(-50, 51) * 0.01f);
									totalVelocity.Normalize();
									totalVelocity *= baseVelocity;
									totalVelocity += npc.velocity;
									playerX2 = totalVelocity.X;
									playerY2 = totalVelocity.Y;
									int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(17f, 17f);
									center3 += totalVelocity * 5f;
									int Skull = Projectile.NewProjectile(npc.GetSource_FromThis(), center3.X, center3.Y, playerX2, playerY2, ProjectileID.Skull, attackDamage_ForProjectiles, 0f, Main.myPlayer, -1f);
									Main.projectile[Skull].timeLeft = 300;
								}
							}

						}
					}
					if (Main.masterMode && npc.life < (int)(npc.lifeMax * 0.15f) && Hands == 0)
					{
                        Rectangle r3 = Utils.CenteredRectangle(npc.Center, Vector2.One * npc.width);
                        Dust.NewDust(r3.TopLeft(), r3.Width, r3.Height, DustID.Shadowflame, 0f, 0f, 0, default, 1f);

						if (Main.rand.NextBool(3))
						{
							int num3 = Dust.NewDust(r3.TopLeft(), r3.Width, r3.Height, DustID.TreasureSparkle, 0f, 0f, 0, default, 0.3f);
							Main.dust[num3].fadeIn = 1f;
							Main.dust[num3].velocity *= 0.1f;
							Main.dust[num3].noLight = true;
						}
                        npc.ai[1] = 1f;

						npc.defense -= 10;
						if (npc.ai[2] != -1f && npc.ai[2] != -2f)
                        {
							RageDusts(npc);
                        
                            npc.ai[2] = -1f;


                        }
                        if (npc.ai[2] == -1f && npc.life < (int)(npc.lifeMax * 0.075f))
                        {
                            RageDusts(npc);
                            npc.ai[2] = -2f;
                        }
                        npc.rotation += npc.direction * 0.3f;
						Vector2 skeletronPosition = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
						float playerX3 = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2) - skeletronPosition.X;
						float playerY3 = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - skeletronPosition.Y;
						float distance3 = (float)Math.Sqrt(playerX3 * playerX3 + playerY3 * playerY3);
						float spinVelocity = 6f;
                        npc.damage = npc.GetAttackDamage_LerpBetweenFinalValues(npc.defDamage, npc.defDamage * 1.3f);

                        if (distance3 > 150f)
                        {
                            double extraSpeed = Math.Pow(1.1, ((double)distance3 - 100) / 50);
                            spinVelocity *= (float)extraSpeed;
                        }
						if (npc.life < (int)(npc.lifeMax * 0.05f))
							spinVelocity *= 1.2f;
                        distance3 = spinVelocity / distance3;
                        npc.velocity.X = playerX3 * distance3;
                        npc.velocity.Y = playerY3 * distance3;




                        return false;

                    }
                    // hovering above the player
                    if (npc.ai[1] == 0f)
					{
						npc.damage = npc.defDamage;
						npc.ai[2] += 1f;
						if (npc.ai[2] >= 800f)
						{
							npc.ai[2] = 0f;
							npc.ai[1] = 1f;
							npc.TargetClosest();
							npc.netUpdate = true;
						}
						npc.rotation = npc.velocity.X / 15f;
						float num169 = 0.02f;
						float num170 = 2f;
						float num171 = 0.05f;
						float distanceAboveThePlayer = 8f;
						if (Main.expertMode)
						{
							num169 = 0.03f;
							num170 = 4f;
							num171 = 0.07f;
							distanceAboveThePlayer = 9.5f;
						}
						if (Main.getGoodWorld)
						{
							num169 += 0.01f;
							num170 += 1f;
							num171 += 0.05f;
							distanceAboveThePlayer += 2f;
						}
						if (npc.position.Y > Main.player[npc.target].position.Y - 250f)
						{
							if (npc.velocity.Y > 0f)
							{
								npc.velocity.Y *= 0.98f;
							}
							npc.velocity.Y -= num169;
							if (npc.velocity.Y > num170)
							{
								npc.velocity.Y = num170;
							}
						}
						else if (npc.position.Y < Main.player[npc.target].position.Y - 250f)
						{
							if (npc.velocity.Y < 0f)
							{
								npc.velocity.Y *= 0.98f;
							}
							npc.velocity.Y += num169;
							if (npc.velocity.Y < 0f - num170)
							{
								npc.velocity.Y = 0f - num170;
							}
						}
						if (npc.position.X + (npc.width / 2) > Main.player[npc.target].position.X + (Main.player[npc.target].width / 2))
						{
							if (npc.velocity.X > 0f)
							{
								npc.velocity.X *= 0.98f;
							}
							npc.velocity.X -= num171;
							if (npc.velocity.X > distanceAboveThePlayer)
							{
								npc.velocity.X = distanceAboveThePlayer;
							}
						}
						if (npc.position.X + (npc.width / 2) < Main.player[npc.target].position.X + (Main.player[npc.target].width / 2))
						{
							if (npc.velocity.X < 0f)
							{
								npc.velocity.X *= 0.98f;
							}
							npc.velocity.X += num171;
							if (npc.velocity.X < 0f - distanceAboveThePlayer)
							{
								npc.velocity.X = 0f - distanceAboveThePlayer;
							}
						}
					}
					else if (npc.ai[1] == 1f)
					{
						npc.defense -= 10;
						npc.ai[2] += 1f;
						if (npc.ai[2] == 2f)
						{
							SoundEngine.PlaySound(SoundID.Roar with { MaxInstances = 0 }, npc.Center);
						}
						if (npc.ai[2] >= 400f)
						{
							npc.ai[2] = 0f;
							npc.ai[1] = 0f;
						}
						npc.rotation += npc.direction * 0.3f;
						Vector2 skeletronPosition = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
						float playerX3 = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2) - skeletronPosition.X;
						float playerY3 = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - skeletronPosition.Y;
						float distance3 = (float)Math.Sqrt(playerX3 * playerX3 + playerY3 * playerY3);
						float spinVelocity = 3.5f;

						npc.damage = npc.GetAttackDamage_LerpBetweenFinalValues(npc.defDamage, npc.defDamage * 1.3f);
						if (Main.expertMode)
						{
							if (distance3 > 150f)
							{
								double extraSpeed = Math.Pow(1.1, ((double)distance3 - 100) / 50);
								spinVelocity *= (float)extraSpeed;
							}

							switch (Hands)
							{
								case 0:
									spinVelocity *= 1.1f;
									break;
								case 1:
									spinVelocity *= 1.05f;
									break;
							}
							if (Main.masterMode && Hands == 0)
							{

								if (npc.life <= (int)(npc.lifeMax * 0.60f))
								{
                                    spinVelocity *= 1.60f;

								}
							}
						}
						if (Main.getGoodWorld)
						{
							spinVelocity *= 1.3f;
						}
						distance3 = spinVelocity / distance3;
						npc.velocity.X = playerX3 * distance3;
						npc.velocity.Y = playerY3 * distance3;
					}
					else if (npc.ai[1] == 2f)
					{
						npc.damage = 1000;
						npc.defense = 9999;
						npc.rotation += npc.direction * 0.3f;
						Vector2 vector21 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
						float num177 = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2) - vector21.X;
						float num178 = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - vector21.Y;
						float num179 = (float)Math.Sqrt(num177 * num177 + num178 * num178);
						num179 = 8f / num179;
						npc.velocity.X = num177 * num179;
						npc.velocity.Y = num178 * num179;
					}
					else if (npc.ai[1] == 3f)
					{
						npc.velocity.Y += 0.1f;
						if (npc.velocity.Y < 0f)
						{
							npc.velocity.Y *= 0.95f;
						}
						npc.velocity.X *= 0.95f;
						npc.EncourageDespawn(50);
					}
					if (npc.ai[1] != 2f && npc.ai[1] != 3f && (Hands != 0 || !Main.expertMode))
					{
						int num180 = Dust.NewDust(new Vector2(npc.position.X + (npc.width / 2) - 15f - npc.velocity.X * 5f, npc.position.Y + npc.height - 2f), 30, 10, 5, (0f - npc.velocity.X) * 0.2f, 3f, 0, default, 2f);
						Main.dust[num180].noGravity = true;
						Main.dust[num180].velocity.X *= 1.3f;
						Main.dust[num180].velocity.X += npc.velocity.X * 0.4f;
						Main.dust[num180].velocity.Y += 2f + npc.velocity.Y;
						for (int num181 = 0; num181 < 2; num181++)
						{
							num180 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 120f), npc.width, 60, 5, npc.velocity.X, npc.velocity.Y, 0, default, 2f);
							Main.dust[num180].noGravity = true;
							Dust dust = Main.dust[num180];
							dust.velocity -= npc.velocity;
							Main.dust[num180].velocity.Y += 5f;
						}
					}

					return false;
				}
				if (npc.type == NPCID.SkeletronHand)
				{
					// npc.ai[0] = hand orientation 
					// npc.ai[1] = skeletron whose arms belong to
					npc.spriteDirection = -(int)npc.ai[0];
					if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].aiStyle != 11)
					{
						npc.ai[2] += 10f;
						if (npc.ai[2] > 50f || Main.netMode != 2)
						{
							npc.life = -1;
							npc.HitEffect();
							npc.active = false;
						}
					}
					if (npc.ai[2] == 0f || npc.ai[2] == 3f)
					{
						if (Main.npc[(int)npc.ai[1]].ai[1] == 3f)
						{
							npc.EncourageDespawn(10);
						}
						if (Main.npc[(int)npc.ai[1]].ai[1] != 0f)
						{
							if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y - 100f)
							{
								if (npc.velocity.Y > 0f)
								{
									npc.velocity.Y *= 0.96f;
								}
								npc.velocity.Y -= 0.07f;
								if (npc.velocity.Y > 6f)
								{
									npc.velocity.Y = 6f;
								}
							}
							else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 100f)
							{
								if (npc.velocity.Y < 0f)
								{
									npc.velocity.Y *= 0.96f;
								}
								npc.velocity.Y += 0.07f;
								if (npc.velocity.Y < -6f)
								{
									npc.velocity.Y = -6f;
								}
							}
							if (npc.position.X + (npc.width / 2) > Main.npc[(int)npc.ai[1]].position.X + (Main.npc[(int)npc.ai[1]].width / 2) - 120f * npc.ai[0])
							{
								if (npc.velocity.X > 0f)
								{
									npc.velocity.X *= 0.96f;
								}
								npc.velocity.X -= 0.1f;
								if (npc.velocity.X > 8f)
								{
									npc.velocity.X = 8f;
								}
							}
							if (npc.position.X + (npc.width / 2) < Main.npc[(int)npc.ai[1]].position.X + (Main.npc[(int)npc.ai[1]].width / 2) - 120f * npc.ai[0])
							{
								if (npc.velocity.X < 0f)
								{
									npc.velocity.X *= 0.96f;
								}
								npc.velocity.X += 0.1f;
								if (npc.velocity.X < -8f)
								{
									npc.velocity.X = -8f;
								}
							}
						}
						else
						{
							npc.ai[3] += 1f;
							if (Main.expertMode)
							{
								npc.ai[3] += 0.5f;
							}
							if (npc.ai[3] >= 300f)
							{
								npc.ai[2] += 1f;
								npc.ai[3] = 0f;
								npc.netUpdate = true;
							}
							if (Main.expertMode)
							{
								if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y + 230f)
								{
									if (npc.velocity.Y > 0f)
									{
										npc.velocity.Y *= 0.96f;
									}
									npc.velocity.Y -= 0.04f;
									if (npc.velocity.Y > 3f)
									{
										npc.velocity.Y = 3f;
									}
								}
								else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y + 230f)
								{
									if (npc.velocity.Y < 0f)
									{
										npc.velocity.Y *= 0.96f;
									}
									npc.velocity.Y += 0.04f;
									if (npc.velocity.Y < -3f)
									{
										npc.velocity.Y = -3f;
									}
								}
								if (npc.position.X + (npc.width / 2) > Main.npc[(int)npc.ai[1]].position.X + (Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0])
								{
									if (npc.velocity.X > 0f)
									{
										npc.velocity.X *= 0.96f;
									}
									npc.velocity.X -= 0.07f;
									if (npc.velocity.X > 8f)
									{
										npc.velocity.X = 8f;
									}
								}
								if (npc.position.X + (npc.width / 2) < Main.npc[(int)npc.ai[1]].position.X + (Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0])
								{
									if (npc.velocity.X < 0f)
									{
										npc.velocity.X *= 0.96f;
									}
									npc.velocity.X += 0.07f;
									if (npc.velocity.X < -8f)
									{
										npc.velocity.X = -8f;
									}
								}
							}
							if (npc.position.Y > Main.npc[(int)npc.ai[1]].position.Y + 230f)
							{
								if (npc.velocity.Y > 0f)
								{
									npc.velocity.Y *= 0.96f;
								}
								npc.velocity.Y -= 0.04f;
								if (npc.velocity.Y > 3f)
								{
									npc.velocity.Y = 3f;
								}
							}
							else if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y + 230f)
							{
								if (npc.velocity.Y < 0f)
								{
									npc.velocity.Y *= 0.96f;
								}
								npc.velocity.Y += 0.04f;
								if (npc.velocity.Y < -3f)
								{
									npc.velocity.Y = -3f;
								}
							}
							if (npc.position.X + (npc.width / 2) > Main.npc[(int)npc.ai[1]].position.X + (Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0])
							{
								if (npc.velocity.X > 0f)
								{
									npc.velocity.X *= 0.96f;
								}
								npc.velocity.X -= 0.07f;
								if (npc.velocity.X > 8f)
								{
									npc.velocity.X = 8f;
								}
							}
							if (npc.position.X + (npc.width / 2) < Main.npc[(int)npc.ai[1]].position.X + (Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0])
							{
								if (npc.velocity.X < 0f)
								{
									npc.velocity.X *= 0.96f;
								}
								npc.velocity.X += 0.07f;
								if (npc.velocity.X < -8f)
								{
									npc.velocity.X = -8f;
								}
							}
						}
						Vector2 vector22 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
						float num182 = Main.npc[(int)npc.ai[1]].position.X + (Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0] - vector22.X;
						float num183 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector22.Y;
						npc.rotation = (float)Math.Atan2(num183, num182) + 1.57f;
					}
					else if (npc.ai[2] == 1f)
					{
						Vector2 vector23 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
						float num185 = Main.npc[(int)npc.ai[1]].position.X + (Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0] - vector23.X;
						float num186 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector23.Y;
						npc.rotation = (float)Math.Atan2(num186, num185) + 1.57f;
						npc.velocity.X *= 0.95f;
						npc.velocity.Y -= 0.1f;
						if (Main.expertMode)
						{
							npc.velocity.Y -= 0.06f;
							if (npc.velocity.Y < -13f)
							{
								npc.velocity.Y = -13f;
							}
						}
						else if (npc.velocity.Y < -8f)
						{
							npc.velocity.Y = -8f;
						}
						if (npc.position.Y < Main.npc[(int)npc.ai[1]].position.Y - 200f)
						{
							npc.TargetClosest();
							npc.ai[2] = 2f;
							vector23 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
							num185 = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2) - vector23.X;
							num186 = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - vector23.Y;
							float num187 = (float)Math.Sqrt(num185 * num185 + num186 * num186);
							float factor = 18f;
							if (Main.expertMode)
							{
								factor = 21f;
							}
							if (Main.masterMode)
							{
								factor = 36f;
							}
							num187 = factor / num187;
							npc.velocity.X = num185 * num187;
							npc.velocity.Y = num186 * num187;
							npc.netUpdate = true;
						}
					}
					else if (npc.ai[2] == 2f)
					{
						if (npc.position.Y > Main.player[npc.target].position.Y || npc.velocity.Y < 0f)
						{
							npc.ai[2] = 3f;
						}
					}
					else if (npc.ai[2] == 4f)
					{
						Vector2 vector24 = new(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
						float num188 = Main.npc[(int)npc.ai[1]].position.X + (Main.npc[(int)npc.ai[1]].width / 2) - 200f * npc.ai[0] - vector24.X;
						float num189 = Main.npc[(int)npc.ai[1]].position.Y + 230f - vector24.Y;
						npc.rotation = (float)Math.Atan2(num189, num188) + 1.57f;
						npc.velocity.Y *= 0.95f;
						npc.velocity.X += 0.1f * (0f - npc.ai[0]);
						if (Main.expertMode)
						{
							npc.velocity.X += 0.07f * (0f - npc.ai[0]);
							if (npc.velocity.X < -12f)
							{
								npc.velocity.X = -12f;
							}
							else if (npc.velocity.X > 12f)
							{
								npc.velocity.X = 12f;
							}
						}
						else if (npc.velocity.X < -8f)
						{
							npc.velocity.X = -8f;
						}
						else if (npc.velocity.X > 8f)
						{
							npc.velocity.X = 8f;
						}
						if (npc.position.X + (npc.width / 2) < Main.npc[(int)npc.ai[1]].position.X + (Main.npc[(int)npc.ai[1]].width / 2) - 500f || npc.position.X + (npc.width / 2) > Main.npc[(int)npc.ai[1]].position.X + (Main.npc[(int)npc.ai[1]].width / 2) + 500f)
						{
							npc.TargetClosest();
							npc.ai[2] = 5f;
							vector24 = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
							num188 = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2) - vector24.X;
							num189 = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2) - vector24.Y;
							float num190 = (float)Math.Sqrt(num188 * num188 + num189 * num189);
							float factor = 18f;
							if (Main.expertMode)
							{
								factor = 21f;
							}
							if (Main.masterMode)
							{
								factor = 40f;
							}
							num190 = factor / num190;
							npc.velocity.X = num188 * num190;
							npc.velocity.Y = num189 * num190;
							npc.netUpdate = true;
						}
					}
					else if (npc.ai[2] == 5f && ((npc.velocity.X > 0f && npc.position.X + (npc.width / 2) > Main.player[npc.target].position.X + (Main.player[npc.target].width / 2)) || (npc.velocity.X < 0f && npc.position.X + (npc.width / 2) < Main.player[npc.target].position.X + (Main.player[npc.target].width / 2))))
					{
						npc.ai[2] = 0f;
					}
					return false;

				}
			}
			return true;
		}
        public override bool PreKill(NPC npc)
        {
            if (npc.type == NPCID.SkeletronHead)
            {
				for (int i = 0; i < 1000; i++)
				{
					if (Main.projectile[i].type == ProjectileType<ShadowflameSkull>())
					{
						Main.projectile[i].Kill();
					}
				}
			}
			return true;
        }
    }
	public class ShadowflameSkull : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Shadowflame Skull");
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.alpha = 255;
			Projectile.timeLeft = 600;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.scale = 1.3f;
		}
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			target.AddBuff(BuffID.ShadowFlame, 750);
		}
		public override void AI()
		{
			if (Projectile.localAI[0] == 0f)
			{
				Projectile.localAI[0] = 1f;
				SoundEngine.PlaySound(SoundID.Item8 with { MaxInstances = 0 }, Projectile.position);
				for (int num134 = 0; num134 < 3; num134++)
				{
					int num135 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 27, Projectile.velocity.X, Projectile.velocity.Y, 0, default, 2f);
					Main.dust[num135].noGravity = true;
					Main.dust[num135].velocity = Projectile.Center - Main.dust[num135].position;
					Main.dust[num135].velocity.Normalize();
					Main.dust[num135].velocity *= -5f;
					Main.dust[num135].velocity += Projectile.velocity / 2f;
					Main.dust[num135].noLight = true;
				}
			}
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 50;
			}
			if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 12)
			{
				Projectile.frameCounter = 0;
			}
			Projectile.frame = Projectile.frameCounter / 2;
			if (Projectile.frame > 3)
			{
				Projectile.frame = 6 - Projectile.frame;
			}
			Vector3 vector24 = NPCID.Sets.MagicAuraColor[54].ToVector3();
			Lighting.AddLight(Projectile.Center, vector24.X, vector24.Y, vector24.Z);
			if (Main.rand.Next(3) == 0)
			{
				int num136 = Dust.NewDust(new Vector2(Projectile.position.X + 4f, Projectile.position.Y + 4f), Projectile.width - 8, Projectile.height - 8, 27, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 2f);
				Main.dust[num136].position -= Projectile.velocity * 2f;
				Main.dust[num136].noLight = true;
				Main.dust[num136].noGravity = true;
				Main.dust[num136].velocity.X *= 0.3f;
				Main.dust[num136].velocity.Y *= 0.3f;
			}
		   Projectile.spriteDirection = Projectile.direction;
			if (Projectile.direction < 0)
			{
				Projectile.rotation = (float)Math.Atan2(0f - Projectile.velocity.Y, 0f - Projectile.velocity.X);
			}
			else
			{
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
			}
			for (int index1 = 0; index1 < 255; index1++)
			{
				if (index1 >= 0 && Main.player[index1].active && !Main.player[index1].dead)
				{
					if (Projectile.Distance(Main.player[index1].Center) <= (double)100)
						return;
					Vector2 unitY = Projectile.DirectionTo(Main.player[index1].Center);
					if (unitY.HasNaNs())
						unitY = Vector2.UnitY;
					Projectile.velocity = Vector2.Multiply(unitY, 6f);
				}
			}
			

		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Dig with { MaxInstances = 0 }, Projectile.Center);
			for (int index1 = 0; index1 < 20; ++index1)
			{
				int index2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Bone, 0.0f, 0.0f, 100, default, 1f);
				Main.dust[index2].noGravity = true;
				Dust dust1 = Main.dust[index2];
				dust1.velocity = Vector2.Multiply(dust1.velocity, 1.2f);
				Main.dust[index2].scale = 1.3f;
				Dust dust2 = Main.dust[index2];
				dust2.velocity = Vector2.Subtract(dust2.velocity, Vector2.Multiply(Projectile.oldVelocity, 0.3f));
				int index3 = Dust.NewDust(new Vector2((float)(Projectile.position.X + 4.0), (float)(Projectile.position.Y + 4.0)), Projectile.width - 8, Projectile.height - 8, DustID.Shadowflame, 0.0f, 0.0f, 100, default, 2f);
				Main.dust[index3].noGravity = true;
				Dust dust3 = Main.dust[index3];
				dust3.velocity = Vector2.Multiply(dust3.velocity, 3f);
			}
		}
		public override Color? GetAlpha(Color newColor)
		{
			byte a = newColor.A;
			newColor = Color.Lerp(newColor, Color.White, 0.5f);
			newColor.A = a;
			return newColor;
		}	
	}	
}