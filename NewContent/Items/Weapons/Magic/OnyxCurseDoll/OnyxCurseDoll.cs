using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using TRAEProject.Changes;
using System;
using TRAEProject.Changes.Projectiles;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Changes.Items;

namespace TRAEProject.NewContent.Items.Weapons.Magic.OnyxCurseDoll
{
    class OnyxCurseDoll : ModItem
    {
        public const int DrainManaOnHit = 3;
        public const int DrainManaPassively = 50;
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Onyx Curse Doll");
            // Tooltip.SetDefault("Summons 3 fireballs to circle around you\nThe fireballs will drain 50 mana per second, affected by gear\nThey will curse nearby enemies, causing damage over time, lower damage or defense\nRight-click to uncast ");
        }
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 38;
            Item.damage = 38;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.mana = 30;

            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 2f;
            Item.shootSpeed = 8f;
            Item.noMelee = true;
            Item.shoot = ProjectileType<CurseDollWeaponflame>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item20;
            Item.GetGlobalItem<TRAEMagicItem>().TooltipDrainManaOnHit = DrainManaOnHit;
            Item.GetGlobalItem<TRAEMagicItem>().TooltipDrainManaPassively = DrainManaPassively;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, 0f);
        }
        public override bool AltFunctionUse(Player player)
        {
            for (int i = 0; i < 1000; i++)
            {
                if ((Main.projectile[i].type == ProjectileType<CurseDollWeaponflame>() || Main.projectile[i].type == ProjectileType<CurseDollArmorflame>() || Main.projectile[i].type == ProjectileType<CurseDollShadowflame>()) && Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI)
                {
                    Main.projectile[i].Kill();
                }
            }
            return false;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
         
            for (int i = 0; i < 1000; i++)
            {
                if ((Main.projectile[i].type == ProjectileType<CurseDollWeaponflame>() || Main.projectile[i].type == ProjectileType<CurseDollArmorflame>() || Main.projectile[i].type == ProjectileType<CurseDollShadowflame>()) && Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI)
                {
                    Main.projectile[i].Kill();
                }
            }
            //Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<CurseDollWeaponflame>(), damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<CurseDollShadowflame>(), damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ProjectileType<CurseDollArmorflame>(), damage, knockback, player.whoAmI);

            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.AncientCloth, 3)
                .AddIngredient(527, 2)
                .AddIngredient(ItemID.SoulofNight, 16)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class CurseDollWeaponflame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Curse Doll Weapon Flame");
        }
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 64;
            Projectile.scale = 1.25f;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Magic; 
            Projectile.GetGlobalProjectile<MagicProjectile>().DrainManaPassively = OnyxCurseDoll.DrainManaPassively;
            Projectile.GetGlobalProjectile<MagicProjectile>().DrainManaOnHit = OnyxCurseDoll.DrainManaOnHit;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.WitheredWeapon;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffDuration = 300;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1800;
            Projectile.tileCollide = false;
        }
       public float angletimer = 0;
        public int manaDrain = 0;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player.statMana <= 0)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if ((Main.projectile[i].type == ProjectileType<CurseDollArmorflame>() || Main.projectile[i].type == ProjectileType<CurseDollShadowflame>()) && Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI)
                    {
                        Main.projectile[i].Kill();
                    }
                }
                Projectile.Kill();
            }
            angletimer += 0.15f;
            if (angletimer > 360)
            {
                angletimer = 0;
            }
            Projectile.Center = player.Center + Vector2.One.RotatedBy(angletimer) * 140;
            Projectile.rotation = angletimer + 10;

            if (Main.rand.Next(3) == 0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 179, 1, Projectile.velocity.Y * -0.33f, 0, default, 0.7f);

            }
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.Center);
            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, 179, 1f);
                dust.noGravity = true;
            }
        }
    }
    public class CurseDollShadowflame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Curse Doll Shadowflame");
        }
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 64; Projectile.scale = 1.25f;

            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10; Projectile.GetGlobalProjectile<MagicProjectile>().DrainManaOnHit = 3;

            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.ShadowFlame;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffDuration = 300;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1800;
            Projectile.tileCollide = false;
        }
        public float angletimer = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner]; 

            angletimer += 0.15f;
            if (angletimer > 450)
            {
                angletimer = 0;
            }
            Projectile.Center = player.Center + Vector2.One.RotatedBy(90 + angletimer) * 140;
            Projectile.rotation = angletimer + 100;
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame, 1, Projectile.velocity.Y * -0.33f, 0, default, 0.7f);

            }
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.Center);
            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, DustID.Shadowflame, 1f);
                dust.noGravity = true;
            }
        }
    }
    public class CurseDollArmorflame : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Curse Doll Armor Flame");
        }
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 64; Projectile.scale = 1.25f;

            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10; Projectile.GetGlobalProjectile<MagicProjectile>().DrainManaOnHit = 3;

            Projectile.GetGlobalProjectile<ProjectileStats>().AddsBuff = BuffID.WitheredArmor;
            Projectile.GetGlobalProjectile<ProjectileStats>().AddedBuffDuration = 300;
                Projectile.penetrate = -1;
            Projectile.timeLeft = 1800;
            Projectile.tileCollide = false;
        }
        public float angletimer = 0;

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
 
            angletimer += 0.15f;
            if (angletimer > 540)
            {
                angletimer = 0f;
            }
            Projectile.Center = player.Center + Vector2.One.RotatedBy(180 + angletimer) * 140;
            Projectile.rotation = angletimer + 190;
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 21, 1, Projectile.velocity.Y * -0.33f, 0, default, 0.7f);

            }
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10 with { MaxInstances = 0 }, Projectile.Center);
            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.oldPosition, Projectile.width, Projectile.height, 21, 1f);
                dust.noGravity = true;
            }
        }
    }
}