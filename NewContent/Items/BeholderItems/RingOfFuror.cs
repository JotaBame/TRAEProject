using static Terraria.ModLoader.ModContent;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.BeholderItems
{

    class RingOfFuror : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemType<RingOfMight>();
            // DisplayName.SetDefault("Ring of Fury");
            // Tooltip.SetDefault("12% increased total damage\n9% reduced maximum HP");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(gold: 7);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 = (int)(player.statLifeMax2 / 1.11f);
            player.GetDamage(DamageClass.Generic) *= 1.12f;
        }
    }
}
