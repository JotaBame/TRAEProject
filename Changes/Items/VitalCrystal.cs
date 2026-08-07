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
    public class VitalCrystal : GlobalItem
    {

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.AegisCrystal:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Permanently lowers respawn time\nPermanent minor increase to life regeneration";
                        }
                    }
                    return;
            }
        }
    }
    public class VitalCrystalPlayer : ModPlayer
    {
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (Player.usedAegisCrystal)
            {
            
                bool foundBoss = false;
                if (Main.netMode != 1)
                {
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC n = Main.npc[i];
                        if (n.active && n.boss)
                        {
                            foundBoss = true;
                            break;
                        }
                    }
                }

 
                if (!foundBoss)
                    Player.respawnTimer = Main.expertMode ? 10 * 60 : 8 * 60;
            }
        }

    }
}
