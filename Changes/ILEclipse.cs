using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;

namespace TRAEilHooks {
    internal static class ILEclipse {

        internal static void DoStuff(ILContext il) {
            ILCursor c = new(il);

            // Jump to after the Eclipse check; this is a bit long and I don't think anything else resets the dial cooldowns here, but I'm being safe
            c.GotoNext(MoveType.Before,
                x => x.MatchLdcI4(0),
                x => x.MatchStsfld("Terraria.Main","sundialCooldown"),
                x => x.MatchLdcI4(0),
                x => x.MatchStsfld("Terraria.Main", "moondialCooldown"),
                x => x.MatchLdcI4(1),
                x => x.MatchStsfld("Terraria.Main", "eclipse")
                );

            
            //ILLabel eclipseLabel = c.MarkLabel(); // Save this instruction for later
            c.Index -= 6; // Jump back to where we want to add a condition
            c.Emit(OpCodes.Ldsfld, typeof(Terraria.NPC).GetField("downedPlantBoss"));
            c.Index += 1;
            c.Emit(OpCodes.Or);



            //c.Emit(OpCodes.Brtrue, eclipseLabel);

            return;
        }
    }
}
