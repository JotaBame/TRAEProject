using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using TRAEProject.NewContent.Items.Armor.Joter;
using TRAEProject.Changes.Weapon.Melee.SpearProjectiles;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
//not motivated to finish sickles right now
/*
namespace TRAEProject.Changes.Weapon
{
    class SickleItems : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public int altShoot = -1;
        public bool canGetMeleeModifiers = false;
        public bool isSickle = false;
        public Vector2 sickleSize = new Vector2(52, 44);
        public Vector2 sickleHoldOrigin = new Vector2(11, 33);
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.IceSickle:
				case ItemID.DeathSickle:
                item.noUseGraphic = true;
                isSickle = true;
                item.useStyle = 666;
                break;
			}
            if(item.type == ItemID.DeathSickle)
            {
                sickleSize = new Vector2(60, 56);
                sickleHoldOrigin = new Vector2(13, 43);
            }
        }
        public override void HoldItemFrame(Item item, Player player)
        {
            if(isSickle)
            {
                IdleSickle.PositionArm(player);
            }
            base.HoldItemFrame(item, player);
        }
        public override void UseItemFrame(Item item, Player player)
        {
            if(isSickle)
            {
                if (player.altFunctionUse == 2)
                {
                    IdleSickle.PositionArm(player);

                    player.bodyFrame.Y = player.bodyFrame.Height * 1;

                    Player.CompositeArmStretchAmount stretch = Player.CompositeArmStretchAmount.Quarter;
                    if(((float)player.itemAnimation / player.itemAnimationMax) > 0.3f && ((float)player.itemAnimation / player.itemAnimationMax) < 0.6f )
                    {
                        stretch = Player.CompositeArmStretchAmount.Full;
                    }
                    else if(((float)player.itemAnimation / player.itemAnimationMax) > 0.2f && ((float)player.itemAnimation / player.itemAnimationMax) < 0.8f )
                    {
                        stretch = Player.CompositeArmStretchAmount.ThreeQuarters;
                    }
                    
                    float rotation = player.itemRotation - (((float)player.itemAnimation / player.itemAnimationMax) * MathF.PI / 8f - MathF.PI / 16f) * player.direction;
                    player.SetCompositeArmFront(enabled: true, stretch, rotation);
                }
                else
                {
                    SwingingSickle.PositionArm(player);
                }
            }
        }
        public override bool AltFunctionUse(Item item, Player player)
        {
            if (isSickle)
            {
                return true;
            }
            return base.AltFunctionUse(item, player);
        }
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if(isSickle)
            {
                if (player.altFunctionUse == 2)
                {
                    Vector2 pointPoisition = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: true);
                    player.itemRotation = (Main.MouseWorld - pointPoisition).ToRotation() - MathF.PI / 2f;
                }
                else
                {
                    type = 0;
                }
            }
        }
        
    }
    class IdleSickle : PlayerDrawLayer
    {
        public static void PositionArm(Player player)
        {
            Player.CompositeArmStretchAmount stretchAmount = Player.CompositeArmStretchAmount.Quarter;
            player.SetCompositeArmBack(enabled: true, stretchAmount, (MathF.PI / -2f) * (float)player.direction);
        }
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
            Player drawPlayer = drawInfo.drawPlayer;
            if(drawPlayer.HeldItem.TryGetGlobalItem<SickleItems>(out SickleItems sItem))
            {
                if(sItem.isSickle && (drawPlayer.altFunctionUse == 2 || drawPlayer.itemAnimation == 0))
                {
                    Player.CompositeArmStretchAmount stretchAmount = Player.CompositeArmStretchAmount.Quarter;
                    Vector2 offset = drawPlayer.GetBackHandPosition(stretchAmount, (MathF.PI / -2f)  * (float)drawPlayer.direction) - drawPlayer.MountedCenter;
                    Vector2 drawAt = drawInfo.Position + new Vector2(drawPlayer.width * 0.5f, drawPlayer.height * 0.5f) + offset;
                    float rotation = drawPlayer.bodyRotation + MathF.PI * -7f /4f;
                    if(drawPlayer.direction == -1)
                    {
                        rotation += -MathF.PI/2;
                        if(drawPlayer.gravDir == -1)
                        {
                            rotation += -MathF.PI/2;
                        }
                    }
                    else if(drawPlayer.gravDir == -1)
                    {
                        rotation += MathF.PI/2;
                    }
                    Vector2 origin = new Vector2(drawPlayer.direction * drawPlayer.gravDir == 1 ? sItem.sickleSize.X - sItem.sickleHoldOrigin.X: sItem.sickleHoldOrigin.X, drawPlayer.gravDir == -1 ? sItem.sickleHoldOrigin.Y : sItem.sickleHoldOrigin.Y);
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

                    DrawData drawData = new DrawData(TextureAssets.Item[drawPlayer.HeldItem.type].Value, drawAt - Main.screenPosition, null, drawInfo.colorArmorBody, rotation, origin, 1f, drawPlayer.direction * drawPlayer.gravDir == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                    drawInfo.DrawDataCache.Add(drawData);
                }
            }
        }
    }
    class SwingingSickle : PlayerDrawLayer
    {
        public static void PositionArm(Player player)
        {
            Player.CompositeArmStretchAmount stretchAmount = Player.CompositeArmStretchAmount.Quarter;
            player.SetCompositeArmBack(enabled: true, stretchAmount, (MathF.PI / -2f) * (float)player.direction);
        }
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
            if(drawPlayer.HeldItem.TryGetGlobalItem<SickleItems>(out SickleItems sItem))
            {
                if(sItem.isSickle && (drawPlayer.altFunctionUse != 2 && drawPlayer.itemAnimation > 0))
                {
                    Player.CompositeArmStretchAmount stretchAmount = Player.CompositeArmStretchAmount.Quarter;
                    Vector2 offset = drawPlayer.GetBackHandPosition(stretchAmount, (MathF.PI / -2f)  * (float)drawPlayer.direction) - drawPlayer.MountedCenter;
                    Vector2 drawAt = drawInfo.Position + new Vector2(drawPlayer.width * 0.5f, drawPlayer.height * 0.5f) + offset;
                    float rotation = drawPlayer.bodyRotation + MathF.PI * -7f /4f;
                    if(drawPlayer.direction == -1)
                    {
                        rotation += -MathF.PI/2;
                        if(drawPlayer.gravDir == -1)
                        {
                            rotation += -MathF.PI/2;
                        }
                    }
                    else if(drawPlayer.gravDir == -1)
                    {
                        rotation += MathF.PI/2;
                    }
                    Vector2 origin = new Vector2(drawPlayer.direction * drawPlayer.gravDir == 1 ? sItem.sickleSize.X - sItem.sickleHoldOrigin.X: sItem.sickleHoldOrigin.X, drawPlayer.gravDir == -1 ? sItem.sickleHoldOrigin.Y : sItem.sickleHoldOrigin.Y);
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

                    DrawData drawData = new DrawData(TextureAssets.Item[drawPlayer.HeldItem.type].Value, drawAt - Main.screenPosition, null, drawInfo.colorArmorBody, rotation, origin, 1f, drawPlayer.direction * drawPlayer.gravDir == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                    drawInfo.DrawDataCache.Add(drawData);
                }
            }
        }
    }
}
*/