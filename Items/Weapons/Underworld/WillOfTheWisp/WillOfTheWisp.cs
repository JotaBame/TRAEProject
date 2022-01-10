using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Projectiles;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Items.Weapons.Underworld.WillOfTheWisp
{
    class WillOfTheWisp : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Will O' Wisp");
            Tooltip.SetDefault("Shoots a smart bouncing fireball");
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.damage = 24;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.mana = 150;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 5);
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 2f;
            Item.shootSpeed = 16f;
            Item.noMelee = true;
            Item.shoot = ProjectileType<WillOfTheWispFlame>();
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item20;
        }
        public override bool Shoot(Player player, ProjectileSource_Item_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].type == ProjectileType<WillOfTheWispFlame>() && Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI)
                {
                    Main.projectile[i].Kill();
                }
            }
            return true;
        }
    }
    public class WillOfTheWispFlame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Will O' Wisp Flame");
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;

            Projectile.DamageType = DamageClass.Magic; 
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.penetrate = 10;
            Projectile.GetGlobalProjectile<TRAEGlobalProjectile>().SmartBouncesOffEnemies = true;
            Projectile.aiStyle = 1;
            Projectile.timeLeft = 600;
        }
        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (Main.rand.Next(3) == 0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 1, Projectile.velocity.Y * -0.33f, 0, default, 0.7f);

            }
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, (int)Projectile.position.X, (int)Projectile.position.Y);
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, DustID.Torch, 1f);
                dust.noGravity = true;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            int[] numArray = new int[10];
            int maxValue = 0;
            int num2 = 700;
            int num3 = 20;
            for (int index2 = 0; index2 < 200; ++index2)
            {
                if (Main.npc[index2].CanBeChasedBy((object)this, false))
                {
                    Vector2 vector2 = Vector2.Subtract(Projectile.Center, Main.npc[index2].Center);
                    // ISSUE: explicit reference operation
                    float num4 = ((Vector2)@vector2).Length();
                    if ((double)num4 > (double)num3 && (double)num4 < (double)num2 && Collision.CanHitLine(Projectile.Center, 1, 1, Main.npc[index2].Center, 1, 1))
                    {
                        numArray[maxValue] = index2;
                        ++maxValue;
                        if (maxValue >= 9)
                            break;
                    }
                }
            }
            if (maxValue > 0)
            {
                int index2 = Main.rand.Next(maxValue);
                Vector2 vector2 = Vector2.Subtract(Main.npc[numArray[index2]].Center, Projectile.Center);
                // ISSUE: explicit reference operation
                float num4 = ((Vector2)Projectile.velocity).Length();
                // ISSUE: explicit reference operation
                ((Vector2)@vector2).Normalize();
                Projectile.velocity = Vector2.Multiply(vector2, num4);
                Projectile.netUpdate = true;
                return false;
            }
            else
            {
                Projectile.velocity.Y = -Projectile.oldVelocity.Y;
                return false;
            }
          
        }
    }   
}