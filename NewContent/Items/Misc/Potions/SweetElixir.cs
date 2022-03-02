using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject;
using TRAEProject.Changes.Prefixes;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Misc.Potions
{
    class SweetElixir : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;

            DisplayName.SetDefault("Sweet Elixir");
            Tooltip.SetDefault("Grants Honey for 45 seconds");
        }
        public override void SetDefaults()
        {

            Item.DefaultToHealingPotion(20, 30, 120);
         Item.consumable = true;
            Item.maxStack = 30;
            Item.useTime = Item.useAnimation = 30;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.buyPrice(silver: 20);
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
        }
        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffID.Honey, 45 * 60);
            int potionSickness = 60;
            if (player.pStone == true)
            {
                potionSickness = 45;
            }
            player.AddBuff(BuffID.PotionSickness, potionSickness * 60);
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ItemID.GreaterHealingPotion, 1)
                .AddIngredient(ItemID.Honeyfin, 1)
                .AddTile(TileID.AlchemyTable)
                .Register();
        }
    }
}