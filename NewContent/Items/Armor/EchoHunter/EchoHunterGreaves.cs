using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.NewContent.Items.Materials;
namespace TRAEProject.NewContent.Items.Armor.EchoHunter
{
    [AutoloadEquip(EquipType.Legs)]
    public class EchoHunterGreaves : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Underworld Warrior Greaves");
            // Tooltip.SetDefault("15% increased critical strike chance and movement speed\n25% increased melee speed");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.width = 22;
            Item.height = 16;
            Item.defense = 12;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<GenericDamageClass>() += 0.09f;

            player.moveSpeed += 0.05f;
            player.jumpSpeedBoost += Mobility.JSV(0.05f);

        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemType<EchoHeart>(), 6)
                .AddIngredient(ItemID.SoulofSight, 8)
                .AddIngredient(ItemID.SoulofMight, 8)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
