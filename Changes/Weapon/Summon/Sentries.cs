using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace TRAEProject.Changes.Weapon.Summon
{
    public class Sentries : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.QueenSpiderStaff:
                    item.damage = 19; // down from 26
                    break;
                case ItemID.DD2LightningAuraT1Popper:
                    item.damage = 7; // up from 4
                    break;
                case ItemID.DD2LightningAuraT2Popper:
                    item.damage = 15; // up from 11
                    break;
                case ItemID.DD2LightningAuraT3Popper:
                    item.damage = 44; // up from 34
                    break;
                case ItemID.DD2FlameburstTowerT1Popper:
                    item.damage = 25; // up from 17
                    break;
                case ItemID.DD2FlameburstTowerT2Popper:
                    item.damage = 58; // up from 42
                    break;
                case ItemID.DD2FlameburstTowerT3Popper:
                    item.damage = 123; // up from 88
                    break;

                case ItemID.RainbowCrystalStaff:
                    item.damage = 30; // down from 150
                    break;
                case ItemID.MoonlordTurretStaff:
                    item.damage = 33; // down from 100

                    item.SetNameOverride("Stardust Portal Staff");
                    break;
                
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.MoonlordTurretStaff:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Summons a stardust portal to shoot lasers at your enemies";
                        }
                    }
                    break;
            }
        }
    }
    public class SentryChanges : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override void SetDefaults(Projectile projectile)
        {
            switch(projectile.type)
            {

                case ProjectileID.FrostBlastFriendly:
                    projectile.usesLocalNPCImmunity = true;
                    projectile.localNPCHitCooldown = -1;
                    projectile.penetrate = 1;
                    projectile.extraUpdates = 100;
                    break;
            }
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            switch (projectile.type)
            {
                case ProjectileID.SpiderEgg:
                case ProjectileID.BabySpider:
                {
                    int findbuffIndex = target.FindBuffIndex(BuffID.Venom);
                    if (findbuffIndex != -1)
                    {
                        target.DelBuff(findbuffIndex);
                    };
                    return;
                }
            }
        }
        public override bool PreAI(Projectile projectile)
        {
            if (projectile.type == ProjectileID.FrostHydra) 
            {
				if (projectile.localAI[0] == 0f) 
                {
					projectile.localAI[1] = 1f;
					projectile.localAI[0] = 1f;
					projectile.ai[0] = 120f;
					int num425 = 80;
					SoundEngine.PlaySound(SoundID.Item46, projectile.position);
                    for (int num426 = 0; num426 < num425; num426++) {
                        int num427 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 16f), projectile.width, projectile.height - 16, 185);
                        Dust dust2 = Main.dust[num427];
                        dust2.velocity *= 2f;
                        Main.dust[num427].noGravity = true;
                        dust2 = Main.dust[num427];
                        dust2.scale *= 1.15f;
                    }
				}

				projectile.velocity.X = 0f;
				projectile.velocity.Y += 0.2f;
				if (projectile.velocity.Y > 16f)
					projectile.velocity.Y = 16f;

				bool flag18 = false;
				float num434 = projectile.Center.X;
				float num435 = projectile.Center.Y;
				float num436 = 2500f;
				int num437 = -1;
				NPC ownerMinionAttackTargetNPC = projectile.OwnerMinionAttackTargetNPC;
				if (ownerMinionAttackTargetNPC != null && ownerMinionAttackTargetNPC.CanBeChasedBy(this)) {
					float num438 = ownerMinionAttackTargetNPC.position.X + (float)(ownerMinionAttackTargetNPC.width / 2);
					float num439 = ownerMinionAttackTargetNPC.position.Y + (float)(ownerMinionAttackTargetNPC.height / 2);
					float num440 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num438) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num439);
					if (num440 < num436 && Collision.CanHit(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC.position, ownerMinionAttackTargetNPC.width, ownerMinionAttackTargetNPC.height)) {
						num436 = num440;
						num434 = num438;
						num435 = num439;
						flag18 = true;
						num437 = ownerMinionAttackTargetNPC.whoAmI;
					}
				}

				if (!flag18) 
                {
					for (int num441 = 0; num441 < 200; num441++) 
                    {
						if (Main.npc[num441].CanBeChasedBy(this)) 
                        {
							float num442 = Main.npc[num441].position.X + (float)(Main.npc[num441].width / 2);
							float num443 = Main.npc[num441].position.Y + (float)(Main.npc[num441].height / 2);
							float num444 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num442) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num443);
							if (num444 < num436 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num441].position, Main.npc[num441].width, Main.npc[num441].height)) 
                            {
								num436 = num444;
								num434 = num442;
								num435 = num443;
								flag18 = true;
								num437 = Main.npc[num441].whoAmI;
							}
						}
					}
				}

				if (flag18) 
                {

					float num445 = num434;
					float num446 = num435;
					num434 -= projectile.Center.X;
					num435 -= projectile.Center.Y;
					int num447 = 0;
                    if (projectile.frameCounter > 0)
                        projectile.frameCounter--;

                    if (projectile.frameCounter <= 0) {
                        int num448 = projectile.spriteDirection;
                        if (num434 < 0f)
                            projectile.spriteDirection = -1;
                        else
                            projectile.spriteDirection = 1;

                        num447 = ((!(num435 > 0f)) ? ((Math.Abs(num435) > Math.Abs(num434) * 3f) ? 4 : ((Math.Abs(num435) > Math.Abs(num434) * 2f) ? 3 : ((!(Math.Abs(num434) > Math.Abs(num435) * 3f)) ? ((Math.Abs(num434) > Math.Abs(num435) * 2f) ? 1 : 2) : 0))) : 0);
                        int num449 = projectile.frame;
                        if (projectile.type == 308)
                            projectile.frame = num447 * 2;
                        else if (projectile.type == 377)
                            projectile.frame = num447;

                        if (projectile.ai[0] > 40f && projectile.localAI[1] == 0f)
                            projectile.frame++;

                        if (num449 != projectile.frame || num448 != projectile.spriteDirection) {
                            projectile.frameCounter = 8;
                            if (projectile.ai[0] <= 0f)
                                projectile.frameCounter = 4;
                        }
                    }

					if (projectile.ai[0] <= 0f) 
                    {
						float num450 = 60f;

						projectile.localAI[1] = 0f;
						projectile.ai[0] = num450;
						projectile.netUpdate = true;
						if (Main.myPlayer == projectile.owner) 
                        {
							float num451 = 6f;
							int num452 = 309;

							Vector2 vector23 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
							
                            switch (num447) 
                            {
                                case 0:
                                    vector23.Y += 12f;
                                    vector23.X += 24 * projectile.spriteDirection;
                                    break;
                                case 1:
                                    vector23.Y += 0f;
                                    vector23.X += 24 * projectile.spriteDirection;
                                    break;
                                case 2:
                                    vector23.Y -= 2f;
                                    vector23.X += 24 * projectile.spriteDirection;
                                    break;
                                case 3:
                                    vector23.Y -= 6f;
                                    vector23.X += 14 * projectile.spriteDirection;
                                    break;
                                case 4:
                                    vector23.Y -= 14f;
                                    vector23.X += 2 * projectile.spriteDirection;
                                    break;
                            }
							

							if ( projectile.spriteDirection < 0)
								vector23.X += 10f;

							float num453 = num445 - vector23.X;
							float num454 = num446 - vector23.Y;
							float num455 = (float)Math.Sqrt(num453 * num453 + num454 * num454);
							float num456 = num455;
							num455 = num451 / num455;
							num453 *= num455;
							num454 *= num455;
							int num457 = projectile.damage;
							int num458 = Projectile.NewProjectile(projectile.GetSource_FromThis(), vector23.X, vector23.Y, num453, num454, num452, num457, projectile.knockBack, Main.myPlayer);
						}
					}
				}
				else 
                {

					if (projectile.ai[0] <= 60f && (projectile.frame == 1 || projectile.frame == 3 || projectile.frame == 5 || projectile.frame == 7 || projectile.frame == 9))
						projectile.frame--;
				}

				if (projectile.ai[0] > 0f)
					projectile.ai[0] -= 1f;
                    return false;
			}
            return base.PreAI(projectile);
        }
    }
}
