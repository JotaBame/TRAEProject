using MonoMod.Cil;

namespace TRAEilHooks {
    internal static class ILMounts {

        internal static void DoStuff(ILContext il) {
            ILCursor c = new(il);

            // Jump to where the basilisk jump is granted
            c.GotoNext(MoveType.After,
                x => x.MatchLdarg1(),
                x => x.MatchLdcI4(1),
                x => x.MatchCallOrCallvirt("Terraria.Player","set_hasJumpOption_Basilisk")
                );

            // Now jump to where the no knockback stuff occurs
            c.GotoNext(MoveType.Before,
                x => x.MatchLdarg1(),
                x => x.MatchLdcI4(1),
                x => x.MatchStfld("Terraria.Player", "noKnockback")
                );

            c.RemoveRange(3);

            return;
        }
    }
}
