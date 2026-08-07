using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Weapons.Magic.FlashRay
{
    public class FlashRay : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.DefaultToStaff(ProjectileType<FlashRayBeam>(), 6, 27, 15);
            Item.width = 40;
            Item.height = 40;
            Item.damage = 24;
            Item.crit = 3;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 22);
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 6.25f;
            Item.UseSound = SoundID.Item72;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;

            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
        }
        public override void AddRecipes()
        {

            CreateRecipe(1).AddIngredient(ItemID.TopazStaff, 1)
                .AddIngredient(ItemID.Blinkroot, 1)
                .AddIngredient(ItemID.Deathweed, 1)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class FlashRayBeam : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.extraUpdates = 10;
            Projectile.timeLeft = 240;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;


        }
 

        public override void AI()
        {
             Projectile.ai[0] += 1f;
            int bounceAt = (int)6;
            if (Projectile.ai[1] > 1)
            {
                bounceAt *= 2;
            }

            float angle = 40f * (MathF.PI / 180f);

            if (Projectile.ai[0] >= bounceAt)
            {
                Projectile.ai[0] = 0;
                Projectile.ai[1] += 1;


                if (Projectile.ai[1] % 2 == 0)
                {
                    angle *= -1;
                }
                if (Projectile.ai[1] > 1)
                {
                    angle *= 2;
                }
                Projectile.velocity = Projectile.velocity.RotatedBy(angle);
            }
            for (int i = 0; i < 2; i++)
			{
                int type = i == 1 ? DustID.YellowTorch : DustID.OrangeTorch;
				int Dust = Terraria.Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, type, 0f, 0f, 100);
				Main.dust[Dust].position = (Main.dust[Dust].position + Projectile.Center) / 2f;
				Main.dust[Dust].noGravity = true;
				Dust dust = Main.dust[Dust];
				dust.velocity *= 0.1f;
				if (i == 1)
				{
					dust = Main.dust[Dust];
					dust.position += Projectile.velocity / 2f;
				}
				float ScaleMult = (1000f - Projectile.ai[0]) / 500f;
				dust = Main.dust[Dust];
				dust.scale *= ScaleMult + 0.1f;
			}

		}
	}
}