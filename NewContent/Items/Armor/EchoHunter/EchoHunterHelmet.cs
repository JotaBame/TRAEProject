using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.NewContent.Items.Materials;

namespace TRAEProject.NewContent.Items.Armor.EchoHunter
{
    [AutoloadEquip(EquipType.Head)]
    public class EchoHunterHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Underworld Warrior Helmet");
            // Tooltip.SetDefault("15% increased damage\n25% reduced mana costs");
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
            player.GetDamage<GenericDamageClass>() += 0.18f;
            player.moveSpeed += 0.05f;
            player.jumpSpeedBoost += Mobility.JSV(0.05f);
        }
        public override void AddRecipes()
        {
            CreateRecipe()

            .AddIngredient(ItemType<EchoHeart>(), 7)
            .AddIngredient(ItemID.SoulofFright, 7)
            .AddIngredient(ItemID.SoulofSight, 7)
            .AddIngredient(ItemID.SoulofMight, 7)
            .AddTile(TileID.MythrilAnvil)
            .Register();

        }
    }
}

