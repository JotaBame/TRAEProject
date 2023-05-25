using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject
{
    public class DrawUmbrella : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.BackAcc);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            DU(ref drawInfo, false);
        }
        public static void DU(ref PlayerDrawSet drawInfo, bool top)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            int useShader = -1;
            if (drawPlayer.HeldItem.type != ItemID.FairyQueenMagicItem)
            {
                Item umbrella = null;
                for (int i = 3; i < 10; i++)
                {
                    if (!drawPlayer.hideVisibleAccessory[i] && (drawPlayer.armor[i].type == ItemID.Umbrella || drawPlayer.armor[i].type == ItemID.TragicUmbrella))
                    {
                        umbrella = drawPlayer.armor[i];
                        useShader = i;
                    }
                }
                for (int i = 13; i < 20; i++)
                {
                    if (drawPlayer.armor[i].type == ItemID.Umbrella || drawPlayer.armor[i].type == ItemID.TragicUmbrella)
                    {
                        umbrella = drawPlayer.armor[i];
                        useShader = i - 10;
                    }
                }
                if (umbrella != null)
                {
                    Texture2D texture;
                    if (umbrella.type == ItemID.Umbrella)
                    {
                        if (top)
                        {
                            texture = Request<Texture2D>("TRAEProject/Changes/Accesory/Umbrella/UmbrellaTop").Value;
                        }
                        else
                        {
                            texture = Request<Texture2D>("TRAEProject/Changes/Accesory/Umbrella/UmbrellaHandle").Value;
                        }
                    }
                    else
                    {
                        if (top)
                        {
                            texture = Request<Texture2D>("TRAEProject/Changes/Accesory/Umbrella/TragicTop").Value;
                        }
                        else
                        {
                            texture = Request<Texture2D>("TRAEProject/Changes/Accesory/Umbrella/TragicHandle").Value;
                        }
                    }
                    bool holdingUp = true;
                    if (!drawPlayer.slowFall || drawPlayer.controlDown || drawPlayer.velocity.Y <= 0)
                    {
                        holdingUp = false;
                    }
                    float turnArmAmt = MathF.PI * -3f / 5f;
                    if(!holdingUp)
                    {
                        turnArmAmt = MathF.PI * -1f / 5f;
                    }
                    if (!top)
                    {
                        drawPlayer.SetCompositeArmBack(enabled: true, Player.CompositeArmStretchAmount.ThreeQuarters, turnArmAmt * (float)drawPlayer.direction);
                    }
                    Vector2 offset = new Vector2(14 * drawPlayer.direction, drawPlayer.gravDir == -1  ? 6 : -6);
                    if(!holdingUp)
                    {
                        offset = new Vector2(8 * drawPlayer.direction, 0);
                    }
                    Vector2 drawAt = drawInfo.Position + new Vector2(drawPlayer.width * 0.5f, drawPlayer.height * 0.5f) + offset;
                    float rotation = drawPlayer.bodyRotation;
                    if(!holdingUp)
                    {
                        rotation += MathF.PI * 0.2f * drawPlayer.direction * -1 * drawPlayer.gravDir;
                    }

                    Vector2 origin = new Vector2(texture.Width * 0.5f, drawPlayer.gravDir == -1 ? 10 : texture.Height-10);
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

                    DrawData drawData = new DrawData(texture, drawAt - Main.screenPosition, null, drawInfo.colorArmorBody, rotation, origin, 1f, drawInfo.playerEffect, 0)
                    {
                        shader = drawPlayer.dye[useShader].dye
                    };
                    drawInfo.DrawDataCache.Add(drawData);
                }
            }
        }

    }
    public class DrawUmbrellaTop : PlayerDrawLayer
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
            DrawUmbrella.DU(ref drawInfo, true);
        }
    }
}
