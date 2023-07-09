
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Accesories.ExtraJumps
{
    public class AdvFlightBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Adv. Flight boots");
            // Tooltip.SetDefault("Rocket boots and wings are activated by pushing UP instead of jump\nIncreases flight time by 40%\nProvides rocket boot flight(10)");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.value = Item.sellPrice(gold: 4);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TRAEJumps>().advFlight = true;
            player.GetModPlayer<Mobility>().flightTimeBonus += 0.4f;
            player.rocketTimeMax += 10;
            player.rocketBoots = player.vanityRocketBoots = 1;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<AdvFlightSystem>(), 1)
                .AddIngredient(ItemID.RocketBoots, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}