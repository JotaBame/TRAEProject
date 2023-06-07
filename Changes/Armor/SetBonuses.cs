using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common;
using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.TRAEDebuffs;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;

namespace TRAEProject.Changes.Armor
{
    public class SetBonuses : ModPlayer
    {
        public int shadowArmorDodgeChance = 0;
        public bool PirateSet = false;
        public bool HolyProtection = false;
        public bool TitaniumArmorOn = false;
        public bool whenHitDodge = false;
        public bool secretPearlwoodSetBonus = false;
        public override void ResetEffects()
        {
            TitaniumArmorOn = false;
            PirateSet = false;
            shadowArmorDodgeChance = 0;
            HolyProtection = false;
            whenHitDodge = false;
            secretPearlwoodSetBonus = false;
        }
        public override void UpdateDead()
        {
            TitaniumArmorOn = false;
            PirateSet = false;
            shadowArmorDodgeChance = 0;
            HolyProtection = false;
            whenHitDodge = false;
            secretPearlwoodSetBonus = false;
        }


        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if(secretPearlwoodSetBonus && Main.rand.NextBool(1000))
            {
                modifiers.GetDamage(6969, false);
                Main.NewText("Nice!");
                SoundEngine.PlaySound(new SoundStyle("TRAEProject/Assets/Sounds/noice") with { MaxInstances = 0 });
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if(secretPearlwoodSetBonus && Main.rand.NextBool(1000))
            {
                modifiers.GetDamage(6969, false);
                Main.NewText("Nice!");
                SoundEngine.PlaySound(new SoundStyle("TRAEProject/Assets/Sounds/noice") with { MaxInstances = 0 });
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (PirateSet && (ProjectileID.Sets.IsAWhip[proj.type]))
            {
                TRAEDebuff.Apply<PirateTag>(target, 240, 5);
            }
        }


        //void Shadowdodge()
        //{
        //    if (HolyProtection)
        //    {
        //       Player.onHitDodge = true;
        //        if (Player.shadowDodgeTimer == 0)
        //        {
        //            Player.AddBuff(BuffID.ShadowDodge, 1800, false);
        //            Player.shadowDodgeTimer = 1800;

        //        }
        //    }
        //}
    }
}
