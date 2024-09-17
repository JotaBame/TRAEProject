
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Accesories.MobilityMisc;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Accesories.MobilityMisc
{
	[AutoloadEquip(EquipType.Shoes)]
   public class FroststrideBoots : ModItem
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
            Item.rare = ItemRarityID.Pink; Item.width = 36;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 4);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!GetInstance<TRAEConfig>().MobilityRework)
            {
                player.accRunSpeed = 6f;
                player.rocketTimeMax += 5;
                player.iceSkate = true;
            }
            else
                player.moveSpeed += 0.20f;
 
            player.desertBoots = false;
            player.sailDash = false;
            player.coldDash = true;
            player.desertDash = false;
            player.GetJumpState(ExtraJump.BlizzardInABottle).Enable();
            player.GetModPlayer<Mobility>().blizzardDash = true;
            player.dashType = 99;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip0" && !GetInstance<TRAEConfig>().MobilityRework)
                {
                    line.Text = "Allows flight, super fast running and extra mobility on ice";

                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.FrostsparkBoots, 1)
            .AddIngredient(ItemID.BlizzardinaBottle, 1)
           .AddIngredient(ItemID.FrostCore, 1)
           .AddTile(TileID.TinkerersWorkbench)
           .Register(); CreateRecipe().AddIngredient(ItemID.FrostsparkBoots, 1)
             .AddIngredient(ItemType<BlizzardSkates>(), 1)
             .AddIngredient(ItemID.FrostCore, 1)
             .AddTile(TileID.TinkerersWorkbench)
             .Register();
            if (!GetInstance<TRAEConfig>().MobilityRework)
            {
                CreateRecipe().AddIngredient(ItemID.FlurryBoots, 1)
                .AddIngredient(ItemID.IceSkates, 1)
                .AddIngredient(ItemID.BlizzardinaBottle, 1)
                .AddIngredient(ItemID.FrostCore, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
                CreateRecipe().AddIngredient(ItemID.FlurryBoots, 1)
          .AddIngredient(ItemType<BlizzardSkates>(), 1)
          .AddIngredient(ItemID.FrostCore, 1)
          .AddTile(TileID.TinkerersWorkbench)
          .Register();
            }

        }
    }
}
