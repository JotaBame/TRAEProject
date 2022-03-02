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
            
			Player.lifeSteal -= 0.41666667f; // this stat increases by 0.5f every frame, or by 30 per second. with this change it goes down to 5 per second.
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
                Player.AddBuff(BuffType<LavaShield>(), 9000);
            }            
            if (Player.shroomiteStealth && !Player.mount.Active) // Always active while on the ground, Stealth disappears slower, reduced all bonuses by 25%, max damage is reduced by a further 10%.  
            {
                if (Player.stealth <= 0.25f)
                {
                    Player.stealth = 0.25f;
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        NetMessage.SendData(MessageID.PlayerStealth, -1, -1, null, Player.whoAmI);
                    }
                }
                if (Player.itemAnimation > 0)
                {
                    Player.stealthTimer = 0;
                }
                if (Player.velocity.Y == 0 && Player.stealth > 0.25f)
                {
                    Player.stealth -= 0.015f;
                }
                if (Player.velocity.X > -0.1 && Player.velocity.X < 0.1 && Player.velocity.Y > -0.1 && Player.velocity.Y < 0.1 && Player.mount.Active)
                {
                    if (Player.stealthTimer == 0 && Player.stealth > 0f)
                    {
                        Player.stealth += 0.015f;
                    }
                }
                else
                {
                    float VelocityY = Math.Abs(Player.velocity.Y);
                    Player.stealth += VelocityY * 0.0035f;
                    float Velocity = Math.Abs(Player.velocity.X) + Math.Abs(Player.velocity.Y);
                    Player.stealth -= Velocity * 0.0075f;
                    if (Player.stealth > 1f)
                    {
                        Player.stealth = 1f;
                    }
                    if (Player.mount.Active)
                    {
                        Player.stealth = 1f;
                    }
                }
                Player.GetDamage<RangedDamageClass>() -= (1f - Player.stealth) * 0.1f;// at maximum stealth (0.25, not 0 like in vanilla), damage is increased by 45%. With this code, it's reduced by 10%, making it +35%
                //Player.aggro -= (int)((1f - Player.stealth) * 750f);
                if (Player.stealthTimer > 0)
                {
                    Player.stealthTimer--;
                }
            }
            if (Player.shieldRaised)
            {
                Player.moveSpeed *= 1.5f;
                Player.maxRunSpeed *= 2f;
            }
        }
        public override void OnRespawn(Player Player)
        {
            Player.statLife = Player.statLifeMax;
            return;
        }
        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (Player.hasRaisableShield && Player.HeldItem.type == ItemID.DD2SquireDemonSword && Main.rand.Next(5) == 0)
            {
                Block();
                return false;
            }
            return true;
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
                Player.meleeSpeed += 0.07f;
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
            if (FastFall && Player.controlDown && Player.gravDir != -1)
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
            if (Player.longInvince)
                Player.immuneTime = Player.immuneTime + 40;
            for (int index = 0; index < Player.hurtCooldowns.Length; ++index)
                Player.hurtCooldowns[index] = Player.immuneTime;
            Player.AddBuff(BuffID.ParryDamageBuff, 600, false);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item37, Player.position);
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, 57, 0f, 0f, 255, default, Main.rand.Next(20, 26) * 0.05f);
                dust.noLight = true;
                dust.noGravity = true;
            }
        }
    }
 }
    




