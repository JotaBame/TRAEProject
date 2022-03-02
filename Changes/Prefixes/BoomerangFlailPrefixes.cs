using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.Prefixes
{
    //This class defines what boomerang prefixes can have: damage, crit, armor penetration, velocity, and knockback
    public abstract class BoomerangFlailPrefix : ModPrefix
    {
        public override float RollChance(Item item)
        {
            return 1f;
        }
        public override bool CanRoll(Item item) => true;
        public override PrefixCategory Category => PrefixCategory.Custom;
        byte damage = 0;
        byte negDamage = 0;
        byte crit = 0;
        byte AP = 0;
        byte velocity = 0;
        byte negVelocity = 0;
        byte knockback = 0;
        byte negKnockback = 0;
        public void SetPrefix(sbyte damage, byte crit, byte AP, sbyte velocity, sbyte knockback)
        {
            if(damage < 0)
            {
                this.negDamage = (byte)(-1 * damage);
            }
            else
            {
                this.damage = (byte)damage;
            }
            if (velocity < 0)
            {
                this.negVelocity = (byte)(-1 * velocity);
            }
            else
            {
                this.velocity = (byte)velocity;
            }
            if (knockback < 0)
            {
                this.negKnockback = (byte)(-1 * knockback);
            }
            else
            {
                this.knockback = (byte)knockback;
            }
            this.AP = AP;
            this.crit = crit;
        }


        public override void ModifyValue(ref float valueMult)
        {
            float multiplier = 1f * (1 + damage * 0.004f) 
                * (1 + crit * 0.004f) 
                * (1 + AP * 0.003f) 
                * (1 + velocity * 0.001f) 
                * (1 + knockback * 0.001f) 
                * (1 - negDamage * 0.004f) 
                * (1 - negVelocity * 0.001f) 
                * (1 - negKnockback * 0.001f);
            valueMult *= multiplier;
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            if (negDamage > 0)
            {
                damageMult = 1f + (-this.negDamage) * .01f;
            }
            else
            {
                damageMult = 1f + (this.damage) * .01f; 
            }

            if (negVelocity > 0)
            {
                shootSpeedMult = 1f + (-this.negVelocity) * .01f;
            }
            else
            {
                shootSpeedMult = 1f + (this.velocity) * .01f;
            }

            if (negKnockback > 0)
            {
                knockbackMult = 1f + (-this.negKnockback) * .01f;
            }
            else
            {
                knockbackMult = 1f + (this.knockback) * .01f;
            }

            critBonus = this.crit;
        }
        public override void Apply(Item item)
        {
            
            item.GetGlobalItem<BoomerangFlailStats>().AP += AP;
            base.Apply(item);
        }
    }
    //each class below is an idividual prefix
    public class Devastating : BoomerangFlailPrefix
    {
        public override void SetStaticDefaults()
        {
            SetPrefix( 15, 5, 5, 20, 35);
        }
    }
    public class Brutal : BoomerangFlailPrefix
    {
        public override void SetStaticDefaults()
        {
            SetPrefix( 10, 3, 3, 10, 20);
        }
    }
    public class Kinetic : BoomerangFlailPrefix
    {
        public override void SetStaticDefaults()
        {
            SetPrefix(5, 0, 0, 50, 50);
        }
    }
    public class Enchanted : BoomerangFlailPrefix
    {
        public override void SetStaticDefaults()
        {
            SetPrefix(0, 5, 8, 35, -35);
        }
    }
    public class Pathetic : BoomerangFlailPrefix
    {
        public override void SetStaticDefaults()
        {
            SetPrefix(-20, 0, 0, -50, -50);
        }
    }
    public class Piercing : BoomerangFlailPrefix
    {
        public override void SetStaticDefaults()
        {
            SetPrefix(0, 0, 10, 0, 0);
        }
    }
    public class Spiked : BoomerangFlailPrefix
    {
        public override void SetStaticDefaults()
        {
            SetPrefix(5, 10, 10, 0, 0);
        }
    }
    public class Dense : BoomerangFlailPrefix
    {
        public override void SetStaticDefaults()
        {
            SetPrefix(20, 0, 0, -35, 35);
        }
    }
    public class Aerodynamic : BoomerangFlailPrefix
    {
        public override void SetStaticDefaults()
        {
            SetPrefix(0, 0, 0, 50, 0);
        }
    }

    //This is where you put custom stats you can modify
    public class BoomerangFlailStats : GlobalItem
    {
        public int AP;

        public override bool InstancePerEntity => true;
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if(AP > 0)
            {
                TooltipLine line = new TooltipLine(TRAEProj.Instance, "TRAEAP", "+" + AP + " armor penetration");
                line.isModifier = true;
                int kbIndex = tooltips.FindIndex(TL => TL.Name == "PrefixKnockback");
                int velIndex = tooltips.FindIndex(TL => TL.Name == "PrefixShootSpeed");
                if(velIndex != -1)
                {
                    tooltips.Insert(velIndex, line);
                }
                else if(kbIndex != -1)
                {
                    tooltips.Insert(kbIndex, line);
                }
                else
                {
                    tooltips.Add(line);
                }
            }
        }
    }

    public class ApplyPrefixAP : ModPlayer
    {
        public override void PostUpdateMiscEffects()
        {
            if (!Player.HeldItem.IsAir)
            {
                //give the player armor penetration
                Player.armorPenetration += Player.HeldItem.GetGlobalItem<BoomerangFlailStats>().AP;


                //heavy flails don't benifit from getting item.shootspeed increased so give them a melee speed bonus
                switch (Player.HeldItem.type)
                {
                    case ItemID.Mace:
                    case ItemID.FlamingMace:
                    case ItemID.BallOHurt:
                    case ItemID.TheMeatball:
                    case ItemID.BlueMoon:
                    case ItemID.Sunfury:
                    case ItemID.DaoofPow:
                    case ItemID.DripplerFlail:
                    case ItemID.FlowerPow:
                    case ItemID.Flairon:
                        int pre = Player.HeldItem.prefix;
                        if (pre == PrefixType<Kinetic>() || pre == PrefixType<Aerodynamic>())
                        {
                            Player.meleeSpeed /= 1.5f;
                        }
                        if (pre == PrefixType<Devastating>() || pre == PrefixType<Enchanted>())
                        {
                            Player.meleeSpeed /= 1.35f;
                        }
                        if (pre == PrefixType<Brutal>())
                        {
                            Player.meleeSpeed /= 1.2f;
                        }
                        if (pre == PrefixType<Dense>() || pre == PrefixType<Pathetic>())
                        {
                            Player.meleeSpeed /= 0.5f;
                        }
                        break;
                }
            }
        }
    }
}