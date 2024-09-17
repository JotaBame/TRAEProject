
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using TRAEProject.NewContent.Items.Accesories.MobilityJumps;
using TRAEProject.NewContent.Items.Accesories.MobilityMisc;
using TRAEProject.Changes.Accesory;

namespace TRAEProject
{
    public class Mobility : ModPlayer
    {
        //constants
        public const int flegSpeed = 24;

        public const int bootSpeed = 20;
        public const int amphibootSpeed = 20;
        public const int swiftSpeed = 10;
        public const int wishSpeed = 15;
        public const float advFlightTimeBonus = 40;

        //abilites
        bool forcedAntiGravity = false;
        public bool blizzardDash = false;
        bool performingBlizzardDash = false;
        public bool ankletAcc = false;
        public bool TRAEwaterwalk = false;
        public int TRAELavaMax = 0;
        public bool TRAEfirewalk = false;
        public bool TRAEMagi = false;
        public float flightTimeBonus = 1;
        public override void ResetEffects()
        {
            TRAEwaterwalk = false;
            TRAEMagi = false;
            forcedAntiGravity = false;
            blizzardDash = false;
            ankletAcc = false;
            TRAEfirewalk = false;
            TRAELavaMax = 0;
            flightTimeBonus = 1;
        }

        //variables
        int ornamentTimer = 0;
        public int crippleTimer = 0;
        public bool skating = false;
        int dashCount = -1;
        int dashCooldown = 0;
        int skateCounter = 0;

