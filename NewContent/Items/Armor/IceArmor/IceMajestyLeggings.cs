using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.NewContent.Items.Materials;
namespace TRAEProject.NewContent.Items.Armor.IceArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class IceMajestyLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glacial Leggings");
            Tooltip.SetDefault("10% increased summon damage and whip speed\n20% increased movement speed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.width = 22;
            Item.height = 16;
            Item.defense = 13;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<IceQueenJewel>(), 1)
                .AddIngredient(ItemID.SpectreBar, 18)
                .AddIngredient(ItemID.Silk, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override void UpdateEquip(Player player)
        {
            player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) *= 1 / 1.1f;
            player.GetDamage<SummonDamageClass>() += 0.1f;
            player.moveSpeed += 0.2f;
        }
    }
}
