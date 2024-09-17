
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Accesories.MobilityMisc
{
	[AutoloadEquip(EquipType.Shoes)]
  public  class SandstormBoots : ModItem
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
            Item.rare = ItemRarityID.Orange; Item.width = 38;
            Item.height = 28;
            Item.value = Item.sellPrice(gold: 2);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
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
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
