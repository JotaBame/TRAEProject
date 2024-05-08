using Microsoft.Xna.Framework;
using System;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using TRAEProject.NewContent.Buffs;
using TRAEProject.NewContent.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TRAEProject.Changes;

namespace TRAEProject
{
    public class AccesoryEffects : ModPlayer
    {

        public bool wErewolf = false;
        public bool icceleration = false;
        public bool waterRunning = false;
        public bool sandRunning = false;
        public bool FastFall = false;
        public bool AquaAffinity = false;
        public bool LavaShield = false;
        public override void ResetEffects()
        {
            wErewolf = false;
            icceleration = false;
            waterRunning = false;
            sandRunning = false;
            FastFall = false;
            AquaAffinity = false;
            LavaShield = false;
        }
        public override void UpdateDead()
        {
            wErewolf = false;
            icceleration = false;
            waterRunning = false;
            sandRunning = false;
            FastFall = false;
            AquaAffinity = false;
        }
        public override void PostUpdate()
        {

            Player.lifeSteal -= 13f / 30f; // this stat increases by 0.5f every frame, or by 30 per second. with this change it goes down to 4 per second.

            if (Player.lifeSteal > 4)
            {

                Player.lifeSteal = 4;

            }
             if (Player.wingsLogic > 0 && Player.rocketBoots != 0 && Player.velocity.Y != 0f && Player.rocketTime != 0)
            {
                int num45 = 6;
                int num46 = Player.rocketTime * num45;
                Player.wingTime += num46;
                if (Player.wingTime > (Player.wingTimeMax + num46))
                {
                    Player.wingTime = Player.wingTimeMax + num46;
                }
                Player.rocketTime = 0;
            } // this is for Obsidian Rocket Boots to increase Flight time 
            if (LavaShield && Player.lavaWet)
            {
                Player.AddBuff(BuffType<LavaShield>(), 900);
            }
            if (LavaShield && waterRunning && Player.wet)
            {
                Player.AddBuff(BuffType<LavaShield>(), 900);
            }            
            
            if (Player.shieldRaised)
            {
                Player.moveSpeed *= 1.5f;
                Player.maxRunSpeed *= 2f;
            }
        }
        public override void OnRespawn()
        {
            Player.statLife = Player.statLifeMax;
            return;
        }

        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (Player.hasRaisableShield && Player.HeldItem.type == ItemID.DD2SquireDemonSword && Main.rand.NextBool(5))
            {
                Block();
                return true;
            }
            return false;
        }
        public override void PostUpdateEquips()
        {
            if (wErewolf && Player.statLife < Player.statLifeMax2 * 0.67)
            {
                Player.buffImmune[BuffID.Werewolf] = false;
                Player.AddBuff(BuffID.Werewolf, 1, false);
                Player.wereWolf = true;
                Player.GetDamage<GenericDamageClass>() += 0.11f;
                Player.GetCritChance<GenericDamageClass>() += 9;
                Player.GetAttackSpeed(DamageClass.Melee) += 0.07f;
            }
            if (sandRunning)
            {
                int num = 2;
                int num2 = 2;
                int num3 = (int)((Player.position.X + (Player.width / 2)) / 16f);
                int num4 = (int)((Player.position.Y + Player.height) / 16f);
                for (int j = num3 - num; j <= num3 + num; j++)
                {
                    for (int k = num4 - num2; k < num4 + num2; k++)
                    {
                        if (Main.tile[j, k].TileType == TileID.Sand ||
                            Main.tile[j, k].TileType == TileID.Sandstone ||
                            Main.tile[j, k].TileType == TileID.HardenedSand ||
                            Main.tile[j, k].TileType == TileID.Ebonsand ||
                       Main.tile[j, k].TileType == TileID.CorruptSandstone||
                            Main.tile[j, k].TileType == TileID.CorruptHardenedSand ||
                            Main.tile[j, k].TileType == TileID.Crimsand ||
                           Main.tile[j, k].TileType == TileID.CrimsonSandstone ||
                            Main.tile[j, k].TileType == TileID.CrimsonHardenedSand ||
                                Main.tile[j, k].TileType == TileID.Pearlsand ||
                       Main.tile[j, k].TileType == TileID.HallowSandstone ||
                            Main.tile[j, k].TileType == TileID.HallowHardenedSand
                            )
                        {
                            Player.AddBuff(BuffType<SandRush>(), 240);
                        }
                    }
                }
            }
            if (waterRunning)
            {
                int num = 2;
                int num2 = 2;
                int num3 = (int)((Player.position.X + (Player.width / 2)) / 16f);
                int num4 = (int)((Player.position.Y + Player.height) / 16f);
                for (int j = num3 - num; j <= num3 + num; j++)
                {
                    for (int k = num4 - num2; k < num4 + num2; k++)
                    {
                        if (Main.tile[j, k].LiquidAmount > 200 && Main.tile[j, k].LiquidType == 0 ||
                            Main.tile[j, k].LiquidAmount > 200 && Main.tile[j, k].LiquidType == 2 ||
                            Main.tile[j, k].LiquidAmount > 200 && Main.tile[j, k].LiquidType == 1
                            )
                        {
                            Player.AddBuff(BuffType<WaterAffinity>(), 600);
                        }
                    }
                }
            }
            if (FastFall && Player.controlDown && Player.velocity.Y != 0)
            {
                Player.velocity.Y += 2f * Player.gravDir;
            }
            else if (FastFall && Player.controlDown && Player.gravDir != -1)
            {
                Player.maxFallSpeed *= 2f;
                if (Player.velocity.Y < 0)
                { 
                    Player.velocity.Y += 0.7f;
                    Player.velocity.Y += 0.2f;
                }
            }
            
        }


        void Block()
        {
            Player.immune = true;
            Player.immuneTime = 80;
 
            for (int index = 0; index < Player.hurtCooldowns.Length; ++index)
                Player.hurtCooldowns[index] = Player.immuneTime;
            Player.AddBuff(BuffID.ParryDamageBuff, 600, false);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item37 with { MaxInstances = 0 }, Player.position);
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 57, 0f, 0f, 255, default, Main.rand.Next(20, 26) * 0.05f);
                dust.noLight = true;
                dust.noGravity = true;
            }
        }
    }
 }
    




