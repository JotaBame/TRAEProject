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
    public class Javelin : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 59f;
            stabStart = 34f;
            stabEnd = -10;
            swingAmount = MathF.PI / 32;
        }
    }
    public class JavelinThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 59f;
            holdAt = 17f;
            Projectile.penetrate = 3;
        }
    }
}
