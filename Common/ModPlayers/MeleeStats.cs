using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.TRAEDebuffs;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Common.ModPlayers
{
	public class MeleeStats : ModPlayer
    {

        public float weaponSize = 1f;
        public float meleeVelocity = 1f;
        public bool TRAEAutoswing = false;
        public override void ResetEffects()
        {
            weaponSize = 1f;
            meleeVelocity = 1f;
            TRAEAutoswing = false;
        }
        public override void PostUpdateEquips()
        {
             if(TRAEAutoswing || Main.SettingsEnabled_AutoReuseAllItems)
            {            
                Player.autoReuseGlove = true;
            }

        }
        public override void PostItemCheck()
        {
            if (!Player.HeldItem.useTurn && Player.HeldItem.CountsAsClass(DamageClass.Melee) && Player.itemAnimation == Player.itemAnimationMax && Player.HeldItem.shoot == 0 || Player.HeldItem.type == ItemID.BeamSword)
            { 
                Vector2 mousePosition = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                if (Player.position.X - mousePosition.X > 0)
                    Player.direction = -1;
                else
                    Player.direction = 1;
            }
        }
        #region Sword Size
        public override void SetStaticDefaults()
        {
            Terraria.IL_Player.GetAdjustedItemScale += HookSize;
            //Terraria.IL_Player.ApplyMeleeScale += HookSize;

            Terraria.IL_Player.ItemCheck_GetMeleeHitbox += HookHey;
            //IL.Terraria.DataStructures.PlayerDrawLayers.DrawPlayer_27_HeldItem += HookHey2;
        }
        private void HookHey(ILContext il)
        {
            var c = new ILCursor(il);
            c.EmitDelegate(() =>
               {
                   //Main.NewText("GetHitbox");
               });
        }
        private void HookHey2(ILContext il)
        {
            var c = new ILCursor(il);
            c.EmitDelegate(() =>
            {
                Main.NewText("GetDraw");
            });
        }
        private void HookSize(ILContext il)
        {
            var c = new ILCursor(il);

            for (int i = 0; i < 2; i++)
            {
                if (!c.TryGotoNext(i => i.MatchLdloc(0)))
                {
                    return; // Patch unable to be applied
                }
            }

            //EDIT: Pop the old value so we don't have stack issues
            c.Index++;
            c.Emit(OpCodes.Pop);

            //push the item onto the stack
            c.Emit(OpCodes.Ldarg_1);
            //push the player onto the stack
            c.Emit(OpCodes.Ldarg_0);
            //push the local variable onto the stack
            c.Emit(OpCodes.Ldloc_0);

            c.EmitDelegate<Func<Item, Player, float, float>>((item, player, scale) =>
            {
                if (item.CountsAsClass(DamageClass.Melee))
                {                    //
                    float bonusSize = 1f;
                    switch (item.prefix)
                    {
                        case PrefixID.Large:
                            bonusSize = (1.18f / 1.15f);
                            break;
                        case PrefixID.Massive:
                            bonusSize = (1.25f / 1.18f);
                            break;
                        case PrefixID.Dangerous:
                            bonusSize = (1.12f / 1.05f);
                            break;
                        case PrefixID.Bulky:
                            bonusSize = (1.2f / 1.1f);
                            break;
                    }
                    scale *= bonusSize;
                    //
                    scale *= player.GetModPlayer<MeleeStats>().weaponSize;
                }
                //Main.NewText(scale);
                return scale;
            });
            //pop the variable at the top of the stack onto the local variable
            c.Emit(OpCodes.Stloc_0);
            //push the local variable onto the stack
            c.Emit(OpCodes.Ldloc_0);
        }
        #endregion
    }
}
