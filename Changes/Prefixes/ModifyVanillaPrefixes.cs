using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common.ModPlayers;

namespace TRAEProject.Changes.Prefixes
{
    class ModifyVanillaPrefixes : ModPlayer
    {
        #region damage
        public const float T1Damage = 1.05f;
        public const float T2Damage = 1.1f;
        public const float T3Damage = 1.12f;
        public const float T4Damage = 1.15f;
        public const float T5Damage = 1.18f;
        public const float T6Damage = 1.2f;
        public const float T7Damage = 1.25f;

        void ModifyDamage(Item item, ref StatModifier damageBonus, float current, float wanted)
        {
            int origonalDamage = (int)Math.Round((float)item.damage / current);
            //Main.NewText("Origonal Damage:" + origonalDamage);
            int finalDamage = (int)((int)Math.Round(origonalDamage * wanted ) * damageBonus);
            //Main.NewText("Final Damage:" + finalDamage);
            float newDamageBonus = (float)finalDamage / (float)item.damage;
            //Main.NewText("New Damage Bonus:" + newDamageBonus);
            damageBonus += newDamageBonus - damageBonus;
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage, ref float flat)
        {
            switch (item.prefix)
            {
                case PrefixID.Sighted:
                    ModifyDamage(item, ref damage, 1.1f, T3Damage);
                    break;
                case PrefixID.Staunch:
                    ModifyDamage(item, ref damage, 1.1f, T4Damage);
                    break;
                case PrefixID.Powerful:
                    ModifyDamage(item, ref damage, 1.1f, T5Damage);
                    break;
                case PrefixID.Bulky:
                    ModifyDamage(item, ref damage, 1.05f, T7Damage);
                    break;
                case PrefixID.Intense:
                    ModifyDamage(item, ref damage, 1.1f, T6Damage);
                    break;
                case PrefixID.Furious:
                    ModifyDamage(item, ref damage, 1.15f, T6Damage);
                    break;

                case PrefixID.Frenzying:
                    ModifyDamage(item, ref damage, 0.85f, 0.95f);
                    break;
                case PrefixID.Manic:
                    ModifyDamage(item, ref damage, 0.9f, 1f);
                    break;


                case PrefixID.Large:
                case PrefixID.Heavy:
                    damage *= T1Damage;
                    break;
                case PrefixID.Strong:
                case PrefixID.Massive:
                    damage *= T2Damage;
                    break;
            }
        }
        #endregion

        #region speed
        public override float UseSpeedMultiplier(Item item)
        {
            if(item.prefix == PrefixID.Savage)
            {
                return (1f / 1.1f);
            }
            return base.UseSpeedMultiplier(item);
        }
        #endregion

        #region crit
        public override void ModifyWeaponCrit(Item item, ref int crit)
        {
            switch(item.prefix)
            {
                case PrefixID.Keen:
                    crit += 5; // 3% crit -> 8% crit
                    break;
                case PrefixID.Zealous:
                    crit += 7; // 5% crit -> 12% crit
                    break;
                case PrefixID.Sighted:
                    crit += 2; // 3% crit -> 5% crit
                    break;
                case PrefixID.Powerful:
                    crit += 2; // 1% crit -> 3% crit
                    break;

                case PrefixID.Unpleasant:
                    crit += 5; // 0% crit -> 5% crit
                    break;
                case PrefixID.Nimble:
                    crit += 2; // 0% crit -> 2% crit
                    break;
            }
        }
        #endregion

        #region size
        public float bonusSize = 1f;
        //check meleeStats.cs to see where swords gain size
        //check Spear.cs for spear size
        public void UpdateWhipSize(Item item)
        {
            if (!item.IsAir && ItemID.Sets.SummonerWeaponThatScalesWithAttackSpeed[item.type])
            {
                switch (item.prefix)
                {
                    case PrefixID.Large:
                        bonusSize = (1.18f / 1.15f);
                        break;
                    case PrefixID.Massive:
                        bonusSize = (1.25f / 1.18f);
                        break;
                    case PrefixID.Dangerous:
                        bonusSize = (1.12f / 1.5f);
                        break;
                    case PrefixID.Bulky:
                        bonusSize = (1.2f / 1.1f);
                        break;
                }
                Player.whipRangeMultiplier *= bonusSize * item.scale;
            }
        }
        public override void UpdateEquips()
        {
            UpdateWhipSize(Player.HeldItem);
        }
        #endregion

        #region velocity
        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if(item.prefix == PrefixID.Hasty)
            {
                velocity *= (1.35f / 1.15f);
            }
            base.ModifyShootStats(item, ref position, ref velocity, ref type, ref damage, ref knockback);
        }
        #endregion

