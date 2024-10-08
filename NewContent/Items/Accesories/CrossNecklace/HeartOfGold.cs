﻿
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Accesory;

namespace TRAEProject.NewContent.Items.Accesories.CrossNecklace
{
    [AutoloadEquip(EquipType.Neck)]
    public class HeartOfGold: ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;


        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.width = 26;
            Item.height = 34;
            Item.value = Item.sellPrice(gold: 4);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<OnHitEffects>().crossNecklace = true;
            player.GetModPlayer<OnHitEffects>().panicNecklaces += 1;
            player.GetModPlayer<HoneyCombPlayer>().combs += 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.CrossNecklace, 1)
                .AddIngredient(ItemID.SweetheartNecklace, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register(); 
            CreateRecipe().AddIngredient(ItemID.CrossNecklace, 1)
                .AddIngredient(ItemID.PanicNecklace, 1)
                .AddIngredient(ItemID.HoneyComb, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<HeartcrossNecklace>(), 1)
                .AddIngredient(ItemID.HoneyComb, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            CreateRecipe().AddIngredient(ModContent.ItemType<HeartcrossNecklace>(), 1)
                .AddIngredient(ItemID.SweetheartNecklace, 1)
               .AddTile(TileID.TinkerersWorkbench)
               .Register();
        }
    }
}