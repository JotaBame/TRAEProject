using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject;
using TRAEProject.Changes.Prefixes;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Items.DreadItems.ShellSpinner
{
    class ShellSpinner : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shell Spinner");
            Tooltip.SetDefault("Spin to win!");
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 32;
            Item.damage = 106;
            Item.useTime = Item.useAnimation = 30;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.sellPrice(gold: 5);
            Item.DamageType = DamageClass.Melee;
            Item.shoot = ProjectileType<ShellSpinnerP>();
            Item.knockBack = 2f;
            Item.shootSpeed = 10;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = 1;
            Item.UseSound = SoundID.Item1;
            Item.GetGlobalItem<GiveWeaponsPrefixes>().canGetBoomerangModifers = true;
        }
    }
    class ShellSpinnerP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 38;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.friendly = true;
        }
        private float speed;
        private float maxSpeed;
        private bool runOnce = true;
        private float decceleration = 1f / 4f;
        private int spinDirection;
        private bool returnToPlayer;

        NPC spinAround = null;
        int timer = 0;
        float rotDistance = 180;

        public override void AI()
        {
            Projectile.friendly = Projectile.penetrate > 1;
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                spinDirection = player.direction;
                speed = Projectile.velocity.Length();
                maxSpeed = speed;
                runOnce = false;
            }
            Projectile.rotation += MathHelper.ToRadians(maxSpeed * spinDirection);
            if (returnToPlayer)
            {
                if (Collision.CheckAABBvAABBCollision(player.position, player.Size, Projectile.position, Projectile.Size))
                {
                    Projectile.Kill();
                }
                Projectile.tileCollide = false;
                Projectile.velocity = TRAEMethods.PolarVector(speed, (player.Center - Projectile.Center).ToRotation());
                speed += decceleration;
                if (speed > maxSpeed)
                {
                    speed = maxSpeed;
                }
            }
            else
            {
                if (spinAround == null || !spinAround.active)
                {
                    Projectile.velocity = Projectile.velocity.SafeNormalize(-Vector2.UnitY) * speed;
                    speed -= decceleration;
                    if (speed < 1f)
                    {
                        returnToPlayer = true;
                    }
                }
                else
                {
                    timer--;
                    if (timer > 0)
                    {
                        Projectile.friendly = false;
                    }
                    else
                    {
                        Projectile.friendly = true;
                    }
                    float direction = (spinAround.Center - Projectile.Center).ToRotation() + (float)Math.PI / 2f;
                    float rotMore = ((rotDistance - Projectile.Center.Distance(spinAround.Center)) / 300f);
                    if (Math.Abs(rotMore) > 1)
                    {
                        rotMore = Math.Sign(rotMore);
                    }
                    rotMore *= (float)Math.PI / 4;
                    direction += rotMore;
                    if(timer < -120)
                    {
                        rotDistance--;
                    }
                    Projectile.velocity = TRAEMethods.PolarVector(maxSpeed, direction);
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if(spinAround == null && !returnToPlayer)
            {
                Projectile.friendly = false;
                Projectile.tileCollide = false;
                timer = 30;
                spinAround = target;
            }
            else
            {
                returnToPlayer = true;
                Projectile.velocity = Vector2.Zero;
                Projectile.damage = (int)(Projectile.damage * 0.8f);
                speed = 0f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            returnToPlayer = true;
            return false;
        }
    }
}
