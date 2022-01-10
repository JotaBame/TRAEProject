using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.DataStructures;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Items.DreadItems.DreadSummon;

namespace TRAEProject.Changes
{
    class FIshingCatch : ModPlayer
    {

        public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType)
        {
            if(!Main.hardMode && (caughtType == 4819 || caughtType == 4820 || caughtType == 4872))
            {
                if(Main.rand.Next(4)==0)
                {
                    caughtType = ItemID.ObsidianSwordfish;
                }
            }
        }
    }
}
