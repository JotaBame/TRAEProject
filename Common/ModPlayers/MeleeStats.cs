using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.TRAEDebuffs;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Common.ModPlayers
{
    class MeleeStats : ModPlayer
    {

        public float weaponSize = 1f;
        public float meleeVelocity = 1f;
        public bool TRAEAutoswing = false;
        public override void ResetEffects()
        {
            weaponSize = 1f;
            meleeVelocity = 1f;
            TRAEAutoswing = false;
        }
        public override void PostUpdateEquips()
        {
            if(TRAEAutoswing)
            {            
                Player.autoReuseGlove = true;
            }

        }
        
    }
}
