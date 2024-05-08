
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.Items
{
    public class DrillItems : GlobalItem
    {
	    public override bool InstancePerEntity => true;       
        public int drillSpeed = -1;
        public override void SetDefaults(Item item)
        {
            switch(item.type)
            {
                case ItemID.CobaltDrill:
                    item.useTime = drillSpeed = 11;
                    item.tileBoost = -1;
                    break;
                case ItemID.PalladiumDrill:
                    item.useTime = drillSpeed = 10; item.tileBoost = -1;

                    break;
                case ItemID.MythrilDrill:
                    item.useTime = drillSpeed = 9; item.tileBoost = -1;

                    break;
                case ItemID.OrichalcumDrill:
                    item.useTime = drillSpeed = 9;
                    item.tileBoost = -1;
                    break;
                case ItemID.AdamantiteDrill:
                    item.useTime = drillSpeed = 8;
                    item.tileBoost = 0;

                    break;
                case ItemID.TitaniumDrill:
                    item.useTime = drillSpeed = 8;
                    item.tileBoost = 0;

                    break;
                case ItemID.Drax:
                    item.tileBoost = 0;

                    item.useTime = drillSpeed = 7;
                    break;
                case ItemID.ChlorophyteDrill:
                    item.useTime = drillSpeed = 6;
                    break;
            }
        }
        public override void HoldItem(Item item, Player player)
        {
            
            if(drillSpeed > 0)
            {
                item.useTime = (int)((float)drillSpeed * (player.pickSpeed)) - 1;
            }
            
        }
    }
}
