
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Accesories.SpaceBalloon;

namespace TRAEProject.NewContent.Items.Accesories.ExtraJumps
{
    [AutoloadEquip(EquipType.Balloon)]

    public class FaeInABalloon : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Fae Balloon");
            // Tooltip.SetDefault("A weak double jump that grant immunity frames\nIncreases jump height\nAllows reducing gravity by hodling up");
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 44;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
			Item.value = Item.buyPrice(gold: 6, silver: 50);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetJumpState<FaeJump>().Enable();
            player.jumpBoost = true; 
            player.noFallDmg = true;

            player.GetModPlayer<SpaceBalloonPlayer>().SpaceBalloon += 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<FaeInABottle>(), 1)
                .AddIngredient(ModContent.ItemType<SpaceBalloonItem>(), 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}