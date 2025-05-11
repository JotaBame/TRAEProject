using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.TRAEDebuffs;

namespace TRAEProject.NewContent.Items.Weapons.Ranged.Ammo
{
    public class BloodyArrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Bloody Arrow");
            // Tooltip.SetDefault("Leaves Blood Drops in its path");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 9;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 8);
            Item.rare = ItemRarityID.Blue;
            Item.width = 14;
            Item.height = 32;
            Item.shootSpeed = 4;
            Item.consumable = true;
            Item.shoot = ProjectileType<BloodyArrowShot>();
            Item.ammo = AmmoID.Arrow;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(33).AddIngredient(ItemID.WoodenArrow, 33)
                .AddIngredient(ItemID.WormTooth, 1)
                .AddIngredient(ItemID.ViciousPowder, 1)

                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class BloodyArrowShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("BloodyArrow");     //The English name of the Projectile

        }
        public override void SetDefaults()
        {
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            Projectile.timeLeft = 1200;
            Projectile.penetrate = 2;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hostile = false;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] % 3 == 0)
            {
                int dust = Dust.NewDust(Projectile.position, 1, 1, DustID.Blood, Projectile.velocity.X, Projectile.velocity.Y, 0, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.2f;
            }
            if (Projectile.localAI[0] % 30 == 0)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ProjectileType<BloodyArrowDrop>(), (int)(Projectile.damage * 0.67f), Projectile.knockBack * 0.5f, Projectile.owner);

                Projectile.localAI[0] = 0;

            }
        }
    }
    public class BloodyArrowDrop : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Blood Drop");     //The English name of the Projectile

        }
        public override void SetDefaults()
        {
            Projectile.timeLeft = 120;
            Projectile.CloneDefaults(ProjectileID.CursedDartFlame);
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            Projectile.localAI[0]++;
            if (Projectile.localAI[0] % 2 == 0)
            {
                int dust = Dust.NewDust(Projectile.position, 1, 1, DustID.Blood, Projectile.velocity.X, Projectile.velocity.Y, 0, default, 1f);
                Main.dust[dust].velocity *= 0.2f;
                Projectile.localAI[0] = 0;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return true;
        }
    }
}

