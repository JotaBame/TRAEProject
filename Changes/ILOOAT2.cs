using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace TRAEilHooks
{
    internal static class ILOOAT2 {

        internal static void DoStuff(ILContext il) {
            ILCursor c = new(il);

            // Hooking the getter doesn't work so we're going to be a bit uglier
            c.GotoNext(MoveType.After,
                x => x.MatchLdcI4(1),
                x => x.MatchStsfld("Terraria.GameContent.Events.DD2Event", "OngoingDifficulty"),
                x => x.MatchCallOrCallvirt("Terraria.GameContent.Events.DD2Event", "get_ReadyForTier2")
                );
           
            //ILLabel t2 = c.DefineLabel();
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldsfld, typeof(Terraria.Main).GetField("hardMode"));

            return;
        }
    }
}
