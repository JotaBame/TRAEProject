using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Creative;
using TRAEProject.Common;
using TRAEProject.NewContent.Items.Materials;
using TRAEProject.NewContent.TRAEDebuffs;

namespace TRAEProject.NewContent.Items.Weapons.Ranged.Ammo
{
    public class BeetleArrow: ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Beetle Arrow");
            // Tooltip.SetDefault("25% chance to stun and create a damaging shockwave");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 99;
        }
        public override void SetDefaults()
        {
            Item.damage = 12;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = Item.sellPrice(0, 0, 0, 50);
            Item.rare = ItemRarityID.Yellow;
            Item.width = 14;
            Item.height = 38;
            Item.shootSpeed = 4;
            Item.consumable = true;
            Item.shoot = ProjectileType<BeetleArrowShot>();
            Item.ammo = AmmoID.Arrow;
            Item.maxStack = 9999;
        }

        public override void AddRecipes()
        {
            CreateRecipe(50).AddIngredient(ItemID.WoodenArrow, 50)
                .AddIngredient(ItemID.BeetleHusk, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class BeetleArrowShot: ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Beetle Arrow");     //The English name of the Projectile

        }
        public override void SetDefaults()
        {
            AIType = ProjectileID.WoodenArrowFriendly;
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
            Projectile.timeLeft = 1200;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hostile = false;
            Projectile.friendly = true;
        }
        public override void OnKill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, DustID.PurpleMoss, 0.33f);
                dust.noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            if (Main.rand.NextBool(4))
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item89 with { MaxInstances = 0 }, Projectile.position);

                int duration = Main.rand.Next(50, 80);
                target.GetGlobalNPC<Stun>().StunMe(target, duration);
                for (int i = 0; i < 60; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(4.8f, 4.8f);
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.WhiteTorch, speed * 5, Scale: 2f);
                    d.noGravity = true;
                }

                int NPCLimit = 0;
                float range = 200f;
                for (int k = 0; k < 200; k++)
                {
                    NPC nPC = Main.npc[k];
                    if (nPC != target && nPC.active && !nPC.friendly && nPC.damage > 0 && !nPC.dontTakeDamage && Vector2.Distance(target.Center, nPC.Center) <= range)
                    {
                        ++NPCLimit;
                        damageDone /= 2;
                        if (NPCLimit < 5)
                        {
                            float finalDefense = nPC.defense - player.GetArmorPenetration(DamageClass.Generic);
                            nPC.ichor = false;
                            nPC.betsysCurse = false;
                            if (finalDefense < 0)
                            {
                                finalDefense = 0;
                            }
                            if (finalDefense > 100)
                            {
                                finalDefense = 100;
                            }
                            damageDone += (int)finalDefense / 2;
                            duration /= 2;
                            nPC.GetGlobalNPC<Stun>().StunMe(target, duration);
                            player.ApplyDamageToNPC(nPC, damageDone, hit.Knockback * 0.5f, 0, crit: false);

                        }
                    }
                }
            }
        }
    }
}


