using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
 
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Accesory
{
    public class JumpsAndBalloons : GlobalItem
    {
        public static void DoubleJumpHorizontalSpeeds(Player Player)
        {

            if (GetInstance<TRAEConfig>().MobilityRework)
            {
                if (Player.GetJumpState(ExtraJump.SandstormInABottle).Active)
                {
                    Player.moveSpeed *= 0.75f;

                }

                if (Player.GetJumpState(ExtraJump.FartInAJar).Active)
                {
                    Player.moveSpeed *= (1.5f / 1.75f);
                }
                if (Player.GetJumpState(ExtraJump.TsunamiInABottle).Active)
                {
                    Player.moveSpeed *= (1.5f / 1.25f);
                }
                if (Player.GetJumpState(ExtraJump.CloudInABottle).Active)
                {
                    Player.moveSpeed *= 1.5f;
                }
            }
        }
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (GetInstance<TRAEConfig>().MobilityRework)
            {
                switch (item.type)
                {
                    case ItemID.LuckyHorseshoe:
                        player.GetModPlayer<AccesoryEffects>().FastFall = true;
                        break;
                    case ItemID.BalloonHorseshoeHoney:
                        player.GetModPlayer<AccesoryEffects>().FastFall = true;
                        break;
                    case ItemID.BundleofBalloons:
                        player.noFallDmg = true;
                        break;
                    case ItemID.BlueHorseshoeBalloon:
                    case ItemID.WhiteHorseshoeBalloon:
                    case ItemID.YellowHorseshoeBalloon:
                    case ItemID.BalloonHorseshoeFart:
                    case ItemID.BalloonHorseshoeSharkron:
                        player.GetModPlayer<AccesoryEffects>().FastFall = true;
                        break;

                }
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (GetInstance<TRAEConfig>().MobilityRework)
            {

                switch (item.type)
                {
                    case ItemID.LuckyHorseshoe:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                            {
                                line.Text += "\nHold DOWN to increase falling speed";
                            }
                        }
                        break;
                    case ItemID.BalloonHorseshoeFart:
                    case ItemID.BalloonHorseshoeSharkron:
                    case ItemID.YellowHorseshoeBalloon:
                    case ItemID.WhiteHorseshoeBalloon:
                    case ItemID.BlueHorseshoeBalloon:
                        foreach (TooltipLine line in tooltips)
                        {
                            if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                            {
                                line.Text += "\nAllows fast fall";
                            }
                        }
                        break;
                }
            }
        }
    }
}