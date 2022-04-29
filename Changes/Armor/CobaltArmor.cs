using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.Armor
{
    public class CobaltArmorEffect : ModPlayer
    {
        public bool CobaltCritical = false;
        public override void ResetEffects()
        {
            CobaltCritical = false;
        }
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if(crit && CobaltCritical)
            {
                damage = (int)(damage * 1.25f);
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (crit && CobaltCritical)
            {
                damage = (int)(damage * 1.25f);
            }
        }
    }
    public class CobaltArmor : GlobalItem
    {
        public override void UpdateEquip(Item item, Player player)
        {
            switch (item.type)
            {
                case ItemID.CobaltHelmet:
                    player.moveSpeed += 0.05f;
                    player.GetDamage(DamageClass.Melee) -= 0.01f;
                    player.GetCritChance(DamageClass.Melee) += 10;
                    return;
                case ItemID.CobaltMask:
                    player.moveSpeed += 0.15f;
                    player.GetDamage(DamageClass.Ranged) -= 0.04f; 
                    player.GetCritChance(DamageClass.Ranged) -= 1;
                    return;
                case ItemID.CobaltHat:
                    player.moveSpeed += 0.15f;
                    player.GetDamage(DamageClass.Magic) -= 0.03f;
                    player.GetCritChance(DamageClass.Magic) -= 1;
                    return;
                case ItemID.CobaltBreastplate:
                    player.moveSpeed += 0.15f;
                    break;
                case ItemID.CobaltLeggings:
                    player.moveSpeed += 0.1f;
                    break;
            }
        }

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if ((head.type == ItemID.CobaltHelmet || head.type == ItemID.CobaltMask || head.type == ItemID.CobaltHat) && body.type == ItemID.CobaltBreastplate && legs.type == ItemID.CobaltLeggings)
            {
                return "CobaltSet";
            }
            return base.IsArmorSet(head, body, legs);
        }
        public override void UpdateArmorSet(Player player, string set)
        {
            if(set == "CobaltSet")
            {
                switch (player.armor[0].type)
                {
                    case ItemID.CobaltHelmet:
                        player.GetAttackSpeed(DamageClass.Melee) -= 0.15f;
                        break;
                    case ItemID.CobaltMask:
                        player.ammoCost80 = false;
                        break;
                    case ItemID.CobaltHat:
                        player.manaCost += 0.14f;
                        break;
                }
                player.setBonus = "Critical Strikes deal 25% more damage";
                player.GetModPlayer<CobaltArmorEffect>().CobaltCritical = true;
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.CobaltHelmet:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "5% increased melee damage and 10% increased melee critical strike chance";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "15% increased movement speed";
                        }
                    }
                    break;
                case ItemID.CobaltMask:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "5% increased ranged damage and 10% increased ranged critical strike chance";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "15% increased movement speed";
                        }
                    }
                    break;
                case ItemID.CobaltHat:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "5% increased magic damage and 10% increased magic critical strike chance";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip1")
                        {
                            line.Text = "15% increased movement speed";
                        }
                        if (line.Mod == "Terraria" && line.Name == "Tooltip2")
                        {
                            line.Text = "Increases maximum mana by 40";
                        }
                    }
                    break;
                case ItemID.CobaltBreastplate:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "15% increased movement speed and 5% increased critical strike chance";
                        }
                    }
                    break;
                case ItemID.CobaltLeggings:
                    foreach (TooltipLine line in tooltips)
                    {
                        if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                        {
                            line.Text = "20% increased movement speed and 3% increased damage";
                        }
                    }
                    break;
            }
        }
    }
}
