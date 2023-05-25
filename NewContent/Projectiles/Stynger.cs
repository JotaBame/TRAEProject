using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using TRAEProject.Common;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Projectiles
{
    public class Stynger : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            // DisplayName.SetDefault("Stynger");     //The English name of the Projectile
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;               //The width of Projectile hitbox
            Projectile.height = 16;              //The height of Projectile hitbox
            Projectile.scale = 1f;
            Projectile.friendly = true;         //Can the Projectile deal damage to enemies?
            Projectile.DamageType = DamageClass.Ranged;           //Is the Projectile shoot by a Melee weapon?
            Projectile.penetrate = 5;           //How many monsters the Projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
            Projectile.CloneDefaults(ProjectileID.Stynger);
            AIType = ProjectileID.Stynger;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.GetGlobalProjectile<ProjectileStats>().FirstHitDamage = 2.25f;

        }
        bool dontDoThisAgain = false;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!dontDoThisAgain)
            {
                SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, Projectile.position);
                for (int num947 = 0; num947 < 10; num947++)
                {
                    int num948 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                    Dust dust2 = Main.dust[num948];
                    dust2.velocity *= 0.9f;
                }
                for (int num949 = 0; num949 < 5; num949++)
                {
                    int num950 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2.5f);
                    Main.dust[num950].noGravity = true;
                    Dust dust2 = Main.dust[num950];
                    dust2.velocity *= 3f;
                    num950 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                    dust2 = Main.dust[num950];
                    dust2.velocity *= 2f;
                }
                int num951 = Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X, Projectile.position.Y), default(Vector2), Main.rand.Next(61, 64));
                Gore gore = Main.gore[num951];
                gore.velocity *= 0.3f;
                Main.gore[num951].velocity.X += Main.rand.Next(-1, 2);
                Main.gore[num951].velocity.Y += Main.rand.Next(-1, 2); Projectile.timeLeft = 3;
                Projectile.position.X += Projectile.width / 2;
                Projectile.position.Y += Projectile.height / 2;
                Projectile.width = 150;
                Projectile.height = 150;
                Projectile.position.X -= Projectile.width / 2;
                Projectile.position.Y -= Projectile.height / 2;
                Projectile.tileCollide = false; Projectile.alpha = 255;

                if (Projectile.owner == Main.myPlayer)
                {

                    int num952 = Main.rand.Next(2, 6);
                    for (int num953 = 0; num953 < num952; num953++)
                    {

                        float num954 = Main.rand.Next(-100, 101);
                        num954 += 0.01f;
                        float num955 = Main.rand.Next(-100, 101);
                        num954 -= 0.01f;
                        float num956 = MathF.Sqrt(num954 * num954 + num955 * num955);
                        num956 = 8f / num956;
                        num954 *= num956;
                        num955 *= num956;
                        Projectile shrapnel = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), new(Projectile.Center.X - Projectile.oldVelocity.X, Projectile.Center.Y - Projectile.oldVelocity.Y), new(num954, num955), ProjectileType<StyngerShrapnel>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                        shrapnel.frame = Main.rand.Next(5);
                    }
                }
                dontDoThisAgain = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            if (!dontDoThisAgain)
            {
                SoundEngine.PlaySound(SoundID.Item14 with { MaxInstances = 0 }, Projectile.position);
                for (int num947 = 0; num947 < 10; num947++)
                {
                    int num948 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 1.5f);
                    Dust dust2 = Main.dust[num948];
                    dust2.velocity *= 0.9f;
                }
                for (int num949 = 0; num949 < 5; num949++)
                {
                    int num950 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 2.5f);
                    Main.dust[num950].noGravity = true;
                    Dust dust2 = Main.dust[num950];
                    dust2.velocity *= 3f;
                    num950 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 1.5f);
                    dust2 = Main.dust[num950];
                    dust2.velocity *= 2f;
                }
                int num951 = Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X, Projectile.position.Y), default(Vector2), Main.rand.Next(61, 64));
                Gore gore = Main.gore[num951];
                gore.velocity *= 0.3f;
                Main.gore[num951].velocity.X += Main.rand.Next(-1, 2);
                Main.gore[num951].velocity.Y += Main.rand.Next(-1, 2); Projectile.timeLeft = 3;
                Projectile.position.X += Projectile.width / 2;
                Projectile.position.Y += Projectile.height / 2;
                Projectile.width = 150;
                Projectile.height = 150;
                Projectile.position.X -= Projectile.width / 2;
                Projectile.position.Y -= Projectile.height / 2;
                Projectile.tileCollide = false; Projectile.alpha = 255;

                if (Projectile.owner == Main.myPlayer)
                {

                    int num952 = Main.rand.Next(2, 6);
                    for (int num953 = 0; num953 < num952; num953++)
                    {

                        float num954 = Main.rand.Next(-100, 101);
                        num954 += 0.01f;
                        float num955 = Main.rand.Next(-100, 101);
                        num954 -= 0.01f;
                        float num956 = MathF.Sqrt(num954 * num954 + num955 * num955);
                        num956 = 8f / num956;
                        num954 *= num956;
                        num955 *= num956;
                        Projectile shrapnel = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), new(Projectile.Center.X - Projectile.oldVelocity.X, Projectile.Center.Y - Projectile.oldVelocity.Y), new(num954, num955), ProjectileType<StyngerShrapnel>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                        shrapnel.frame = Main.rand.Next(5);
                    }
                }
                dontDoThisAgain = true;
            }
            return false;
        }
    }
}

