﻿
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Accesory;

namespace TRAEProject.NewContent.Items.Accesories.CrossNecklace
{    [AutoloadEquip(EquipType.Neck)]
    public class HeartcrossNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

     
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.width = 26;
            Item.height = 32;
            Item.value = Item.sellPrice(gold:3, silver: 50);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<OnHitEffects>().crossNecklace = true;
            player.GetModPlayer<OnHitEffects>().panicNecklaces += 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.CrossNecklace, 1)
                .AddIngredient(ItemID.PanicNecklace, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
