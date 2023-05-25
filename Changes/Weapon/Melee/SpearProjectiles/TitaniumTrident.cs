using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using TRAEProject.Common;

namespace TRAEProject.Changes.Weapon.Melee.SpearProjectiles
{
    public class TitaniumTrident : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 155.5f;
            stabStart = 124.5f;
            stabEnd = -10;
            swingAmount = MathF.PI / 32;
        }
    }
    public class TitaniumTridentThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            DustOnDeath = DustID.Titanium;
            DustOnDeathCount = 40;

            spearLength = 155.5f;
            holdAt = 93f;
            Projectile.ignoreWater = true;
            maxSticks = 5;
            stickingDps = 20;
            floatTime = 60;
        }
    }
}
