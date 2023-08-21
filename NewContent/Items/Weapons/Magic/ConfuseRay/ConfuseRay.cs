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

namespace TRAEProject.NewContent.Items.Weapons.Magic.ConfuseRay
{
    class ConfuseRay : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.DefaultToStaff(ProjectileType<ConfuseRayBeam>(), 6, 24, 10);
            Item.width = 38;
            Item.height = 32;
            Item.damage = 23;
            Item.crit = 9;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 20);
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item72;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.AmethystStaff, 1)
                .AddIngredient(ItemID.Blinkroot, 1)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Anvils)
                .Register(); 
            CreateRecipe(1).AddIngredient(ItemID.TopazStaff, 1)
                .AddIngredient(ItemID.Blinkroot, 1)
                .AddIngredient(ItemID.FallenStar, 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    class ConfuseRayBeam : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.friendly = true; 

            Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 10;
		    Projectile.height = 10;
			Projectile.extraUpdates = 20;
            Projectile.timeLeft = 240;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.Confused;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffMinDuration = 60;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffDuration = 150;

        }
        float angle = 60f * (MathF.PI / 180f);
        float zigzagTimer = 3;
        float bounceAt = 12;
        int bounced = 0;

        public override void AI()
        {
			Projectile.ai[0] += 1f;
           
            if (Projectile.ai[0] >= bounceAt)
            {
                Projectile.ai[0] = 0;
                Projectile.velocity = Projectile.velocity.RotatedBy(angle);
                angle *= -1;

                if (bounced == 1)
                {
                    bounced++;
                    bounceAt = zigzagTimer * 2; //takes twice as long after the first bounce
                }
                if (bounced == 0)
                {
                    bounced++;
                    angle *= 2f;
                    bounceAt = zigzagTimer;
                }
             

            }
            for (int i = 0; i < 2; i++)
			{
                int type = i == 1 ? DustID.PinkTorch : DustID.OrangeTorch;
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