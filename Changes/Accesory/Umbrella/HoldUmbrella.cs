
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Golf;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject
{
    class HoldUmbrella : ModPlayer
    {
        public override void PostItemCheck()
        {
            if (Player.HeldItem.type != ItemID.FairyQueenMagicItem && !GolfHelper.IsPlayerHoldingClub(Player) && Player.HeldItem.holdStyle != 5)
            {
                Item umbrella = null;
                for (int i = 3; i < 10; i++)
                {
                    if (!Player.hideVisibleAccessory[i] && (Player.armor[i].type == ItemID.Umbrella || Player.armor[i].type == ItemID.TragicUmbrella))
                    {
                        umbrella = Player.armor[i];
                    }
                }
                for (int i = 13; i < 20; i++)
                {
                    if (Player.armor[i].type == ItemID.Umbrella || Player.armor[i].type == ItemID.TragicUmbrella)
                    {
                        umbrella = Player.armor[i];
                    }
                }
                if (umbrella != null)
                {
                    bool holdingUp = true;
                    if (!Player.slowFall || Player.controlDown || Player.velocity.Y <= 0)
                    {
                        holdingUp = false;
                    }
                    float turnArmAmt = MathF.PI * -3f / 5f;
                    if (!holdingUp)
                    {
                        turnArmAmt = MathF.PI * -1f / 5f;
                    }
                    Player.SetCompositeArmBack(enabled: true, Player.CompositeArmStretchAmount.ThreeQuarters, turnArmAmt * (float)Player.direction);
                }
            }
        }
    }
}
