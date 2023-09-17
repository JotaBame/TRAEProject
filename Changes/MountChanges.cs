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

namespace TRAEProject
{
    class MountChanges : ModPlayer
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public static void MountRunSpeeds(Player Player)
        {
            if(Player.GetModPlayer<TRAEJumps>().isBoosting)
            {
                Player.accRunSpeed = 0;
                return;
            }
            float mountSpeedBonus = 1f;
            if(Player.GetModPlayer<Mobility>().TRAEMagi)
            {
                mountSpeedBonus *= 1.15f;
            }
            //Main.NewText("AccRunSpeed: " + Player.accRunSpeed);
            //Main.NewText("Acceleration: " + Player.runAcceleration);
            //Main.NewText("Deceleration: " + Player.runSlowdown);
            //Main.NewText("Movement Speed: " + Player.moveSpeed);
            //Main.NewText("Jump Speed: " + Player.jumpSpeedBoost);
            //Main.NewText("Jump Height: " + Player.jumpHeight);
            
            //nerfed to ~52mph
            if (Player.mount.Type == MountID.SpookyWood || Player.mount.Type == MountID.Unicorn || Player.mount.Type == MountID.WallOfFleshGoat)
            {
                Player.accRunSpeed = 10.6f * mountSpeedBonus;
                if(Math.Abs(Player.velocity.X) > Player.accRunSpeed)
                {
                    Player.velocity.X = Math.Sign(Player.velocity.X) * (Player.accRunSpeed-Player.runAcceleration - 0.2f);
                }
            }
            if (Player.mount.Type == MountID.Flamingo)
            {
                Player.runAcceleration *= 1.75f;
                Player.accRunSpeed = 5f * mountSpeedBonus;
            }
            //horses
            if (Player.mount.Type == MountID.DarkHorse)
            {
                Player.accRunSpeed = 8.6f * mountSpeedBonus;
            }
            if (Player.mount.Type == MountID.PaintedHorse)
            {
                Player.jumpHeight = 12;
                Player.accRunSpeed = 7.8f * mountSpeedBonus;
            }
            if (Player.mount.Type == MountID.MajesticHorse)
            {
                Player.runAcceleration *= 1.5f;
                Player.accRunSpeed = 7.8f * mountSpeedBonus;
            }
            //Nerfed max horizontal speed to ~34mph
            if (Player.mount.Type == MountID.Basilisk)
            {
                Player.accRunSpeed = 6.75f * mountSpeedBonus;
                if (Math.Abs(Player.velocity.X) > Player.accRunSpeed)
                {
                    Player.velocity.X = Math.Sign(Player.velocity.X) * (Player.accRunSpeed-Player.runAcceleration - 0.2f);
                }
            }
            if (Player.mount.Type == MountID.Wolf)
            {
                Player.accRunSpeed = 7f * mountSpeedBonus; 
                if (Math.Abs(Player.velocity.X) > Player.accRunSpeed)
                {
                    Player.velocity.X = Math.Sign(Player.velocity.X) * (Player.accRunSpeed - Player.runAcceleration - 0.2f);
                }
                Player.runAcceleration *= 2f;
                Player.runSlowdown *= 2f;
            }
            //Nerfed max horizontal speed to ~30mph
            //Improved jumping ability WHY IS A BUNNY SO BAD AT JUMPING
            //Gave it a double jump
            //Improved acceration
            if (Player.mount.Type == MountID.Bunny)
            {
                Player.accRunSpeed = 5.6f * mountSpeedBonus;
                Player.runAcceleration = 0.3f;
                Player.jumpHeight = 10;
                Player.jumpSpeed *= 1.4f;
            }

            //max speed reduced to ~35mph
            //acceleration improved significantly
            if(Player.mount.Type == MountID.Pigron)
            {
                Player.accRunSpeed = 7 * mountSpeedBonus;
                Player.runAcceleration = 0.25f;
                //Main.NewText(Player.mount.FlyTime);
                if((Player.controlUp || Player.controlJump) && Player.mount.FlyTime > 0)
                {
			        Player.velocity.Y -= 0.2f * Player.gravDir;
                }
                if(Player.velocity.Y < -7f)
                {
                    Player.velocity.Y = -7f;
                }
            }

            //max speed reduced to ~40mph
            //acceleration improved significantly
            if(Player.mount.Type == MountID.Rudolph)
            {
                Player.accRunSpeed = 8 * mountSpeedBonus;
                Player.runAcceleration = 0.35f;
                if((Player.controlUp || Player.controlJump) && Player.mount.FlyTime > 0)
                {
			        Player.velocity.Y -= 0.2f * Player.gravDir;
                }
            }
            
            //max speed nerfed to ~30mph
            if(Player.mount.Type == MountID.GolfCartSomebodySaveMe)
            {
                float maxSpeed  = 5.6f * mountSpeedBonus;
                if(Math.Abs(Player.velocity.X) > maxSpeed)
                {
                    Player.velocity.X = Math.Sign(Player.velocity.X) * (maxSpeed-Player.runAcceleration);
                }
            }

            //infinite flight, reduced top speed to ~35mph
            if(Player.mount.Type == MountID.DarkMageBook)
            {
                Player.mount.ResetFlightTime(Player.velocity.X);
                float infinimountMax  = 7 * mountSpeedBonus;
                if(Math.Abs(Player.velocity.X) > infinimountMax)
                {
                    Player.velocity.X = Math.Sign(Player.velocity.X) * (infinimountMax-Player.runAcceleration);
                }
            }

            //infiniflight mounts
            //nerfed speed to ~20mph horizontal and vertical, now has infinite flight
            if (Player.mount.Type == MountID.Bee)
            {
                Player.mount.ResetFlightTime(Player.velocity.X);
                float infinimountMax  = 4 * mountSpeedBonus;
                if(Math.Abs(Player.velocity.X) > infinimountMax)
                {
                    Player.velocity.X = Math.Sign(Player.velocity.X) * (infinimountMax-Player.runAcceleration);
                }
                if(Math.Abs(Player.velocity.Y) > infinimountMax)
                {
                    Player.velocity.Y = Math.Sign(Player.velocity.Y) * (infinimountMax-Player.runAcceleration);
                }
            }

            //nerfed speed to ~25mph horizontal and vertical, improved acceleration
            if (Player.mount.Type == MountID.PirateShip)
            {
                float infinimountMax  = 5 * mountSpeedBonus;
                Player.runAcceleration = 0.16f;
                if(Math.Abs(Player.velocity.X) > infinimountMax)
                {
                    Player.velocity.X = Math.Sign(Player.velocity.X) * (infinimountMax-Player.runAcceleration);
                }
                if(Math.Abs(Player.velocity.Y) > infinimountMax)
                {
                    Player.velocity.Y = Math.Sign(Player.velocity.Y) * (infinimountMax-Player.runAcceleration);
                }
            }
            //nerfed to ~30mph horizontal and vertical max speed
            if (Player.mount.Type == MountID.UFO)
            {
                float VerticalSpeed  = 7 * mountSpeedBonus;
                float HorizontalSpeed = 5 * mountSpeedBonus;

                if (Math.Abs(Player.velocity.X) > HorizontalSpeed)
                {
                    Player.velocity.X = Math.Sign(Player.velocity.X) * (HorizontalSpeed-Player.runAcceleration);
                }
                if(Math.Abs(Player.velocity.Y) > VerticalSpeed)
                {
                    Player.velocity.Y = Math.Sign(Player.velocity.Y) * (VerticalSpeed-Player.runAcceleration);
                }
            }
            if (Player.mount.Type == MountID.WitchBroom)
            {
                float VerticalSpeed = 5 * mountSpeedBonus;
                float HorizontalSpeed = 7 * mountSpeedBonus;

                if (Math.Abs(Player.velocity.X) > HorizontalSpeed)
                {
                    Player.velocity.X = Math.Sign(Player.velocity.X) * (HorizontalSpeed - Player.runAcceleration);
                }
                if (Math.Abs(Player.velocity.Y) > VerticalSpeed)
                {
                    Player.velocity.Y = Math.Sign(Player.velocity.Y) * (VerticalSpeed - Player.runAcceleration);
                }
            }
            
            if ( Player.mount.Type == MountID.CuteFishron)
            {
                float VerticalSpeed = 6 * mountSpeedBonus;
                float HorizontalSpeed = 6 * mountSpeedBonus;
                Player.runAcceleration = 0.16f;
                if (Player.controlUp || Player.controlJump)
                {
                    Player.velocity.Y -= 0.1f;
                }
                else if (Player.controlDown)
                {
                    Player.velocity.Y += 0.1f;
                }
                if (Player.wet || Player.ZoneRain)
                {
                    Player.GetDamage<GenericDamageClass>() -= 0.05f;
                    VerticalSpeed *= 1.1f;
                    HorizontalSpeed *= 1.1f;
                }
                if (Math.Abs(Player.velocity.X) > HorizontalSpeed)
                {
                    Player.velocity.X = Math.Sign(Player.velocity.X) * (HorizontalSpeed - Player.runAcceleration);
                }
                if (Math.Abs(Player.velocity.Y) > VerticalSpeed)
                {
                    Player.velocity.Y = Math.Sign(Player.velocity.Y) * (VerticalSpeed - Player.runAcceleration);
                }
            }
            if(Player.GetModPlayer<Mobility>().crippleTimer <= 0)
            {

            }
            else
            {
                Player.runAcceleration *= 0.5f;
                Player.runSlowdown *= 0.5f;
                Player.GetModPlayer<Mobility>().crippleTimer--;
            }
        }
        public override void PostUpdateEquips()
        {
            if(Player.mount.Type == MountID.Bunny)
            {
                Player.GetJumpState(ExtraJump.CloudInABottle).Enable();
            }
        }
    }
}