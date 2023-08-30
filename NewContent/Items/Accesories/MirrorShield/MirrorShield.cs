
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Accesories.MirrorShield
{    [AutoloadEquip(EquipType.Shield)]
    class MirrorShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Mirror Shield");
            // Tooltip.SetDefault("Increases max life by 40\nReduces damage taken from projectiles by 12%");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.width = 30;
            Item.height = 36;
            Item.value = Item.sellPrice(gold: 4);
            Item.defense = 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.PocketMirror, 1)
                .AddIngredient(ItemType<PalladiumShield.PalladiumShield>(), 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 40;
            player.GetModPlayer<Defense>().pocketMirror = true;
        }

    }
}
