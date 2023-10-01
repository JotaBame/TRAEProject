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
    public class FantasyPack : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
			Item.value = Item.buyPrice(0, 40);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TRAEJumps>().boosterCount++;
            player.GetJumpState<JetJump>().Enable();
            player.GetJumpState<LevitationJump>().Enable();
            player.GetJumpState<FaeJump>().Enable();
            player.noFallDmg = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<FaeInABottle>(), 1)
                .AddIngredient(ModContent.ItemType<Booster>(), 1)
                .AddIngredient(ModContent.ItemType<LevitationJuice>(), 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}