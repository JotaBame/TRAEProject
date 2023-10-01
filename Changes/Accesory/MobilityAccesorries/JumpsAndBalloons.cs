using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using TRAEProject.NewContent.Items.Accesories.ExtraJumps;

namespace TRAEProject.Changes.Accesory
{
    public class JumpsAndBalloons : GlobalItem
    {
        public static void DoubleJumpHorizontalSpeeds(Player Player)
        {
            if (Player.sandStorm)
            {
                Player.moveSpeed *= 0.5f;
            }
            if (Player.GetJumpState<SandstormInABottleJump>().Active)
            {
                Player.moveSpeed *= 1.5f;
            }

            if (Player.GetJumpState<FartInAJarJump>().Active)
            {
                Player.moveSpeed *= (1.5f / 1.75f);
            }
            if (Player.GetJumpState<TsunamiInABottleJump>().Active)

            {
                Player.moveSpeed *= (1.5f / 1.25f);
            }
            if (Player.GetJumpState<CloudInABottleJump>().Active)
            {
                Player.moveSpeed *= 1.5f;
            }

        }
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
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
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
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