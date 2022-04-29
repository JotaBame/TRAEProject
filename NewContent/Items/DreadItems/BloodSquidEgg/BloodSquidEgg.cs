using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.DreadItems.BloodSquidEgg
{
	public class BloodSquidEgg : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Squid Egg");
			Tooltip.SetDefault("Summons a Baby Blood Squid");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DukeFishronPetItem);
			Item.shoot = ProjectileType<BloodSquidPet>();
			Item.buffType = BuffType<BloodSquidBuff>();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
	public class BloodSquidPet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Baby Blood Squid");
			Main.projFrames[Projectile.type] = 6;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			AIType = ProjectileID.ZephyrFish;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			player.zephyrfish = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 6)
			{
				Projectile.frameCounter = 0;
				Projectile.frame = (Projectile.frame + 1) % 6;
			}
			if (player.dead)
			{
				player.GetModPlayer<BloodSquidPlayer>().bloodSquidPet = false;
			}
			if (player.GetModPlayer<BloodSquidPlayer>().bloodSquidPet)
			{
				Projectile.timeLeft = 2;
			}
		}
	}
	public class BloodSquidPlayer : ModPlayer
	{
		public bool bloodSquidPet;
		public override void ResetEffects()
		{
			bloodSquidPet = false;
		}
	}
	public class BloodSquidBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName and Description are automatically set from the .lang files, but below is how it is done normally.
			DisplayName.SetDefault("Baby Blood Squid");
			Description.SetDefault("It drips and skips!");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			player.GetModPlayer<BloodSquidPlayer>().bloodSquidPet = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ProjectileType<BloodSquidPet>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.GetSource_FromThis(), player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ProjectileType<BloodSquidPet>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}
