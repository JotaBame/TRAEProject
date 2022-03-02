using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Items.Accesories.ShamanNecklace
{    [AutoloadEquip(EquipType.Neck)]
    class ShamanNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Shaman Necklace");
            Tooltip.SetDefault("Increases your max number of minions by 1\n12% increased summon damage");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = 75000;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ++player.maxMinions;
            player.GetDamage(DamageClass.Summon) += 0.12f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.PygmyNecklace, 1)
                .AddIngredient(ItemID.AvengerEmblem, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
