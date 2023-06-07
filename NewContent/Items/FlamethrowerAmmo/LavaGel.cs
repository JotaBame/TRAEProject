
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
namespace TRAEProject.NewContent.Items.FlamethrowerAmmo
{
    public class LavaGel : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lava Gel");
            // Tooltip.SetDefault("Ignite it to know what REAL fire is!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99; AmmoID.Sets.IsSpecialist[Item.type] = true;

        }

        public override void SetDefaults()
        {
            Item.damage = 3;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 0, 0, 5);
            Item.rare = ItemRarityID.Orange;
            Item.width = 22;
            Item.height = 18;
            Item.shootSpeed = 4.5f;
            Item.consumable = true;
            Item.shoot = ProjectileType<LavaGelP>();
            Item.ammo = AmmoID.Gel;
            Item.maxStack = 9999;
        }
        public override void AddRecipes()
        {
            CreateRecipe(50).AddIngredient(ItemID.Hellstone, 3)
                .AddIngredient(ItemID.Gel, 20)
                .AddTile(TileID.Hellforge)
                .Register();
        } 
    }
    public class LavaGelP : FlamethrowerProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("CursedFlamethrower");     //The English name of the Projectile

        }
        public override string Texture => "Terraria/Images/Item_0";
        public override void FlamethrowerDefaults()
        {
            ColorMiddle = new Color(255, 50, 50, 200);
            ColorBack = new Color(240, 170, 23, 200);
            ColorLerp = Color.Lerp(ColorMiddle, ColorBack, 0.25f);
            ColorSmoke = new Color(65, 65, 65, 100);
            dustID = DustID.RedTorch;
            dustScale = 0.8f;
            Projectile.ArmorPenetration = 3;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.OnFire3;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuffDuration = 60;
            Projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.2f;
            dieInWater = true;
        }
    }
}




