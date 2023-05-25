using System;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

using Terraria.ModLoader;
using TRAEProject.NewContent.Items.Materials;
using Microsoft.Xna.Framework;
using TRAEProject.Common;
using TRAEProject.Changes.Items;
using Terraria.DataStructures;
using Terraria.Audio;

namespace TRAEProject.NewContent.Items.Weapons.SuperRose
{
    class SuperRose : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Cursed Flower");
            // Tooltip.SetDefault("Summons petals to seek out your foes");
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.damage = 80;
            Item.useAnimation = 6;
            Item.useTime = 6;
            Item.mana = 25;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 6);
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 5f;
            Item.shootSpeed = 8f;
            Item.noMelee = true;
            Item.shoot = ProjectileType<SuperRoseProj>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item20;
            Item.GetGlobalItem<TRAEMagicItem>().rightClickSideWeapon = true;

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, 0f);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].type == type && Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI)
                {
                    Main.projectile[i].timeLeft += 40;
                    return false;
                }
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.ObsidianRose, 1)
                .AddIngredient(ItemType<DriedRose>(), 2)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

    }
    public class SuperRoseProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 0;
            // DisplayName.SetDefault("PetalSpawner");     //The English name of the Projectile
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.CountsAsClass<MagicDamageClass>();

            Projectile.penetrate = -1;
            Projectile.timeLeft = 40;
            Projectile.tileCollide = false;

        }

        public virtual bool? CanHitNPC(Projectile projectile, NPC target)
        {
            return false;
        }
        // Note, this Texture is actually just a blank texture, FYI.

        readonly int fireRate = 20; 

        public override void AI()
        {
            if (Projectile.localAI[0] < fireRate)
            {
                Projectile.localAI[0] += 1 + Main.rand.NextFloat(0.05f, 0.15f);
            }

            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.Center;
            if (Projectile.localAI[0] >= fireRate)
            {
                Projectile.localAI[0] -= fireRate;
                int petal = ProjectileType<PetalFriendly>();
                int damage = Projectile.damage;
                if (Main.rand.NextBool(4))
                {
                    petal = ProjectileType<FirePetalFriendly>();
                    damage = (int)(damage * 1.25);
                }
                Vector2 position = new(Projectile.position.X + Main.rand.Next(-2, 2), Projectile.position.Y + Main.rand.Next(-2, 2));
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, Vector2.Zero, petal, damage, Projectile.knockBack, Projectile.owner, 0, 0);
            }

        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item20 with { MaxInstances = 0 }, Projectile.Center);
        }
    }
    public class PetalFriendly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            // DisplayName.SetDefault("Petal");     //The English name of the Projectile
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.CountsAsClass<MagicDamageClass>();

            Projectile.penetrate = 1;
            Projectile.timeLeft = 360;
            Projectile.tileCollide = false;

        }

        int frame = Main.rand.Next(0, 3);
        float maxSpin = Main.rand.NextFloat(4f, 5f);

        public override void AI()
        {

            Projectile.frame = frame;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            Player player = Main.player[Projectile.owner];


            if (Projectile.ai[0] < maxSpin)
            {
                Projectile.ai[0] += 1f / 60f;

                int radius = 75;
                Projectile.Center = player.Center + Vector2.One.RotatedBy(Projectile.ai[0] * Math.PI) * radius;
            }
            if (Projectile.ai[0] >= maxSpin)
            {
                NPC npc = null;
                if (TRAEMethods.ClosestNPC(ref npc, 700f, player.Center))
                {
                    float posX = npc.Center.X;
                    float posY = npc.Center.Y;
                    float speed = 8f;
                    float velX = posX - Projectile.Center.X;
                    float velY = posY - Projectile.Center.Y;
                    float sqrRoot = MathF.Sqrt(velX * velX + velY * velY);
                    sqrRoot = speed / sqrRoot;
                    velX *= sqrRoot;
                    velY *= sqrRoot;
                    Projectile.velocity.X = velX;
                    Projectile.velocity.Y = velY;
                    return;
                }
                else
                    Projectile.Kill();
            }
        }
    }
  
    public class FirePetalFriendly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            // DisplayName.SetDefault("Fire Petal");     //The English name of the Projectile
        }
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.CountsAsClass<MagicDamageClass>();
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.OnFire3;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuffDuration = 300;

            Projectile.penetrate = 1;
            Projectile.timeLeft = 360;
            Projectile.tileCollide = false;

        }
        int frame = Main.rand.Next(0, 3);
        float maxSpin = Main.rand.NextFloat(1.75f, 2.75f);
        public override void AI()
        {
            if (Main.rand.NextBool(8))
            {
                int num117 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width, Projectile.height, 6, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num117].noGravity = true;
                Main.dust[num117].velocity.X *= 1f;
                Main.dust[num117].velocity.Y *= 1f;
            }
            Projectile.frame = frame;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            Player player = Main.player[Projectile.owner];


            if (Projectile.ai[0] < maxSpin)
            {
                Projectile.ai[0] += 1f / 60f;

                int radius = 75;
                Projectile.Center = player.Center + Vector2.One.RotatedBy(Projectile.ai[0] * Math.PI) * radius;
            }
            if (Projectile.ai[0] >= maxSpin)
            {
                NPC npc = null;
                if (TRAEMethods.ClosestNPC(ref npc, 700f, player.Center))
                {
                    float posX = npc.Center.X;
                    float posY = npc.Center.Y;
                    float speed = 12f;
                    float velX = posX - Projectile.Center.X;
                    float velY = posY - Projectile.Center.Y;
                    float sqrRoot = MathF.Sqrt(velX * velX + velY * velY);
                    sqrRoot = speed / sqrRoot;
                    velX *= sqrRoot;
                    velY *= sqrRoot;
                    Projectile.velocity.X = velX;
                    Projectile.velocity.Y = velY;
                    return;
                }
                else
                    Projectile.Kill();
            }
        }
    }
  

    
}