using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject;
using TRAEProject.Changes.Prefixes;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Misc.Potions
{
    class PowerBrew : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;

            // DisplayName.SetDefault("Power Brew");
            // Tooltip.SetDefault("Increases damage by 30% for 20 seconds after drunk");
        }
        public override void SetDefaults()
        {
            Item.DefaultToHealingPotion(22, 32, 80);
            Item.consumable = true;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.buyPrice(silver: 20);
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
        }
        public override void OnConsumeItem(Player player)
        {
            player.AddBuff(BuffType<Power>(), 20 * 60);
            int potionSickness = 60;
            if (player.pStone == true)
            {
                potionSickness = 45;
            }
            player.AddBuff(BuffID.PotionSickness, potionSickness * 60);
        }
    }
    class Power : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            // DisplayName.SetDefault("Power");
            // Description.SetDefault("Damage increased by 30%");
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) *= 1.30f;
        }
    }

}