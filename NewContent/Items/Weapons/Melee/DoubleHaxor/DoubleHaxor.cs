using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject;
using TRAEProject.Changes.Prefixes;
using TRAEProject.NewContent.Items.Materials;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Weapons.Melee.DoubleHaxor
{
    class DoubleHaxor : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Double Haxor");
            // Tooltip.SetDefault("A chasing axe and a powerful hammer");
        }
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 42;
            Item.damage = 102;
            Item.useTime = Item.useAnimation = 18;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 17);
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.shoot = ProjectileType<HaxorSmall>();
            Item.knockBack = 4f;
            Item.shootSpeed = 9f;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = 1;
            Item.UseSound = SoundID.Item1;
            Item.GetGlobalItem<GiveWeaponsPrefixes>().canGetMeleeOtherModifers = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[ProjectileType<HaxorBig>()] == 0)
            {
                Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<HaxorBig>(), (int)(damage * 1), knockback * 1.8f, player.whoAmI); ;

            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.PaladinsHammer, 1)
                .AddIngredient(ItemID.PossessedHatchet, 1)
                .AddIngredient(ItemType<GraniteBattery>(), 2)
                .AddIngredient(ItemID.MartianConduitPlating, 100)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    class HaxorSmall : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.PossessedHatchet);
            AIType = ProjectileID.PossessedHatchet;
            Projectile.width = Projectile.height = 38;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.friendly = true;
        }
        public override void AI()
        {
			if (Main.rand.NextBool(3))
			{
                Vector2 ProjectilePosition = Projectile.Center;
                int dust = Dust.NewDust(ProjectilePosition, 1, 1, DustID.Electric, 0f, 0f, 0, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].position = ProjectilePosition;
                Main.dust[dust].velocity *= 0.2f;
            }
		}
		
    }
    class HaxorBig : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.PaladinsHammerFriendly);
            Projectile.aiStyle = -1;
			Projectile.scale = 1.2f;
            Projectile.width = Projectile.height = 10;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 8;
			Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.friendly = true;
        }
        public override void AI()
        {
			if (Projectile.soundDelay == 0)
			{
				Projectile.soundDelay = 8;
				SoundEngine.PlaySound(SoundID.Item7 with { MaxInstances = 0 }, Projectile.position);
			}
			if (Projectile.ai[0] == 0f)
			{
				Projectile.ai[1] += 1f;
				if (Projectile.ai[1] >= 30f)
				{
					Projectile.ai[0] = 1f;
					Projectile.ai[1] = 0f;
					Projectile.netUpdate = true;
				}
			}
			else
			{
				Player player = Main.player[Projectile.owner];
				Projectile.tileCollide = false;
				float num63 = 20f;
				float num64 = 3f;
				Vector2 vector6 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num65 = player.position.X + (float)(player.width / 2) - vector6.X;
				float num66 = player.position.Y + (float)(player.height / 2) - vector6.Y;
				float num67 = MathF.Sqrt(num65 * num65 + num66 * num66);
				if (num67 > 3000f)
				{
					Projectile.Kill();
				}
				num67 = num63 / num67;
				num65 *= num67;
				num66 *= num67;

				if (Projectile.velocity.X < num65)
				{
					Projectile.velocity.X += num64;
					if (Projectile.velocity.X < 0f && num65 > 0f)
					{
						Projectile.velocity.X += num64;
					}
				}
				else if (Projectile.velocity.X > num65)
				{
					Projectile.velocity.X -= num64;
					if (Projectile.velocity.X > 0f && num65 < 0f)
					{
						Projectile.velocity.X -= num64;
					}
				}
				if (Projectile.velocity.Y < num66)
				{
					Projectile.velocity.Y += num64;
					if (Projectile.velocity.Y < 0f && num66 > 0f)
					{
						Projectile.velocity.Y += num64;
					}
				}
				else if (Projectile.velocity.Y > num66)
				{
					Projectile.velocity.Y -= num64;
					if (Projectile.velocity.Y > 0f && num66 < 0f)
					{
						Projectile.velocity.Y -= num64;
					}
				}

				if (Main.myPlayer == Projectile.owner)
				{
					Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
					Rectangle value = new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height);
					if (rectangle.Intersects(value))
					{
						Projectile.Kill();
					}
				}
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Main.rand.Next(2) == 0)
			{
				int num68 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 212);
				Dust dust2 = Main.dust[num68];
				dust2.velocity *= 0.1f;
				Main.dust[num68].noGravity = true;
			}

			if (Projectile.ai[0] == 0f)
			{
				Projectile.rotation = Projectile.velocity.ToRotation() + MathF.PI / 4f;
				if (Main.rand.Next(2) == 0)
				{
					int num69 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 200, default(Color), 1.2f);
					Dust dust2 = Main.dust[num69];
					dust2.velocity += Projectile.velocity * 0.3f;
					dust2 = Main.dust[num69];
					dust2.velocity *= 0.2f;
					Main.dust[num69].noGravity = true;
				}
				if (Main.rand.Next(3) == 0)
				{
					int num70 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 43, 0f, 0f, 254, default(Color), 0.3f);
					Dust dust2 = Main.dust[num70];
					dust2.velocity += Projectile.velocity * 0.5f;
					dust2 = Main.dust[num70];
					dust2.velocity *= 0.5f;
					Main.dust[num70].noGravity = true;
				}
			}
			else
			{
				Projectile.rotation += 0.1f * (float)Projectile.direction;
			}		
		}
    }
}
