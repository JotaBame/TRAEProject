using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Items.Accesories.TwoFlowers
{    [AutoloadEquip(EquipType.Waist)]
    class TwoFlowers : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Two Flowers");
            Tooltip.SetDefault("Magic critical hits have a chance to spawn a mana star\nAutomatically uses mana potions when needed\nMagic attacks lower enemy contact damage by 15%");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = 50000;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.lavaRose = true;
            player.manaFlower = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.ObsidianRose, 1)
                .AddIngredient(ItemID.ManaFlower, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
