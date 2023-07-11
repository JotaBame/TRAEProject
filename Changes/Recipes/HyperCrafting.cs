using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Recipes
{
    public class HyperCrafting : ModPlayer
    {
        public override bool PreItemCheck()
        {

            return base.PreItemCheck();
        }
        public override bool HoverSlot(Item[] inventory, int context, int slot)
        {
            return base.HoverSlot(inventory, context, slot);
        }
    }
}
