
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.NewContent.Items.DreadItems.BloodBoiler;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Common.ModPlayers
{
    class RangedStats : ModPlayer
    {
        public int RocketsStun = 0;
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
            RocketsStun = 0;
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
}
