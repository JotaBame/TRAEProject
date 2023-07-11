using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < Main.maxTilesX; i++)
                    {
                        for (int j = 0; j < Main.maxTilesY; j++)
                        {
                            if (WorldGen.InWorld(i, j))
                            {
                                Main.Map.Update(i, j, 255);
                            }
                        }
                    }
                }
                else
                {
                    Point center = Main.LocalPlayer.Center.ToTileCoordinates();
                    int range = 300;
                    for (int i = center.X - range / 2; i < center.X + range / 2; i++)
                    {
                        for (int j = center.Y - range / 2; j < center.Y + range / 2; j++)
                        {
                            if (WorldGen.InWorld(i, j))
                            {
                                Main.Map.Update(i, j, 255);
                            }
                        }
                    }

                }
                Main.refreshMap = true;

                item.stack -= 1;
            }
            return true;
        }
    }
}
