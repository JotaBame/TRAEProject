
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
            Item.width = 34;
            Item.height = 40; 
            Item.value = Item.sellPrice(gold: 15);
            Item.accessory = true;
            Item.rare = ItemRarityID.Cyan;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TRAEJumps>().boosterCount++;
            player.GetJumpState<JetJump>().Enable();
            player.GetJumpState<LevitationJump>().Enable();
            player.GetJumpState<FaeJump>().Enable();
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