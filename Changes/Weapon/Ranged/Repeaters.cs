using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Common;

namespace TRAEProject.Changes.Weapon.Ranged
{
    public class Repeaters : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.CobaltRepeater:
                    item.damage = 26;
                    item.useTime = item.useAnimation = 19;
                    item.crit = 24;
                    item.SetNameOverride("Cobalt Yumi");
                    break;
                case ItemID.PalladiumRepeater:
                    item.damage = 34;
                    item.useTime = item.useAnimation = 19;
                    item.SetNameOverride("Palladium Pinaka");
                    break;
                case ItemID.MythrilRepeater:
                    item.damage = 96;
                    item.useTime = item.useAnimation = 42;
                    item.autoReuse = false;
                    item.scale = 1.8f;
                    item.shootSpeed = 16f;
                    item.SetNameOverride("Mythril Ballista");
                    break;
                case ItemID.OrichalcumRepeater:
                    item.damage = 36;
                    item.useTime = item.useAnimation = 19;
                    item.SetNameOverride("Orichalcum Crossbow");
                    break;
                case ItemID.TitaniumRepeater:
                    item.damage = 30;
                    item.useTime = item.useAnimation = 25;
                    item.SetNameOverride("Titanium Obliterator");
                    break;
                case ItemID.HallowedRepeater:
                    item.damage = 53;
                    item.useTime = item.useAnimation = 16;
                    item.SetNameOverride("Sharanga");
                    break;
                case ItemID.ChlorophyteShotbow:
                    item.useTime = item.useAnimation = 24;
                    break;
            }
        }
        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            base.ModifyShootStats(item, player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.type == ItemID.TitaniumRepeater)
            {
                int count = Main.rand.Next(3) + 3;
                for (int i = 0; i < count; i++)
                {
                    Projectile.NewProjectile(player.GetSource_ItemUse(item), position, (velocity * 1f).RotatedByRandom(MathF.PI / 10), ProjectileType<TitaniumShrapnel>(), damage / 4, 0, player.whoAmI);
                }
            }
            if (item.type == ItemID.MythrilRepeater)
            {
                Projectile arrow = Main.projectile[Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI)];
                arrow.scale *= item.scale;
                arrow.GetGlobalProjectile<ProjectileStats>().heavyCritter = true;
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.MythrilRepeater:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Knockback")
                        {
                            line.Text += "\nDeals 25% more damage on critical hits";
                        }
                    }
                    break;
            }
        }
    }
        public class RepeaterHits : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(projectile.arrow)
            {
                Player player = Main.player[projectile.owner];
                switch (player.HeldItem.type)
                {
                    case ItemID.PalladiumRepeater:

                            player.AddBuff(BuffID.RapidHealing, 300);
                        return;
                    case ItemID.OrichalcumRepeater:
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
                        Projectile.NewProjectile(player.GetSource_FromThis(), k, y2, num2, num3, 221, 20, 0f, player.whoAmI);
                        return;
                }
            }
        }
    }
    public class TitaniumShrapnel : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.width = Projectile.height = 10;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.timeLeft = 30;
            Projectile.ArmorPenetration = 15;
            Projectile.extraUpdates = 1;
            Projectile.scale = 1.35f;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, DustID.Titanium, 0.75f);
                dust.noGravity = true;
            }
        }
    }
}
