using Terraria;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
namespace TRAEProject.NewContent.Projectiles
{
    public class Star1: ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Star1");     //The English name of the Projectile
        }

        public override void SetDefaults()
        {
            Projectile.width = 34;               //The width of Projectile hitbox
            Projectile.height = 34;              //The height of Projectile hitbox
            Projectile.scale = 1.1f;
            Projectile.friendly = true;         //Can the Projectile deal damage to enemies?
            Projectile.DamageType = DamageClass.Melee;           //Is the Projectile shoot by a Melee weapon?
            Projectile.penetrate = 4;           //How many monsters the Projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
            Projectile.timeLeft = 90;          //The live time for the Projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.light = 1f;            //How much light emit around the Projectile
            Projectile.tileCollide = false;          //Can the Projectile collide with tiles?
            Projectile.extraUpdates = 0;            //Set to above 0 if you want the Projectile to update multiple time in a frame
        }
        public override void AI()
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 43, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 0, default, 0.7f);
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 57, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 0, default, 0.7f);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            Vector2 move = Vector2.Zero;
            bool target = false;
            float distance = 300f;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && !Main.npc[k].immortal)
                {
                    Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                    float distanceTo = MathF.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                    if (distanceTo < distance)
                    {
                        move = newMove;
                        target = true;
                    }
                    if (target)
                    {
                        float scaleFactor2 = Projectile.velocity.Length();
                        move.Normalize();
                        move *= scaleFactor2;
                        Projectile.velocity = (Projectile.velocity * 20f + move) / 21f;
                        Projectile.velocity.Normalize();
                        Projectile.velocity *= scaleFactor2;
                    }
                }
            }
        }
    }
}