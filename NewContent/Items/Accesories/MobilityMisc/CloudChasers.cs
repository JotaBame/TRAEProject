
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Accesories.AdvFlight;
using TRAEProject.NewContent.Items.Accesories.MobilityJumps;
using TRAEProject.NewContent.Items.Accesories.MobilityMisc;
using TRAEProject.NewContent.Items.Armor.Joter;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Accesories.MobilityMisc
{
	[AutoloadEquip(EquipType.Shoes)]
    public class CloudChasers: ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Sandstorm Boots");
            // Tooltip.SetDefault("20% increased movement speed\nThe wearer can perform an improved double jump\n20% increased movement speed on sand");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink; Item.width = 32;
            Item.height = 30;
            Item.value = Item.sellPrice(gold: 6);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
 
            player.GetModPlayer<TRAEJumps>().advFlight = true; player.noFallDmg = true;

            player.GetJumpState(ExtraJump.CloudInABottle).Enable();
            player.GetModPlayer<Mobility>().flightTimeBonus += 0.4f;
            player.accRunSpeed = 4.8f;
            if (!GetInstance<TRAEConfig>().MobilityRework)
            {
                player.accRunSpeed = 6f;
            }
            else
                player.moveSpeed += 0.20f;
            player.desertBoots = false;
            player.sailDash = false;
            player.coldDash = false;
            player.desertDash = true;
            player.GetModPlayer<AccesoryEffects>().sandRunning = true;

            player.GetJumpState(ExtraJump.SandstormInABottle).Enable();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip0" && !GetInstance<TRAEConfig>().MobilityRework)
                {
                    line.Text = "Allows fast running";

                }
            }
         }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.SandBoots, 1)
                .AddIngredient(ItemID.SandstorminaBottle, 1)
                .AddIngredient(ItemID.CloudinaBottle, 1)
                .AddIngredient(ItemType<AdvFlightSystem>(), 1)

                .AddIngredient(ItemID.AncientBattleArmorMaterial, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            CreateRecipe().AddIngredient(ItemID.CloudinaBottle, 1)
                          .AddIngredient(ItemType<AdvFlightSystem>(), 1)
                          .AddIngredient(ItemType<SandstormBoots>(), 1)
                          .AddIngredient(ItemID.AncientBattleArmorMaterial, 1)
                          .AddTile(TileID.TinkerersWorkbench)
                           .Register();
            CreateRecipe().AddIngredient(ItemType<FlightForecast>(), 1)
                          .AddIngredient(ItemType<SandstormBoots>(), 1)
                          .AddIngredient(ItemID.AncientBattleArmorMaterial, 1)
                          .AddTile(TileID.TinkerersWorkbench)
                          .Register();
            CreateRecipe().AddIngredient(ItemType<FlightForecast>(), 1)
                          .AddIngredient(ItemID.SandBoots, 1)
                          .AddIngredient(ItemID.SandstorminaBottle, 1)
                          .AddIngredient(ItemID.AncientBattleArmorMaterial, 1)
                          .AddTile(TileID.TinkerersWorkbench)
                          .Register();
        }
    }
}
