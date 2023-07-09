
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Accesories.ShamanNecklace
{    [AutoloadEquip(EquipType.Neck)]
    class ShamanNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Shaman Necklace");
            // Tooltip.SetDefault("Increases your max number of minions by 1\n12% increased summon damage");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.width = 30;
            Item.height = 32;
            Item.value = Item.sellPrice(gold:10);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ++player.maxMinions;
            player.GetDamage(DamageClass.Summon) += 0.12f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.PygmyNecklace, 1)
                .AddIngredient(ItemID.AvengerEmblem, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
