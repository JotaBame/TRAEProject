using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.NewContent.Items.Materials;
namespace TRAEProject.NewContent.Items.Armor.IceArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class IceMajestyCrown : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glacial Crown");
           
            Head.Sets.DrawHatHair[Item.headSlot] = true;
            Tooltip.SetDefault("Increases your maximum number of minions by 1\n20% increased whip speed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.width = 22;
            Item.height = 30;
            Item.defense = 11;
        }
        public override void UpdateEquip(Player player)
        {
            ++player.maxMinions;
            player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) *= 1 / 1.2f;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemType<IceQueenJewel>(), 1)
                .AddIngredient(ItemID.SpectreBar, 15)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
