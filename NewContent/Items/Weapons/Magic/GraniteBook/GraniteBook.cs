using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Items;
using TRAEProject.NewContent.Items.Materials;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Weapons.Magic.GraniteBook
{
    class GraniteBook : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Electro Shield");
            // Tooltip.SetDefault("Summons an electric ring around you\nDrains 30 mana per second, affected by gear");
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.damage = 60;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.mana = 30;
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.sellPrice(gold: 5);
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 2.5f;
            Item.shootSpeed = 8f;
            Item.noMelee = true;
            Item.shoot = ProjectileType<ElectricRIng>();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item20; 
            Item.GetGlobalItem<TRAEMagicItem>().rightClickSideWeapon = true;
            Item.GetGlobalItem<TRAEMagicItem>().TooltipDrainManaPassively = 30;

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2f, 0f);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
         
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].type == ProjectileType<ElectricRIng>() && Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI)
                {
                    Main.projectile[i].Kill();
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.SpellTome, 1)
                .AddIngredient(ItemType<GraniteBattery>(), 2)
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
    public class ElectricRIng : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Electricity Ring");
        }
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1800;
            Projectile.tileCollide = false;
        }
       public float angletimer = 0;
        public int manaDrain = 0;
        public int attackDelay = 12;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.Center;
            manaDrain += (int)(30 * player.manaCost);
            if (manaDrain >= 60)
            {
                manaDrain -= 60;
                player.statMana--;
            }
            if (player.statMana <= 0)
            {
                Projectile.Kill();
            }
            int dusts = 7;
            int NPCLimit = 0;
            int Range = 250;
            int damage = Projectile.damage;
            float dustScale = 1.25f;

            Projectile.ai[0]++;
            if (Projectile.ai[0] >= attackDelay)
            {
                Projectile.ai[0] = 0;

                for (int k = 0; k < 200; k++)
                {
                    NPC nPC = Main.npc[k];
                    if (nPC.active && !nPC.friendly && nPC.damage > 0 && !nPC.dontTakeDamage && Vector2.Distance(Projectile.Center, nPC.Center) <= Range)
                    {
                        
                        if (NPCLimit < 3)
                        {
							++NPCLimit;
                            player.ApplyDamageToNPC(nPC, damage, Projectile.knockBack, nPC.direction * -1, crit: false);
                            SoundEngine.PlaySound(SoundID.Item93 with { MaxInstances = 0 }, nPC.position);
                        }
                    }
                }

            }
            for (int i = 0; i < dusts; i++)
            {
                Vector2 spawnPos = Projectile.Center + Main.rand.NextVector2CircularEdge(Range, Range);
                Vector2 offset = spawnPos - Projectile.Center;
                if (Math.Abs(offset.X) > Main.screenWidth * 0.6f || Math.Abs(offset.Y) > Main.screenHeight * 0.6f) //dont spawn dust if its pointless
                    continue;
                Dust dust = Main.dust[Dust.NewDust(spawnPos, 0, 0, DustID.Electric, 0, 0, 100, default, dustScale)];
                dust.velocity = player.velocity;
                if (Main.rand.NextBool(5))
                {
                    dust.velocity += Vector2.Normalize(Projectile.Center - dust.position) * Main.rand.NextFloat(5f);
                    dust.position += dust.velocity * 5f;
                }
                dust.noGravity = true;
            }
        }
    }
}