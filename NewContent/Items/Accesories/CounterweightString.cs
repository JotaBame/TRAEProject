
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Accesories
{
    class CounterweightString : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Counterweight String");
            // Tooltip.SetDefault("Increases yoyo range\nThrows a counterweight after hitting an enemy with a yoyo");
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(gold: 3);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.counterWeight = 556 + Main.rand.Next(6);
            player.yoyoString = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.WhiteString, 1)
                .AddIngredient(ItemID.GreenCounterweight, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            CreateRecipe().AddIngredient(ItemID.WhiteString, 1)
               .AddIngredient(ItemID.RedCounterweight, 1)
               .AddTile(TileID.TinkerersWorkbench)
               .Register();
            CreateRecipe().AddIngredient(ItemID.WhiteString, 1)
               .AddIngredient(ItemID.BlueCounterweight, 1)
               .AddTile(TileID.TinkerersWorkbench)
               .Register();
            CreateRecipe().AddIngredient(ItemID.WhiteString, 1)
               .AddIngredient(ItemID.BlackCounterweight, 1)
               .AddTile(TileID.TinkerersWorkbench)
               .Register();
            CreateRecipe().AddIngredient(ItemID.WhiteString, 1)
               .AddIngredient(ItemID.PurpleCounterweight, 1)
               .AddTile(TileID.TinkerersWorkbench)
               .Register();
            CreateRecipe().AddIngredient(ItemID.WhiteString, 1)
               .AddIngredient(ItemID.YellowCounterweight, 1)
               .AddTile(TileID.TinkerersWorkbench)
               .Register();
        }
    }
}
