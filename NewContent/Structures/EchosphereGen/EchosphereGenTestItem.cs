using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.NewContent.Structures.EchosphereGen
{
    internal class EchosphereGenTestItem : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.WandofFrosting;
        public override void SetDefaults()
        {
            Item.useStyle = Item.useAnimation = 3;
            Item.shoot = ProjectileID.PurificationPowder;//dummy value, needed for Shoot to execute
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Item.color = Main.hslToRgb(Main.rand.NextFloat(), 1, .5f);
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            ModContent.GetInstance<EchosphereGeneratorSystem>().PostWorldGen();
            return false;
            //goal, do a flood fill on empty tiles with the null tile id
            //then the same with walls.
            //as a failsafe, use this maxBlocks variable so it doesn't fill too many
            int maxBlocks = 1000;
            ushort nullTile = StructureHelper.StructureHelper.NullTileID;
            ushort nullWall = StructureHelper.StructureHelper.NullWallID;
            Point startingCoords = Main.MouseWorld.ToTileCoordinates();
            Queue<Point> queue = new();
            HashSet<Point> visited = [];
            queue.Enqueue(startingCoords);
            visited.Add(startingCoords);
            int placed = 0;
            while (queue.Count > 0 && placed < maxBlocks)
            {
                Point current = queue.Dequeue();
                Tile tile = Main.tile[current.X, current.Y];

                if (tile == null)
                    continue;
                if (!tile.HasTile)
                {
                    tile.TileType = nullTile;
                    tile.HasTile = true;
                    WorldGen.SquareTileFrame(current.X, current.Y);
                    placed++;
                }
                else
                {
                    continue;
                }
                Point[] neighbors =
                [
                new Point(current.X + 1, current.Y),
                new Point(current.X - 1, current.Y),
                new Point(current.X, current.Y + 1),
                new Point(current.X, current.Y - 1)
                ];
                for (int i = 0; i < neighbors.Length; i++)
                {
                    Point neighbor = neighbors[i];
                    if (!visited.Contains(neighbor) &&
                        neighbor.X >= 0 && neighbor.X < Main.maxTilesX &&
                        neighbor.Y >= 0 && neighbor.Y < Main.maxTilesY)
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }




            queue = new();
            visited = [];
            queue.Enqueue(startingCoords);
            visited.Add(startingCoords);
            placed = 0;
            while (queue.Count > 0 && placed < maxBlocks)
            {
                Point current = queue.Dequeue();
                Tile wall = Main.tile[current.X, current.Y];

                if (wall == null)
                    continue;
                if (wall.WallType == 0)
                {
                    wall.WallType = nullWall;
                    WorldGen.SquareWallFrame(current.X, current.Y);
                    placed++;
                }
                else
                {
                    continue;
                }
                Point[] neighbors =
                [
                new Point(current.X + 1, current.Y),
                new Point(current.X - 1, current.Y),
                new Point(current.X, current.Y + 1),
                new Point(current.X, current.Y - 1)
                ];
                for (int i = 0; i < neighbors.Length; i++)
                {
                    Point neighbor = neighbors[i];
                    if (!visited.Contains(neighbor) &&
                        neighbor.X >= 0 && neighbor.X < Main.maxTilesX &&
                        neighbor.Y >= 0 && neighbor.Y < Main.maxTilesY)
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }



            return false;
        }
    }
}
