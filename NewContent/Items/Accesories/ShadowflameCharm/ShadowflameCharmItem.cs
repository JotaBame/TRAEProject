using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using TRAEProject.NewContent.Items.Weapons.Summoner.Whip;
using TRAEProject.Common;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Accesories.ShadowflameCharm
{
    public class ShadowflameCharmItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1; 
            // DisplayName.SetDefault("Shadowflame Charm");
            // Tooltip.SetDefault("Minion damage is stored as Shadowflame energy, up to 3000\nWhip strikes spawn a friendly Shadowflame Apparition for every 750 damage stored");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.width = 36;
            Item.height = 44;
            Item.value = Item.sellPrice(gold:3);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ShadowflameCharmPlayer>().ShadowflameCharm += 1;
        }
    }
    public class ShadowflameCharmPlayer : ModPlayer
    {
        public int ShadowflameCharm = 0;
        public int ShadowflameCharmCharge = 0;
		public int ShadowflameCharmLimit = 3000;
        public int MoltenCharm = 0;
        public int MoltenCharmCharge = 0;
        public int MoltenCharmLimit = 3000;

        public override void ResetEffects()
        {
            ShadowflameCharm = 0;
			ShadowflameCharmLimit = 3000;
            MoltenCharm = 0;
			MoltenCharmLimit = 3000;
        }
        public override void UpdateDead()
        {
            ShadowflameCharm = 0;
            ShadowflameCharmCharge = 0;
            MoltenCharm = 0;
            MoltenCharmCharge = 0;
        }

        public override void PostUpdate()
        {
			if (ShadowflameCharmCharge > ShadowflameCharmLimit)
            {
                ShadowflameCharmCharge = ShadowflameCharmLimit;
            }
            if (MoltenCharmCharge > MoltenCharmLimit)
            {
                MoltenCharmCharge = MoltenCharmLimit;
            }
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.minion || ProjectileID.Sets.MinionShot[proj.type])
            {
                ShadowflameCharmCharge += damageDone * ShadowflameCharm;
                MoltenCharmCharge += damageDone * MoltenCharm;
            }
            if (ProjectileID.Sets.IsAWhip[proj.type] || proj.type == ProjectileType<WhipProjectile>())
            {
                if (ShadowflameCharmCharge > 750)
                {
                    Player player = Main.player[proj.owner];
                    for (int i = 0; i < ShadowflameCharmCharge / 750; ++i)
                    {
                        int direction = Main.rand.NextFromList(-1, 1);
                        float k = Main.screenPosition.X;
                        if (direction < 0)
                        {
                            k += (float)Main.screenWidth;
                        }
                        float y2 = Main.screenPosition.Y;
                        y2 += (float)Main.rand.Next(Main.screenHeight);
                        Vector2 vector = new Vector2(k, y2);
                        float num2 = target.Center.X - vector.X;
                        float num3 = target.Center.Y - vector.Y;
                        num2 += (float)Main.rand.Next(-50, 51) * 0.1f;
                        num3 += (float)Main.rand.Next(-50, 51) * 0.1f;
                        float num4 = MathF.Sqrt(num2 * num2 + num3 * num3);
                        num4 = 24f / num4;
                        num2 *= num4;
                        num3 *= num4;
                        Projectile.NewProjectile(player.GetSource_FromThis(), k, y2, num2, num3, ProjectileType<ShadowflameApparition>(), 50 * ShadowflameCharm, 0f, player.whoAmI);
                        ShadowflameCharmCharge -= 750;
                    }
                }
                if (MoltenCharmCharge > 750)
                {
                    for (int i = 0; i < MoltenCharmCharge / 750; ++i)
                    {
                        Player player = Main.player[proj.owner];
                        int direction = player.direction;
                        float k = Main.screenPosition.X;
                        if (direction < 0)
                        {
                            k += (float)Main.screenWidth;
                        }
                        float y2 = Main.screenPosition.Y;
                        y2 += (float)Main.rand.Next(Main.screenHeight);
                        Vector2 vector = new Vector2(k, y2);
                        float num2 = target.Center.X - vector.X;
                        float num3 = target.Center.Y - vector.Y;
                        num2 += (float)Main.rand.Next(-50, 51) * 0.1f;
                        num3 += (float)Main.rand.Next(-50, 51) * 0.1f;
                        float num4 = MathF.Sqrt(num2 * num2 + num3 * num3);
                        num4 = 24f / num4;
                        num2 *= num4;
                        num3 *= num4;
                        Projectile.NewProjectile(player.GetSource_FromThis(), k, y2, num2, num3, ProjectileType<MoltenApparition>(), 50 + 25 * MoltenCharm, 0f, player.whoAmI);
                        MoltenCharmCharge -= 750;
                    }
                }    
            }
            return;
        }
    }
    public class ShadowflameApparition : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
            // DisplayName.SetDefault("ShadowflameApparition");     //The English name of the Projectile
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.light = 0.1f;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.GetGlobalProjectile<ProjectileStats>().homesIn = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.ShadowFlame;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffDuration = 240;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() +MathHelper.ToRadians(90f);
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 6;
            }
        }
    }
    public class MoltenApparition : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
            // DisplayName.SetDefault("ShadowflameApparition");     //The English name of the Projectile
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.light = 0.1f;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;  
			Projectile.GetGlobalProjectile<ProjectileStats>().homesIn = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().explodes = true;
            Projectile.GetGlobalProjectile<ProjectileStats>().ExplosionRadius = 160;            
			Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.Daybreak;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuffChance = 2;

            Projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffDuration = 60;
        }


        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 6;
            }
        }
    }
}
