using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Weapons.Magic.MagicGrenade
{
    class MagicGrenade : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.DefaultToStaff(ProjectileType<MagicGrenadeP>(), 5.75f, 55, 200);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.width = 14;
            Item.height = 20;
            Item.damage = 75;
            Item.crit = 16;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 80);
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;

        }
    }
    class MagicGrenadeP : ModProjectile
    {

        public override void SetDefaults()
        {
            Projectile.friendly = true; 
            Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 14;
		    Projectile.height = 20;
            Projectile.timeLeft = 110;
			Projectile.penetrate = 5;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.GetGlobalProjectile<ProjectileStats>().explodes = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().ExplosionRadius = 200;
            Projectile.GetGlobalProjectile<ProjectileStats>().dontExplodeOnTiles = true;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.ai[0] == 75f)
            {
                modifiers.SetCrit();
            }
        }

        public override void AI()
        {
            if (Main.rand.NextBool(2)) {
                int num31 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.PinkTorch, Scale: 0.9f);
                Main.dust[num31].scale *= 1f + Main.rand.Next(10) * 0.1f;
                Main.dust[num31].velocity *= 0.2f;
                Main.dust[num31].noGravity = true;
            }
            

            Player player = Main.player[Projectile.owner];
            Projectile.rotation += Projectile.rotation += Projectile.velocity.X * 0.1f;
            ++Projectile.ai[0];
            if (Projectile.ai[0] > 15f)
            {
                Projectile.velocity.Y += 0.2f;
                Projectile.velocity.X *= 0.98f;

            }
            if (Projectile.ai[0] == 75f)
            {
                Projectile.GetGlobalProjectile<ProjectileStats>().ExplosionRadius = 300;

                Projectile.velocity.Y = -1.5f;
            }
            if (Projectile.ai[0] >= 75f)
            {
                for (int num847 = 0; num847 < 2; num847++)
                {
                    Dust dust53 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror, 0f, 0f, 100, default, 1f);
                }
                Projectile.velocity.X *= 0.95f;
                Projectile.velocity.Y -= 0.275f;

            }



            if (Projectile.timeLeft < 3)
            {
                SoundEngine.PlaySound(SoundID.Item29 with { MaxInstances = 0 }, Projectile.position);
                float num846 = 3f;
                for (int num847 = 0; num847 < 30; num847++)
                {
                    Dust dust53 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror, 0f, 0f, 100, default, 1f);
                    dust53.velocity = (dust53.position - Projectile.Center).SafeNormalize(Vector2.Zero);
                    Dust dust = dust53;
                    dust.velocity *= 1f + (float)Main.rand.Next(5) * 0.1f;
                    dust53.velocity.Y -= num846 * 0.5f;
                    dust53.color = Color.Black * 0.9f;
                    if (Main.rand.NextBool(2))
                    {
                        dust53.scale = 0.5f;
                        dust53.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                        dust53.color = Color.Blue * 0.8f;
                    }
                }
                for (int num848 = 0; num848 < 30; num848++)
                {
                    int DustType = DustID.YellowTorch;
                    if (num848 % 3 != 0)
                    {
                        DustType = num848 % 2 == 0 ? DustID.BlueTorch : DustID.PinkTorch;
                    }
                    Dust dust54 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustType, 0f, 0f, 100);
                    dust54.noGravity = true;
                    dust54.fadeIn = 1.4f;
                    dust54.velocity = (dust54.position - Projectile.Center).SafeNormalize(Vector2.Zero);
                    Dust dust = dust54;
                    dust.velocity *= 1.25f + (float)Main.rand.Next(61) * 0.1f;
                    dust54.velocity.Y -= num846 * 0.5f;
                    dust54 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 6, 0f, 0f, 100);
                    dust54.velocity = (dust54.position - Projectile.Center).SafeNormalize(Vector2.Zero);
                    dust54.velocity.Y -= num846 * 0.25f;
                    dust = dust54;
                    dust.velocity *= 1f + (float)Main.rand.Next(5) * 0.1f;
                    dust54.fadeIn = 0f;
                    dust54.scale = 0.6f;
                    dust54.fadeIn = 1.2f;
                    if (!dust54.noGravity)
                    {
                        dust54.scale = 0.4f;
                        dust54.fadeIn = 0f;
                    }
                    else
                    {
                        dust = dust54;
                        dust.velocity *= 1.2f + (float)Main.rand.Next(5) * 0.2f;
                        dust54.velocity.Y -= num846 * 0.5f;
                    }
                }
            }
		}
	}
}