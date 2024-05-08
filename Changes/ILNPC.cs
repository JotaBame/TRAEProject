using MonoMod.Cil;

//Feel free to kill off this one if you don't want it

namespace TRAEilHooks {
    internal static class ILNPC {

        internal static void DoStuff(ILContext il) {
            ILCursor c = new(il);

            // Jump to where the game checks if there are any Prismatic Lacewings spawned
            //c.GotoNext(MoveType.After,
            //    x => x.MatchLdcI4(0x295),
            //    x => x.MatchCall("Terraria.NPC", "AnyNPCs")
            //    );

            //// Use that to jump back to where the Plantera check occurrs
            //c.GotoPrev(MoveType.Before,
            //    x => x.MatchLdsfld("Terraria.NPC", "downedPlantBoss")
            //    );
            //var lab = c.IncomingLabels; //Cache labels pointing to this instruction
            //c.RemoveRange(2); //kill the plantera check and branch
            //foreach ( var label in lab ) { //and restore labels to the following check
            //    label.Target = c.Next;
            //}

            //DISABLED Hell Spawn changes
            //From here, to spawn Hellbat and Red Devil, we jump down to the Underworld section
            //c.GotoNext(MoveType.Before,
            //    x => x.MatchLdloc(5),
            //    x => x.MatchLdsfld("Terraria.Main","maxTilesY"),
            //    x => x.MatchLdcI4(0xBE),
            //    x => x.MatchSub()
            //    );

            //Then we can just jump to the two times the game checks for a mechanical boss kill and remove that
            //for (int i = 0; i < 2; i++) {
            //    c.GotoNext(MoveType.Before,
            //        x => x.MatchLdsfld("Terraria.NPC","downedMechBossAny")
            //        );
            //    c.RemoveRange(2);
            //}

            return;
        }
    }
}
