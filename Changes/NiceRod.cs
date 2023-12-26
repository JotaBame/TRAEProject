using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.Audio;
using Terraria.GameContent.Shaders;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using TRAEProject.NewContent.Items.Accesories.MobilityJumps;

namespace TRAEProject.Changes
{
    class NiceRod : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if(item.type == ItemID.IceRod)
            {
                item.accessory = true;
            }
        }
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if(item.type == ItemID.IceRod)
            {
                player.GetModPlayer<NiceRodEffect>().effect = true;
            }
        }
    }
    class NiceRodEffect : ModPlayer
    {
        public bool effect = false;
        public override void ResetEffects()
        {
            effect = false;
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if(effect && Main.rand.NextBool(1000))
            {
                modifiers.FinalDamage += 6969 - modifiers.FinalDamage.Base;
                modifiers.DisableCrit();
                modifiers.DamageVariationScale *= 0;
                modifiers.ArmorPenetration += 500;
                Main.NewText("Nice!");
                SoundEngine.PlaySound(new SoundStyle("TRAEProject/Assets/Sounds/noice") with { MaxInstances = 0 });
            }
        }
        public override void PostUpdateRunSpeeds()
        {
            if(effect)
            {
                Player.runAcceleration *= -1;
            }
        }
    }
}