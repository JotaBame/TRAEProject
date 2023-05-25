using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.WorldBuilding;
using TRAEProject.Common;

namespace TRAEProject.Changes.Weapon.Melee.SpearProjectiles
{
    public class Trident : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 99f;
            stabStart = 79;
            stabEnd = -10;
            swingAmount = MathF.PI / 32;
        }
        public override void SpearModfiyHitNPCMelee(NPC target, ref NPC.HitModifiers modifiers)
        {
            if(Projectile.wet)
            {
                modifiers.FinalDamage.Base *= 1.33f;
            }
        }
    }
    public class TridentThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 99f;
            holdAt = 45f;
            Projectile.ignoreWater = true;
            maxSticks = 3;
            stickingDps = 2;
        }
        public override void SpearModfiyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.wet)
            {
                modifiers.FinalDamage.Base *= 1.33f;
            }
        }
    }
}
