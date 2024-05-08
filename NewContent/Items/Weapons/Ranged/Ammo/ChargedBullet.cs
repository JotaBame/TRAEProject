using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.Common;
using TRAEProject.Changes.Accesory;
using System;

namespace TRAEProject.NewContent.Items.Weapons.Ranged.Ammo
{
    public class ChargedBullet: ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Charged Bullet");
            // Tooltip.SetDefault("Jumps between hit enemies");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 20);
            Item.rare = ItemRarityID.Yellow;
            Item.width = 12;
            Item.height = 15;
            Item.shootSpeed = 4;
            Item.consumable = true;
            Item.shoot = ProjectileType<ChargedBulletShot>();
            Item.ammo = AmmoID.Bullet;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(500).AddIngredient(ItemID.EmptyBullet, 500)
                .AddIngredient(ItemType<GraniteBattery>(), 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class ChargedBulletShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("ChargedSHot");     //The English name of the Projectile

        }
        public override void SetDefaults()
        {
            AIType = ProjectileID.Bullet;
            Projectile.CloneDefaults(ProjectileID.Bullet);
            Projectile.GetGlobalProjectile<ScopeAndQuiver>().AffectedByReconScope = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().DamageFalloff = 0.33f;

            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 2;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hostile = false;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f); ;
            if (Projectile.ai[1] == 1)
            {

                Vector2 ProjectilePosition = Projectile.position;
                int dust = Dust.NewDust(ProjectilePosition, 1, 1, DustID.Electric, 0f, 0f, 0, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].position = ProjectilePosition;
                Main.dust[dust].velocity *= 0.2f;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.localNPCImmunity[target.whoAmI] = -1;
            target.immune[Projectile.owner] = 0;
            int[] array = new int[10];
            int num6 = 0;
            int num7 = 500;
            int num8 = 20;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].CanBeChasedBy(this, false) && Projectile.localNPCImmunity[Main.npc[j].whoAmI] != -1)
                {
                    float num9 = (Projectile.Center - Main.npc[j].Center).Length();
                    if (num9 > num8 && num9 < num7 && Collision.CanHitLine(Projectile.Center, 1, 1, Main.npc[j].Center, 1, 1))
                    {
                        array[num6] = j;
                        num6++;
                        if (num6 >= 9)
                        {
                            break;
                        }
                    }
                }
            }
            if (num6 > 0)
            {
                num6 = Main.rand.Next(num6);
                Vector2 value2 = Main.npc[array[num6]].Center - Projectile.Center;
                float scaleFactor2 = Projectile.velocity.Length();
                value2.Normalize();
                Projectile.velocity = value2 * scaleFactor2;
                Projectile.netUpdate = true;
                if (Projectile.ai[1] != 1)
                {
                    Projectile.timeLeft = 25;
                    Projectile.friendly = true;
                    Projectile.extraUpdates = 100;
                    Projectile.alpha = 255;
                    Projectile.ai[1] = 1;
					                    SoundEngine.PlaySound(SoundID.Item93 with { MaxInstances = 0 }, Projectile.position);            

                }
                return;
            }         
            Projectile.Kill();
            return;
        }
    }
}


