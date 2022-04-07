using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.Items
{
    class TrifoldMap : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.TrifoldMap)
            {
                item.accessory = false;
                item.useTime = item.useAnimation = 40;
                item.useStyle = 4;
                item.consumable = true;
                item.value = 100 * 100 * 100 * 5;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (item.type == ItemID.TrifoldMap)
                {

                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = "Reveals the entire map";
                    }

                }
            }
        }


        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ItemID.TrifoldMap)
            {
                for (int i = 0; i < Main.maxTilesX; i++)
                {
                    for (int j = 0; j < Main.maxTilesY; j++)
                    {
                        if (WorldGen.InWorld(i, j))
                        {
                            if(!Main.Map.IsRevealed(i, j))
                            {
                                Main.Map.Update(i, j, 255);
                            }
                        }
                    }
                }
                Main.refreshMap = true;
                return true;
            }
            return base.CanUseItem(item, player);
        }
    }
}
