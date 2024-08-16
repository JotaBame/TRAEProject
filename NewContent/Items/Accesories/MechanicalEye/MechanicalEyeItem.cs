using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using TRAEProject.Common.ModPlayers;

namespace TRAEProject.NewContent.Items.Accesories.MechanicalEye
{
    [AutoloadEquip(EquipType.Face)]

    public class MechanicalEyeItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; 
            // DisplayName.SetDefault("Cyber Eye");
            // Tooltip.SetDefault("Rocket critical strikes stun enemies for 1 second");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.width = 32;
            Item.height = 34;
            Item.value = Item.sellPrice(gold: 5);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RangedStats>().CyberEye += 1;
        }
    }
}
