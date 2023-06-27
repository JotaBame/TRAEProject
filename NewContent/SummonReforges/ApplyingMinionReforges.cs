using System;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.IO;
using System.IO;

namespace TRAEProject.NewContent.SummonReforges
{
	public partial class TestForSummonReforgesMinionChanges : GlobalProjectile
	{
      
        //THESE ARE FOR THE STATS THAT CHANGE IN REAL TIME
        float GetPlayerMinionAggroRangeEquipStats(Player player)
        {
            float totalAggro = 0;
            if (HasAccEquiped(ItemID.LongRainbowTrailWings, player))
                totalAggro += 2.5f;
            return totalAggro;
        }
        float GetPlayerMinionAttackRateEquipStats(Player player)
        {
            float totalAttackRate = 0;
            if (HasAccEquiped(ItemID.LongRainbowTrailWings, player))
                totalAttackRate += 2;
            return totalAttackRate;
        }
        float GetPlayerMinionAttackVelocityEquipStats(Player player)
        {
            float totalAttackVelocity = 0;
            if (HasAccEquiped(ItemID.LongRainbowTrailWings, player))
                totalAttackVelocity += 1.5f;
            return totalAttackVelocity;
        }
        float GetPlayerMinionMoveAccelerationEquipStats(Player player)
        {
            float totalAccel = 0;
            if (HasAccEquiped(ItemID.LongRainbowTrailWings, player))
                totalAccel += 1.5f;
            return totalAccel;
        }
        float GetPlayerMinionMoveSpeedEquipStats(Player player)
        {
            float totalMoveSpeed = 0;
            if (HasAccEquiped(ItemID.LongRainbowTrailWings, player))
                totalMoveSpeed += 0.3f;
            return totalMoveSpeed;
        }
        static bool HasAccEquiped(int type, Player player)
        {

            for (int i = 3; i < 7 + player.GetAmountOfExtraAccessorySlotsToShow(); i++)
            {
                if(player.armor[i].type == type)
                    return true;
            }
            return false;
        }
        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            if (projectile.DamageType == DamageClass.Summon && !ProjectileID.Sets.IsAWhip[projectile.type])
            {
                
                binaryWriter.Write((ushort)((prefixAggroRangeBoost) * 32768));//honestly I don't know if this will work or help at all
                binaryWriter.Write((ushort)((prefixAttackRate) * 32768));//but what I'm trying to do is halving the amount of data that
                binaryWriter.Write((ushort)((prefixMoveAcceleration) * 32768));//needs to be sent, from 20 bytes to 10 bytes
                binaryWriter.Write((ushort)((prefixAttackVelocity) * 32768));
                binaryWriter.Write((ushort)((prefixMoveSpeed) * 32768));
            }

        }
        //HONESTLY IDK IF YOU NEED TO SYNC THEM LIKE THIS I'M JUST PARANOID
        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
            if (projectile.DamageType == DamageClass.Summon && !ProjectileID.Sets.IsAWhip[projectile.type])
            {
                prefixAggroRangeBoost = (float)(binaryReader.ReadUInt16()) / 32768f;//then decompress it
                prefixAttackRate = (float)(binaryReader.ReadUInt16()) / 32768f + 1f;
                prefixMoveAcceleration = (float)(binaryReader.ReadUInt16()) / 32768f;
                prefixAttackVelocity = (float)(binaryReader.ReadUInt16()) / 32768f;
                prefixMoveSpeed = (float)(binaryReader.ReadUInt16()) / 32768f;
            }
        }
        
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            Player player = Main.player[projectile.owner];
            if(player.whoAmI == Main.myPlayer)
            {
                if(HasAccEquiped(ItemID.Yoraiz0rWings, player))//yorai spell, testing thing. THESE ARE PREFIX ONLY,
                {
                    prefixAggroRangeBoost = 3;
                    prefixAttackRate = 3;
                    prefixMoveAcceleration = 3;
                    prefixAttackVelocity = 3;
                    prefixMoveSpeed = 3;
                }
            }
        }
    }
}
