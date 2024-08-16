using MonoMod.Cil;
using System;
using Terraria.ID;
using Terraria;
using TRAEProject.Changes.NPCs.Miniboss.Santa;

//Feel free to kill off this one if you don't want it

namespace TRAEilHooks {
    internal static class ILBOC2 {


    
        internal static void DoDebuffStuff(ILContext il)
        {
            ILCursor c = new(il);

            ILLabel skipTo = null; //Surprise tool that will help us later

            // This should bring us to checking if a Creeper is the NPC in question
            c.GotoNext(MoveType.After,
    x => x.MatchLdarg(1),
    x => x.MatchLdfld("Terraria.NPC", "type"),
    x => x.MatchLdcI4(267),
    x => x.MatchBneUn(out skipTo)
);

            var labels = c.IncomingLabels;

            c.EmitLdarg0();

            foreach (ILLabel lab in labels)
            {
                lab.Target = c.Prev;
            }

            c.EmitDelegate<Action<Terraria.Player>>((player) => {
                 int buff = Main.rand.NextFromList(BuffID.Weak, BuffID.BrokenArmor, BuffID.Slow, BuffID.Bleeding);
                int duration = Main.rand.Next(240, 360);
                if (buff == BuffID.Slow)
                    duration = duration * 2 / 5;
                if (buff == BuffID.Bleeding)
                    duration = duration * 4 / 3;
                player.AddBuff(buff, duration);
            });

            c.EmitLdcI4(10);
            c.EmitBrtrue(skipTo);



            return;
        }
    }
}
