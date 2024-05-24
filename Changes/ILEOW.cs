using MonoMod.Cil;

//Feel free to kill off this one if you don't want it

namespace TRAEilHooks {
    internal static class ILEoW {

        internal static void DoStuff(ILContext il) {
            ILCursor c = new(il);


            // Lazy unsafe check because there's nothing else in this method
            c.GotoNext(MoveType.After,
           x => x.MatchLdcI4(65)
       );
            c.EmitPop();
            c.EmitLdcI4(56);

            c.GotoNext(MoveType.After,
                x => x.MatchLdcI4(70)
            );
            c.EmitPop();
            c.EmitLdcI4(56);

 
            return;
        }
    }
}
