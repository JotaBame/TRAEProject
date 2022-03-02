using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.UI;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.GameContent.UI.ResourceSets;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TRAEProject.Changes
{
    public class Mana : ModPlayer
    {
        public float newManaRegen = 0;
        public float manaRegenBoost = 1;
        public int manaFlowerTimer = 0;
        public int manaFlowerLimit = 0;
        int maxManaOverride = 0;
        int manaOver400 = 0;
        public int overloadedMana = 0;
        public bool celestialCuffsOverload;
        public bool newManaFlower = false;
        public bool manaCloak = false;
        public override void ResetEffects()
        {
            manaRegenBoost = 1;
            celestialCuffsOverload = false;
            newManaFlower = false; 
            manaCloak = false;
        }
        public override void UpdateDead()
        {
            manaRegenBoost = 1;
            newManaFlower = false;
            manaCloak = false;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {

            if (proj.CountsAsClass(DamageClass.Magic) && manaCloak == true && crit && Main.rand.Next(3) == 0)
            {
                int[] spread = { 3, 4, 5 };
                TRAEMethods.SpawnProjectilesFromAbove(Player, target.position, 1, 400, 600, spread, 20, ProjectileID.ManaCloakStar, damage / 3, 2f, Player.whoAmI);
            }
            if (proj.CountsAsClass(DamageClass.Magic) && newManaFlower == true && crit && manaFlowerLimit < 3)
            {
                if (Main.rand.Next(3) == 0)
                {
                    Item.NewItem(Player.GetItemSource_OnHit(target, Player.HeldItem.type), target.getRect(), ItemID.Star, 1);
                    ++manaFlowerLimit;
                }
            }
        }
        public override void PostUpdateEquips()
        {
            //Main.NewText("PUE: " + Player.statManaMax2);
            maxManaOverride = Player.statManaMax2;
            if(Player.statMana > 400)
            {
                manaOver400 = Player.statMana - 400;
            }
            else
            {
                manaOver400 = 0;
            }
        }
        public override void PostUpdate()
        {
            Player.manaRegenCount = 0;
            Player.manaRegen = 0;
            Player.manaRegenDelay = 999;
            Player.manaSickTimeMax = 9999;
            int reachThisNumberAndThenIncreaseManaBy1 = 60;
            if (Player.statMana < Player.statManaMax2)
            {
                newManaRegen += Player.statManaMax2 * 0.1f * manaRegenBoost;
                if (newManaRegen >= reachThisNumberAndThenIncreaseManaBy1)
                {
                    newManaRegen -= 60;
                    ++Player.statMana;
                }
            }
            if (newManaFlower)
            {
                ++manaFlowerTimer;
                if (manaFlowerTimer >= 60)
                {
                    manaFlowerTimer = 0;
                    manaFlowerLimit = 0;
                }
            }

            //Main.NewText("PU: " + Player.statManaMax2);
            Player.statManaMax2 = maxManaOverride;
            Player.statMana += manaOver400;
            if(overloadedMana > Player.statManaMax2 * 2)
            {
                overloadedMana = Player.statManaMax2 * 2;
            }
        }
        int counter = 0;
        public override bool PreItemCheck()
        {
            if(overloadedMana > 0)
            {
               // Main.NewText("Overloaded Mana:" + overloadedMana);
            }
            return base.PreItemCheck();
        }
        public void GiveManaOverloadable(int amount)
        {
            Player.statMana += amount;
            int bonusMana = 0;
            if(Player.statMana > Player.statManaMax2)
            {
                bonusMana += Player.statMana - Player.statManaMax2;
                Player.statMana = Player.statManaMax2;
            }
            if(bonusMana > 0)
            {
                overloadedMana += bonusMana;
                if(bonusMana != amount)
                {
                    Player.ManaEffect(amount - bonusMana);
                }
                CombatText.NewText(new Rectangle((int)Player.position.X, (int)Player.position.Y, Player.width, Player.height), Color.Orange, bonusMana);
            }
            else
            {
                Player.ManaEffect(amount);
            }

        }
        public override void SetStaticDefaults()
        {
            IL.Terraria.Player.ItemCheck_PayMana += PayManaHook2; //this method is used by most magic weapons
            IL.Terraria.Player.CheckMana_Item_int_bool_bool += CheckManaHook2; //this is used by held projectiles like LMG
        }
        private void PayManaHook2(ILContext il)
        {
            var c = new ILCursor(il);
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldarg_1);
            c.Emit(OpCodes.Ldarg_2);
            c.EmitDelegate<Action<Player, Item, bool>>((player, item, canUse) =>
            {
                if (player.GetModPlayer<Mana>().overloadedMana > 0)
                {

                    int amount = player.GetManaCost(item);
                    int overloadedManaLoss = Math.Min(player.GetModPlayer<Mana>().overloadedMana, amount);
                    player.GetModPlayer<Mana>().overloadedMana -= overloadedManaLoss;
                    //amount -= overloadedManaLoss;
                    player.statMana += overloadedManaLoss;
                }
            });
        }
        private void CheckManaHook2(ILContext il)
        {

            var c = new ILCursor(il);
            c.Emit(OpCodes.Ldarg_0);
            c.Emit(OpCodes.Ldarg_1);
            c.Emit(OpCodes.Ldarg_2);
            c.Emit(OpCodes.Ldarg_3);
            c.Emit(OpCodes.Ldarg, 4);
            c.EmitDelegate<Action<Player, Item, int, bool, bool>>((player, item, amount, pay, blockQuickMana) =>
            {
                if (player.GetModPlayer<Mana>().overloadedMana > 0)
                {
                    if (amount <= -1)
                    {
                        amount = player.GetManaCost(item);
                    }

                    int overloadedManaLoss = Math.Min(player.GetModPlayer<Mana>().overloadedMana, amount);
                    player.GetModPlayer<Mana>().overloadedMana -= overloadedManaLoss;
                    //amount -= overloadedManaLoss;
                    player.statMana += overloadedManaLoss;
                }
            });

        }

    }
    public class DisplayOverload : ModSystem
    {
        const int OManaPerStar = 40;
        public static IPlayerResourcesDisplaySet ActivePlayerResourcesSet;
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {

            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            LegacyGameInterfaceLayer overloadManaDraw = new LegacyGameInterfaceLayer("OverloadedMana",
                    delegate
                    {
                        if(ActivePlayerResourcesSet is ClassicPlayerResourcesDisplaySet)
                        {
                            DrawClassicMana();
                        }
                        if (ActivePlayerResourcesSet is FancyClassicPlayerResourcesDisplaySet)
                        {
                            DrawFancyClassicMana();
                        }
                        if(ActivePlayerResourcesSet is HorizontalBarsPlayerReosurcesDisplaySet)
                        {
                            DrawManaBar();
                        }
                        return true;
                    },
                    InterfaceScaleType.UI);
            layers.Insert(index+1, overloadManaDraw);
        }
        void DrawClassicMana()
        {
            Player localPlayer = Main.LocalPlayer;
            SpriteBatch spriteBatch = Main.spriteBatch;
            Color color = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
            int UIDisplay_ManaPerStar = OManaPerStar;
            int UI_ScreenAnchorX = Main.screenWidth - 800;
            if (localPlayer.ghost || localPlayer.statManaMax2 <= 0)
            {
                return;
            }
            for (int i = 1; i < (localPlayer.statManaMax2 * 2) / UIDisplay_ManaPerStar + 1; i++)
            {
                int num2 = 255;
                bool flag = false;
                float num3 = 1f;
                if (localPlayer.GetModPlayer<Mana>().overloadedMana >= i * UIDisplay_ManaPerStar)
                {
                    num2 = 255;
                    if (localPlayer.GetModPlayer<Mana>().overloadedMana == i * UIDisplay_ManaPerStar)
                    {
                        flag = true;
                    }
                }
                else
                {
                    float num4 = (float)(localPlayer.GetModPlayer<Mana>().overloadedMana - (i - 1) * UIDisplay_ManaPerStar) / (float)UIDisplay_ManaPerStar;
                    num2 = (int)(30f + 225f * num4);
                    if (num2 < 30)
                    {
                        num2 = 0;
                    }
                    num3 = num4 / 4f + 0.75f;
                    if ((double)num3 < 0.75)
                    {
                        num3 = 0.75f;
                    }
                    if (num4 > 0f)
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    num3 += Main.cursorScale - 1f;
                }
                int a = (int)((double)num2 * 0.9);
                spriteBatch.Draw(Request<Texture2D>("TRAEProject/Changes/OMana").Value, new Vector2(775 + UI_ScreenAnchorX, (float)(30 + TextureAssets.Mana.Height() / 2) + ((float)TextureAssets.Mana.Height() - (float)TextureAssets.Mana.Height() * num3) / 2f + (float)(28 * (i - 1))), new Rectangle(0, 0, TextureAssets.Mana.Width(), TextureAssets.Mana.Height()), new Color(num2, num2, num2, a), 0f, new Vector2(TextureAssets.Mana.Width() / 2, TextureAssets.Mana.Height() / 2), num3, SpriteEffects.None, 0f);
            }
        }
        void DrawFancyClassicMana()
        {
            Player localPlayer = Main.LocalPlayer;
            int manaPerStar = OManaPerStar;
            int _starCount = (int)((float)(localPlayer.statManaMax2*2) / manaPerStar);


            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 vector = new Vector2(Main.screenWidth - 40, 22f);
            bool isHovered = false;
            ResourceDrawSettings resourceDrawSettings = default(ResourceDrawSettings);
            resourceDrawSettings.ElementCount = _starCount;
            resourceDrawSettings.ElementIndexOffset = 0;
            resourceDrawSettings.TopLeftAnchor = vector + new Vector2(15f, 16f);
            resourceDrawSettings.GetTextureMethod = StarFillingDrawer;
            resourceDrawSettings.OffsetPerDraw = Vector2.UnitY * -2f;
            resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitY;
            resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
            resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = new Vector2(0.5f, 0.5f);
            resourceDrawSettings.Draw(spriteBatch, ref isHovered);
        }
        void StarFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
        {

            Player localPlayer = Main.LocalPlayer;
            int _lastStarFillingIndex = (int)(localPlayer.GetModPlayer<Mana>().overloadedMana / 40f);
            sourceRect = null;
            offset = Vector2.Zero;
            sprite = Request<Texture2D>("TRAEProject/Changes/OManaFancy");
            float num = (drawScale = Utils.GetLerpValue(OManaPerStar * (float)elementIndex, OManaPerStar * (float)(elementIndex + 1), localPlayer.GetModPlayer<Mana>().overloadedMana, clamped: true));
            if (elementIndex == _lastStarFillingIndex && num > 0f)
            {
                drawScale += Main.cursorScale - 1f;
            }
        }
        void DrawManaBar()
        {
            
            int num = 16;
            int num2 = 18;
            int num3 = Main.screenWidth - 300 - 22 + num;
            Player localPlayer = Main.LocalPlayer;
            int manaPerStar = OManaPerStar;
            int _mpSegmentsCount = (int)((float)(localPlayer.statManaMax2 * 2) / manaPerStar);
            Vector2 vector2 = new Vector2(num3 - 10, num2 + 24);
            vector2.X += (400 - _mpSegmentsCount) * 12;
            SpriteBatch spriteBatch = Main.spriteBatch;
            bool isHovered = false;

           

            ResourceDrawSettings resourceDrawSettings = default(ResourceDrawSettings);
            resourceDrawSettings.ElementCount = _mpSegmentsCount;
            resourceDrawSettings.ElementIndexOffset = 0;
            resourceDrawSettings.TopLeftAnchor = vector2 + new Vector2(6f, 6f);
            resourceDrawSettings.GetTextureMethod = ManaFillingDrawer;
            resourceDrawSettings.OffsetPerDraw = new Vector2(12, 0f);
            resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.Zero;
            resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
            resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
            resourceDrawSettings.Draw(spriteBatch, ref isHovered);
        }
        private static void FillBarByValues(int elementIndex, Asset<Texture2D> sprite, int segmentsCount, float fillPercent, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
        {
            sourceRect = null;
            offset = Vector2.Zero;
            float num = 1f;
            float num2 = 1f / (float)segmentsCount;
            float t = 1f - fillPercent;
            float lerpValue = Utils.GetLerpValue(num2 * (float)elementIndex, num2 * (float)(elementIndex + 1), t, clamped: true);
            num = 1f - lerpValue;
            drawScale = 1f;
            Rectangle value = sprite.Frame();
            int num3 = (int)((float)value.Width * (1f - num));
            offset.X += num3;
            value.X += num3;
            value.Width -= num3;
            sourceRect = value;
        }

        private void ManaFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
        {

            
            Player localPlayer = Main.LocalPlayer;
            int manaPerStar = OManaPerStar;
            int _mpSegmentsCount = (int)((float)(localPlayer.statManaMax2 * 2) / manaPerStar);
            float _mpPercent = (float)localPlayer.GetModPlayer<Mana>().overloadedMana / (float)(localPlayer.statManaMax2 * 2);
            sprite = Request<Texture2D>("TRAEProject/Changes/OMPFill");
            FillBarByValues(elementIndex, sprite, _mpSegmentsCount, _mpPercent, out offset, out drawScale, out sourceRect);
        }
    }
}
