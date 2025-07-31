using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;

namespace TRAEilHooks {
    internal static class ILEclipse {

        internal static void DoStuff(ILContext il) {
            ILCursor c = new(il);

            // Just go to downedMechBossAny it only appears here once
			c.GotoNext(MoveType.After,
				x => x.MatchStsfld(typeof(Terraria.Main).GetField("eclipse"))
			);
            c.GotoPrev(MoveType.After,
                x => x.MatchLdsfld(typeof(Terraria.NPC).GetField("downedMechBossAny"))
			);

            c.Emit(OpCodes.Ldsfld, typeof(Terraria.NPC).GetField("downedPlantBoss"));
            c.Emit(OpCodes.Or);

            return;
        }
    }
}
