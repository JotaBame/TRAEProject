﻿using Microsoft.Xna.Framework;
using System;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Changes;

namespace TRAEProject.Common.ModPlayers
{
    public class CritDamage : ModPlayer
    {
        public float critDamage = 0f;
        public float meleeCritDamage = 0f;
        public float rangedCritDamage = 0f;
        public float magicCritDamage = 0f;
        public float arrowCritDamage = 0f; 
        public override void ResetEffects()
        {
            critDamage = 0f;
            meleeCritDamage = 0f;
            rangedCritDamage = 0f; 
            magicCritDamage = 0f;
            arrowCritDamage = 0f;
        }
        public override void UpdateDead()
        {
            critDamage = 0f;
            meleeCritDamage = 0f;
            rangedCritDamage = 0f;
            magicCritDamage = 0f;
            arrowCritDamage = 0f;
        }
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {   
            if (crit)
            {
                float multiplier = critDamage + meleeCritDamage;
                damage += (int)(damage * multiplier);
            }
                
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (crit)
            {
                float multiplier = critDamage + proj.GetGlobalProjectile<ProjectileStats>().CritDamage;
                if (proj.CountsAsClass(DamageClass.Melee))
                {
                    multiplier += meleeCritDamage;
                }
                if (proj.CountsAsClass(DamageClass.Ranged))
                {
                    if (proj.arrow)
                    {
                        multiplier += arrowCritDamage;
                    }
                    multiplier += rangedCritDamage;
                }
                if (proj.CountsAsClass(DamageClass.Magic))
                {
                    multiplier += magicCritDamage;
                }
                damage += (int)(damage * multiplier);

            }

        }
    }
}