using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TRAEProject.Common;

namespace TRAEProject.Changes.Weapon.Melee.SpearProjectiles
{
    public class BasicSpear : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 76f;
            stabStart = 54f;
            stabEnd = -10;
            swingAmount = MathF.PI / 32;
        }
    }
    public class BasicSpearThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 76f;
            holdAt = 45f;
            Projectile.penetrate = 3;
        }
    }
}
