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
    class CobaltNaginata : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 141.4f;
            stabStart = 93.3f;
            stabEnd = -10;
            swingAmount = MathF.PI / 16;
        }
    }
    public class CobaltNaginataThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 141.4f;
            holdAt = 93f;
            Projectile.penetrate = 3;
            floatTime = 90; DustOnDeath = DustID.Cobalt; DustOnDeathCount = 30;


        }
    }
}
