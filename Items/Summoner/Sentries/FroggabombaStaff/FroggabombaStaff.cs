using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Projectiles;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Items.Summoner.Sentries.FroggabombaStaff
{
    public class FroggabombaStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Froggybomba Staff");
            Tooltip.SetDefault("Summons a next of kamikaze frogs");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 46; // what does this do
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useTurn = false;
            Item.autoReuse = true;
            Item.sentry = true;
            Item.mana = 25;
            Item.UseSound = SoundID.Item78;

            Item.damage = 20;
            //item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ProjectileType<FrogEggs>();

            //item.shootSpeed = 3.5f;



            Item.noMelee = true;
        }
        /* public override Vector2? HoldoutOffset()
         {
             return new Vector2(5, 0);
         }*/
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            position = Main.MouseWorld;

            return true;

        }
    }

    //______________________________________________________________________________________________________
    public class FrogEggs : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FrogEggs");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

        }
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.sentry = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 99999;
            Projectile.tileCollide = false;

        }
        public override bool? CanDamage()
        {

            return false;
        }
        int shoottime = 0;
        NPC target;
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }
            Main.player[Projectile.owner].UpdateMaxTurrets();
            Player player = Main.player[Projectile.owner];
            shoottime++;
            //Getting the npc to fire at
            for (int i = 0; i < 200; i++)
            {

                if (player.HasMinionAttackTargetNPC)
                {
                    target = Main.npc[player.MinionAttackTargetNPC];
                }
                else
                {
                    target = Main.npc[i];

                }

                //Getting the shooting trajectory
                float shootToX = target.position.X + target.width * 0.5f - Projectile.Center.X;
                float shootToY = target.position.Y + target.height * 0.5f - Projectile.Center.Y;
                float distance = (float)Math.Sqrt((shootToX * shootToX + shootToY * shootToY));
                //bool lineOfSight = Collision.CanHitLine(Projectile.Center, 1, 1, target.Center, 1, 1);
                //If the distance between the projectile and the live target is active

                if (distance < 200f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.type != NPCID.TargetDummy && Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                {

                    if (shoottime > 20)
                    {

                        Vector2 perturbedSpeed = new Vector2(shootToX, shootToY).RotatedByRandom(MathHelper.ToRadians(15));

                        Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ProjectileType<Froggy>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0f, 0f);

                        shoottime = 0;
                    }
                }
            }
        }
    }
    public class Froggy : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Froggy");
            Main.projFrames[Projectile.type] = 4;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.BabySpider);
            AIType = ProjectileID.BabySpider;
            Projectile.GetGlobalProjectile<TRAEGlobalProjectile>().explodes = true;
            Projectile.GetGlobalProjectile<TRAEGlobalProjectile>().ExplosionRadius = 80;
            Projectile.GetGlobalProjectile<TRAEGlobalProjectile>().UsesDefaultExplosion = true;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }
            Main.player[Projectile.owner].UpdateMaxTurrets();
            Player player = Main.player[Projectile.owner];

        }
    }
}
