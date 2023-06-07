
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
namespace TRAEProject.NewContent.Items.FlamethrowerAmmo
{
    public class BlessedGel : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Water Gel");
            // Tooltip.SetDefault("Flames bounce to their targets");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
            AmmoID.Sets.IsSpecialist[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 3;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 5);
            Item.rare = ItemRarityID.Green;
            Item.width = 16;
            Item.height = 14;
            Item.shootSpeed = 3;
            Item.consumable = true;
            Item.shoot = ProjectileType<BlessedGelP>();
            Item.ammo = AmmoID.Gel;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(2).AddIngredient(ItemID.Gel, 1)
                .AddIngredient(ItemID.Bone, 1)
                .AddTile(TileID.WaterCandle)
                .Register();
        }
    }
    public class BlessedGelP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("BlessedFlamethrower");     //The English name of the Projectile
        }
        public override string Texture => "Terraria/Images/Item_0";
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WaterStream);
            AIType = ProjectileID.WaterStream; 
            Projectile.timeLeft = 210;
            Projectile.penetrate = 3;
            Projectile.extraUpdates = 2;
            Projectile.ignoreWater = true; 
            Projectile.ArmorPenetration = 3;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.friendly = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().SmartBouncesOffEnemies = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().SmartBouncesOffTiles = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontHitTheSameEnemyMultipleTimes = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.2f;
        }
        public override void AI()
        {
            Projectile.localAI[0] += 1;
            if (Projectile.localAI[0] % 8f == 0f)
            {
                Dust.NewDustDirect(Projectile.Center, 1, 1, DustID.Smoke, 0, -1);
            }
	}		
	public override void Kill(int timeLeft)
	{
		{
            for (int i = 0; i < 6; ++i)
            {
                int dust = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 6, 6, DustID.Smoke, 0.0f, 0.0f, 100, default, 3f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 0.8f;
                Main.dust[dust].velocity.Y -= 0.3f;
            }
        }
    }}
}


