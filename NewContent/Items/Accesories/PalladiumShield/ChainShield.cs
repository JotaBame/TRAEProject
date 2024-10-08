﻿
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Accesory;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Accesories.PalladiumShield
{    [AutoloadEquip(EquipType.Shield)]
   public class ChainShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Chain Shield");
            // Tooltip.SetDefault("Increases max life by 40\nTemporarily increases defense when damaged");
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 3);
                Item.defense = 3;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 50;
                    player.GetModPlayer<OnHitEffects>().shackles += 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.Shackle, 1)
                .AddIngredient(ItemType<PalladiumShield>(), 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
