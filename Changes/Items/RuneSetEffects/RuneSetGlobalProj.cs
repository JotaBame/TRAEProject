using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TRAEProject.Changes.Items.RuneSetEffects.SpecificWeapons;

namespace TRAEProject.Changes.Items.RuneSetEffects
{
    public class RuneSetGlobalProj : GlobalProjectile
    {
        public int amethystHomingTarget;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            switch (entity.type)
            {
                case ProjectileID.CrystalStorm:
                    return true;
                case ProjectileID.CrystalPulse://crystal serpent shot
                    return true;
                case ProjectileID.CrystalPulse2://cystal serpent frag
                    return true; 
                case ProjectileID.Meteor1://meteor staff meteors
                case ProjectileID.Meteor2:
                case ProjectileID.Meteor3:
                    return true;
                case ProjectileID.CrystalVileShardHead:
                case ProjectileID.CrystalVileShardShaft:
                    return true;
                   
                default:
                    return false;
            }
        }
        public override void SetDefaults(Projectile entity)
        {
            if (!entity.usesLocalNPCImmunity)
            {
                entity.usesLocalNPCImmunity = true;
                entity.localNPCHitCooldown = 20;//for explosion to avoid hitting twice
            }
        }
        public override bool PreAI(Projectile projectile)
        {
            switch (projectile.type)
            {
                case ProjectileID.CrystalStorm:
                    return RuneSetWeaponEffects.CrystalStormAIAddon(projectile);
                case ProjectileID.CrystalPulse:
                    return RuneSetWeaponEffects.CrystalSerpentAIAddon(projectile);
                case ProjectileID.CrystalVileShardHead:
                case ProjectileID.CrystalVileShardShaft:
                    RuneSetCrystalVileShard.CrystalAI(projectile);
                    return false;
                case ProjectileID.Meteor1:
                case ProjectileID.Meteor2:
                case ProjectileID.Meteor3:
                    RuneSetWeaponEffects.MeteorStaffAI(projectile);
                    break;
                default:
                    break;
            }
            return true;
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (projectile.type == ProjectileID.CrystalStorm)
            {
                return RuneSetWeaponEffects.CrystalStormExtraDraw(projectile, ref lightColor);
            }
            else if (projectile.type == ProjectileID.CrystalPulse)
            {
                return RuneSetWeaponEffects.CrystalSerpentExtraDraw(projectile, ref lightColor);
            }
            return true;
        }
        public override bool? CanHitNPC(Projectile projectile, NPC target)
        {
            if (projectile.type == ProjectileID.CrystalStorm)
            {
                return projectile.localNPCImmunity[target.whoAmI] <= 0;
            }
            return null;
        }
        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            switch (projectile.type)
            {
                case ProjectileID.CrystalStorm:
                    if (RuneSetHelper.AoEExplosion(projectile.ai[2]))
                    {
                        RuneSetWeaponEffects.CrystalStormExplode(projectile);
                        //projectile.ai[2] = RuneSetHelper.DisableExplosiveness(projectile.ai[2]);
                    }
                    break;
                case ProjectileID.Meteor1:
                case ProjectileID.Meteor2:
                case ProjectileID.Meteor3:
                    if (RuneSetHelper.AoEExplosion(projectile.ai[2]))
                    {
                        RuneSetWeaponEffects.MeteorStaffKill(projectile);
                        return false;
                    }
                    break;
                default:
                    break;
            }
            return true;
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
     
        }
        public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox)
        {
            if (RuneSetHelper.BiggerHitbox(projectile.ai[2]))
            {
                hitbox.Inflate(50, 50);
            }
        }
        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            if (RuneSetHelper.IsHoming(projectile.ai[2]))
            {
                binaryWriter.Write((byte)amethystHomingTarget);
            }
        }
        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
            if (RuneSetHelper.IsHoming(projectile.ai[2]))
            {
                amethystHomingTarget = binaryReader.ReadByte();
            }
        }
    }
}
