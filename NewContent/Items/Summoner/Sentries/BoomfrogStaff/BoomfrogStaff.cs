using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Summoner.Sentries.BoomfrogStaff
{
    public class BoomfrogStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Boomfrog Staff");
            Tooltip.SetDefault("Summons a parent boomfrog sentry that spawns kamikaze frogs");
            //ItemID.Sets.SortingPriorityMaterials[Item.type] = 46; // what does this do
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            Item.width = 38;
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
            Item.DamageType = DamageClass.Summon;
            Item.damage = 20;
            //item.crit = 4;
            Item.knockBack = 1f;

            Item.shoot = ProjectileType<Boomfrog>();

            //item.shootSpeed = 3.5f;



            Item.noMelee = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim(false);
            }
            return base.UseItem(player);
        }
    }

    //______________________________________________________________________________________________________
    public class Boomfrog : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Boomfrog");
            Main.projFrames[Projectile.type] = 8;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

        }
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.sentry = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.tileCollide = true;
            Projectile.aiStyle = 2;
        }
        public override bool? CanDamage()
        {

            return false;
        }
        int summontime;
        int shoottime = 0;
        NPC target;
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {

            fallThrough = false;

            return true;
        }

        public override void AI()
        {
            Projectile.velocity.Y = 10;
            Projectile.rotation = 0;

            Main.player[Projectile.owner].UpdateMaxTurrets();

            shoottime++;
            if (Main.rand.Next(5) == 0)     //this defines how many dust to spawn
            {
                int dust = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 20, 20, DustID.Torch, 0, 0, 130, default, 1.5f);
                Main.dust[dust].noGravity = true; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 0.5f;
            }
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.1f) / 255f, ((30 - Projectile.alpha) * 0.1f) / 255f, ((30 - Projectile.alpha) * 0.1f) / 255f);   //this is the light colors

            //Getting the npc to fire at
            Player player = Main.player[Projectile.owner];

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
                float shootToX = target.position.X + (float)target.width * 0.5f - Projectile.Center.X;
                float shootToY = target.position.Y + (float)target.height * 0.5f - Projectile.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

                if (shoottime > 35)
                {
                    if (distance < 400f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.type != NPCID.TargetDummy)
                    {

                        if (Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                        {
                            target.TargetClosest(true);
                            SoundEngine.PlaySound(SoundID.Item17, Projectile.position);
                            int type = ProjectileType<Froggy>();
                            //Dividing the factor of 2f which is the desired velocity by distance
                            distance = 1f / distance;

                            //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
                            shootToX *= distance * 5f;
                            shootToY *= distance * 5f;
                            Vector2 perturbedSpeed = new Vector2(shootToX, shootToY).RotatedByRandom(MathHelper.ToRadians(30));
                            if (perturbedSpeed.X < 0f)
                            {
                                Projectile.spriteDirection = 1;
                            }
                            if (perturbedSpeed.X > 0f)
                            {
                                Projectile.spriteDirection = -1;
                            }
                            Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Top.Y + 14), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), type, Projectile.damage, Projectile.knockBack, Projectile.owner);

                            shoottime = 0;


                        }

                    }


                }
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame == 8)
            {
                Projectile.frame = 0;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {

            //Main.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 45);
            for (int i = 0; i < 20; i++)
            {
                var dust2 = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 15, Projectile.position.Y), 30, Projectile.height, 6);
                dust2.noGravity = true;
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
            Projectile.width = 18;
            Projectile.height = 20;
            Projectile.timeLeft = 120;
            Projectile.penetrate = 3;
            Projectile.friendly = true;
            Projectile.aiStyle = 63;
            AIType = ProjectileID.BabySpider; 
            ProjectileID.Sets.SentryShot[Projectile.type] = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().explodes = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontExplodeOnTiles = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().ExplosionRadius = 80;
            Projectile.GetGlobalProjectile<ProjectileStats>().UsesDefaultExplosion = true;
        }
        public override void AI()
        {
                Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }

        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
    }
}
