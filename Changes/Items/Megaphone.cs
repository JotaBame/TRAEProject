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
    public class Megaphone : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Megaphone)
            {
                item.accessory = false;
                item.useTime = item.useAnimation = 40;
                item.useStyle = 999;
                item.noUseGraphic = true;
            }
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (item.type == ItemID.Megaphone)
            {
                SoundEngine.PlaySound(SoundID.Roar with { MaxInstances = 0 }, player.position);
                return true;
            }
            return base.CanUseItem(item, player);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.Megaphone:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "Use it to scream";
                        }
                    }
return;
            }
        }
}
    public class HoldingMegaphone : ModPlayer
    {
        public override void PostItemCheck()
        {
            if (Player.HeldItem.type == ItemID.Megaphone && Player.itemAnimation > 0)
            {
                float turnArmAmt = MathF.PI * -2f / 5f;
                Player.SetCompositeArmBack(enabled: true, Player.CompositeArmStretchAmount.Full, turnArmAmt * (float)Player.direction);

                Player.bodyFrame.Y = 0;
                if(Player.velocity.Y != 0)
                {
                    if (Player.sliding)
                    {
                        Player.bodyFrame.Y = Player.bodyFrame.Height * 3;
                    }
                    else if (Player.sandStorm || Player.carpetFrame >= 0)
                    {
                        Player.bodyFrame.Y = Player.bodyFrame.Height * 6;
                    }
                    else if (Player.eocDash > 0)
                    {
                        Player.bodyFrame.Y = Player.bodyFrame.Height * 6;
                    }
                    else if (Player.wings > 0)
                    {
                        if (Player.wings == 22 || Player.wings == 28 || Player.wings == 45)
                        {
                            Player.bodyFrame.Y = 0;
                        }
                        else if (Player.velocity.Y > 0f)
                        {
                            if (Player.controlJump)
                            {
                                Player.bodyFrame.Y = Player.bodyFrame.Height * 6;
                            }
                            else
                            {
                                Player.bodyFrame.Y = Player.bodyFrame.Height * 5;
                            }
                        }
                        else
                        {
                            Player.bodyFrame.Y = Player.bodyFrame.Height * 6;
                        }
                    }
                    else
                    {
                        Player.bodyFrame.Y = Player.bodyFrame.Height * 5;
                    }
                    Player.bodyFrameCounter = 0.0;
                }
                else if (Player.velocity.X != 0f)
                {
                    if (Player.legs == 140)
                    {
                        Player.bodyFrameCounter += Math.Abs(Player.velocity.X) * 0.5f;
                        while (Player.bodyFrameCounter > 8.0)
                        {
                            Player.bodyFrameCounter -= 8.0;
                            Player.bodyFrame.Y += Player.bodyFrame.Height;
                        }
                        if (Player.bodyFrame.Y < Player.bodyFrame.Height * 7)
                        {
                            Player.bodyFrame.Y = Player.bodyFrame.Height * 19;
                        }
                        else if (Player.bodyFrame.Y > Player.bodyFrame.Height * 19)
                        {
                            Player.bodyFrame.Y = Player.bodyFrame.Height * 7;
                        }
                    }
                    else
                    {
                        Player.bodyFrameCounter += (double)Math.Abs(Player.velocity.X) * 1.5;
                        Player.bodyFrame.Y = Player.legFrame.Y;
                    }
                }
            }
        }
    }
    public class DrawMegaphone : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.HeldItem);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            if (drawPlayer.HeldItem.type == ItemID.Megaphone && drawPlayer.itemAnimation > 0)
            {

                Texture2D texture = TextureAssets.Item[ItemID.Megaphone].Value;

                float turnArmAmt = MathF.PI * -2f / 5f;
                Player.CompositeArmStretchAmount stretchAmount = Player.CompositeArmStretchAmount.Full;
                float rotExtra = -MathF.PI / 4f;
                if(drawPlayer.gravDir == -1)
                {
                    rotExtra += MathF.PI / 2 ;
                }

                Vector2 offset = drawPlayer.GetBackHandPosition(stretchAmount, turnArmAmt * (float)drawPlayer.direction) - drawPlayer.MountedCenter;
                drawPlayer.SetCompositeArmBack(enabled: true, stretchAmount, turnArmAmt * (float)drawPlayer.direction);
                Vector2 drawAt = drawInfo.Position + new Vector2(drawPlayer.width * 0.5f, drawPlayer.height * 0.5f) + offset;
                float rotation = drawPlayer.bodyRotation + drawPlayer.direction * rotExtra;

                Vector2 origin = new Vector2(drawPlayer.direction == -1 ? 28-3: 3, drawPlayer.gravDir == -1 ? 24 - 17 : 17);
                int fHeight = 56;
                if (drawPlayer.bodyFrame.Y == 7 * fHeight || drawPlayer.bodyFrame.Y == 8 * fHeight || drawPlayer.bodyFrame.Y == 9 * fHeight || drawPlayer.bodyFrame.Y == 14 * fHeight || drawPlayer.bodyFrame.Y == 15 * fHeight || drawPlayer.bodyFrame.Y == 16 * fHeight)
                {
                    if (drawPlayer.gravDir == -1)
                    {
                        drawAt.Y += 2;
                    }
                    else
                    {
                        drawAt.Y -= 2;
                    }
                }

                DrawData drawData = new DrawData(texture, drawAt - Main.screenPosition, null, drawInfo.colorArmorBody, rotation, origin, 1f, drawInfo.playerEffect, 0);
                drawInfo.DrawDataCache.Add(drawData);

            }
        }

    }
}
