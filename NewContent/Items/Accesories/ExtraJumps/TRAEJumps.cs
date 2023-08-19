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
	public class JetJump : ExtraJump
	{
		public const float boosterSpeedMultiplier = 1.6f;
		public override Position GetDefaultPosition() => new Before(SandstormInABottle);
		
		public override IEnumerable<Position> GetModdedConstraints() 
		{
			// By default, modded extra jumps set to be between two vanilla extra jumps (via After and Before) are ordered in load order.
			// This hook allows you to organize where this extra jump is located relative to other modded extra jumps that are also
			// placed between the same two vanila extra jumps.
			yield return new Before(ModContent.GetInstance<LevitationJump>());
		}
		

		public override float GetDurationMultiplier(Player player) 
		{
			// Use this hook to set the duration of the extra jump
			// The XML summary for this hook mentions the values used by the vanilla extra jumps
			return player.GetModPlayer<TRAEJumps>().boosterCount * 5f + 2f;
		}

		public override void OnStarted(Player player, ref bool playSound) 
		{
			playSound = false;
			// Use this hook to trigger effects that should appear at the start of the extra jump
			// This example mimicks the logic for spawning the puff of smoke from the Cloud in a Bottle
		}
		void Puff(Player player)
        {
			for (int i = 0; i < 8; i++)
            {
				Vector2 pos = player.Center + TRAEMethods.PolarVector(5, ((float)i / 8f) * 2 * MathF.PI);
				Dust d = Dust.NewDustPerfect(pos, DustID.Smoke);
				d.frame.Y = 0;
            }
			SoundEngine.PlaySound(SoundID.DoubleJump with { MaxInstances = 0 }, player.Center);
		}

		public override void ShowVisuals(Player player) 
		{
			if(player.GetModPlayer<TRAEJumps>().colorCounter % 4 == 0)
			{
				Puff(player);
			}
			float dir = -MathF.PI / 2f;
			if (player.controlRight && player.controlUp)
			{
				dir = -MathF.PI / 4f;
			}
			else if (player.controlUp && player.controlLeft)
			{
				dir = -3 * MathF.PI / 4f;
			}
			else if (player.controlDown && player.controlLeft)
			{
				dir = 3 * MathF.PI / 4f;
			}
			else if (player.controlDown && player.controlRight)
			{
				dir = MathF.PI / 4f;
			}
			else if (player.controlDown)
			{
				dir = MathF.PI / 2f;
			}
			else if (player.controlRight && !player.controlLeft)
			{
				dir = 0;
			}
			else if (!player.controlRight && player.controlLeft)
			{
				dir = MathF.PI;
			}
			player.velocity = TRAEMethods.PolarVector(Terraria.Player.jumpSpeed * boosterSpeedMultiplier, dir);
			player.velocity.Y += 1E-06f;
			player.velocity.Y *= player.gravDir;
			
		}
	}
	public class LevitationJump : ExtraJump
	{
		public override Position GetDefaultPosition() => new Before(SandstormInABottle);
		
		public override IEnumerable<Position> GetModdedConstraints() 
		{
			// By default, modded extra jumps set to be between two vanilla extra jumps (via After and Before) are ordered in load order.
			// This hook allows you to organize where this extra jump is located relative to other modded extra jumps that are also
			// placed between the same two vanila extra jumps.
			yield return new Before(ModContent.GetInstance<FaeJump>());
		}
		

		public override float GetDurationMultiplier(Player player) 
		{
			// Use this hook to set the duration of the extra jump
			// The XML summary for this hook mentions the values used by the vanilla extra jumps
			return 10f;
		}

		public override void UpdateHorizontalSpeeds(Player player) 
		{
			// Use this hook to modify "player.runAcceleration" and "player.maxRunSpeed"
			// The XML summary for this hook mentions the values used by the vanilla extra jumps
			player.moveSpeed *= 1.5f;
		}

		public override void OnStarted(Player player, ref bool playSound) 
		{
			// Use this hook to trigger effects that should appear at the start of the extra jump
			// This example mimicks the logic for spawning the puff of smoke from the Cloud in a Bottle
		}

		public override void ShowVisuals(Player player) 
		{
			// Use this hook to trigger effects that should appear throughout the duration of the extra jump
			// This example mimics the logic for spawning the dust from the Blizzard in a Bottle
			player.GetModPlayer<TRAEJumps>().levitationResetCounter = 3;
			
			player.fullRotation += player.direction * player.gravDir * MathF.PI / 15f;
			player.fullRotationOrigin = player.Size * 0.5f;
			
			for(int i = 0; i < 30; i++)
			{
				Dust d = Dust.NewDustPerfect(player.Center + TRAEMethods.PolarVector(30, MathF.PI * 2f * ((float)i / 3f) + player.fullRotation), DustID.SilverFlame, Vector2.UnitY * player.gravDir * -6);
			}
			player.GetModPlayer<TRAEJumps>().isBoosting = true;
		}
	}
	public class FaeJump : ExtraJump
	{
		public override Position GetDefaultPosition() => new Before(SandstormInABottle);
		/*
		public override IEnumerable<Position> GetModdedConstraints() 
		{
			// By default, modded extra jumps set to be between two vanilla extra jumps (via After and Before) are ordered in load order.
			// This hook allows you to organize where this extra jump is located relative to other modded extra jumps that are also
			// placed between the same two vanila extra jumps.
			yield return new Before(ModContent.GetInstance<MultipleUseExtraJump>());
		}
		*/

		public override float GetDurationMultiplier(Player player) 
		{
			// Use this hook to set the duration of the extra jump
			// The XML summary for this hook mentions the values used by the vanilla extra jumps
			return 1f;
		}

		public override void UpdateHorizontalSpeeds(Player player) 
		{
			// Use this hook to modify "player.runAcceleration" and "player.maxRunSpeed"
			// The XML summary for this hook mentions the values used by the vanilla extra jumps
			player.moveSpeed *= 1.5f;
		}

		public override void OnStarted(Player player, ref bool playSound) 
		{
			// Use this hook to trigger effects that should appear at the start of the extra jump
			// This example mimicks the logic for spawning the puff of smoke from the Cloud in a Bottle
			float rot = player.velocity.ToRotation();
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
				FairyQueenDust(player, player.Center, TRAEMethods.PolarVector(d * 2, rot + 2 * MathF.PI * ((float)i / 30f)));
			}
			if(player.GetModPlayer<TRAEJumps>().faejumpTime <= 0)
			{
				player.SetImmuneTimeForAllTypes(40);
				player.brainOfConfusionDodgeAnimationCounter = 300;
			}
			player.GetModPlayer<TRAEJumps>().faejumpTime = 180;
		}

		public override void ShowVisuals(Player player) 
		{
			// Use this hook to trigger effects that should appear throughout the duration of the extra jump
			// This example mimics the logic for spawning the dust from the Blizzard in a Bottle
			FairyQueenDust(player, player.position + new Vector2(Main.rand.Next(player.width), Main.rand.Next(player.height)), Vector2.Zero);
		}

		Dust FairyQueenDust(Player player, Vector2 pos, Vector2 vel)
		{
			Color fairyQueenWeaponsColor = player.GetModPlayer<TRAEJumps>().GetFairyQueenWeaponsColor(player, 1f, Main.rand.NextFloat() * 0.4f); //this method never uses any information from the projectile it needs to be called from
			Dust d = Dust.NewDustPerfect(pos, 267, vel, newColor: fairyQueenWeaponsColor);
			d.noGravity = true;
			return d;
		}
	}
    public class TRAEJumps : ModPlayer
    {
		public int levitationResetCounter = 0;
		public int faejumpTime = 0;
		public int colorCounter = 0;
		public int boosterCount = 0;
		bool blockJumps = false;

        public override void PreUpdateMovement()
		{
			blockJumps = false;
			if(advFlight && !Player.TryingToHoverDown)
            {
                //Player.gravControl = false;
                //Player.gravControl2 = false;
				if(Player.controlJump && advFlyAttempt <=0)
				{
					AdvFlightFreeze();
				}
				else if(Player.controlUp)
				{
					blockJumps = true;
					AdvRecoverFlight();
					advFlyAttempt = 2;
					Player.controlJump = true;
				}
			}
			if(levitationResetCounter > 0)
			{
				levitationResetCounter--;
				if(levitationResetCounter == 0)
				{
					Player.fullRotation = 0;
					Player.fullRotationOrigin = Player.Size * 0.5f;
				}
			}
			if(faejumpTime > 0)
			{
				faejumpTime--;
			}
			colorCounter++;
			isBoosting = false;
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
		public override void PostUpdateBuffs()
		{
			Player.blockExtraJumps = Player.blockExtraJumps || blockJumps;
		}

		public bool isBoosting = false;
		
		public bool doVanillaJumps = false;
		public bool advFlight = false;
		float? advFlightStorage = null;
		int? advBootStorage = null;
		int advFlyAttempt = 0;
		public bool allowBlizzardDash = false;
		public bool usedBlizzardDash = false;

        public void RestoreJumps()
        {
			Player.RefreshExtraJumps();
		}
		public override void ResetEffects()
		{
			advFlight = false;
			boosterCount = 0;

			if(advFlyAttempt > 0)
			{
				advFlyAttempt--;
				if(advFlyAttempt > 0)
				{
					Player.controlJump = true;
				}
			}
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
					float amount14 = MathF.Cos(num * (MathF.PI * 2f)) * 0.5f + 0.5f;
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
					float amount11 = MathF.Cos(num * (MathF.PI * 2f)) * 0.5f + 0.5f;
					luminosity = MathHelper.Lerp(1f, 0.4f, amount11);
					break;
				}
				case "darthmorf":
				{
					num2 = 0f;
					float amount10 = MathF.Cos(num * (MathF.PI * 2f)) * 0.5f + 0.5f;
					luminosity = MathHelper.Lerp(0f, 0.2f, amount10);
					break;
				}
				case "Discipile":
				{
					num2 = 0.53f;
					float amount9 = MathF.Cos(num * (MathF.PI * 2f)) * 0.5f + 0.5f;
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
					float amount8 = MathF.Cos(num * (MathF.PI * 2f)) * 0.5f + 0.5f;
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
					float amount7 = MathF.Cos(num * (MathF.PI * 2f)) * 0.5f + 0.5f;
					luminosity = MathHelper.Lerp(0.15f, 0.4f, amount7);
					break;
				}
				case "Khaios":
				{
					num2 = 0.33f;
					float amount6 = MathF.Cos(num * (MathF.PI * 2f)) * 0.5f + 0.5f;
					luminosity = MathHelper.Lerp(0f, 0.2f, amount6);
					break;
				}
				case "Loki":
				{
					num2 = 0f;
					float amount5 = MathF.Cos(num * (MathF.PI * 2f)) * 0.5f + 0.5f;
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
					float amount4 = MathF.Cos(num * (MathF.PI * 2f)) * 0.5f + 0.5f;
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
					float amount3 = MathF.Cos(num * (MathF.PI * 2f)) * 0.5f + 0.5f;
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
					float amount = MathF.Cos(num * (MathF.PI * 2f)) * 0.5f + 0.5f;
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
