

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject;
using System.Collections.Generic;
using TRAEProject.Changes.Items;
using TRAEProject.Changes.Armor;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;

namespace ChangesArmor
{
    public class ShroomiteArmor : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.ShroomiteHeadgear:
                item.defense = 6;
                break;
                case ItemID.ShroomiteHelmet:
                item.defense = 16;
                break;
            }
        }
        public override void UpdateEquip(Item item, Player player)
        {
            switch (item.type)
            {
                case ItemID.ShroomiteBreastplate:
                player.GetDamage(DamageClass.Ranged) += -0.03f;
                player.GetCritChance(DamageClass.Ranged) += -3f;
                break;
                case ItemID.ShroomiteMask:
                    //negate vanilla bonus
                    player.bulletDamage += -0.15f;
                    player.GetCritChance(DamageClass.Ranged) += -5f;

         
                //new Bonuses
                player.GetDamage(DamageClass.Ranged) += 0.15f;
                player.GetCritChance(DamageClass.Ranged) += 5f;
                player.moveSpeed += 0.05f;
                break;
                case ItemID.ShroomiteHeadgear:
                    //negate vanilla bonus
                    player.arrowDamage += -0.15f;
                    player.GetCritChance(DamageClass.Ranged) += -5f;

                    //new Bonuses
                    player.GetCritChance(DamageClass.Ranged) += 28f;
                break;
                case ItemID.ShroomiteHelmet:
                //negate vanilla bonus
                player.specialistDamage += -0.15f;
                player.GetCritChance(DamageClass.Ranged) += -5f;

                //new Bonuses
                player.GetDamage(DamageClass.Ranged) += 0.10f;
                player.noKnockback = true;
                break;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.ShroomiteBreastplate:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            //line.Text.Replace("13%", "10%");
                            line.Text = "10% increased ranged damage and critical strike chance";
                        }
                    }
                break;
                case ItemID.ShroomiteMask:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "15% increased ranged damage";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "5% increased ranged critical strike chance and movement speed";
                        }
                    }
                break;
                case ItemID.ShroomiteHeadgear:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "28% increased ranged critical strike chance";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "";
                        }
                    }
                break;
                case ItemID.ShroomiteHelmet:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "10% increased ranged damage";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "Provides immunity to knockback";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = "";
                        }
                    }
                break;
            }
        }
    }
    public class ShroomiteEffects : ModPlayer
    {
        public float ShroomDodge = 0;
        public float ShroomIframes = 0;
        int noRangedTimer = 0;
        int headgearStartStealthTime = 15;
        int timeToMax = 120 - 15;
        float maxHeadDamage = 3f;
        public float damageMult = 1f;
        float traeStealth = 0f;
        bool maskSet = false;
        int shroomDodgeAnim = 0;
        public override void PostUpdateEquips()
        {

            maskSet = false;
            ShroomDodge = 0;
            ShroomIframes = 0;
            if (Player.armor[1].type == ItemID.ShroomiteBreastplate && Player.armor[2].type == ItemID.ShroomiteLeggings)
            {
                switch (Player.armor[0].type)
                {
                    case ItemID.ShroomiteMask:
                        Player.setBonus = "Build up stealth while on the ground\nStealth provides up to 35% ranged damage, 10% ranged critical strike chance and a reduced chance for enemies to target you\nStealth slowly depletes when off the ground";
                        if (traeStealth < 1f && Player.velocity.Y == 0)
                        {
                            traeStealth += 1f / 60f;
                        }
                        else if (Player.velocity.Y != 0)
                        {
                            traeStealth -= 1f / 180f;
                        }
                        if (traeStealth > 0)
                        {
                            Player.GetDamage(DamageClass.Ranged) += 0.35f * traeStealth;
                            Player.GetCritChance(DamageClass.Ranged) += 10f * traeStealth;
                            Player.aggro += (int)(-750 * traeStealth);
                        }
                        break;
                    case ItemID.ShroomiteHeadgear:
                        maskSet = true;
                        Player.setBonus = "Build up stealth while not using ranged attacks\nStealth provides up to 3x increased ranged damage\nStealth depletes while using ranged attacks";
                        if (noRangedTimer < headgearStartStealthTime + timeToMax && Player.itemAnimation == 0)
                        {
                            noRangedTimer++;
                        }
                        if (noRangedTimer == headgearStartStealthTime + timeToMax - 1)
                        {
                            for (int i = 0; i < 80; i++)
                            {
                                Dust d = Dust.NewDustPerfect(Player.Center, 135, TRAEMethods.PolarVector(12, ((float)Main.rand.Next(80) / 50f) * MathF.PI * 2f), 100);
                                d.noGravity = true;
                            }

                        }
                        if (noRangedTimer > headgearStartStealthTime)
                        {
                            traeStealth = (float)(noRangedTimer - headgearStartStealthTime) / timeToMax;
                            damageMult = 1f + (traeStealth * (maxHeadDamage - 1f));
                        }
                        else if (noRangedTimer >= 0)
                        {
                            damageMult = 1f;
                            traeStealth = 0f;
                        }
                        break;
                    case ItemID.ShroomiteHelmet:
                        Player.setBonus = "Build up stealth while on the ground\nStealth provides up to 30% dodge chance and doubled base length of invincibility after taking damage\nStealth depletes after dodging an attack";
                        if (traeStealth < 1f && Player.velocity.Y == 0)
                        {
         
                            traeStealth += 1f / 90f;
                       
                        }
  
                        if (traeStealth > 0)
                        {
                            ShroomDodge = 0.3f * traeStealth;
                            ShroomIframes = (int)(40f * traeStealth);
                        }
                        break;
                    default:
                        traeStealth = 0;
                        break;
                }

            }
            else
            {
                traeStealth = 0;
                ShroomDodge = 0;
                ShroomIframes = 0;
            }
            Player.shroomiteStealth = false;
            if (!maskSet)
            {
                noRangedTimer = 0;
                damageMult = 1f;
            }
        }
        public override void PostUpdate()
        {
            if(traeStealth > 0)
            {
                Player.stealth = (1f - traeStealth * 0.75f);
                Player.shroomiteStealth = true;
            }
            if(shroomDodgeAnim > 0)
            {
                shroomDodgeAnim--;
                //Main.NewText("huh");
                /*
                for(int i = 0; i < 8; i++)
                {
                    Main.PlayerRenderer.DrawPlayer(Main.Camera, Player, Player.position + TRAEMethods.PolarVector(20, MathF.PI * 2f *(float)i / 8f), 0f, Player.Size * 0.5f, 0.5f, 1f);
                }
                */
            }
            base.PostUpdate();
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if(maskSet && item.CountsAsClass(DamageClass.Ranged))
            {
                damage *= damageMult;
            }
        }
        public override void PostItemCheck()
        {
            if(maskSet && Player.HeldItem.DamageType == DamageClass.Ranged && Player.itemAnimation > 0)
            {
                if(damageMult > 1f)
                {
                }
                if(noRangedTimer > 0)
                {
                    if(noRangedTimer < headgearStartStealthTime)
                    {
                        noRangedTimer = 0;
                    }
                    else
                    {
                        noRangedTimer -= 1;

                        Dust d = Dust.NewDustPerfect(Player.Center, 135, TRAEMethods.PolarVector(12, ((float)Main.rand.Next(80) / 50f) * MathF.PI * 2f), 100);
                        d.noGravity = true;

                    }
                }
            }
        }
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (Main.rand.NextFloat() < ShroomDodge)
            {
                traeStealth = 0;

                //Main.NewText("ShroomDodge");
                Player.brainOfConfusionDodgeAnimationCounter = 300;
			    Player.SetImmuneTimeForAllTypes(Player.longInvince ? 120 : 80);
                shroomDodgeAnim = 60;
                if (Player.longInvince)
                {
                    int invintime = (int)(info.Damage * 3 / 5); // every point of info.Damage adds 0.01 seconds 
                    if (invintime > 120)
                        invintime = 120;
                    Player.immuneTime += invintime - 40; // cross necklace adds .67 seconds so we need to substract that from the total.
                }
                if(ShroomIframes > 0)
                {
                    Player.immuneTime += (int)ShroomIframes;
                }
                return true;
            }
            return base.FreeDodge(info);
        }
        public override void PostHurt(Player.HurtInfo info)
        {
            if(ShroomIframes > 0)
            {
                Player.immuneTime += (int)ShroomIframes;
            }
        }
        /*
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(maskSet && hit.DamageType == DamageClass.Ranged)
            {
                if(damageMult > 1f)
                {
                }
                if(noRangedTimer > 0)
                {
                    if(noRangedTimer < maskStartStealthTime)
                    {
                        noRangedTimer = 0;
                    }
                    else
                    {
                        noRangedTimer = -20;
                    }
                }
            }
        }
        */
    }
}



