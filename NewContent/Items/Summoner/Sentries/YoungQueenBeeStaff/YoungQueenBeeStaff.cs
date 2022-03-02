using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Summoner.Sentries.YoungQueenBeeStaff
{
    public class YoungQueenBeeStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Young Queen Bee Staff");
            Tooltip.SetDefault("Summons a bee Sentry that shoots bees at your enemies");
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useTurn = false;
            Item.autoReuse = true;

            Item.mana = 25;
            Item.UseSound = SoundID.Item78;

            Item.damage = 9;
            Item.DamageType = DamageClass.Summon;
            Item.knockBack = 1f;

            Item.sentry = true;
            Item.shoot = ProjectileType<YoungQueenBee>();

            //item.shootSpeed = 3.5f;



            Item.noMelee = true;
        }
        /* public override Vector2? HoldoutOffset()
         {
             return new Vector2(5, 0);
         }*/
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
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient(ItemID.BeeWax, 12)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    //______________________________________________________________________________________________________
    public class YoungQueenBee : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Young Queen Bee");
            Main.projFrames[Projectile.type] = 2;
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
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.aiStyle = 2;
        }
        public override bool? CanDamage()
        {

            return false;
        }
        int summontime;
        int shoottime = 0;
        bool animate = false;
        NPC target;
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {

            fallThrough = false;

            return true;
        }

        public override void AI()
        {
            //Projectile.velocity.Y = 10;
            Projectile.rotation = 0;

            Main.player[Projectile.owner].UpdateMaxTurrets();

            shoottime++;
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
       
                if (shoottime > 180)
                {
                    if (distance < 400f && !target.friendly && target.active && !target.dontTakeDamage && target.lifeMax > 5 && target.type != NPCID.TargetDummy)
                    {

                        if (Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
                        {
                            target.TargetClosest(true);
                            SoundEngine.PlaySound(SoundID.Item97, Projectile.position);
                            int type = ProjectileID.Bee;
                            //Dividing the factor of 2f which is the desired velocity by distance
                            distance = 1.6f / distance;

                            //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
                            shootToX *= distance * 5f;
                            shootToY *= distance * 5f;

                            
                          
                            int numberProjectiles = 2 + Main.rand.Next(3); // 2 to 4 shots
                            for (int l = 0; l < numberProjectiles; l++)
                            {
                               
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
                                if (Main.rand.Next(4) == 0)
                                {
                                    Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Top.Y + 14), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ProjectileID.GiantBee, Projectile.damage, Projectile.knockBack, Projectile.owner);

                                }

                            }
                            shoottime = 0;


                        }

                    }


                }
            }
           
            Projectile.velocity.Y = 5;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 12)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 2;
            }



        }




        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override void Kill(int timeLeft)
        {
            int num528 = Gore.NewGore(Projectile.position, new Vector2(0f, 0f), Main.rand.Next(61, 64), 1f);
            Gore gore = Main.gore[num528];
            gore.velocity *= 0.1f;
        }
       
            
    }
}