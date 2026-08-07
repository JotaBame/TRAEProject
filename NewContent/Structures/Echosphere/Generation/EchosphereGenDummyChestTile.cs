using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Structures.Echosphere.Generation
{
    /// <summary>
    /// PLACE WHERE THE TOP LEFT OF THE CHEST WILL BE
    /// </summary>
    public class EchosphereGenDummyChestTile : ModTile
    {
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Main.tileSolid[Type] = true;
            Vector2 worldPos = new Vector2(i * 16 + 8, j * 16 + 8);
            Vector2 screenPos = worldPos - Main.screenPosition;
            // screenPos += new Vector2(16 * 16, 7 * 16);//why this? idk
            screenPos += new Vector2(12 * 16, 12 * 16);//why this? idk

            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            spriteBatch.Draw(tex, screenPos, null, Color.White, 0, tex.Size() / 2, 16f / tex.Width, SpriteEffects.None, 0f);
            return false;
        }
        public override string Texture => "Terraria/Images/Item_" + ItemID.ShadowChest;
    }
    public class EchosphereGenDummyOreTile : EchosphereGenDummyChestTile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.GoldOre;
    }
    /// <summary>
    /// PLACE ON TOP LEFT OF PAINTING SPOT
    /// </summary>
    public class EchosphereGenDummyPaintingTile6x4 : EchosphereGenDummyChestTile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Constellation;//bunny constellation painting
    }
    /// <summary>
    /// PLACE ON TOP LEFT OF PAINTING SPOT
    /// </summary>
    public class EchosphereGenDummyPaintingTile3x3 : EchosphereGenDummyChestTile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.TerraBladeChronicles;//terra blade painting
    }
    /// <summary>
    /// PLACE ON TOP LEFT OF PAINTING SPOT
    /// </summary>
    public class EchosphereGenDummyPaintingTile3x2 : EchosphereGenDummyChestTile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.PlaceAbovetheClouds;//floating island painting
    }
    /// <summary>
    /// PLACE ON TOP LEFT OF PAINTING SPOT
    /// </summary>
    public class EchosphereGenDummyPaintingTile2x3 : EchosphereGenDummyChestTile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.BlessingfromTheHeavens;//floating island painting
    }
    /// <summary>
    /// PLACE ON TOP LEFT OF STATUE SPOT
    /// </summary>
    public class EchosphereGenDummyStatueTile : EchosphereGenDummyChestTile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.SwordStatue;//floating island painting
    }
    public class EchosphereGenDummyTilePlacerItem : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.SkyFracture;
        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.noMelee = true;
            Item.useTime = Item.useAnimation = 3;
            Item.useStyle = 1;
            Item.reuseDelay = 0;
            Item.shoot = ProjectileID.FairyQueenHymn;//dummy value, needed for Shoot to execute
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Item.color = Main.hslToRgb(Main.rand.NextFloat(), 1, .5f);
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
        public override bool AltFunctionUse(Player player)
        {
            return player.ItemTimeIsZero;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips[1].Text = "right click = chest, left click = ore";
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int i, j;
            Point tileCoords = Main.MouseWorld.ToTileCoordinates();
            i = tileCoords.X;
            j = tileCoords.Y;

            Tile tile = Main.tile[tileCoords];
            if (player.altFunctionUse == 2)
            {
                tile.ClearTile();
                tile.ClearBlockPaintAndCoating();
                WorldGen.PlaceTile(i, j, ModContent.TileType<EchosphereGenDummyChestTile>());
                return false;
            }
            WorldGen.PlaceTile(i, j, ModContent.TileType<EchosphereGenDummyOreTile>());
            return false;
        }
    }
    public class EchosphereGenDummyDecorPlacerItem1 : EchosphereGenDummyTilePlacerItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.CrystalLamp;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips[1].Text = "right click = statue, left click = painting 6x4";
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int i, j;
            Point tileCoords = Main.MouseWorld.ToTileCoordinates();
            i = tileCoords.X;
            j = tileCoords.Y;

            Tile tile = Main.tile[tileCoords];
            if (player.altFunctionUse == 2)
            {
                tile.ClearTile();
                tile.ClearBlockPaintAndCoating();
                WorldGen.PlaceTile(i, j, ModContent.TileType<EchosphereGenDummyStatueTile>());
                return false;
            }
            WorldGen.PlaceTile(i, j, ModContent.TileType<EchosphereGenDummyPaintingTile6x4>());
            return false;
        }
    }
    public class EchosphereGenDummyDecorPlacerItem2 : EchosphereGenDummyTilePlacerItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.LampPost;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips[1].Text = "right click = painting 3x2, left click = painting 3x3";
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int i, j;
            Point tileCoords = Main.MouseWorld.ToTileCoordinates();
            i = tileCoords.X;
            j = tileCoords.Y;

            Tile tile = Main.tile[tileCoords];
            if (player.altFunctionUse == 2)
            {
                tile.ClearTile();
                tile.ClearBlockPaintAndCoating();
                WorldGen.PlaceTile(i, j, ModContent.TileType<EchosphereGenDummyPaintingTile3x2>());
                return false;
            }
            WorldGen.PlaceTile(i, j, ModContent.TileType<EchosphereGenDummyPaintingTile3x3>());
            return false;
        }
    }
    public class EchosphereGenDummyDecorPlacerItem3 : EchosphereGenDummyTilePlacerItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.RainbowMoss;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips[1].Text = "left click = painting 2x3, right click = print hovered tile framing";
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int i, j;
            Point tileCoords = Main.MouseWorld.ToTileCoordinates();
            i = tileCoords.X;
            j = tileCoords.Y;

            Tile tile = Main.tile[tileCoords];
            if (player.altFunctionUse == 2)
            {
                //  tile.Clear(TileDataType.All);
                //WorldGen.PlaceTile(i, j, ModContent.TileType<EchosphereGenDummyPaintingTile3x2>());
                Main.NewText("framex: " + tile.TileFrameX + ", framey: " + tile.TileFrameY);
                return false;
            }
            WorldGen.PlaceTile(i, j, ModContent.TileType<EchosphereGenDummyPaintingTile2x3>());
            return false;
        }
    }
}
