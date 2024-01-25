using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.DreadItems.RedPearl
{
    class RedPearl : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Red Pearl");
            // Tooltip.SetDefault("Increases enemy spawn rates and luck");
        }
        public override void SetDefaults()
        {
            Item.expert = true;
            Item.accessory = true;
            Item.width = 26;
            Item.height = 32;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.sellPrice(gold: 5);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.luck += 0.4f;
            player.GetModPlayer<PearlEffects>().spawnUp = true;
        }
    }
    public class PearlEffects : ModPlayer
    {
        public bool spawnUp = false;
        public override void ResetEffects()
        {
            spawnUp = false;
        }
    }
}
