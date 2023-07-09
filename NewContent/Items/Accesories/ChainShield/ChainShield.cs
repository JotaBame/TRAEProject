
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Accesory;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Accesories.ChainShield
{    [AutoloadEquip(EquipType.Shield)]
    class ChainShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Chain Shield");
            // Tooltip.SetDefault("Increases max life by 40\nTemporarily increases defense when damaged");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 3);
                Item.defense = 3;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 50;
            player.GetModPlayer<ShackleEffects>().cuffs += 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.Shackle, 1)
                .AddIngredient(ItemType<PalladiumShield.PalladiumShield>(), 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
