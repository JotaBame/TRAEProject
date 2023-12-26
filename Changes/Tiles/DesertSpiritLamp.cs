using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using static Terraria.ModLoader.ModContent;

using TRAEProject.NewContent.Items.Misc.PermaBuffs;
using System.Collections.Generic;
using TRAEProject.NewContent.Buffs;

namespace TRAEProject.Changes
{
    public class DesertLamp : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.DjinnLamp:
                    item.rare = ItemRarityID.Yellow;
                    break;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.DjinnLamp:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Placeable")
                        {
                            line.Text += "\nRight click it to make a wish";
                        }
                    }
                    return;
            }
        }
    }
    public class DesertSpiritLamp : GlobalTile
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void MouseOver(int i, int j, int type)
        {
            if(type == TileID.DjinnLamp)
            {
                Player player = Main.LocalPlayer;
                player.noThrow = 2;
                player.cursorItemIconEnabled = true;
                player.cursorItemIconID = ItemID.DjinnLamp;
            }
            base.MouseOver(i, j, type);
        }
        public override void RightClick(int i, int j, int type)
        {
            if(type == TileID.DjinnLamp)
            {
                Player player = Main.LocalPlayer;
                player.noThrow = 2;
                //Main.NewText(Main.tile[i, j].TileFrameX);
                Tile tile = Main.tile[i, j];
                Vector2 lampCenter = new Vector2(i * 16 + (tile.TileFrameX == 0 ?  8 : -8), j * 16) + new Vector2(8, 8);
                //Main.NewText(lampCenter + ", " + player.MountedCenter);
                if((lampCenter - player.MountedCenter).Length()  < 48)
                {
                    player.GetModPlayer<LampInteraction>().rubHere = lampCenter;
                }

            }
        }
    }
    public class LampInteraction : ModPlayer
    {
        public Vector2? rubHere = null;
        int rubTime = 0;
        public override void PostUpdate()
        {
            if(rubHere != null)
            {
                Vector2 rubAt = (Vector2)rubHere;
                if((rubAt - Player.MountedCenter).Length()  > 48)
                {
                    rubTime = 0;
                    rubHere = null;
                    return;
                }
                Player.direction = Math.Sign(rubAt.X - Player.MountedCenter.X);
                rubTime++;

				Player.CompositeArmStretchAmount stretch = Player.CompositeArmStretchAmount.Quarter;
				if (rubTime % 30 > 10)
				{
					stretch = Player.CompositeArmStretchAmount.ThreeQuarters;
				}
				if (rubTime % 30 > 20)
				{
					stretch = Player.CompositeArmStretchAmount.Full;
				}
				float rotation = (rubAt - Player.MountedCenter).ToRotation() - MathF.PI / 2f;
				Player.SetCompositeArmBack(enabled: true, stretch, rotation);
                //Dust.NewDustPerfect(rubAt, 6);
                Vector2 nozzle = rubAt + new Vector2(-14, -6);
                Dust d = Dust.NewDustPerfect(nozzle, 6, -1.5f * Vector2.UnitY);
                d.noGravity = true;
                d.color = Color.SkyBlue;
                Player.AddBuff(ModContent.BuffType<WishForSpeedBuff>(), 2);
                /*
                if(rubTime > 180)
                {
                    for(int i =0; i < 50; i++)
                    {
                        float r = MathF.PI / -2f + Main.rand.NextFloat()*MathF.PI / 4f * (Main.rand.NextBool() ? -1 : 1);
                        Dust c = Dust.NewDustPerfect(nozzle, 6, TRAEMethods.PolarVector(Main.rand.NextFloat()*8f, r));
                        c.noGravity = true;
                        c.color = Color.SkyBlue;
                    }
                    Item.NewItem(Player.GetSource_TileInteraction((int)rubAt.X, (int)rubAt.Y), nozzle, ItemType<SpeedWish>()); 
                    rubTime = 0;
                    rubHere = null;
                }
                */

            }
            else
            {
                rubTime = 0;
            }
        }
    }
}