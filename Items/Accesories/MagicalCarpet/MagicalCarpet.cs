using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Items.Accesories.MagicalCarpet
{

    [AutoloadEquip(EquipType.Wings)]
    class MagicalCarpet : ModItem
    {

		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Grants flight, slow fall, and hover\n'I will show you the world...'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(75, 6.25f, 1f, hasHoldDownHoverFeatures: true, 7f, 7f);
			
		}

		public override void SetDefaults()
		{
		    Item.width = 66;
			Item.height = 24;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient(ItemID.FlyingCarpet)
				.AddIngredient(ItemID.AncientBattleArmorMaterial, 1)
				.AddIngredient(ItemID.SoulofFlight, 20)
				.AddTile(TileID.TinkerersWorkbench)
				.Register();
		}


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<Hover>().hasHoverWing = true;
        }


        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.5f;
            ascentWhenRising = 0.1f;
            maxCanAscendMultiplier = .5f;
            maxAscentMultiplier = 1.8f;
            constantAscend = 0.1f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            if (player.controlDown && player.controlJump && player.wingTime > 0f)
            {
                if (!player.controlLeft && !player.controlRight)
                {
                    player.wingTime += .8f;
                }
                speed = 7f;
                acceleration = 7f;
            }
            else
            {
                speed = 6.25f;
            }
        }
        public override bool WingUpdate(Player player, bool inUse)
        {
            
            if (player.velocity.Y != 0f || player.grappling[0] > -1)
            {
                if (player.ShouldDrawWingsThatAreAlwaysAnimated())
                {
                    player.legFrame.Y = 0;
                    return false;
                }
            }

            if (!player.ShouldDrawWingsThatAreAlwaysAnimated() || player.mount.Type > 0)
            {
                player.wingFrame = 5;
            }
            
            return true;
        }
    }

    public class Hover : ModPlayer
    {
        public bool hasHoverWing = false;

        public override void ResetEffects()
        {
            hasHoverWing = false;
        }

        public override void PostUpdateEquips()
        {
            if (hasHoverWing && Player.controlDown && Player.controlJump && Player.wingTime > 0f && !Player.merman)
            {
                Player.velocity.Y = Player.velocity.Y * 0.9f;
                if (Player.velocity.Y > -2f && Player.velocity.Y < 1f)
                {
                    Player.velocity.Y = 1E-05f;
                    Player.position.Y += .1f;
                }
            }
        }
    }

}
