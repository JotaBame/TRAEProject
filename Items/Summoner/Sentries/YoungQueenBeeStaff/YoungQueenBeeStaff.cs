using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Items.Summoner.Sentries.YoungQueenBeeStaff
{
    public class YoungQueenBeeStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Young Queen Bee Staff");
            Tooltip.SetDefault("Summons a bee Sentry that shoots bees at your enemies");
            ItemID.Sets.SortingPriorityMaterials[Item.type] = 46; // what does this do
            ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
            ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
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

            Item.damage = 10;
            Item.DamageType = DamageClass.Summon;
            Item.knockBack = 1f;

            Item.shoot = ProjectileType<YoungQueenBee>();

            //item.shootSpeed = 3.5f;



            Item.noMelee = true;
        }
        /* public override Vector2? HoldoutOffset()
         {
             return new Vector2(5, 0);
         }*/
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.SpawnMinionOnCursor(source, player.whoAmI, type, Item.damage, knockback);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.BeeWax, 12)
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
            Projectile.minion = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.tileCollide = true;

        }
        public override bool? CanDamage()
        {

            return false;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 12)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 2;
            }
            Player player = Main.player[Projectile.owner];
            //Getting the npc to fire at
            int SentryRange = 40; //The sentry's range
            int Speed = 60; //How fast the sentry can shoot the Projectile.
            float FireVelocity = 15f; //The velocity the sentry's shot projectile will travel. Slows down the closer the NPC is.
            Main.player[Projectile.owner].UpdateMaxTurrets(); //This makes the sentry be able to spawn more if your sentry cap is greater than one.
            for (int t = 0; t < Main.maxNPCs; t++)
            {
                NPC npc = Main.npc[t];

                float distance = Projectile.Distance(npc.Center); //Set the distance from the NPC and the sentry Projectile.

                //Convert distance to tile position, and continue if the following NPC parameters are met.
                if (distance / 16 < SentryRange && Main.npc[t].active && !Main.npc[t].dontTakeDamage && !Main.npc[t].friendly && Main.npc[t].lifeMax > 5 && Main.npc[t].type != NPCID.TargetDummy)
                {
                    Projectile.ai[1] = npc.whoAmI;
                }
            }

            NPC target = Main.npc[(int)Projectile.ai[1]] ?? new NPC();

            Projectile.ai[0]++;
            if (target.active && Projectile.Distance(target.Center) / 16 < SentryRange && Projectile.ai[0] % Speed == 5)
            {

                Vector2 direction = target.Center - Projectile.Center; //The direction the projectile will fire.

                direction.Normalize(); //Normalizes the direction vector.
                direction.X *= FireVelocity; //Multiply direction by fireVelocity so the sentry can fire the projectile faster the farther the NPC is away.
                direction.Y *= FireVelocity; //Same as above, but with Y velocity.

                SoundEngine.PlaySound(SoundID.Item97, Projectile.Center); //Play a sound.

                int damage = Projectile.damage; //How much damage the projectile shot from the sentry will do.
               
                int bees = Main.rand.Next(4, 7);

                for (int index1 = 0; index1 < bees; ++index1)
                {
                    float knockback = 0;
                    int type = ProjectileID.Bee; //The type of projectile the sentry will shoot. Use ModContent.ProjectileType<>() to fire a modded Projectile.
                    if (Main.rand.Next(4) == 0)
                    {
                        knockback = 2f;
                        type = ProjectileID.GiantBee;
                    };
                    float num4 = 6f;
                    float num8 = 6f;
                    float SpeedX2 = num4 + (float)Main.rand.Next(-35, 36) * 0.02f;
                    float SpeedY2 = num8 + (float)Main.rand.Next(-35, 36) * 0.02f;
                    int index2 = Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, SpeedX2, SpeedY2, type, damage, knockback);
                }
            }
        }
    }
}