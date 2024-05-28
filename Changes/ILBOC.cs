using MonoMod.Cil;

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
            c.EmitLdcI4(32);

            c.GotoNext(MoveType.After,
                x => x.MatchLdcI4(20)
            );
            c.EmitPop();
            c.EmitLdcI4(16);

   
 

            return;
        }
    }
}
