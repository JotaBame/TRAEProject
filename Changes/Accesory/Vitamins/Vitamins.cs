using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NewContent.Accesory.Vitamins
{
    class Vitamins : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if(item.type == ItemID.Vitamins || item.type == ItemID.ArmorBracing)
            {
                item.vanity = true;
                item.canBePlacedInVanityRegardlessOfConditions = true;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (item.type == ItemID.Vitamins)
                {

                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = "Who needs a workout?";
                    }
                }
                if (item.type == ItemID.ArmorBracing)
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = "Gives you shiny armor and great arms";
                    }

                }
            }
        }
    }
    class DrawBuffArms : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Torso);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;

            bool buffArms = false;
            for (int i = 3; i < 10; i++)
            {
                if ((!drawPlayer.hideVisibleAccessory[i]) && (drawPlayer.armor[i].type == ItemID.Vitamins || drawPlayer.armor[i].type == ItemID.ArmorBracing))
                {
                    buffArms = true;
                }
            }
            for (int i = 13; i < 20; i++)
            {
                if (drawPlayer.armor[i].type == ItemID.Vitamins || drawPlayer.armor[i].type == ItemID.ArmorBracing)
                {
                    buffArms = true;
                }
            }
            if (buffArms)
            {
                Color color12 = drawInfo.colorBodySkin;
                Texture2D texture = Request<Texture2D>("TRAEProject/Changes/Accesory/Vitamins/BuffArms").Value;
                Vector2 vector = new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.bodyPosition + new Vector2(drawInfo.drawPlayer.bodyFrame.Width / 2, drawInfo.drawPlayer.bodyFrame.Height / 2);
                Vector2 value = Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height];
                value.Y -= 2f;
                vector += value * -drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
                float bodyRotation = drawInfo.drawPlayer.bodyRotation;
                Vector2 value2 = vector;
                Vector2 bodyVect = drawInfo.bodyVect;
                Vector2 compositeOffset_BackArm = GetCompositeOffset_BackArm(ref drawInfo);
                _ = value2 + compositeOffset_BackArm;
                bodyVect += compositeOffset_BackArm;
                DrawData drawData;
                if (!drawInfo.drawPlayer.invis || IsArmorDrawnWhenInvisible(drawInfo.drawPlayer.body))
                {
                    Rectangle useFrame = drawInfo.compTorsoFrame;
                    drawData = new DrawData(texture, vector, useFrame, color12, bodyRotation, drawInfo.bodyVect, 1f, drawInfo.playerEffect, 0);
                    DrawCompositeArmorPiece(ref drawInfo, CompositePlayerDrawContext.Torso, drawData);
                }
            }
        }

        public static Vector2 GetCompositeOffset_BackArm(ref PlayerDrawSet drawInfo)
        {
            return new Vector2(6 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 2 * ((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1)));
        }
        public static bool IsArmorDrawnWhenInvisible(int torsoID)
        {
            if ((uint)(torsoID - 21) <= 1u)
            {
                return false;
            }
            return true;
        }
        public static void DrawCompositeArmorPiece(ref PlayerDrawSet drawInfo, CompositePlayerDrawContext context, DrawData data)
        {
            drawInfo.DrawDataCache.Add(data);
            switch (context)
            {
                case CompositePlayerDrawContext.BackShoulder:
                case CompositePlayerDrawContext.BackArm:
                case CompositePlayerDrawContext.FrontArm:
                case CompositePlayerDrawContext.FrontShoulder:
                    {
                        if (drawInfo.armGlowColor.PackedValue == 0)
                        {
                            break;
                        }
                        DrawData item2 = data;
                        item2.color = drawInfo.armGlowColor;
                        Rectangle value3 = item2.sourceRect.Value;
                        value3.Y += 224;
                        item2.sourceRect = value3;
                        drawInfo.DrawDataCache.Add(item2);
                        break;
                    }
                case CompositePlayerDrawContext.Torso:
                    {
                        if (drawInfo.bodyGlowColor.PackedValue == 0)
                        {
                            break;
                        }
                        DrawData item = data;
                        item.color = drawInfo.bodyGlowColor;
                        Rectangle value = item.sourceRect.Value;
                        value.Y += 224;
                        item.sourceRect = value;
                        drawInfo.DrawDataCache.Add(item);
                        break;
                    }
            }
        }


    }
    class ArmDrawExtra : PlayerDrawLayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return true;
        }
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.ArmOverItem);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;

            bool buffArms = false;
            for (int i = 3; i < 10; i++)
            {
                if ((!drawPlayer.hideVisibleAccessory[i]) && (drawPlayer.armor[i].type == ItemID.Vitamins))
                {
                    buffArms = true;
                }
            }
            for (int i = 13; i < 20; i++)
            {
                if (drawPlayer.armor[i].type == ItemID.Vitamins)
                {
                    buffArms = true;
                }
            }
            if (buffArms)
            {
                Color color12 = drawInfo.colorBodySkin;
                Texture2D texture = Request<Texture2D>("TRAEProject/Changes/Accesory/Vitamins/BuffArms").Value;

                Vector2 vector = new Vector2((int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawInfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawInfo.drawPlayer.width / 2)), (int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawInfo.drawPlayer.height - (float)drawInfo.drawPlayer.bodyFrame.Height + 4f)) + drawInfo.drawPlayer.bodyPosition + new Vector2(drawInfo.drawPlayer.bodyFrame.Width / 2, drawInfo.drawPlayer.bodyFrame.Height / 2);
                Vector2 value = Main.OffsetsPlayerHeadgear[drawInfo.drawPlayer.bodyFrame.Y / drawInfo.drawPlayer.bodyFrame.Height];
                value.Y -= 2f;
                vector += value * -drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
                float bodyRotation = drawInfo.drawPlayer.bodyRotation;
                float rotation = drawInfo.drawPlayer.bodyRotation + drawInfo.compositeFrontArmRotation;
                Vector2 bodyVect = drawInfo.bodyVect;
                Vector2 compositeOffset_FrontArm = GetCompositeOffset_FrontArm(ref drawInfo);
                bodyVect += compositeOffset_FrontArm;
                vector += compositeOffset_FrontArm;
                Vector2 position = vector + drawInfo.frontShoulderOffset;
                if (drawInfo.compFrontArmFrame.X / drawInfo.compFrontArmFrame.Width >= 7)
                {
                    vector += new Vector2((!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1), (!drawInfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1));
                }
                _ = drawInfo.drawPlayer.invis;
                int num2 = (drawInfo.compShoulderOverFrontArm ? 1 : 0);
                int num3 = ((!drawInfo.compShoulderOverFrontArm) ? 1 : 0);
                int num4 = ((!drawInfo.compShoulderOverFrontArm) ? 1 : 0);
                bool flag = !drawInfo.hidesTopSkin;
                DrawData drawData;

                if (!drawInfo.drawPlayer.invis || DrawBuffArms.IsArmorDrawnWhenInvisible(drawInfo.drawPlayer.body))
                {

                    for (int i = 0; i < 2; i++)
                    {
                        if (i == num2 && !drawInfo.hideCompositeShoulders)
                        {
                            drawData = new DrawData(texture, position, drawInfo.compFrontShoulderFrame, color12, bodyRotation, bodyVect, 1f, drawInfo.playerEffect, 0);
                            DrawBuffArms.DrawCompositeArmorPiece(ref drawInfo, CompositePlayerDrawContext.FrontShoulder, drawData);
                        }
                        if (i == num3)
                        {
                            drawData = new DrawData(texture, vector, drawInfo.compFrontArmFrame, color12, rotation, bodyVect, 1f, drawInfo.playerEffect, 0);
                            DrawBuffArms.DrawCompositeArmorPiece(ref drawInfo, CompositePlayerDrawContext.FrontArm, drawData);
                        }
                    }
                }
            }

        }
        private static Vector2 GetCompositeOffset_FrontArm(ref PlayerDrawSet drawinfo)
        {
            return new Vector2(-5 * ((!drawinfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 0f);
        }
    }
}
