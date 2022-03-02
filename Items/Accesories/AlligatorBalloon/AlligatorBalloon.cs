using Terraria.GameContent.Creative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Items.Accesories.AlligatorBalloon
{
    [AutoloadEquip(EquipType.Balloon)]
    class AlligatorBalloon : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Croco Balloon");
            Tooltip.SetDefault("Allows the holder to double jump\nGreatly increases jump speed and fall resistance\nAllows auto-jump");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = 50000;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.jumpBoost = true;
            player.hasJumpOption_Sail = true;
            player.extraFall += 15;
			player.autoJump = true;
            player.jumpSpeedBoost += 1.4f;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.FrogLeg, 1)
                .AddIngredient(ItemID.SharkronBalloon, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
