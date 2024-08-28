using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ID.ArmorIDs;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.NewContent.Items.Materials;
namespace TRAEProject.NewContent.Items.Armor.Joter
{
    [AutoloadEquip(EquipType.Head)]
    public class JoterMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Joter Mask");

            // Tooltip.SetDefault("Great for impersonating abominations impersonating developers!");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.ShimmerTransformToItem[Item.type] = ItemType<FinalBoss>();
        }
        public override void SetDefaults()
        {
            Item.value = Item.sellPrice(0, 0, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.width = 26;
            Item.vanity = true;
            Item.height = 32;
        }
    }
}
