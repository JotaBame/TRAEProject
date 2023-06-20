using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Accesories.PalladiumShield
{   
    [AutoloadEquip(EquipType.Shield)]
    class PalladiumShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Palladium Shield");
            // Tooltip.SetDefault("Increases max life by 40");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = 75000;
            Item.defense = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 50;
        }

    }
}
