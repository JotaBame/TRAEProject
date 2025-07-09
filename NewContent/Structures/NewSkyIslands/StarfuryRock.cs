using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using ReLogic.Content;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TRAEProject.NewContent.Structures.NewSkyIslands
{
    public class StarfuryPlacesSTarfuryRockAsTest : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.type == ItemID.Starfury;
        }
        public override void SetDefaults(Item entity)
        {
            entity.createTile = ModContent.TileType<StarfuryRock>();
        }
    }
    public class StarfuryRock : ModTile
    {
        public static Asset<Texture2D> starfury;
        public override void SetStaticDefaults()
        {
            Main.tileNoAttach[Type] = true;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = [16, 16, 18];
            DustType = DustID.GemAmethyst;
            TileObjectData.addTile(Type);
            Main.tileLighted[Type] = true;
            
            AddMapEntry(new Color(237, 63, 133));
        }
        private static Vector2 TileOffset => Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile t = Main.tile[i, j];
            if (t.TileFrameX != 0 || t.TileFrameY != 0)
            {
                return true;
            }
            
            Vector2 pos = new Vector2(i * 16, j * 16);
            pos += TileOffset;
            pos -= Main.screenPosition;
            pos.X += 20;
            pos.Y += 10;
            Color drawColor = Color.White;
            drawColor.A = 150;
            drawColor.R = drawColor.G = drawColor.B = 250;

            spriteBatch.Draw(starfury.Value, pos, null, drawColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            return true;
        }
        public override void Load()
        {
            starfury = ModContent.Request<Texture2D>("TRAEProject/NewContent/Structures/NewSkyIslands/StarfuryRockStarfury");
        }
        public override void NumDust(int x, int y, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
