using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Accesories.ExtraJumps
{
    public class LevitatingSoles : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Levitating Soles");
            // Tooltip.SetDefault("Provides a long lasting double jump\n20% increased movement speed\n25% increased jump speed");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
			Item.value = Item.buyPrice(0, 40);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetJumpState<LevitationJump>().Enable();
            player.moveSpeed += 0.20f;
            player.jumpSpeedBoost += Mobility.JSV(0.24f);
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<LevitationJuice>(), 1)
                .AddIngredient(ItemID.AmphibianBoots, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}