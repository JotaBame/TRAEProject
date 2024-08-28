
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Changes.Weapon.Ranged.Rockets;
using TRAEProject.NewContent.Items.DreadItems.BloodBoiler;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Common.ModPlayers
{
   public class RangedStats : ModPlayer
    {
        public int CyberEye = 0;
        public int Magicquiver = 0;
        public bool GunScope = false;
        public int ReconScope = 0;
        public int AlphaScope = 0;
        public float rangedVelocity = 1f;
        public float gunVelocity = 1f;
        public int chanceNotToConsumeAmmo = 0;

        public override void ResetEffects()
        {
            AlphaScope = 0;
            CyberEye = 0;
            Magicquiver = 0;
            ReconScope = 0;
            GunScope = false;
            rangedVelocity = 1f;
            gunVelocity = 1f;
            chanceNotToConsumeAmmo = 0;
        }

        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            Player Player = Main.player[weapon.playerIndexTheItemIsReservedFor];
            if (Main.rand.Next(100) < chanceNotToConsumeAmmo)
                return false;
            if ((weapon.type == ItemID.Flamethrower) && Main.rand.NextBool(3))
                return false;
            if ((weapon.type == ItemID.ElfMelter) && Main.rand.NextBool(3))
                return false;
            if ((weapon.type == ItemType<BloodBoiler>()) && Main.rand.NextBool(3))
                return false;
            if ((weapon.type == ItemID.VenusMagnum) && Main.rand.NextBool(3))
                return false;
            if (weapon.type == ItemID.ChainGun && Main.rand.NextBool(3))
                return false;
            if (weapon.CountsAsClass<RangedDamageClass>() && Player.ammoPotion)
            {

                if (weapon.type != ItemID.StarCannon && weapon.type != ItemID.Clentaminator && weapon.type != ItemID.CoinGun && weapon.type != ItemID.SuperStarCannon && ammo.type != ItemID.PinkGel)
                {
                    return false;
                }
            }
            return true;
        }
    }
   public class RangedStatsProjectile : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.GetGlobalProjectile<NewRockets>().IsARocket && player.GetModPlayer<RangedStats>().CyberEye > 0 || player.GetModPlayer<RangedStats>().AlphaScope > 0)
            {
                projectile.GetGlobalProjectile<ProjectileStats>().FirstHitDamage += 0.1f * player.GetModPlayer<RangedStats>().CyberEye;
            }
        }
    }
}
