using MonoMod.Cil;
using Mono.Cecil.Cil;

namespace TRAEilHooks {
    internal static class ILPlantBulb {

        internal static void DoStuff(ILContext il) {
            ILCursor c = new(il);

            // Match close to where the Plantera Bulb checks occur
            c.GotoNext(MoveType.Before,
                x => x.MatchLdcI4(1),
                x => x.MatchStloc(15),
                x => x.MatchLdcI4(150),
                x => x.MatchStloc(16)
            );


            c.Index -= 10; // Jump back to where all 3 mech bosses check happens
            c.RemoveRange(6); // Remove all 6 instructions for checking mech bosses

            // Match close to where the Lifefruit checks occur
            c.GotoNext(MoveType.Before,
                x => x.MatchLdcI4(1),
                x => x.MatchStloc(19),
                x => x.MatchLdcI4(60),
                x => x.MatchStloc(20)
                );

            // Put a label here so we can branch to it later
            ILLabel fruitSpawn = c.MarkLabel();

            // Jump back to before the downedMechbossAny check
            c.Index -= 6;
            c.Emit(OpCodes.Ldsfld, typeof(Terraria.NPC).GetField("downedPlantBoss"));
            c.Emit(OpCodes.Brtrue, fruitSpawn);

            return;
        }
    }
}
