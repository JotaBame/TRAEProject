
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Accesories.MobilityJumps;

namespace TRAEProject.NewContent.Items.Accesories.AdvFlight
{[AutoloadEquip(EquipType.Shoes)]
    public class JetBoots : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Jet boots");
            // Tooltip.SetDefault("Rocket boots and wings are activated by pushing UP instead of jump\nIncreases flight time by 40%\nProvides rocket boot flight(10)\nProvides a booster double jump");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 40;
            Item.value = Item.sellPrice(gold: 9);
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TRAEJumps>().advFlight = true;
            player.rocketTimeMax += 10;
            player.GetModPlayer<TRAEJumps>().boosterCount++;
            player.GetJumpState<JetJump>().Enable();
            player.GetModPlayer<Mobility>().flightTimeBonus += 0.4f;
            player.rocketBoots = player.vanityRocketBoots = 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<AdvFlightBoots>(), 1)
                .AddIngredient(ModContent.ItemType<Booster>(), 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}