using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;

namespace TRAEProject.NewContent.Projectiles
{
    public class StyngerShrapnel : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
            // DisplayName.SetDefault("StyngerShrapnel");     //The English name of the Projectile
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;               //The width of Projectile hitbox
            Projectile.height = 16;              //The height of Projectile hitbox
            Projectile.scale = 1f;
            Projectile.friendly = true;         //Can the Projectile deal damage to enemies?
            Projectile.DamageType = DamageClass.Ranged;           //Is the Projectile shoot by a Melee weapon?
            Projectile.penetrate = 3;           //How many monsters the Projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
            Projectile.CloneDefaults(ProjectileID.StyngerShrapnel);
            AIType = ProjectileID.StyngerShrapnel;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

           
        }
        bool dontDoThisAgain = false;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!dontDoThisAgain)
            {
                if (!dontDoThisAgain)
                {
                    SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, Projectile.position);
                    for (int num958 = 0; num958 < 7; num958++)
                    {
                        int num959 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                        Dust dust2 = Main.dust[num959];
                        dust2.velocity *= 0.8f;
                    }
                    for (int num960 = 0; num960 < 2; num960++)
                    {
                        int num961 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2.5f);
                        Main.dust[num961].noGravity = true;
                        Dust dust2 = Main.dust[num961];
                        dust2.velocity *= 2.5f;
                        num961 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                        dust2 = Main.dust[num961];
                        dust2.velocity *= 1.5f;
                    }
                    int num962 = Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X, Projectile.position.Y), default(Vector2), Main.rand.Next(61, 64));
                    Gore gore2 = Main.gore[num962];
                    gore2.velocity *= 0.2f;
                    Main.gore[num962].velocity.X += Main.rand.Next(-1, 2);
                    Main.gore[num962].velocity.Y += Main.rand.Next(-1, 2); 
                    Projectile.timeLeft = 3;
                    Projectile.position.X += Projectile.width / 2;
                    Projectile.position.Y += Projectile.height / 2;
                    Projectile.width = 100;
                    Projectile.height = 100; Projectile.alpha = 255;

                    Projectile.position.X -= Projectile.width / 2;
                    Projectile.position.Y -= Projectile.height / 2;
                    dontDoThisAgain = true;
                }
                return;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!dontDoThisAgain)
            {

                SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, Projectile.position);
                for (int num958 = 0; num958 < 7; num958++)
                {
                    int num959 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                    Dust dust2 = Main.dust[num959];
                    dust2.velocity *= 0.8f;
                }
                for (int num960 = 0; num960 < 2; num960++)
                {
                    int num961 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2.5f);
                    Main.dust[num961].noGravity = true;
                    Dust dust2 = Main.dust[num961];
                    dust2.velocity *= 2.5f;
                    num961 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                    dust2 = Main.dust[num961];
                    dust2.velocity *= 1.5f;
                }
                Projectile.position.X += Projectile.width / 2;
                Projectile.position.Y += Projectile.height / 2;
                Projectile.width = 100;
                Projectile.height = 100;
                Projectile.alpha = 255;
                Projectile.position.X -= Projectile.width / 2;
                Projectile.position.Y -= Projectile.height / 2;
                Projectile.tileCollide = false;
                int num962 = Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X, Projectile.position.Y), default(Vector2), Main.rand.Next(61, 64));
                Gore gore2 = Main.gore[num962];
                gore2.velocity *= 0.2f;
                Main.gore[num962].velocity.X += Main.rand.Next(-1, 2);
                Main.gore[num962].velocity.Y += Main.rand.Next(-1, 2);
                Projectile.timeLeft = 3;
                dontDoThisAgain = true;

            }
            return false;
        }
    }
}