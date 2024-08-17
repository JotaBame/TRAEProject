
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Accesories
{    [AutoloadEquip(EquipType.Neck)]
  public  class PutridToothNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Bone Biter Necklace");
            // Tooltip.SetDefault("Increases armor penetration by 5\n5% increased damage and critical strike chance\nEnemies are less likely to target you");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.width = 26;
            Item.height = 36;
            Item.value = Item.sellPrice(gold: 9);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += 0.04f;
            player.GetCritChance(DamageClass.Generic) += 4;
            player.GetArmorPenetration(DamageClass.Generic) += 5;
            player.aggro -= 500;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.PutridScent, 1)
                .AddIngredient(ItemID.SharkToothNecklace, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
