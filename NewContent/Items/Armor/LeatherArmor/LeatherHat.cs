using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.NewContent.Items.Materials;
namespace TRAEProject.NewContent.Items.Armor.LeatherArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class LeatherHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Leather Hood");
           
            Head.Sets.DrawHatHair[Item.headSlot] = true;
            // Tooltip.SetDefault("3% increased summon damage");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(0, 0, 1, 90);
            Item.rare = ItemRarityID.Blue;
            Item.width = 26;
            Item.height = 24;
            Item.defense = 3;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<SummonDamageClass>() += 0.03f;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ItemID.WormTooth, 9)
                .AddIngredient(ItemID.Leather, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
