using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TRAEProject.NewContent.Structures.NewSkyIslands;

namespace TRAEProject.NewContent.Structures.Echosphere.Generation
{
    internal class EchosphereGenTestItem : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.WandofFrosting;
        public override void SetDefaults()
        {
            Item.useTime = Item.useAnimation = 3;
            Item.useStyle = 1;
            Item.shoot = ProjectileID.PurificationPowder;//dummy value, needed for Shoot to execute
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Item.color = Main.hslToRgb(Main.rand.NextFloat(), 1, .5f);
            return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
        static Vector2 ClampLength(Vector2 vec, float maxLength)
        {
            if (vec.Length() > maxLength)
            {
                vec.Normalize();
                vec *= maxLength;
            }
            return vec;
        }
        public static void ShootMarkerTowards(Vector2 from, Vector2 to)
        {
            int type = ProjectileID.HeatRay;
            int projDuration = ContentSamples.ProjectilesByType[type].timeLeft;
            Projectile p = Projectile.NewProjectileDirect(null, from, ClampLength((to - from) / projDuration, 30), type, -1, 0, -1);
            p.VanillaAI();
            p.VanillaAI();
            p.tileCollide = false;
            p.ignoreWater = true;
            Dust.QuickDust(EchosphereGeneratorSystem.echosphereBottomRight, Color.White).scale *= 5;   
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.mouseRight)
            {
             //   NewSkyIslandsGen.GenerateHouses();
                //  EchosphereGenHelper.Debug_ClearTop200Tiles();
                //  ModContent.GetInstance<EchosphereGeneratorSystem>().PostWorldGen();
            }
            // type = ProjectileID.HeatRay;
            //   Main.NewText("echosphere top left: " + EchosphereGeneratorSystem.echosphereTopLeft);
            //   Main.NewText("echosphere bottom right: " + EchosphereGeneratorSystem.echosphereBottomRight);
            //   int projDuration = ContentSamples.ProjectilesByType[type].timeLeft;

            //Main.NewText(GenVars.skyLakes, Main.DiscoColor);
            //for (int i = 0; i < GenVars.floatingIslandHouseX.Length; i++)
            //{
            //    Main.NewText(GenVars.skyLake[i], Main.DiscoColor);
            //    Vector2 islandPos = new Vector2(GenVars.floatingIslandHouseX[i] * 16 + 8, GenVars.floatingIslandHouseY[i] * 16 + 8);
            //    ShootMarkerTowards(position, islandPos);
            //}
            ShootMarkerTowards(position, EchosphereGeneratorSystem.echosphereTopLeft);
            ShootMarkerTowards(position, EchosphereGeneratorSystem.echosphereBottomRight);
            //Projectile.NewProjectile(null, position, ClampLength((EchosphereGeneratorSystem.echosphereTopLeft - position) / projDuration, 20), type, -1, 0, -1);
            //Projectile.NewProjectile(null, position, ClampLength((EchosphereGeneratorSystem.echosphereBottomRight - position) / projDuration, 20), type, -1, 0, -1);
            //Dust.QuickDust(EchosphereGeneratorSystem.echosphereBottomRight, Color.White).scale *= 5;
            //Dust.QuickDust(EchosphereGeneratorSystem.echosphereTopLeft, Color.White).scale *= 5;
            return false;
            ////goal, do a flood fill on empty tiles with the null tile id
            ////then the same with walls.
            ////as a failsafe, use this maxBlocks variable so it doesn't fill too many
            int maxBlocks = 10000;
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

                if (tile == null || tile.LiquidAmount > 0)
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
