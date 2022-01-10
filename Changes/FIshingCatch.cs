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
using Microsoft.Xna.Framework;

namespace TRAEProject.Changes
{
    class FIshingCatch : ModPlayer
    {
		private void Fishing_GetBait(out int baitPower, out int baitType, out int baitIndex)
		{
			baitPower = 0;
			baitType = 0;
			baitIndex = 0;
			for (int num = 54; num < 58; num++)
			{
				if (Player.inventory[num].stack > 0 && Player.inventory[num].bait > 0)
				{
					baitPower = Player.inventory[num].bait;
					baitType = Player.inventory[num].type;
					baitIndex = num;
					break;
				}
			}
			if (baitPower != 0 || baitType != 0)
			{
				return;
			}
			for (int num2 = 0; num2 < 50; num2++)
			{
				if (Player.inventory[num2].stack > 0 && Player.inventory[num2].bait > 0)
				{
					baitPower = Player.inventory[num2].bait;
					baitType = Player.inventory[num2].type;
					baitIndex = num2;
					break;
				}
			}
		}

		public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            if(Main.bloodMoon)
            {
				Fishing_GetBait(out _, out int baitID, out int baitIndex);
				if(baitID == ItemType<DreadSummon>())
                {
					npcSpawn = NPCID.BloodNautilus;
				}
				Player.inventory[baitIndex].stack--;
            }
            if (!Main.hardMode && (itemDrop == 4819 || itemDrop == 4820 || itemDrop == 4872))
            {
                if (Main.rand.Next(4) == 0)
                {
                    itemDrop = ItemID.ObsidianSwordfish;
                }
            }
        }
    }
}
