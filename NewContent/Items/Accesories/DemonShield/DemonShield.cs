﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

using TRAEProject.NewContent.Items.Materials;

namespace TRAEProject.NewContent.Items.Accesories.DemonShield
{
    class DemonShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Demon Shield");
            Tooltip.SetDefault("Absorbs 25% of damage done to players on your team when above 25% life\nGrants immunity to knockback\n8% increased damage to nearby enemies");
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 66666; 
            Item.width = 32;          
            Item.height = 40;
            Item.defense = 8;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noKnockback = true;
            player.hasPaladinShield = true;
            player.GetModPlayer<DemonShieldEffect>().demonShield++;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient(ItemID.PaladinsShield, 1)
                .AddIngredient(ItemID.ObsidianShield, 1)
                .AddIngredient(ItemType<ObsidianScale>(), 3)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

    }
    public class DemonShieldEffect : ModPlayer
    {
        public int demonShield = 0;

        public override void ResetEffects()
        {
            demonShield = 0; 
        }
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (demonShield > 0)
            {
                if (target.Distance(Player.Center) <= 160f)
                {
                    damage = (int)(damage * 1.1 * demonShield);
                    for (int i = 0; i < damage / 20 + 5; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                        Dust d = Dust.NewDustPerfect(target.Center, DustID.ShadowbeamStaff, speed * 5, Scale: 1.5f);
                        d.noGravity = true;
                    }
                }
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (demonShield > 0)
            {
                if (target.Distance(Player.Center) <= 300f)
                {
                    damage = (int)(damage * 1.1 * demonShield);
                    for (int i = 0; i < damage / 20 + 5; i++)
                    {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                        Dust d = Dust.NewDustPerfect(target.Center, DustID.ShadowbeamStaff, speed * 5, Scale: 1.5f);
                        d.noGravity = true;
                    }
                }
            }    
        }
    }
}