        /// <summary> percentage value for new default jump speed of 32</summary>
        public static float JSV(float v)
        {
            return (v * 5) * 1.28f;
        }
        public override void PreUpdate()
        {
            Player.rocketTimeMax = 0;
   
        }
        public override void PreUpdateMovement()
        {
            if(skating)
            {
                Player.coldDash = true;
            }
        }
        void DoCommonDashHandle(out int dir, out bool dashing, Player.DashStartAction dashStartAction = null)
		{
			dir = 0;
			dashing = false;
            if(Player.mount.Active)
            {
                return;
            }
			if (Player.dashTime > 0)
			{
				Player.dashTime--;
			}
			if (Player.dashTime < 0)
			{
				Player.dashTime++;
			}
			if (Player.controlRight && Player.releaseRight)
			{
				if (Player.dashTime > 0)
				{
					dir = 1;
					dashing = true;
					Player.dashTime = 0;
					Player.timeSinceLastDashStarted = 0;
					dashStartAction?.Invoke(dir);
				}
				else
				{
					Player.dashTime = 15;
				}
			}
			else if (Player.controlLeft && Player.releaseLeft)
			{
				if (Player.dashTime < 0)
				{
					dir = -1;
					dashing = true;
					Player.dashTime = 0;
					Player.timeSinceLastDashStarted = 0;
					dashStartAction?.Invoke(dir);
				}
				else
				{
					Player.dashTime = -15;
				}
			}
		}
        public override void PostUpdateEquips()
        {
            Player.wingTimeMax = (int)(Player.wingTimeMax * flightTimeBonus);
            Player.rocketTimeMax = (int)(Player.rocketTimeMax * flightTimeBonus);
            Player.lavaMax = TRAELavaMax;


            Player.waterWalk = false;
            Player.fireWalk = false;

            if (!Player.lavaWet && Player.velocity.X == 0f && Player.velocity.Y == 0f)
            {
                Player.lavaTime += 3;
            }


            if (TRAEwaterwalk)
            {
                Player.waterWalk = true;
                Player.waterWalk2 = true;
            }
            if (TRAEfirewalk)
            {
                Player.fireWalk = true;
            }
            if (Player.dash == 99)
            {
                DoCommonDashHandle(out var dir, out var dashing);
                //Main.NewText(Player.dash);
                //Main.NewText("Dash Delay: " + Player.dashDelay + ", Dash Time: " + Player.dashTime);
                if (dashing && Player.velocity.Y == 0)
                {
                    Player.velocity.X = 10f * (float)dir;
                    Player.dashDelay = -1;
                    Point point = (Player.Center + new Vector2(dir * Player.width / 2 + 2, Player.gravDir * (float)(-Player.height) / 2f + Player.gravDir * 2f)).ToTileCoordinates();
                    Point point2 = (Player.Center + new Vector2(dir * Player.width / 2 + 2, 0f)).ToTileCoordinates();
                    if (WorldGen.SolidOrSlopedTile(point.X, point.Y) || WorldGen.SolidOrSlopedTile(point2.X, point2.Y))
                    {
                        Player.velocity.X /= 2f;
                    }
                    /*
                    for (int num18 = 0; num18 < 20; num18++)
                    {
                        int num19 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, 31, 0f, 0f, 100, default(Color), 2f);
                        Main.dust[num19].position.X += Main.rand.Next(-5, 6);
                        Main.dust[num19].position.Y += Main.rand.Next(-5, 6);
                        Main.dust[num19].velocity *= 0.2f;
                        Main.dust[num19].scale *= 1f + (float)Main.rand.Next(20) * 0.01f;
                    }
                    int num20 = Gore.NewGore(new Vector2(Player.position.X + (float)(Player.width / 2) - 24f, Player.position.Y + (float)(Player.height / 2) - 34f), default(Vector2), Main.rand.Next(61, 64));
                    Main.gore[num20].velocity.X = (float)Main.rand.Next(-50, 51) * 0.01f;
                    Main.gore[num20].velocity.Y = (float)Main.rand.Next(-50, 51) * 0.01f;
                    Main.gore[num20].velocity *= 0.4f;
                    num20 = Gore.NewGore(new Vector2(Player.position.X + (float)(Player.width / 2) - 24f, Player.position.Y + (float)(Player.height / 2) - 14f), default(Vector2), Main.rand.Next(61, 64));
                    Main.gore[num20].velocity.X = (float)Main.rand.Next(-50, 51) * 0.01f;
                    Main.gore[num20].velocity.Y = (float)Main.rand.Next(-50, 51) * 0.01f;
                    Main.gore[num20].velocity *= 0.4f;
                    */
                }
                else if (dashing && blizzardDash && (Player.GetJumpState(ExtraJump.BlizzardInABottle).Available || (Player.GetModPlayer<TRAEJumps>().allowBlizzardDash && !Player.GetModPlayer<TRAEJumps>().usedBlizzardDash)))
                {

                    Player.GetModPlayer<TRAEJumps>().usedBlizzardDash = true;
                    Player.velocity.X = 16f * (float)dir;
                    Player.dashDelay = -1;
                    performingBlizzardDash = true;
                    Player.GetJumpState(ExtraJump.BlizzardInABottle).Available = false;
                    Point point = (Player.Center + new Vector2(dir * Player.width / 2 + 2, Player.gravDir * (float)(-Player.height) / 2f + Player.gravDir * 2f)).ToTileCoordinates();
                    Point point2 = (Player.Center + new Vector2(dir * Player.width / 2 + 2, 0f)).ToTileCoordinates();
                    if (WorldGen.SolidOrSlopedTile(point.X, point.Y) || WorldGen.SolidOrSlopedTile(point2.X, point2.Y))
                    {
                        Player.velocity.X /= 2f;
                    }
                    SoundEngine.PlaySound(SoundID.DoubleJump with { MaxInstances = 0 }, Player.Center);
                }
            }
           
            if (GetInstance<TRAEConfig>().MobilityRework)
            {
                if (Player.shadowArmor)
                {
                    Player.shadowArmor = false;
                    ankletAcc = true;
                    Player.moveSpeed += 0.2f;
                }

                Player.jumpSpeedBoost += 1.4f;
                Player.moveSpeed *= 1.33f;
   
                JumpsAndBalloons.DoubleJumpHorizontalSpeeds(Player);

                if (TRAEMagi)
                {
                    Player.moveSpeed *= 1.15f;
                }

                if (Player.accRunSpeed >= 4.8f)
                {
                    if (Player.controlLeft && Player.velocity.X <= 0f - Player.accRunSpeed && Player.dashDelay >= 0)
                    {
                        if (Player.velocity.X < 0 - Player.accRunSpeed && Player.velocity.Y == 0f && !Player.mount.Active)
                        {
                            SpawnFastRunParticles();
                        }
                    }
                    else if (Player.controlRight && Player.velocity.X >= Player.accRunSpeed && Player.dashDelay >= 0)
                    {
                        if (Player.mount.Active && Player.mount.Cart)
                        {
                            if (Player.velocity.X > 0f)
                            {
                                Player.direction = -1;
                            }
                        }
                        if (Player.velocity.X > Player.accRunSpeed && Player.velocity.Y == 0f && !Player.mount.Active)
                        {
                            SpawnFastRunParticles();
                        }
                    }
                }
                if (Player.wingsLogic == 26)
                {
                    Player.GetModPlayer<AccesoryEffects>().FastFall = true;
                }
                if (Player.wingsLogic == 23)
                {
                    ornamentTimer++;
                    if (Player.velocity.Y != 0 && ornamentTimer % 60 == 0)
                    {
                        Projectile.NewProjectile(Player.GetSource_ItemUse(Player.HeldItem), Player.Center, Vector2.UnitY * 2, ProjectileID.OrnamentFriendly, 50, 0, Player.whoAmI, -1);
                    }
                }
                if (Player.wingsLogic == 30 && Player.TryingToHoverDown) // vortex booster
                {
                    Player.runAcceleration /= 3;
                    Player.moveSpeed += 0.55f;
                }

                if (Player.wingsLogic == 37 && Player.TryingToHoverDown)
                {
                    Player.runAcceleration /= 3;
                    Player.moveSpeed += 0.45f;
                }
                else if (Player.wingsLogic == 22 && Player.TryingToHoverDown)
                {
                    Player.runAcceleration /= 3;
                    Player.moveSpeed += 0.45f;
                }
            
                else if (Player.wingsLogic == 45 && Player.TryingToHoverDown)
                {
                    Player.runAcceleration /= 3;
                    Player.moveSpeed += 1f;
                }
                 else if (Player.TryingToHoverDown && Player.controlJump && ArmorIDs.Wing.Sets.Stats[Player.wingsLogic].HasDownHoverStats && Player.wingTime > 0)
                {
                    Player.GetModPlayer<Mobility>().ankletAcc = true;
                    Player.wingTime += 0.5f;
                    Player.runAcceleration /= 2;
                    //Player.runSlowdown *= 2;
                    Player.moveSpeed += 0.45f;
                    Player.velocity.Y = Player.velocity.Y * 0.9f;
                    if (Player.velocity.Y > -2f && Player.velocity.Y < 1f)
                    {
                        Player.velocity.Y = 1E-05f;
                        //Player.position.Y += .1f;
                    }
                }
            }



            skating = false;

            //Main.NewText(Player.dashDelay + ", " + Player.dashTime + ", " + Player.dash+ ", " + Player.dashType);
            //Player.dashTime = 0;

            if (Player.dashType != 2)
            {
                if (Player.dashDelay == -1 && dashCount == -1)
                {
                    dashCount = 30;
                    dashCooldown = 45;
                }
                if (dashCount > 0)
                {
                    dashCount--;
                    Player.dashDelay = -1;
                    switch (Player.dashType)
                    {
                        case 1: //tabi
                            Player.moveSpeed += 0.5f;
                            break;
                        case 2: //SoC
                            Player.moveSpeed += 0.1f;
                            break;
                        case 3: //Solar Shield
                            Player.moveSpeed += 1.5f;
                            break;
                        case 5: //Crystal
                            Player.moveSpeed += 0.5f;
                            break;
                        case 99: //ice skates
                            if (performingBlizzardDash)
                            {
                                Player.moveSpeed += 0.2f;
                            }
                            Player.moveSpeed += 0.33f;
                            skating = true;
                            Player.armorEffectDrawShadow = false;
                            if (Player.velocity.Y == 0 && ((Math.Sign(Player.velocity.X) == -1 && Player.controlLeft) || (Math.Sign(Player.velocity.X) == 1 && Player.controlRight)))
                            {
                                dashCount = 120;
                            }
                            else if (dashCount > 30 && Player.velocity.Y == 0)
                            {
                                dashCount = 30;
                            }
                            if (Player.velocity.Y == 0)
                            {
                                performingBlizzardDash = false;
                            }

                            break;
                    }
                    if (Player.dash == 99 && Player.velocity.Y != 0 && !performingBlizzardDash && ((Math.Sign(Player.velocity.X) == 1 && !Player.controlRight) || (Math.Sign(Player.velocity.X) == -1 && !Player.controlLeft)))
                    {
                        dashCount = 0;
                    }
                    if ((Math.Sign(Player.velocity.X) == 1 && Player.controlLeft) || (Math.Sign(Player.velocity.X) == -1 && Player.controlRight))
                    {
                        dashCount = 0;
                        Player.velocity.X = 0;
                    }
                    if (dashCooldown <= 0)
                    {
                        dashCooldown = 1;
                    }
                    //Main.NewText(dashCount);
                }
                if (dashCount == 0)
                {
                    dashCount = -1;
                    Player.dashDelay = 0;
                    performingBlizzardDash = false;
                }
                if (performingBlizzardDash)
                {
                    int num12 = Player.height - 6;
                    if (Player.gravDir == -1f)
                    {
                        num12 = 6;
                    }
                    for (int k = 0; k < 2; k++)
                    {
                        int num13 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)num12), Player.width, 12, 76, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f);
                        Main.dust[num13].velocity *= 0.1f;
                        if (k == 0)
                        {
                            Main.dust[num13].velocity += Player.velocity * 0.03f;
                        }
                        else
                        {
                            Main.dust[num13].velocity -= Player.velocity * 0.03f;
                        }
                        Main.dust[num13].velocity -= Player.velocity * 0.1f;
                        Main.dust[num13].noGravity = true;
                        Main.dust[num13].noLight = true;
                    }
                    for (int l = 0; l < 3; l++)
                    {
                        int num14 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)num12), Player.width, 12, 76, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f);
                        Main.dust[num14].fadeIn = 1.5f;
                        Main.dust[num14].velocity *= 0.6f;
                        Main.dust[num14].velocity += Player.velocity * 0.8f;
                        Main.dust[num14].noGravity = true;
                        Main.dust[num14].noLight = true;
                    }
                    for (int m = 0; m < 3; m++)
                    {
                        int num15 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)num12), Player.width, 12, 76, Player.velocity.X * 0.3f, Player.velocity.Y * 0.3f);
                        Main.dust[num15].fadeIn = 1.5f;
                        Main.dust[num15].velocity *= 0.6f;
                        Main.dust[num15].velocity -= Player.velocity * 0.8f;
                        Main.dust[num15].noGravity = true;
                        Main.dust[num15].noLight = true;
                    }
                }
            }

            if (dashCooldown > 0)
            {
                Player.dashTime = 0;
                dashCooldown--;
            }
            for (int n = 3; n < 10; n++)
            {
                if (Player.IsItemSlotUnlockedAndUsable(n) && Player.armor[n].type == ItemType<MagicalCarpet>() && Player.TryingToHoverDown && Player.wingTime > 0)
                {
                    Player.moveSpeed += 0.25f;
                    Player.runAcceleration *= 3;
                }
            }

            if (Player.GetModPlayer<TRAEJumps>().isBoosting)
            {
                Player.gravControl = false;
                Player.gravControl2 = false;
            }
            if (Player.slowFall && Player.velocity.Y < 0)
            {
                Player.slowFall = false;
            }
            if (Player.GetModPlayer<SpaceBalloonPlayer>().SpaceBalloon > 0)
            {
             
                    if (Player.controlUp && !Player.gravControl && !Player.gravControl2)
                    {
                        Player.slowFall = true;
                    }
              
         
            }
            //int num39 = (int)(Player.position.Y / 16f) - Player.fallStart;
            //if(Math.Abs(num39) > Player.extraFall + 25 + (Player.statLifeMax2 / 20))
            //{
            //    Player.extraFall += Math.Abs(num39) - (Player.extraFall + 25 + (Player.statLifeMax2 / 20));
            //}
            if (ankletAcc)
            {
                Player.runAcceleration *= 1.5f;
            }


            //Main.NewText("Speed: " + Player.moveSpeed + ", Acceleration: " + Player.runAcceleration);
            //Main.NewText(Player.rocketTimeMax);
        }
    
    
        public override void PostUpdateRunSpeeds()
        {
            if (GetInstance<TRAEConfig>().FurtherImprovedMovement)
            {
                Player.runAcceleration *= 2f;
                Player.runSlowdown *= 2f;
            }
            if (GetInstance<TRAEConfig>().MobilityRework)
            {
                TRAEProject.Changes.Accesory.WingChanges.PostProcessChanges(Player);
                Player.runSlowdown += 0.3f;
                MountChanges.MountRunSpeeds(Player);
            }
        }
        public override void PostUpdate()
        {
            if(skating)
            {

                Player.armorEffectDrawShadow = false;
                skateCounter++;
                
                Player.runSoundDelay = 2;
                if(Player.velocity.Y == 0)
                {
                    //Main.NewText(Player.legFrameCounter);
                    //Player.legFrameCounter = 2;
                    //Main.NewText(Player.legFrame.Y);
                    int slideTime = 30;
                    int slideTrans = 10;
                    if(skateCounter %  ((slideTime+slideTrans)*2) < slideTime)
                    {
                        Player.legFrame.Y = Player.legFrame.Height * 6;
                    }
                    else if (skateCounter % ((slideTime+slideTrans)*2) < slideTime + slideTrans)
                    {
                        Player.legFrame.Y = Player.legFrame.Height * 7;
                    }
                    else if (skateCounter % ((slideTime+slideTrans)*2) < slideTime * 2 + slideTrans)
                    {
                        Player.legFrame.Y = Player.legFrame.Height * 12;
                    }
                    else
                    {
                        Player.legFrame.Y = Player.legFrame.Height * 13;
                    }

                    float rot = MathF.PI / 8f;
                    if(skateCounter % (slideTime+slideTrans) >=  slideTime)
                    {
                        rot = 0;
                    }
                    else if(skateCounter % ((slideTime+slideTrans)*2) > (slideTime+slideTrans))
                    {
                        rot = MathF.PI / -8f;
                    }
                    Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, -Player.direction * rot);
                    
                    if(Player.itemAnimation == 0)
                    {
                        
                        Player.bodyFrame.Y = 0;
                        Player.direction = Math.Sign(Player.velocity.X);
                        
                        Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Player.direction * rot);
                    }
                    else
                    {
                        //Player.legFrame.Y = Player.legFrame.Height * 6;
                    }
                }
                else
                {
                    if (Player.miscCounter % 4 == 0 && Player.itemAnimation == 0 && !Player.sandStorm && Player.wingsLogic == 0)
                    {
                        Player.direction = Player.miscCounter % 8 >= 4 ? 1 : -1;
                        if (Player.inventory[Player.selectedItem].holdStyle == 2)
                        {
                            if (Player.inventory[Player.selectedItem].type == 946 || Player.inventory[Player.selectedItem].type == 4707)
                            {
                                Player.itemLocation.X = Player.position.X + (float)Player.width * 0.5f - (float)(16 * Player.direction);
                            }
                            if (Player.inventory[Player.selectedItem].type == 186)
                            {
                                Player.itemLocation.X = Player.position.X + (float)Player.width * 0.5f + (float)(6 * Player.direction);
                                Player.itemRotation = 0.79f * (float)(-Player.direction);
                            }
                        }
                        Player.legFrameCounter = 0.0;
                        Player.legFrame.Y = 0;
                    }
                }
            }
            if (forcedAntiGravity)
            {
                Player.gravDir = -1;
            }
            Player.oldVelocity = Player.velocity;
        }
        void SpawnFastRunParticles()
        {
            int num = 0;
            if (Player.gravDir == -1f)
            {
                num -= Player.height;
            }
            if (Player.runSoundDelay == 0 && Player.velocity.Y == 0f)
            {
                SoundEngine.PlaySound(Player.hermesStepSound.Style.GetValueOrDefault() with { MaxInstances = 0 }, Player.position);
                Player.runSoundDelay = Player.hermesStepSound.IntendedCooldown;
            }
            if (Player.wings == 3)
            {
                int num2 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + Player.height + num), Player.width + 8, 4, 186, (0f - Player.velocity.X) * 0.5f, Player.velocity.Y * 0.5f, 50, default(Color), 1.5f);
                Main.dust[num2].velocity *= 0.025f;
                Main.dust[num2].shader = GameShaders.Armor.GetSecondaryShader(Player.cWings, Player);
                num2 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + Player.height + num), Player.width + 8, 4, 186, (0f - Player.velocity.X) * 0.5f, Player.velocity.Y * 0.5f, 50, default(Color), 1.5f);
                Main.dust[num2].velocity *= 0.2f;
                Main.dust[num2].shader = GameShaders.Armor.GetSecondaryShader(Player.cWings, Player);
            }
            else if (Player.sailDash)
            {
                for (int i = 0; i < 4; i++)
                {
                    int num3 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y), Player.width + 8, Player.height, 253, (0f - Player.velocity.X) * 0.5f, Player.velocity.Y * 0.5f, 100, default(Color), 1.5f);
                    Main.dust[num3].noGravity = true;
                    Main.dust[num3].velocity.X = Main.dust[num3].velocity.X * 0.2f;
                    Main.dust[num3].velocity.Y = Main.dust[num3].velocity.Y * 0.2f;
                    Main.dust[num3].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                    Main.dust[num3].scale += (float)Main.rand.Next(-5, 3) * 0.1f;
                    Vector2 vector = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    vector.Normalize();
                    vector *= (float)Main.rand.Next(81) * 0.1f;
                }
            }
            else if (Player.desertDash)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Player.position.X - 4f, Player.position.Y + Player.height + num), Player.width + 8, 4, 32, (0f - Player.velocity.X) * 0.5f, Player.velocity.Y * 0.5f);
                dust.velocity *= 0.2f;
                dust.velocity.Y -= Player.gravDir * 2f;
                dust.shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
            }
            else if (Player.coldDash || Player.GetModPlayer<Mobility>().skating)
            {
                for (int j = 0; j < 2; j++)
                {
                    int num4 = ((j != 0) ? Dust.NewDust(new Vector2(Player.position.X + (Player.width / 2), Player.position.Y + Player.height + Player.gfxOffY), Player.width / 2, 6, 76, 0f, 0f, 0, default(Color), 1.35f) : Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + (float)Player.height + Player.gfxOffY), Player.width / 2, 6, 76, 0f, 0f, 0, default(Color), 1.35f));
                    Main.dust[num4].scale *= 1f + (float)Main.rand.Next(20, 40) * 0.01f;
                    Main.dust[num4].noGravity = true;
                    Main.dust[num4].noLight = true;
                    Main.dust[num4].velocity *= 0.001f;
                    Main.dust[num4].velocity.Y -= 0.003f;
                    Main.dust[num4].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                }
            }
            else if (Player.fairyBoots)
            {
                int type = Main.rand.NextFromList(new short[6] { 61, 61, 61, 242, 64, 63 });
                int alpha = 0;
                for (int k = 1; k < 3; k++)
                {
                    float scale = 1.5f;
                    if (k == 2)
                    {
                        scale = 1f;
                    }
                    int num5 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num), Player.width + 8, 4, type, (0f - Player.velocity.X) * 0.5f, Player.velocity.Y * 0.5f, alpha, default(Color), scale);
                    Main.dust[num5].velocity *= 1.5f;
                    if (k == 2)
                    {
                        Main.dust[num5].position += Main.dust[num5].velocity;
                    }
                    Main.dust[num5].noGravity = true;
                    Main.dust[num5].noLightEmittence = true;
                    Main.dust[num5].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
                }
            }
            else
            {
                int num7 = Dust.NewDust(new Vector2(Player.position.X - 4f, Player.position.Y + (float)Player.height + (float)num), Player.width + 8, 4, 16, (0f - Player.velocity.X) * 0.5f, Player.velocity.Y * 0.5f, 50, default(Color), 1.5f);
                Main.dust[num7].velocity.X = Main.dust[num7].velocity.X * 0.2f;
                Main.dust[num7].velocity.Y = Main.dust[num7].velocity.Y * 0.2f;
                Main.dust[num7].shader = GameShaders.Armor.GetSecondaryShader(Player.cShoe, Player);
            }
        }
        
    }
}