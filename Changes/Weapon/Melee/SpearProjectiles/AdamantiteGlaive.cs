using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TRAEProject.Common;
using Terraria.ID;
namespace TRAEProject.Changes.Weapon.Melee.SpearProjectiles
{
    class AdamantiteGlaive : Spear
    {
        public override void SpearDefaults()
        {
            spearLength = 155.5f;
            stabStart = 123f;
            stabEnd = -10;
            swingAmount = MathF.PI / 16;
        }
    }
    public class AdamantiteGlaiveThrow : SpearThrow
    {
        public override void SpearDefaults()
        {
            spearLength = 155.5f;
            holdAt = 93f;
            Projectile.penetrate = 3;
            floatTime = 120;
            DustOnDeath = DustID.Adamantite; 
            DustOnDeathCount = 40;

        }
    }
}
