
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Accesories.ExtraJumps
{
    public class FlightForecast : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 38;
            Item.value = Item.sellPrice(gold: 6);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TRAEJumps>().advFlight = true;
            player.hasJumpOption_Cloud = true;
            player.GetModPlayer<Mobility>().flightTimeBonus += 0.2f;

            player.jumpSpeedBoost += Mobility.JSV(0.2f);

        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<AdvFlightSystem>(), 1)
                .AddIngredient(ItemID.CloudinaBottle, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}