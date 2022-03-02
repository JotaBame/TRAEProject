using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Items.Accesories.WeirdBundle
{
    [AutoloadEquip(EquipType.Balloon)]
    public class BundleOfWeirdBalloons : ModItem
    {

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Bundle of Weird Balloons");
            Tooltip.SetDefault("Allows the user to triple jump\nIncreases regeneration and jump height\nReleases bees and douses the wielder in honey when damaged\n'Weird people need weird people'");
        }

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Yellow;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.hasJumpOption_Fart = true;
            player.hasJumpOption_Sail = true;
            player.honeyCombItem = Item;
                player.jumpBoost = true;
            player.lifeRegen += 2;
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
    
