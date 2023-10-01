using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Items.Accesories.ExtraJumps
{
    public class AdvFlightSystem : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Adv. Flight System");
            // Tooltip.SetDefault("Rocket boots and wings are activated by pushing UP instead of jump\nIncreases flight time by 40%");
        }
        public override void SetDefaults()
        {
            Item.rare = 5;
            Item.value = 300000;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TRAEJumps>().advFlight = true;
            player.GetModPlayer<Mobility>().flightTimeBonus += 0.4f;
        }
    }
}