        #region knockback
        public const float T1Knockback = 1.1f;
        public const float T2Knockback = 1.2f;
        public const float T3Knockback = 1.35f;
        public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback, ref float flat)
        {
            switch (item.prefix)
            {
                case PrefixID.Strong:
                case PrefixID.Large:
                case PrefixID.Massive:
                    knockback *= T1Knockback;
                    break;
                case PrefixID.Zealous:
                    knockback *= T2Knockback;
                    break;
                case PrefixID.Forceful:
                case PrefixID.Godly:
                case PrefixID.Heavy:
                case PrefixID.Intimidating:
                case PrefixID.Staunch:
                    float div = (T3Knockback / 1.15f); //all these weapon already have 15% knockback
                    knockback *= div;
                    break;
                case PrefixID.Savage:
                    knockback /= T1Knockback;
                    break;
                case PrefixID.Frenzying:
                    knockback *= (2f-T2Knockback);
                    break;
            }
            base.ModifyWeaponKnockback(item, ref knockback, ref flat);
        }
        #endregion

        
    }
    class PrefixTooltips : GlobalItem
    {
        string ModifyDamage(Item item, float current, float wanted)
        {
            int origonalDamage = (int)Math.Round((float)item.damage / current);

            int finalDamage = (int)Math.Round(origonalDamage * wanted);

            float newDamageBonus = (float)finalDamage / (float)origonalDamage;

            int per = (int)Math.Round((newDamageBonus - 1f) * 100f);
            if(per == 0)
            {
                return "";
            }
            if(per < 0)
            {
               return per + "% damage";
            }
            return "+" + per + "% damage";
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            #region damage
            //modify damage
            TooltipLine line = tooltips.FirstOrDefault(x => x.Name == "PrefixDamage" && x.Mod == "Terraria");
            if (line != null)
            {
                switch (item.prefix)
                {
                    case PrefixID.Sighted:
                        line.Text = ModifyDamage(item, 1.1f, ModifyVanillaPrefixes.T3Damage);
                        break;
                    case PrefixID.Staunch:
                        line.Text = ModifyDamage(item, 1.1f, ModifyVanillaPrefixes.T4Damage);
                        break;
                    case PrefixID.Powerful:
                        line.Text = ModifyDamage(item, 1.1f, ModifyVanillaPrefixes.T5Damage);
                        break;
                    case PrefixID.Bulky:
                        line.Text = ModifyDamage(item, 1.05f, ModifyVanillaPrefixes.T7Damage);
                        break;
                    case PrefixID.Intense:
                        line.Text = ModifyDamage(item, 1.1f, ModifyVanillaPrefixes.T6Damage);
                        break;
                    case PrefixID.Furious:
                        line.Text = ModifyDamage(item, 1.15f, ModifyVanillaPrefixes.T6Damage);
                        break;

                    case PrefixID.Frenzying:
                        line.Text = ModifyDamage(item, 0.85f, 0.95f);
                        break;
                    case PrefixID.Manic:
                        line.Text = ModifyDamage(item, 0.9f, 1f);
                        break;
                }
            }

            //insert damage
            switch (item.prefix)
            {
                case PrefixID.Large:
                    int sizeIndex = tooltips.FindIndex(TL => TL.Name == "PrefixSize");

                    line = new TooltipLine(TRAEProj.Instance, "TRAEDamage", "+"+ (int)((ModifyVanillaPrefixes.T1Damage - 1f) * 100f) + "% damage");
                    line.IsModifier = true;
                    tooltips.Insert(sizeIndex, line);
                    break;
                case PrefixID.Heavy:
                    int speedIndex = tooltips.FindIndex(TL => TL.Name == "PrefixSpeed");

                    line = new TooltipLine(TRAEProj.Instance, "TRAEDamage", "+" + (int)((ModifyVanillaPrefixes.T1Damage - 1f) * 100f) + "% damage");
                    line.IsModifier = true;
                    tooltips.Insert(speedIndex, line);
                    break;
                case PrefixID.Strong:
                    int kbIndex = tooltips.FindIndex(TL => TL.Name == "PrefixKnockback");

                    line = new TooltipLine(TRAEProj.Instance, "TRAEDamage", "+" + (int)((ModifyVanillaPrefixes.T2Damage - 1f) * 100f) + "% damage");
                    line.IsModifier = true;
                    tooltips.Insert(kbIndex, line);
                    break;
                case PrefixID.Massive:
                    int sizeIndex2 = tooltips.FindIndex(TL => TL.Name == "PrefixSize");

                    line = new TooltipLine(TRAEProj.Instance, "TRAEDamage", "+" + (int)((ModifyVanillaPrefixes.T2Damage - 1f) * 100f) + "% damage");
                    line.IsModifier = true;
                    tooltips.Insert(sizeIndex2, line);
                    break;
            }
            #endregion

            #region speed

            if (item.prefix == PrefixID.Savage)
            {
                int damageIndex = tooltips.FindIndex(TL => TL.Name == "PrefixDamage");

                line = new TooltipLine(TRAEProj.Instance, "TRAESpeed", "+10% speed");
                line.IsModifier = true;
                tooltips.Insert(damageIndex + 1, line);
            }
            #endregion

            #region crit
            //modify crit
            line = tooltips.FirstOrDefault(x => x.Name == "PrefixCritChance" && x.Mod == "Terraria");
            if (line != null)
            {
                switch (item.prefix)
                {
                    case PrefixID.Keen:
                        line.Text = "+8% critical strike chance";
                        break;
                    case PrefixID.Zealous:
                        line.Text = "+12% critical strike chance";
                        break;
                    case PrefixID.Sighted:
                        line.Text = "+5% critical strike chance";
                        break;
                    case PrefixID.Powerful:
                        line.Text = "+3% critical strike chance";
                        break;

                }
            }

            //insert crit
            if (item.prefix == PrefixID.Nimble)
            {
                line = new TooltipLine(TRAEProj.Instance, "TRAECrit", "+2% critical strike chance");
                line.IsModifier = true;
                tooltips.Add(line);
            }
            if (item.prefix == PrefixID.Unpleasant)
            {
                int kbIndex = tooltips.FindIndex(TL => TL.Name == "PrefixKnockback");

                line = new TooltipLine(TRAEProj.Instance, "TRAECrit", "+5% critical strike chance");
                line.IsModifier = true;
                tooltips.Insert(kbIndex, line);
            }
            #endregion

            #region size
            //modify size
            line = tooltips.FirstOrDefault(x => x.Name == "PrefixSize" && x.Mod == "Terraria");
            if (line != null)
            {
                switch (item.prefix)
                {
                    case PrefixID.Large:
                        line.Text = "+18% size";
                        break;
                    case PrefixID.Massive:
                        line.Text = "+25% size";
                        break;
                    case PrefixID.Dangerous:
                        line.Text = "+12% size";
                        break;
                    case PrefixID.Bulky:
                        line.Text = "+20% size";
                        break;
                }
            }
            #endregion

            #region velocity
            //modify size
            line = tooltips.FirstOrDefault(x => x.Name == "PrefixShootSpeed" && x.Mod == "Terraria");
            if (line != null)
            {
                switch (item.prefix)
                {
                    case PrefixID.Hasty:
                        line.Text = "+35% velocity";
                        break;
                }
            }
            #endregion

            #region knockback
            //modify knockback
            line = tooltips.FirstOrDefault(x => x.Name == "PrefixKnockback" && x.Mod == "Terraria");
            if (line != null)
            {
                switch (item.prefix)
                {    
				    case PrefixID.Strong:
                    case PrefixID.Forceful:
                    case PrefixID.Godly:
                    case PrefixID.Heavy:
                    case PrefixID.Intimidating:
                    case PrefixID.Staunch:
                        line.Text = "+" + (int)((ModifyVanillaPrefixes.T3Knockback - 1f) * 100f) + "% knockback";
                        break;
                    case PrefixID.Savage:
                        line.Text = "";
                        break;
                }
            }
            //insert knockback
            switch (item.prefix)
            {
            
                case PrefixID.Large:
                case PrefixID.Massive:
                    line = new TooltipLine(TRAEProj.Instance, "TRAEKnockback", "+" + (int)((ModifyVanillaPrefixes.T1Knockback - 1f) * 100f) + "% knockback");
                    line.IsModifier = true;
                    tooltips.Add(line);
                    break;
                case PrefixID.Zealous:
                    line = new TooltipLine(TRAEProj.Instance, "TRAEKnockback", "+" + (int)((ModifyVanillaPrefixes.T2Knockback - 1f) * 100f) + "% knockback");
                    tooltips.Add(line);
                    line.IsModifier = true;
                    break;
                case PrefixID.Frenzying:
                    line = new TooltipLine(TRAEProj.Instance, "TRAEKnockback", "-" + (int)((ModifyVanillaPrefixes.T2Knockback - 1f) * 100f) + "% knockback");
                    tooltips.Add(line);
                    line.IsModifier = true;
                    line.IsModifierBad = true;
                    break;
            }
            #endregion
        }
    }
}