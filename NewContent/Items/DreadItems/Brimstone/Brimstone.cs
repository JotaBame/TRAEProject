using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject;
using Terraria.DataStructures;

namespace TRAEProject.NewContent.Items.DreadItems.Brimstone
{
    class Brimstone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Brimstone");
            Tooltip.SetDefault("Blood laser barrage");
        }
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.useStyle = 5;
            Item.damage = 60;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 125;
            Item.useTime = Item.useAnimation = 60;
            Item.reuseDelay = 45;
			Item.noMelee = true;
			Item.knockBack = 5f;
            Item.autoReuse = true;
            Item.shootSpeed = 4;
            Item.shoot = ProjectileType<BrimstoneBeam>();
            Item.SetShopValues(ItemRarityColor.LightPurple6, Item.sellPrice(0, 5));
            Item.UseSound = SoundID.NPCDeath13;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 54f; //This gets the direction of the flame projectile, makes its length to 1 by normalizing it. It then multiplies it by 54 (the item width) to get the position of the tip of the flamethrower.
            if (Collision.CanHit(position, 6, 6, position + muzzleOffset, 6, 6))
            {
                position += muzzleOffset;
            }
            // This is to prevent shooting through blocks and to make the fire shoot from the muzzle.
            return true;
        }
        public override Vector2? HoldoutOffset()
        // HoldoutOffset has to return a Vector2 because it needs two values (an X and Y value) to move your flamethrower sprite. Think of it as moving a point on a cartesian plane.
        {
            return new Vector2(0, -2); // If your own flamethrower is being held wrong, edit these values. You can test out holdout offsets using Modder's Toolkit.
        }
    }
    class BrimstoneBeam : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.penetrate = -1;
        }
        int beamLength = 1250;

        public const int chargeTime = 30;
        int timer = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if(player.itemTime > 1)
            {
                Projectile.timeLeft = 2;
                timer++;
                Projectile.velocity = Vector2.Zero;
                Projectile.Center = player.Center;
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.rotation = (Main.MouseWorld - player.Center).ToRotation();
                }
                beamLength = 1250;
                for (int i =0; i < beamLength; i++)
                {
                    if(!Collision.CanHitLine(Projectile.Center, 1, 1, Projectile.Center + TRAEMethods.PolarVector(i, Projectile.rotation), 1, 1))
                    {
                        beamLength = i;
                        break;
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Draw(Projectile, beamLength, timer);
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0;
            if (timer > chargeTime)
            {
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + TRAEMethods.PolarVector(beamLength, Projectile.rotation), 26, ref point);
            }
            return false;
        }
        public static void Draw(Projectile Projectile, int beamLength, int timer)
        {
            if (Projectile.timeLeft < chargeTime)
            {
                Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
                int frame = (timer / 6) % 4;
                float scaleY = timer;
                if(timer > 30)
                {
                    scaleY = 30;
                }
                scaleY /= 30f;
                for (float k = beamLength; k > 0; k -= texture.Width)
                {
                    Main.EntitySpriteDraw(texture, Projectile.Center + TRAEMethods.PolarVector(k, Projectile.rotation) - Main.screenPosition, new Rectangle(0, frame * 26, 32, 26), Color.White, Projectile.rotation + (float)Math.PI, Vector2.UnitY * 13, new Vector2(1f, scaleY), SpriteEffects.None, 0);
                }
            }
        }
    }
}
