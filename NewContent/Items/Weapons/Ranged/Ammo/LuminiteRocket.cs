using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;

namespace TRAEProject.NewContent.Items.Weapons.Ranged.Ammo
{
    public class LuminiteRocket: ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Luminite Rocket");
            // Tooltip.SetDefault("Giant explosion\nExplodes multiple times");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
            AmmoID.Sets.IsSpecialist[Item.type] = true;

        }
    public override void SetDefaults()
        {
            Item.damage = 200;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(0, 0, 0, 74);
            Item.rare = ItemRarityID.Yellow;
            Item.width = 12;
            Item.height = 15;
            Item.shootSpeed = 6;
            Item.consumable = true;
            Item.ammo = AmmoID.Rocket;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(500).AddIngredient(ItemID.RocketI, 500)
                .AddIngredient(ItemID.LunarBar, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

}


