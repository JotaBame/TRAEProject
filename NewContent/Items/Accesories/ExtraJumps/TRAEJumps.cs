using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Accesories.ExtraJumps
{
    public class TRAEJumps : ModPlayer
    {
        public int boosterFlightTimeMax = 0;
        public int boosterFlightTime = 0;
        public const int boosterStartCost = 0;
        public const float boosterSpeedMultiplier = 1.6f;
		public bool isBoosting = false;
		bool canStartBoosting = false;
		public bool levitation = false;
		bool canLevitate = false;
		public bool doVanillaJumps = false;
		public bool isLevitating = true;
		public bool faeJump = false;
		bool canFaeJump = false;
		int faejumpTime = 0;
		int colorCounter = 0;
		public bool advFlight = false;
		
		bool?[] advJumpStorage = new bool?[] {null, null, null, null, null, null, null, null, null, null, null};
		int? advboosterstorage = null;
		float? advFlightStorage = null;
		int? advBootStorage = null;
		int advFlyAttempt = 0;
		public bool allowBlizzardDash = false;
		public bool usedBlizzardDash = false;
		public bool BlizzardStored()
		{
			return advJumpStorage[1] != null && (bool)advJumpStorage[1];
		}
		public void SpendBlizzardJump()
		{
			advJumpStorage[1] = false;
		}
		void AdvFlightFreeze()
		{
			if(advFlightStorage == null)
			{
				advFlightStorage = Player.wingTime;
				advBootStorage = Player.rocketTime;
			}
			Player.wingTime = 0;
			Player.rocketTime = 0;
		}
		void AdvJumpFreeze()
		{
			if(advJumpStorage[0] == null)
			{
				advJumpStorage[9] = canLevitate;
				advJumpStorage[10] = canFaeJump;
				advboosterstorage = boosterFlightTime;
				if(canFaeJump || canLevitate || boosterFlightTime > 0)
				{
					advJumpStorage[0] = Player.hasJumpOption_Basilisk;
					advJumpStorage[1] = Player.hasJumpOption_Blizzard;
					advJumpStorage[2] = Player.hasJumpOption_Cloud;
					advJumpStorage[3] = Player.hasJumpOption_Fart;
					advJumpStorage[4] = Player.hasJumpOption_Sail;
					advJumpStorage[5] = Player.hasJumpOption_Sandstorm;
					advJumpStorage[6] = Player.hasJumpOption_Unicorn;
					advJumpStorage[7] = Player.hasJumpOption_WallOfFleshGoat;
					advJumpStorage[8] = Player.hasJumpOption_Santank;
				}
				else
				{
					advJumpStorage[0] = Player.canJumpAgain_Basilisk;
					advJumpStorage[1] = Player.canJumpAgain_Blizzard;
					advJumpStorage[2] = Player.canJumpAgain_Cloud;
					advJumpStorage[3] = Player.canJumpAgain_Fart;
					advJumpStorage[4] = Player.canJumpAgain_Sail;
					advJumpStorage[5] = Player.canJumpAgain_Sandstorm;
					advJumpStorage[6] = Player.canJumpAgain_Unicorn;
					advJumpStorage[7] = Player.canJumpAgain_WallOfFleshGoat;
					advJumpStorage[8] = Player.canJumpAgain_Santank;
				}
			}
			Player.canJumpAgain_Basilisk = false;
			Player.canJumpAgain_Blizzard = false;
			Player.canJumpAgain_Cloud = false;
			Player.canJumpAgain_Fart = false;
			Player.canJumpAgain_Sail = false;
			Player.canJumpAgain_Sandstorm = false;
			Player.canJumpAgain_Unicorn = false;
			Player.canJumpAgain_WallOfFleshGoat = false;
			Player.canJumpAgain_Santank = false;
			canLevitate = false;
			canFaeJump = false;
			boosterFlightTime = 0;
		}
		void AdvRecoverFlight()
		{
			if(advFlightStorage != null)
			{
				Player.wingTime = (float)advFlightStorage;
				Player.rocketTime = (int)advBootStorage;
				advFlightStorage = null;
				advBootStorage = null;
			}
		}
		void AdvRecoverJump()
		{
			if(advJumpStorage[0] != null)
			{
				Player.canJumpAgain_Basilisk = (bool)advJumpStorage[0];
				Player.canJumpAgain_Blizzard = (bool)advJumpStorage[1];
				Player.canJumpAgain_Cloud = (bool)advJumpStorage[2];
				Player.canJumpAgain_Fart = (bool)advJumpStorage[3];
				Player.canJumpAgain_Sail = (bool)advJumpStorage[4];
				Player.canJumpAgain_Sandstorm = (bool)advJumpStorage[5];
				Player.canJumpAgain_Unicorn = (bool)advJumpStorage[6];
				Player.canJumpAgain_WallOfFleshGoat = (bool)advJumpStorage[7];
				Player.canJumpAgain_Santank = (bool)advJumpStorage[8];
				canLevitate = (bool)advJumpStorage[9];
				canFaeJump = (bool)advJumpStorage[10];
				boosterFlightTime = (int)advboosterstorage;

				advboosterstorage = null;
				for(int i = 0; i < 11; i++)
				{
					advJumpStorage[i] = null;
				}
				if(canFaeJump || canLevitate || boosterFlightTime > 0)
				{
					BlockVanillaJumps();
				}

				Player.releaseJump = true;
			}

		}
		void AdvReset()
		{
			advboosterstorage = null;
			for(int i = 0; i < 11; i++)
			{
				advJumpStorage[i] = null;
			}
			if(advFlightStorage != null)
			{
				Player.wingTime = (float)advFlightStorage;
				Player.rocketTime = (int)advBootStorage;
				advFlightStorage = null;
				advBootStorage = null;
			}
		}

		public override void ResetEffects()
        {
            boosterFlightTimeMax = 0;
			levitation = false;
			faeJump = false;
			advFlight = false;
			if(advFlyAttempt > 0)
			{
				advFlyAttempt--;
				if(advFlyAttempt > 0)
				{
					Player.controlJump = true;
				}
			}
        }
        public void RestoreJumps()
        {
            boosterFlightTime = boosterFlightTimeMax;
			isBoosting = false;
			canStartBoosting = false;
			canLevitate = levitation;
			doVanillaJumps = true;
			canFaeJump = faeJump;
			usedBlizzardDash = false;
		}
        void BlockVanillaJumps()
        {
            Player.canJumpAgain_Basilisk = false;
            Player.canJumpAgain_Blizzard = false;
			if(Player.GetModPlayer<Mobility>().blizzardDash)
			{
				allowBlizzardDash = true;
			}
            Player.canJumpAgain_Cloud = false;
            Player.canJumpAgain_Fart = false;
            Player.canJumpAgain_Sail = false;
            Player.canJumpAgain_Sandstorm = false;
            Player.canJumpAgain_Unicorn = false;
            Player.canJumpAgain_WallOfFleshGoat = false;
			Player.canJumpAgain_Santank = false;
			if(!advFlight)
			{
				Player.rocketTime = 0;
			}
        }
		void RefreshVanillaJumps()
		{
			if (Player.hasJumpOption_Cloud)
			{
				Player.canJumpAgain_Cloud = true;
			}
			if (Player.hasJumpOption_Sandstorm)
			{
				Player.canJumpAgain_Sandstorm = true;
			}
			if (Player.hasJumpOption_Blizzard && !usedBlizzardDash)
			{
				Player.canJumpAgain_Blizzard = true;
			}
			if (Player.hasJumpOption_Fart)
			{
				Player.canJumpAgain_Fart = true;
			}
			if (Player.hasJumpOption_Sail)
			{
				Player.canJumpAgain_Sail = true;
			}
			if (Player.hasJumpOption_Unicorn)
			{
				Player.canJumpAgain_Unicorn = true;
			}
			if (Player.hasJumpOption_Santank)
			{
				Player.canJumpAgain_Santank = true;
			}
			if (Player.hasJumpOption_WallOfFleshGoat)
			{
				Player.canJumpAgain_WallOfFleshGoat = true;
			}
			if (Player.hasJumpOption_Basilisk)
			{
				Player.canJumpAgain_Basilisk = true;
			}
			Player.rocketTime = Player.rocketTimeMax;
			Player.wingTime = Player.wingTimeMax;
			
			if(advFlight)
			{
				AdvReset();
			}

		}
		public override void PostUpdateEquips()
		{
			if (advFlight)
			{
				Player.gravControl = false;
				Player.gravControl2 = false;
			}
		}

        public override void PreUpdateMovement()
        {

			if(Player.grappling[0] != -1)
			{
				RestoreJumps();
			}
			if(advFlight && !Player.TryingToHoverDown)
            {
                Player.gravControl = false;
                Player.gravControl2 = false;
                if (Player.oldVelocity.Y == 0 || Player.grappling[0] != -1)
				{
					AdvReset();
				}
				{
					if(Player.controlJump && advFlyAttempt <= 0)
					{
						AdvFlightFreeze();
						AdvRecoverJump();
					}
					else if(Player.controlUp)
					{
						advFlyAttempt = 2;
						AdvJumpFreeze();
						AdvRecoverFlight();
						Player.controlJump = true;
					}
				}
			}
			else
			{
				AdvReset();
			}
			if (Player.jump <= 0)
			{
				if (!Player.controlJump)
                {
					canStartBoosting = true;
                }
				//Main.NewText(boosterFlightTime + "/" + boosterFlightTimeMax);
				if (boosterFlightTime > 0)
				{
					BlockVanillaJumps();
					if (Player.controlJump && (canStartBoosting || boosterFlightTime < boosterFlightTimeMax))
					{
						if (isBoosting)
						{
							boosterFlightTime--;
							if(boosterFlightTime == 0)
							{
								canStartBoosting = false;
								isBoosting = false;
							}
							if(boosterFlightTime % 6 == 0)
                            {
								Puff();
                            }
						}
						else
						{
							boosterFlightTime -= boosterStartCost;
							isBoosting = true;
							Puff();
						}
						float dir = -(float)Math.PI / 2f;
						if (Player.controlRight && Player.controlUp)
						{
							dir = -(float)Math.PI / 4f;
						}
						else if (Player.controlUp && Player.controlLeft)
						{
							dir = -3 * (float)Math.PI / 4f;
						}
						else if (Player.controlDown && Player.controlLeft)
						{
							dir = 3 * (float)Math.PI / 4f;
						}
						else if (Player.controlDown && Player.controlRight)
						{
							dir = (float)Math.PI / 4f;
						}
						else if (Player.controlDown)
						{
							dir = (float)Math.PI / 2f;
						}
						else if (Player.controlRight && !Player.controlLeft)
						{
							dir = 0;
						}
						else if (!Player.controlRight && Player.controlLeft)
						{
							dir = (float)Math.PI;
						}
						Player.velocity = TRAEMethods.PolarVector(Terraria.Player.jumpSpeed * boosterSpeedMultiplier, dir);
						Player.velocity.Y += 1E-06f;
						Player.velocity.Y *= Player.gravDir;
						//Player.controlUp = false;
						//Player.gravControl = Player.gravControl2 = false;
					}
					else
					{
						if(isBoosting)
						{
							boosterFlightTime = 0;
							canStartBoosting = false;
							isBoosting = false;
						}
					}
				}
				else if(canLevitate)
				{
					BlockVanillaJumps();
					if(Player.controlJump && canStartBoosting)
					{
						canLevitate = false;
						Player.velocity.Y = (0f - Player.jumpSpeed) * Player.gravDir;
						Player.jump = Player.jumpHeight * 8;
						isLevitating = true;
						SoundEngine.PlaySound(SoundID.DoubleJump with { MaxInstances = 0 }, Player.Center);
					}
				}
				else if(canFaeJump)
				{
					BlockVanillaJumps();
					if(Player.controlJump && canStartBoosting)
					{
						canFaeJump = false;
						Player.velocity.Y = (0f - Player.jumpSpeed) * Player.gravDir;
						Player.jump = Player.jumpHeight;
						float rot = Player.velocity.ToRotation();
						for(int i =0; i < 30; i++)
						{
							int d = i % 6;
							if(d == 4)
							{
								d = 2;
							}
							if(d == 5)
							{
								d = 1;
							}
							FairyQueenDust(Player, Player.Center, TRAEMethods.PolarVector(d * 2, rot + 2 * (float)Math.PI * ((float)i / 30f)));
						}
						if(faejumpTime <= 0)
						{
							Player.SetImmuneTimeForAllTypes(40);
							Player.brainOfConfusionDodgeAnimationCounter = 300;
						}
						faejumpTime = 180;
						SoundEngine.PlaySound(SoundID.DoubleJump with { MaxInstances = 0 }, Player.Center);
					}
				}
				else if(Player.releaseJump && doVanillaJumps && advJumpStorage[0] == null)
				{
					doVanillaJumps = false;
					RefreshVanillaJumps();
				}
			}
			else 
            {
				canStartBoosting = false;
            }
			if(Player.jump == 0)
			{
				if(isLevitating)
				{
					isLevitating = false;
					if(Player.mount.Active && Player.mount.BlockExtraJumps)
					{

					}
					else
					{
						Player.fullRotation = 0;
					}
				}
			}
			else if(isLevitating)
			{
				{
					Player.fullRotation += Player.direction * Player.gravDir * (float)Math.PI / 15f;
					Player.fullRotationOrigin = Player.Size * 0.5f;
				}
				for(int i = 0; i < 30; i++)
				{
					Dust d = Dust.NewDustPerfect(Player.Center + TRAEMethods.PolarVector(30, (float)Math.PI * 2f * ((float)i / 3f) + Player.fullRotation), DustID.SilverFlame, Vector2.UnitY * Player.gravDir * -6);
				}
			}
			if(faejumpTime > 0)
			{
				faejumpTime--;
				FairyQueenDust(Player, Player.position + new Vector2(Main.rand.Next(Player.width), Main.rand.Next(Player.height)), Vector2.Zero);
			}
			colorCounter++;
			base.PreUpdateMovement();
        }
        public override void PostUpdate()
		{
			if (Player.velocity.Y == 0 || Player.sliding)
			{
				RestoreJumps();
			}
		}
		void Puff()
        {
			for (int i = 0; i < 8; i++)
            {
				Vector2 pos = Player.Center + TRAEMethods.PolarVector(5, ((float)i / 8f) * 2 * (float)Math.PI);
				Dust d = Dust.NewDustPerfect(pos, DustID.Smoke);
				d.frame.Y = 0;
            }
			SoundEngine.PlaySound(SoundID.DoubleJump with { MaxInstances = 0 }, Player.Center);
		}
		Dust FairyQueenDust(Player player, Vector2 pos, Vector2 vel)
		{
			Color fairyQueenWeaponsColor = GetFairyQueenWeaponsColor(player, 1f, Main.rand.NextFloat() * 0.4f); //this method never uses any information from the projectile it needs to be called from
			Dust d = Dust.NewDustPerfect(pos, 267, vel, newColor: fairyQueenWeaponsColor);
			d.noGravity = true;
			return d;
		}
		public Color GetFairyQueenWeaponsColor(Player player, float alphaChannelMultiplier = 1f, float lerpToWhite = 0f, float? rawHueOverride = null)
		{
			float num = (float)(colorCounter % 60) / 60f;
			if (rawHueOverride.HasValue)
			{
				num = rawHueOverride.Value;
			}
			float num2 = (num + 0.5f) % 1f;
			float saturation = 1f;
			float luminosity = 0.5f;
			if (player.active)
			{
				switch (player.name)
				{
				case "Cenx":
				{
					float amount13 = Utils.PingPongFrom01To010(num2);
					amount13 = MathHelper.SmoothStep(0f, 1f, amount13);
					amount13 = MathHelper.SmoothStep(0f, 1f, amount13);
					Color color3 = Color.Lerp(new Color(0.3f, 1f, 0.2f), Color.HotPink, amount13);
					if (lerpToWhite != 0f)
					{
						color3 = Color.Lerp(color3, Color.White, lerpToWhite);
					}
					color3.A = (byte)((float)(int)color3.A * alphaChannelMultiplier);
					return color3;
				}
				case "Crowno":
					luminosity = MathHelper.Lerp(0.25f, 0.4f, Utils.Turn01ToCyclic010(num2));
					num2 = MathHelper.Lerp(127f / 180f, 47f / 60f, Utils.Turn01ToCyclic010(num2));
					alphaChannelMultiplier = MathHelper.Lerp(alphaChannelMultiplier, 0.5f, 0.5f);
					break;
				case "Yoraiz0r":
					num2 = MathHelper.Lerp(0.9f, 0.95f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.5f;
					break;
				case "Jaxrud":
					num2 = MathHelper.Lerp(13f / 72f, 157f / 360f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.5f;
					break;
				case "Lazure":
					num2 = MathHelper.Lerp(8f / 15f, 83f / 90f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.5f;
					break;
				case "Leinfors":
					num2 = MathHelper.Lerp(0.7f, 0.77f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.5f;
					break;
				case "Grox The Great":
					num2 = MathHelper.Lerp(0.31f, 0.5f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.5f;
					alphaChannelMultiplier = MathHelper.Lerp(alphaChannelMultiplier, 1f, 0.8f);
					break;
				case "Acamaeda":
					num2 = MathHelper.Lerp(0.06f, 0.28f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.5f;
					alphaChannelMultiplier = MathHelper.Lerp(alphaChannelMultiplier, 0.6f, 0.5f);
					break;
				case "Alchemystics":
					num2 = MathHelper.Lerp(0.74f, 0.96f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.6f;
					alphaChannelMultiplier = MathHelper.Lerp(alphaChannelMultiplier, 0.6f, 0.5f);
					break;
				case "Antithesis":
				{
					num2 = 0.51f;
					float amount14 = (float)Math.Cos(num * ((float)Math.PI * 2f)) * 0.5f + 0.5f;
					luminosity = MathHelper.Lerp(0f, 0.5f, amount14);
					break;
				}
				case "Aurora3500":
					num2 = MathHelper.Lerp(0.33f, 0.8f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.5f;
					alphaChannelMultiplier = MathHelper.Lerp(alphaChannelMultiplier, 0.5f, 0.5f);
					break;
				case "Bame":
				{
					float amount12 = Utils.PingPongFrom01To010(num2);
					amount12 = MathHelper.SmoothStep(0f, 1f, amount12);
					amount12 = MathHelper.SmoothStep(0f, 1f, amount12);
					Color color2 = Color.Lerp(Color.Yellow, new Color(0.4f, 0f, 0.75f), amount12);
					if (lerpToWhite != 0f)
					{
						color2 = Color.Lerp(color2, Color.White, lerpToWhite);
					}
					color2.A = (byte)((float)(int)color2.A * alphaChannelMultiplier);
					return color2;
				}
				case "Criddle":
					num2 = MathHelper.Lerp(0.05f, 0.15f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.5f;
					alphaChannelMultiplier = MathHelper.Lerp(alphaChannelMultiplier, 0.5f, 0.5f);
					break;
				case "Darthkitten":
				{
					num2 = 1f;
					float amount11 = (float)Math.Cos(num * ((float)Math.PI * 2f)) * 0.5f + 0.5f;
					luminosity = MathHelper.Lerp(1f, 0.4f, amount11);
					break;
				}
				case "darthmorf":
				{
					num2 = 0f;
					float amount10 = (float)Math.Cos(num * ((float)Math.PI * 2f)) * 0.5f + 0.5f;
					luminosity = MathHelper.Lerp(0f, 0.2f, amount10);
					break;
				}
				case "Discipile":
				{
					num2 = 0.53f;
					float amount9 = (float)Math.Cos(num * ((float)Math.PI * 2f)) * 0.5f + 0.5f;
					luminosity = MathHelper.Lerp(0.05f, 0.5f, amount9);
					break;
				}
				case "Doylee":
					num2 = MathHelper.Lerp(0.68f, 1f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.5f;
					alphaChannelMultiplier = MathHelper.Lerp(alphaChannelMultiplier, 0.6f, 0.5f);
					break;
				case "Ghostar":
				{
					num2 = 0.66f;
					float amount8 = (float)Math.Cos(num * ((float)Math.PI * 2f)) * 0.5f + 0.5f;
					luminosity = MathHelper.Lerp(0.15f, 0.85f, amount8);
					break;
				}
				case "Jenosis":
					num2 = MathHelper.Lerp(0.9f, 1.13f, Utils.Turn01ToCyclic010(num2)) % 1f;
					luminosity = 0.5f;
					alphaChannelMultiplier = MathHelper.Lerp(alphaChannelMultiplier, 0.5f, 0.5f);
					break;
				case "Kazzymodus":
				{
					num2 = 0.33f;
					float amount7 = (float)Math.Cos(num * ((float)Math.PI * 2f)) * 0.5f + 0.5f;
					luminosity = MathHelper.Lerp(0.15f, 0.4f, amount7);
					break;
				}
				case "Khaios":
				{
					num2 = 0.33f;
					float amount6 = (float)Math.Cos(num * ((float)Math.PI * 2f)) * 0.5f + 0.5f;
					luminosity = MathHelper.Lerp(0f, 0.2f, amount6);
					break;
				}
				case "Loki":
				{
					num2 = 0f;
					float amount5 = (float)Math.Cos(num * ((float)Math.PI * 2f)) * 0.5f + 0.5f;
					luminosity = MathHelper.Lerp(0f, 0.25f, amount5);
					break;
				}
				case "ManaUser":
					num2 = MathHelper.Lerp(0.41f, 0.57f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.5f;
					break;
				case "Mid":
				{
					num2 = 0f;
					float amount4 = (float)Math.Cos(num * ((float)Math.PI * 2f)) * 0.5f + 0.5f;
					luminosity = MathHelper.Lerp(0f, 0.9f, amount4);
					break;
				}
				case "Nimbus":
					num2 = MathHelper.Lerp(0.75f, 1f, Utils.Turn01ToCyclic010(num2));
					luminosity = 1f;
					alphaChannelMultiplier = MathHelper.Lerp(alphaChannelMultiplier, 0.5f, 0.8f);
					break;
				case "Nike Leon":
					num2 = MathHelper.Lerp(0.04f, 0.1f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.5f;
					alphaChannelMultiplier = MathHelper.Lerp(alphaChannelMultiplier, 0.5f, 0.5f);
					break;
				case "ppowersteef":
					num2 = MathHelper.Lerp(0f, 0.15f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.5f;
					alphaChannelMultiplier = MathHelper.Lerp(alphaChannelMultiplier, 0.6f, 0.5f);
					break;
				case "RBrandon":
					num2 = 0.03f;
					luminosity = 0.3f;
					alphaChannelMultiplier = MathHelper.Lerp(alphaChannelMultiplier, 0.6f, 0.5f);
					break;
				case "Redigit":
					num2 = 0.7f;
					luminosity = 0.5f;
					break;
				case "Serenity":
				{
					num2 = 0.85f;
					float amount3 = (float)Math.Cos(num * ((float)Math.PI * 2f)) * 0.5f + 0.5f;
					luminosity = MathHelper.Lerp(1f, 0.5f, amount3);
					break;
				}
				case "Sigma":
					num2 = MathHelper.Lerp(0f, 0.12f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.5f;
					alphaChannelMultiplier = MathHelper.Lerp(alphaChannelMultiplier, 0.6f, 0.5f);
					break;
				case "teiull":
					num2 = MathHelper.Lerp(0.66f, 1f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.5f;
					break;
				case "Unit One":
				{
					float amount2 = Utils.PingPongFrom01To010(num2);
					amount2 = MathHelper.SmoothStep(0f, 1f, amount2);
					Color color = Color.Lerp(Color.Yellow, Color.Blue, amount2);
					if (lerpToWhite != 0f)
					{
						color = Color.Lerp(color, Color.White, lerpToWhite);
					}
					color.A = (byte)((float)(int)color.A * alphaChannelMultiplier);
					return color;
				}
				case "Vulpes Inculta":
					num2 = MathHelper.Lerp(0.65f, 0.75f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.5f;
					alphaChannelMultiplier = MathHelper.Lerp(alphaChannelMultiplier, 0.5f, 0.5f);
					break;
				case "Waze3174":
					num2 = MathHelper.Lerp(0.33f, 0f, Utils.Turn01ToCyclic010(num2));
					luminosity = 0.3f;
					alphaChannelMultiplier = MathHelper.Lerp(alphaChannelMultiplier, 0.6f, 0.5f);
					break;
				case "Xman101":
				{
					num2 = 0.06f;
					float amount = (float)Math.Cos(num * ((float)Math.PI * 2f)) * 0.5f + 0.5f;
					luminosity = MathHelper.Lerp(0f, 0.5f, amount);
					break;
				}
				case "Zoomo":
					num2 = 0.77f;
					luminosity = 0.5f;
					alphaChannelMultiplier = MathHelper.Lerp(alphaChannelMultiplier, 0.6f, 0.5f);
					break;
				}
			}
			Color color4 = Main.hslToRgb(num2, saturation, luminosity);
			if (lerpToWhite != 0f)
			{
				color4 = Color.Lerp(color4, Color.White, lerpToWhite);
			}
			color4.A = (byte)((float)(int)color4.A * alphaChannelMultiplier);
			return color4;
		}
    }
}
