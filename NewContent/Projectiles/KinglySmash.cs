using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Items;
using static Terraria.ModLoader.ModContent;
namespace TRAEProject.NewContent.Projectiles
{
    public class KinglySmash : ModProjectile
    {

        public override void SetDefaults()
        {
            //Projectile.CloneDefaults(ProjectileID.DD2OgreStomp);
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.alpha = 255;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 20;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (target.type == NPCID.KingSlime)
                return false;
            return null;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= Main.masterMode ? 6 : Main.expertMode ? 4 : 2;
        }
        public override void AI()
        {
            //float num = 20f;

            //Projectile.ai[0] += 1f;
            //if (Projectile.ai[0] > 9f)
            //{
            //    Projectile.Kill();
            //    return;
            //}
            //Projectile.velocity = Vector2.Zero;
            //Projectile.position = Projectile.Center;
            //Projectile.Size = new Vector2(16f, 4f) * MathHelper.Lerp(5f, num, Utils.GetLerpValue(0f, 9f, Projectile.ai[0]));
            //Projectile.Center = Projectile.position;
            //Point point = Projectile.TopLeft.ToTileCoordinates();
            //Point point2 = Projectile.BottomRight.ToTileCoordinates();
            //int num2 = point.X / 2 + point2.X / 2;
            //int num3 = Projectile.width / 2;
            //if ((int)Projectile.ai[0] % 3 != 0)
            //{
            //    return;
            //}
            //int num4 = (int)Projectile.ai[0] / 3;
            //for (int i = point.X; i <= point2.X; i++)
            //{
            //    for (int j = point.Y; j <= point2.Y; j++)
            //    {
            //        if (Vector2.Distance(Projectile.Center, new Vector2(i * 16, j * 16)) > (float)num3)
            //        {
            //            continue;
            //        }
            //        Tile tileSafely = Framing.GetTileSafely(i, j);
            //        if (tileSafely.HasUnactuatedTile || !Main.tileSolid[tileSafely.TileType] || Main.tileSolidTop[tileSafely.TileType] || Main.tileFrameImportant[tileSafely.TileType])
            //        {
            //            continue;
            //        }
            //        Tile tileSafely2 = Framing.GetTileSafely(i, j - 1);
            //        if (tileSafely2.HasUnactuatedTile && Main.tileSolid[tileSafely2.TileType] && !Main.tileSolidTop[tileSafely2.TileType])
            //        {
            //            continue;
            //        }
            //        int num5 = WorldGen.KillTile_GetTileDustAmount(fail: true, tileSafely, i, j);
            //        for (int k = 0; k < num5; k++)
            //        {
            //            Dust dust = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, tileSafely)];
            //            dust.velocity.Y -= 3f + (float)num4 * 1.5f;
            //            dust.velocity.Y *= Main.rand.NextFloat();
            //            dust.velocity.Y *= 0.75f;
            //            dust.scale += (float)num4 * 0.03f;
            //        }
            //        if (num4 >= 2)
            //        {

            //            for (int m = 0; m < num5 - 1; m++)
            //            {
            //                Dust dust2 = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, tileSafely)];
            //                dust2.velocity.Y -= 1f + (float)num4;
            //                dust2.velocity.Y *= Main.rand.NextFloat();
            //                dust2.velocity.Y *= 0.75f;
            //            }
            //        }
            //        if (num5 <= 0 || Main.rand.Next(3) == 0)
            //        {
            //            continue;
            //        }
            //        float num7 = (float)Math.Abs(num2 - i) / (num / 2f);

            //        //Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, 61 + Main.rand.Next(3), 1f - (float)num4 * 0.15f + num7 * 0.5f);
            //        //gore.velocity.Y -= 0.1f + (float)num4 * 0.5f + num7 * (float)num4 * 1f;
            //        //gore.velocity.Y *= Main.rand.NextFloat();

            //    }

            //}
        }
    }
}


