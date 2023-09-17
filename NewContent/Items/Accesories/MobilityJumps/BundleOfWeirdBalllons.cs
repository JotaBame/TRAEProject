
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Accesory;

namespace TRAEProject.NewContent.Items.Accesories.MobilityJumps
{
    [AutoloadEquip(EquipType.Balloon)]
    public class BundleOfWeirdBalloons : ModItem
    {

        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Bundle of Weird Balloons");
            // Tooltip.SetDefault("Allows the user to triple jump\nIncreases jump height\nReleases bees and douses the wielder in honey when damaged\nMultiple combs increase efficiency and life regeneration");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 3);
            Item.rare = ItemRarityID.Yellow;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            player.GetJumpState(ExtraJump.FartInAJar).Enable();
            player.GetJumpState(ExtraJump.TsunamiInABottle).Enable();
            player.jumpBoost = true;
            player.GetModPlayer<HoneyCombPlayer>().combs += 1;
            player.noFallDmg = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.SoulofFlight, 20)
               .AddIngredient(ItemID.FartInABalloon)
          .AddRecipeGroup("TsunamiJump")
            .AddRecipeGroup("HoneyBalloon")
           .AddTile(TileID.TinkerersWorkbench)
                .Register(); 
            CreateRecipe().AddIngredient(ItemID.SoulofFlight, 20)
        .AddIngredient(ItemID.SharkronBalloon)
   .AddRecipeGroup("FartJump")
     .AddRecipeGroup("HoneyBalloon")
    .AddTile(TileID.TinkerersWorkbench)
         .Register(); 
            CreateRecipe().AddIngredient(ItemID.SoulofFlight, 20)
        .AddIngredient(ItemID.HoneyBalloon)
   .AddRecipeGroup("TsunamiJump")
     .AddRecipeGroup("FartJump")
    .AddTile(TileID.TinkerersWorkbench)
         .Register();
        }
    }
}
    
