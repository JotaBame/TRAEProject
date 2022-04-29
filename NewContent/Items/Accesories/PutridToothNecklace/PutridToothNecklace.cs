using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Accesories.PutridToothNecklace
{    [AutoloadEquip(EquipType.Neck)]
    class PutridToothNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bone Biter Necklace");
            Tooltip.SetDefault("Increases armor penetration by 5\n5% increased damage and critical strike chance\nEnemies are less likely to target you");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = 75000;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += 0.05f;
            player.GetCritChance(DamageClass.Generic) += 5;
            player.GetArmorPenetration(DamageClass.Generic) += 5;
            player.aggro -= 500;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.PutridScent, 1)
                .AddIngredient(ItemID.SharkToothNecklace, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
