using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
using TRAEProject.NewContent.TRAEDebuffs;
using TRAEProject.Changes.Accesory;

namespace TRAEProject.NewContent.Items.Weapons.Ranged.Ammo
{
    public class ShroomiteBullet: ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Shroomite Bullet");
            // Tooltip.SetDefault("Critical strikes deal 20% more damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(0, 0, 0, 20);
            Item.rare = ItemRarityID.Yellow;
            Item.width = 12;
            Item.height = 15;
            Item.shootSpeed = 6;
            Item.consumable = true;
            Item.shoot = ProjectileType<ShroomiteShot>();
            Item.ammo = AmmoID.Bullet;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(ItemID.EmptyBullet, 100)
                .AddIngredient(ItemID.ShroomiteBar, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class ShroomiteShot: ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("ShroomitSHot");     //The English name of the Projectile

        }
        public override void SetDefaults()
        {
            AIType = ProjectileID.Bullet;
            Projectile.CloneDefaults(ProjectileID.Bullet);
            Projectile.GetGlobalProjectile<ScopeAndQuiver>().AffectedByReconScope = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().CritDamage = 0.25f;
            Projectile.timeLeft = 1200;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 2;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hostile = false;
            Projectile.friendly = true;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            Lighting.AddLight(Projectile.Center, 0f, 0f, 0.4f);
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.position);
        }
    }
}


