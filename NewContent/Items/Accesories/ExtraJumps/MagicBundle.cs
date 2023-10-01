using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Accesories.AlligatorBalloon;
using TRAEProject.NewContent.Items.Accesories.SpaceBalloon;

namespace TRAEProject.NewContent.Items.Accesories.ExtraJumps
{
    [AutoloadEquip(EquipType.Balloon)]

    public class MagicBundle : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

      
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Cyan;
			Item.value = Item.buyPrice(gold: 10);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            player.GetJumpState(ExtraJump.TsunamiInABottle).Enable();
            player.extraFall += 50;
            player.autoJump = true;
            player.jumpSpeedBoost += Mobility.JSV(0.24f); 
            player.GetJumpState<FaeJump>().Enable();
            player.jumpBoost = true;
            player.GetModPlayer<SpaceBalloonPlayer>().SpaceBalloon += 1;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ModContent.ItemType<FaeInABalloon>(), 1)
                .AddIngredient(ModContent.ItemType<AlligatorBalloonItem>(), 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}