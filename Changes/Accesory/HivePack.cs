using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TRAEProject.Changes.Accesory
{
    public class HivePack : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if(item.type == ItemID.HiveBackpack)
            {
                player.GetModPlayer<HivePackPlayer>().Hivepack = true;
                player.strongBees = false;
            }
        }
    }
    public class HivePackPlayer : ModPlayer
    {
        public bool Hivepack = false;
        int beetimer = 0;
        int beesStored = 0;
        int timebeforeReleasingBees = 0;
        int ifHoneyedWithBeepack = 1;
        public override void ResetEffects()
        {
            Hivepack = false;
            ifHoneyedWithBeepack = 1;
        }
        public override void UpdateDead()
        {

            Hivepack = false;
            beetimer = 0;
            beesStored = 0;
            timebeforeReleasingBees = 0;
            ifHoneyedWithBeepack = 1;
        }
        public override void PostUpdateEquips()
        {
            if (Hivepack)
            {
                if (Player.HasBuff(BuffID.Honey))
                {
                    ifHoneyedWithBeepack = 2;
                }
                if (Player.velocity.Y > -0.1 && Player.velocity.Y < 0.1)
                {
                    timebeforeReleasingBees = 0;
                    ++beetimer;
                }
                if (beetimer == 8 * ifHoneyedWithBeepack && beesStored < 16)
                {
                    ++beesStored;
                    beetimer = 0;
                    Dust.NewDustDirect(Player.oldPosition, Player.width, Player.height, DustID.Honey, 1, 1, 0, default, 0.8f);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, (int)Player.position.X, (int)Player.position.Y);
                }
                if (Player.velocity.Y < -0.1 || Player.velocity.Y > 0.1)
                {
                    ++timebeforeReleasingBees;
                    beetimer = 0;
                    if (timebeforeReleasingBees > 10 * ifHoneyedWithBeepack && beesStored > 0)
                    {
                        timebeforeReleasingBees = 0;
                        --beesStored;
                        int BeeID = ProjectileID.Bee;
                        if (ifHoneyedWithBeepack == 2)
                        {
                            BeeID = ProjectileID.GiantBee;
                        }
                        Projectile.NewProjectile(Player.GetProjectileSource_Misc(Player.whoAmI), Player.position.X, Player.position.Y, 1 * Player.direction, 0, BeeID, 5 * ifHoneyedWithBeepack, 2, Player.whoAmI);
                    }
                }

                Player.jumpSpeedBoost += 0.225f * beesStored;
            }
        }
    }
}
