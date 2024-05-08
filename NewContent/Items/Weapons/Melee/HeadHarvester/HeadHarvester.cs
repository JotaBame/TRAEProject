using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Prefixes;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Weapons.Melee.HeadHarvester
{
    class HeadHarvester : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Head Harvester");
        }
        public override void SetDefaults()
        {
            Item.width = 58;
            Item.height = 60;
            Item.damage = 94;
            Item.useTime = 38;
            Item.useAnimation = 38;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 10);
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.shoot = ProjectileType<FlamingScythe>();
            Item.knockBack = 4f;
            Item.shootSpeed = 2f;
            Item.useStyle = 1;
            Item.scale = 1.35f;
            Item.UseSound = SoundID.Item71;
        }    
		public override float UseAnimationMultiplier(Player player)
        {
			return 1 / player.GetAttackSpeed(DamageClass.Melee);
        }
    }
    class FlamingScythe : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 106; Projectile.height = 84;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 8;
            Projectile.timeLeft = 300;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
        }
       

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.ai[0]++;
            if (Projectile.ai[0] > 25f)
            {
                if (Projectile.localAI[0] == 0f)
                {
                    Projectile.localAI[0] = 1f;
                    Projectile.spriteDirection = -(int)Projectile.ai[1];
                }
                if (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y) < 16f)
                {
                    Projectile.velocity *= 1.033f;
                }
                if (Projectile.velocity.X < 0f)
                {
                    Projectile.direction = -1;
                }
                else
                {
                    Projectile.direction = 1;
                }
            }
            Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.02f * (float)Projectile.direction;

        }
    }
}
