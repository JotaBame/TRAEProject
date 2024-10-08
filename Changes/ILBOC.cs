﻿using MonoMod.Cil;
using System;
using Terraria.ID;
using Terraria;
using TRAEProject.Changes.NPCs.Miniboss.Santa;

//Feel free to kill off this one if you don't want it

namespace TRAEilHooks {
    internal static class ILBOC {

        internal static void DoStuff(ILContext il) {
            ILCursor c = new(il);


            // Lazy unsafe check because there's nothing else in this method
            c.GotoNext(MoveType.After,
                x => x.MatchLdcI4(40)
            );
            c.EmitPop();
            c.EmitLdcI4(30);

            c.GotoNext(MoveType.After,
                x => x.MatchLdcI4(20)
            );
            c.EmitPop();
            c.EmitLdcI4(15);

   
 

            return;
        }
    

    }
}
