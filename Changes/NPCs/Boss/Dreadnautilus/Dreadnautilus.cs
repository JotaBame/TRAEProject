using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TRAEProject.NewContent.Items.DreadItems.DreadRelic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.DreadItems.BloodWings;
using TRAEProject.NewContent.Items.DreadItems.BossBag;
using TRAEProject.NewContent.Items.DreadItems.BottomlessChumBucket;
using TRAEProject.NewContent.Items.DreadItems.Brimstone;
using TRAEProject.NewContent.Items.DreadItems.DreadSummon;
using TRAEProject.NewContent.Items.DreadItems.DreadTrophy;
using TRAEProject.NewContent.Items.DreadItems.ShellSpinner;
using TRAEProject.NewContent.Items.DreadItems.BloodSquidEgg;
using TRAEProject.NewContent.Items.DreadItems.BloodBoiler;
using TRAEProject.NewContent.Items.DreadItems.DreadMask;
using static Terraria.ModLoader.ModContent;
using TRAEProject;
namespace TRAEProject.Changes.NPCs.Boss.Dreadnautilus
{
	class Dreadnautilus : GlobalNPC
	{
		public override void SetDefaults(NPC npc)
		{
			if (npc.type == NPCID.BloodNautilus)
			{
				npc.boss = true;
				npc.lifeMax = 30000;
				npc.defense = 32;
				npc.catchItem = (short)ItemType<DreadSummon>();
			}
		}
		public override void ScaleExpertStats(NPC npc, int numPlayers, float bossLifeScale)
		{
			if (npc.type == NPCID.BloodNautilus)
			{
				npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
				if (Main.masterMode)
				{
					npc.lifeMax = (int)(npc.lifeMax * 0.8f);
				}
			}
		}
        public override bool PreKill(NPC npc)
        {
			if (npc.type == NPCID.BloodNautilus)
			{
				NPCLoader.blockLoot.Add(ItemID.SanguineStaff);
				NPCLoader.blockLoot.Add(ItemID.BloodMoonMonolith);
				NPCLoader.blockLoot.Add(ItemID.ChumBucket);
				NPCLoader.blockLoot.Add(ItemID.BloodMoonStarter);
			}
			return base.PreKill(npc);
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
			if (npc.type == NPCID.BloodNautilus)
            {
				//Add the treasure bag (automatically checks for expert mode)
				npcLoot.Add(ItemDropRule.BossBag(ItemType<DreadBag>())); //this requires you to set BossBag in SetDefaults accordingly
				//
				LeadingConditionRule MasterRule = new LeadingConditionRule(new Conditions.IsMasterMode());
				//All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
				LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			
				//Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
				notExpertRule.OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, ItemType<ShellSpinner>(), ItemType<ShellSpinner>(), ItemType<Brimstone>(), ItemType<BloodBoiler>(), ItemID.SanguineStaff));
				//Finally add the leading rule
				npcLoot.Add(notExpertRule);

