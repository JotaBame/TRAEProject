using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.BeholderItems
{

    class RingOfTenacity : ModItem
    {

        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemType<RingOfFuror>();

            // DisplayName.SetDefault("Ring of Tenacity");
            // Tooltip.SetDefault("18% increased maximum hp\n6% reduced total damage");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(gold: 7);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 = (int)(player.statLifeMax2 * 1.22f);
            player.GetDamage(DamageClass.Generic) *= 0.94f;
        }

    }
}
