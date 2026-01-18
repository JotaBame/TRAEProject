using MonoMod.Cil;
using Mono.Cecil.Cil;

namespace TRAEilHooks {
    internal static class ILPlantBulb {

        internal static void DoStuff(ILContext il) {
            ILCursor c = new(il);

			// Should match the Mech Boss check precisely ; if it doesn't match, someone else has already changed this block!
			c.GotoNext(MoveType.Before,
				x => x.MatchLdsfld(typeof(Terraria.NPC).GetField("downedMechBoss1")),
				x => x.MatchBrfalse(out _),
				x => x.MatchLdsfld(typeof(Terraria.NPC).GetField("downedMechBoss2")),
				x => x.MatchBrfalse(out _),
				x => x.MatchLdsfld(typeof(Terraria.NPC).GetField("downedMechBoss3"))
			);
			// Remove this check because we want Plantera to spawn as soon as HM starts
			c.RemoveRange(6);
			
			// Jump to the lifefruit check
			c.GotoNext(MoveType.After,
				x => x.MatchLdsfld(typeof(Terraria.NPC).GetField("downedMechBossAny")),
				x => x.MatchBrfalse(out _)
			);
			c.Index--; // Move just before the branch
			// And then or that with the new thing we want to check
            c.Emit(OpCodes.Ldsfld, typeof(Terraria.NPC).GetField("downedPlantBoss"));
			c.Emit(OpCodes.Or);
			
            return;
        }
    }
}
