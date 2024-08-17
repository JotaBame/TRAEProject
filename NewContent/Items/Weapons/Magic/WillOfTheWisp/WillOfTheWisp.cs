using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Projectiles;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Changes.Items;
using TRAEProject;using TRAEProject.Common;

using System.Collections.Generic;
namespace TRAEProject.NewContent.Items.Weapons.Magic.WillOfTheWisp
{
    public class WillOfTheWisp : ModItem
    {
        public const int DrainManaOnHit = 10;
        public const int DrainManaPassively = 0;
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Will O' Wisp");
            // Tooltip.SetDefault("Shoots a smart bouncing fireball");
        }
        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 42;
            Item.damage = 25;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.mana = 15;
            Item.GetGlobalItem<TRAEMagicItem>().rightClickSideWeapon = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(gold: 5);
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 2f;
            Item.shootSpeed = 8f;
            Item.noMelee = true;
            Item.shoot = ProjectileType<WillOfTheWispFlame>();
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item20;
            Item.GetGlobalItem<TRAEMagicItem>().TooltipDrainManaOnHit = DrainManaOnHit;
            Item.GetGlobalItem<TRAEMagicItem>().TooltipDrainManaPassively = DrainManaPassively;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
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
            // DisplayName.SetDefault("Will O' Wisp Flame");
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
            Projectile.GetGlobalProjectile<MagicProjectile>().DrainManaOnHit = WillOfTheWisp.DrainManaOnHit;            
            Projectile.penetrate = -1;
            Projectile.aiStyle = 1;
            AIType = ProjectileID.Bullet;
            Projectile.timeLeft = 600;
            Projectile.extraUpdates = 1;
			                    Projectile.GetGlobalProjectile<ProjectileStats>().BouncesOffTiles = true;

            Projectile.tileCollide = true;
        }
        int timeBeforebounce = 16;
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= timeBeforebounce)
            {
                for (int i = 0; i < 8; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X, Projectile.velocity.Y);
                    dust.noGravity = true;
                }
                Projectile.ai[0] = 0;
                if(TRAEMethods.NPCsInRange(out List<NPC> targets, 250, Projectile.Center, false, delegate(NPC possibleTarget){ return (possibleTarget.Center - Projectile.Center).Length() > 20;}))
                {
                    int index = Main.rand.Next(targets.Count);
                    Vector2 value2 = targets[index].Center - Projectile.Center;
                    float scaleFactor2 = 8f;
                    value2.Normalize();
                    Projectile.velocity = value2 * scaleFactor2;
                    Projectile.netUpdate = true;
                }
                return;
            }
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
        public override void OnKill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.Center);
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, DustID.Torch, 1f);
                dust.noGravity = true;
            }
        }
    }   
}