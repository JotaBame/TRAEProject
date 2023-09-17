
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Accesory;

namespace TRAEProject.NewContent.Items.Accesories.CrossNecklace
{
    [AutoloadEquip(EquipType.Neck)]
    class HeartOfGold: ModItem
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
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 4);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.longInvince = true;
            player.panic = true;
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