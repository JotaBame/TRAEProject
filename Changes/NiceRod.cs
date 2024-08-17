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
using Microsoft.Xna.Framework.Graphics;

namespace TRAEProject.Changes
{
 
    class NiceRodEffect : GlobalProjectile
    {
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (projectile.type == ProjectileID.IceBlock && Main.rand.NextBool(1000))
            {
                modifiers.FinalDamage += 6969 - modifiers.FinalDamage.Base;
                modifiers.DisableCrit();
                modifiers.DamageVariationScale *= 0;
                modifiers.ArmorPenetration += 500;
                Main.NewText("Nice!");
                SoundEngine.PlaySound(new SoundStyle("TRAEProject/Assets/Sounds/noice") with { MaxInstances = 0 });
            }
        }
 
 
    }
}