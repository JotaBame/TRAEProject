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

namespace TRAEProject.NewContent.Items.Weapons.Magic.SharpLament
{
    class SharpLament : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.DefaultToStaff(ProjectileType<FlamingWood>(), 12, 12, 19);
            Item.width = 38;
            Item.height = 32;
            Item.damage = 135;
			Item.crit = 17;
            Item.scale = 1.1f;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 10);
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item42;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(8));
        }
    }
    class FlamingWood : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.FlamingWood);
            AIType = ProjectileID.FlamingWood;
			Projectile.hostile = false;
			Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 4;
            Projectile.scale = 0.9f;
			Projectile.tileCollide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.GetGlobalProjectile<Common.ProjectileStats>().DamageFalloff = 0.15f;
           
        }
        
    }
}
