using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.Items.RuneSetEffects
{
    public class RuneSetGlobalItem : GlobalItem
    {
        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (!RuneSetPlayer.RuneAffectedItems.Contains(item.type))
            {
                return;
            }
            Player plr = Main.LocalPlayer;
            RuneSetPlayer runePlr = plr.GetModPlayer<RuneSetPlayer>();
            if (!runePlr.runeEffects)
            {
                return;
            }
            int itemSlot = RuneSetHelper.FindItemInvSlot(plr, item);
            RuneSetHelper.FindClosestStaffToSlot(plr, itemSlot, out int closestGemStaffID);
            if(closestGemStaffID <= 0)
            {
                return;
            }
            Color color = new Color(200, 200, 200, 0);
            float opacity = Utils.Remap( MathF.Sin(Main.GlobalTimeWrappedHourly * 3), -1, 1, 0.5f, 1f);
            color *= opacity;
            int gemID = GetLargeGemID(closestGemStaffID);
            Main.instance.LoadItem(gemID);
            Texture2D tex = TextureAssets.Item[gemID].Value;
            scale = 32f / MathF.Max(tex.Height, tex.Width);
            spriteBatch.Draw(tex, position, null, color, 0, tex.Size() / 2, scale, SpriteEffects.None, 0f);
        }
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return RuneSetPlayer.RuneAffectedItems.Contains(entity.type);
        }
        static int GetLargeGemID(int staffID)
        {
            return staffID switch
            {
                ItemID.AmethystStaff => ItemID.LargeAmethyst,
                ItemID.TopazStaff => ItemID.LargeTopaz,
                ItemID.EmeraldStaff => ItemID.LargeEmerald,
                ItemID.SapphireStaff => ItemID.LargeSapphire,
                ItemID.RubyStaff => ItemID.LargeRuby,
                ItemID.DiamondStaff => ItemID.LargeDiamond,
                _ => (int)ItemID.LargeAmber,
            };
        }
    }
}
