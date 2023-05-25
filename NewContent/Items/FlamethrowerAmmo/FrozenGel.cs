
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.Common;
namespace TRAEProject.NewContent.Items.FlamethrowerAmmo
{
    public class FrozenGel : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Glacial Gel");
            // Tooltip.SetDefault("Create the coldest winds");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99; AmmoID.Sets.IsSpecialist[Item.type] = true;

        }

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 0.5f;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.rare = ItemRarityID.Pink;
            Item.width = 24;
            Item.height = 24;
            Item.shootSpeed = 0;
            Item.consumable = true;
            Item.shoot = ProjectileType<FrozenGelP>();
            Item.ammo = AmmoID.Gel;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(200).AddIngredient(ItemType<IceQueenJewel>())
                .AddIngredient(ItemID.Gel, 10)
                .AddTile(TileID.Solidifier)
                .Register();
        }
    }
    public class FrozenGelP : FlamethrowerProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("CursedFlamethrower");     //The English name of the Projectile

        }
        public override string Texture => "Terraria/Images/Item_0";
        public override void FlamethrowerDefaults()
        {
            ColorMiddle = new Color(95, 120, 255, 200);
            ColorBack = new Color(215, 255, 249, 200);
            ColorLerp = new Color(95, 160, 255, 200);
            ColorSmoke = new Color(33, 125, 202, 100);
            dustID = 135;
            scalemodifier = 0.2f;
            Projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.1f;
        }

        public override bool PreAI()
        {
            if (scalemodifier < 2.4f)               
                scalemodifier += 2f / 60;
            return true;
        }

    }
}