				//Trophies are spawned with 1/10 chance
				npcLoot.Add(ItemDropRule.Common(ItemType<DreadTrophy>(), 10));
				//Boss masks are spawned with 1/7 chance
				notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
				notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<DreadMask>(), 7));
				npcLoot.Add(notExpertRule);

				notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
				notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.BloodHamaxe, 8));
				npcLoot.Add(notExpertRule);

				notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
				notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<BloodWings>(), 13));
				npcLoot.Add(notExpertRule);
				
				notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
				notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.BloodMoonMonolith, 12));
				npcLoot.Add(notExpertRule);

				notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
				notExpertRule.OnSuccess(ItemDropRule.Common(ItemType<BottomlessChumBucket>(), 12));
				npcLoot.Add(notExpertRule);

				notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
				notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.ChumBucket, 1, 5, 9));
				npcLoot.Add(notExpertRule);

				notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
				notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.BloodMoonStarter, 2));
				npcLoot.Add(notExpertRule);
				
				MasterRule = new LeadingConditionRule(new Conditions.IsMasterMode()); 
				MasterRule.OnSuccess(ItemDropRule.Common(ItemType<DreadnautilusRelic>()));
				MasterRule.OnSuccess(ItemDropRule.Common(ItemType<BloodSquidEgg>(), 4));
				npcLoot.Add(MasterRule);
		
			}

		}
        public override void HitEffect(NPC npc, int hitDirection, double damage)
		{
			if (npc.type == NPCID.BloodNautilus)
			{
				if (npc.life > 0)
				{
					for (int num65 = 0; (double)num65 < damage / (double)npc.lifeMax * 200.0; num65++)
					{
						Dust dust10 = Dust.NewDustDirect(npc.position, npc.width, npc.height, 5, hitDirection, -1f);
						Dust dust = dust10;
						dust.velocity *= 2.5f;
						dust10.scale = 2f;
					}
				}
				else
				{
					for (int num66 = 0; (float)num66 < 200f; num66++)
					{
						Dust dust11 = Dust.NewDustDirect(npc.position, npc.width, npc.height, 5, hitDirection, -1f);
						Dust dust = dust11;
						dust.velocity *= 2.5f;
						dust11.scale = 2.5f;
					}
					int num67 = 1172;
					//Gore.NewGore(new Vector2(npc.Right.X - 30f, npc.position.Y), npc.velocity, num67, npc.scale);
					//Gore.NewGore(npc.position, npc.velocity, num67, npc.scale);
					//Gore.NewGore(new Vector2(npc.Right.X - 30f, npc.position.Y), npc.velocity, num67 + 1, npc.scale);
					//Gore.NewGore(npc.position, npc.velocity, num67 + 1, npc.scale);
					Gore.NewGore(npc.position, npc.velocity, num67 + 2, npc.scale);
					//Gore.NewGore(npc.position, npc.velocity, num67 + 3, npc.scale);
					Gore.NewGore(npc.position, npc.velocity, num67 + 4, npc.scale);
					Gore.NewGore(npc.position, npc.velocity, num67 + 4, npc.scale);
					Gore.NewGore(npc.position, npc.velocity, num67 + 5, npc.scale);
					Gore.NewGore(npc.position, npc.velocity, num67 + 5, npc.scale);
					Gore.NewGore(npc.position, npc.velocity, num67 + 6, npc.scale);
					Gore.NewGore(npc.position, npc.velocity, num67 + 6, npc.scale);
				}
			}
		}
		void BreakShell(NPC npc)
        {
			int num67 = 1172;
			Gore.NewGore(new Vector2(npc.Right.X - 30f, npc.position.Y), npc.velocity, num67, npc.scale);
			Gore.NewGore(npc.position, npc.velocity, num67, npc.scale);
			Gore.NewGore(new Vector2(npc.Right.X - 30f, npc.position.Y), npc.velocity, num67 + 1, npc.scale);
			Gore.NewGore(npc.position, npc.velocity, num67 + 1, npc.scale);
			//Gore.NewGore(npc.position, npc.velocity, num67 + 2, npc.scale);
			Gore.NewGore(npc.position, npc.velocity, num67 + 3, npc.scale);
			//Gore.NewGore(npc.position, npc.velocity, num67 + 4, npc.scale);
			//Gore.NewGore(npc.position, npc.velocity, num67 + 4, npc.scale);
			//Gore.NewGore(npc.position, npc.velocity, num67 + 5, npc.scale);
			//Gore.NewGore(npc.position, npc.velocity, num67 + 5, npc.scale);
			//Gore.NewGore(npc.position, npc.velocity, num67 + 6, npc.scale);
			//Gore.NewGore(npc.position, npc.velocity, num67 + 6, npc.scale);
			phase = 2;
			npc.ai[3] = 0;
			SoundEngine.PlaySound(npc.DeathSound, npc.Center);
		}
		static float snipeVelocity = 32;
		static float snipePullbackTime = 10;
		float snipeDuration = 210;
		static int snipes = 3;
		static float extraSnipeTime = 10;
		public override bool InstancePerEntity => true;
		int phase = 1;
		Projectile beam;
		public override bool PreAI(NPC npc)
		{
			if (npc.type == NPCID.BloodNautilus)
			{
				Lighting.AddLight(npc.Center, 0.8f, 0.6f, 0.6f);

				float num2 = 0.15f;
				float num3 = 7.5f;
				float idleTime = 60f;
				float chargeChargeTime = 90f;
				float chargeDuration = 180f;
				float bloodSpitChargeTime = 90f;
				float bloodSpitChargeEndTime = 90f;
				int bloodShotVolleys = 3;
				int phaseTransitionTime = 60;
				float bloodMachinegunChargeTime = 50f;
				float bloodMachineGunDuration = 160f;
				int bloodMachinegunShots = 30;
				int bloodBeamChargeTime = BloodBeam.chargeTime;
				int bloodBeamDuration = BloodBeam.duration;
				int phase3attackSpeed = 20;
				int phase3spamDuration = 360;
				int phase3chargeStart = 30;
				int phase3chargeDuration = 30;
				float phase3orbitDistance = 400f;

				if (Main.expertMode)
                {
					bloodShotVolleys = 5;
					bloodSpitChargeEndTime = 120f;
					snipeDuration = 150;
				}
				float bloodSquidTime = 180f;
				if(phase == 2)
                {
					idleTime /= 2;
					chargeChargeTime /= 2;
					bloodSpitChargeTime /= 2;
					bloodShotVolleys += 3;
					bloodSquidTime = 60;
					chargeDuration = 60;
					npc.defense = 12;

				}
				bool num9 = false;
				if (npc.localAI[0] == 0f)
				{
					npc.localAI[0] = 1f;
					npc.alpha = 255;
					if (Main.netMode != 1)
					{
						npc.ai[0] = -1f;
						npc.netUpdate = true;
					}
				}
				#region dust
				if (npc.ai[0] != -1f && Main.rand.Next(4) == 0)
				{
					npc.position += npc.netOffset;
					Dust dust = Dust.NewDustDirect(npc.position + new Vector2(5f), npc.width - 10, npc.height - 10, 5);
					dust.velocity *= 0.5f;
					if (dust.velocity.Y < 0f)
					{
						dust.velocity.Y *= -1f;
					}
					dust.alpha = 120;
					dust.scale = 1f + Main.rand.NextFloat() * 0.4f;
					dust.velocity += npc.velocity * 0.3f;
					npc.position -= npc.netOffset;
				}
				#endregion
				if (npc.target == 255)
				{
					npc.TargetClosest();
					npc.ai[2] = npc.direction;
				}
				if (Main.player[npc.target].dead || Vector2.Distance(Main.player[npc.target].Center, npc.Center) > 2000f)
				{
					npc.TargetClosest();
				}
				NPCAimedTarget nPCAimedTarget = npc.GetTargetData();
				if (Main.dayTime || !Main.bloodMoon)
				{
					nPCAimedTarget = default(NPCAimedTarget);
				}
				npc.immortal = false;
				int nextAttack = -1;
				switch ((int)npc.ai[0])
				{
					case -1:
						{
							npc.velocity *= 0.98f;
							int num17 = Math.Sign(nPCAimedTarget.Center.X - npc.Center.X);
							if (num17 != 0)
							{
								npc.direction = num17;
								npc.spriteDirection = -npc.direction;
							}
							if (npc.localAI[1] == 0f && npc.alpha < 100)
							{
								npc.localAI[1] = 1f;
								#region dust
								int num18 = 36;
								for (int l = 0; l < num18; l++)
								{
									npc.position += npc.netOffset;
									Vector2 value4 = (Vector2.Normalize(npc.velocity) * new Vector2((float)npc.width / 2f, npc.height) * 0.75f * 0.5f).RotatedBy((float)(l - (num18 / 2 - 1)) * ((float)Math.PI * 2f) / (float)num18) + npc.Center;
									Vector2 value5 = value4 - npc.Center;
									int num19 = Dust.NewDust(value4 + value5, 0, 0, 5, value5.X * 2f, value5.Y * 2f, 100, default(Color), 1.4f);
									Main.dust[num19].noGravity = true;
									Main.dust[num19].velocity = Vector2.Normalize(value5) * 3f;
									npc.position -= npc.netOffset;
								}
								#endregion
							}
							if (npc.ai[2] > 5f)
							{
								npc.velocity.Y = -2.5f;
								npc.alpha -= 10;
								if (Collision.SolidCollision(npc.position, npc.width, npc.height))
								{
									npc.alpha += 15;
									if (npc.alpha > 150)
									{
										npc.alpha = 150;
									}
								}
								if (npc.alpha < 0)
								{
									npc.alpha = 0;
								}
							}
							npc.ai[2] += 1f;
							if (npc.ai[2] >= 50f)
							{
								npc.ai[0] = 0f;
								npc.ai[1] = 0f;
								npc.ai[2] = 0f;
								npc.ai[3] = 0f;
								npc.netUpdate = true;
							}
							break;
						}
					case 0:
						{
							//Idle
							npc.velocity *= 0.95f;
							Vector2 destination = nPCAimedTarget.Center + new Vector2((0f - npc.ai[2]) * 300f, -200f);
							if (npc.Center.Distance(destination) > 50f)
							{
								Vector2 desiredVelocity = npc.DirectionTo(destination) * num3;
								npc.SimpleFlyMovement(desiredVelocity, num2);
							}
							npc.direction = ((npc.Center.X < nPCAimedTarget.Center.X) ? 1 : (-1));
							float num15 = npc.Center.DirectionTo(nPCAimedTarget.Center).ToRotation() - 213f / 452f * (float)npc.spriteDirection;
							if (npc.spriteDirection == -1)
							{
								num15 += (float)Math.PI;
							}
							if (npc.spriteDirection != npc.direction)
							{
								npc.spriteDirection = npc.direction;
								npc.rotation = 0f - npc.rotation;
								num15 = 0f - num15;
							}
							npc.rotation = npc.rotation.AngleTowards(num15, 0.02f);
							npc.ai[1] += 1f;
							if (npc.ai[1] > idleTime)
							{
                                int attackCount = (int)npc.ai[3];
								if(Main.masterMode && (float)npc.life / (float)npc.lifeMax < 0.15f)
								{
									nextAttack = 9;
									break;
								}
								switch(phase)
                                {
									case 1:
										if ((float)npc.life / (float)npc.lifeMax < 0.5f || (Main.expertMode && (float)npc.life / (float)npc.lifeMax < 0.65f))
										{
											nextAttack = 5;
											break;
										}
										if (attackCount % 7 == 3)
										{
											nextAttack = 3;
										}
										else if (attackCount % 2 == 0)
										{
											SoundEngine.PlaySound(SoundID.Item170, npc.Center);
											nextAttack = 2;
										}
										else
										{
											SoundEngine.PlaySound(SoundID.Item170, npc.Center);
											nextAttack = 1;
										}
										break;
									case 2:
										SoundEngine.PlaySound(SoundID.Item170, npc.Center);
										int c = Main.expertMode ? 6 : 5;
										switch (attackCount % c)
                                        {
											case 0:
												nextAttack = 4;
												break;
											case 1:
												nextAttack = 1;
												break;
											case 2:
												nextAttack = 3;
												break;
											case 3:
												nextAttack = 6;
												break;
											case 4:
												nextAttack = 2;
                                                break;
											case 5:
												nextAttack = 8;
												break;
										}
										break;
								}
							}
							break;
						}
					case 1:
						{
							//charge attack
							npc.direction = ((!(npc.Center.X < nPCAimedTarget.Center.X)) ? 1 : (-1));
							float num20 = npc.Center.DirectionFrom(nPCAimedTarget.Center).ToRotation() - 213f / 452f * (float)npc.spriteDirection;
							if (npc.spriteDirection == -1)
							{
								num20 += (float)Math.PI;
							}
							bool flag = npc.ai[1] < chargeChargeTime;
							if (npc.spriteDirection != npc.direction && flag)
							{
								npc.spriteDirection = npc.direction;
								npc.rotation = 0f - npc.rotation;
								num20 = 0f - num20;
							}
							if (npc.ai[1] < chargeChargeTime)
							{
								if (npc.ai[1] == chargeChargeTime - 1f)
								{
									SoundEngine.PlaySound(SoundID.Item172, npc.Center);
								}
								num9 = true;
								npc.velocity *= 0.95f;
								npc.rotation = npc.rotation.AngleLerp(num20, 0.02f);
								#region dust
								npc.position += npc.netOffset;
								npc.BloodNautilus_GetMouthPositionAndRotation(out var mouthPosition4, out var mouthDirection4);
								Dust dust6 = Dust.NewDustDirect(mouthPosition4 + mouthDirection4 * 60f - new Vector2(40f), 80, 80, 16, 0f, 0f, 150, Color.Transparent, 0.6f);
								dust6.fadeIn = 1f;
								dust6.velocity = dust6.position.DirectionTo(mouthPosition4 + Main.rand.NextVector2Circular(15f, 15f)) * dust6.velocity.Length();
								dust6.noGravity = true;
								dust6 = Dust.NewDustDirect(mouthPosition4 + mouthDirection4 * 100f - new Vector2(30f), 60, 60, 16, 0f, 0f, 100, Color.Transparent, 0.9f);
								dust6.fadeIn = 1.5f;
								dust6.velocity = dust6.position.DirectionTo(mouthPosition4 + Main.rand.NextVector2Circular(15f, 15f)) * (dust6.velocity.Length() + 5f);
								dust6.noGravity = true;
								npc.position -= npc.netOffset;
								#endregion
							}
							else if (npc.ai[1] < chargeChargeTime + chargeDuration)
							{
								npc.position += npc.netOffset;
								if(phase == 1)
                                {
									npc.rotation = npc.rotation.AngleLerp(num20, 0.05f);
								}
								else
                                {
									npc.rotation = npc.rotation.AngleLerp(num20, 0.05f);
									
                                }
								
								npc.BloodNautilus_GetMouthPositionAndRotation(out var mouthPosition5, out var mouthDirection5);
								if (npc.Center.Distance(nPCAimedTarget.Center) > 30f)
								{
									npc.velocity = mouthDirection5 * -17f * (phase == 2 ? 1.5f : 1f);
								}
								#region dust
								for (int m = 0; m < 4; m++)
								{
									Dust dust7 = Dust.NewDustDirect(mouthPosition5 + mouthDirection5 * 60f - new Vector2(15f), 30, 30, 5, 0f, 0f, 0, Color.Transparent, 1.5f);
									dust7.velocity = dust7.position.DirectionFrom(mouthPosition5 + Main.rand.NextVector2Circular(5f, 5f)) * dust7.velocity.Length();
									dust7.position -= mouthDirection5 * 60f;
									dust7 = Dust.NewDustDirect(mouthPosition5 + mouthDirection5 * 100f - new Vector2(20f), 40, 40, 5, 0f, 0f, 100, Color.Transparent, 1.5f);
									dust7.velocity = dust7.position.DirectionFrom(mouthPosition5 + Main.rand.NextVector2Circular(10f, 10f)) * (dust7.velocity.Length() + 5f);
									dust7.position -= mouthDirection5 * 100f;
								}
								#endregion
								npc.position -= npc.netOffset;
							}
							npc.ai[1] += 1f;
							if (npc.ai[1] >= chargeChargeTime + chargeDuration)
							{
								nextAttack = 0;
							}
							break;
						}
					case 2:
						{
							//Blood shot
							//Dread rotation
							npc.direction = ((npc.Center.X < nPCAimedTarget.Center.X) ? 1 : (-1));
							float num12 = npc.Center.DirectionTo(nPCAimedTarget.Center).ToRotation() - 213f / 452f * (float)npc.spriteDirection;
							if (npc.spriteDirection == -1)
							{
								num12 += (float)Math.PI;
							}
							if (npc.spriteDirection != npc.direction)
							{
								npc.spriteDirection = npc.direction;
								npc.rotation = 0f - npc.rotation;
								num12 = 0f - num12;
							}
							npc.rotation = npc.rotation.AngleLerp(num12, 0.2f);

							if (npc.ai[1] < bloodSpitChargeTime)
							{
								npc.position += npc.netOffset;
								npc.velocity *= 0.95f;
								npc.BloodNautilus_GetMouthPositionAndRotation(out var mouthPosition2, out var mouthDirection2);
								#region dust
								if (Main.rand.Next(4) != 0)
								{
									Dust dust3 = Dust.NewDustDirect(mouthPosition2 + mouthDirection2 * 60f - new Vector2(60f), 120, 120, 16, 0f, 0f, 150, Color.Transparent, 0.6f);
									dust3.fadeIn = 1f;
									dust3.velocity = dust3.position.DirectionTo(mouthPosition2 + Main.rand.NextVector2Circular(15f, 15f)) * (dust3.velocity.Length() + 3f);
									dust3.noGravity = true;
									dust3 = Dust.NewDustDirect(mouthPosition2 + mouthDirection2 * 100f - new Vector2(80f), 160, 160, 16, 0f, 0f, 100, Color.Transparent, 0.9f);
									dust3.fadeIn = 1.5f;
									dust3.velocity = dust3.position.DirectionTo(mouthPosition2 + Main.rand.NextVector2Circular(15f, 15f)) * (dust3.velocity.Length() + 5f);
									dust3.noGravity = true;
								}
								#endregion
								npc.position -= npc.netOffset;
							}
							else if (npc.ai[1] < bloodSpitChargeTime + bloodSpitChargeEndTime)
							{
								npc.position += npc.netOffset;
								npc.velocity *= 0.9f;
								float volleyValue = (npc.ai[1] - bloodSpitChargeTime) % (bloodSpitChargeEndTime / (float)bloodShotVolleys);
								npc.BloodNautilus_GetMouthPositionAndRotation(out var mouthPosition3, out var mouthDirection3);
								#region dust
								if (volleyValue < bloodSpitChargeEndTime / (float)bloodShotVolleys * 0.8f)
								{
									for (int i = 0; i < 5; i++)
									{
										Dust dust4 = Dust.NewDustDirect(mouthPosition3 + mouthDirection3 * 50f - new Vector2(15f), 30, 30, 5, 0f, 0f, 0, Color.Transparent, 1.5f);
										dust4.velocity = dust4.position.DirectionFrom(mouthPosition3 + Main.rand.NextVector2Circular(5f, 5f)) * dust4.velocity.Length();
										dust4.position -= mouthDirection3 * 60f;
										dust4 = Dust.NewDustDirect(mouthPosition3 + mouthDirection3 * 90f - new Vector2(20f), 40, 40, 5, 0f, 0f, 100, Color.Transparent, 1.5f);
										dust4.velocity = dust4.position.DirectionFrom(mouthPosition3 + Main.rand.NextVector2Circular(10f, 10f)) * (dust4.velocity.Length() + 5f);
										dust4.position -= mouthDirection3 * 100f;
									}
								}
								#endregion
								if ((int)volleyValue == 0)
								{
									npc.velocity += mouthDirection3 * -4f;
									#region dust
									for (int j = 0; j < 20; j++)
									{
										Dust dust5 = Dust.NewDustDirect(mouthPosition3 + mouthDirection3 * 60f - new Vector2(15f), 30, 30, 5, 0f, 0f, 0, Color.Transparent, 1.5f);
										dust5.velocity = dust5.position.DirectionFrom(mouthPosition3 + Main.rand.NextVector2Circular(5f, 5f)) * dust5.velocity.Length();
										dust5.position -= mouthDirection3 * 60f;
										dust5 = Dust.NewDustDirect(mouthPosition3 + mouthDirection3 * 100f - new Vector2(20f), 40, 40, 5, 0f, 0f, 100, Color.Transparent, 1.5f);
										dust5.velocity = dust5.position.DirectionFrom(mouthPosition3 + Main.rand.NextVector2Circular(10f, 10f)) * (dust5.velocity.Length() + 5f);
										dust5.position -= mouthDirection3 * 100f;
									}
									#endregion
									if (Main.netMode != 1)
									{
										int projCount = 5;
										if ((npc.ai[1] - bloodSpitChargeTime) % (2 * (bloodSpitChargeEndTime / (float)bloodShotVolleys)) != 0)
										{
											projCount = 4;
										}
										Vector2 projVelocity = mouthDirection3 * 10f;
										int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(30f, 25f);
										for (int k = 0; k < projCount; k++)
										{
											Vector2 velocity = projVelocity.RotatedBy((float)Math.PI / 2 * ((float)k / (float)(projCount - 1)) - (float)Math.PI / 4f);
											Projectile.NewProjectile(npc.GetSpawnSourceForNPCFromNPCAI(), mouthPosition3 - mouthDirection3 * 5f, velocity, 814, attackDamage_ForProjectiles, 0f, Main.myPlayer);
										}
									}
								}

								npc.position -= npc.netOffset;
							}
							npc.ai[1] += 1f;
							if (npc.ai[1] >= bloodSpitChargeTime + bloodSpitChargeEndTime)
							{
								nextAttack = 0;
							}
							break;
						}
					case 3:
						{
							//summon blood squids
							npc.direction = ((npc.Center.X < nPCAimedTarget.Center.X) ? 1 : (-1));
							float targetAngle = 0f;
							npc.spriteDirection = npc.direction;
							if (npc.ai[1] < bloodSquidTime)
							{
								npc.position += npc.netOffset;
								float num11 = MathHelper.Clamp(1f - npc.ai[1] / bloodSquidTime * 1.5f, 0f, 1f);
								npc.velocity = Vector2.Lerp(value2: new Vector2(0f, num11 * -1.5f), value1: npc.velocity, amount: 0.03f);
								npc.velocity = Vector2.Zero;
								npc.rotation = npc.rotation.AngleLerp(targetAngle, 0.02f);
								npc.BloodNautilus_GetMouthPositionAndRotation(out var _, out var _);
								float t = npc.ai[1] / bloodSquidTime;
								float scaleFactor = Utils.GetLerpValue(0f, 0.5f, t) * Utils.GetLerpValue(1f, 0.5f, t);
								Lighting.AddLight(npc.Center, new Vector3(1f, 0.5f, 0.5f) * scaleFactor);
								#region dust
								if (Main.rand.Next(3) != 0)
								{
									Dust dust2 = Dust.NewDustDirect(npc.Center - new Vector2(6f), 12, 12, 5, 0f, 0f, 60, Color.Transparent, 1.4f);
									dust2.position += new Vector2(npc.spriteDirection * 12, 12f);
									dust2.velocity *= 0.1f;
								}
								#endregion
								npc.position -= npc.netOffset;
							}
							if (npc.ai[1] == 10f || npc.ai[1] == 20f || npc.ai[1] == 30f)
							{
								BloodNautilus_CallForHelp(npc);
							}
							npc.ai[1] += 1f;
							if (npc.ai[1] >= bloodSquidTime)
							{
								nextAttack = 0;
							}
							break;
						}
					case 4:
                        {
                            //Blood Snipe!
                            //Dread rotation
                            float aim = TRAEMethods.PredictiveAim(npc.Center, snipeVelocity, nPCAimedTarget.Center, nPCAimedTarget.Velocity, out float time);
                            Vector2 aimLoc = npc.Center + TRAEMethods.PolarVector(snipeVelocity, aim) * time;
                            time += extraSnipeTime;
                            npc.direction = ((npc.Center.X < aimLoc.X) ? 1 : (-1));
                            float num12 = npc.Center.DirectionTo(aimLoc).ToRotation() - 213f / 452f * (float)npc.spriteDirection;
                            if (npc.spriteDirection == -1)
                            {
                                num12 += (float)Math.PI;
                            }
                            if (npc.spriteDirection != npc.direction)
                            {
                                npc.spriteDirection = npc.direction;
                                npc.rotation = 0f - npc.rotation;
                                num12 = 0f - num12;
                            }
                            npc.rotation = npc.rotation.AngleLerp(num12, 0.2f);

                            if (npc.ai[1] < snipePullbackTime)
                            {
                                npc.position += npc.netOffset;
                                npc.BloodNautilus_GetMouthPositionAndRotation(out var mouthPosition5, out var mouthDirection5);
                                if (npc.Center.Distance(nPCAimedTarget.Center) > 30f)
                                {
                                    npc.velocity = mouthDirection5 * -17f;
                                }
                                #region dust
                                for (int m = 0; m < 4; m++)
                                {
                                    Dust dust7 = Dust.NewDustDirect(mouthPosition5 + mouthDirection5 * 60f - new Vector2(15f), 30, 30, 5, 0f, 0f, 0, Color.Transparent, 1.5f);
                                    dust7.velocity = dust7.position.DirectionFrom(mouthPosition5 + Main.rand.NextVector2Circular(5f, 5f)) * dust7.velocity.Length();
                                    dust7.position -= mouthDirection5 * 60f;
                                    dust7 = Dust.NewDustDirect(mouthPosition5 + mouthDirection5 * 100f - new Vector2(20f), 40, 40, 5, 0f, 0f, 100, Color.Transparent, 1.5f);
                                    dust7.velocity = dust7.position.DirectionFrom(mouthPosition5 + Main.rand.NextVector2Circular(10f, 10f)) * (dust7.velocity.Length() + 5f);
                                    dust7.position -= mouthDirection5 * 100f;
                                }
                                #endregion
                                npc.position -= npc.netOffset;
                            }
                            else if (npc.ai[1] < snipePullbackTime + snipeDuration)
                            {
                                npc.position += npc.netOffset;
                                npc.velocity *= 0.8f;
                                float volleyValue = (npc.ai[1] - snipePullbackTime) % (snipeDuration / (float)snipes);
								//Main.NewText("Timer: " + (npc.ai[1] - snipePullbackTime) + " VV: " + volleyValue);
                                npc.BloodNautilus_GetMouthPositionAndRotation(out var mouthPosition3, out var mouthDirection3);
                                #region dust
                                if (volleyValue < bloodSpitChargeEndTime / (float)bloodShotVolleys * 0.8f)
                                {
                                    for (int i = 0; i < 5; i++)
                                    {
                                        Dust dust4 = Dust.NewDustDirect(mouthPosition3 + mouthDirection3 * 50f - new Vector2(15f), 30, 30, 5, 0f, 0f, 0, Color.Transparent, 1.5f);
                                        dust4.velocity = dust4.position.DirectionFrom(mouthPosition3 + Main.rand.NextVector2Circular(5f, 5f)) * dust4.velocity.Length();
                                        dust4.position -= mouthDirection3 * 60f;
                                        dust4 = Dust.NewDustDirect(mouthPosition3 + mouthDirection3 * 90f - new Vector2(20f), 40, 40, 5, 0f, 0f, 100, Color.Transparent, 1.5f);
                                        dust4.velocity = dust4.position.DirectionFrom(mouthPosition3 + Main.rand.NextVector2Circular(10f, 10f)) * (dust4.velocity.Length() + 5f);
                                        dust4.position -= mouthDirection3 * 100f;
                                    }
                                }
                                #endregion
                                if ((int)volleyValue == (snipeDuration / (float)snipes) - 1)
                                {
                                    npc.velocity += mouthDirection3 * -16f;
                                    #region dust
                                    for (int j = 0; j < 20; j++)
                                    {
                                        Dust dust5 = Dust.NewDustDirect(mouthPosition3 + mouthDirection3 * 60f - new Vector2(15f), 30, 30, 5, 0f, 0f, 0, Color.Transparent, 1.5f);
                                        dust5.velocity = dust5.position.DirectionFrom(mouthPosition3 + Main.rand.NextVector2Circular(5f, 5f)) * dust5.velocity.Length();
                                        dust5.position -= mouthDirection3 * 60f;
                                        dust5 = Dust.NewDustDirect(mouthPosition3 + mouthDirection3 * 100f - new Vector2(20f), 40, 40, 5, 0f, 0f, 100, Color.Transparent, 1.5f);
                                        dust5.velocity = dust5.position.DirectionFrom(mouthPosition3 + Main.rand.NextVector2Circular(10f, 10f)) * (dust5.velocity.Length() + 5f);
                                        dust5.position -= mouthDirection3 * 100f;
                                    }
                                    #endregion
                                    if (Main.netMode != 1)
                                    {
                                        int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(37.5f, 31.25f);
                                        Projectile p = Main.projectile[Projectile.NewProjectile(npc.GetSpawnSourceForNPCFromNPCAI(), npc.Center, TRAEMethods.PolarVector(snipeVelocity, aim), ProjectileType<BloodSnipe>(), attackDamage_ForProjectiles, 0, 255, time)];
                                        p.ai[0] = time;
                                        p.netUpdate = true;
                                    }
                                }

                                npc.position -= npc.netOffset;
                            }
                            npc.ai[1] += 1f;
                            if (npc.ai[1] >= snipePullbackTime + snipeDuration)
                            {
                                nextAttack = 0;
                            }
                            break;
                        }
					case 5:
						{
							//phase 2 transition
							npc.velocity *= 0.8f;
							npc.ai[1] += 1f;
							if (npc.ai[1] >= phaseTransitionTime)
							{
								BreakShell(npc);
								nextAttack = 0;
							}

							break;
						}
					case 6:
						{
							//Blood Machine Gun
							//Dread rotation
							npc.direction = ((npc.Center.X < nPCAimedTarget.Center.X) ? 1 : (-1));
							float num12 = npc.Center.DirectionTo(nPCAimedTarget.Center).ToRotation() - 213f / 452f * (float)npc.spriteDirection;
							if (npc.spriteDirection == -1)
							{
								num12 += (float)Math.PI;
							}
							if (npc.spriteDirection != npc.direction)
							{
								npc.spriteDirection = npc.direction;
								npc.rotation = 0f - npc.rotation;
								num12 = 0f - num12;
							}
							npc.rotation = npc.rotation.AngleLerp(num12, 0.2f);

							if (npc.ai[1] < bloodMachinegunChargeTime)
							{
								npc.position += npc.netOffset;
								npc.BloodNautilus_GetMouthPositionAndRotation(out var mouthPosition2, out var mouthDirection2);
								if (npc.Center.Distance(nPCAimedTarget.Center) > 100f)
                                {
									npc.velocity = mouthDirection2 * 6f;
								}
									
								#region dust
								if (Main.rand.Next(4) != 0)
								{
									Dust dust3 = Dust.NewDustDirect(mouthPosition2 + mouthDirection2 * 60f - new Vector2(60f), 120, 120, 16, 0f, 0f, 150, Color.Transparent, 0.6f);
									dust3.fadeIn = 1f;
									dust3.velocity = dust3.position.DirectionTo(mouthPosition2 + Main.rand.NextVector2Circular(15f, 15f)) * (dust3.velocity.Length() + 3f);
									dust3.noGravity = true;
									dust3 = Dust.NewDustDirect(mouthPosition2 + mouthDirection2 * 100f - new Vector2(80f), 160, 160, 16, 0f, 0f, 100, Color.Transparent, 0.9f);
									dust3.fadeIn = 1.5f;
									dust3.velocity = dust3.position.DirectionTo(mouthPosition2 + Main.rand.NextVector2Circular(15f, 15f)) * (dust3.velocity.Length() + 5f);
									dust3.noGravity = true;
								}
								#endregion
								npc.position -= npc.netOffset;
							}
							else if (npc.ai[1] < bloodMachinegunChargeTime + bloodMachineGunDuration)
							{
								npc.position += npc.netOffset;
								float volleyValue = (npc.ai[1] - bloodSpitChargeTime) % (bloodMachineGunDuration / (float)bloodMachinegunShots);
								npc.BloodNautilus_GetMouthPositionAndRotation(out var mouthPosition3, out var mouthDirection3);
								if (npc.Center.Distance(nPCAimedTarget.Center) > 100f)
								{
									npc.velocity = mouthDirection3 * 6f;
								}
								#region dust
								if (volleyValue < bloodMachineGunDuration / (float)bloodShotVolleys * 0.8f)
								{
									for (int i = 0; i < 5; i++)
									{
										Dust dust4 = Dust.NewDustDirect(mouthPosition3 + mouthDirection3 * 50f - new Vector2(15f), 30, 30, 5, 0f, 0f, 0, Color.Transparent, 1.5f);
										dust4.velocity = dust4.position.DirectionFrom(mouthPosition3 + Main.rand.NextVector2Circular(5f, 5f)) * dust4.velocity.Length();
										dust4.position -= mouthDirection3 * 60f;
										dust4 = Dust.NewDustDirect(mouthPosition3 + mouthDirection3 * 90f - new Vector2(20f), 40, 40, 5, 0f, 0f, 100, Color.Transparent, 1.5f);
										dust4.velocity = dust4.position.DirectionFrom(mouthPosition3 + Main.rand.NextVector2Circular(10f, 10f)) * (dust4.velocity.Length() + 5f);
										dust4.position -= mouthDirection3 * 100f;
									}
								}
								#endregion
								if ((int)volleyValue == 0)
								{
									#region dust
									for (int j = 0; j < 20; j++)
									{
										Dust dust5 = Dust.NewDustDirect(mouthPosition3 + mouthDirection3 * 60f - new Vector2(15f), 30, 30, 5, 0f, 0f, 0, Color.Transparent, 1.5f);
										dust5.velocity = dust5.position.DirectionFrom(mouthPosition3 + Main.rand.NextVector2Circular(5f, 5f)) * dust5.velocity.Length();
										dust5.position -= mouthDirection3 * 60f;
										dust5 = Dust.NewDustDirect(mouthPosition3 + mouthDirection3 * 100f - new Vector2(20f), 40, 40, 5, 0f, 0f, 100, Color.Transparent, 1.5f);
										dust5.velocity = dust5.position.DirectionFrom(mouthPosition3 + Main.rand.NextVector2Circular(10f, 10f)) * (dust5.velocity.Length() + 5f);
										dust5.position -= mouthDirection3 * 100f;
									}
									#endregion
									if (Main.netMode != 1)
									{
										Vector2 projVelocity = mouthDirection3 * 13f;
										int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(30f, 25f);

										Vector2 velocity = projVelocity;
										Projectile projectile = Main.projectile[Projectile.NewProjectile(npc.GetSpawnSourceForNPCFromNPCAI(), mouthPosition3 - mouthDirection3 * 5f, velocity, 814, attackDamage_ForProjectiles, 0f, Main.myPlayer, -180)];
										projectile.ai[0] = -180;
										projectile.netUpdate = true;

									}
								}

								npc.position -= npc.netOffset;
							}
							npc.ai[1] += 1f;
							if (npc.ai[1] >= bloodMachinegunChargeTime + bloodMachineGunDuration)
							{
								nextAttack = 0;
							}
							break;
						}
					case 7:
						{
							//phase 3
							int totalTime = phase3chargeDuration + phase3spamDuration + phase3chargeStart;
							npc.direction = ((!(npc.Center.X < nPCAimedTarget.Center.X)) ? 1 : (-1));
							float num20 = npc.Center.DirectionFrom(nPCAimedTarget.Center).ToRotation() - 213f / 452f * (float)npc.spriteDirection;
							if (npc.spriteDirection == -1)
							{
								num20 += (float)Math.PI;
							}
							bool flag = npc.ai[1] < chargeChargeTime;
							if (npc.spriteDirection != npc.direction && flag)
							{
								npc.spriteDirection = npc.direction;
								npc.rotation = 0f - npc.rotation;
								num20 = 0f - num20;
							}
							if (npc.ai[1] % totalTime < phase3spamDuration)
							{
								num20 += (float)Math.PI / 2f;
								float rotMore = ((phase3orbitDistance - npc.Center.Distance(nPCAimedTarget.Center)) / 300f);
								if (Math.Abs(rotMore) > 1)
								{
									rotMore = Math.Sign(rotMore);
								}
								rotMore *= (float)Math.PI / 4;
								num20 += rotMore;
							}
							else if ((npc.ai[1] % totalTime < phase3spamDuration + phase3chargeStart))
							{
								if((npc.ai[1] % totalTime == phase3spamDuration))
                                {
									SoundEngine.PlaySound(SoundID.Item172, npc.Center);
								}
							}
							else
							{
								num20 = npc.rotation;
							}
							npc.position += npc.netOffset;
							npc.rotation = npc.rotation.AngleLerp(num20, 0.15f);
							npc.BloodNautilus_GetMouthPositionAndRotation(out var mouthPosition5, out var mouthDirection5);
							if (npc.Center.Distance(nPCAimedTarget.Center) > 30f)
							{
								npc.velocity = mouthDirection5 * -12f;
							}
							if (npc.ai[1] % totalTime < phase3spamDuration && npc.ai[1] % phase3attackSpeed == 0)
                            {
								int attackDamage_ForProjectiles = npc.GetAttackDamage_ForProjectiles(30f, 25f);
								for (int i = 0; i < 8; i++)
								{
                                    Projectile projectile = Main.projectile[Projectile.NewProjectile(npc.GetSpawnSourceForNPCFromNPCAI(), npc.Center, TRAEMethods.PolarVector(10, mouthDirection5.ToRotation() + ((float)i / 8f) * (float)Math.PI * 2f), ProjectileID.BloodShot, attackDamage_ForProjectiles, 0)];
									projectile.ai[0] = -180;
									projectile.netUpdate = true;
								}
							}
							#region dust
								for (int m = 0; m < 4; m++)
							{
								Dust dust7 = Dust.NewDustDirect(mouthPosition5 + mouthDirection5 * 60f - new Vector2(15f), 30, 30, 5, 0f, 0f, 0, Color.Transparent, 1.5f);
								dust7.velocity = dust7.position.DirectionFrom(mouthPosition5 + Main.rand.NextVector2Circular(5f, 5f)) * dust7.velocity.Length();
								dust7.position -= mouthDirection5 * 60f;
								dust7 = Dust.NewDustDirect(mouthPosition5 + mouthDirection5 * 100f - new Vector2(20f), 40, 40, 5, 0f, 0f, 100, Color.Transparent, 1.5f);
								dust7.velocity = dust7.position.DirectionFrom(mouthPosition5 + Main.rand.NextVector2Circular(10f, 10f)) * (dust7.velocity.Length() + 5f);
								dust7.position -= mouthDirection5 * 100f;
							}
							#endregion
							npc.position -= npc.netOffset;

							npc.ai[1] += 1f;
							break;
						}
					case 8:
						{
							//Blood Beam!
							//Dread rotation
							npc.position += npc.netOffset;
							npc.velocity *= 0.8f;
							float volleyValue = (npc.ai[1] - snipePullbackTime) % (snipeDuration / (float)snipes);
							npc.BloodNautilus_GetMouthPositionAndRotation(out var mouthPosition3, out var mouthDirection3);
							npc.direction = ((!(npc.Center.X < nPCAimedTarget.Center.X)) ? 1 : (-1));
							float num20 = npc.Center.DirectionFrom(nPCAimedTarget.Center).ToRotation() - 213f / 452f * (float)npc.spriteDirection;
							if (npc.spriteDirection == -1)
							{
								num20 += (float)Math.PI;
							}
							num20 += (float)Math.PI;
							npc.rotation = npc.rotation.AngleLerp(num20, 0.01f);
							if (npc.ai[1] < bloodBeamChargeTime - bloodBeamChargeTime/4)
                            {
								npc.rotation = npc.rotation.AngleLerp(num20, 0.15f);
							}
							if(npc.ai[1] == 0)
                            {
								beam = Main.projectile[Projectile.NewProjectile(npc.GetSpawnSourceForNPCFromNPCAI(), npc.Center, Vector2.Zero, ProjectileType<BloodBeam>(), 50, 0, 255, npc.whoAmI)];
								beam.ai[0] = npc.whoAmI;
								beam.netUpdate = true;
                            }
							if(beam != null && beam.active)
                            {
								beam.rotation = mouthDirection3.ToRotation();
								beam.Center = mouthPosition3;
							}
							if(npc.ai[1] == bloodBeamChargeTime)
                            {
								SoundEngine.PlaySound(SoundID.NPCDeath13, npc.Center);

							}
							if (npc.ai[1] < bloodBeamChargeTime + bloodBeamDuration)
							{
								#region dust
								if (volleyValue < bloodSpitChargeEndTime / (float)bloodShotVolleys * 0.8f)
								{
									for (int i = 0; i < 5; i++)
									{
										Dust dust4 = Dust.NewDustDirect(mouthPosition3 + mouthDirection3 * 50f - new Vector2(15f), 30, 30, 5, 0f, 0f, 0, Color.Transparent, 1.5f);
										dust4.velocity = dust4.position.DirectionFrom(mouthPosition3 + Main.rand.NextVector2Circular(5f, 5f)) * dust4.velocity.Length();
										dust4.position -= mouthDirection3 * 60f;
										dust4 = Dust.NewDustDirect(mouthPosition3 + mouthDirection3 * 90f - new Vector2(20f), 40, 40, 5, 0f, 0f, 100, Color.Transparent, 1.5f);
										dust4.velocity = dust4.position.DirectionFrom(mouthPosition3 + Main.rand.NextVector2Circular(10f, 10f)) * (dust4.velocity.Length() + 5f);
										dust4.position -= mouthDirection3 * 100f;
									}
								}
								#endregion
							}
							npc.position -= npc.netOffset;
							npc.ai[1] += 1f;
							if (npc.ai[1] >= bloodBeamChargeTime + bloodBeamDuration)
							{
								nextAttack = 0;
							}
							break;
						}
					case 9:
						{
							//phase 3 transition
							npc.immortal = true;
							npc.velocity *= 0.8f;
							npc.ai[1] += 1f;
							if (npc.ai[1] >= phaseTransitionTime)
							{
								SoundEngine.PlaySound(SoundID.Roar, npc.Center, 0);
								nextAttack = 7;
							}
							break;
						}
				}
				if (nextAttack != -1)
				{
					npc.ai[0] = nextAttack;
					npc.ai[1] = 0f;
					npc.ai[2] = 0f;
					npc.netUpdate = true;
					npc.TargetClosest();
					if (nextAttack == 0)
					{
						npc.ai[2] = npc.direction;
					}
					else
					{
						npc.ai[3] += 1f;
					}
				}
				npc.reflectsProjectiles = false;

				return false;
			}
			return base.PreAI(npc);
		}
		void BloodNautilus_CallForHelp(NPC npc)
		{
			if (Main.netMode == 1 || !Main.player[npc.target].active || Main.player[npc.target].dead || npc.Distance(Main.player[npc.target].Center) > 2000f || NPC.CountNPCS(619) >= 3)
			{
				return;
			}
			Point point = npc.Center.ToTileCoordinates();
			Point point2 = point;
			int num = 20;
			int num2 = 3;
			int num3 = 8;
			int num4 = 2;
			int num5 = 0;
			bool flag = false;
			while (!flag && num5 < 100)
			{
				num5++;
				int num6 = Main.rand.Next(point2.X - num, point2.X + num + 1);
				int num7 = Main.rand.Next(point2.Y - num, point2.Y + num + 1);
				if ((num7 < point2.Y - num3 || num7 > point2.Y + num3 || num6 < point2.X - num3 || num6 > point2.X + num3) && (num7 < point.Y - num2 || num7 > point.Y + num2 || num6 < point.X - num2 || num6 > point.X + num2) && !Main.tile[num6, num7].HasUnactuatedTile)
				{
					bool flag2 = true;
					if (flag2 && Main.tile[num6, num7].LiquidType == LiquidID.Lava)
					{
						flag2 = false;
					}
					if (flag2 && Collision.SolidTiles(num6 - num4, num6 + num4, num7 - num4, num7 + num4))
					{
						flag2 = false;
					}
					if (flag2 && !Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
					{
						flag2 = false;
					}
					if (flag2)
					{
						Projectile.NewProjectile(npc.GetSpawnSourceForNPCFromNPCAI(), num6 * 16 + 8, num7 * 16 + 8, 0f, 0f, 813, 0, 0f, Main.myPlayer);
						flag = true;
						break;
					}
				}
			}
		}
		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (npc.type == NPCID.BloodNautilus)
			{
				if ((int)npc.ai[0] == 4)
				{
					if (npc.ai[1] < snipePullbackTime + snipeDuration)
					{
						float volleyValue = (npc.ai[1] - snipePullbackTime) % (snipeDuration / (float)snipes);
						if (volleyValue >= (snipeDuration / (float)snipes) / 2 )
						{
							float aim = TRAEMethods.PredictiveAim(npc.Center, snipeVelocity, Main.player[npc.target].Center, Main.player[npc.target].velocity, out float time);
							time += extraSnipeTime;
							Vector2 aimLoc = npc.Center + TRAEMethods.PolarVector(snipeVelocity, aim) * time;
							float distance = snipeVelocity * time;
							Texture2D drawBlood = Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/Dreadnautilus/BloodDraw").Value;
							Color color = new Color(0.3f, 0.3f, 0.3f, 0.3f);
							spriteBatch.Draw(drawBlood, npc.Center - Main.screenPosition + TRAEMethods.PolarVector(distance / 2f, aim), null, color, aim, drawBlood.Size() * .5f, new Vector2(distance / 4f, 1f), SpriteEffects.None, 0f);
							float subLength = 96;
							for (int k = 0; k < 8; k++)
							{
							
								float r = aim + ((float)k / 8f) * (float)Math.PI * 2f;
									spriteBatch.Draw(drawBlood, aimLoc  - Main.screenPosition + TRAEMethods.PolarVector(subLength / 2f, r), null, color, r, drawBlood.Size() * .5f, new Vector2(subLength / 4f, 1f), SpriteEffects.None, 0f);
								
							}
						}
					}

				}
				if ((int)npc.ai[0] == 8)
                {
					float distance = BloodBeam.beamLength;
					Texture2D drawBlood = Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/Dreadnautilus/BloodDraw").Value;
					Color color = new Color(0.3f, 0.3f, 0.3f, 0.3f);
					npc.BloodNautilus_GetMouthPositionAndRotation(out var mouthPosition3, out var mouthDirection3);
					spriteBatch.Draw(drawBlood, mouthPosition3 - Main.screenPosition + TRAEMethods.PolarVector(distance / 2f, mouthDirection3.ToRotation()), null, color, mouthDirection3.ToRotation(), drawBlood.Size() * .5f, new Vector2(distance / 4f, 2f), SpriteEffects.None, 0f);
					
					if(npc.ai[1] > 60)
                    {
						if(beam != null && beam.active)
                        {
							BloodBeam.Draw(beam);
                        }
                    }
				}
					SpriteEffects spriteEffects = SpriteEffects.None;
				if (npc.spriteDirection == 1)
				{
					spriteEffects = SpriteEffects.FlipHorizontally;
				}
				Texture2D dreadTexture = TextureAssets.Npc[npc.type].Value;
				Texture2D tentacles = TextureAssets.Extra[129].Value;
				if (phase > 1)
				{
					dreadTexture = Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/Dreadnautilus/Phase2").Value;
					tentacles = Request<Texture2D>("TRAEProject/Changes/NPCs/Boss/Dreadnautilus/Phase2Tentacles").Value;
				}
				Vector2 halfSize = new Vector2(TextureAssets.Npc[npc.type].Width() / 2, TextureAssets.Npc[npc.type].Height() / Main.npcFrameCount[npc.type] / 2);
				Vector2 vector35 = npc.Center - screenPos;
				vector35 -= new Vector2(dreadTexture.Width, dreadTexture.Height / Main.npcFrameCount[npc.type]) * npc.scale / 2f;
				vector35 += halfSize * npc.scale + new Vector2(0f, npc.gfxOffY);
				Vector2 offset = Vector2.Zero;
				if (npc.ai[0] == 5 || npc.ai[0] == 9)
                {
					offset = new Vector2(-4 + Main.rand.Next(9), -4 + Main.rand.Next(9));
				}
				spriteBatch.Draw(dreadTexture, vector35 + offset, npc.frame, Color.White, npc.rotation, halfSize, npc.scale, spriteEffects, 0f);
				spriteBatch.Draw(tentacles, vector35 + offset, npc.frame, Color.White, npc.rotation, halfSize, npc.scale, spriteEffects, 0f);
				return false;
			}

			return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
		}
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			base.PostDraw(npc, spriteBatch, screenPos, drawColor);
		}
		public override void BossHeadSlot(NPC npc, ref int index)
		{
			if(npc.type == NPCID.BloodNautilus)
            {
				if(phase >1)
                {
					index = NPCHeadLoader.GetBossHeadSlot(TRAEProj.DreadHead2);
				}
				else
                {
					index = NPCHeadLoader.GetBossHeadSlot(TRAEProj.DreadHead1);
				}
            }
		}
	}
}
