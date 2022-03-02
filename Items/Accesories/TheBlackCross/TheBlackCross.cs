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

namespace TRAEProject.Items.Accesories.TheBlackCross
{    [AutoloadEquip(EquipType.Neck)]
    class TheBlackCross : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("The Black Cross");
            Tooltip.SetDefault("Increases length of invincibility after taking damage.\nGrants the wearer a chance to dodge an attack");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 75000;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.longInvince = true;
            player.GetModPlayer<BlackCrossDodge>().BlackCrossBelt = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.CrossNecklace, 1)
                .AddIngredient(ItemID.BlackBelt, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
    public class BlackCrossDodge : ModPlayer
    {
        public bool BlackCrossBelt = false;
		
		public override void ResetEffects()
        {
            BlackCrossBelt = false;
        }
        public override void UpdateDead()
        {
            BlackCrossBelt = false;
        }
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (BlackCrossBelt && Main.rand.Next(10) == 0)
            {
                Player.NinjaDodge();
                Player.SetImmuneTimeForAllTypes(120);
                return false;
            }
            return true;
        }
    }
}
