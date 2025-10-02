using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
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
            Item.value = Item.sellPrice(0, 6, 6, 6);
            Item.rare = ItemRarityID.Cyan;
            Item.width = 22;
            Item.height = 16;
            Item.defense = 15;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetCritChance<GenericDamageClass>() += 15; 
            player.GetAttackSpeed<MeleeDamageClass>() += 0.25f;
        }
    }
}
